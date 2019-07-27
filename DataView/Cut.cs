using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    /*
    /// <summary>
    /// 
    /// </summary>
    class Cut
    {
        private int[,] cut;
        private int[,] realCut; // cut with spacing
        private int direction; // 0-x, 1-y, 2-z
        private int distance; // the length from zero point to the point of the cut
        private double xSpacing;
        private double ySpacing;
        private int width;
        private int height;
        private int realWidth;
        private int realHeight;
        private double iSpacing; // new spacing for realCut

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sample"></param>
        /// <param name="vd"></param>
        /// <param name="distance"></param>
        /// <param name="direction"></param>
        /// <param name="iSpacing"></param>
        public Cut(Sample sample, VolumetricData vd, int distance, int direction, double iSpacing)
        {
            this.distance = distance;
            this.direction = direction;
            this.iSpacing = iSpacing;

            Init(sample, vd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sample"></param>
        /// <param name="vd"></param>
        /// <param name="distance"></param>
        /// <param name="direction"></param>
        public Cut(Sample sample, VolumetricData vd, int distance, int direction)
        {
            this.distance = distance;
            this.direction = direction;

            Init(sample, vd);

            this.iSpacing = Math.Min(xSpacing, ySpacing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sample"></param>
        /// <param name="vd"></param>
        public void Init(Sample sample, VolumetricData vd)
        {
            int width = sample.DimSize[0];
            int depth = sample.DimSize[1];
            int height = sample.DimSize[2];

            try
            {
                if (direction == 0)
                {
                    cut = new int[depth, height];
                    for (int i = 0; i < depth; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            cut[i, j] = vd.VData[distance, i, j];
                        }
                    }
                    this.xSpacing = sample.ElementSpacing[1];
                    this.ySpacing = sample.ElementSpacing[2];
                    this.width = depth;
                    this.height = height;
                }

                else if (direction == 1)
                {

                    cut = new int[width, height];
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            cut[i, j] = vd.VData[i, distance, j];
                        }
                    }
                    this.xSpacing = sample.ElementSpacing[0];
                    this.ySpacing = sample.ElementSpacing[2];
                    this.width = width;
                    this.height = height;
                }

                else
                {
                    cut = new int[width, depth];
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < depth; j++)
                        {
                            cut[i, j] = vd.VData[i, j, distance];
                        }
                    }
                    this.xSpacing = sample.ElementSpacing[0];
                    this.ySpacing = sample.ElementSpacing[1];
                    this.width = width;
                    this.height = depth;
                }

            }
            catch (Exception e)
            {
                Console.Write(e.GetBaseException());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int[,] Convert()
        {
            realWidth = (int)((width * xSpacing) / iSpacing);
            realHeight = (int)((height * ySpacing) / iSpacing);
            realCut = new int[realWidth, realHeight];

            for (int i = 0; i < realWidth; i++)
            {
                for (int j = 0; j < realHeight; j++)
                {
                    double pX = i * iSpacing;
                    double pY = j * iSpacing; // real coordinates of pixel
                    double dX = pX / xSpacing;
                    double dY = pY / ySpacing;
                    int xLDC = (int)Math.Floor(dX);
                    int yLDC = (int)Math.Floor(dY); // coordinates of left down corner of the rectangle in the array in with the pixel is situated
                    int xRDC = xLDC + 1;
                    int yRDC = yLDC;
                    int xLUC = xLDC;
                    int yLUC = yLDC + 1;
                    int xRUC = xLDC + 1;
                    int yRUC = yLDC + 1;

                    Boolean b = xLDC >= width || xLUC >= width || xRDC >= width || xRUC >= width || yLDC >= height || yLUC >= height || yRDC >= height || yRUC >= height;
                    if (!b)
                    {
                        int valueA = cut[xLDC, yLDC];
                        int valueB = cut[xRDC, yRDC];
                        int valueC = cut[xLUC, yLUC];
                        int valueD = cut[xRUC, yRUC];

                        int helpValueDown = CountValue(valueA, valueB, i, xLDC, 0);
                        int helpValueUp = CountValue(valueC, valueD, i, xLUC, 0);
                        int value = CountValue(helpValueDown, helpValueUp, j, yLDC, 1);

                        realCut[i, j] = value;
                    }

                }
            }

            return realCut;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueA"></param>
        /// <param name="valueB"></param>
        /// <param name="indexOfPixel"></param>
        /// <param name="indexOfA"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public int CountValue(int valueA, int valueB, int indexOfPixel, int indexOfA, int direction)
        {
            double spacing = ySpacing;
            if (direction == 0)
            {
                spacing = xSpacing;
            }
            double d = indexOfPixel * iSpacing - indexOfA * spacing;
            //Console.WriteLine(indexOfPixel + " " + indexOfA + " " + d);
            double r = d / spacing;
            return (int)(r * valueB + (1 - r) * valueA);
        }

        public int GetRealHeight()
        {
            return realHeight;
        }

        public int GetRealWidth()
        {
            return realWidth;
        }

        public int Direction { get => direction; set => direction = value; }
        public int Distance { get => distance; set => distance = value; }
        public int Width { get => width; }
        public int Height { get => height; }
        public int[,] CutPart { get => cut; }
        public int RealWidth { get => realWidth; }
        public int RealHeight { get => realHeight; }

    }
    */
}
