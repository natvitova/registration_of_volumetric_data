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
    class KDTree
    {
        /// <summary>
        /// 
        /// </summary>
        class Node
        {
            public Node left, right;
            public int point;
            public int axis;
        }

        Node root;
        FeatureVector[] fVectors;
        double smDist, smDistSq;
        int nearest;
        FeatureVector searchFeatureVector;
        Random rnd = new Random(0); // seed

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fVectors"></param>
        public KDTree(FeatureVector[] fVectors)
        {
            this.fVectors = fVectors;
            List<int> list = new List<int>();
            for (int i = 0; i < fVectors.Length; i++)
                list.Add(i);
            root = new Node();
            AddChildren(root, list, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="maxDist"></param>
        /// <returns></returns>
        public int FindNearest(FeatureVector p)
        {
            smDist = double.MaxValue;
            smDistSq = double.MaxValue;
            nearest = -1;
            searchFeatureVector = p;
            SearchSubtree(root);

            // debug
            /*int nFull = fullSearch(p);
            double tmd = pnts[nFull].DistanceTo(p);
            if (tmd < maxDist)
            {
                if (nFull != nearest)
                    Console.WriteLine("Error in KDTree");
            }*/
            // end debug
            return nearest;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        private void SearchSubtree(Node n)
        {
            // node point
            FeatureVector np = fVectors[n.point];

            if (searchFeatureVector.Coordinate(n.axis) < np.Coordinate(n.axis))
            {
                double minDist = np.Coordinate(n.axis) - searchFeatureVector.Coordinate(n.axis);
                if (minDist < smDist)
                {
                    double dist = searchFeatureVector.DistTo2(np);
                    if (dist < smDistSq)
                    {
                        smDistSq = dist;
                        smDist = Math.Sqrt(dist);
                        nearest = n.point;
                    }
                }
                if (n.left != null)
                {
                    SearchSubtree(n.left);
                }
                if (n.right != null)
                {
                    if (minDist < smDist)
                            SearchSubtree(n.right);
                }
            }
            else
            {
                double minDist = searchFeatureVector.Coordinate(n.axis) - np.Coordinate(n.axis);
                if (minDist < smDist)
                {
                    double dist = searchFeatureVector.DistTo2(np);
                    if (dist < smDistSq)
                    {
                        smDist = Math.Sqrt(dist);
                        smDistSq = dist;
                        nearest = n.point;
                    }
                }
                if (n.right != null)
                {
                    SearchSubtree(n.right);
                }
                if (n.left != null)
                {
                    if (minDist < smDist)
                            SearchSubtree(n.left);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="list"></param>
        /// <param name="axis"></param>
        private void AddChildren(Node n, List<int> list, int axis)
        {
            n.axis = axis;

            if (list.Count == 1)
            {
                n.point = list[0];
                return;
            }

            int med = Median(list, axis);
            n.point = list[med];

            List<int> left = new List<int>();
            List<int> right = new List<int>();

            for (int i = 0; i < list.Count; i++)
            {
                if (i == med)
                    continue;
                if (fVectors[list[i]].Coordinate(axis) < fVectors[list[med]].Coordinate(axis))
                    left.Add(list[i]);
                else
                    right.Add(list[i]);
            }

            if (left.Count > 0)
            {
                n.left = new Node();
                AddChildren(n.left, left, (axis + 1) % 5);
            }
            if (right.Count > 0)
            {
                n.right = new Node();
                AddChildren(n.right, right, (axis + 1) % 5);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        private int Median(List<int> points, int axis)
        {
            return (FindMedian(points, 0, points.Count, axis));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        private int FindMedian(List<int> points, int min, int max, int axis)
        {
            if (min == max)
                return (min);
            if (min == (max - 1))
            {
                if (fVectors[points[min]].Coordinate(axis) > fVectors[points[max]].Coordinate(axis))
                {
                    int tmp = points[min];
                    points[min] = points[max];
                    points[max] = tmp;
                }
                return (points.Count / 2);
            }
            int pivot = min + rnd.Next(max - min);
            int l = min;
            int r = max - 1;
            while (l < r)
            {
                while (fVectors[points[l]].Coordinate(axis) < fVectors[points[pivot]].Coordinate(axis))
                    l++;
                while (fVectors[points[r]].Coordinate(axis) > fVectors[points[pivot]].Coordinate(axis))
                    r--;
                if (l < r)
                {
                    int tmp = points[l];
                    points[l] = points[r];
                    points[r] = tmp;
                    l++;
                    r--;
                }
            }
            if (l < points.Count / 2)
                return (FindMedian(points, l, max, axis));
            else
                return (FindMedian(points, min, l, axis));
        }

    }
}
