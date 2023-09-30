using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DataView
{
    /// <summary>
    /// 
    /// </summary>
    class PictureMaker
    {
        private double[,] array;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        public PictureMaker(double[,] array)
        {
            this.array = array;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Bitmap MakeBitmap()
        {
            int width = array.GetLength(0);
            int height = array.GetLength(1);

            double max = GetMax(width, height); //could be set to 1, which is the least it can get;
            if (max == 0)
                max = 1;

            Bitmap bitmap = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double h = array[x, y];
                    int a = (int)(h * 255 / max);
                    Color color = Color.FromArgb(a, a, a);
                    bitmap.SetPixel(x, y, color);
                }
            }
            return bitmap;
        }

        private double GetMax(int width, int height) {
            double max = 0; //could be set to 1, which is the least it can get; 
            double c;

            for (int i = 0; i < width; i++) //TODO .Max() ?
            {
                for (int j = 0; j < height; j++)
                {
                    c = array[i, j];
                    if (c > max)
                    {
                        max = c;
                    }
                }
            }
            return max;
        }
    }
}
