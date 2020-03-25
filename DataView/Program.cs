using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;

namespace DataView
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        private static Data sampleMicro;
        private static IData iDataMicro; //micro
        private static Data sample2;
        private static IData iData2; //macro

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string fileName = @"P01_b_Prase_1_druhe_vys.mhd";
            string fileName2 = fileName;
            int[] translation = new int[3];
            translation[0] = 170; //512
            translation[1] = 160; //512
            translation[2] = 190; //636

            double[] ax = new double[3];
            ax[0] = 0;
            ax[1] = 0;
            ax[2] = 1;

            double fi = 12; //degrees


            //TransData td = FunctionAD(fileName, translation, fi, ax);
            //int x = 30;
            //int y = 40;
            //int z = 10;

            //double[] pole = new double[3];
            //pole[0] = x * iData.XSpacing;
            //pole[1] = y * iData.YSpacing;
            //pole[2] = z * iData.ZSpacing;

            //double val = iData.GetValue(pole[0], pole[1], pole[2]);
            //double val2 = iData2.GetValue(95.5, 101.5, 60);
            //FeatureComputer fc = new FeatureComputer();
            //Point3D p = new Point3D(pole[0], pole[1], pole[2]);
            //Point3D p2 = new Point3D(95.5, 101.5,60);
            //FeatureVector featureVector = fc.ComputeFeatureVector(iData, p);
            //FeatureVector featureVector2 = fc.ComputeFeatureVector(iData2, p2);

            //Console.ReadKey();
            //________________________________________________________________________________________________________TEST START
            //TransData td = FunctionAD(fileName, translation, fi, ax);
            //fi = Math.PI * fi / 180;
            //double[] t2 = new double[3];
            //t2[0] = translation[0] * iData.XSpacing;
            //t2[1] = translation[1] * iData.YSpacing;
            //t2[2] = translation[2] * iData.ZSpacing;

            //Matrix<double> RotationM = Matrix<double>.Build.Dense(3, 3);
            //RotationM[0, 0] = Math.Cos(fi) + ax[0] * ax[0] * (1 - Math.Cos(fi));
            //RotationM[0, 1] = ax[0] * ax[1] * (1 - Math.Cos(fi)) - ax[2] * Math.Sin(fi);
            //RotationM[0, 2] = ax[0] * ax[2] * (1 - Math.Cos(fi)) + ax[1] * Math.Sin(fi);
            //RotationM[1, 0] = ax[0] * ax[1] * (1 - Math.Cos(fi)) + ax[2] * Math.Sin(fi);
            //RotationM[1, 1] = Math.Cos(fi) + ax[1] * ax[1] * (1 - Math.Cos(fi));
            //RotationM[1, 2] = ax[1] * ax[2] * (1 - Math.Cos(fi)) - ax[0] * Math.Sin(fi);
            //RotationM[2, 0] = ax[0] * ax[2] * (1 - Math.Cos(fi)) - ax[1] * Math.Sin(fi);
            //RotationM[2, 1] = ax[1] * ax[2] * (1 - Math.Cos(fi)) + ax[0] * Math.Sin(fi);
            //RotationM[2, 2] = Math.Cos(fi) + ax[2] * ax[2] * (1 - Math.Cos(fi));

            //Random r = new Random(0);

            //for (int i = 0; i < 10; i++)
            //{
            //    Point3D pMacro = new Point3D(r.Next(translation[0], iData2.Measures[0]), r.Next(translation[1], iData2.Measures[1]), r.Next(translation[2], iData2.Measures[2]-1));
            //    pMacro.X *= iData.XSpacing;
            //    pMacro.Y *= iData.YSpacing;
            //    pMacro.Z *= iData.ZSpacing;

            //    Console.WriteLine("macro point: " + pMacro);
            //    Point3D pMicro = new Point3D(0,0,0);
            //    double b = iData2.GetValue(pMacro);
            //    FeatureComputer fc = new FeatureComputer();

            //    Vector<double> v = Vector<double>.Build.Dense(3);
            //    v[0] = pMacro.X- t2[0];
            //    v[1] = pMacro.Y- t2[1];
            //    v[2] = pMacro.Z- t2[2];

            //    Matrix<double> rm = RotationM.Inverse();
            //    Vector<double> u = rm.Multiply(v);
            //    pMicro.X = u[0] ;
            //    pMicro.Y = u[1] ;
            //    pMicro.Z = u[2] ;
            //    Console.WriteLine("micro point: " + pMicro);
            //    //Console.WriteLine("matrix original: " + RotationM);
            //   // Console.WriteLine("matrix inverse: " + rm);
            //    double a = iData.GetValue(pMicro);
            //    Console.WriteLine("macro: " + b + "; micro: " + a);
            //    FeatureVector fMicro = fc.ComputeFeatureVector(iData, pMicro);
            //    FeatureVector fMacro = fc.ComputeFeatureVector(iData2, pMacro);
            //    Console.WriteLine("macro f vector: " + fMacro);
            //    Console.WriteLine("micro f vector: " + fMicro);
            //    Console.WriteLine();
            //}
            //Console.ReadKey();
            //________________________________________________________________________________________________________TEST END

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double alpha = MainFunctionAD3(translation, fi, ax, fileName);
            stopwatch.Stop();
            Console.WriteLine("Alpha: " + alpha);
            Console.WriteLine("Elapsed Time is {0} s", stopwatch.ElapsedMilliseconds / 1000.0);
            Console.ReadKey();

            //string fileName = @"P01_a_MikroCT-nejhrubsi_rozliseni_DICOM_liver-1st-important_Macro_pixel-size53.0585um.mhd";
            //string fileName2 = @"P01_b_Prase_1_druhe_vys.mhd";
        }

        public static void MainFunction(string micro, string macro)
        {
            //----------------------------------------PARAMETERS----------------------------------------------
            int numberOfPointsMicro = 1_000;
            int numberOfPointsMacro = 1_000;
            IFeatureComputer fc = new FeatureComputer();
            ISampler s = new Sampler();
            //----------------------------------------MICRO CT------------------------------------------------
            sampleMicro = new Data();
            sampleMicro.SetFeatures(micro);
            VolumetricData vDataMicro = new VolumetricData(sampleMicro);
            Console.WriteLine("Reading micro data.");
            vDataMicro.Read();
            iDataMicro = vDataMicro;
            Console.WriteLine("Data read succesfully.");
            Console.WriteLine("Sampling.");
            Point3D[] pointsMicro = s.Sample(iDataMicro, numberOfPointsMicro);

            FeatureVector[] featureVectorsMicro = new FeatureVector[pointsMicro.Length];

            Console.WriteLine("Computing feature vectors.");
            for (int i = 0; i < pointsMicro.Length; i++)
            {
                featureVectorsMicro[i] = fc.ComputeFeatureVector(iDataMicro, pointsMicro[i]);
                //Console.WriteLine("fv1:" + i + " " + featureVectors[i].ToString());
            }

            //----------------------------------------MACRO CT------------------------------------------------
            sample2 = new Data();
            sample2.SetFeatures(macro);
            VolumetricData vData2 = new VolumetricData(sample2);
            Console.WriteLine("\nReading second data.");
            vData2.Read();
            iData2 = vData2;
            Console.WriteLine("Data read succesfully.");
            IFeatureComputer fc2 = new FeatureComputer();
            ISampler s2 = new Sampler();

            Console.WriteLine("Sampling.");
            Point3D[] points2 = s2.Sample(iData2, 10000);

            FeatureVector[] featureVectors2 = new FeatureVector[points2.Length];

            Console.WriteLine("Computing feature vectors.");
            for (int i = 0; i < points2.Length; i++)
            {
                featureVectors2[i] = fc2.ComputeFeatureVector(iData2, points2[i]);
                //Console.WriteLine("fv2:" + i + " " + featureVectors2[i].ToString());
            }

            //----------------------------------------MATCHES-------------------------------------------------
            IMatcher matcher = new Matcher();
            Console.WriteLine("\nMatching.");
            Match[] matches = matcher.Match(featureVectorsMicro, featureVectors2);

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
                    transformations.Add(transformer.GetTransformation(matches[i], iDataMicro, iData2));
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

            Candidate.initSums(iDataMicro.Measures[0] / iDataMicro.XSpacing, iDataMicro.Measures[1] / iDataMicro.YSpacing, iDataMicro.Measures[2] / iDataMicro.ZSpacing);
            Density d = new Density(); // finder, we need an instance for certain complicated reason
            Transform3D solution = d.Find(transformations.ToArray());
            Console.WriteLine(solution);
            Console.WriteLine("Solution found.");
            Console.ReadKey();
        }

        public static TransData FunctionAD(string macro, int[] translation, double phi, double[] axis)
        {
            sampleMicro = new Data();
            sampleMicro.SetFeatures(macro);
            VolumetricData vData2 = new VolumetricData(sampleMicro); //macro
            //Console.WriteLine("Reading first data.");
            vData2.Read();
            TransData td = new TransData(vData2, translation, phi, axis);
            iDataMicro = td; // micro
            iData2 = vData2; // macro
            //Console.WriteLine("Data read succesfully.");
            return td;
        }
        public static double MainFunctionAD3(int[] translation, double phi, double[] axis, string macro)
        {
            FileStream f = new FileStream("d:\\b2.txt", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(f);
            //----------------------------------------PARAMS -------------------------------------------------
            double threshold = 100; // percentage
            int numberOfPoints = 1_000; // micro
            int numberOfPoints2 = 1_000;
            sw.WriteLine("Number of points in micro data: " + numberOfPoints);
            sw.WriteLine("Number of points in macro data: " + numberOfPoints2);
            TransData td = FunctionAD(macro, translation, phi, axis);

            Console.WriteLine("Artificial data created succesfully.");
            //----------------------------------------DATA ---------------------------------------------------
            FeatureComputer fc = new FeatureComputer();
            ISampler s = new Sampler();
            FeatureComputer fc2 = new FeatureComputer();

            Console.WriteLine("Sampling.");
            Point3D[] points = s.Sample(iDataMicro, numberOfPoints);
            //Point3D[] points2 = s.Sample(iData2, numberOfPoints2); // macro
            Point3D[] points2 = td.Sample(points);

            FeatureVector[] featureVectors = new FeatureVector[points.Length];
            FeatureVector[] featureVectors2 = new FeatureVector[points2.Length]; // macro

            Console.WriteLine("Computing feature vectors.");
            for (int i = 0; i < points.Length; i++)
            {
                FeatureVector ff = fc.ComputeFeatureVector3(iDataMicro, points[i]);
                featureVectors[i] = ff;
            }

            for (int i = 0; i < points2.Length; i++)
            {
                FeatureVector ff = fc2.ComputeFeatureVector3(iData2, points2[i]);
                featureVectors2[i] = ff;
            }

            //----------------------------------------MATCHES-------------------------------------------------
            IMatcher matcher = new Matcher();
            Matcher matcher2 = new Matcher();
            Console.WriteLine("\nMatching.");
            Match[] matches = matcher.Match(featureVectors, featureVectors2, threshold); //normal
            Match[] matches2 = matcher2.FakeMatch(featureVectors, featureVectors2, threshold); //fake

            Console.WriteLine(translation[0] * iDataMicro.XSpacing + " " + translation[1] * iDataMicro.YSpacing + " " + translation[2] * iDataMicro.ZSpacing + " .......................... TRANSLATION ..............................");
            sw.WriteLine(translation[0] * iDataMicro.XSpacing + " " + translation[1] * iDataMicro.YSpacing + " " + translation[2] * iDataMicro.ZSpacing + " .......................... TRANSLATION ..............................");
            Console.WriteLine(axis[0] + " " + axis[1] + " " + axis[2] + "; " + phi + " .......................... AXIS & PHI  ..............................");
            sw.WriteLine(axis[0] + " " + axis[1] + " " + axis[2] + "; " + phi + " .......................... AXIS & PHI  ..............................");
            Console.WriteLine("Count of matches: " + matches.Length);
            sw.WriteLine("Count of matches: " + matches.Length);
            sw.WriteLine();

            //------------------------------------GET TRANSFORMATION -----------------------------------------
            Transformer3D transformer = new Transformer3D();
            Console.WriteLine("Computing transformations.\n");
            List<Transform3D> transformations = new List<Transform3D>();
            List<Transform3D> transformations2 = new List<Transform3D>();

            for (int i = 0; i < matches.Length; i++)
            {
                Transform3D t = transformer.GetTransformation(matches[i], iDataMicro, iData2);
                Transform3D t2 = transformer.GetTransformation(matches2[i], iDataMicro, iData2);
                transformations.Add(t);
                transformations2.Add(t2);
            }

            Console.WriteLine("Looking for optimal transformation.\n");
            Candidate.initSums(iDataMicro.Measures[0] / iDataMicro.XSpacing, iDataMicro.Measures[1] / iDataMicro.YSpacing, iDataMicro.Measures[2] / iDataMicro.ZSpacing); // micro
            Density d = new Density(); // finder, we need an instance for certain complicated reason
            Transform3D solution = d.Find(transformations.ToArray());
            Transform3D solution2 = d.Find(transformations2.ToArray());

            Console.WriteLine("Fake matcher.");
            Console.WriteLine(solution2);

            Console.WriteLine("Solution found.");
            Console.WriteLine(solution);

            Console.WriteLine("Expected rotation and translation");
            Console.WriteLine(td.RotationM);

            sw.WriteLine("Fake matcher:");
            sw.WriteLine(solution2);
            sw.WriteLine("Solution:");
            sw.WriteLine(solution);

            sw.WriteLine("Expected rotation and translation:");
            sw.WriteLine(td.RotationM);
            double alpha = td.GetAlpha(solution.RotationMatrix);
            double alpha2 = td.GetAlpha(solution2.RotationMatrix);
            sw.WriteLine("Fake alpha: " + alpha2);
            sw.WriteLine("Alpha: " + alpha);
            sw.Close();
            f.Close();

            double[] alphas = new double[numberOfPoints];
            double[] alphas2 = new double[numberOfPoints];
            List<double> a1 = new List<double>();
            List<double> a2 = new List<double>();
            List<Test1> testik = new List<Test1>();
            for (int i = 0; i < numberOfPoints; i++)
            {
                a1.Add(td.GetAlpha(transformations[i].RotationMatrix));
                a2.Add(td.GetAlpha(transformations2[i].RotationMatrix));
                testik.Add(new Test1(transformations2[i], td.GetAlpha(transformations2[i].RotationMatrix)));
            }
            a1.Sort();
            a2.Sort();
            alphas = a1.ToArray();
            alphas2 = a2.ToArray();
            testik.Sort((x, y) => x.alpha.CompareTo(y.alpha));
            //__________________________________________________________________________TEST_____________________________________________________________
            Chart ch = MakeChart(alphas,alphas2);
            Form1 formik = new Form1();
            formik.AddChart(ch);
            formik.ShowDialog();

            return alpha;
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

            for (int i = 0; i < 180; i += 1)
            {
                int count = 0;
                for (int j = 0; j < 1000; j++)
                {
                    if (alphas[j] < i)
                    {
                        count++;
                    }
                }
                double a = count / 10;
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

            for (int i = 0; i < 180; i += 1)
            {
                int count = 0;
                for (int j = 0; j < 1000; j++)
                {
                    if (alphas2[j] < i)
                    {
                        count++;
                    }
                }
                double a = count / 10;
                series2.Points.AddXY(i, a);
            }
            return chart1;
        }


        public static Transform3D MainFunctionAD2(int[] translation, string macro)
        {
            //----------------------------------------PARAMS -------------------------------------------------
            double threshold = 20; // percentage
            int numberOfPoints = 100_000; // micro
            int numberOfPoints2 = 100_000;
            int radius = 10;

            //FunctionAD(macro);
            //iData = iData2.CutVol(translation); //micro

            //Console.WriteLine("Artificial data created succesfully.");
            //----------------------------------------DATA ---------------------------------------------------
            FeatureComputer fc = new FeatureComputer();
            ISampler s = new Sampler();
            FeatureComputer fc2 = new FeatureComputer();

            SamplerFake s2 = new SamplerFake();
            SamplerHalfFake s3 = new SamplerHalfFake();
            SamplerRandomFake s4 = new SamplerRandomFake();
            s2.SetTranslation(translation);
            s3.SetTranslation(translation);
            s4.SetTranslation(translation);

            //Console.WriteLine("Sampling.");
            //Point3D[] points = s.Sample(vData, numberOfPoints);
            //Point3D[] points2 = s.Sample(vData2, numberOfPoints2); // macro
            Point3D[] points2 = s4.Sample(iData2, numberOfPoints2, radius); // macro
            Point3D[] points = s4.PointsMin;

            FeatureVector[] featureVectors = new FeatureVector[points.Length];
            FeatureVector[] featureVectors2 = new FeatureVector[points2.Length];

            //Console.WriteLine("Computing feature vectors.");
            for (int i = 0; i < points.Length; i++)
            {
                featureVectors[i] = fc.ComputeFeatureVector(iDataMicro, points[i]);
                //Console.WriteLine("fv1:" + i + " " + featureVectors[i].ToString());
            }

            for (int i = 0; i < points2.Length; i++)
            {
                featureVectors2[i] = fc2.ComputeFeatureVector(iData2, points2[i]);
                //Console.WriteLine("fv2:" + i + " " + featureVectors2[i].ToString());
            }

            //----------------------------------------MATCHES-------------------------------------------------
            IMatcher matcher = new Matcher();
            //Console.WriteLine("\nMatching.");
            Match[] matches = matcher.Match(featureVectors, featureVectors2, threshold);

            //Console.WriteLine(translation[0] + " " + translation[1] + " " + translation[2] + " .......................... TRANSFORMATION ..............................");

            //------------------------------------GET TRANSFORMATION -----------------------------------------
            Transformer3D transformer = new Transformer3D();
            //Console.WriteLine("Computing transformations.\n");
            //Transform3D[] transformations = new Transform3D[matches.Length];

            List<Transform3D> transformations = new List<Transform3D>();
            //int countT9 = 0;

            for (int i = 0; i < matches.Length; i++)
            {
                Transform3D t = transformer.GetTransformation(matches[i], iDataMicro, iData2);
                transformations.Add(t);
                //Console.WriteLine(t);
            }

            //Console.WriteLine("Looking for optimal transformation.\n");
            Candidate.initSums(iDataMicro.Measures[0] / iDataMicro.XSpacing, iDataMicro.Measures[1] / iDataMicro.YSpacing, iDataMicro.Measures[2] / iDataMicro.ZSpacing); // micro
            Density d = new Density(); // finder, we need an instance for certain complicated reason
            Transform3D solution = d.Find(transformations.ToArray());
            //Console.WriteLine(solution);
            //Console.WriteLine("Solution found.");
            //Console.ReadKey();
            return solution;
        }

        public static Transform3D MainFunctionAD(int[] translation)
        {
            //----------------------------------------PARAMS -------------------------------------------------
            double threshold = 20; // percentage
            int numberOfPoints = 10_000; // micro
            int numberOfPoints2 = numberOfPoints;
            int radius = 10;

            IFunction fce1 = new LinearFunction(1, 3, 8);
            IFunction fce2 = new NonLinearFunction(1, 3, 11);
            ArtificialData aData = new ArtificialData(fce2);

            aData.SetSmallerData(translation);
            iDataMicro = aData.VD2; // micro
            iData2 = aData.VD;
            //Console.WriteLine("Artificial data created succesfully.");

            //----------------------------------------DATA ---------------------------------------------------
            FeatureComputer fc = new FeatureComputer();
            ISampler s = new Sampler();
            FeatureComputer fc2 = new FeatureComputer();
            //ISampler s2 = new SamplerFake();
            SamplerFake s2 = new SamplerFake();
            SamplerHalfFake s3 = new SamplerHalfFake();
            s2.SetTranslation(translation);

            //Console.WriteLine("Sampling.");
            //Point3D[] points = s.Sample(vData, numberOfPoints);
            //Point3D[] points2 = s2.Sample(vData2, numberOfPoints2, radius); // macro
            //Point3D[] points = s2.PointsMin;
            Point3D[] points2 = s2.Sample(iData2, numberOfPoints2, radius); // macro
            Point3D[] points = s2.PointsMin;

            FeatureVector[] featureVectors = new FeatureVector[points.Length];
            FeatureVector[] featureVectors2 = new FeatureVector[points2.Length];

            //Console.WriteLine("Computing feature vectors.");
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
            //Console.WriteLine("\nMatching.");
            Match[] matches = matcher.Match(featureVectors, featureVectors2, threshold);

            //Console.WriteLine(translation[0] + " " + translation[1] + " " + translation[2] + " .......................... TRANSFORMATION ..............................");

            //------------------------------------GET TRANSFORMATION -----------------------------------------
            Transformer3D transformer = new Transformer3D();
            //Console.WriteLine("Computing transformations.\n");
            //Transform3D[] transformations = new Transform3D[matches.Length];

            List<Transform3D> transformations = new List<Transform3D>();
            //int countT9 = 0;

            for (int i = 0; i < matches.Length; i++)
            {
                Transform3D t = transformer.GetTransformationA(matches[i], aData, translation, tr2);
                transformations.Add(t);
                //Console.WriteLine(t);
            }

            //Console.WriteLine("Looking for optimal transformation.\n");
            Candidate.initSums(iDataMicro.Measures[0] / iDataMicro.XSpacing, iDataMicro.Measures[1] / iDataMicro.YSpacing, iDataMicro.Measures[2] / iDataMicro.ZSpacing); // micro
            Density d = new Density(); // finder, we need an instance for certain complicated reason
            Transform3D solution = d.Find(transformations.ToArray());
            //Console.WriteLine(solution);
            //Console.WriteLine("Solution found.");
            //Console.ReadKey();
            return solution;
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
                double[] spacings = { iDataMicro.XSpacing, iDataMicro.YSpacing, iDataMicro.ZSpacing };
                double[] realPoint = new double[3];
                for (int i = 0; i < realPoint.Length; i++)
                {
                    realPoint[i] = point[i] * spacings[i];
                }
                Console.WriteLine("Cutting...");
                //int[,] cut = iData.Cut(realPoint, v1, v2, xRes, yRes, spacing);

                Console.WriteLine("Cut finished");
                Console.WriteLine("Creating bitmap...");
                //PictureMaker pm = new PictureMaker(cut);
                //Bitmap bitmap = pm.MakeBitmap();
                Console.WriteLine("Bitmap finished");

                Console.WriteLine("Saving bitmap to file...");
                try
                {
                    //bitmap.Save(finalFile, System.Drawing.Imaging.ImageFormat.Bmp);
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
            int[] m = iDataMicro.Measures;
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
                        double a = 1; //wtf
                        //double a = iData.VData[k][i, j];
                        // DEBUG 
                        //double b = vData2.GetValue(i * vData2.GetXSpacing(), j * vData2.GetYSpacing(), k * vData2.GetZSpacing());
                        double b = iDataMicro.GetValue(i, j, k);
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
            sampleMicro = new Data();
            sampleMicro.SetFeatures(fileName);
            VolumetricData vData = new VolumetricData(sampleMicro);
            Console.WriteLine("Reading data.");
            vData.Read();
            iDataMicro = vData;

            if (distance < sampleMicro.DimSize[direction])
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

