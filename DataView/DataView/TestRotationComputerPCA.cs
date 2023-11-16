using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace DataView
{
    class TestRotationComputerPCA
    {

        public static double radius = 1;

        /// <summary>
        /// Calculates the rotation matrix from d1 to d2
        /// </summary>
        /// <param name="dMicro">Data1</param>
        /// <param name="dMacro">Data2</param>
        /// <param name="pointMicro">Point in data1 to take samples around</param>
        /// <param name="pointMacro">Point in data2 to take samples around</param>
        /// <param name="count">Number of samples taken</param>
        /// <returns></returns>
        public static Matrix<double> CalculateRotation(IData dMicro, IData dMacro, Point3D pointMicro, Point3D pointMacro, int count)
        {
            Random mainRnd = new Random();


            //DIFFERENT RANDOM POINTS USED FOR MICRO AND MACRO
            /*
            Matrix<double> basisMicro = GetPointBasis(dMicro, pointMicro, count, radius, mainRnd);
            Matrix<double> basisMacro = GetPointBasis(dMacro, pointMacro, count, radius, mainRnd);
            */

            //RANDOM POINTS, BUT SAME POINTS USED FOR MICRO AND MACRO
            int randomSeed = new Random().Next();

            Matrix<double> basisMicro;
            Matrix<double> basisMacro;

            try
            {
                basisMicro = GetPointBasis(dMicro, pointMicro, count, radius, new Random(randomSeed));
                basisMacro = GetPointBasis(dMacro, pointMacro, count, radius, new Random(randomSeed));
            }
            catch (Exception e) { throw e; }

            Matrix<double> transitionMatrix = Matrix<double>.Build.Dense(3, 3);

            Matrix<double> equationMatrix = Matrix<double>.Build.Dense(3, 4);


            //Calculation of the transition matrix
            for (int basisNumber = 0; basisNumber<3; basisNumber++)
            {
                for (int i = 0; i < 3; i++)
                    equationMatrix.SetColumn(i, basisMacro.Column(i));

                equationMatrix.SetColumn(3, basisMicro.Column(basisNumber));

                Vector<double> result;

                try { result = EquationComputer.CalculateSolution(equationMatrix); }
                catch (Exception e) { throw e; }

                transitionMatrix.SetColumn(basisNumber, result);
            }

            //Replace 0 values
            for(int i = 0; i<transitionMatrix.RowCount; i++)
            {
                for(int j = 0; j<transitionMatrix.ColumnCount; j++)
                {
                    if (Math.Abs(transitionMatrix[i, j]) <= Double.Epsilon)
                        transitionMatrix[i, j] = 0;
                }
            }

            return transitionMatrix;
        }

        /// <summary>
        /// This method tests whether the given vectors are parallel or antiparallel.
        /// Both of the passed vectors are expected to have dimension 3
        /// </summary>
        /// <param name="vectorA">First vector</param>
        /// <param name="vectorB">Second vector</param>
        /// <returns>Returns true if vectors are parallel or antiparallel, otherwise false.</returns>
        /// <exception cref="ArgumentException">Exception when the given arguments</exception>
        /// 
        private static bool ParallelVectors(Vector<double> vectorA, Vector<double> vectorB)
        {
            if (vectorA.Count != 3)
                throw new ArgumentException("VectorA is expected to have dimension 3");
            if (vectorB.Count != 3)
                throw new ArgumentException("VectorB is expected to have dimension 3");
            if (vectorA.Equals(Vector<double>.Build.Dense(3)))
                throw new ArgumentException("VectorA has only zeros.");
            if(vectorB.Equals(Vector<double>.Build.Dense(3)))
                throw new ArgumentException("VectorB has only zeros.");

            double scalingFactor = vectorA[0] / vectorB[0];
            for(int i = 1; i<vectorA.Count; i++)
            {
                double scaledVector = vectorB[i] * scalingFactor;
                if (compareWithTolerance(scaledVector, vectorA[i]) || compareWithTolerance(scaledVector, -vectorA[i]))
                    return true;
            }
            return false;
        }

        private static bool compareWithTolerance(double numberA, double numberB)
        {
            double epsilon = 0.00000001;
            if ((numberA + epsilon > numberB) && (numberA - epsilon < numberB))
                return true;
            
            return false;
        }

        /// <summary>
        /// Finds orthogonal vector to the one passed as a parameter
        /// </summary>
        /// <param name="inputVector"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static Vector<double> findOrthogonalVector(Vector<double> inputVector)
        {
            if (inputVector.Count != 3)
                throw new ArgumentException("Vector needs to have dimension 3");

            Vector<double> orthogonalVector = Vector<double>.Build.Dense(3);
            double a1 = 1;
            double b1 = 1;

            orthogonalVector[0] = a1;
            orthogonalVector[1] = b1;
            orthogonalVector[2] = (inputVector[0] * a1 + inputVector[1] * b1) / (-inputVector[2]);

            return orthogonalVector;
        }

        /// <summary>
        /// This method makes sure value is within bounds
        /// </summary>
        /// <param name="currentValue">Value to be constrained</param>
        /// <param name="minimumValue">Minimum value</param>
        /// <param name="maxiumValue">Maximum value</param>
        /// <returns>Returns constrained value</returns>
        private static double Constrain(double currentValue, double minimumValue, double maxiumValue)
        {
            if (currentValue > maxiumValue)
                return maxiumValue;

            if (currentValue < minimumValue)
                return minimumValue;

            return currentValue;
        }

        /// <summary>
        /// Normalizes vector passsed as a parameter
        /// </summary>
        /// <param name="vector">Vector to normalize</param>
        /// <returns>
        /// Returns normalized vector if its magnitude is not equal to 0.
        /// Otherwise it returns Vector with 3 rows and 0 columns.
        /// </returns>
        private static Vector<double> NormalizeVector(Vector<double> vector) {

            double magnitude = vector.L2Norm();

            //Prevents from crashing when denominator(magnitude) is equal to 0
            if (magnitude == 0)
                return Vector<double>.Build.Dense(3, 0);

            return vector / magnitude;
        }

        /// <summary>
        /// Computes the crossProduct of vectors size 3
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private static Matrix<double> CrossProduct(Vector<double> u, Vector<double> v)
        {
            Matrix<double> w = Matrix<double>.Build.Dense(1,3);
            w[0, 0] = u[1] * v[2] - v[1] * u[2];
            w[0, 1] = v[0] * u[2] - u[0] * v[2];
            w[0, 2] = u[0] * v[1] - v[0] * u[1];
            return w;
        }

        /// <summary>
        /// Calculates the dot product for the two given vectors
        /// </summary>
        /// <param name="vector1">First vector</param>
        /// <param name="vector2">Second vector</param>
        /// <returns>Returns the scalar product of the two given vectors</returns>
        /// <exception cref="ArgumentException"></exception>
        private static double ScalarProduct(Vector<double> vector1, Vector<double> vector2)
        {
            int upperBound = vector1.AsArray().Length;

            if (upperBound != vector2.AsArray().Length)
                throw new ArgumentException("Vectors should be the same size");

            double result = 0;
            for(int i = 0; i<upperBound; i++)
                result += vector1[i] * vector2[i];

            return result;
        }

        // od pana Váši
        private static Matrix<double> GetPointBasis(IData d, Point3D point, int count, double radius, Random rnd)
        {
            List<Point3D> pointsInSphere = GetSphere(point, radius, count, rnd.Next()); 

            double[] values = new double[count];
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
                    values[i] = d.GetValue(pointsInSphere[i]);

                    min = Math.Min(min, values[i]);
                    max = Math.Max(max, values[i]);
                }
                catch
                {
                    //The point is out of bounds for the particular 3D object
                    continue;
                }
                
            }

            //Values min and max are the same
            if (Math.Abs(min - max) < Double.Epsilon)
                throw new ArgumentException("Basis cannot be calculated because all sampled values in the point surrounding are the same.");

            for (int i = 0; i < count; i++)
            {
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

            Vector<double> crossProductVector = Vector<double>.Build.Dense(3);

            if (ParallelVectors(highConcentrationVector, lowConcentrationVector))
                crossProductVector = findOrthogonalVector(highConcentrationVector);
            else
                crossProductVector = CrossProduct(highConcentrationVector, lowConcentrationVector).Row(0);

            crossProductVector = NormalizeVector(crossProductVector);


            Matrix<double> resultMatrix = Matrix<double>.Build.Dense(3, 3);

            //The matrix is composed out of unit vectors

            resultMatrix[0, 0] = highConcentrationVector[0];
            resultMatrix[1, 0] = highConcentrationVector[1];
            resultMatrix[2, 0] = highConcentrationVector[2];

            resultMatrix[0, 1] = lowConcentrationVector[0];
            resultMatrix[1, 1] = lowConcentrationVector[1];
            resultMatrix[2, 1] = lowConcentrationVector[2];

            resultMatrix[0, 2] = crossProductVector[0];
            resultMatrix[1, 2] = crossProductVector[1];
            resultMatrix[2, 2] = crossProductVector[2];

            return resultMatrix;
        }

        private static List<Point3D> GetSphere(Point3D p, double r, int count, int seed)
        {
            List<Point3D> points = new List<Point3D>();

            Random rnd = new Random(seed);
            double rSquared = r * r;
            do
            {
                double xCoordinate = GetRandomDouble(p.X - r, p.X + r, rnd);
                double yCoordinate = GetRandomDouble(p.Y - r, p.Y + r, rnd);
                double zCoordinate = GetRandomDouble(p.Z - r, p.Z + r, rnd);

                double distance = (xCoordinate - p.X) * (xCoordinate - p.X) + (yCoordinate - p.Y) * (yCoordinate - p.Y) + (zCoordinate - p.Z) * (zCoordinate - p.Z);
                if (distance <= rSquared)
                {
                    points.Add(new Point3D(xCoordinate, yCoordinate, zCoordinate));
                }
            } while (
                points.Count < count
            );

            return points;
        }

        private static double GetRandomDouble(double minimum, double maximum, Random r)
        {
            return r.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}