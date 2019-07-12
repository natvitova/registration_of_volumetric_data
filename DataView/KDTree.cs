using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    class KDTree
    {
        class Node
        {
            public Node left, right;
            public int point;
            public int axis;
        }

        Node root;

        VertexWithNormal[] pnts;

        public KDTree(Vertex3D[] points, Vertex3D[] normals)
        {
            this.pnts = new VertexWithNormal[points.Length];
            for (int i = 0; i < points.Length; i++)
                pnts[i] = new VertexWithNormal(points[i], normals[i]);
            List<int> list = new List<int>();
            for (int i = 0; i < points.Length; i++)
                list.Add(i);
            root = new Node();
            AddChildren(root, list, 0);
        }

        double smDist, smDistSq, maxDist;
        int nearest;
        VertexWithNormal searchVertex;

        public int FindNearest(VertexWithNormal p, double maxDist)
        {

            smDist = double.MaxValue;
            smDistSq = double.MaxValue;
            nearest = -1;
            this.maxDist = maxDist;
            searchVertex = p;
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
            if (pnts[nearest].DistTo2(searchVertex) > (maxDist * maxDist))
                return -1;
            return nearest;
        }

        private int FullSearch(VertexWithNormal p)
        {
            int result = 0;
            double minDist = p.DistTo2(pnts[0]);
            for (int i = 1; i < pnts.Length; i++)
                if (p.DistTo2(pnts[i]) < minDist)
                {
                    minDist = p.DistTo2(pnts[i]);
                    result = i;
                }
            return result;
        }

        void SearchSubtree(Node n)
        {
            // node point
            VertexWithNormal np = pnts[n.point];

            if (searchVertex.Coordinate(n.axis) < np.Coordinate(n.axis))
            {
                double minDist = np.Coordinate(n.axis) - searchVertex.Coordinate(n.axis);
                if (minDist < smDist)
                {
                    double dist = searchVertex.DistTo2(np);
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
                        if (minDist < maxDist)
                            SearchSubtree(n.right);
                }
            }
            else
            {
                double minDist = searchVertex.Coordinate(n.axis) - np.Coordinate(n.axis);
                if (minDist < smDist)
                {
                    double dist = searchVertex.DistTo2(np);
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
                        if (minDist < maxDist)
                            SearchSubtree(n.left);
                }
            }
        }

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
                if (pnts[list[i]].Coordinate(axis) < pnts[list[med]].Coordinate(axis))
                    left.Add(list[i]);
                else
                    right.Add(list[i]);
            }

            if (left.Count > 0)
            {
                n.left = new Node();
                AddChildren(n.left, left, (axis + 1) % 6);
            }
            if (right.Count > 0)
            {
                n.right = new Node();
                AddChildren(n.right, right, (axis + 1) % 6);
            }
        }

        public int Median(List<int> points, int axis)
        {
            return (FindMedian(points, 0, points.Count, axis));
        }

        Random rnd = new Random(0);

        private int FindMedian(List<int> points, int min, int max, int axis)
        {
            if (min == max)
                return (min);
            if (min == (max - 1))
            {
                if (pnts[points[min]].Coordinate(axis) > pnts[points[max]].Coordinate(axis))
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
                while (pnts[points[l]].Coordinate(axis) < pnts[points[pivot]].Coordinate(axis))
                    l++;
                while (pnts[points[r]].Coordinate(axis) > pnts[points[pivot]].Coordinate(axis))
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
