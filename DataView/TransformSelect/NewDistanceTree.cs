using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;

namespace DataView
{
    class NewDTNode
    {
        int node;
        NewDTNode close;
        NewDTNode far;
        double threshold;
        List<int> items;

        bool leaf;

        public static Transform3D[] candidates;

        public NewDTNode(int n, List<int> children, int depth)
        {
            if (children.Count<5)
            {
                this.leaf = true;
                this.items = new List<int>();
                items.AddRange(children);
                items.Add(n);
            }
            else
            {
                this.node = n;
                this.leaf = false;

                this.threshold = findMedian(node, children);

                

                //May be a list of candidates that are considered close to the checked one
                List<int> c = new List<int>();

                //May be a list of candidates that are not considered close to the checked one
                List<int> f = new List<int>();

                foreach(int i in children)
                {
                    double distance = candidates[node].SqrtDistanceTo(candidates[i]);
                    if (candidates[node].SqrtDistanceTo(candidates[i]) >= threshold)
                        f.Add(i);
                    else
                        c.Add(i);
                }

                if (c.Count != 0)
                {
                    int cn = c[c.Count - 1];
                    c.RemoveAt(c.Count - 1);
                    this.close = new NewDTNode(cn, c, depth+1);
                }

                if(f.Count != 0)
                {
                    int fn = f[f.Count - 1];
                    f.RemoveAt(f.Count - 1);
                    this.far = new NewDTNode(fn, f, depth + 1);
                }
            }
        }

        public bool contains(int i)
        {
            if (leaf)
                return items.Contains(i);

            if (node == i)
                return true;

            if (close != null)
            {
                if (close.contains(i))
                    return true;
            }

            if (far != null)
            {
                if (far.contains(i))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// This method outputs the median in terms of distance between the candidate transformations
        /// </summary>
        /// <param name="node">Index of a currently checked node in the original array</param>
        /// <param name="children">List of children of the specified node</param>
        /// <returns>Returns the median of distances</returns>
        private double findMedian(int node, List<int> children)
        {            
            double[] distances = new double[children.Count];
            for (int i = 0; i < distances.Length; i++)
                distances[i] = candidates[node].SqrtDistanceTo(candidates[children[i]]);
            quickSelect(distances, 0, distances.Length-1, distances.Length / 2);
            return distances[distances.Length / 2];
        }

        private void quickSelect(double[] d, int l, int r, int s)
        {
            int t = split(d, l, r);
            if (t == s)
                return;
            if (t > s)
                quickSelect(d, l, t - 1, s);
            else
                quickSelect(d, t + 1, r, s);            
        }

        private int split(double[] d, int l, int r)
        {
            double pivot = d[r];
            while (true)
            {
                while ((d[l] < pivot)&&(l<r))
                    l++;
                if (l < r)
                {
                    d[r] = d[l];
                    r--;
                } else break;
                while ((d[r] >= pivot)&&(l<r))
                    r--;
                if (l < r)
                {
                    d[l] = d[r];
                    l++;
                } else break;
            }
            d[l] = pivot;
            return l;
        }

        internal void findAllCloserThan(Transform3D query, double dist, List<int> result)
        {
            double d;

            if (leaf)
            {
                foreach(int i in items)
                {
                    d = query.SqrtDistanceTo(candidates[i]);
                    if (d < dist)
                        result.Add(i);
                }
                return;
            }

            d = query.SqrtDistanceTo(candidates[node]);

            if (d < dist)
                result.Add(node);

            if (d <= (threshold + dist) && close != null)
                close.findAllCloserThan(query, dist, result);

            if (d >= (threshold - dist))
                far.findAllCloserThan(query, dist, result);
        }

        public NewDTNode getClose()
        {
            return close;
        }

        public NewDTNode getFar()
        {
            return far;
        }
        public int getNode()
        {
            return node;
        }
    }

    class NewDistanceTree
    {
        NewDTNode root;



        public NewDistanceTree(Transform3D[] candidates)
        {
            NewDTNode.candidates = candidates;

            List<int> children = new List<int>();
            for (int i = 1; i < candidates.Length; i++)
                children.Add(i);

            root = new NewDTNode(0, children, 0);
        }


        public List<int> findAllCloserThan(Transform3D query, double dist)
        {
            List<int> result = new List<int>();
            root.findAllCloserThan(query, dist, result);
            return result;
        }
    }
}
