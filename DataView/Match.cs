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
    class Match
    {
        private FeatureVector f1;
        private FeatureVector f2;
        private double percentage;

        public Match(FeatureVector f1)
        {
            this.f1 = f1;
            this.f2 = new FeatureVector();
            this.percentage = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="percentage"></param>
        public Match(FeatureVector f1, FeatureVector f2, double percentage)
        {
            this.f1 = f1;
            this.f2 = f2;
            this.percentage = percentage;
        }

        public FeatureVector GetF1()
        {
            return this.f1;
        }

        public FeatureVector GetF2()
        {
            return this.f2;
        }

        public override string ToString()
        {
            return "f1: " + f1.ToString() + " f2: " + f2.ToString();
        }
    }
}
