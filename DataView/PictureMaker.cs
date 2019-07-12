using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    /// <summary>
    /// 
    /// </summary>
    class PictureMaker
    {
        private int[,] array;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        public PictureMaker(int[,] array)
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

            double max = 0;
            double c;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    c = array[i, j];
                   // Console.Write(c + " ");//
                    if (c > max)
                    {
                        max = c;
                    }
                }
               // Console.WriteLine();//
            }
          //  Console.ReadKey();//
            Bitmap bitmap = new Bitmap(width, height);
            if(max == 0)
            {
                max = 1;
            }
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int h = array[x, y];
                    h = (int)((h * 255) / max);
                    Color color = Color.FromArgb(h, h, h);
                    bitmap.SetPixel(x, y, color);
                }
            }

            return bitmap;
        }

    }
}
