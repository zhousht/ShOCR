using AForge.Imaging;
using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Tesseract;

namespace ShOCRLib
{
    public class Proprocessor
    {
        public void Process(Bitmap image)
        {
            if(image== null)
            {
                return;
            }

            ToGray(ref image);
            //ToBlackWhite(ref image);
            RemoveNoise(ref image);
            Detection(ref image);
        }

        private System.Drawing.Image ResizeImage(System.Drawing.Image imgToResize)
        {
            float resolutionH = imgToResize.HorizontalResolution;
            float resolutionV = imgToResize.VerticalResolution;

            int scaleH = 300 / (int)resolutionH + 1;
            int scaleV = 300 / (int)resolutionV + 1;

            int newH = imgToResize.Width * scaleH;
            int newV = imgToResize.Height * scaleV;

            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            Bitmap b = new Bitmap(newH, newV);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, newH, newV);
            g.Dispose();

            return (System.Drawing.Image)b;
        }

        /*
         * 1- gray scale transform;
         * */
        public void ToGray(ref Bitmap image)
        {

            image = (Bitmap)ResizeImage((System.Drawing.Image)image);

            // Create a blank bitmap with the same dimensions
            Bitmap imageGray = new Bitmap(image);

            
            Median medianFilter = new Median();
            // apply the filter
            medianFilter.ApplyInPlace(image);
            

            // create grayscale filter (BT709)
            Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
            // apply the filter
            image = filter.Apply(imageGray);
            
            // create filter
            Invert invertFilter = new Invert();
            // apply the filter
            invertFilter.ApplyInPlace(image);
            
            // create filter
            OtsuThreshold filterOtsuThreshold = new OtsuThreshold();
            // apply the filter
            filterOtsuThreshold.ApplyInPlace(image);
            // check threshold value
            int t = filterOtsuThreshold.ThresholdValue;

            /*
            // create and configure the filter
            FillHoles holesFilter = new FillHoles();
            holesFilter.MaxHoleHeight = 2;
            holesFilter.MaxHoleWidth = 2;
            holesFilter.CoupledSizeFiltering = false;
            // apply the filter
            Bitmap result = holesFilter.Apply(image);

            
            BinaryDilatation3x3 bd = new BinaryDilatation3x3();
            bd.ApplyInPlace(image);
            bd.ApplyInPlace(image);
            
            // create filter
            BlobsFiltering filterBlobsFiltering = new BlobsFiltering();
            // configure filter
            filterBlobsFiltering.CoupledSizeFiltering = true;
            filterBlobsFiltering.MinWidth = 5;
            filterBlobsFiltering.MinHeight = 5;
            // apply the filter
            filterBlobsFiltering.ApplyInPlace(image);
            */
        }

        /*
         * 2- binarization (Otsu's method is the most referenced technique);
         * (i don't know, yet, if i can apply binarization in a color img or if i must do grayscale first)
         * */
        public void ToBlackWhite(ref Bitmap image)
        {
            // create filter
            Threshold filter = new Threshold(100);
            // apply the filter
            filter.ApplyInPlace(image);
        }

        /*
         * 3- noise elimination to delete isolated pixel and improve the next step. 
         * (median filter technique seems to be the best way);
         * */
        public void RemoveNoise(ref Bitmap image)
        {
            /*
            FiltersSequence filterSquence = new FiltersSequence(Grayscale.CommonAlgorithms.BT709,
                                 new Threshold(100), new FillHoles());
            image = filterSquence.Apply(image);
            */
            // create filter
            ConservativeSmoothing filterSmoothing = new ConservativeSmoothing();
            // apply the filter
            filterSmoothing.ApplyInPlace(image);
            
        }

        /*
         * 4- do a feature detection/extraction with edge detection technique (maybe) to identify characters 
         * (groups of connected pixels). (i don't know what's the best for character: edge, corner or blob detection...)
         * */
        public void Detection(ref Bitmap image)
        {
            // create instance of skew checker
            DocumentSkewChecker skewChecker = new DocumentSkewChecker();
            // get documents skew angle
            double angle = skewChecker.GetSkewAngle(image);
            // create rotation filter
            RotateBilinear rotationFilter = new RotateBilinear(-angle);
            rotationFilter.FillColor = Color.White;
            // rotate image applying the filter
            Bitmap rotatedImage = rotationFilter.Apply(image);


        }

        public string GetText(Bitmap imgsource)
        {
            var ocrtext = string.Empty;
            using (TesseractEngine engine = new TesseractEngine(Path.Combine(HttpRuntime.AppDomainAppPath, "tessdata"), "eng", EngineMode.Default))
            {
                //engine.DefaultPageSegMode = PageSegMode.SingleLine;
                using (var img = PixConverter.ToPix(imgsource))
                {
                    using (var page = engine.Process(img))
                    {
                        ocrtext = page.GetText();
                    }
                }
            }
            /*
            var ocrtext = string.Empty;
            using (var engine = new TesseractEngine(Path.Combine(HttpRuntime.AppDomainAppPath, "tessdata"), "eng", EngineMode.Default))
            {
                using (var img = PixConverter.ToPix(imgsource))
                {
                    using (var page = engine.Process(img))
                    {
                        ocrtext = page.GetText();
                    }
                }
            }
            */
            return ocrtext;
        }
    }
}
