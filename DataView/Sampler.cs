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
                double x = r.Next(rSphere, measures[0] - rSphere);
                double y = r.Next(rSphere, measures[1] - rSphere);
                double z = r.Next(rSphere, measures[2] - rSphere);

                points[i] = new Point3D(x, y, z);
            }
            return points;
        }
    }
}
