using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    class FeatureComputer : IFeatureComputer
    {


        public List<Point3D> GetSphere(Point3D x, VolumetricData d, double r)
        {
            List<Point3D> points = new List<Point3D>();

            Point3D A = new Point3D(Math.Floor((x.x - r) / d.GetXSpacing()), Math.Ceiling((x.y + r) / d.GetYSpacing()), Math.Floor((x.z - r) / d.GetZSpacing()));
            Point3D B = new Point3D(Math.Ceiling((x.x + r) / d.GetXSpacing()), Math.Ceiling((x.y + r) / d.GetYSpacing()), Math.Floor((x.z - r) / d.GetZSpacing()));
            Point3D C = new Point3D(Math.Ceiling((x.x + r) / d.GetXSpacing()), Math.Floor((x.y - r) / d.GetYSpacing()), Math.Floor((x.z - r) / d.GetZSpacing()));
            Point3D D = new Point3D(Math.Floor((x.x - r) / d.GetXSpacing()), Math.Floor((x.y - r) / d.GetYSpacing()), Math.Floor((x.z - r) / d.GetZSpacing()));
            Point3D E = new Point3D(Math.Floor((x.x - r) / d.GetXSpacing()), Math.Ceiling((x.y + r) / d.GetYSpacing()), Math.Ceiling((x.z + r) / d.GetZSpacing()));
            Point3D F = new Point3D(Math.Ceiling((x.x + r) / d.GetXSpacing()), Math.Ceiling((x.y + r) / d.GetYSpacing()), Math.Ceiling((x.z + r) / d.GetZSpacing()));
            Point3D G = new Point3D(Math.Ceiling((x.x + r) / d.GetXSpacing()), Math.Floor((x.y - r) / d.GetYSpacing()), Math.Ceiling((x.z + r) / d.GetZSpacing()));
            Point3D H = new Point3D(Math.Floor((x.x - r) / d.GetXSpacing()), Math.Floor((x.y - r) / d.GetYSpacing()), Math.Ceiling((x.z + r) / d.GetZSpacing()));


            for (double i = D.x; i <= C.x; i += d.GetXSpacing())
            {
                for (double j = D.y; j <= A.y; j += d.GetYSpacing())
                {
                    for (double k = D.z; k <= H.z; k += d.GetZSpacing())
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

        public double[] ComputeFeatureVector(VolumetricData d, Point3D p)
        {
            double[] featureVector = new double[5];
            for (double r = 0.5; r <= 2.5; r += 0.5) // prumer ?
            {
                double sum = 0;
                List<Point3D> points = GetSphere(p, d, r);
                foreach (Point3D point in points)
                {
                    sum += d.GetValue(point.x, point.y, point.z);
                }
                int i = (int)(r / 0.5) - 1;
                featureVector[i] = sum;
            }

            return featureVector;
        }
    }
}
