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
    class FeatureComputer : IFeatureComputer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="d"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static List<Point3D> GetSphere(Point3D x, VolumetricData d, double r)
        {
            List<Point3D> points = new List<Point3D>();
            double step = r / 5;

            //Point3D A = new Point3D(Math.Floor((x.x - r) / d.GetXSpacing()), Math.Ceiling((x.y + r) / d.GetYSpacing()), Math.Floor((x.z - r) / d.GetZSpacing()));
            //Point3D B = new Point3D(Math.Ceiling((x.x + r) / d.GetXSpacing()), Math.Ceiling((x.y + r) / d.GetYSpacing()), Math.Floor((x.z - r) / d.GetZSpacing()));
            //Point3D C = new Point3D(Math.Ceiling((x.x + r) / d.GetXSpacing()), Math.Floor((x.y - r) / d.GetYSpacing()), Math.Floor((x.z - r) / d.GetZSpacing()));
            //Point3D D = new Point3D(Math.Floor((x.x - r) / d.GetXSpacing()), Math.Floor((x.y - r) / d.GetYSpacing()), Math.Floor((x.z - r) / d.GetZSpacing()));
            //Point3D E = new Point3D(Math.Floor((x.x - r) / d.GetXSpacing()), Math.Ceiling((x.y + r) / d.GetYSpacing()), Math.Ceiling((x.z + r) / d.GetZSpacing()));
            //Point3D F = new Point3D(Math.Ceiling((x.x + r) / d.GetXSpacing()), Math.Ceiling((x.y + r) / d.GetYSpacing()), Math.Ceiling((x.z + r) / d.GetZSpacing()));
            //Point3D G = new Point3D(Math.Ceiling((x.x + r) / d.GetXSpacing()), Math.Floor((x.y - r) / d.GetYSpacing()), Math.Ceiling((x.z + r) / d.GetZSpacing()));
            //Point3D H = new Point3D(Math.Floor((x.x - r) / d.GetXSpacing()), Math.Floor((x.y - r) / d.GetYSpacing()), Math.Ceiling((x.z + r) / d.GetZSpacing()));

            Point3D A = new Point3D(x.x - r, x.y + r, x.z - r);
            Point3D C = new Point3D(x.x + r, x.y - r, x.z - r);
            Point3D D = new Point3D(x.x - r, x.y - r, x.z - r);
            Point3D H = new Point3D(x.x - r, x.y - r, x.z + r);

            for (double i = D.x; i <= C.x; i += step)
            {
                for (double j = D.y; j <= A.y; j += step)
                {
                    for (double k = D.z; k <= H.z; k += step)
                    {
                        double d2 = (i - x.x) * (i - x.x) + (j - x.y) * (j - x.y) + (k - x.z) * (k - x.z);
                        if (d2 <= r * r)
                        {
                            points.Add(new Point3D(i, j, k));
                        }
                    }
                }
            }
            return points;
        }

        /// <summary>
        /// Overload with step as a parameter
        /// </summary>
        /// <param name="x"></param>
        /// <param name="d"></param>
        /// <param name="r"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static List<Point3D> GetSphere(Point3D x, VolumetricData d, double r, double step)
        {
            List<Point3D> points = new List<Point3D>();
     
            Point3D A = new Point3D(x.x - r, x.y + r, x.z - r);
            Point3D C = new Point3D(x.x + r, x.y - r, x.z - r);
            Point3D D = new Point3D(x.x - r, x.y - r, x.z - r);
            Point3D H = new Point3D(x.x - r, x.y - r, x.z + r);

            for (double i = D.x; i <= C.x; i += step)
            {
                for (double j = D.y; j <= A.y; j += step)
                {
                    for (double k = D.z; k <= H.z; k += step)
                    {
                        double d2 = (i - x.x) * (i - x.x) + (j - x.y) * (j - x.y) + (k - x.z) * (k - x.z);
                        if (d2 <= r * r)
                        {
                            points.Add(new Point3D(i, j, k));
                        }
                    }
                }
            }
            return points;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public FeatureVector ComputeFeatureVector(VolumetricData d, Point3D p)
        {
            double[] fv = new double[5];
            double norm = 0;
            for (double r = 0.5; r <= 2.5; r += 0.5)
            {
                double sum = 0;
                List<Point3D> points = GetSphere(p, d, r);
                foreach (Point3D point in points)
                {
                    //DEBUG 
                    //sum += d.GetValue(point.x*d.GetXSpacing(), point.y*d.GetYSpacing(), point.z*d.GetZSpacing());
                    sum += d.GetValue(point.x, point.y, point.z);
                }
                int i = (int)(r / 0.5) - 1;
                double avg = sum/points.Count;
                norm += avg;
                fv[i] = avg;
            }
            norm = Math.Sqrt(norm);
            return new FeatureVector(p, fv[0]/norm, fv[1]/norm, fv[2]/norm, fv[3]/norm, fv[4]/norm) ;
        }
    }
}
