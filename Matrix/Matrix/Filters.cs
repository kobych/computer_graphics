using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Matrix
{
    abstract class Filters
    {
        public Bitmap original ;
        protected abstract Color calculateNewPixelColor(Bitmap sourceImage, int x, int y);

        public int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

        public Bitmap processImage(Bitmap sourceImage)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            for(int i = 0; i < sourceImage.Width; i++)
            {
                for(int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }
            return resultImage;

        }
        
    }

    class InvertFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            Color resultColor = Color.FromArgb(255 - sourceColor.R, 255 - sourceColor.G, 255 - sourceColor.B);
            return resultColor;
        }
    }

    class SepiaFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int tr = (int)(0.393 * sourceColor.R + 0.769 * sourceColor.G + 0.189 * sourceColor.B);
            if (tr > 255) tr = 255;
            int tg = (int)(0.349 * sourceColor.R + 0.686 * sourceColor.G + 0.168 * sourceColor.B);
            if (tg > 255) tg = 255;
            int tb = (int)(0.272 * sourceColor.R + 0.534 * sourceColor.G + 0.131 * sourceColor.B);
            if (tb > 255) tb = 255;
            Color resultColor = Color.FromArgb(tr, tg, tb);
            return resultColor;
        }
    }
    class GreyFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int intensity =(int) (0.299 * sourceColor.R + 0.587 * sourceColor.G + 0.114 * sourceColor.B);
            Color resultColor = Color.FromArgb(intensity, intensity, intensity);
            return resultColor;
        }
    }

    class LuxFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int LuxConst = 100;
            Color sourceColor = sourceImage.GetPixel(x, y);
            int tr = (int)(sourceColor.R + LuxConst);
            if (tr > 255) tr = 255;
            int tg = (int)(sourceColor.G + LuxConst);
            if (tg > 255) tg = 255;
            int tb = (int)(sourceColor.B + LuxConst);
            if (tb > 255) tb = 255;
            Color resultColor = Color.FromArgb(tr, tg, tb);
            return resultColor;
        }
    }
}
