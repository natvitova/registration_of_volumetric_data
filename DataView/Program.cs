using System;
using System.Collections.Generic;
using System.Drawing;


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

            string fileName = @"P01_b_Prase_1_druhe_vys.mhd";
            string fileName2 = fileName;

            //MainFunction(fileName, fileName2);

            MainFunctionAD();



            //---------------------------------------Matějův divný kód - NESAHAT!!!!!!!!!!!!!!!!!!!!----------
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
            //---------------------------------------Matějův divný kód - NESAHAT!!!!!!!!!!!!!!!!!!!!----------
        }

        public static void MainFunction(string micro, string macro)
        {
            //----------------------------------------MICRO CT------------------------------------------------
            sample = new Data();
            sample.SetFeatures(micro);
            vData = new VolumetricData(sample);
            Console.WriteLine("Reading first data.");
            vData.Read();
            Console.WriteLine("Data read succesfully.");
            IFeatureComputer fc = new FeatureComputer();
            ISampler s = new Sampler();

            Console.WriteLine("Sampling.");
            Point3D[] points = s.Sample(vData, 10);

            FeatureVector[] featureVectors = new FeatureVector[points.Length];

            Console.WriteLine("Computing feature vectors.");
            for (int i = 0; i < points.Length; i++)
            {
                featureVectors[i] = fc.ComputeFeatureVector(vData, points[i]);
                //Console.WriteLine("fv1:" + i + " " + featureVectors[i].ToString());
            }

            //----------------------------------------MACRO CT------------------------------------------------
            sample2 = new Data();
            sample2.SetFeatures(macro);
            vData2 = new VolumetricData(sample2);
            Console.WriteLine("\nReading second data.");
            vData2.Read();
            Console.WriteLine("Data read succesfully.");
            IFeatureComputer fc2 = new FeatureComputer();
            ISampler s2 = new Sampler();

            Console.WriteLine("Sampling.");
            Point3D[] points2 = s2.Sample(vData2, 10000);

            FeatureVector[] featureVectors2 = new FeatureVector[points2.Length];

            Console.WriteLine("Computing feature vectors.");
            for (int i = 0; i < points2.Length; i++)
            {
                featureVectors2[i] = fc2.ComputeFeatureVector(vData2, points2[i]);
                //Console.WriteLine("fv2:" + i + " " + featureVectors2[i].ToString());
            }

            //----------------------------------------MATCHES-------------------------------------------------
            IMatcher matcher = new Matcher();
            Console.WriteLine("\nMatching.");
            Match[] matches = matcher.Match(featureVectors, featureVectors2);

            //Console.WriteLine(".......................... MATCHES ..............................");
            //for (int i = 0; i < matches.Length; i++)
            //{
            //    Console.WriteLine(matches[i].ToString());
            //}


            //------------------------------------GET TRANSFORMATION -----------------------------------------
            ITransformer transformer = new Transformer3D();
            Console.WriteLine("Computing transformations.\n");
            //Transform3D[] transformations = new Transform3D[matches.Length];

            double threshold = 99.9999;
            List<Transform3D> transformations = new List<Transform3D>();
            //int countT9 = 0;

            for (int i = 0; i < matches.Length; i++)
            {
                if (matches[i].Similarity > threshold)
                {
                    transformations.Add(transformer.GetTransformation(matches[i], vData, vData2));
                }
            }

            //for (int i = 0; i < transformations.Length; i++)
            //{
            //    transformations[i] = transformer.GetTransformation(matches[i], vData, vData2);
            //    if (matches[i].Percentage > trashHold)
            //    {
            //        countT9++;
            //        Console.WriteLine(matches[i].ToString());
            //        Console.WriteLine(transformations[i].ToString());
            //    }
            //}

            //Console.WriteLine("\nAll transformations obtained.\n");
            //Console.WriteLine("Count of all transformations: ..........          " + transformations.Length);
            //Console.WriteLine("Count of all transformations better than " + trashHold + ": " + countT9);
            //Console.ReadKey();

            Candidate.initSums(vData.Measures[0] / vData.XSpacing, vData.Measures[1] / vData.YSpacing, vData.Measures[2] / vData.ZSpacing);
            Density d = new Density(); // finder, we need an instance for certain complicated reason
            Transform3D solution = d.Find(transformations.ToArray());
            Console.WriteLine(solution);
            Console.WriteLine("Solution found.");
            Console.ReadKey();
        }

        public static void MainFunctionAD()
        {
            //----------------------------------------PARAMS -------------------------------------------------
            double threshold = 20; // percentage
            int numberOfPoints = 10_000; // micro
            int numberOfPoints2 = 10_000;
            ArtificialData aData = new ArtificialData(1, 3, 8);
            int [] translation = new int[] { 15, 15, 15 };
            aData.SetSmallerData(translation);
            vData = aData.VD2; // micro
            vData2 = aData.VD;
            Console.WriteLine("Artificial data created succesfully.");

            //----------------------------------------DATA ---------------------------------------------------
            FeatureComputer fc = new FeatureComputer();
            ISampler s = new Sampler();
            FeatureComputer fc2 = new FeatureComputer();
            //ISampler s2 = new SamplerFake();
            SamplerFake s2 = new SamplerFake();
            s2.getTranslation(translation);

            Console.WriteLine("Sampling.");
            //Point3D[] points = s.Sample(vData, numberOfPoints);
            Point3D[] points2 = s2.Sample(vData2, numberOfPoints2); // macro
            Point3D[] points = new Point3D[points2.Length];
            int[] cut = aData.Cut;
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new Point3D(points2[i].X - cut[0], points2[i].Y - cut[1], points2[i].Z - cut[2]);
            }

            FeatureVector[] featureVectors = new FeatureVector[points.Length];
            FeatureVector[] featureVectors2 = new FeatureVector[points2.Length];

            Console.WriteLine("Computing feature vectors.");
            for (int i = 0; i < points.Length; i++)
            {
                featureVectors[i] = fc.ComputeFeatureVectorA(translation, points[i]);
                //Console.WriteLine("fv1:" + i + " " + featureVectors[i].ToString());
            }

            int[] tr2 = new int[] { 0, 0, 0 };
            for (int i = 0; i < points2.Length; i++)
            {
                featureVectors2[i] = fc2.ComputeFeatureVectorA(tr2, points2[i]);
                //Console.WriteLine("fv2:" + i + " " + featureVectors2[i].ToString());
            }

            //----------------------------------------MATCHES-------------------------------------------------
            IMatcher matcher = new Matcher();
            Console.WriteLine("\nMatching.");
            Match[] matches = matcher.Match(featureVectors, featureVectors2, threshold);

            Console.WriteLine(matches.Length + ".......................... MATCHES ..............................");

            //------------------------------------GET TRANSFORMATION -----------------------------------------
            ITransformer transformer = new Transformer3D();
            Console.WriteLine("Computing transformations.\n");
            //Transform3D[] transformations = new Transform3D[matches.Length];

            List<Transform3D> transformations = new List<Transform3D>();
            //int countT9 = 0;

            for (int i = 0; i < matches.Length; i++)
            {
                Transform3D t = transformer.GetTransformation(matches[i], vData, vData2);
                transformations.Add(t);
                //Console.WriteLine(t);
            }

            Console.WriteLine("Looking for optimal transformation.\n");
            Candidate.initSums(vData.Measures[0] / vData.XSpacing, vData.Measures[1] / vData.YSpacing, vData.Measures[2] / vData.ZSpacing);
            Density d = new Density(); // finder, we need an instance for certain complicated reason
            Transform3D solution = d.Find(transformations.ToArray());
            Console.WriteLine(solution);
            Console.WriteLine("Solution found.");
            Console.ReadKey();
        }

        public static void Cut()
        {
            //TODO change for 3D/2D, make nonspecific
            string fileName = "P01_HEAD_5_0_H31S_0004.mhd";
            int distance = 20;
            int direction = 2;

            double[] point = { 100, 100, 20 };
            double[] v1 = { 1, 0, 0 };
            double[] v2 = { 0, 1, 0 };

            //int distance = 750;
            //int direction = 2;

            //double[] point = { 150, 150, 150};
            //double[] v1 = { 1, 0, 0};
            //double[] v2 = { 0, 1, 0 };
            //double[] v3 = { 2, 1, 5 };

            int xRes = 500;
            int yRes = 500;
            double spacing = 0.5;
            string finalFile = GenerateFinalFileName(point, v1, v2, xRes, yRes, spacing);

            Console.WriteLine("Controlling data...");
            if (ControlData(fileName, distance, direction))
            {
                //int[] dimenses = vData.GetMeassures();
                //Console.WriteLine(dimenses[0] + " " + dimenses[1] + " " + dimenses[2]);
                //Console.ReadKey();

                Console.WriteLine("Data passed");
                double[] spacings = { vData.XSpacing, vData.YSpacing, vData.ZSpacing };
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

                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("The distance is higher than the dimension.");
                Console.ReadKey();
            }

        }

        public static void TestData()
        {
            int[] m = vData.Measures;
            Console.WriteLine("x:" + m[0] + " y:" + m[1] + " z:" + m[2]);

            Console.WriteLine("Going through the data.");
            int aaa = 0;
            int bbb = 0;
            int ccc = 0;
            for (int k = 0; k < m[2]; k++)
            {
                for (int i = 0; i < m[0]; i++)
                {
                    for (int j = 0; j < m[1]; j++)
                    {
                        double a = vData.VData[k][i, j];
                        // DEBUG 
                        //double b = vData2.GetValue(i * vData2.GetXSpacing(), j * vData2.GetYSpacing(), k * vData2.GetZSpacing());
                        double b = vData.GetValueMatrixCoordinates(i, j, k);
                        if ((a - b) > 1)
                        {
                            aaa++;
                            if ((a - b) > a * 0.5)
                            {
                                bbb++;
                            }

                            Console.WriteLine("a:" + a + " b:" + b);
                        }
                        if (b == 0 && a != 0)
                        {
                            //Console.WriteLine("x:" + i + " y:" + j + " z:" + k);
                            //Console.WriteLine(a);
                            ccc++;
                        }
                    }
                }
            }
            Console.WriteLine("Count of mistakes ................................." + aaa);
            Console.WriteLine("Count of mistakes (abs < 50% ......................" + bbb);
            Console.WriteLine("Count of mistakes (b==0) .........................." + ccc);
        }

        public static void TestRotation(string fileName1, string fileName2)
        {
            Console.WriteLine("Loading vData1 ({0})", fileName1);
            Data data1 = new Data();
            data1.SetFeatures(fileName1);
            VolumetricData vData1 = new VolumetricData(data1);
            vData1.Read();
            Console.WriteLine("vData1 loaded");

            Console.WriteLine("Loading vData2 ({0})", fileName2);
            Data data2 = new Data();
            data2.SetFeatures(fileName2);
            VolumetricData vData2 = new VolumetricData(data2);
            vData2.Read();
            Console.WriteLine("vData2 loaded");

            //Point3D a = new Point3D(100, 250, 250);
            //Point3D b = new Point3D(100, 250, 250);
            //Console.WriteLine("Value at point {0} is {1}", nameof(a), vData1.GetValue(a));
            //Console.WriteLine("Value at point {0} is {1}", nameof(b), vData1.GetValue(b));


            //Point3D c = new Point3D(100, 100, 40);
            //Console.WriteLine("Value at point {0} is {1}", nameof(c), vData1.GetValue(c));

            Point3D a = null;
            Point3D b = null;

            for (int i = 100; i < 500; i += 100)
            {
                for (int j = 100; j < 500; j += 100)
                {
                    for (int k = 100; k < 500; k += 100)
                    {
                        a = new Point3D(i, j, k);
                        b = new Point3D(i, j, k);
                        Console.WriteLine(RotationComputer.CalculateRotation(vData1, vData2, a, b, 10_000).ToString());
                    }
                }
            }
            Console.ReadLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static void TestDataFeatures(string fileName)
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
            Console.WriteLine("Reading data.");
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

