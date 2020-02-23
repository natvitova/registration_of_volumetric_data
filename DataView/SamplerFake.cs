using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DataView
{
    class SamplerFake
    {
        int translationX = 0;
        int translationY = 0;
        int translationZ = 0;
        // for macro data
        Point3D[] pointsMax;
        Point3D[] pointsMin;

        IConfiguration configuration;

        public SamplerFake(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Point3D[] Sample(VolumetricData d, int count, int radius)
        {
            this.pointsMax = new Point3D[count];
            this.pointsMin = new Point3D[count];
            int[] measures = d.Measures;
            Random r = new Random(); // change rnd

            for (int i = 0; i < count; i++)
            {
                double x = r.Next(translationX+radius, measures[0]+1-radius);
                double y = r.Next(translationY+radius, measures[1]+1-radius);
                double z = r.Next(translationZ+radius, measures[2]+1-radius);

                pointsMax[i] = new Point3D(x, y, z);
            }
            GetSamples2();
            return pointsMax;
        }

        //Uses configuration file
        public Point3D[] Sample(VolumetricData d)
        {
            return Sample(d, Convert.ToInt32(configuration["Sampling:count"]), Convert.ToInt32(configuration["Sampling:radius"]));
            
        }

        public void SetTranslation(int[] translation)
        {
            this.translationX = translation[0];
            this.translationY = translation[1];
            this.translationZ = translation[2];
        }

        private void GetSamples2()
        {
            for (int i = 0; i < pointsMax.Length; i++)
            {
                this.pointsMin[i] = new Point3D(pointsMax[i].X - translationX, pointsMax[i].Y - translationY, pointsMax[i].Z - translationZ);
            }
        }       
        
        internal Point3D[] PointsMin { get => pointsMin; set => pointsMin = value; }
    }
}
