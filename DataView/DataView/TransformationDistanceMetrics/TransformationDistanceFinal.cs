using System;
using MathNet.Numerics.LinearAlgebra;

namespace DataView
{
    /// <summary>
    /// This class is not finished implementing - some prerequisites need to be fullfiled in order for this method to work
    /// </summary>
    class TransformationDistanceFinal
    {
        IData microData;

        /// <summary>
        /// This represents the red part in the Equation 7 (On evaluating consensus in RANSAC surface registration)
        /// </summary>
        double precomputedScalarValue;

        /// <summary>
        /// This represents the diag marked with pink color as qi
        /// </summary>
        Vector<double> precomputedDiagonalVector;

        public TransformationDistanceFinal(IData microData)
        {
            this.microData = microData;
            PrecomputeValues();
        }

        private void PrecomputeValues()
        {
            int sizeX = microData.Measures[0];
            int sizeY = microData.Measures[1];
            int sizeZ = microData.Measures[2];

            precomputedScalarValue = 0;
            precomputedDiagonalVector = Vector<double>.Build.Dense(3);

            for (double x = 0; x <= sizeX; x += microData.XSpacing)
            {
                for (double y = 0; y <= sizeY; y += microData.YSpacing)
                 {
                    for (double z = 0; z <= sizeZ; z += microData.ZSpacing)
                    {

                        double currentX = Math.Pow(x, 2);
                        double currentY = Math.Pow(y, 2);
                        double currentZ = Math.Pow(z, 2);

                        precomputedDiagonalVector[0] += currentX;
                        precomputedDiagonalVector[1] += currentY;
                        precomputedDiagonalVector[2] += currentZ;

                        precomputedScalarValue += currentX + currentY + currentZ;
                    }
                }
            }
        }

        public double GetTransformationsDistances(Transform3D transformation1, Transform3D transformation2, IData microData)
        {
            Vector<double> centerPoint = Vector<double>.Build.DenseOfArray(new double[] { microData.Measures[0] / 2.0, microData.Measures[1] / 2.0, microData.Measures[2] / 2.0 });

            Matrix<double> rotationMatrix1 = transformation1.RotationMatrix;
            Vector<double> translationVector1 = transformation1.TranslationVector;

            //Now I will apply rotation and translation to the center
            Vector<double> shiftedCenterPoint1 = rotationMatrix1 * centerPoint;
            shiftedCenterPoint1 += translationVector1;
            //Subtract the vector from origin to center the object after the transformation
            translationVector1 -= shiftedCenterPoint1;


            Matrix<double> rotationMatrix2 = transformation2.RotationMatrix;
            Vector<double> translationVector2 = transformation2.TranslationVector;

            //Now I will apply rotation and translation to the center
            Vector<double> shiftedCenterPoint2 = rotationMatrix2 * centerPoint;
            shiftedCenterPoint2 += translationVector2;
            //Subtract the vector from origin to center the object after the transformation
            translationVector2 -= shiftedCenterPoint2;


            int sizeX = (int)(microData.Measures[0] / microData.XSpacing) + 1; //+1 because origin point needs to be taken into consideration in either of the sizes
            int sizeY = (int)(microData.Measures[1] / microData.YSpacing);
            int sizeZ = (int)(microData.Measures[2] / microData.ZSpacing);

            int numberOfVertices = sizeX * sizeY * sizeZ;

            /*
            Matrix<double> firstMultiplicationMatrix = translationVector1.Multiply(numberOfVertices).ToRowMatrix() * translationVector1.ToColumnMatrix();
            Matrix<double> secondMethod = translationVector1.ToRowMatrix() * translationVector1.ToColumnMatrix();
            secondMethod.Multiply(numberOfVertices);
            */


            double firstMultiplication = numberOfVertices * MultiplyVectors(translationVector1, translationVector1);
            double secondMultiplication = -2 * numberOfVertices * MultiplyVectors(translationVector1, translationVector2);
            double thirdMultiplication = numberOfVertices * MultiplyVectors(translationVector2, translationVector2);

            double translationCalculations1 = firstMultiplication + secondMultiplication + thirdMultiplication;

            double translationCalculations2 = numberOfVertices * (MultiplyVectors(translationVector1, translationVector1) - 2 * MultiplyVectors(translationVector1, translationVector2) + MultiplyVectors(translationVector2, translationVector2));

            

            //double secondMultiplication = (translationVector1.ToRowMatrix() * translationVector2.ToColumnMatrix())[0,0];
            //double thirdMultiplication = (translationVector2.ToRowMatrix() * translationVector2.ToColumnMatrix())[0, 0];
            Vector<double> rotationMatricesDiagVector = MultiplyRotationMatrices(rotationMatrix1, rotationMatrix2);

            //double sum = numberOfVertices * firstMultiplication - 2 * numberOfVertices * secondMultiplication + numberOfVertices * thirdMultiplication - (diagVector1.Multiply(2)).DotProduct(precomputedDiagonalVector);

            //sum = sum;
            //sum += precomputedScalarValue;

            double sum = 2*precomputedScalarValue + firstMultiplication + secondMultiplication + thirdMultiplication - 2* rotationMatricesDiagVector.DotProduct(precomputedDiagonalVector);

            /*
            Console.WriteLine("This is the translation calculation 1: " + translationCalculations1);
            Console.WriteLine("This is the translation calculation 2: " + translationCalculations2);

            Console.WriteLine("This is the difference: " + Math.Abs(translationCalculations1 - translationCalculations2));
            */
            //double desiredVq = desiredResult - 2 * precomputedScalarValue + 2 * rotationMatricesDiagVector.DotProduct(precomputedDiagonalVector);
            //desiredVq /= (MultiplyVectors(translationVector1, translationVector1) - 2 * MultiplyVectors(translationVector1, translationVector2) + MultiplyVectors(translationVector2, translationVector2));

            return sum;
        }

