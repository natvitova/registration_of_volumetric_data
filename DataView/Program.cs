using System;

namespace DataView
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        private static Data sample;
        private static VolumetricData vData;
        private static Data sample2;
        private static VolumetricData vData2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //string fileName = @"P01_a_MikroCT-nejhrubsi_rozliseni_DICOM_liver-1st-important_Macro_pixel-size53.0585um.mhd";
            //string fileName2 = @"P01_b_Prase_1_druhe_vys.mhd";

            ////----------------------------------------MICRO CT------------------------------------------------
            //sample = new Data();
            //sample.SetFeatures(fileName);
            //vData = new VolumetricData(sample);
            //Console.WriteLine("Reading first data.");
            //vData.Read();
            //Console.WriteLine("Data read succesfully.");
            //IFeatureComputer fc = new FeatureComputer();
            //ISampler s = new Sampler();

            //Console.WriteLine("Sampling");
            //Point3D[] points = s.Sample(vData, 10);

            //FeatureVector[] featureVectors = new FeatureVector[points.Length];

            //Console.WriteLine("Computing feature vectors.");
            //for (int i = 0; i < points.Length; i++)
            //{
            //    featureVectors[i] = fc.ComputeFeatureVector(vData, points[i]);
            //    Console.WriteLine("fv1:" + i + " " + featureVectors[i].ToString());
            //}

            ////----------------------------------------MACRO CT------------------------------------------------
            //sample2 = new Data();
            //sample2.SetFeatures(fileName2);
            //vData2 = new VolumetricData(sample2);
            //Console.WriteLine("\nReading second data.");
            //vData2.Read();
            //Console.WriteLine("Data read succesfully.");
            //IFeatureComputer fc2 = new FeatureComputer();
            //ISampler s2 = new Sampler();

            //Console.WriteLine("Sampling");
            //Point3D[] points2 = s2.Sample(vData2, 500);
            //FeatureVector[] featureVectors2 = new FeatureVector[points2.Length];

            //int[] m = vData.GetMeassures();
            //Console.WriteLine("x:" + m[0] + " y:" + m[1] + " z:" + m[2]);

            //Console.WriteLine("Going through the data.");
            //int aaa = 0;
            //int bbb = 0;
            //int ccc = 0;
            //for (int k = 0; k < m[2]; k++)
            //{
            //    for (int i = 0; i < m[0]; i++)
            //    {
            //        for (int j = 0; j < m[1]; j++)
            //        {
            //            double a = vData.vData[k][i, j];
            //            // DEBUG 
            //            //double b = vData2.GetValue(i * vData2.GetXSpacing(), j * vData2.GetYSpacing(), k * vData2.GetZSpacing());
            //            double b = vData.GetValue(i, j, k);
            //            if ((a - b) > 1)
            //            {
            //                aaa++;
            //                if ((a - b) > a * 0.5)
            //                {
            //                    bbb++;
            //                }

            //                Console.WriteLine("a:" + a + " b:" + b);
            //            }
            //            if (b == 0 && a != 0)
            //            {
            //                //Console.WriteLine("x:" + i + " y:" + j + " z:" + k);
            //                //Console.WriteLine(a);
            //                ccc++;
            //            }
            //        }
            //    }
            //}
            //Console.WriteLine("Count of mistakes ................................." + aaa);
            //Console.WriteLine("Count of mistakes (abs < 75%) ....................." + bbb);
            //Console.WriteLine("Count of mistakes (b==0) .........................." + ccc);


            //Console.WriteLine("Computing feature vectors.");
            //for (int i = 0; i < points2.Length; i++)
            //{
            //    featureVectors2[i] = fc2.ComputeFeatureVector(vData2, points2[i]);
            //    Console.WriteLine("fv2:" + i + " " + featureVectors2[i].ToString());
            //}


            //IMatcher matcher = new Matcher();
            //Console.WriteLine("Matching.");
            //Match[] matches = matcher.Match(featureVectors, featureVectors2);
            //Console.WriteLine(".......................... MATCHES ..............................");
            //for (int i = 0; i < matches.Length; i++)
            //{
            //    Console.WriteLine(matches[i].ToString());
            //}

            //Console.ReadKey();

            // ------------------------------------------Cut-------------------------------------------
            //int distance = 250;
            //int direction = 2;

            //double[] point = { 150, 500, 150};
            //double[] v1 = { 1, 0, 0};
            //double[] v2 = { 0, 0, 1 };
            //double[] v3 = { 2, 1, 5 };
            //int xRes = 500;
            //int yRes = 500;
            //double spacing = 0.5;
            //string finalFile = GenerateFinalFileName(point, v1, v2, xRes, yRes, spacing);

            //if (ControlData(fileName2, distance, direction))
            //{
            //    //int[] dimenses = vData.GetMeassures();
            //    //Console.WriteLine(dimenses[0] + " " + dimenses[1] + " " + dimenses[2]);
            //    //Console.ReadKey();
            //    double[] spacings = { vData.GetXSpacing(), vData.GetYSpacing(), vData.GetZSpacing() };
            //    double[] realPoint = new double[3];
            //    for (int i = 0; i < realPoint.Length; i++)
            //    {
            //        realPoint[i] = point[i] * spacings[i];
            //    }
            //    int[,] cut = vData.Cut(realPoint, v1, v2, xRes, yRes, spacing);

            //    Console.WriteLine("Cut finished");
            //    PictureMaker pm = new PictureMaker(cut);
            //    Bitmap bitmap = pm.MakeBitmap();
            //    Console.WriteLine("Bitmap finished");
            //    Console.ReadKey();

            //    try
            //    {
            //        bitmap.Save(finalFile, System.Drawing.Imaging.ImageFormat.Bmp);
            //    }
            //    catch (Exception e)
            //    {

            //        Console.Write(e.Message);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("The distance is higher than the dimension.");
            //    Console.ReadKey();
            //}
            // ------------------------------------------Cut-------------------------------------------


            //string filename1 = "x.mhd";
            //string filename2 = "y.mhd";


            //Console.WriteLine("Reading vData1");
            //Data data1 = new Data();
            //data1.SetFeatures(fileName1);
            //VolumetricData vData1 = new VolumetricData(data1);
            //vData1.Read();
            //Console.WriteLine("vData1 read");

            //Console.WriteLine("Reading vData2");
            //Data data2 = new Data();
            //data2.SetFeatures(fileName2);
            //VolumetricData vData2 = new VolumetricData(data2);
            //vData2.Read();
            //Console.WriteLine("vData2 read");

            //Console.WriteLine("Calculating transformation");
            //Transform3D transform = new Transform3D();
            //Point3D a = new Point3D(100, 100, 100);
            //Point3D b = new Point3D(100, 100, 100);
            //transform.GetTransformation(a, b, vData1, vData2);
            //Console.WriteLine("Transformation calculation finished");
            //Console.ReadLine();

            /*
            Transform3D transform = new Transform3D();
            transform.testPCA();
            Console.ReadLine();
            */
            /*
            

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
            
            */

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

            string fileName1 = "P01_a_MikroCT-nejhrubsi_rozliseni_DICOM_liver-1st-important_Macro_pixel-size53.0585um.raw";
            string fileName2 = "P01_a_MikroCT-nejhrubsi_rozliseni_DICOM_liver-1st-important_Macro_pixel-size53.0585um.mhd";
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
            sample = new Data();
            sample.SetFeatures(fileName2);

            vData = new VolumetricData(sample);
            vData.Read();

            Console.WriteLine("Data passed");
            Console.WriteLine("Sampling...");
            ISampler sampler = new SamplerFeatureVector();
            sampler.Sample(vData, 100);
            Console.WriteLine("Creating cuts...");

                //CreateCutsInDirection(v1, v2, xRes, yRes, spacing);

                Console.WriteLine("All cuts created");

                Console.ReadLine();

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

