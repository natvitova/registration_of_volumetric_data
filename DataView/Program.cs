using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using System.Diagnostics;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;

using Microsoft.Extensions.Configuration;
using CsvHelper;
using System.Text;
using System.Globalization;

namespace DataView
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        private static IData iDataMicro;
        private static IData iDataMacro;
        private static IConfiguration configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string fileNameMicro = @"P01_a_MikroCT-nejhrubsi_rozliseni_DICOM_liver-1st-important_Macro_pixel-size53.0585um.mhd";
            string fileNameMacro = @"P01_MakroCT_HEAD_5_0_H31S_0004.mhd";
            configuration = new ConfigurationBuilder().AddJsonFile("config.json", optional: true).Build();

            string fileName = @"P01_b_Prase_1_druhe_vys.mhd";
            int[] translation = new int[3];
            translation[0] = 180; //512
            translation[1] = 200; //512
            translation[2] = 220; //636

            double[] ax = new double[3];
            ax[0] = 0;
            ax[1] = 0;
            ax[2] = 1;

            double fi = 0; //degrees

            VolumetricData vDataMicro = new VolumetricData(fileNameMicro);
            VolumetricData vDataMacro = new VolumetricData(fileNameMacro);

            //int maxMicro = vDataMicro.GetMax();
            //int minMicro = vDataMicro.GetMin();
            //int maxMacro = vDataMacro.GetMax();
            //int minMacro = vDataMacro.GetMin();
            //int[] histoMacro = vDataMacro.GetHistogram();
            //WriteCSV(histoMacro, "d:\\macro.csv");
            //int[] histoMicro = vDataMicro.GetHistogram();
            //WriteCSV(histoMicro, "d:\\micro.csv");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double alpha = MainFunctionFakeData(translation, fi, ax, fileName);
            stopwatch.Stop();
            //Console.WriteLine("Alpha: " + alpha);
            Console.WriteLine("Elapsed Time is {0} s", stopwatch.ElapsedMilliseconds / 1000.0);
            //Cut();
            Console.WriteLine("done");
            Console.ReadKey();
        }

        public static void MainFunction(string micro, string macro) // the truly main function
        {
            //----------------------------------------PARAMETERS----------------------------------------------
            int numberOfPointsMicro = 1_000;
            int numberOfPointsMacro = 1_000;
            double threshold = 100; //percentage
            ISampler s = new Sampler(configuration);
            IFeatureComputer fc = new FeatureComputer();
            IMatcher matcher = new Matcher();
            ITransformer transformer = new Transformer3D();

            //----------------------------------------MICRO CT------------------------------------------------
            Console.WriteLine("Reading micro data.");
            VolumetricData vDataMicro = new VolumetricData(micro);
            iDataMicro = vDataMicro;
            Console.WriteLine("Data read succesfully.");
            Console.WriteLine("Sampling.");
            Point3D[] pointsMicro = s.Sample(iDataMicro, numberOfPointsMicro);

            FeatureVector[] featureVectorsMicro = new FeatureVector[pointsMicro.Length];

            Console.WriteLine("Computing feature vectors.");
            for (int i = 0; i < pointsMicro.Length; i++)
            {
                featureVectorsMicro[i] = fc.ComputeFeatureVector(iDataMicro, pointsMicro[i]);
            }

            //----------------------------------------MACRO CT------------------------------------------------
            Console.WriteLine("\nReading macro data.");
            VolumetricData vDataMacro = new VolumetricData(macro);
            iDataMacro = vDataMacro;
            Console.WriteLine("Data read succesfully.");
            Console.WriteLine("Sampling.");
            Point3D[] pointsMacro = s.Sample(iDataMacro, numberOfPointsMacro);

            FeatureVector[] featureVectorsMacro = new FeatureVector[pointsMacro.Length];

            Console.WriteLine("Computing feature vectors.");
            for (int i = 0; i < pointsMacro.Length; i++)
            {
                featureVectorsMacro[i] = fc.ComputeFeatureVector(iDataMacro, pointsMacro[i]);
            }

            //----------------------------------------MATCHES-------------------------------------------------
            Console.WriteLine("\nMatching.");
            Match[] matches = matcher.Match(featureVectorsMicro, featureVectorsMacro, threshold);
            Console.WriteLine("Count of matches: " + matches.Length);

            //------------------------------------GET TRANSFORMATION -----------------------------------------
            Console.WriteLine("Computing transformations.\n");

            List<Transform3D> transformations = new List<Transform3D>();
            for (int i = 0; i < matches.Length; i++)
            {
                transformations.Add(transformer.GetTransformation(matches[i], iDataMicro, iDataMacro));
                //transformations.Add(transformer.GetTransformation(matches[i], vData, vData2, configuration));
            }

            Candidate.initSums(iDataMicro.Measures[0] / iDataMicro.XSpacing, iDataMicro.Measures[1] / iDataMicro.YSpacing, iDataMicro.Measures[2] / iDataMicro.ZSpacing);
            Density d = new Density(); // finder, we need an instance for certain complicated reason
            Transform3D solution = d.Find(transformations.ToArray());
            Console.WriteLine("Solution found.");
            Console.WriteLine(solution);
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
            FileStream fileStream = new FileStream("d:\\b2.txt", FileMode.OpenOrCreate);
            StreamWriter streamWriter = new StreamWriter(fileStream);

            //----------------------------------------PARAMETERS ---------------------------------------------
            double threshold = 100; // percentage
            int numberOfPointsMicro = 1_000;
            int numberOfPointsMacro = 1_000;
            streamWriter.WriteLine("Number of points in micro data: " + numberOfPointsMicro);
            streamWriter.WriteLine("Number of points in macro data: " + numberOfPointsMacro);
            TransData td = SettingFakeData(macro, translation, phi, axis);

            Console.WriteLine("Artificial data created succesfully.");

            //----------------------------------------DATA ---------------------------------------------------
            FeatureComputer fc = new FeatureComputer();
            ISampler s = new Sampler(configuration);

            Console.WriteLine("Sampling.");
            Point3D[] pointsMicro = s.Sample(iDataMicro, numberOfPointsMicro);
            //Point3D[] pointsMacro = s.Sample(iDataMacro, numberOfPointsMacro);
            Point3D[] pointsMacro = td.Sample(pointsMicro);

            FeatureVector[] featureVectorsMicro = new FeatureVector[pointsMicro.Length];
            FeatureVector[] featureVectorsMacro = new FeatureVector[pointsMacro.Length];

            Console.WriteLine("Computing feature vectors.");
            for (int i = 0; i < pointsMicro.Length; i++)
            {
                featureVectorsMicro[i] = fc.ComputeFeatureVector3(iDataMicro, pointsMicro[i]);
            }

            for (int i = 0; i < pointsMacro.Length; i++)
            {
                featureVectorsMacro[i] = fc.ComputeFeatureVector3(iDataMacro, pointsMacro[i]);
            }

            //----------------------------------------MATCHES-------------------------------------------------
            IMatcher matcher = new Matcher();
            Matcher matcherFake = new Matcher();
            Console.WriteLine("\nMatching.");
            Match[] matches = matcher.Match(featureVectorsMicro, featureVectorsMacro, threshold);
            Match[] matchesFake = matcherFake.FakeMatch(featureVectorsMicro, featureVectorsMacro, threshold);

            Console.WriteLine(axis[0] + " " + axis[1] + " " + axis[2] + "; " + phi + " .......................... AXIS & PHI  ..............................");
            streamWriter.WriteLine(axis[0] + " " + axis[1] + " " + axis[2] + "; " + phi + " .......................... AXIS & PHI  ..............................");
            Console.WriteLine("Count of matches: " + matchesFake.Length);
            streamWriter.WriteLine("Count of matches: " + matchesFake.Length);
            streamWriter.WriteLine();

            //------------------------------------GET TRANSFORMATION -----------------------------------------
            ITransformer transformer = new Transformer3D();
            Console.WriteLine("Computing transformations.\n");
            List<Transform3D> transformations = new List<Transform3D>();
            List<Transform3D> transformationsFake = new List<Transform3D>();

            for (int i = 0; i < matchesFake.Length; i++)
            {
                transformations.Add(transformer.GetTransformation(matches[i], iDataMicro, iDataMacro));
                transformationsFake.Add(transformer.GetTransformation(matchesFake[i], iDataMicro, iDataMacro));
            }

            Console.WriteLine("Looking for optimal transformation.\n");
            Candidate.initSums(iDataMicro.Measures[0] / iDataMicro.XSpacing, iDataMicro.Measures[1] / iDataMicro.YSpacing, iDataMicro.Measures[2] / iDataMicro.ZSpacing); // micro
            Density d = new Density(); // finder, we need an instance for certain complicated reason
            Transform3D solution = d.Find(transformations.ToArray());
            Transform3D solutionFake = d.Find(transformationsFake.ToArray());
            double alpha = td.GetAlpha(solution.RotationMatrix);
            double alphaFake = td.GetAlpha(solutionFake.RotationMatrix);

            Console.WriteLine("Fake matcher.");
            Console.WriteLine(solutionFake);
            Console.WriteLine("Solution found.");
            Console.WriteLine(solution);
            Console.WriteLine("Expected rotation and translation.");
            Console.WriteLine(td.RotationM);
            Console.WriteLine(translation[0] * iDataMicro.XSpacing + " " + translation[1] * iDataMicro.YSpacing + " " + translation[2] * iDataMicro.ZSpacing);
            Console.WriteLine();
            Console.WriteLine("Fake alpha: " + alphaFake);
            Console.WriteLine("Alpha: " + alpha);

            double[] computedAxis = td.GetAxis(solution.RotationMatrix);
            double computedPhi = td.GetAngle(solution.RotationMatrix);
            Console.WriteLine(Math.Round(computedAxis[0], 2) + " " + Math.Round(computedAxis[1], 2) + " " + Math.Round(computedAxis[2], 2) + "; " + Math.Round(computedPhi, 2) + " .......................... AXIS & PHI  ..............................");

            streamWriter.WriteLine("Fake matcher:");
            streamWriter.WriteLine(solutionFake);
            streamWriter.WriteLine("Solution:");
            streamWriter.WriteLine(solution);
            streamWriter.WriteLine("Expected rotation and translation:");
            streamWriter.WriteLine(td.RotationM);
            streamWriter.WriteLine(translation[0] * iDataMicro.XSpacing + " " + translation[1] * iDataMicro.YSpacing + " " + translation[2] * iDataMicro.ZSpacing);
            streamWriter.WriteLine("Fake alpha: " + alphaFake);
            streamWriter.WriteLine("Alpha: " + alpha);
            streamWriter.Close();
            fileStream.Close();

            List<double> a1 = new List<double>();
            List<double> a2 = new List<double>();
            List<Test1> testik = new List<Test1>();
            for (int i = 0; i < numberOfPointsMicro; i++)
            {
                a1.Add(td.GetAlpha(transformations[i].RotationMatrix));
                a2.Add(td.GetAlpha(transformationsFake[i].RotationMatrix));
                //testik.Add(new Test1(transformationsFake[i], td.GetAlpha(transformationsFake[i].RotationMatrix)));
            }
            a1.Sort();
            a2.Sort();
            double[] alphas = a1.ToArray();
            double[] alphasFake = a2.ToArray();
            //testik.Sort((x, y) => x.alpha.CompareTo(y.alpha));

            //__________________________________________________________________________TEST_____________________________________________________________
            Chart ch = MakeChart(alphas, alphasFake);
            Form1 formik = new Form1();
            formik.AddChart(ch);
            formik.ShowDialog();

            //FileStream f2 = new FileStream("d:\\testAlpha.txt", FileMode.OpenOrCreate);
            //StreamWriter sw2 = new StreamWriter(f2);

            //sw2.WriteLine("Expected rotation: ");
            //sw2.WriteLine(td.RotationM);
            //sw2.WriteLine();
            //foreach (Test1 t in testik)
            //{
            //    sw2.WriteLine("Alpha: " + t.alpha);
            //    sw2.WriteLine("Rotation: ");
            //    sw2.WriteLine(t.t.RotationMatrix);
            //    sw2.WriteLine();
            //}

            //streamWriter.Close();
            //fileStream.Close();

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

        public static void Cut()
        {
            //TODO change for 3D/2D, make nonspecific

            /*
            int distance = 20;
            int direction = 2;
            int xRes = 500;
            int yRes = 500;
            double spacing = 0.5;
            */

            //int distance = Convert.ToInt32(configuration["Program:CutDistance"]); ;
            //int direction = Convert.ToInt32(configuration["Program:CutDirection"]); ;
            //int xRes = Convert.ToInt32(configuration["Program:CutXRes"]); ;
            //int yRes = Convert.ToInt32(configuration["Program:CutYRes"]); ;
            //double spacing = Convert.ToDouble(configuration["Program:CutSpacing"]); ;

            double[] point = { 0, 0, 300 };
            double[] v1 = { 1, 0, 0 };
            double[] v2 = { 0, 1, 0 };
            double spacing = 0.5;

            // string finalFile = GenerateFinalFileName(point, v1, v2, xRes, yRes, spacing);

            double[] spacings = { iDataMacro.XSpacing, iDataMacro.YSpacing, iDataMacro.ZSpacing };
            int[] dimenses = iDataMacro.Measures;
            int xRes = (int)(dimenses[0] * spacings[0] / spacing);
            int yRes = (int)(dimenses[1] * spacings[1] / spacing);
            int zRes = (int)(dimenses[2] * spacings[2] / spacing);
            string finalFile = "cutTestZspacing05point300.bmp";

            double[] realPoint = new double[3];
            for (int i = 0; i < realPoint.Length; i++)
            {
                realPoint[i] = point[i] * spacings[i];
            }
            Console.WriteLine("Cutting...");
            //int[,] cutMicro = iDataMicro.Cut(realPoint, v1, v2, xRes, yRes, spacing);
            int[,] cutMacro = iDataMacro.Cut(realPoint, v1, v2, yRes, xRes, spacing);


            Console.WriteLine("Cut finished");
            Console.WriteLine("Creating bitmap...");
            PictureMaker pm = new PictureMaker(cutMacro);
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

        public static void WriteCSV(int[] data, string fileName)
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

    }
}

