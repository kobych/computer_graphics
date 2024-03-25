﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    class MathMorphology : Filters // Операции морфологии 
    {
        protected bool isDilation;
        protected int[,] mask = null;
        protected MathMorphology()
        {
            mask = new int[,] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } };
        }
        public MathMorphology(int[,] mask)
        {
            this.mask = mask;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int radiusX = mask.GetLength(0) / 2;
            int radiusY = mask.GetLength(1) / 2;
            int minR = 255; int minG = 255; int minB = 255;
            int maxR = 0; int maxG = 0; int maxB = 0;
            for (int i = -radiusX; i <= radiusX; i++)
                for (int j = -radiusY; j <= radiusY; j++)
                {
                    if (isDilation)
                    {
                        if ((mask[i + radiusX, j + radiusY] != 0) && (sourceImage.GetPixel(x + i, y + j).R > maxR))
                            maxR = sourceImage.GetPixel(x + i, y + j).R;
                        if ((mask[i + radiusX, j + radiusY] != 0) && (sourceImage.GetPixel(x + i, y + j).G > maxG))
                            maxG = sourceImage.GetPixel(x + i, y + j).G;
                        if ((mask[i + radiusX, j + radiusY] != 0) && (sourceImage.GetPixel(x + i, y + j).B > maxB))
                            maxB = sourceImage.GetPixel(x + i, y + j).B;
                    }
                    else
                    {
                        if ((mask[i + radiusX, j + radiusY] != 0) && (sourceImage.GetPixel(x + i, y + j).R < minR))
                            minR = sourceImage.GetPixel(x + i, y + j).R;
                        if ((mask[i + radiusX, j + radiusY] != 0) && (sourceImage.GetPixel(x + i, y + j).G < minG))
                            minG = sourceImage.GetPixel(x + i, y + j).G;
                        if ((mask[i + radiusX, j + radiusY] != 0) && (sourceImage.GetPixel(x + i, y + j).B < minB))
                            minB = sourceImage.GetPixel(x + i, y + j).B;
                    }
                }
            if (isDilation)
                return Color.FromArgb(maxR, maxG, maxB);
            else
                return Color.FromArgb(minR, minG, minB);
        }
        public override Bitmap processImage(Bitmap sourceImage)
        {
            int radiusX = mask.GetLength(0) / 2;
            int radiusY = mask.GetLength(1) / 2;
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = radiusX; i < sourceImage.Width - radiusX; i++)
            {
                for (int j = radiusY; j < sourceImage.Height - radiusY; j++)
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
            }
            return resultImage;
        }
    }

    class Dilation : MathMorphology // Морфологическое расширение
    {
        public Dilation()
        {
            isDilation = true;
        }
        public Dilation(int[,] mask)
        {
            this.mask = mask;
            isDilation = true;
        }
    }

    class Erosion : MathMorphology // Морфологическое сужение
    {
        public Erosion()
        {
            isDilation = false;
        }
        public Erosion(int[,] mask)
        {
            this.mask = mask;
            isDilation = false;
        }
    }

    class Opening : MathMorphology // Морфологическое открытие
    {
        //public Opening(int[,] mask)
        //{
        //    this.mask = mask;
        //}
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Filters filter1 = new Erosion(mask);
            Bitmap result = filter1.processImage(sourceImage);
            Filters filter2 = new Dilation(mask);
            result = filter2.processImage(result);
            return result;
        }
    }

    class Closing : MathMorphology // Морфологическое закрытие
    {
        //public Closing(int[,] mask)
        //{
        //    this.mask = mask;
        //}
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Filters filter1 = new Dilation(mask);
            Bitmap result = filter1.processImage(sourceImage);
            Filters filter2 = new Erosion(mask);
            result = filter2.processImage(result);
            return result;
        }
    }

    class Grad : MathMorphology // Градиент
    {
        //public Grad(int[,] mask)
        //{
        //    this.mask = mask;
        //}
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage)
        {
            Bitmap result = new Bitmap(sourceImage.Width, sourceImage.Height);
            Filters filter1 = new Dilation(mask);
            Bitmap result1 = filter1.processImage(sourceImage);
            Filters filter2 = new Erosion(mask);
            Bitmap result2 = filter2.processImage(sourceImage);
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    int newR = Clamp(result1.GetPixel(i, j).R - result2.GetPixel(i, j).R, 0, 255);
                    int newG = Clamp(result1.GetPixel(i, j).G - result2.GetPixel(i, j).G, 0, 255);
                    int newB = Clamp(result1.GetPixel(i, j).B - result2.GetPixel(i, j).B, 0, 255);
                    result.SetPixel(i, j, Color.FromArgb(newR, newG, newB));
                }
            }
            return result;
        }
    }

    class TopHat : MathMorphology // TopHat (цилиндр)
    {
        //public TopHat(int[,] mask)
        //{
        //    this.mask = mask;
        //}
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage)
        {
            Bitmap result = new Bitmap(sourceImage.Width, sourceImage.Height);
            Filters filter1 = new Erosion(mask);
            Bitmap result1 = filter1.processImage(sourceImage);
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    int newR = Clamp(sourceImage.GetPixel(i, j).R - result1.GetPixel(i, j).R, 0, 255);
                    int newG = Clamp(sourceImage.GetPixel(i, j).G - result1.GetPixel(i, j).G, 0, 255);
                    int newB = Clamp(sourceImage.GetPixel(i, j).B - result1.GetPixel(i, j).B, 0, 255);
                    result.SetPixel(i, j, Color.FromArgb(newR, newG, newB));
                }
            }
            return result;
        }
    }

    class BlackHat : MathMorphology // BlackHat (эффект черной шляпы)
    {
        //public BlackHat(int[,] mask)
        //{
        //    this.mask = mask;
        //}
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage)
        {
            Bitmap result = new Bitmap(sourceImage.Width, sourceImage.Height);
            Filters filter1 = new Dilation(mask);
            Bitmap result1 = filter1.processImage(sourceImage);
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    int newR = Clamp(result1.GetPixel(i, j).R - sourceImage.GetPixel(i, j).R, 0, 255);
                    int newG = Clamp(result1.GetPixel(i, j).G - sourceImage.GetPixel(i, j).G, 0, 255);
                    int newB = Clamp(result1.GetPixel(i, j).B - sourceImage.GetPixel(i, j).B, 0, 255);
                    result.SetPixel(i, j, Color.FromArgb(newR, newG, newB));
                }
            }
            return result;
        }
    }
}
