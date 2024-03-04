using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Matrix
{
    class MatrixFilters : Filters
    {
        protected float[,] kernel;
        public MatrixFilters() { }
        public MatrixFilters(float[,] kernel)
        {
            this.kernel = kernel;
        }


        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;

            float resultR = 0;
            float resultG = 0;
            float resultB = 0;

            for (int i = -radiusX; i <= radiusX; i++)
            {
                for (int j = -radiusY; j <= radiusY; j++)
                {
                    int idx = Clamp(x + i, 0, sourceImage.Width - 1);
                    int idy = Clamp(y + j, 0, sourceImage.Height - 1);

                    Color neighborColor = sourceImage.GetPixel(idx, idy);

                    resultR += neighborColor.R * kernel[i + radiusX, j + radiusY];
                    resultG += neighborColor.G * kernel[i + radiusX, j + radiusY];
                    resultB += neighborColor.B * kernel[i + radiusX, j + radiusY];
                }
            }

            resultR = Clamp((int)resultR, 0, 255);
            resultG = Clamp((int)resultG, 0, 255);
            resultB = Clamp((int)resultB, 0, 255);

            return Color.FromArgb((int)resultR, (int)resultG, (int)resultB);
        }
    }

    class  BlurFilter : MatrixFilters
    {
        public BlurFilter()
        {
            int sizeX = 3;
            int sizeY = 3;
            kernel = new float[sizeX, sizeY];
            for (int i = 0; i < sizeX; i++)
                for (int j = 0; j < sizeY; j++)
                    kernel[i, j] = 1.0f / (float)(sizeX * sizeY);
        }
    }

    class GaussianFilter : MatrixFilters
    {
        public void createGaussianKernel(int radius, float sigma)
        {
            int size = 2 * radius + 1;
            kernel = new float[size, size];
            float norm = 0;
            for(int i = -radius; i <= radius; i++)
                for(int j = -radius; j <= radius; j++)
                {
                    kernel[i + radius, j + radius] = (float)(Math.Exp(-(i * i + j * j) / (2 * sigma * sigma)));
                    norm += kernel[i + radius, j + radius];
                }
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    kernel[i, j] /= norm;
        }
        public GaussianFilter()
        {
            createGaussianKernel(3, 2);
        }
    }

    
}
