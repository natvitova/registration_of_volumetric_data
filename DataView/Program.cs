using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace DataView
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        private static Data sample;
        private static VolumetricData vData;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            
            string fileName1 = "x.mhd";
            string fileName2 = "x.mhd";

            Data data1 = new Data();
            data1.SetFeatures(fileName1);
            VolumetricData vData1 = new VolumetricData(data1);
            vData1.Read();
            Console.WriteLine("vData1 read");

            Data data2 = new Data();
            data2.SetFeatures(fileName2);
            VolumetricData vData2 = new VolumetricData(data2);
            vData2.Read();
            Console.WriteLine("vData2 read");

            Transform3D transform = new Transform3D();
            transform.CalculateRotation(vData1, vData2);
            Console.WriteLine("Transformation calculation finished");
            Console.ReadLine();
            


            /*
            string rotation = "x";
            TestDataBuilder builder = new TestDataBuilder();
            builder.BuildData(rotation);
            Console.WriteLine("uspech");
            //Console.ReadLine()

            string fileName = rotation + @".mhd";

            int distance = 750;
            int direction = 2;

            double[] point = { 150, 150, 150 };
            double[] v1 = { 1, 0, 0 };
            double[] v2 = { 0, 1, 0 };
            
            int xRes = 500;
            int yRes = 500;
            double spacing = 0.5;

            Console.WriteLine("Controlling data...");
            if (ControlData(fileName, distance, direction))
            {
                Console.WriteLine("Data passed");
                Console.WriteLine("Creating cuts...");
                CreateCutsInDirection(v1, v2, xRes, yRes, spacing, rotation);
                Console.WriteLine("All cuts created");
            }

            rotation = "y";
            builder = new TestDataBuilder();
            builder.BuildData(rotation);
            Console.WriteLine("uspech");
            //Console.ReadLine()

            fileName = rotation + @".mhd";


            Console.WriteLine("Controlling data...");
            if (ControlData(fileName, distance, direction))
            {
                Console.WriteLine("Data passed");
                Console.WriteLine("Creating cuts...");
                CreateCutsInDirection(v1, v2, xRes, yRes, spacing, rotation);
                Console.WriteLine("All cuts created");
            }



            rotation = "z";
            builder = new TestDataBuilder();
            builder.BuildData(rotation);
            Console.WriteLine("uspech");
            //Console.ReadLine()

            fileName = rotation + @".mhd";

            Console.WriteLine("Controlling data...");
            if (ControlData(fileName, distance, direction))
            {
                Console.WriteLine("Data passed");
                Console.WriteLine("Creating cuts...");
                CreateCutsInDirection(v1, v2, xRes, yRes, spacing, rotation);
                Console.WriteLine("All cuts created");
            }
            
            Console.WriteLine("\n\n\nHOTOVO");
            Console.ReadLine();
            */


            //string fileName = @"P01_a_MikroCT-nejhrubsi_rozliseni_DICOM_liver-1st-important_Macro_pixel-size53.0585um.mhd";
            //string fileName2 = @"P01_b_Prase_1_druhe_vys.mhd";

            //sample = new Data();
            //sample.SetFeatures(fileName2);
            //vData = new VolumetricData(sample);
            //vData.Read();
            //FeatureComputer fc = new FeatureComputer();
            //Sampler s = new Sampler();

            //Point3D[] points = s.Sample(vData, 15);
            //double[][] featureVectors = new double[points.Length][];

            //for (int i = 0; i < points.Length; i++)
            //{
            //    featureVectors[0] = fc.ComputeFeatureVector(vData, points[i]);
            //    Console.WriteLine(points[i].x + " " + points[i].y + " " + points[i].z);
            //}
            //Console.ReadKey();


            /*
            int distance = 750;
            int direction = 2;

            double[] point = { 150, 150, 150};
            double[] v1 = { 1, 0, 0};
            double[] v2 = { 0, 1, 0 };
            double[] v3 = { 2, 1, 5 };
            int xRes = 500;
            int yRes = 500;
            double spacing = 0.5;
            string finalFile = GenerateFinalFileName(point, v1, v2, xRes, yRes, spacing);
            Console.WriteLine("Final file name: " + finalFile);

            Console.WriteLine("Controlling data...");
            if (ControlData(fileName, distance, direction))
            {
                Console.WriteLine("Data passed");
                Console.WriteLine("Sampling...");
                ISampler sampler = new SamplerFeatureVector();
                sampler.Sample(vData, 100);
                Console.WriteLine("Creating cuts...");

                CreateCutsInDirection(v1, v2, xRes, yRes, spacing);

                Console.WriteLine("All cuts created");

                Console.ReadLine();
            }
            */
            //sample = new Data();
            //sample.SetFeatures(fileName);
            //vData = new VolumetricData(sample);
            //double[] vertical2 = vData.Orthogonalize2D(v1, v2);
            //double[] vertical3 = vData.Orthogonalize3D(v1, vertical2, v3);

            //double scalar12 = vData.ScalarProduct(v1, vertical2);
            //double scalar13 = vData.ScalarProduct(v1, vertical3);
            //double scalar23 = vData.ScalarProduct(vertical2, vertical3);

            //Console.WriteLine(v1[0] + " " + v1[1] + " " + v1[2]);
            //Console.WriteLine(vertical2[0] + " " + vertical2[1] + " " + vertical2[2]);
            //Console.WriteLine(vertical3[0] + " " + vertical3[1] + " " + vertical3[2]);
            //Console.WriteLine(scalar12 + " " + scalar13 + " " + scalar23);
            //Console.ReadKey();
            /*
                            Console.WriteLine("Controlling data...");
                        if (ControlData(fileName, distance, direction))
                        {
                            //int[] dimenses = vData.GetMeassures();
                            //Console.WriteLine(dimenses[0] + " " + dimenses[1] + " " + dimenses[2]);
                            //Console.ReadKey();

                            Console.WriteLine("Data passed");
                            double[] spacings = { vData.GetXSpacing(), vData.GetYSpacing(), vData.GetZSpacing() };
                            double[] realPoint = new double[3];
                            for(int i = 0; i < realPoint.Length; i++)
                            {
                                realPoint[i] = point[i] * spacings[i];
                            }
                            Console.WriteLine("Cutting...");
                            int[,] cut = vData.Cut(realPoint, v1, v2, xRes, yRes, spacing);

                            Console.WriteLine("Cut finished");
                            Console.WriteLine("Creating bitmap...");
                            PictureMaker pm = new PictureMaker(cut);
                            Bitmap bitmap = pm.MakeBitmap();
                            Console.WriteLine("Bitmap finished");

                            Console.WriteLine("Saving bitmap to file...");
                            try
                            {
                                bitmap.Save(finalFile, System.Drawing.Imaging.ImageFormat.Bmp);
                                Console.WriteLine("Save to bitmap succesful");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Save to bitmap failed");
                                Console.Write(e.Message);
                            }

                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("The distance is higher than the dimension.");
                            Console.ReadKey();
                        }
                        */
        }


        public static void CreateCutsInDirection(double[] v1, double[] v2, int xRes, int yRes, double spacing, string prefix)
        {

            double[] point = { 0, 0, 0 };
            for (int j = 0; j < 10; j++)
            {
                point[2] = j * 50;
            


                string finalFile = prefix + "_" + GenerateFinalFileName(point, v1, v2, xRes, yRes, spacing);
                Console.WriteLine("Final file name: " + finalFile);

                double[] spacings = { vData.GetXSpacing(), vData.GetYSpacing(), vData.GetZSpacing() };
                double[] realPoint = new double[3];
                for (int i = 0; i < realPoint.Length; i++)
                {
                    realPoint[i] = point[i] * spacings[i];
                }
                Console.WriteLine("Cutting...");
                int[,] cut = vData.Cut(realPoint, v1, v2, xRes, yRes, spacing);

                Console.WriteLine("Cut finished");
                Console.WriteLine("Creating bitmap...");
                PictureMaker pm = new PictureMaker(cut);
                Bitmap bitmap = pm.MakeBitmap();
                Console.WriteLine("Bitmap finished");

                Console.WriteLine("Saving bitmap to file...");
                try
                {
                    bitmap.Save(finalFile, System.Drawing.Imaging.ImageFormat.Bmp);
                    Console.WriteLine("Save to bitmap succesful");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Save to bitmap failed");
                    Console.Write(e.Message);
                }
            }
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static void TestData(string fileName)
        {
            Data d = new Data();
            d.SetFeatures(fileName);
            string s = "type: " + d.ElementType + " file: " + d.ElementDataFile + " dimSize: ";
            for (int i = 0; i < d.DimSize.Length; i++)
            {
                s += d.DimSize[i] + " ";
            }
            s += "spacing: ";
            for (int i = 0; i < d.ElementSpacing.Length; i++)
            {
                s += d.ElementSpacing[i] + " ";
            }

            Console.Write(s);
            Console.ReadKey();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static Boolean ControlData(string fileName, int distance, int direction)
        {
            sample = new Data();
            sample.SetFeatures(fileName);
            vData = new VolumetricData(sample);
            vData.Read();

            if (distance < sample.DimSize[direction])
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="direction"></param>
        /// <param name="iSpacing"></param>
        /// <returns></returns>
        public static string GenerateFinalFileName(double[] point, double[] v1, double[] v2, int xRes, int yRes, double spacing)
        {
            string p = "p" + point[0] + "-" + point[1] + "-" + point[2];
            string v = "v" + v1[0] + "-" + v1[1] + "-" + v1[2] + "_" + v2[0] + "-" + v2[1] + "-" + v2[2];
            string r = "r" + xRes + "-" + yRes;

            return "testCut" + "_" + p + "_" + v + "_" + r + "_" + spacing + ".bmp";
        }

    }
}