        /// <summary>
        /// This method takes in two vectors and returns result of expression firstVector^T (transposed) * secondVector
        /// </summary>
        /// <param name="firstVector">First vector</param>
        /// <param name="secondVector">Second vector</param>
        /// <returns>Returns result of expression: firstVector^T (transposed) * secondVector</returns>
        private double MultiplyVectors(Vector<double> firstVector, Vector<double> secondVector)
        {
            if (firstVector.Count != secondVector.Count)
                throw new ArgumentException("Vectors are required to be the same size.");

            double result = 0;
            for (int i = 0; i < firstVector.Count; i++)
                result += (firstVector[i] * secondVector[i]);

            return result;
        }

        /// <summary>
        /// This method multiplies the matrices like R1^T (transposed) * R2 and returns only their diagonal elements in a form of Vector<double>
        /// </summary>
        /// <param name="rotationMatrix1">Rotation matrix 1</param>
        /// <param name="rotationMatrix2">Rotation matrix 2</param>
        /// <returns></returns>
        private Vector<double> MultiplyRotationMatrices(Matrix<double> rotationMatrix1, Matrix<double> rotationMatrix2)
        {
            if (rotationMatrix1.ColumnCount != rotationMatrix1.RowCount)
                throw new ArgumentException("Rotation matrix 1 should be of size n x n.");

            if (rotationMatrix2.ColumnCount != rotationMatrix2.RowCount)
                throw new ArgumentException("Rotation matrix 2 should be of size n x n.");

            if (rotationMatrix1.ColumnCount != rotationMatrix1.ColumnCount)
                throw new ArgumentException("Rotation matrices should have the sime dimensions.");


            Vector<double> resultVector = Vector<double>.Build.Dense(rotationMatrix1.RowCount);

            for(int i = 0; i<rotationMatrix1.RowCount; i++)
            {
                for (int j = 0; j < rotationMatrix1.RowCount; j++)
                {
                    resultVector[i] += rotationMatrix1[j, i] * rotationMatrix2[j, i];
                }
            }

            return resultVector;
        }
    }
}

