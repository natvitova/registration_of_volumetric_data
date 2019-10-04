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

        public Point3D[] SampleSphereAroundPoint(VolumetricData d, Point3D centerPoint, int radius, int count)
        {
            
            Point3D[] selectedPoints = new Point3D[count];

            Random r = new Random();
            Point3D tmpPoint;
            int randomIndex;

            List<Point3D> pointsInSphere = FeatureComputer.GetSphere(centerPoint, d, radius); //gets all points in a given sphere

            for (int i = 0; i < count; i++)
            {
                randomIndex = r.Next(0, pointsInSphere.Count);

                tmpPoint = pointsInSphere[randomIndex]; //gets a point
                pointsInSphere.RemoveAt(randomIndex); //removes the gotten point from the sphere

                selectedPoints[i] = tmpPoint; //randomly selects points from the sphere

            }
            return selectedPoints;
        }
    }
}
