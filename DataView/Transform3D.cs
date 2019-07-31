using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace DataView
{
    class Transform3D : ITransformer
    {

        Matrix<double> changeOfBasisMatrixFromA1toA2;

        Matrix<double> A1;
        Matrix<double> A2;

        public Transform3D GetTransformation(Match m, VolumetricData d1, VolumetricData d2)
        {
            int count = 10000000;

            A1 = getSymetricMatrixForEigenVectors(d1, count);
            var evd1 = A1.Evd();//eigenvalues for d1

            A2 = getSymetricMatrixForEigenVectors(d2, count);
            var evd2 = A2.Evd();//eigenvalues for d2

            //var svd = A2.Svd();
            
            changeOfBasisMatrixFromA1toA2 = ComputeChangeOfBasisMatrix(evd1.EigenVectors, evd2.EigenVectors); //eigenvectors make up an orthogonal basis 
            //TODO: potrebuju pridat translaci, ale nemam tuseni jak ji zjistit...

            return null;
        }

        public void CalculateRotation(VolumetricData d1, VolumetricData d2)
        {
            int count = 1_000_000_0;

            A1 = getSymetricMatrixForEigenVectors(d1, count);
            var evd1 = A1.Evd();//eigenvalues for d1

            A2 = getSymetricMatrixForEigenVectors(d2, count);
            var evd2 = A2.Evd();//eigenvalues for d2

            //var svd = A2.Svd();

            changeOfBasisMatrixFromA1toA2 = ComputeChangeOfBasisMatrix(evd1.EigenVectors, evd2.EigenVectors); //eigenvectors make up an orthogonal basis 

            
        }

        /// <summary>
        /// Computes the change of basis matrix for L: base1 -> base2
        /// 
        /// change of basis matrix A = (a1, a2, a3)
        /// a1 = base1^-1 * u1
        /// 
        /// [v]w = A * [v]u
        /// 
        /// https://eli.thegreenplace.net/2015/change-of-basis-in-linear-algebra/
        /// </summary>
        /// <param name="base1"></param>
        /// <param name="base2"></param>
        /// <returns></returns>
        private Matrix<double> ComputeChangeOfBasisMatrix(Matrix<double> base1, Matrix<double> base2)
        {
            Matrix<double> base2Inverse = base2.Inverse();

            Vector<double> u1 = base1.Column(0);
            Vector<double> a1 = base2Inverse.Multiply(u1);

            Vector<double> u2 = base1.Column(1);
            Vector<double> a2 = base2Inverse.Multiply(u2);

            Vector<double> u3 = base1.Column(2);
            Vector<double> a3 = base2Inverse.Multiply(u3);

            Matrix<double> changeOfBasisMatrix = Matrix<double>.Build.DenseOfColumnVectors(a1, a2, a3);

            return changeOfBasisMatrix;
        }



        /// <summary>
        /// Samples the volumetric data, 
        /// computes matrix D* = samples - average sample
        /// returns D* times transpose(D*)
        /// returned matrix is symetric and its eigenvectors are orthogonal
        /// </summary>
        /// <param name="d"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private Matrix<double> getSymetricMatrixForEigenVectors(VolumetricData d, int count)
        {
            ISampler sampler = new Sampler();
            Point3D[] sample1 = sampler.Sample(d, count);
            Matrix<double> sampleMatrix = Point3DArrayToMatrix(sample1); //matrix D
            double[] averageCoordinates = getAverageCoordinate(sampleMatrix); // vector overline{x}
            Matrix<double> sampleMatrix1Subtracted = subtractVectorFromMatrix(sampleMatrix, averageCoordinates); //matrix D*
            Matrix<double> A = sampleMatrix1Subtracted.TransposeAndMultiply(sampleMatrix1Subtracted); // D* times transpose(D*)
            return A;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m">3*n matrix</param>
        /// <param name="v">vector of size 3</param>
        /// <returns></returns>
        private Matrix<double> subtractVectorFromMatrix(Matrix<double> m, double[] v)
        {

            Matrix<double> output = Matrix<double>.Build.Dense(3, m.ColumnCount);
            for(int i = 0; i < m.ColumnCount; i++)
            {
                output[0, i] = m[0, i] - v[0];  //newX1 = x1 - overline{x}
                output[1, i] = m[1, i] - v[1];  
                output[2, i] = m[2, i] - v[2];
            }
            return output;
        }

        /// <summary>
        /// Converts array of Point3Ds to matrix of dimensions 3*points.length
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private Matrix<double> Point3DArrayToMatrix(Point3D[] points)
        {
            Matrix<double> m = Matrix<double>.Build.Dense(3, points.Length);
            for(int i = 0; i < points.Length; i++)
            {
                m[0, i] = points[i].x;
                m[1, i] = points[i].y;
                m[2, i] = points[i].z;
            }

            return m;
        }
        /// <summary>
        /// Computes a vector v of size 3 where v[0] = average(m[0,0], m[0,1], m[0,2], ..., m[0,m.CollumCount - 1])
        /// </summary>
        /// <param name="m">3 * n  matrix</param>
        /// <returns></returns>
        private double[] getAverageCoordinate(Matrix<double> m)
        {
            double[] averages = new double[3];
            double[] sums = new double[3];
            for(int i = 0; i < m.ColumnCount; i++)
            {
                sums[0] += m[0, i];
                sums[1] += m[1, i];
                sums[2] += m[2, i];
            }
            averages[0] = sums[0] / m.ColumnCount;
            averages[1] = sums[1] / m.ColumnCount;
            averages[2] = sums[2] / m.ColumnCount;
            return averages;
        }
    }
}
