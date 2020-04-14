using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DataView
{
    class Sampler : ISampler
    {
        //private int rSphere = 5; //Included in config

        IConfiguration configuration;
        private int rSphere;
        public Sampler(IConfiguration configuration)
        {
            this.configuration = configuration;
            rSphere = Convert.ToInt32(configuration["Sampling:rSphere"]);
        }

        public Point3D[] Sample(IData d, int count)
        {
            Point3D[] points = new Point3D[count];

            int[] measures = d.Measures;
            Random r = new Random();
            for (int i = 0; i < count; i++)
            {
                double x = GetRandomDouble(rSphere, measures[0]* d.XSpacing - rSphere, r) ;
                double y = GetRandomDouble(rSphere, measures[1]* d.YSpacing - rSphere, r) ;
                double z = GetRandomDouble(rSphere, measures[2]* d.ZSpacing - rSphere, r) ;

                points[i] = new Point3D(x, y, z);
            }
            return points;
        }

        //Uses configuration file
        public Point3D[] Sample(VolumetricData d)
        {
            return Sample(d, Convert.ToInt32(configuration["Sampling:count"]));
        }

        private double GetRandomDouble(double minimum, double maximum, Random r)
        { 
            return r.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
