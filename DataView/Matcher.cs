﻿using System;
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
                    double s = Similarity(f1[i], f2[index]);
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
                j++;
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

            for (int i = 0; i < f1.R; i++)
            {
                num += f1.Features[i] * f2.Features[i];
            }

            double s = num / denom * 100;
            if (s < 0) { s = 0; }
            return s;
        }

    }
}
