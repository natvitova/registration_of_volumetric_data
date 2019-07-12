using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    class Sampler : ISampler
    {
        public Point3D[] Sample(VolumetricData d, int count)
        {
            Point3D[] points = new Point3D[count];

            int[] measures = d.GetMeassures();
            int rSphere = 5;
            Random r = new Random();

            for (int i = 0; i < count; i++)
            {
                int x = r.Next(rSphere, measures[0] - rSphere);
                int y = r.Next(rSphere, measures[1] - rSphere);
                int z = r.Next(rSphere, measures[2] - rSphere);

                points[i] = new Point3D(x, y, z);
            }
            return points;
        }
    }
}
