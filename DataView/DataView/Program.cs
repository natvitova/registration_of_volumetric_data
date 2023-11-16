using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.Extensions.Configuration;
using CsvHelper;
using System.Globalization;
using System.Security.Cryptography;
using System.Threading;

namespace DataView
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        public static string directoryBigger = @"/Users/pepazetek/Desktop/registration_of_volumetric_data/TestData/BiggerRange/";
        public static string directorySmaller = @"/Users/pepazetek/Desktop/registration_of_volumetric_data/TestData/SmallerRange/";

        //Choose one from the above options
        public static string directory = directoryBigger;

        private static IData iDataMicro;
        private static IData iDataMacro;
        private static IConfiguration configuration;

        static void Main(string[] args)
        {
            //string fileNameMicro = directory + @"P01_a_MikroCT-nejhrubsi_rozliseni_DICOM_liver-1st-important_Macro_pixel-size53.0585um.mhd";//1012,1024,1014
            //string fileNameMicro2 = @"P01_c_DICOM-8bit_130926_liver-29-8-12-C_1x.mhd"; //1244,1267,1246
            //string fileNameMacro =  directory + @"P01_MakroCT_HEAD_5_0_H31S_0004.mhd";

            //string fileNameMacro = directory + @"fileMacro.mhd";
            //string fileNameMicro = directory + @"fileMicro.mhd";

            //Paths where the files are located
            string fileNameMacroBiggerRange = directoryBigger + @"ellipsoidMacroBiggerRange.mhd";
            string fileNameMicroBiggerRange = directoryBigger + @"ellipsoidMicroBiggerRange.mhd";

            string fileNameMacroSmallerRange = directorySmaller + @"ellipsoidMacroSmallerRange.mhd";
            string fileNameMicroSmallerRange = directorySmaller + @"ellipsoidMicroSmallerRange.mhd";

            configuration = new ConfigurationBuilder().AddJsonFile("config.json", optional: true).Build();

            MainFunction(fileNameMicroBiggerRange, fileNameMacroBiggerRange, new Point3D[0], new Point3D[0]);

            /*
            string fileName = @"P01_b_Prase_1_druhe_vys.mhd"; //512,512,636
            int[] translation = new int[3];
            translation[0] = 181; //512
            translation[1] = 215; //512
            translation[2] = 242; //636

            double[] ax = new double[3];
            ax[0] = 0;
            ax[1] = 0;
            ax[2] = 1;

            double norm = Math.Sqrt(ax[0] * ax[0] + ax[1] * ax[1] + ax[2] * ax[2]);
            ax[0] /= norm;
            ax[1] /= norm;
            ax[2] /= norm;

            double fi = 32; //degrees

            TestFeatureVector(fileName, 100000, 1000_000, 92);
            */
            //Console.WriteLine("done");
            //Console.ReadKey();

            //VolumetricData vDataMicro = new VolumetricData(fileNameMicro);
            //iDataMicro = vDataMicro;
            //VolumetricData vDataMacro = new VolumetricData(fileNameMacro); //512,512,46
            //iDataMacro = vDataMacro;

            //Random rnd = new Random();
            //for (int i = 0; i < 10; i++)
            //{
            //    int index = rnd.Next(0, 1024);
            //    Cut(index, iDataMicro);
            //}

            //Console.WriteLine("done");
            //Console.ReadKey();

            //int maxMicro = vDataMicro.GetMax();
            //int minMicro = vDataMicro.GetMin();
            //int maxMacro = vDataMacro.GetMax();
            //int minMacro = vDataMacro.GetMin();
            //int[] histoMacro = vDataMacro.GetHistogram();
            //WriteCSV(histoMacro, "d:\\macro.csv");
            //int[] histoMicro = vDataMicro.GetHistogram();
            //WriteCSV(histoMicro, "d:\\micro.csv");

            //DataTable samplesMicro = new DataTable();
            //using (var csvReader = new LumenWorks.Framework.IO.Csv.CsvReader(new StreamReader(System.IO.File.OpenRead(@"P01_a_MikroCT-nejhrubsi_rozliseni_DICOM_liver-1st-important_Macro_pixel-size53.0585um.csv")), true))
            //{
            //    samplesMicro.Load(csvReader);
            //}
            //Point3D[] pointsMicro = new Point3D[samplesMicro.Rows.Count];
            //for (int i = 0; i < samplesMicro.Rows.Count; i++)
            //{
            //    pointsMicro[i] = new Point3D(Convert.ToDouble(samplesMicro.Rows[i][4].ToString().Replace(".", ",")), Convert.ToDouble(samplesMicro.Rows[i][5].ToString().Replace(".", ",")), Convert.ToDouble(samplesMicro.Rows[i][6].ToString().Replace(".", ",")));
            //}

            //DataTable samplesMacro = new DataTable();
            //using (var csvReader = new LumenWorks.Framework.IO.Csv.CsvReader(new StreamReader(System.IO.File.OpenRead(@"P01_MakroCT_HEAD_5_0_H31S_0004.csv")), true))
            //{
            //    samplesMacro.Load(csvReader);
            //}
            //Point3D[] pointsMacro = new Point3D[samplesMacro.Rows.Count];
            //for (int i = 0; i < samplesMacro.Rows.Count; i++)
            //{
            //    pointsMacro[i] = new Point3D(Convert.ToDouble(samplesMacro.Rows[i][4].ToString().Replace(".", ",")), Convert.ToDouble(samplesMacro.Rows[i][5].ToString().Replace(".", ",")), Convert.ToDouble(samplesMacro.Rows[i][6].ToString().Replace(".", ",")));
            //}

            //int[] phis = new int[] { 0,30,60, 90, 120, 180 };
            //Random rnd = new Random();
            //for (int i = 0; i < phis.Length; i++)
            //{
            //    fi =phis[i];// rnd.Next(0, 180);
            //    //translation[0] = rnd.Next(0, 400); //512
            //    //translation[1] = rnd.Next(0, 400); //512
            //    //translation[2] = rnd.Next(0, 500); //636
            //    //ax[0] = rnd.Next(0, 10);
            //    //ax[1] = rnd.Next(0, 10);
            //    //ax[2] = rnd.Next(0, 10);

            //    //norm = Math.Sqrt(ax[0] * ax[0] + ax[1] * ax[1] + ax[2] * ax[2]);
            //    //ax[0] /= norm;
            //    //ax[1] /= norm;
            //    //ax[2] /= norm;
            //    double alpha = MainFunctionFakeData(translation, fi, ax, fileName);
            //}

            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

            //stopwatch.Stop();
            ////Console.WriteLine("Alpha: " + alpha);
            //Console.WriteLine("Elapsed Time is {0} s", stopwatch.ElapsedMilliseconds / 1000.0);
            ////Cut();
            //MainFunction(fileNameMicro, fileNameMacro, pointsMicro, pointsMacro);
            //Console.WriteLine("done");
            //Console.ReadKey();
        }

        public static void MainFunction(string micro, string macro, Point3D[] pointsMicro, Point3D[] pointsMacro) // the truly main function
        {
            //Sets Locale to US
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");


            //----------------------------------------TEST OF DISTRIBUTION CLASS----------------------------------------------
            //VolumetricDataDistribution volumetricDataDistribution = new VolumetricDataDistribution(0.5, 100);
            //volumetricDataDistribution.AddValue(2);
            //volumetricDataDistribution.AddValue(4);
            //volumetricDataDistribution.CreateDistributionArray();
            //volumetricDataDistribution.GetDistributionPercentage(2);
            //----------------------------------------END OF TEST----------------------------------------------


            //----------------------------------------PARAMETERS----------------------------------------------
            int numberOfPointsMicro = 1_000;
            int numberOfPointsMacro = 1_000;
            double threshold = 10; //percentage - top 10% best matches should be kept
            ISampler s = new Sampler(configuration);
            //IFeatureComputer fc = new FeatureComputer();
            //IFeatureComputer fc = new TestFeatureComputer();
            IFeatureComputer fc = new FeatureComputerVectorDeviation();
            IMatcher matcher = new Matcher();

            ITransformer transformer = new Transformer3D();

            //----------------------------------------MICRO CT------------------------------------------------
            Console.WriteLine("Reading micro data.");
            VolumetricData vDataMicro = new VolumetricData(micro);
            iDataMicro = vDataMicro;

            Console.WriteLine("Data read succesfully.");
            Console.WriteLine("Sampling.");
            pointsMicro = s.Sample(iDataMicro, numberOfPointsMicro);

            FeatureVector[] featureVectorsMicro = new FeatureVector[pointsMicro.Length];

            Console.WriteLine("Computing micro feature vectors.");
            for (int i = 0; i < pointsMicro.Length; i++)
                featureVectorsMicro[i] = fc.ComputeFeatureVector(iDataMicro, pointsMicro[i]);

            //----------------------------------------MACRO CT------------------------------------------------
            Console.WriteLine("\nReading macro data.");
            VolumetricData vDataMacro = new VolumetricData(macro);
            iDataMacro = vDataMacro;
            Console.WriteLine("Data read succesfully.");
            Console.WriteLine("Sampling.");
            pointsMacro = s.Sample(iDataMacro, numberOfPointsMacro);

            FeatureVector[] featureVectorsMacro = new FeatureVector[pointsMacro.Length];

            Console.WriteLine("Computing macro feature vectors.");
            for (int i = 0; i < pointsMacro.Length; i++)
                featureVectorsMacro[i] = fc.ComputeFeatureVector(iDataMacro, pointsMacro[i]);


            //----------------------------------------TEST OF TRANSFORMATION METRICS---------------------------

            Random random = new Random(5);

            const int NUMBER_OF_TESTS = 5;

            Candidate.initSums(iDataMicro.Measures[0] / iDataMicro.XSpacing, iDataMicro.Measures[1] / iDataMicro.YSpacing, iDataMicro.Measures[2] / iDataMicro.ZSpacing);


            Console.WriteLine();
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine();

            double difference12 = 0;
            double difference15 = 0;
            double difference16 = 0;
            double difference17 = 0;

            Candidate.initSums(iDataMicro.Measures[0] / iDataMicro.XSpacing, iDataMicro.Measures[1] / iDataMicro.YSpacing, iDataMicro.Measures[2] / iDataMicro.ZSpacing);

            TransformationDistanceFirst transformationDistanceFirst = new TransformationDistanceFirst();
            TransformationDistanceSecond transformationDistanceSecond = new TransformationDistanceSecond();
            TransformationDistanceFive transformationDistanceFive = new TransformationDistanceFive(iDataMicro);
            TransformationDistanceSix transformationDistanceSix = new TransformationDistanceSix(iDataMicro);
            TransformationDistanceSeven transformationDistanceSeven = new TransformationDistanceSeven(iDataMicro);

            for (int i = 0; i < NUMBER_OF_TESTS; i++)
            {
                Matrix<double> rotationMatrix1 = GenerateRandomRotationMatrix(random);
                Matrix<double> rotationMatrix2 = GenerateRandomRotationMatrix(random);


                //Random translation vectors
                Vector<double> translationVector1 = Vector<double>.Build.DenseOfArray(new double[] { random.NextDouble() * 10, random.NextDouble() * 10, random.NextDouble() * 10 });
                Vector<double> translationVector2 = Vector<double>.Build.DenseOfArray(new double[] { random.NextDouble() * 10, random.NextDouble() * 10, random.NextDouble() * 10 });

                //Vector<double> translationVector1 = Vector<double>.Build.Dense(3);
                //Vector<double> translationVector2 = Vector<double>.Build.Dense(3);

                Transform3D transformation1 = new Transform3D(rotationMatrix1, translationVector1);
                Transform3D transformation2 = new Transform3D(rotationMatrix2, translationVector2);

                double distanceFirstMethod = transformationDistanceFirst.GetTransformationsDistance(transformation1, transformation2, iDataMicro);
                Console.WriteLine("This is distance calculated by the first method: " + distanceFirstMethod);

                double distanceSecondMethod = transformationDistanceSecond.GetTransformationsDistance(transformation1, transformation2, iDataMicro);
                Console.WriteLine("This is distance calculated by the second method: " + distanceSecondMethod);

                double distanceFifthMethod = transformationDistanceFive.GetTransformationsDistance(transformation1, transformation2, iDataMicro);
                Console.WriteLine("This is distance calculated by the fifth method: " + distanceFifthMethod);

                double distanceSixthMethod = transformationDistanceSix.GetTransformationsDistance(transformation1, transformation2, iDataMicro);
                Console.WriteLine("This is distance calculated by the sixth method: " + distanceSixthMethod);

                double distanceSeventhMethod = transformationDistanceSeven.GetTransformationsDistance(transformation1, transformation2, iDataMicro);
                Console.WriteLine("This is distance calculated by the seventh method: " + distanceSeventhMethod);

                Candidate candidate1 = new Candidate(transformation1);
                Candidate candidate2 = new Candidate(transformation2);

                double distanceCandidate = candidate1.DistanceTo(candidate2);
                Console.WriteLine("This is distance calculated by the candidate method: " + distanceCandidate);

                difference12 += Math.Abs(distanceFirstMethod - distanceSecondMethod);
                difference15 += Math.Abs(distanceFirstMethod - distanceFifthMethod);
                difference16 += Math.Abs(distanceFirstMethod - distanceSixthMethod);
                difference17 += Math.Abs(distanceFirstMethod - distanceSeventhMethod);


                Console.WriteLine();
                Console.WriteLine("-------------------------------------------------");
                Console.WriteLine();

                //Calculating the ratio to see if the results of "Candidate" method doesn't return results always multiplied by the same factor - it doesn't (factor changes)
                /*
                Console.WriteLine("Ratio elemenetary/candidate: " + (distanceFirstMethod / distanceCandidate));
                Console.WriteLine("Ratio elemenetary/candidate: " + (distanceSecondMethod / distanceCandidate));
                */

            }


            Console.WriteLine("This is the total difference between 1st and 2nd method: " + difference12);
            Console.WriteLine("This is the total differnece between 1st and 5th method: " + difference15);
            Console.WriteLine("This is the total differnece between 1st and 6th method: " + difference16);
            Console.WriteLine("This is the total difference between 1st and 7th method: " + difference17);
            //----------------------------------------END OF TEST----------------------------------------------

            //Test if some of the points even have the possibilty for matching with the correct point

            Console.WriteLine("The number of sample points is the same: " + ((pointsMacro.Length == pointsMicro.Length) ? "True" : "No"));
            
            int similarPoints = 0;
            const int toleranceRadius = 1;
            for(int i = 0; i<pointsMacro.Length; i++)
            {
                for (int j = 0; j < pointsMicro.Length; j++)
                {
                    if (pointsMacro[i].Distance(pointsMicro[j]) < toleranceRadius)
                    {
                       // Console.WriteLine("these points are similar: " )
                        similarPoints++;
                        break;
                    }
                }
            }

            Console.WriteLine("Number of similar points: " + similarPoints);
            Console.WriteLine();
            

            //----------------------------------------MATCHES-------------------------------------------------
            Console.WriteLine("Matching.");

            //temporarily turned off
            Match[] matches = matcher.Match(featureVectorsMicro, featureVectorsMacro, threshold);

            //Forced correct values for matches

            /*
            Match[] matches = new Match[100];
            Random rnd = new Random();

            for (int i = 0; i < matches.Length; i++) {

                double randomCoordinate = rnd.NextDouble() * 5;

                FeatureVector fv1 = new FeatureVector(new Point3D(randomCoordinate, randomCoordinate, randomCoordinate), i, i, i, i, i);
                FeatureVector fv2 = new FeatureVector(new Point3D(randomCoordinate, randomCoordinate, randomCoordinate), i, i, i, i, i);


                /*
                //Adding false matches
                if ((i%8) != 0)
                {
                    fv1 = new FeatureVector(new Point3D(rnd.NextDouble() * iDataMicro.Measures[0], rnd.NextDouble() * iDataMicro.Measures[1], rnd.NextDouble() * iDataMicro.Measures[2]), i, i, i, i, i);
                    fv2 = new FeatureVector(new Point3D(rnd.NextDouble() * iDataMacro.Measures[0], rnd.NextDouble() * iDataMacro.Measures[1], rnd.NextDouble() * iDataMacro.Measures[2]), i, i, i, i, i);
                }
                

                matches[i] = new Match(fv1, fv2, 100);
            }
            

            Console.WriteLine("Data are loaded correctly: " + CheckDataIntegrity(iDataMicro, iDataMacro));
            
            */
            Console.WriteLine("Matches are " + CalculateCorrectMatches(matches, 3) + " % correct");


            //Filtering of matches outside of bounds

            /*
            List<Match> listOfMatches = new List<Match>();
            foreach(Match currentMatch in matches) {
                if (matchInBounds(iDataMicro, iDataMacro, currentMatch.F1.Point) && matchInBounds(iDataMicro, iDataMacro, currentMatch.F2.Point))
                    listOfMatches.Add(currentMatch);
            }

            //matches = new Match[listOfMatches.Count];
            for(int i = 0; i<listOfMatches.Count; i++) {
                matches[i] = listOfMatches[i];
            }
            */

            //------------------------------------GET TRANSFORMATION -----------------------------------------

            Console.WriteLine("Computing transformations.\n");


            List<Transform3D> transformations = new List<Transform3D>();

            

            for (int i = 0; i < matches.Length; i++)
            {

                /*
                //CODE BELLOW IS FOR TESTING PURPOSES ONLY (Tests whether the density search works)

                
                Matrix<double> rotationMatrix = Matrix<double>.Build.Dense(3, 3);
                Vector<double> translationVector = Vector<double>.Build.Dense(3);

                //Try generating a bad transformation
                if(i%7 != 0)
                {
                    for(int j = 0; j<3; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            rotationMatrix[j, k] = random.Next(1000);
                        }
                    }

                    translationVector[0] = random.Next(100);
                    translationVector[1] = random.Next(100);
                    translationVector[2] = random.Next(100);
                }

                else
                {
                    rotationMatrix[0, 0] = 2 + random.Next(-1, 2) * random.NextDouble();
                    rotationMatrix[1, 0] = 3 + random.Next(-1, 2) * random.NextDouble();
                    rotationMatrix[2, 0] = 8 + random.Next(-1, 2) * random.NextDouble();

                    rotationMatrix[0, 1] = 3 + random.Next(-1, 2) * random.NextDouble();
                    rotationMatrix[1, 1] = 6 + random.Next(-1, 2) * random.NextDouble();
                    rotationMatrix[2, 1] = 9 + random.Next(-1, 2) * random.NextDouble();

                    rotationMatrix[0, 2] = 4 + random.Next(-1, 2) * random.NextDouble();
                    rotationMatrix[1, 2] = 7 + random.Next(-1, 2) * random.NextDouble();
                    rotationMatrix[2, 2] = 3 + random.Next(-1, 2) * random.NextDouble();

                    translationVector[0] = 1 + random.Next(-1, 2) * random.NextDouble();
                    translationVector[1] = 2 + random.Next(-1, 2) * random.NextDouble();
                    translationVector[2] = 3 + random.Next(-1, 2) * random.NextDouble();


                    if (i%3 == 0)
                    {
                        translationVector[0] = random.Next();
                        translationVector[1] = random.Next();
                        translationVector[2] = random.Next();
                    }

                }

                transformations.Add(new Transform3D(rotationMatrix, translationVector));
                */

                //END OF A TEST

                //Calculate transformation and if the transformation doesnt exist, it will skip it and print out the error message

                
                try
                {
                    transformations.Add(transformer.GetTransformation(matches[i], iDataMicro, iDataMacro));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                
                
            }

            Candidate.initSums(iDataMicro.Measures[0] / iDataMicro.XSpacing, iDataMicro.Measures[1] / iDataMicro.YSpacing, iDataMicro.Measures[2] / iDataMicro.ZSpacing);

            TestDensityAccurate testDensity = new TestDensityAccurate();
            //Density testDensity = new Density();
            Transform3D t = testDensity.Find(transformations.ToArray());

            /*
            Density d = new Density(); // finder, we need an instance for certain complicated reason
            Transform3D solution = d.Find(transformations.ToArray());
            */
            Console.WriteLine("Solution found.");
            Console.WriteLine(t);
            
            
            //Cut(solution.RotationMatrix, solution.TranslationVector);
        }

        /// <summary>
        /// ONLY FOR TEST PURPOSES
        /// Generates random rotation matrix
        /// </summary>
        /// <param name="random">Instance of class Random</param>
        /// <returns>Returns random rotation matrix</returns>
        public static Matrix<double> GenerateRandomRotationMatrix(Random random)
        {
            double angleX = random.NextDouble() * 2*Math.PI;
            double angleY = random.NextDouble() * 2*Math.PI;
            double angleZ = random.NextDouble() * 2*Math.PI;

            Matrix<double> rotationMatrixX = Matrix<double>.Build.DenseOfArray(new double[,]
            {
                { 1, 0, 0 },
                { 0, Math.Cos(angleX), -Math.Sin(angleX) },
                { 0, Math.Sin(angleX), Math.Cos(angleX) }
            });

            Matrix<double> rotationMatrixY = Matrix<double>.Build.DenseOfArray(new double[,]
            {
                {Math.Cos(angleY), 0, Math.Sin(angleY) },
                { 0, 1, 0 },
                { -Math.Sin(angleY), 0, Math.Cos(angleY) }
            });

            Matrix<double> rotationMatrixZ = Matrix<double>.Build.DenseOfArray(new double[,]
            {
                { Math.Cos(angleZ), -Math.Sin(angleZ), 0 },
                { Math.Sin(angleZ), Math.Cos(angleZ), 0 },
                { 0, 0, 1 }
            });

            return rotationMatrixX * rotationMatrixY * rotationMatrixZ;
        }

        /// <summary>
        /// Checks whether the match is in bounds for both micro and macro data
        /// </summary>
        /// <param name="iMicroData">Reference for micro data</param>
        /// <param name="iMacroData">Reference for macro data</param>
        /// <param name="point">Center point</param>
        /// <returns>Returns true if the points + radius is within bounds for both micro and macro data</returns>
        public static bool matchInBounds(IData iMicroData, IData iMacroData, Point3D point)
        {
            int maxX = (int)(Math.Min((iMicroData.Measures[0] - 1) * iMicroData.XSpacing, (iMacroData.Measures[0] - 1) * iMacroData.XSpacing));
            int maxY = (int)(Math.Min((iMicroData.Measures[1] - 1) * iMicroData.YSpacing, (iMacroData.Measures[1] - 1) * iMacroData.YSpacing));
            int maxZ = (int)(Math.Min((iMicroData.Measures[2] - 1) * iMicroData.ZSpacing, (iMacroData.Measures[2] - 1) * iMacroData.ZSpacing));

            bool returnValue = ((point.X + TestRotationComputer.radius) < maxX) &&
                ((point.Y + TestRotationComputer.radius) < maxY) &&
                ((point.Z + TestRotationComputer.radius) < maxZ);
            return returnValue;
        }


        public static int CalculateSameHeightMatches(Match[] matches, double threshold)
        {
            int correctMatches = 0;

            foreach (Match currentMatch in matches)
            {
                if (currentMatch.F1.Point.Distance(currentMatch.F2.Point) <= threshold)
                    correctMatches++;
            }
            return (int)(((double)correctMatches / (double)matches.Length) * 100);
        }

        /// <summary>
        /// This method calculates the percentage of correct matches
        /// Works only for known test data
        /// </summary>
        /// <param name="matches">Array of matches</param>
        /// <param name="threshold">Tolerance for considering match a correct one</param>
        /// <returns>The percentage of correct matches</returns>
        public static int CalculateCorrectMatches(Match[] matches, double threshold) {
            int correctMatches = 0;

            foreach (Match currentMatch in matches)
            {
                if (currentMatch.F1.Point.Distance(currentMatch.F2.Point) <= threshold)
                    correctMatches++;
                else
                {
                    Console.WriteLine("These points are not considered correctly matched: " + currentMatch.F1.Point + ", " + currentMatch.F2.Point);
                    Console.WriteLine("This is their distance: "  + currentMatch.F1.Point.Distance(currentMatch.F2.Point));
                }
            }
            return (int) (((double)correctMatches / (double) matches.Length) * 100);
        }

        public static bool CheckDataIntegrity(IData iMicroData, IData iMacroData)
        {
            int maxX = (int)(Math.Min((iMicroData.Measures[0] - 1) * iMicroData.XSpacing, (iMacroData.Measures[0] - 1) * iMacroData.XSpacing));
            int maxY = (int)(Math.Min((iMicroData.Measures[1] - 1) * iMicroData.YSpacing, (iMacroData.Measures[1] - 1) * iMacroData.YSpacing));
            int maxZ = (int)(Math.Min((iMicroData.Measures[2] - 1) * iMicroData.ZSpacing, (iMacroData.Measures[2] - 1) * iMacroData.ZSpacing));

            Console.WriteLine("Loop dimensions are [" + maxX + ", " + maxY + ", " + maxZ + "]");
            for(int x = 0; x<maxX; x++)
            {
                for(int y = 0; y<maxY; y++)
                {
                    for(int z = 0; z<maxZ; z++)
                    {
                        if (iMicroData.GetValue(x, y, z) != iMacroData.GetValue(x, y, z))
                            return false;
                    }
                }
            }

            return true;
        }


        public static TransData SettingFakeData(string macro, int[] translation, double phi, double[] axis)
        {
            VolumetricData vDataMacro = new VolumetricData(macro);
            TransData td = new TransData(vDataMacro, translation, phi, axis);
            iDataMicro = td;
            iDataMacro = vDataMacro;

            return td;
        }

        public static double MainFunctionFakeData(int[] translation, double phi, double[] axis, string macro)
        {
            //----------------------------------------PARAMETERS ---------------------------------------------
            double threshold = 100; // percentage
            int numberOfPointsMicro = 10_000;
            int numberOfPointsMacro = 10_000;

            string finalName = "d:\\ALPHAS/output_test" + phi + ".txt";
            FileStream fileStream = new FileStream(finalName, FileMode.OpenOrCreate);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            Stopwatch stopwatchL = new Stopwatch();
            stopwatchL.Start();

            streamWriter.WriteLine("Number of points in micro data: " + numberOfPointsMicro);
            streamWriter.WriteLine("Number of points in macro data: " + numberOfPointsMacro);
            TransData td = SettingFakeData(macro, translation, phi, axis);

            Console.WriteLine("Artificial data created succesfully.");

            //----------------------------------------DATA ---------------------------------------------------
            IFeatureComputer fc = new FeatureComputer();
            ISampler s = new Sampler(configuration);

            Console.WriteLine("Sampling.");
            Point3D[] pointsMicro = s.Sample(iDataMicro, numberOfPointsMicro);
            //Point3D[] pointsMacro = s.Sample(iDataMacro, numberOfPointsMacro);
            Point3D[] pointsMacro = td.SampleShifted(pointsMicro);
            //Point3D[] pointsMacro = td.Sample(pointsMicro);

            FeatureVector[] featureVectorsMicro = new FeatureVector[pointsMicro.Length];
            FeatureVector[] featureVectorsMacro = new FeatureVector[pointsMacro.Length];

            Console.WriteLine("Computing feature vectors.");
            for (int i = 0; i < pointsMicro.Length; i++)
            {
                featureVectorsMicro[i] = fc.ComputeFeatureVector(iDataMicro, pointsMicro[i]);
            }

            for (int i = 0; i < pointsMacro.Length; i++)
            {
                featureVectorsMacro[i] = fc.ComputeFeatureVector(iDataMacro, pointsMacro[i]);
            }

            //----------------------------------------MATCHES-------------------------------------------------
            IMatcher matcher = new Matcher();
            //Matcher matcherFake = new Matcher();
            Console.WriteLine("\nMatching.");
            Match[] matches = matcher.Match(featureVectorsMicro, featureVectorsMacro, threshold);
            //Match[] matches2 = matcher.Match(featureVectorsMicro2, featureVectorsMacro2, threshold);
            //Match[] matches3 = matcher.Match(featureVectorsMicro3, featureVectorsMacro3, threshold);
            //Match[] matchesFake = matcherFake.FakeMatch(featureVectorsMicro, featureVectorsMacro, threshold);

            Console.WriteLine(axis[0] + " " + axis[1] + " " + axis[2] + "; " + phi + " .......................... AXIS & PHI  ..............................");
            streamWriter.WriteLine(axis[0] + " " + axis[1] + " " + axis[2] + "; " + phi + " .......................... AXIS & PHI  ..............................");
            Console.WriteLine("Count of matches: " + matches.Length);
            streamWriter.WriteLine("Count of matches: " + matches.Length);
            streamWriter.WriteLine();

            //------------------------------------GET TRANSFORMATION -----------------------------------------
            ITransformer transformer = new Transformer3D();
            Console.WriteLine("Computing transformations.\n");
            List<Transform3D> transformations = new List<Transform3D>();
            //List<Transform3D> transformations2 = new List<Transform3D>();
            //List<Transform3D> transformations3 = new List<Transform3D>();
            //List<Transform3D> transformationsFake = new List<Transform3D>();

            for (int i = 0; i < matches.Length; i++)
            {
                //transformations.Add(transformer.GetTransformation(matches[i], iDataMicro, iDataMacro));
                transformations.Add(transformer.GetTransformation(matches[i], iDataMicro, iDataMacro));
                //transformations2.Add(transformer.GetTransformation(matches2[i], iDataMicro, iDataMacro));
                //transformations3.Add(transformer.GetTransformation(matches3[i], iDataMicro, iDataMacro));
            }

            Console.WriteLine("Looking for optimal transformation.\n");
            Candidate.initSums(iDataMicro.Measures[0] / iDataMicro.XSpacing, iDataMicro.Measures[1] / iDataMicro.YSpacing, iDataMicro.Measures[2] / iDataMicro.ZSpacing); // micro
            Density d = new Density(); // finder, we need an instance for certain complicated reason
            Transform3D solution = d.Find(transformations.ToArray());
            stopwatchL.Stop();

            double alpha = td.GetAlpha(solution.RotationMatrix);

            Console.WriteLine("Solution found.");
            Console.WriteLine(solution);
            Console.WriteLine("Expected rotation and translation.");
            Console.WriteLine(td.RotationM);
            Console.WriteLine(translation[0] * iDataMicro.XSpacing + " " + translation[1] * iDataMicro.YSpacing + " " + translation[2] * iDataMicro.ZSpacing);
            Console.WriteLine();
            Console.WriteLine("Alpha: " + alpha);

            double[] computedAxis = td.GetAxis(solution.RotationMatrix);
            double computedPhi = td.GetAngle(solution.RotationMatrix);
            Console.WriteLine(Math.Round(computedAxis[0], 2) + " " + Math.Round(computedAxis[1], 2) + " " + Math.Round(computedAxis[2], 2) + "; " + Math.Round(computedPhi, 2) + " .......................... AXIS & PHI  ..............................");

            streamWriter.WriteLine("Solution:");
            streamWriter.WriteLine(solution);
            streamWriter.WriteLine("Expected rotation and translation:");
            streamWriter.WriteLine(td.RotationM);
            streamWriter.WriteLine(translation[0] * iDataMicro.XSpacing + " " + translation[1] * iDataMicro.YSpacing + " " + translation[2] * iDataMicro.ZSpacing);
            streamWriter.WriteLine("Alpha: " + alpha);
            streamWriter.WriteLine(Math.Round(computedAxis[0], 2) + " " + Math.Round(computedAxis[1], 2) + " " + Math.Round(computedAxis[2], 2) + "; " + Math.Round(computedPhi, 2) + " .......................... AXIS & PHI  ..............................");
            streamWriter.WriteLine("Elapsed Time is {0} s", stopwatchL.ElapsedMilliseconds / 1000.0);
            streamWriter.Close();
            fileStream.Close();

            List<double> a1 = new List<double>();
            List<Test> testik = new List<Test>();
            List<Test> testik2 = new List<Test>();
            List<Test> testikF1 = new List<Test>();
            for (int i = 0; i < matches.Length; i++)
            {
                a1.Add(td.GetAlpha(transformations[i].RotationMatrix));
                testik.Add(new Test(transformations[i], td.GetAlpha(transformations[i].RotationMatrix)));
                testik2.Add(new Test(transformations[i], td.GetAlpha(transformations[i].RotationMatrix), matches[i].F1.Point));
                testikF1.Add(new Test(transformations[i], td.GetAlpha(transformations[i].RotationMatrix), matches[i].F1.Point, matches[i].F2.Point, matches[i].Similarity));
            }
            double[] alphasNonSorted = a1.ToArray();
            a1.Sort();
            //a2.Sort();
            double[] alphas = a1.ToArray();
            //double[] alphasFake = a2.ToArray();
            testik.Sort((x, y) => x.alpha.CompareTo(y.alpha));
            testik2.Sort((x, y) => x.alpha.CompareTo(y.alpha));
            testikF1.Sort((x, y) => x.alpha.CompareTo(y.alpha));

            WriteCSVdouble(alphasNonSorted, "d:\\ALPHAS/nonSortedAlphas" + phi + "_001_hFAKE" + ".csv");
            WriteCSVdouble(alphas, "d:\\ALPHAS/alphas" + phi + "_001_hFAKE" + ".csv");

            //__________________________________________________________________________TEST_____________________________________________________________
            //Chart ch = MakeChart(alphas);
            //Form1 formik = new Form1();
            //formik.AddChart(ch);
            //formik.ShowDialog();

            FileStream fs2 = new FileStream("d:\\ALPHAS/testAlpha" + phi + ".txt", FileMode.OpenOrCreate);
            StreamWriter sw2 = new StreamWriter(fs2);
            sw2.WriteLine("Expected rotation: ");
            sw2.WriteLine(td.RotationM);
            sw2.WriteLine();
            foreach (Test t in testik)
            {
                sw2.WriteLine("Alpha: " + t.alpha);
                sw2.WriteLine("Rotation: ");
                sw2.WriteLine(t.t.RotationMatrix);
                sw2.WriteLine();
            }
            sw2.Close();
            fs2.Close();

            FileStream fs4 = new FileStream("d:\\ALPHAS/testMatcherFC2" + phi + ".txt", FileMode.OpenOrCreate);
            StreamWriter sw4 = new StreamWriter(fs4);
            sw4.WriteLine("Alpha: " + alpha);
            foreach (Test t in testikF1)
            {
                sw4.WriteLine("Alpha: " + t.alpha);
                sw4.Write("p micro: " + t.pointMicro.ToString() + ", p macro: " + t.pointMacro.ToString());
                sw4.WriteLine();
                sw4.WriteLine("Similarity: " + t.similarity);
                sw4.WriteLine();
            }
            sw4.Close();
            fs4.Close();
            //______________________________________________________________________END TEST_____________________________________________________________

            return alpha;
        }

        /// <summary>
        /// Test feature vektoru
        /// </summary>
        /// <param name="macro"></param>
        /// <param name="countPoints"></param>
        /// <param name="countSimilarities"></param>
        /// <param name="a"></param>
        public static void TestFeatureVector(string macro, int countPoints, int countSimilarities, int a)
        {
            Random rnd = new Random();
            IFeatureComputer fc = new FeatureComputerNormedRings();
            VolumetricData vDataMacro = new VolumetricData(macro);
            iDataMacro = vDataMacro;
            ISampler s = new Sampler(configuration);

            Point3D[] pointsM = s.Sample(iDataMacro, countPoints);


            double[] distanceFC = new double[countSimilarities];
            double[] distanceReal = new double[countSimilarities];
            Console.WriteLine("Computing similarities");
            int index1 = rnd.Next(0, countPoints);
            int index2 = index1;
            //Point3D p1 = pointsM[index1];
            //List<Point3D> pMacro = GetSphere(p1, 15, countSimilarities);
            //Point3D[] pointsMacro = pMacro.ToArray();
            //double[] point = new double[] { p1.X, p1.Y, p1.Z };

            //FeatureVector f1 = fc.ComputeFeatureVector(iDataMacro, p1);
            for (int i = 0; i < countSimilarities; i++)
            {
                if (i % 10000 == 0)
                {
                    Console.WriteLine("i= " + i);
                }
                Point3D p1 = pointsM[index1];
                Point3D p2 = pointsM[index2];
                FeatureVector f2 = fc.ComputeFeatureVector(iDataMacro, p2);
                FeatureVector f1 = fc.ComputeFeatureVector(iDataMacro, p1);
                // counting similarity
                distanceFC[i] = f1.DistTo2(f2);
                distanceReal[i] = Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y) + (p1.Z - p2.Z) * (p1.Z - p2.Z));
                index2 = rnd.Next(0, countPoints);
                index1 = rnd.Next(0, countPoints);
            }

            //WriteCSVdouble(point, "d:\\FC_test/point" + a + ".csv");
            WriteCSVdouble(distanceFC, "d:\\FC_test/distanceFC" + a + ".csv");
            WriteCSVdouble(distanceReal, "d:\\FC_test/distanceReal" + a + ".csv");
        }

        public static Chart MakeChart(double[] alphas, double[] alphas2)
        {
            Chart chart1 = new Chart();
            chart1.Location = new Point(10, 10);
            chart1.Width = 1200;
            chart1.Height = 600;

            // chartArea
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "First Area";
            chart1.ChartAreas.Add(chartArea);
            chartArea.BackColor = Color.Azure;
            chartArea.BackGradientStyle = GradientStyle.HorizontalCenter;
            chartArea.BackHatchStyle = ChartHatchStyle.LargeGrid;
            chartArea.BorderDashStyle = ChartDashStyle.Solid;
            chartArea.BorderWidth = 1;
            chartArea.BorderColor = Color.Red;
            chartArea.ShadowColor = Color.Purple;
            chartArea.ShadowOffset = 0;
            chart1.ChartAreas[0].Axes[0].MajorGrid.Enabled = false;//x axis
            chart1.ChartAreas[0].Axes[1].MajorGrid.Enabled = false;//y axis

            //Cursor：only apply the top area
            chartArea.CursorX.IsUserEnabled = true;
            chartArea.CursorX.AxisType = AxisType.Primary;//act on primary x axis
            chartArea.CursorX.Interval = 1;
            chartArea.CursorX.LineWidth = 1;
            chartArea.CursorX.LineDashStyle = ChartDashStyle.Dash;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.CursorX.SelectionColor = Color.Yellow;
            chartArea.CursorX.AutoScroll = true;

            chartArea.CursorY.IsUserEnabled = true;
            chartArea.CursorY.AxisType = AxisType.Primary;//act on primary y axis
            chartArea.CursorY.Interval = 1;
            chartArea.CursorY.LineWidth = 1;
            chartArea.CursorY.LineDashStyle = ChartDashStyle.Dash;
            chartArea.CursorY.IsUserSelectionEnabled = true;
            chartArea.CursorY.SelectionColor = Color.Yellow;
            chartArea.CursorY.AutoScroll = true;

            // Axis
            chartArea.AxisY.Minimum = 0d;//Y axis Minimum value
            chartArea.AxisY.Title = @"Percentage of rotations with lower alpha (from 1000)";
            //chartArea.AxisY.Maximum = 100d;//Y axis Maximum value
            chartArea.AxisX.Minimum = 0d; //X axis Minimum value
            chartArea.AxisX.Maximum = 180d;
            chartArea.AxisX.IsLabelAutoFit = true;
            //chartArea.AxisX.LabelAutoFitMaxFontSize = 12;
            chartArea.AxisX.LabelAutoFitMinFontSize = 5;
            chartArea.AxisX.LabelStyle.Angle = -20;
            chartArea.AxisX.LabelStyle.IsEndLabelVisible = true;//show the last label
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chartArea.AxisX.IntervalType = DateTimeIntervalType.NotSet;
            chartArea.AxisX.Title = @"Alpha";
            chartArea.AxisX.TextOrientation = TextOrientation.Auto;
            chartArea.AxisX.LineWidth = 2;
            chartArea.AxisX.LineColor = Color.DarkOrchid;
            chartArea.AxisX.Enabled = AxisEnabled.True;
            chartArea.AxisX.ScaleView.MinSizeType = DateTimeIntervalType.Months;
            chartArea.AxisX.ScrollBar = new AxisScrollBar();

            //Series
            Series series1 = new Series();
            series1.ChartArea = "First Area";
            chart1.Series.Add(series1);
            //Series style
            series1.Name = @"series：Test One";
            series1.ChartType = SeriesChartType.Line;  // type
            series1.BorderWidth = 2;
            series1.Color = Color.Green;
            series1.XValueType = ChartValueType.Int32;//x axis type
            series1.YValueType = ChartValueType.Int32;//y axis type
            // series.YValuesPerPoint = 6;

            //Marker 
            //series1.MarkerStyle = MarkerStyle.Star4;
            //series1.MarkerSize = 10;
            //series1.MarkerStep = 1;
            //series1.MarkerColor = Color.Red;
            //series1.ToolTip = @"ToolTip";

            //Label
            series1.IsValueShownAsLabel = true;
            series1.SmartLabelStyle.Enabled = false;
            series1.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
            series1.LabelForeColor = Color.Gray;
            series1.LabelToolTip = @"LabelToolTip";

            //Empty Point Style 
            DataPointCustomProperties p = new DataPointCustomProperties();
            p.Color = Color.Green;
            series1.EmptyPointStyle = p;

            //Legend
            series1.LegendText = "Normal matcher";
            series1.LegendToolTip = @"LegendToolTip";

            for (int i = 0; i <= 180; i += 1)
            {
                int count = 0;
                for (int j = 0; j < 1000; j++)
                {
                    if (alphas[j] <= i)
                    {
                        count++;
                    }
                }
                double a = count / 10.0;
                series1.Points.AddXY(i, a);
            }



            //Series
            Series series2 = new Series("");
            chart1.Series.Add(series2);
            chart1.Series[1].YAxisType = AxisType.Secondary;//Secondary axis

            series2.Name = @"series：Test Two";
            series2.ChartType = SeriesChartType.Spline;
            series2.BorderWidth = 2;
            series2.Color = Color.Red;
            series2.XValueType = ChartValueType.Int32;
            series2.YValueType = ChartValueType.Int32;

            //Marker
            //series2.MarkerStyle = MarkerStyle.Triangle;
            //series2.MarkerSize = 10;
            //series2.MarkerStep = 1;
            //series2.MarkerColor = Color.Gray;
            //series2.ToolTip = @"ToolTip";

            //Label:
            series2.IsValueShownAsLabel = true;
            series2.SmartLabelStyle.Enabled = false;
            series2.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
            series2.LabelForeColor = Color.Gray;
            series2.LabelToolTip = @"LabelToolTip";

            //Legend
            series2.LegendText = "Fake matcher";
            series2.LegendToolTip = @"LegendToolTip";

            for (int i = 0; i <= 180; i += 1)
            {
                int count = 0;
                for (int j = 0; j < 1000; j++)
                {
                    if (alphas2[j] <= i)
                    {
                        count++;
                    }
                }
                double a = count / 10.0;
                series2.Points.AddXY(i, a);
            }
            return chart1;
        }

        public static Chart MakeChart(double[] alphas)
        {
            Chart chart1 = new Chart();
            chart1.Location = new Point(10, 10);
            chart1.Width = 1200;
            chart1.Height = 600;

            // chartArea
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "First Area";
            chart1.ChartAreas.Add(chartArea);
            chartArea.BackColor = Color.Azure;
            chartArea.BackGradientStyle = GradientStyle.HorizontalCenter;
            chartArea.BackHatchStyle = ChartHatchStyle.LargeGrid;
            chartArea.BorderDashStyle = ChartDashStyle.Solid;
            chartArea.BorderWidth = 1;
            chartArea.BorderColor = Color.Red;
            chartArea.ShadowColor = Color.Purple;
            chartArea.ShadowOffset = 0;
            chart1.ChartAreas[0].Axes[0].MajorGrid.Enabled = false;//x axis
            chart1.ChartAreas[0].Axes[1].MajorGrid.Enabled = false;//y axis

            //Cursor：only apply the top area
            chartArea.CursorX.IsUserEnabled = true;
            chartArea.CursorX.AxisType = AxisType.Primary;//act on primary x axis
            chartArea.CursorX.Interval = 1;
            chartArea.CursorX.LineWidth = 1;
            chartArea.CursorX.LineDashStyle = ChartDashStyle.Dash;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.CursorX.SelectionColor = Color.Yellow;
            chartArea.CursorX.AutoScroll = true;

            chartArea.CursorY.IsUserEnabled = true;
            chartArea.CursorY.AxisType = AxisType.Primary;//act on primary y axis
            chartArea.CursorY.Interval = 1;
            chartArea.CursorY.LineWidth = 1;
            chartArea.CursorY.LineDashStyle = ChartDashStyle.Dash;
            chartArea.CursorY.IsUserSelectionEnabled = true;
            chartArea.CursorY.SelectionColor = Color.Yellow;
            chartArea.CursorY.AutoScroll = true;

            // Axis
            chartArea.AxisY.Minimum = 0d;//Y axis Minimum value
            chartArea.AxisY.Title = @"Percentage of rotations with lower alpha (from 1000)";
            //chartArea.AxisY.Maximum = 100d;//Y axis Maximum value
            chartArea.AxisX.Minimum = 0d; //X axis Minimum value
            chartArea.AxisX.Maximum = 180d;
            chartArea.AxisX.IsLabelAutoFit = true;
            //chartArea.AxisX.LabelAutoFitMaxFontSize = 12;
            chartArea.AxisX.LabelAutoFitMinFontSize = 5;
            chartArea.AxisX.LabelStyle.Angle = -20;
            chartArea.AxisX.LabelStyle.IsEndLabelVisible = true;//show the last label
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chartArea.AxisX.IntervalType = DateTimeIntervalType.NotSet;
            chartArea.AxisX.Title = @"Alpha";
            chartArea.AxisX.TextOrientation = TextOrientation.Auto;
            chartArea.AxisX.LineWidth = 2;
            chartArea.AxisX.LineColor = Color.DarkOrchid;
            chartArea.AxisX.Enabled = AxisEnabled.True;
            chartArea.AxisX.ScaleView.MinSizeType = DateTimeIntervalType.Months;
            chartArea.AxisX.ScrollBar = new AxisScrollBar();

            //Series
            Series series1 = new Series();
            series1.ChartArea = "First Area";
            chart1.Series.Add(series1);
            //Series style
            series1.Name = @"series：Test One";
            series1.ChartType = SeriesChartType.Line;  // type
            series1.BorderWidth = 2;
            series1.Color = Color.Green;
            series1.XValueType = ChartValueType.Int32;//x axis type
            series1.YValueType = ChartValueType.Int32;//y axis type
            // series.YValuesPerPoint = 6;

            //Marker
            //series1.MarkerStyle = MarkerStyle.Star4;
            //series1.MarkerSize = 10;
            //series1.MarkerStep = 1;
            //series1.MarkerColor = Color.Red;
            //series1.ToolTip = @"ToolTip";

            //Label
            series1.IsValueShownAsLabel = true;
            series1.SmartLabelStyle.Enabled = false;
            series1.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
            series1.LabelForeColor = Color.Gray;
            series1.LabelToolTip = @"LabelToolTip";

            //Empty Point Style 
            DataPointCustomProperties p = new DataPointCustomProperties();
            p.Color = Color.Green;
            series1.EmptyPointStyle = p;

            //Legend
            series1.LegendText = "Normal matcher";
            series1.LegendToolTip = @"LegendToolTip";

            for (int i = 0; i <= 180; i += 1)
            {
                int count = 0;
                for (int j = 0; j < 1000; j++)
                {
                    if (alphas[j] <= i)
                    {
                        count++;
                    }
                }
                double a = count / 10.0;
                series1.Points.AddXY(i, a);
            }

            return chart1;
        }

        public static void Cut(int t, IData data)
        {
            double[] point = { 0, t, 0 };
            double[] v1 = { 1, 0, 0 };
            double[] v2 = { 0, 0, 1 };
            double spacing = 0.1;
            // string finalFile = GenerateFinalFileName(point, v1, v2, xRes, yRes, spacing);

            double[] spacings = { data.XSpacing, data.YSpacing, data.ZSpacing };
            int[] dimenses = data.Measures;
            int xRes = (int)(dimenses[0] * spacings[0] / spacing);
            int yRes = (int)(dimenses[1] * spacings[1] / spacing);
            int zRes = (int)(dimenses[2] * spacings[2] / spacing);
            string finalFile = "cutTestYspacing01point" + t + "Micro.bmp";

            double[] realPoint = new double[3];
            for (int i = 0; i < realPoint.Length; i++)
            {
                realPoint[i] = point[i] * spacings[i];
            }

            Console.WriteLine("Cutting...");
            double[,] cut = data.Cut(realPoint, v1, v2, xRes, zRes, spacing);


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

        public static void Cut(Matrix<double> rotation, Vector<double> translation)
        {
            for (int i = 0; i < 10; i++)
            {
                double[] point = { 0, i * 100, 0 };
                double[] v1 = { 1, 0, 0 };
                double[] v2 = { 0, 0, 1 };
                double spacing = 0.1;

                double[] spacings = { iDataMicro.XSpacing, iDataMicro.YSpacing, iDataMicro.ZSpacing };
                int[] dimenses = iDataMicro.Measures;
                int xRes = (int)(dimenses[0] * spacings[0] / spacing);
                int yRes = (int)(dimenses[1] * spacings[1] / spacing);
                int zRes = (int)(dimenses[2] * spacings[2] / spacing);

                string finalFileMicro = "cutTestMicro" + i + ".bmp";
                string finalFileMacro = "cutTestMacro" + i + ".bmp";

                double[] realPoint = new double[3];
                for (int ii = 0; ii < realPoint.Length; ii++)
                {
                    realPoint[ii] = point[ii] * spacings[ii];
                }

                Vector<double> v = Vector<double>.Build.Dense(3);
                v[0] = realPoint[0];
                v[1] = realPoint[1];
                v[2] = realPoint[2];

                Vector<double> u = rotation.Multiply(v);
                u += translation;
                double[] Q = new double[] { u[0], u[1], u[2] };

                Vector<double> vec1 = Vector<double>.Build.Dense(3);
                vec1[0] = v1[0];
                vec1[1] = v1[1];
                vec1[2] = v1[2];

                Vector<double> uvec1 = rotation.Multiply(vec1);
                uvec1 += translation;
                double[] Qv1 = new double[] { uvec1[0], uvec1[1], uvec1[2] };

                Vector<double> vec2 = Vector<double>.Build.Dense(3);
                vec2[0] = v2[0];
                vec2[1] = v2[1];
                vec2[2] = v2[2];

                Vector<double> uvec2 = rotation.Multiply(vec2);
                uvec2 += translation;
                double[] Qv2 = new double[] { uvec2[0], uvec2[1], uvec2[2] };

                Console.WriteLine("Cutting...");
                double[,] cutMacro = iDataMacro.Cut(Q, Qv1, Qv2, xRes, zRes, spacing);
                double[,] cutMicro = iDataMicro.Cut(realPoint, v1, v2, xRes, zRes, spacing);


                Console.WriteLine("Cuts finished");
                Console.WriteLine("Creating macro bitmap...");
                PictureMaker pmMacro = new PictureMaker(cutMacro);
                Bitmap bitmapMacro = pmMacro.MakeBitmap();
                Console.WriteLine("Macro bitmap finished");

                Console.WriteLine("Saving macro bitmap to file...");
                try
                {
                    bitmapMacro.Save(finalFileMacro, System.Drawing.Imaging.ImageFormat.Bmp);
                    Console.WriteLine("Save to bitmap succesful");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Save to bitmap failed");
                    Console.Write(e.Message);
                }

                Console.WriteLine("Creating micro bitmap...");
                PictureMaker pmMicro = new PictureMaker(cutMicro);
                Bitmap bitmapMicro = pmMicro.MakeBitmap();
                Console.WriteLine("Micro bitmap finished");

                Console.WriteLine("Saving micro bitmap to file...");
                try
                {
                    bitmapMicro.Save(finalFileMicro, System.Drawing.Imaging.ImageFormat.Bmp);
                    Console.WriteLine("Save to bitmap succesful");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Save to bitmap failed");
                    Console.Write(e.Message);
                }
            }
        }

        public static void WriteCSV(int[] data, string fileName)
        {
            using (var writer = new StreamWriter(fileName))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(data);
            }
        }

        public static void WriteCSVdouble(double[] data, string fileName)
        {
            using (var writer = new StreamWriter(fileName))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(data);
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

        static List<Point3D> GetSphere(Point3D x, double r, int count)
        {
            List<Point3D> points = new List<Point3D>();

            Random rnd = new Random();
            double rSquared = r * r;
            do
            {
                double xCoordinate = GetRandomDouble(x.X - r, x.X + r, rnd);
                double yCoordinate = GetRandomDouble(x.Y - r, x.Y + r, rnd);
                double zCoordinate = GetRandomDouble(x.Z - r, x.Z + r, rnd);

                double distance = (xCoordinate - x.X) * (xCoordinate - x.X) + (yCoordinate - x.Y) * (yCoordinate - x.Y) + (zCoordinate - x.Z) * (zCoordinate - x.Z);
                if (distance <= rSquared)
                {
                    points.Add(new Point3D(xCoordinate, yCoordinate, zCoordinate));
                }
            } while (
                points.Count < count
            );

            return points;
        }

        static double GetRandomDouble(double minimum, double maximum, Random r)
        {
            return r.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}

