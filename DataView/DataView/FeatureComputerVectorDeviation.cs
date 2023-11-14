using System;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;

namespace DataView
{
    public class FeatureComputerVectorDeviation : IFeatureComputer
	{

        FeatureVector IFeatureComputer.ComputeFeatureVector(IData d, Point3D p)
        {
            const int COUNT = 10_000;
            const int RADIUS = 5;

            Random random = new Random();
            int seed = random.Next();

            seed = 20; //Currently set to 20 for test purposes
            
            PointSurrounding pointSurrounding = GetPointSurrounding(d, p, COUNT, RADIUS, seed);


            return new FeatureVector(p, pointSurrounding.HighConcentrationValue, pointSurrounding.LowConcentrationValue, pointSurrounding.AngleXY, pointSurrounding.AngleXZ, pointSurrounding.DistributionValueAvg);
        }

        private PointSurrounding GetPointSurrounding(IData d, Point3D point, int count, double radius, int seed)
        {
            List<Point3D> pointsInSphere = GetSphere(point, radius, count, seed);

            Matrix<double> result = Matrix<double>.Build.Dense(3, 3);

            //double[] values = new double[count];
            List<double> values = new List<double>();

            double min = double.MaxValue;
            double max = double.MinValue;

            Point3D wAvgHigh = new Point3D(0, 0, 0);
            Point3D wAvgLow = new Point3D(0, 0, 0);

            double wsHigh = 0;
            double wsLow = 0;


            for (int i = 0; i < count; i++)
            {
                try
                {
                    values.Add(d.GetValue(pointsInSphere[i]));

                    min = Math.Min(min, values[values.Count-1]);
                    max = Math.Max(max, values[values.Count-1]);
                }
                catch
                {
                    //Point is outside of bounds for the currently sampled object
                    continue;
                }
            }

            //Values min and max are the same
            if (Math.Abs(min - max) < Double.Epsilon)
                throw new ArgumentException("Basis cannot be calculated because all sampled values in the point surrounding are the same.");

            double distributionValueAvg = 0;

            for (int i = 0; i < values.Count; i++)
            {
                //Add value distribution
                distributionValueAvg += d.GetValueDistribution(values[i]);

                double wHigh = (values[i] - min) / (max - min); //percentage from the overall range
                double wLow = 1 - wHigh;

                wsLow += wLow;
                wsHigh += wHigh;

                wAvgHigh.X += pointsInSphere[i].X * wHigh;
                wAvgHigh.Y += pointsInSphere[i].Y * wHigh;
                wAvgHigh.Z += pointsInSphere[i].Z * wHigh;

                wAvgLow.X += pointsInSphere[i].X * wLow;
                wAvgLow.Y += pointsInSphere[i].Y * wLow;
                wAvgLow.Z += pointsInSphere[i].Z * wLow;
            }


            wAvgHigh.X /= wsHigh;
            wAvgHigh.Y /= wsHigh;
            wAvgHigh.Z /= wsHigh;

            wAvgLow.X /= wsLow;
            wAvgLow.Y /= wsLow;
            wAvgLow.Z /= wsLow;

            double diffXHigh = wAvgHigh.X - point.X;
            double diffYHigh = wAvgHigh.Y - point.Y;
            double diffZHigh = wAvgHigh.Z - point.Z;

            double diffXLow = wAvgLow.X - point.X;
            double diffYLow = wAvgLow.Y - point.Y;
            double diffZLow = wAvgLow.Z - point.Z;


            Vector<double> highConcentrationVector = Vector<double>.Build.Dense(3);
            highConcentrationVector[0] = diffXHigh;
            highConcentrationVector[1] = diffYHigh;
            highConcentrationVector[2] = diffZHigh;

            Vector<double> lowConcentrationVector = Vector<double>.Build.Dense(3);
            lowConcentrationVector[0] = diffXLow;
            lowConcentrationVector[1] = diffYLow;
            lowConcentrationVector[2] = diffZLow;


            highConcentrationVector = NormalizeVector(highConcentrationVector);
            lowConcentrationVector = NormalizeVector(lowConcentrationVector);

            double highConcentrationValue = 0;
            double lowConcentrationValue = 0;

            try
            {
                highConcentrationValue = d.GetValue(wAvgHigh.X, wAvgHigh.Y, wAvgHigh.Z);
                lowConcentrationValue = d.GetValue(wAvgLow.X, wAvgLow.Y, wAvgLow.Z);
                //lowConcentrationValue = d.GetValue(wAvgLow);
            }
            catch
            {
                throw new ApplicationException("The average for low and high concentration vector has been calculated, but both of them are outside of bounds. This indicates theres a bug in the FeatureComputerVectorDeviation class that needs to be resolved.");
            }

            //Normalize the highConcentration and lowConcentration values - percentage range (normalizing based on the distribution should be considered)
            highConcentrationValue = d.GetValueDistribution(highConcentrationValue);
            lowConcentrationValue = d.GetValueDistribution(lowConcentrationValue);
            //highConcentrationValue = (highConcentrationValue - min) / (max - min);
            //lowConcentrationValue = (highConcentrationValue - min) / (max - min);

            //Calculation of the angle XY between concentration vectors
            Vector<double> firstVector = Vector<double>.Build.DenseOfArray(new double[] { highConcentrationVector[0], highConcentrationVector[1] });
            Vector<double> secondVector = Vector<double>.Build.DenseOfArray(new double[] { lowConcentrationVector[0], lowConcentrationVector[1] });
            double angleXY = Math.Acos(DotProduct(firstVector, secondVector) / (firstVector.L2Norm() * secondVector.L2Norm()));

            //Calculation of the angle XZ between concentration vectors
            firstVector = Vector<double>.Build.DenseOfArray(new double[] { highConcentrationVector[0], highConcentrationVector[2] });
            secondVector= Vector<double>.Build.DenseOfArray(new double[] { lowConcentrationVector[0], lowConcentrationVector[2] });
            double angleXZ = Math.Acos(DotProduct(firstVector, secondVector) / (firstVector.L2Norm() * secondVector.L2Norm()));

            return new PointSurrounding(distributionValueAvg/values.Count, highConcentrationValue, lowConcentrationValue, angleXY, angleXZ);
        }

