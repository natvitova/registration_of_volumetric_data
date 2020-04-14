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
        public List<Point3D> GetSphere(Point3D x, double r)
        {
            return GetSphere(x, r, 500);
        }

        private List<Point3D> GetSphere(Point3D x, double r, int count)
        {
            List<Point3D> points = new List<Point3D>();

            Random rnd = new Random();
            double rSquared = r * r;
            do
            {
                double xCoordinate = GetRandomDouble(x.X - r, x.X + r, rnd);
                double yCoordinate = GetRandomDouble(x.Y - r, x.Y + r, rnd);
                double zCoordinate = GetRandomDouble(x.Z - r, x.Z + r, rnd);

                double distance = (xCoordinate - x.X) * (xCoordinate - x.X) + (yCoordinate - x.Y) * (yCoordinate - x.Y) + (zCoordinate - x.Z) * (zCoordinate - x.Z);
                if (distance <= rSquared)
                {
                    points.Add(new Point3D(xCoordinate, yCoordinate, zCoordinate));
                }
            } while (
                points.Count < count
            );

            return points;
        }

        public List<Point3D> GetSphere2(Point3D x, double r, int count)
        {
            List<Point3D> points = new List<Point3D>();

            Random rnd = new Random();
            double rSquared = r * r;
            do
            {
                double xCoordinate = GetRandomDouble(x.X - r, x.X + r, rnd);
                double yCoordinate = GetRandomDouble(x.Y - r, x.Y + r, rnd);
                double zCoordinate = GetRandomDouble(x.Z - r, x.Z + r, rnd);

                double distance = (xCoordinate - x.X) * (xCoordinate - x.X) + (yCoordinate - x.Y) * (yCoordinate - x.Y) + (zCoordinate - x.Z) * (zCoordinate - x.Z);
                if (distance <= rSquared)
                {
                    points.Add(new Point3D(xCoordinate, yCoordinate, zCoordinate));
                }
            } while (
                points.Count < count
            );

            return points;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public FeatureVector ComputeFeatureVector(IData d, Point3D p)
        {
            double[] fv = new double[5];
            double norm = 0;

            int i = 0;
            for (double r = 0.5; r <= 2.5; r += 0.5)
            {
                double sum = 0;
                List<Point3D> points = GetSphere(p, r, (i+1)*500);

                foreach (Point3D point in points)
                {
                    sum += d.GetValue(point); //real coordinates
                }
                double avg = sum / points.Count;
                norm += avg * avg;
                fv[i] = avg;
                i++;
            }

            norm = Math.Sqrt(norm);
            if (norm == 0)
            {
                norm = 1;
            }
            return new FeatureVector(p, fv[0] / norm, fv[1] / norm, fv[2] / norm, fv[3] / norm, fv[4] / norm);
        }

        public FeatureVector ComputeFeatureVector2(IData d, Point3D p)
        {
            double[] fv = new double[5];

            int i = 0;
            double sum = 0;
            double countOld = 0;

            for (double r = 0.5; r <= 2.5; r += 0.5)
            {
                double sumOld = sum;
                List<Point3D> points = GetSphere(p, r, (i+1)*500); // r change nconst!

                foreach (Point3D point in points)
                {
                    sum += d.GetValue(point); //real coordinates
                }
                sum -= sumOld;

                double count = points.Count - countOld;
                double avg = sum / count;
                countOld = points.Count;
                fv[i] = avg;
                i++;
            }

            return new FeatureVector(p, fv[0], fv[1], fv[2], fv[3], fv[4]);
        }

        public FeatureVector ComputeFeatureVector3(IData d, Point3D p)
        {
            double[] fv = new double[5];
            double norm = 0;

            int i = 0;
            double sum = 0;
            double countOld = 0;

            for (double r = 1; r <= 5; r += 1)
            {
                double sumOld = sum;
                List<Point3D> points = GetSphere(p, r, (i+1)*500); // r change nconst!

                foreach (Point3D point in points)
                {
                    sum += d.GetValue(point); //real coordinates
                }
                sum -= sumOld;

                double count = points.Count - countOld;
                double avg = sum / count;
                countOld = points.Count;
                norm += avg * avg;
                fv[i] = avg;
                i++;
            }

            norm = Math.Sqrt(norm);
            if (norm == 0)
            {
                norm = 1;
            }
            return new FeatureVector(p, fv[0] / norm, fv[1] / norm, fv[2] / norm, fv[3] / norm, fv[4] / norm);
        }

        private double GetRandomDouble(double minimum, double maximum, Random r)
        {
            return r.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
