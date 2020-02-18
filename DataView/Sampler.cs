using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    class Sampler : ISampler
    {
        private int rSphere = 5;
        public Point3D[] Sample(VolumetricData d, int count)
        {
            Point3D[] points = new Point3D[count];

            int[] measures = d.Measures;
            Random r = new Random();
            for (int i = 0; i < count; i++)
            {
                double x = GetRandomDouble(rSphere, measures[0] - rSphere, r);
                double y = GetRandomDouble(rSphere, measures[1] - rSphere, r);
                double z = GetRandomDouble(rSphere, measures[2] - rSphere, r);

                points[i] = new Point3D(x, y, z);
            }
            return points;
        }

        private double GetRandomDouble(double minimum, double maximum, Random r)
        { 
            return r.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
