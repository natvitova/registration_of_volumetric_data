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
        private int[][,] vData;
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
            XSpacing = data.ElementSpacing[0];
            YSpacing = data.ElementSpacing[1];
            ZSpacing = data.ElementSpacing[2];

            this.Data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int[][,] Read()
        {
            using (BinaryReader br = new BinaryReader(new FileStream(Data.ElementDataFile, FileMode.Open)))
            {
                int width = Data.DimSize[0];
                int depth = Data.DimSize[1];
                int height = Data.DimSize[2];

                VData = new int[height][,];
                int c = 0;

                if (Data.ElementType == "MET_USHORT")
                {
                    for (int k = 0; k < height; k++)
                    {
                        VData[k] = new int[width, depth];
                        for (int i = 0; i < width; i++)
                        {
                            for (int j = 0; j < depth; j++)
                            {

                                byte a = br.ReadByte();
                                byte b = br.ReadByte();
                                c = 256 * b + a;

                                VData[k][i, j] = c;
                                //Console.WriteLine(c);

                            }
                        }
                    }
                }

                else if (Data.ElementType == "MET_UCHAR")
                {
                    for (int k = 0; k < height; k++)
                    {
                        VData[k] = new int[width, depth];
                        for (int i = 0; i < width; i++)
                        {
                            for (int j = 0; j < depth; j++)
                            {
                                c = br.ReadByte();

                                VData[k][i, j] = c;
                                //Console.WriteLine(c);
                            }
                        }
                    }
                    
                }
                else
                {
                    Console.WriteLine("Wrong element type.");
                }

                br.Close();
                return VData;
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
            ISpacing = spacing;

            double[] vertical2 = Orthogonalize2D(v1, v2);
            double lengthV1 = Math.Sqrt(ScalarProduct(v1, v1));
            double lengthV2 = Math.Sqrt(ScalarProduct(v2, v2));

            double[] unitVector1 = new double[3];
            double[] unitVector2 = new double[3];
            for (int i = 0; i < v1.Length; i++)
            {
                unitVector1[i] = v1[i] * ISpacing / lengthV1;
                unitVector2[i] = vertical2[i] * ISpacing / lengthV2;
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
                    cut[i, j] = GetValueMatrixCoordinates(x, y, z);
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
            ISpacing = spacing;

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
                unitVector1[i] = v1[i] * ISpacing / lengthV1;
                unitVector2[i] = vertical2[i] * ISpacing / lengthV2;
                unitVector3[i] = vertical3[i] * ISpacing / lengthV3;
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
                        cut[k][i, j] = GetValueMatrixCoordinates(x, y, z);

                        x += unitVector1[0];
                        y += unitVector1[1];
                        z += unitVector1[2];
                    }
                }
            }

            VolumetricData vDnew = new VolumetricData(Data);
            vDnew.VData = cut;
            
            return vDnew;
        }

        public VolumetricData CutVol(int[] translation)
        {
            int xRes = Measures[0] - translation[0];
            int yRes = Measures[1] - translation[1];
            int zRes = Measures[2] - translation[2];

            int[][,] cut = new int[zRes][,];

            for (int k = 0; k < zRes; k++)
            {
                cut[k] = new int[xRes, yRes];
                for (int i = 0; i < xRes; i++)
                {
                    for (int j = 0; j < yRes; j++)
                    {
                        cut[k][i, j] = VData[translation[2] + k][translation[0] + i, translation[1] + j];
                    }
                }
            }

            Data copy = new Data(this.Data);
            VolumetricData vDnew = new VolumetricData(copy);
            vDnew.VData = cut;
            vDnew.Measures = new int[] { xRes, yRes, zRes };

            return vDnew;
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
            ISpacing = spacing;

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
                unitVector1[i] = v1[i] * ISpacing / lengthV1;
                unitVector2[i] = vertical2[i] * ISpacing / lengthV2;
                unitVector3[i] = vertical3[i] * ISpacing / lengthV3;
            }

            int[][,] cut = new int[zRes][,];
            for (int k = 0; k < zRes; k++)
            {
                cut[k] = new int[xRes, yRes];
                for (int i = 0; i < xRes; i++)
                {
                    double x = point[0] + i * unitVector2[0] + k * unitVector3[0];
                    double y = point[1] + i * unitVector2[1] + k * unitVector3[1];
                    double z = point[2] + i * unitVector2[2] + k * unitVector3[2];
                    for (int j = 0; j < yRes; j++)
                    {
                        cut[k][i, j] = GetValueMatrixCoordinates(x, y, z);

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
        public int GetValueMatrixCoordinates(double x, double y, double z) // Interpolation3D in matrix coordinates
        {
            //TODO CUTS - real or matrix coordinates

            int xLDC = (int)(x); // coordinates of left down corner of the rectangle in the array in which the pixel is situated
            int yLDC = (int)(y);
            int zLDC = (int)(z);

            if (xLDC < Data.DimSize[0] && yLDC < Data.DimSize[1] && zLDC < Data.DimSize[2] && xLDC >= 0 && yLDC >= 0 && zLDC >= 0)
            {
                int zRDC = zLDC + 1;
                if (zLDC == this.Data.DimSize[2] - 1)
                {
                    zRDC = zLDC;
                }

                int valueA = Interpolation2DWithinMatrix(x, y, zLDC, xLDC, yLDC);
                int valueB = Interpolation2DWithinMatrix(x, y, zRDC, xLDC, yLDC);

                return InterpolationWithinMatrix(valueA, valueB, z, zLDC);
            }
            else
            {
                //Console.WriteLine(-1);
                return 0;
            }
        }

        public int GetValueMatrixCoordinates(Point3D point)
        {
            return GetValueMatrixCoordinates(point.X, point.Y, point.Z);
        }

        public int GetValue(double x, double y, double z) // Interpolation3D in real coordinates 
        {
            int xLDC = (int)(x / xSpacing); // coordinates of left down corner of the rectangle in the array in which the pixel is situated
            int yLDC = (int)(y / ySpacing);
            int zLDC = (int)(z / zSpacing);

            if (xLDC < Data.DimSize[0] && yLDC < Data.DimSize[1] && zLDC < Data.DimSize[2] && xLDC >= 0 && yLDC >= 0 && zLDC >= 0)
            {
                int zRDC = zLDC + 1;
                if (zLDC == this.Data.DimSize[2] - 1)
                {
                    zRDC = zLDC;
                }

                int valueA = Interpolation2DReal(x, y, zLDC, xLDC, yLDC);
                int valueB = Interpolation2DReal(x, y, zRDC, xLDC, yLDC);

                return InterpolationReal(valueA, valueB, z, zLDC, ZSpacing);
            }
            else
            {
                return 0;
            }
        }

        public int GetValue(Point3D point)
        {
            return GetValue(point.X, point.Y, point.Z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueA"></param>
        /// <param name="valueB"></param>
        /// <param name="coordinateOfPixel"></param>
        /// <param name="indexOfA"></param>
        /// <returns></returns>
        private int InterpolationWithinMatrix(int valueA, int valueB, double coordinateOfPixel, int indexOfA)
        {
            double d = coordinateOfPixel - indexOfA;
            return (int)(d * valueB + (1 - d) * valueA);
        }

        private int InterpolationReal(int valueA, int valueB, double coordinateOfPixel, int indexOfA, double spacing)
        {

            double d = coordinateOfPixel - indexOfA * spacing;
            double r = d / spacing;
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
        private int Interpolation2DWithinMatrix(double pixelX, double pixelY, int indexLDCZ, int xLDC, int yLDC)
        {
            int xRDC = xLDC + 1;
            int yRDC = yLDC + 1;

            if (xLDC == this.Data.DimSize[0] - 1)
            {
                xRDC = xLDC;
            }

            if (yLDC == this.Data.DimSize[1] - 1)
            {
                yRDC = yLDC;
            }

            int valueA = VData[indexLDCZ][xLDC, yLDC];
            int valueB = VData[indexLDCZ][xRDC, yLDC];

            int helpValueA = InterpolationWithinMatrix(valueA, valueB, pixelX, xLDC);

            int valueC = VData[indexLDCZ][xLDC, yRDC];
            int valueD = VData[indexLDCZ][xRDC, yRDC];

            int helpValueB = InterpolationWithinMatrix(valueC, valueD, pixelX, xLDC);

            return InterpolationWithinMatrix(helpValueA, helpValueB, pixelY, yLDC);
        }

        private int Interpolation2DReal(double pixelX, double pixelY, int indexLDCZ, int xLDC, int yLDC)
        {
            int xRDC = xLDC + 1;
            int yRDC = yLDC + 1;

            if (xLDC == this.Data.DimSize[0] - 1)
            {
                xRDC = xLDC;
            }

            if (yLDC == this.Data.DimSize[1] - 1)
            {
                yRDC = yLDC;
            }

            int valueA = VData[indexLDCZ][xLDC, yLDC];
            int valueB = VData[indexLDCZ][xRDC, yLDC];

            int helpValueA = InterpolationReal(valueA, valueB, pixelX, xLDC, XSpacing);

            int valueC = VData[indexLDCZ][xLDC, yRDC];
            int valueD = VData[indexLDCZ][xRDC, yRDC];

            int helpValueB = InterpolationReal(valueC, valueD, pixelX, xLDC, XSpacing);

            return InterpolationReal(helpValueA, helpValueB, pixelY, yLDC, YSpacing);
        }

        // methods for testing
        public VolumetricData()
        {
            //this.vData = vData;
            this.xSpacing = 1;
            this.ySpacing = 1;
            this.zSpacing = 1;
            this.iSpacing = 1;
            this.data = new Data();
        }

        public double XSpacing { get => xSpacing; set => xSpacing = value; }
        public double YSpacing { get => ySpacing; set => ySpacing = value; }
        public double ZSpacing { get => zSpacing; set => zSpacing = value; }
        public double ISpacing { get => iSpacing; set => iSpacing = value; }
        internal Data Data { get => data; set => data = value; }
        public int[] Measures { get => Data.DimSize; set => Data.DimSize = value; }
        public int[][,] VData { get => vData; set => vData = value; }

    }
}
