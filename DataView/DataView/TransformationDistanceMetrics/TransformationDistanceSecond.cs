using System;
using MathNet.Numerics.LinearAlgebra;

namespace DataView
{
    /// <summary>
    /// This class calculates the transformation distance based on the second equation from article "On evaluating consensus in RANSAC surface registration"
    /// Equation: d(T1, T2) = Σ(from i = 1 to vq) (R1 * qi + t1 - R2 * qi - t2)^T * (R1 * qi + t1 - R2 * qi - t2)
    /// These differences are squared, summed and finally returned as a result
    /// This class is just for testing purposes and should work as a refference method for checking results
    /// Outputs the same results as the first method - as expected
    /// </summary>
    class TransformationDistanceSecond : ITransformationDistance
    {
        public double GetTransformationsDistance(Transform3D transformation1, Transform3D transformation2, IData microData)
        {
            Matrix<double> rotationMatrix1 = transformation1.RotationMatrix;
            Vector<double> translationVector1 = transformation1.TranslationVector;

            Matrix<double> rotationMatrix2 = transformation2.RotationMatrix;
            Vector<double> translationVector2 = transformation2.TranslationVector;

            double sum = 0;

            int sizeX = microData.Measures[0];
            int sizeY = microData.Measures[1];
            int sizeZ = microData.Measures[2];

            for (double x = 0; x < sizeX; x += microData.XSpacing)
            {
                for (double y = 0; y < sizeY; y += microData.YSpacing)
                {
                    for (double z = 0; z < sizeZ; z += microData.ZSpacing)
                    {
                        Vector<double> originalVector = Vector<double>.Build.DenseOfArray(new double[] { x, y, z });

                        Matrix<double> calculation = (rotationMatrix1.Multiply(originalVector) + translationVector1 - rotationMatrix2.Multiply(originalVector) - translationVector2).ToColumnMatrix();
                        Matrix<double> result = calculation.Transpose() * calculation;

                        sum += result[0,0];
                    }
                }
            }
            return sum;
        }
    }
}

