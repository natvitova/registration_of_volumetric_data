using System;
using MathNet.Numerics.LinearAlgebra;

namespace DataView
{
    /// <summary>
    /// This class calculates the transformation distance based on the first equation from article "On evaluating consensus in RANSAC surface registration" - applies transformations to all the vertices and calculates the difference
    /// These differences are squared, summed and finally returned as a result
    /// This class is just for testing purposes and should work as a refference method for checking results
    /// </summary>
	class TransformationDistanceFirst : ITransformationDistance
	{
        public double GetTransformationsSecond(Transform3D transformation1, Transform3D transformation2, IData micro)
        {

            Matrix<double> rotationMatrix1 = transformation1.RotationMatrix;
            Vector<double> translationVector1 = transformation1.TranslationVector;

            Matrix<double> rotationMatrix2 = transformation2.RotationMatrix;
            Vector<double> translationVector2 = transformation2.TranslationVector;

            double sum = 0;

            int sizeX = micro.Measures[0];
            int sizeY = micro.Measures[1];
            int sizeZ = micro.Measures[2];

            for(double x = 0; x<=sizeX; x+=micro.XSpacing)
            {
                for (double y = 0; y < sizeY; y+= micro.YSpacing)
                {
                    for (double z = 0; z < sizeZ; z += micro.ZSpacing)
                    {
                        Vector<double> originalVector = Vector<double>.Build.DenseOfArray(new double[] { x, y, z });

                        Vector<double> resultVector1 = rotationMatrix1.Multiply(originalVector);
                        resultVector1 += translationVector1;

                        Vector<double> resultVector2 = rotationMatrix2.Multiply(originalVector);
                        resultVector2 += translationVector2;

                        sum += Math.Pow((resultVector1-resultVector2).L2Norm(), 2);
                    }
                }
            }
            return sum;
        }
    }
}

