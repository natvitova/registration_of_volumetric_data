using System;

namespace DataView
{
    /// <summary>
    /// 
    /// </summary>
    class FeatureVector
    {
        private int r;
        private double[] features;
        private Point3D point;

        /// <summary>
        /// 
        /// </summary>
        public FeatureVector()
        {
            new FeatureVector(new Point3D(0, 0, 0), 0, 0, 0, 0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="x3"></param>
        /// <param name="x4"></param>
        /// <param name="x5"></param>
        public FeatureVector(Point3D p, double x1, double x2, double x3, double x4, double x5)
        {
            this.Point = p;
            this.Features = new double[] { x1, x2, x3, x4, x5 };
            this.R = 5;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double Magnitude()
        {
            double sum = 0;
            for (int i = 0; i < this.R; i++)
            {
                sum += this.Features[i] * this.Features[i];
            }

            return Math.Sqrt(sum);
        }

        /// <summary>
        /// Calculates the difference
        /// </summary>
        /// <param name="fv"></param>
        /// <returns>Distance between the two vectors</returns>
        public double DistTo2(FeatureVector fv)
        {
            double sum = 0;
            for (int i = 0; i < R; i++)
            {
                double d = this.Features[i] - fv.Features[i];
                sum += d * d;
            }

            return sum;
        }

        public double[] Features { get => features; set => features = value; }
        internal Point3D Point { get => point; set => point = value; }
        public int R { get => r; set => r = value; }

        public override string ToString()
        {
            string returnS = "";
            for (int i = 0; i < R; i++)
            {
                returnS += Math.Round(Features[i], 2);
                if (i != R - 1) { returnS += ", "; }
            }

            return returnS;
        }
    }
}