        /// <summary>
        /// Calculates the dot product for the two given vectors
        /// </summary>
        /// <param name="vector1">First vector</param>
        /// <param name="vector2">Second vector</param>
        /// <returns>Returns the scalar product of the two given vectors</returns>
        /// <exception cref="ArgumentException"></exception>
        private static double DotProduct(Vector<double> vector1, Vector<double> vector2)
        {
            int upperBound = vector1.AsArray().Length;

            if (upperBound != vector2.AsArray().Length)
                throw new ArgumentException("Vectors should be the same size");

            double result = 0;
            for (int i = 0; i < upperBound; i++)
                result += vector1[i] * vector2[i];

            return result;
        }

        private static bool compareWithTolerance(double numberA, double numberB)
        {
            double epsilon = 0.00000001;
            if ((numberA + epsilon > numberB) && (numberA - epsilon < numberB))
                return true;

            return false;
        }

        /// <summary>
        /// Normalizes vector passsed as a parameter
        /// </summary>
        /// <param name="vector">Vector to normalize</param>
        /// <returns>
        /// Returns normalized vector if its magnitude is not equal to 0.
        /// Otherwise it returns Vector with 3 rows and 0 columns.
        /// </returns>
        private static Vector<double> NormalizeVector(Vector<double> vector)
        {

            double magnitude = vector.L2Norm();

            //Prevents from crashing when denominator(magnitude) is equal to 0
            if (magnitude == 0)
                return Vector<double>.Build.Dense(3, 0);

            return vector / magnitude;
        }

