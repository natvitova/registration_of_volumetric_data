using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.Features = new double[] { 0, 0, 0, 0, 0 };
            this.R = 5;
            this.Point = new Point3D(0, 0, 0);
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
                sum += this.Coordinate(i) * this.Coordinate(i);
            }
            return Math.Sqrt(sum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public double DistTo2(FeatureVector p)
        {
            double sum = 0;
            for (int i = 0; i < R; i++)
            {
                double d = this.Features[i] - p.Features[i];
                sum += d * d;
            }
            return sum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public double Coordinate(int axis)
        {
            if (axis == 0)
                return this.Features[0];
            if (axis == 1)
                return this.Features[1];
            if (axis == 2)
                return this.Features[2];
            if (axis == 3)
                return this.Features[3];
            return this.Features[4];
        }

        public double[] Features { get => features; set => features = value; }
        internal Point3D Point { get => point; set => point = value; }
        public int R { get => r; set => r = value; }

        public override string ToString()
        {
            return Point.ToString() + "; " + Math.Round(Features[0], 2) + ", " + Math.Round(Features[1], 2) + ", " + Math.Round(Features[2], 2) + ", " + Math.Round(Features[3], 2) + ", " + Math.Round(Features[4], 2);
        }
    }
}
