using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    /// <summary>
    /// 
    /// </summary>
    class VolumetricData
    {
        public int[][,] vData;
        private double xSpacing;
        private double ySpacing;
        private double zSpacing;
        private double iSpacing;
        private Data data;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public VolumetricData(Data data)
        {
            xSpacing = data.ElementSpacing[0];
            ySpacing = data.ElementSpacing[1];
            zSpacing = data.ElementSpacing[2];

            this.data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int[][,] Read()
        {
            using (BinaryReader br = new BinaryReader(new FileStream(data.ElementDataFile, FileMode.Open)))
            {
                int width = data.DimSize[0];
                int depth = data.DimSize[1];
                int height = data.DimSize[2];

                vData = new int[height][,];
                int c = 0;

                for (int k = 0; k < height; k++)
                {
                    vData[k] = new int[width, depth];
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < depth; j++)
                        {
                            if (data.ElementType == "MET_USHORT")
                            {
                                byte a = br.ReadByte();
                                byte b = br.ReadByte();
                                c = 256 * b + a;
                            }

                            else if (data.ElementType == "MET_UCHAR")
                            {
                                c = br.ReadByte();
                            }

                            else
                            {
                                Console.WriteLine("Wrong element type.");
                            }

                            vData[k][i, j] = c;
                            //Console.WriteLine(c);
                        }
                    }
                }
                br.Close();
                return vData;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="xRes"></param>
        /// <param name="yRes"></param>
        /// <param name="spacing"></param>
        /// <returns></returns>
        public int[,] Cut(double[] point, double[] v1, double[] v2, int xRes, int yRes, double spacing)
        {
            iSpacing = spacing;

            double[] vertical2 = Orthogonalize2D(v1, v2);
            double lengthV1 = Math.Sqrt(ScalarProduct(v1, v1));
            double lengthV2 = Math.Sqrt(ScalarProduct(v2, v2));

            double[] unitVector1 = new double[3];
            double[] unitVector2 = new double[3];
            for (int i = 0; i < v1.Length; i++)
            {
                unitVector1[i] = v1[i] * iSpacing / lengthV1;
                unitVector2[i] = vertical2[i] * iSpacing / lengthV2;
            }

            int[,] cut = new int[xRes, yRes];
            double x, y, z;
            for (int i = 0; i < xRes; i++)
            {
                x = point[0] + i * unitVector2[0];
                y = point[1] + i * unitVector2[1];
                z = point[2] + i * unitVector2[2];

                for (int j = 0; j < yRes; j++)
                {
                    cut[i, j] = GetValue(x, y, z);
                    //Console.WriteLine(GetValue(x, y, z));

                    x += unitVector1[0];
                    y += unitVector1[1];
                    z += unitVector1[2];
                }
            }
            return cut;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="xRes"></param>
        /// <param name="yRes"></param>
        /// <param name="zRes"></param>
        /// <param name="spacing"></param>
        /// <returns></returns>
        public VolumetricData Cut3D(double[] point, double[] v1, double[] v2, double[] v3, int xRes, int yRes, int zRes, double spacing)
        {
            iSpacing = spacing;

            double[] vertical2 = Orthogonalize2D(v1, v2);
            double[] vertical3 = Orthogonalize3D(v1, vertical2, v3);
            double lengthV1 = Math.Sqrt(ScalarProduct(v1, v1));
            double lengthV2 = Math.Sqrt(ScalarProduct(vertical2, vertical2));
            double lengthV3 = Math.Sqrt(ScalarProduct(vertical3, vertical3));

            double[] unitVector1 = new double[3];
            double[] unitVector2 = new double[3];
            double[] unitVector3 = new double[3];
            for (int i = 0; i < v1.Length; i++)
            {
                unitVector1[i] = v1[i] * iSpacing / lengthV1;
                unitVector2[i] = vertical2[i] * iSpacing / lengthV2;
                unitVector3[i] = vertical3[i] * iSpacing / lengthV3;
            }

            int[][,] cut = new int[zRes][,];
            double x, y, z;

            for (int k = 0; k < zRes; k++)
            {
                cut[k] = new int[xRes, yRes];
                for (int i = 0; i < xRes; i++)
                {
                    x = point[0] + i * unitVector2[0] + k * unitVector3[0];
                    y = point[1] + i * unitVector2[1] + k * unitVector3[1];
                    z = point[2] + i * unitVector2[2] + k * unitVector3[2];

                    for (int j = 0; j < yRes; j++)
                    {
                        cut[i][j, k] = GetValue(x, y, z);

                        x += unitVector1[0];
                        y += unitVector1[1];
                        z += unitVector1[2];
                    }
                }
            }

            VolumetricData vData = new VolumetricData(data);
            vData.SetVData(cut);
            return vData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="xRes"></param>
        /// <param name="yRes"></param>
        /// <param name="zRes"></param>
        /// <param name="spacing"></param>
        /// <returns></returns>
        public int[][,] Cut3D2(double[] point, double[] v1, double[] v2, double[] v3, int xRes, int yRes, int zRes, double spacing)
        {
            iSpacing = spacing;

            double[] vertical2 = Orthogonalize2D(v1, v2);
            double[] vertical3 = Orthogonalize3D(v1, vertical2, v3);
            double lengthV1 = Math.Sqrt(ScalarProduct(v1, v1));
            double lengthV2 = Math.Sqrt(ScalarProduct(vertical2, vertical2));
            double lengthV3 = Math.Sqrt(ScalarProduct(vertical3, vertical3));

            double[] unitVector1 = new double[3];
            double[] unitVector2 = new double[3];
            double[] unitVector3 = new double[3];
            for (int i = 0; i < v1.Length; i++)
            {
                unitVector1[i] = v1[i] * iSpacing / lengthV1;
                unitVector2[i] = vertical2[i] * iSpacing / lengthV2;
                unitVector3[i] = vertical3[i] * iSpacing / lengthV3;
            }

            int[][,] cut = new int[zRes][,];
            double x, y, z;

            for (int k = 0; k < zRes; k++)
            {
                cut[k] = new int[xRes, yRes];
                for (int i = 0; i < xRes; i++)
                {
                    x = point[0] + i * unitVector2[0] + k * unitVector3[0];
                    y = point[1] + i * unitVector2[1] + k * unitVector3[1];
                    z = point[2] + i * unitVector2[2] + k * unitVector3[2];

                    for (int j = 0; j < yRes; j++)
                    {
                        cut[i][j, k] = GetValue(x, y, z);

                        x += unitVector1[0];
                        y += unitVector1[1];
                        z += unitVector1[2];
                    }
                }
            }

            return cut;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        private double[] Orthogonalize2D(double[] v1, double[] v2)
        {
            double alpha = -ScalarProduct(v1, v2) / ScalarProduct(v1, v1);

            double[] verticalVector = new double[3];
            for (int i = 0; i < verticalVector.Length; i++)
            {
                verticalVector[i] = v2[i] + alpha * v1[i];
            }
            return verticalVector;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <returns></returns>
        private double[] Orthogonalize3D(double[] v1, double[] v2, double[] v3)
        {
            double betha1 = -ScalarProduct(v1, v3) / ScalarProduct(v1, v1);
            double betha2 = -ScalarProduct(v2, v3) / ScalarProduct(v2, v2);

            double[] verticalVector = new double[3];
            for (int i = 0; i < verticalVector.Length; i++)
            {
                verticalVector[i] = v3[i] + betha1 * v1[i] + betha2 * v2[i];
            }
            return verticalVector;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        private double ScalarProduct(double[] v1, double[] v2)
        {
            return v1[0] * v2[0] + v1[1] * v2[1] + v1[2] * v2[2];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public int GetValue(double x, double y, double z) // Interpolation3D 
        {
            //DEBUG, NOT IN CUTS!!!!!
            //int xLDC = (int)(x / xSpacing); // coordinates of left down corner of the rectangle in the array in which the pixel is situated
            //int yLDC = (int)(y / ySpacing);
            //int zLDC = (int)(z / zSpacing);
            //Console.WriteLine("x: " + xLDC + " y: " + yLDC + " z: " + zLDC);

            int xLDC = (int)(x); // coordinates of left down corner of the rectangle in the array in which the pixel is situated
            int yLDC = (int)(y);
            int zLDC = (int)(z);

            if (xLDC < data.DimSize[0] && yLDC < data.DimSize[1] && zLDC < data.DimSize[2] && xLDC >= 0 && yLDC >= 0 && zLDC >= 0)
            {
                int zRDC = zLDC + 1;
                if (zLDC == this.data.DimSize[2] - 1)
                {
                    zRDC = zLDC;
                }

                int valueA = Interpolation2D(x, y, zLDC, xLDC, yLDC);
                int valueB = Interpolation2D(x, y, zRDC, xLDC, yLDC);

                return Interpolation(valueA, valueB, z, zLDC, zSpacing);
            }
            else
            {
                //Console.WriteLine(-1);
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueA"></param>
        /// <param name="valueB"></param>
        /// <param name="coordinateOfPixel"></param>
        /// <param name="indexOfA"></param>
        /// <param name="spacing"></param>
        /// <returns></returns>
        private int Interpolation(int valueA, int valueB, double coordinateOfPixel, int indexOfA, double spacing)
        {

            //double d = coordinateOfPixel - indexOfA * spacing;
            //double r = d / spacing;
            double d = coordinateOfPixel - indexOfA;
            double r = d;
            return (int)(r * valueB + (1 - r) * valueA);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelX"></param>
        /// <param name="pixelY"></param>
        /// <param name="indexLDCZ"></param>
        /// <param name="xLDC"></param>
        /// <param name="yLDC"></param>
        /// <returns></returns>
        private int Interpolation2D(double pixelX, double pixelY, int indexLDCZ, int xLDC, int yLDC)
        {
            int xRDC = xLDC + 1;
            int yRDC = yLDC + 1;

            if (xLDC == this.data.DimSize[0] - 1)
            {
                xRDC = xLDC;
            }

            if (yLDC == this.data.DimSize[1] - 1)
            {
                yRDC = yLDC;
            }

            int valueA = vData[indexLDCZ][xLDC, yLDC];
            int valueB = vData[indexLDCZ][xRDC, yLDC];

            int helpValueA = Interpolation(valueA, valueB, pixelX, xLDC, xSpacing);

            int valueC = vData[indexLDCZ][xLDC, yRDC];
            int valueD = vData[indexLDCZ][xRDC, yRDC];

            int helpValueB = Interpolation(valueC, valueD, pixelX, xLDC, xSpacing);

            return Interpolation(helpValueA, helpValueB, pixelY, yLDC, ySpacing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vData"></param>
        public void SetVData(int[][,] vData)
        {
            this.vData = vData;
        } //TODO set private?


        // methods for testing
        public VolumetricData()
        {

        }

        public double GetXSpacing()
        {
            return this.xSpacing;
        }

        public void SetXSpacing(double dx)
        {
            this.xSpacing = dx;
        }

        public double GetYSpacing()
        {
            return this.ySpacing;
        }

        public void SetYSpacing(double dy)
        {
            this.ySpacing = dy;
        }

        public double GetZSpacing()
        {
            return this.zSpacing;
        }

        public void SetZSpacing(double dz)
        {
            this.zSpacing = dz;
        }

        public int[] GetMeassures()
        {
            return new int[] { data.DimSize[0], data.DimSize[1], data.DimSize[2] };
        }

    }
}