        /// <summary>
        /// This method returns list of points whose maximum distance is defined as radius from the reference point passed as parameter.
        /// </summary>
        /// <param name="p">Point around which should the points be generated</param>
        /// <param name="r">Maximum distance of generated points to the reference point</param>
        /// <param name="count">Number of points</param>
        /// <param name="seed">Seed for generating the points</param>
        /// <returns>This method returns list of points. None of them is further than radius from the reference point.</returns>
        private List<Point3D> GetSphere(Point3D p, double r, int count, int seed)
        {
            List<Point3D> points = GetSpheresIntersticePoints(point: p, minRadius: 0, maxRadius: r, count: count, seed: seed);
            return points;
        }

        /// <summary>
        /// This method returns list of points that are within certain distance defined as minRadius and maxRadius from the center of the sphere passed as a point
        /// </summary>
        /// <param name="point">Center of a sphere</param>
        /// <param name="minRadius">Generated point's minimum distance from the center of a sphere</param>
        /// <param name="maxRadius">Generated point's maximum distance from the center of a sphere</param>
        /// <returns>Returns list of points that are within certain distance from the center of the sphere.</returns>
        private List<Point3D> GetSpheresIntersticePoints(Point3D point, double minRadius, double maxRadius, int count, int seed)
        {
            if (minRadius > maxRadius)
                throw new ArgumentException("The min radius should be smaller than the max radius");

            Random random = new Random(seed);

            List<Point3D> listOfPoints = new List<Point3D>();
            while(listOfPoints.Count < count)
            {
                double radius = GetRandomDouble(minRadius, maxRadius, random);
                double angleTheta = random.NextDouble() * 2 * Math.PI;
                double anglePhi = random.NextDouble() * 2 * Math.PI;
                double x = radius * Math.Cos(anglePhi) * Math.Sin(angleTheta);
                double y = radius * Math.Sin(angleTheta) * Math.Sin(anglePhi);
                double z = radius * Math.Cos(angleTheta);

                
                if (x < 0 || y < 0 || z < 0)
                    continue;
                
                listOfPoints.Add(new Point3D(x, y, z));
            }

            return listOfPoints;
        }

        /// <summary>
        /// Returns random values within specified range
        /// </summary>
        /// <param name="minValue">Min value that could be generated</param>
        /// <param name="maxValue">Max value that could be generated</param>
        /// <returns>Returns random value within a specified range</returns>
        private double GetRandomDouble(double minValue, double maxValue, Random random)
        {
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }

        /// <summary>
        /// This class is a messenger for important
        /// values describing point's surrounding
        /// These values are then used as for feature vector creating within this class
        /// </summary>
        private class PointSurrounding
        {
            private double distributionValueAvg;

            /// <summary>
            /// Value in the high concentration position (point + highConcentratoinVector)
            /// </summary>
            private double highConcentrationValue;

            /// <summary>
            /// Value in the low concentration position (point + lowConcentratoinVector)
            /// </summary>
            private double lowConcentrationValue;

            /// <summary>
            /// Angle between the vectors - view from above
            /// </summary>
            private double angleXY;

            /// <summary>
            /// Angle between the vectors - vew from side
            /// </summary>
            private double angleXZ;


            public PointSurrounding(double distributionValueAvg, double highConcentrationValue, double lowConcentrationValue, double angleXY, double angleXZ)
            {
                //DistributionValueAverage
                this.distributionValueAvg = distributionValueAvg;

                //Values for the places
                this.highConcentrationValue = highConcentrationValue;
                this.lowConcentrationValue = lowConcentrationValue;

                //Angles
                this.angleXY = angleXY;
                this.angleXZ = angleXZ;
            }


            //GETTERS
            public double DistributionValueAvg
            {
                get { return distributionValueAvg; }
            }

            public double HighConcentrationValue
            {
                get { return highConcentrationValue; }
            }

            public double LowConcentrationValue
            {
                get { return lowConcentrationValue; }
            }

            public double AngleXY
            {
                get { return angleXY; }
            }

            public double AngleXZ
            {
                get { return angleXZ; }
            }
        }
    }

    
}

