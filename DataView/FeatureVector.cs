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
        public int r;
        private double[] features;

        /// <summary>
        /// 
        /// </summary>
        public FeatureVector()
        {
            this.features = new double[5];
            this.r = 5;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="x3"></param>
        /// <param name="x4"></param>
        /// <param name="x5"></param>
        public FeatureVector(double x1, double x2, double x3, double x4, double x5)
        {
            this.features = new double[] { x1, x2, x3, x4, x5};
            this.r = 5;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double Magnitude()
        {
            double sum = 0;
            for(int i = 0; i < this.r; i++)
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
            for(int i = 0; i < r; i++)
            {
                double d = this.features[i] - p.features[i];
                sum += d*d;
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
                return this.features[0];
            if (axis == 1)
                return this.features[1];
            if (axis == 2)
                return this.features[2];
            if (axis == 3)
                return this.features[3];
            return this.features[4];
        }
    }
}
