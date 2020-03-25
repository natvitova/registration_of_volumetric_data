using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace DataView
{
    /// <summary>
    /// 
    /// </summary>
    class Matcher : IMatcher
    {
        public Match[] Match(FeatureVector[] f1, FeatureVector[] f2, double threshold)
        {
            KDTree tree = new KDTree(f2);
            List<Match> matches = new List<Match>();

            for (int i = 0; i < f1.Length; i++)
            {
                int index = tree.FindNearest(f1[i]);
                if (index >= 0)
                {
                    double s = Similarity(f1[i], f2[index]); //TODO ZMĚNA?
                    matches.Add(new Match(f1[i], f2[index], s));
                }
            }
            matches.Sort((x, y) => x.Similarity.CompareTo(y.Similarity));
            int numberOfMatches = (int)(matches.Count / 100.0 * threshold);
            Match[] matchesReturn = new Match[numberOfMatches];
            int j = 0;
            for (int i = matches.Count - 1; i > matches.Count - 1 - numberOfMatches; i--)
            {
                matchesReturn[j] = matches.ElementAt(i);
                //Console.WriteLine(matchesReturn[j]);
                j++;
            }
            return matchesReturn;
        }

        public Match[] Match2(FeatureVector[] f1, FeatureVector[] f2, double threshold)
        {
            KDTree tree = new KDTree(f2);
            List<Match> matches = new List<Match>();

            for (int i = 0; i < f1.Length; i++)
            {
                int index = tree.FindNearest(f1[i]);
                if (index >= 0)
                {
                    //________________________________________________TEST_______________________________________
                    double[] ax = new double[3];
                    ax[0] = 0;
                    ax[1] = 0;
                    ax[2] = 1;

                    double fi = 12; //degrees
                    fi = Math.PI * fi / 180;

                    Matrix<double> rotationMatrix = Matrix<double>.Build.Dense(3, 3);
                    rotationMatrix[0, 0] = Math.Cos(fi) + ax[0] * ax[0] * (1 - Math.Cos(fi));
                    rotationMatrix[0, 1] = ax[0] * ax[1] * (1 - Math.Cos(fi)) - ax[2] * Math.Sin(fi);
                    rotationMatrix[0, 2] = ax[0] * ax[2] * (1 - Math.Cos(fi)) + ax[1] * Math.Sin(fi);
                    rotationMatrix[1, 0] = ax[0] * ax[1] * (1 - Math.Cos(fi)) + ax[2] * Math.Sin(fi);
                    rotationMatrix[1, 1] = Math.Cos(fi) + ax[1] * ax[1] * (1 - Math.Cos(fi));
                    rotationMatrix[1, 2] = ax[1] * ax[2] * (1 - Math.Cos(fi)) - ax[0] * Math.Sin(fi);
                    rotationMatrix[2, 0] = ax[0] * ax[2] * (1 - Math.Cos(fi)) - ax[1] * Math.Sin(fi);
                    rotationMatrix[2, 1] = ax[1] * ax[2] * (1 - Math.Cos(fi)) + ax[0] * Math.Sin(fi);
                    rotationMatrix[2, 2] = Math.Cos(fi) + ax[2] * ax[2] * (1 - Math.Cos(fi));

                    Point3D p = f1[i].Point.Rotate(rotationMatrix);
                    double s = Math.Abs(f2[index].Point.X - p.X - 84.336) + Math.Abs(f2[index].Point.Y - p.Y - 79.375) + Math.Abs(f2[index].Point.Z - p.Z - 57);

                    matches.Add(new Match(f1[i], f2[index], s));
                }

            }
            matches.Sort((x, y) => x.Similarity.CompareTo(y.Similarity));
            int numberOfMatches = (int)(matches.Count / 100.0 * threshold);
            Match[] matchesReturn = new Match[numberOfMatches];

            //________________________________________________TEST__________________________________________
            for (int i = 0; i < numberOfMatches; i++)
            {
                matchesReturn[i] = matches.ElementAt(i);
            }
            return matchesReturn;
        }

        public Match[] FakeMatch(FeatureVector[] f1, FeatureVector[] f2, double threshold)
        {
            int numberOfMatches = (int)(f1.Length / 100.0 * threshold);
            Match[] matchesReturn = new Match[numberOfMatches];
            for (int i =0; i < numberOfMatches; i++)
            {
                matchesReturn[i] = new Match(f1[i],f2[i],100);
            }
            return matchesReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns></returns>
        public Match[] Match(FeatureVector[] f1, FeatureVector[] f2)
        {
            return Match(f1, f2, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns></returns>
        private double Similarity(FeatureVector f1, FeatureVector f2)
        {
            double num = 0;
            double denom = f1.Magnitude() * f2.Magnitude();
            double cnst = Math.Sqrt(0.2);
            int h = 0;
            int e = 0;

            for (int i = 0; i < f1.R; i++)
            {
                num += f1.Coordinate(i) * f2.Coordinate(i);
            }

            //for (int a = 0; a < 5; a++)
            //{
            //    double dif1 = Math.Abs(f1.Features[a] - cnst);
            //    double dif2 = Math.Abs(f2.Features[a] - cnst);
            //    if (dif1 < 0.02) { h++; }
            //    if (dif2 < 0.02) { h++; }
            //    if (f1.Features[a] == 0) { e++; }
            //    if (f2.Features[a] == 0) { e++; }
            //}

            double s = num / denom * 100 - h*0.1-e*0.1;
            if (s < 0) { s = 0; }
            return s;
        }

    }
}
