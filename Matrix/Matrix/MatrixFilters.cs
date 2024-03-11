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

    class SobelFilter : MatrixFilters
    {
        //Sobel operator kernel for horizontal pixel changes
        private static float[,] xSobel
        {
            get
            {
                return new float[,]
                {
            { -1, 0, 1 },
            { -2, 0, 2 },
            { -1, 0, 1 }
                };
            }
        }

        //Sobel operator kernel for vertical pixel changes
        private static float[,] ySobel
        {
            get
            {
                return new float[,]
                {
            {  1,  2,  1 },
            {  0,  0,  0 },
            { -1, -2, -1 }
                };
            }
        }

        public SobelFilter()
        {
            int sizeX = 3;
            int sizeY = 3;
            kernel = new float[sizeX, sizeY];
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    kernel[y, x] = ySobel[y, x] * xSobel[y, x];
                }
            }
        }

    }
    class SharpFilter : MatrixFilters
    {

        private static float[,] sharp
        {
            get
            {
                return new float[,]
                {
            { 0, -1, 0 },
            { -1, 5, -1 },
            { 0, -1, 0 }
                };
            }
        }

        public SharpFilter()
        {
            int sizeX = 3;
            int sizeY = 3;
            kernel = new float[sizeX, sizeY];
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    kernel[y, x] = sharp[y, x];
                    
                }
            }
        }

    }
    class EmbossFilter : MatrixFilters
    {

        private static float[,] emboss
        {
            get
            {
                return new float[,]
                {
            { 0, 1, 0 },
            { 1, 0, -1 },
            { 0, -1, 0 }
                };
            }
        }

        public EmbossFilter()
        {
            int sizeX = 3;
            int sizeY = 3;
            kernel = new float[sizeX, sizeY];
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    kernel[y, x] = emboss[y, x];
                    

                }
            }
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
            resultR += 255;
            resultG += 255;
            resultB += 255;
            resultR /= 2;
            resultG /= 2;
            resultB /= 2;
            resultR = Clamp((int)resultR, 0, 255);
            resultG = Clamp((int)resultG, 0, 255);
            resultB = Clamp((int)resultB, 0, 255);

            return Color.FromArgb((int)resultR, (int)resultG, (int)resultB);
        }
    }
    class HarraFilter : MatrixFilters
    {

        private static float[,] xHarra
        {
            get
            {
                return new float[,]
                {
            { 3, 0, -3 },
            { 10, 0, -10 },
            { 3, 0, -3 }
                };
            }
        }

        
        private static float[,] yHarra
        {
            get
            {
                return new float[,]
                {
            {  3,  10,  3 },
            {  0,  0,  0 },
            { -3, -10, -3 }
                };
            }
        }

        public HarraFilter()
        {
            int sizeX = 3;
            int sizeY = 3;
            kernel = new float[sizeX, sizeY];
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    kernel[y, x] = yHarra[y, x] * xHarra[y, x];
                }
            }
        }
    }
    class MedianFilter : MatrixFilters
    {
        protected float median;
        static float FindMedian(float[,] matrix)
        {
            // Flatten the matrix into a 1D array
            float[] arr = new float[matrix.GetLength(0)];
            foreach (float row in matrix)
            {
                arr.AddRange(row);
            }

            // Sort the array
            arr.Sort();

            // Find the median element
            int mid = arr.Count / 2;
            float median;
            if (arr.Count % 2 == 0)
            {
                median = (arr[mid - 1] + arr[mid]) / 2.0f;
            }
            else
            {
                median = arr[mid];
            }

            return median;
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
            median = ;
            resultR = Clamp((int)resultR, 0, 255);
            resultG = Clamp((int)resultG, 0, 255);
            resultB = Clamp((int)resultB, 0, 255);

            return Color.FromArgb((int)resultR, (int)resultG, (int)resultB);
        }
        public MedianFilter()
        {
            int sizeX = 3;
            int sizeY = 3;
            kernel = new float[sizeX, sizeY];
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    kernel[y, x] = median;


                }
            }
        }
    }

}
