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

        public virtual Bitmap processImage(Bitmap sourceImage)
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
    
    class GreyWorld : Filters
    {
        protected int Avg;
        protected int R, G, B;
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            R += sourceColor.R;
            G += sourceColor.G;
            B += sourceColor.B;
            Avg = (R + G + B) / 3;
            Color resultColor = Color.FromArgb(Clamp(sourceColor.R * Avg / R, 0, 255), Clamp(sourceColor.G * Avg / G, 0, 255), Clamp(sourceColor.B * Avg / B, 0, 255));
            return resultColor;
        }

    }

    class WavesFilter : Filters 
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int newX = Clamp((int)(x + 20 * Math.Sin(2 * Math.PI * x / 60)), 0, sourceImage.Width - 1);
            int newY = y;
            return sourceImage.GetPixel(newX, newY); ;
        }
    }

    class GlassEffect : Filters 
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Random Rand = new Random();
            int newX = Clamp((int)(x + (Rand.NextDouble() - 0.5) * 10.0), 0, sourceImage.Width - 1);
            int newY = Clamp((int)(y + (Rand.NextDouble() - 0.5) * 10.0), 0, sourceImage.Height - 1);
            return sourceImage.GetPixel(newX, newY); ;
        }
    }

    class Shift : Filters 
    {
        double x0, y0; 
        public Shift(double _x0, double _y0)
        {
            x0 = _x0; y0 = _y0;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourseColor = sourceImage.GetPixel(x, y);
            int newX = Clamp((int)(x + x0), 0, sourceImage.Width - 1);
            int newY = Clamp((int)(y + y0), 0, sourceImage.Height - 1); ;
            return sourceImage.GetPixel(newX, newY); ;
        }
    }

    class Rotation : Filters 
    {
        double x0, y0; 
        double alfa; 
        public Rotation(double _x0, double _y0, double _alfa)
        {
            x0 = _x0; y0 = _y0; alfa = _alfa;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            double p1 = Math.Cos(alfa); double p2 = Math.Sin(alfa);
            int newX = Clamp((int)((x - x0) * p1 - (y - y0) * p2 + x0), 0, sourceImage.Width - 1);
            int newY = Clamp((int)((x - x0) * p2 + (y - y0) * p1 + y0), 0, sourceImage.Height - 1);
            return sourceImage.GetPixel(newX, newY);
        }
    }

    class BinaryFilter : Filters 
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color s = sourceImage.GetPixel(x, y);
            if (s.R < 127 && s.G < 127 && s.B < 127)
                return Color.FromArgb(0, 0, 0);
            else
                return Color.FromArgb(255, 255, 255);
        }
    }

    class MainColorFilter : Filters // Коррекция с опорным цветом
    {
        Color c;
        public MainColorFilter(Color _c)
        {
            c = _c;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage)
        {
            double Rsrc = 0, Gsrc = 0, Bsrc = 0, Rdst = c.R, Gdst = c.G, Bdst = c.B;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    if (sourceImage.GetPixel(i, j).R > Rsrc) Rsrc = sourceImage.GetPixel(i, j).R;
                    if (sourceImage.GetPixel(i, j).G > Gsrc) Gsrc = sourceImage.GetPixel(i, j).G;
                    if (sourceImage.GetPixel(i, j).B > Bsrc) Bsrc = sourceImage.GetPixel(i, j).B;
                }
            }
            
            Bitmap result = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    int newR = Clamp((int)(calculateNewPixelColor(sourceImage, i, j).R * Rdst / Rsrc), 0, 255);
                    int newG = Clamp((int)(calculateNewPixelColor(sourceImage, i, j).G * Gdst / Gsrc), 0, 255);
                    int newB = Clamp((int)(calculateNewPixelColor(sourceImage, i, j).B * Bdst / Bsrc), 0, 255);
                    result.SetPixel(i, j, Color.FromArgb(newR, newG, newB));
                }
            }
            return result;
        }
    }

    
}
