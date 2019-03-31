using System;

namespace SZOCREngine
{
    public static class Preprocessor
    {
        public static void Binarization()
        {
            ToGray();
            ToBlackWhite();
        }

        public static void ToBlackWhite()
        {

        }

        public static void ToGray()
        {/*
            // create filter
            Threshold filter = new Threshold(100);
            // apply the filter
            filter.ApplyInPlace(image);
            */
        }
    }
}
