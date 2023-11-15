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
    /// vq*t1^T*t1 - 2*vq*t1^T*t2 + vq*t2^T*t2 - black part
    /// - 2*R1^T*R2 : Σ(from i = 1 to vq)qi*qi^T
    /// </summary>
    class TransformationDistanceSix : ITransformationDistance
	{
        private int numberOfVertices = 0;
        Matrix<double> vertexSumMatrix = Matrix<double>.Build.Dense(3, 3);

        double scalarVectorSum = 0;

        Vector<double> centerPoint;

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


        public TransformationDistanceSix(IData microData)
        {
            int sizeX = microData.Measures[0];
            int sizeY = microData.Measures[1];
            int sizeZ = microData.Measures[2];

            numberOfVertices = (int)(sizeX / microData.XSpacing * sizeY/microData.YSpacing * sizeZ/microData.ZSpacing);

            centerPoint = Vector<double>.Build.DenseOfArray(new double[]
            {
                (sizeX - microData.XSpacing) / 2.0,
                (sizeY - microData.YSpacing) / 2.0,
                (sizeZ - microData.ZSpacing) / 2.0
            });


            for (double x = 0; x < sizeX; x += microData.XSpacing)
            {
                for (double y = 0; y < sizeY; y += microData.YSpacing)
                {
                    for (double z = 0; z < sizeZ; z += microData.ZSpacing)
                    {
                        Vector<double> currentVertex = Vector<double>.Build.DenseOfArray(new double[] { x, y, z });
                        currentVertex -= centerPoint;

                        scalarVectorSum += MultiplyVectorsScalar(currentVertex, currentVertex);
                        vertexSumMatrix += MultiplyVerticesMatrix(currentVertex, currentVertex);
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the matrix by checking if R^T = R^(-1)
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private bool IsRotationMatrix(Matrix<double> matrix)
        {
            if (matrix.RowCount != matrix.ColumnCount)
                return false;

            Matrix<double> evaluationMatrix = matrix.Transpose() - matrix.Inverse();
            double result = 0;

            for(int i = 0; i<evaluationMatrix.RowCount; i++)
            {
                for(int j = 0; j<evaluationMatrix.ColumnCount; j++)
                {
                    result += evaluationMatrix[i, j];
                }
            }

            if (result < 0.00001)
                return true;

            Console.WriteLine("Passed matrix isnt rotation matrix.");
            return false;
        }

        public double GetTransformationsDistance(Transform3D transformation1, Transform3D transformation2, IData microData)
        {
            Console.WriteLine("This is the limit for X values: " + microData.Measures[0]);
            Console.WriteLine("This is the limit for Y values: " + microData.Measures[1]);
            Console.WriteLine("This is the limit for Z values: " + microData.Measures[2]);

            Matrix<double> rotationMatrix1 = transformation1.RotationMatrix;
            Matrix<double> rotationMatrix2 = transformation2.RotationMatrix;
            if (!IsRotationMatrix(rotationMatrix1) || !IsRotationMatrix(rotationMatrix2))
            {
                throw new ArgumentException("Matrix isnt rotation matrix");
            }

            Vector<double> translationVector1 = transformation1.TranslationVector;
            Vector<double> translationVector2 = transformation2.TranslationVector;

            //Apply the transformation to centerPoint
            /*
            Vector<double> endPosition1 = rotationMatrix1.Multiply(centerPoint);
            Vector<double> endPosition2 = rotationMatrix2.Multiply(centerPoint);

            */

            //The centroid of the object was translated to the origin, thus the translation
            //relative to the current state is the opposite of the translation vector applied to
            //shift centroid to origin rotated using the rotation matrix
            translationVector1 += rotationMatrix1.Multiply(centerPoint);
            translationVector2 += rotationMatrix2.Multiply(centerPoint);


            double redPart = calculateRedPart(microData);
            double blackPart = calculateBlackPart(translationVector1, translationVector2);
            double pinkPart = calculatePinkPart(rotationMatrix1, rotationMatrix2);

            double result = redPart;
            result += blackPart;
            result += pinkPart;

            return result;
        }

        private double calculatePinkPart(Matrix<double> rotationMatrix1, Matrix<double> rotationMatrix2)
        {
            Matrix<double> leftMatrix = -2 * rotationMatrix1.Transpose() * rotationMatrix2;
            Matrix<double> rightMatrix = vertexSumMatrix;

            return calculateFrobeniusMatrixProduct(leftMatrix, rightMatrix);
        }


        private double  calculateRedPart(IData microData)
        {
            return 2*scalarVectorSum;
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

