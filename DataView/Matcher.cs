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
        public Match[] Match(FeatureVector[] f1, FeatureVector[] f2, double threshold)
        {
            KDTree tree = new KDTree(f2);
            List<Match> matches = new List<Match>();

            for (int i = 0; i < f1.Length; i++)
            {
                int index = tree.FindNearest(f1[i]);
                if (index < 0)
                {

                }
                else
                {
                    double s = Similarity(f1[i], f2[index]);
                    matches.Add(new Match(f1[i], f2[index], s));
                }
            }
            matches.Sort((x, y) => x.Similarity.CompareTo(y.Similarity));
            int numberOfMatches = (int)(matches.Count / 100.0 * threshold);
            Match[] matchesReturn = new Match[numberOfMatches];
            //for (int i = 0; i < matches.Count; i++)
            //{
            //    Console.WriteLine(matches.ElementAt(i));
            //}
            int j = 0;
            for (int i = matches.Count - 1; i > matches.Count - 1 - numberOfMatches; i--)
            {
                matchesReturn[j] = matches.ElementAt(i);
                //Console.WriteLine(matchesReturn[j]);
                j++;
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
                num += f1.Coordinate(i) * f2.Coordinate(i);
            }

            return num / denom * 100;
        }

    }
}
