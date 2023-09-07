using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace DataView.PepaContrib
{
    public class TestFeatureComputer : IFeatureComputer
    {
        FeatureVector IFeatureComputer.ComputeFeatureVector(IData d, Point3D p)
        {
            return new FeatureVector();
        }

        /// <summary>
        /// This method returns values of coordinates distanced from origin
        /// </summary>
        /// <param name="minRadius"></param>
        /// <param name="maxRadius"></param>
        /// <param name="count"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private List<Point3D> getRingPoints(double minRadius, double maxRadius, int count, int seed)
        {
            if (minRadius > maxRadius)
                throw new ArgumentException("Min radius is expected to be lower than max radius");

            if (count < 0)
                throw new ArgumentException("Count needs to be positive");

            List<Point3D> points = new List<Point3D>();
            Random random = new Random(seed);

            for (int i = 0; i < count; i++)
            {
                double randomRadius = random.NextDouble() * (maxRadius - minRadius) + minRadius;
                double randomAngle1 = random.NextDouble() * 2 * Math.PI;
                double randomAngle2 = random.NextDouble() * 2 * Math.PI;

                //The calculations are based on formula in section Generalization here https://en.wikipedia.org/wiki/Spherical_coordinate_system

                double xCoordinate = randomRadius * Math.Sin(randomAngle1) * Math.Cos(randomAngle2);
                double yCoordinate = randomRadius * Math.Sin(randomAngle1) * Math.Sin(randomAngle2);
                double zCoordinate = randomRadius * Math.Cos(randomAngle1);

                points.Add(new Point3D(xCoordinate, yCoordinate, zCoordinate));
            }

            //Test of ring points
            bool works = testRingPoints(minRadius, maxRadius, points);
            Console.WriteLine(works ? "Works fine" : "Doesn't work");

            return points;
        }

        private Vector<double> getDirectionVector(List<Point3D> sampledPoints, Point3D centerPoint, IData data)
        {
            Vector<double> directionVector = Vector<double>.Build.Dense(3);

            double[] values = new double[sampledPoints.Count];
            double min = double.MaxValue;
            double max = double.MinValue;

            Point3D wAvg = new Point3D(0, 0, 0);
            double ws = 0;

            //Get min & max values and load data to values array
            for (int i = 0; i < sampledPoints.Count; i++)
            {
                values[i] = data.GetValue(sampledPoints[i]);

                min = Math.Min(min, values[i]);
                max = Math.Max(max, values[i]);
            }

            for (int i = 0; i < values.Length; i++)
            {

                double w = (values[i] - min) / (max - min); //percentage from the overall range
                ws += w;

                wAvg.X += sampledPoints[i].X * w;
                wAvg.Y += sampledPoints[i].Y * w;
                wAvg.Z += sampledPoints[i].Z * w;
            }


            wAvg.X /= ws;
            wAvg.Y /= ws;
            wAvg.Z /= ws;

            double diffX = wAvg.X - centerPoint.X;
            double diffY = wAvg.Y - centerPoint.Y;
            double diffZ = wAvg.Z - centerPoint.Z;

            directionVector[0] = diffX;
            directionVector[1] = diffY;
            directionVector[2] = diffZ;

            return directionVector;
        }

        private bool testRingPoints(double minRadius, double maxRadius, List<Point3D> points)
        {
            foreach (Point3D currentPoint in points)
            {
                double magnitude = calculateMagnitude(currentPoint);
                if (magnitude > maxRadius || magnitude < minRadius)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Calculates the magnitude of a given vector
        /// </summary>
        /// <param name="point">Vector for the magnitude calculation</param>
        /// <returns>Return the magnitude</returns>
        private double calculateMagnitude(Point3D point)
        {
            return Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Y, 2) + Math.Pow(point.Z, 2));
        }
    }
}

