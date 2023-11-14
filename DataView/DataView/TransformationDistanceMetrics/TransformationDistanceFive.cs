using System;
using MathNet.Numerics.LinearAlgebra;

namespace DataView
{

    /// <summary>
    /// This class calculates the transformation distance based on the equation marked with number five from article "On evaluating consensus in RANSAC surface registration"
    /// Equation: d(T1, T2) = 2*Σ(from i = 1 to vq) (qi^T * qi) + 2*(t1 - t2)^T * R1 * Σ(from i = 1 to vq) qi + 2*(t2 - t1)^T * R2 * Σ(from i = 1 to vq) qi + vq*t1^T*t1 - 2*vq*t1^T*t2 + vq*t2^T*t2 - 2*R1^T*R2 : Σ(from i = 1 to vq)qi*qi^T
    /// where ":" denotes frobenius matrix product

    /// This class is supposed to output the same result as the first two methods, but it doesn't
    /// 2*Σ(from i = 1 to vq) (qi^T * qi) - red part
    /// 2*(t1 - t2)^T * R1 * Σ(from i = 1 to vq) qi - green part
    /// 2*(t2 - t1)^T * R2 * Σ(from i = 1 to vq) qi - blue part
    /// vq*t1^T*t1 - 2*vq*t1^T*t2 + vq*t2^T*t2 - black part
    /// - 2*R1^T*R2 : Σ(from i = 1 to vq)qi*qi^T
    /// 
    /// 
    /// </summary>
    class TransformationDistanceFive : ITransformationDistance
	{
        private int numberOfVertices = 0;
        Matrix<double> vertexSumMatrix = Matrix<double>.Build.Dense(3, 3);

        Vector<double> vectorCoordinatesSum = Vector<double>.Build.Dense(3);

        private double MultiplyVectorsScalar(Vector<double> vector1, Vector<double> vector2)
        {
            if (vector1.Count != vector2.Count)
                throw new ArgumentException("Vectors need to be the same size");

            double sum = 0;

            for (int i = 0; i < vector1.Count; i++)
                sum += vector1[i] * vector2[i];

            return sum;
        }

        private Matrix<double> MultiplyVerticesMatrix(Vector<double> vector1, Vector<double> vector2)
        {
            if (vector1.Count != vector2.Count)
                throw new ArgumentException("Vectors need to be the same size");

            Matrix<double> resultMatrix = vector1.ToColumnMatrix() * vector2.ToRowMatrix();
            return resultMatrix;
        }

        public double GetTransformationsDistance(Transform3D transformation1, Transform3D transformation2, IData microData)
        {
            Matrix<double> rotationMatrix1 = transformation1.RotationMatrix;
            Matrix<double> rotationMatrix2 = transformation2.RotationMatrix;

            Vector<double> translationVector1 = transformation1.TranslationVector;
            Vector<double> translationVector2 = transformation2.TranslationVector;

            Vector<double> centerObject = Vector<double>.Build.DenseOfArray(new double[] { microData.Measures[0] / 2.0, microData.Measures[1] / 2.0, microData.Measures[2] / 2.0 });
            /*
            translationVector1 -= centerObject;
            translationVector2 -= centerObject;
            */

            double redPart = calculateRedPart(microData);
            double greenPart = calculateGreenPart(translationVector1, translationVector2, rotationMatrix1);
            double bluePart = calculateBluePart(translationVector1, translationVector2, rotationMatrix2);
            double blackPart = calculateBlackPart(translationVector1, translationVector2);
            double pinkPart = calculatePinkPart(rotationMatrix1, rotationMatrix2);

            double result = redPart;
            result += greenPart;
            result += bluePart;
            result += blackPart;
            result += pinkPart;

            return result;
        }

        private double calculatePinkPart(Matrix<double> rotationMatrix1, Matrix<double> rotationMatrix2)
        {

            Console.WriteLine("This is rotation matrix 1: " + rotationMatrix1.Transpose());
            Console.WriteLine("This is rotation matrix 2: " + rotationMatrix2);

            Matrix<double> leftMatrix = -2 * rotationMatrix1.Transpose() * rotationMatrix2;

            Matrix<double> rightMatrix = vertexSumMatrix;

            return calculateFrobeniusMatrixProduct(leftMatrix, rightMatrix);
        }


        private double  calculateRedPart(IData microData)
        {
            int sizeX = microData.Measures[0];
            int sizeY = microData.Measures[1];
            int sizeZ = microData.Measures[2];

            double sum = 0;

            for(double x = 0; x<=sizeX; x += microData.XSpacing)
            {
                for (double y = 0; y <= sizeY; y += microData.YSpacing)
                {
                    for (double z = 0; z <= sizeZ; z += microData.ZSpacing)
                    {
                        Vector<double> currentVertex = Vector<double>.Build.DenseOfArray(new double[] { x, y, z });
                        sum += MultiplyVectorsScalar(currentVertex, currentVertex);
                        numberOfVertices++;
                        vectorCoordinatesSum += currentVertex;
                        vertexSumMatrix += MultiplyVerticesMatrix(currentVertex, currentVertex);
                    }
                }
            }

            return 2*sum;
        }

        private double calculateGreenPart(Vector<double> translationVector1, Vector<double> translationVector2, Matrix<double> rotationMatrix1)
        {

            Matrix<double> matrix = 2 * (translationVector1 - translationVector2).ToRowMatrix() * rotationMatrix1 * vectorCoordinatesSum.ToColumnMatrix();
            return matrix[0, 0];
        }

        private double calculateBluePart(Vector<double> translationVector1, Vector<double> translationVector2, Matrix<double> rotationMatrix2)
        {
            Matrix<double> matrix = 2 * (translationVector2 - translationVector1).ToRowMatrix() * rotationMatrix2 * vectorCoordinatesSum.ToColumnMatrix();
            return matrix[0, 0];
        }

        private double calculateBlackPart(Vector<double> translationVector1, Vector<double> translationVector2)
        {
            double result = (numberOfVertices * MultiplyVectorsScalar(translationVector1, translationVector1));
            result += (-2 * numberOfVertices * MultiplyVectorsScalar(translationVector1, translationVector2));
            result += (numberOfVertices * MultiplyVectorsScalar(translationVector2, translationVector2));
            return result;
        }

        private double calculateFrobeniusMatrixProduct(Matrix<double> matrix1, Matrix<double> matrix2)
        {
            if (matrix1.ColumnCount != matrix2.ColumnCount)
                throw new ArgumentException("Matrices need to have the same number of columns");
            if (matrix1.RowCount != matrix2.RowCount)
                throw new ArgumentException("Matrices need to have the same number of rows");

            
            double sum = 0;
            for (int i = 0; i < matrix1.RowCount; i++)
            {
                for(int j = 0; j<matrix1.ColumnCount; j++)
                {
                    sum += (matrix1[i, j] * matrix2[i, j]);
                }
            }

            return sum;
        }
    }
}

