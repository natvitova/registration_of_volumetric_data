using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;

namespace DataView
{
    class DTNode
    {
        int node;
        DTNode close;
        DTNode far;
        double threshold;
        List<int> items;
        bool leaf;
        public static Candidate[] cands;


        public static int totalClose = 0;
        

        public DTNode(int n, List<int> children, int depth)
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
                    double distance = cands[node].SqrtDistanceTo(cands[i]);
                    if (cands[node].SqrtDistanceTo(cands[i]) >= threshold)
                        f.Add(i);
                    else
                    {
                        c.Add(i);
                        totalClose++;
                    }
                }
                if (c.Count != 0)
                {
                    int cn = c[c.Count - 1];
                    c.RemoveAt(c.Count - 1);
                    this.close = new DTNode(cn, c, depth+1);
                }

                //Pokus o odstraneni chyby - podminka na velikost fn

                if(f.Count != 0)
                {
                    int fn = f[f.Count - 1];
                    f.RemoveAt(f.Count - 1);
                    this.far = new DTNode(fn, f, depth + 1);
                }
            }
        }

        public bool contains(int i)
        {
            if (leaf)
            {
                return items.Contains(i);
            }
            else
            {
                if (node == i)
                    return true;
                if (close != null)
                    if (close.contains(i))
                        return true;
                if (far.contains(i))
                    return true;
                return false;
            }
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
                distances[i] = cands[node].SqrtDistanceTo(cands[children[i]]);
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

        internal void findAllCloserThan(Candidate query, double dist, List<int> result)
        {
            if (leaf)
            {
                foreach(int i in items)
                {
                    double d = query.SqrtDistanceTo(cands[i]);
                    if (d < dist)
                        result.Add(i);
                }
            }
            else
            {
                double d = query.SqrtDistanceTo(cands[node]);
                if (d < dist)
                    result.Add(node);
                if (d <= (threshold + dist))
                    if (close!= null)
                        close.findAllCloserThan(query, dist, result);
                if (d >= (threshold - dist))
                    far.findAllCloserThan(query, dist, result);
            }
        }

        public DTNode getClose()
        {
            return close;
        }

        public DTNode getFar()
        {
            return far;
        }
        public int getNode()
        {
            return node;
        }
    }
    class DistanceTree
    {
        DTNode root;
        Candidate[] cands;
        public DistanceTree(Candidate[] candidates)
        {
            cands = candidates;
            DTNode.cands = candidates;
            List<int> children = new List<int>();
            for (int i = 1; i < candidates.Length; i++)
                children.Add(i);
            root = new DTNode(0, children, 0);



            //Test
            Dictionary<int, List<Transform3D>> dict = new Dictionary<int, List<Transform3D>>();

            double maxDistance = double.MinValue;
            double minDistance = double.MaxValue;

            for (int i = 1; i < candidates.Length; i++)
            {
                double distance = cands[0].SqrtDistanceTo(candidates[i]);
                minDistance = Math.Min(distance, minDistance);
                maxDistance = Math.Max(distance, maxDistance);
            }

            for (int i = 1; i < candidates.Length; i++)
            {
                double currentDistance = candidates[0].SqrtDistanceTo(candidates[i]);

                double percentage = (currentDistance - minDistance) / (maxDistance - minDistance);
                percentage = percentage * 100;
                int percentageIndex = (int)(percentage/5);
                //int percentage = (int)(((currentDistance - minDistance) / ((maxDistance - minDistance)*5)));
                if (dict.ContainsKey(percentageIndex))
                {
                    try
                    {
                        List<Transform3D> transformations = dict[percentageIndex];
                        transformations.Add(candidates[i].toTransform3D());
                        dict[percentageIndex] = transformations;
                    }
                    catch (Exception e)
                    {

                    }
                }
                else
                {
                    List<Transform3D> transformations = new List<Transform3D>
                    {
                        candidates[i].toTransform3D()
                    };
                    dict[percentageIndex] = transformations;
                }
            }

            foreach (int key in dict.Keys)
            {
                Console.WriteLine("This is the percentage " + (key*5).ToString());
                foreach (Transform3D transformation in dict[key])
                {
                    Console.WriteLine(transformation.ToString());
                }
            }
            //END OF A TEST


        }

        public List<int> findAllCloserThan(Candidate query, double dist)
        {
            List<int> result = new List<int>();
            root.findAllCloserThan(query, dist, result);
            return result;
        }

        List<int> findAllCloserThanBF(Candidate query, double dist)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < cands.Length; i++)
                if (query.SqrtDistanceTo(cands[i]) < dist)
                    result.Add(i);
            return result;
        }

        public void selfTest()
        {
            Random rnd = new Random();
            double m1 = 0;
            double m2 = 0;
            for (int i = 0;i<10000;i++)
            {
                int query = rnd.Next(cands.Length);
                double dist = rnd.NextDouble()*10;
                DateTime s = DateTime.Now;
                var r1 = findAllCloserThan(cands[query], dist);
                m1 += (DateTime.Now - s).TotalMilliseconds;
                s = DateTime.Now;
                var r2 = findAllCloserThanBF(cands[query], dist);
                m2 += (DateTime.Now - s).TotalMilliseconds;
                if (r1.Count != r2.Count)
                    throw new Exception("Different number of results");
                foreach (int r in r1)
                    if (!r2.Contains(r))
                        throw new Exception("Result mismatch");
                foreach (int r in r2)
                    if (!r1.Contains(r))
                        throw new Exception("Result mismatch");
            }
            Console.WriteLine("m1: {0}, m2:{1}", m1, m2);
        }
    }
}
