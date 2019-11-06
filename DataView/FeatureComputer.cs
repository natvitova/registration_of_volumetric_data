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
        public static List<Point3D> GetSphere(Point3D x, double r)
        {
            List<Point3D> points = new List<Point3D>();
            double step = 0.5;

            //Point3D A = new Point3D(Math.Floor((x.x - r) / d.GetXSpacing()), Math.Ceiling((x.y + r) / d.GetYSpacing()), Math.Floor((x.z - r) / d.GetZSpacing()));
            //Point3D B = new Point3D(Math.Ceiling((x.x + r) / d.GetXSpacing()), Math.Ceiling((x.y + r) / d.GetYSpacing()), Math.Floor((x.z - r) / d.GetZSpacing()));
            //Point3D C = new Point3D(Math.Ceiling((x.x + r) / d.GetXSpacing()), Math.Floor((x.y - r) / d.GetYSpacing()), Math.Floor((x.z - r) / d.GetZSpacing()));
            //Point3D D = new Point3D(Math.Floor((x.x - r) / d.GetXSpacing()), Math.Floor((x.y - r) / d.GetYSpacing()), Math.Floor((x.z - r) / d.GetZSpacing()));
            //Point3D E = new Point3D(Math.Floor((x.x - r) / d.GetXSpacing()), Math.Ceiling((x.y + r) / d.GetYSpacing()), Math.Ceiling((x.z + r) / d.GetZSpacing()));
            //Point3D F = new Point3D(Math.Ceiling((x.x + r) / d.GetXSpacing()), Math.Ceiling((x.y + r) / d.GetYSpacing()), Math.Ceiling((x.z + r) / d.GetZSpacing()));
            //Point3D G = new Point3D(Math.Ceiling((x.x + r) / d.GetXSpacing()), Math.Floor((x.y - r) / d.GetYSpacing()), Math.Ceiling((x.z + r) / d.GetZSpacing()));
            //Point3D H = new Point3D(Math.Floor((x.x - r) / d.GetXSpacing()), Math.Floor((x.y - r) / d.GetYSpacing()), Math.Ceiling((x.z + r) / d.GetZSpacing()));

            Point3D A = new Point3D(x.X - r, x.Y + r, x.Z - r);
            Point3D C = new Point3D(x.X + r, x.Y - r, x.Z - r);
            Point3D D = new Point3D(x.X - r, x.Y - r, x.Z - r);
            Point3D H = new Point3D(x.X - r, x.Y - r, x.Z + r);

            for (double i = D.X; i <= C.X; i += step)
            {
                for (double j = D.Y; j <= A.Y; j += step)
                {
                    for (double k = D.Z; k <= H.Z; k += step)
                    {
                        double d2 = (i - x.X) * (i - x.X) + (j - x.Y) * (j - x.Y) + (k - x.Z) * (k - x.Z);
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
        public static List<Point3D> GetSphere(Point3D x, double r, double step)
        {
            List<Point3D> points = new List<Point3D>();

            Point3D A = new Point3D(x.X - r, x.Y + r, x.Z - r);
            Point3D C = new Point3D(x.X + r, x.Y - r, x.Z - r);
            Point3D D = new Point3D(x.X - r, x.Y - r, x.Z - r);
            Point3D H = new Point3D(x.X - r, x.Y - r, x.Z + r);

            for (double i = D.X; i <= C.X; i += step)
            {
                for (double j = D.Y; j <= A.Y; j += step)
                {
                    for (double k = D.Z; k <= H.Z; k += step)
                    {
                        double d2 = (i - x.X) * (i - x.X) + (j - x.Y) * (j - x.Y) + (k - x.Z) * (k - x.Z);
                        if (d2 <= r * r)
                        {
                            points.Add(new Point3D(i, j, k));
                        }
                    }
                }
            }
            return points;
        }

        public static List<Point3D> GetSphereN(Point3D x, VolumetricData d, double r, double step)
        {
            List<Point3D> points = new List<Point3D>();

            Point3D A = new Point3D(Math.Floor((x.X - r) / d.XSpacing), Math.Ceiling((x.Y + r) / d.YSpacing), Math.Floor((x.Z - r) / d.ZSpacing));
            Point3D C = new Point3D(Math.Ceiling((x.X + r) / d.XSpacing), Math.Floor((x.Y - r) / d.YSpacing), Math.Floor((x.Z - r) / d.ZSpacing));
            Point3D D = new Point3D(Math.Floor((x.X - r) / d.XSpacing), Math.Floor((x.Y - r) / d.YSpacing), Math.Floor((x.Z - r) / d.ZSpacing));
            Point3D H = new Point3D(Math.Floor((x.X - r) / d.XSpacing), Math.Floor((x.Y - r) / d.YSpacing), Math.Ceiling((x.Z + r) / d.ZSpacing));

            for (double i = D.X; i <= C.X; i += step)
            {
                for (double j = D.Y; j <= A.Y; j += step)
                {
                    for (double k = D.Z; k <= H.Z; k += step)
                    {
                        double d2 = (i - x.X) * (i - x.X) + (j - x.Y) * (j - x.Y) + (k - x.Z) * (k - x.Z);
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
                List<Point3D> points = GetSphere(p, r);
                foreach (Point3D point in points)
                {
                    //DEBUG 
                    //sum += d.GetValueRealCoordinates(point.x*d.XSpacing, point.y*d.YSpacing, point.z*d.ZSpacing);
                    sum += d.GetValueMatrixCoordinates(point.X, point.Y, point.Z);
                }
                int i = (int)(r / 0.5) - 1;
                //double avg = sum/points.Count;
                //norm += avg;
                //fv[i] = avg;
                norm += sum;
                fv[i] = sum;
            }
            norm = Math.Sqrt(norm);
            return new FeatureVector(p, fv[0] / norm, fv[1] / norm, fv[2] / norm, fv[3] / norm, fv[4] / norm);
            //return new FeatureVector(p, fv[0], fv[1], fv[2], fv[3], fv[4]);
        }

        public FeatureVector ComputeFeatureVectorA(int[] t, Point3D p)
        {
            double[] fv = new double[5];
            double norm = 0;
            for (double r = 0.5; r <= 2.5; r += 0.5)
            {
                double sum = 0;
                List<Point3D> points = GetSphere(p, r);
                foreach (Point3D point in points)
                {
                    sum += (point.X + t[0]) + 3 * (point.Y + t[1]) + 8 * (point.Z + t[2]);
                }
                int i = (int)(r / 0.5) - 1;
                norm += sum;
                fv[i] = sum;
            }
            norm = Math.Sqrt(norm);
            return new FeatureVector(p, fv[0] / norm, fv[1] / norm, fv[2] / norm, fv[3] / norm, fv[4] / norm);
        }
    }
}
