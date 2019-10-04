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
    class Matcher : IMatcher
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns></returns>
        public Match[] Match(FeatureVector[] f1, FeatureVector[] f2)
        {
            KDTree tree = new KDTree(f1);
            Match[] matches = new Match[f2.Length];

            for (int i = 0; i < f2.Length; i++)
            {
                int index = tree.FindNearest(f2[i], 5);
                matches[i] = new Match(f1[i], f2[index], Similarity(f1[i], f2[index]));
            }
            return matches;
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

            for (int i = 0; i < f1.r; i++)
            {
                num += f1.Coordinate(i) * f2.Coordinate(i);
            }

            return num / denom * 100;
        }

    }
}