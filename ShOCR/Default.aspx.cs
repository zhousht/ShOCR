using ShOCRLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShOCR
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Process_Click(object sender, EventArgs e)
        {
            Stream stream = imageUpload.PostedFile.InputStream;
            Bitmap image = new Bitmap(stream);

            imageOrigion.ImageUrl = ToImageSource(image);

            Proprocessor lib = new Proprocessor();

            lib.ToGray(ref image);
            
            imageGray.ImageUrl = ToImageSource(image);
            
            lib.ToBlackWhite(ref image);

            imageBlackWhite.ImageUrl = ToImageSource(image);
            /*
            lib.RemoveNoise(ref image);

            imageNoiseRmoved.ImageUrl = ToImageSource(image);
            */

            string result = lib.GetText(image);

            txtResult.InnerText = result;

        }

        private string ToImageSource(Bitmap image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Gif);
            var base64Data = Convert.ToBase64String(ms.ToArray());
            return "data:image/gif;base64," + base64Data;
        }
    }
}