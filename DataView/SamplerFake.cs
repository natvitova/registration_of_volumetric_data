using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    class SamplerFake : ISampler
    {
        int translationX = 0;
        int translationY = 0;
        int translationZ = 0;

        public Point3D[] Sample(VolumetricData d, int count)
        {
            // for macro data
            Point3D[] pointsMax = new Point3D[count];

            int[] measures = d.Measures;
            Random r = new Random();

            for (int i = 0; i < count; i++)
            {
                double x = r.Next(translationX, measures[0]+1);
                double y = r.Next(translationY, measures[1]+1);
                double z = r.Next(translationZ, measures[2]+1);

                pointsMax[i] = new Point3D(x, y, z);
            }
            return pointsMax;
        }

        public void getTranslation(int[] translation)
        {
            this.translationX = translation[0];
            this.translationY = translation[1];
            this.translationZ = translation[2];
        }
    }
}
