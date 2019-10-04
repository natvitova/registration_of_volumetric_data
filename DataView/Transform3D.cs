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

        Matrix<double> changeOfBasisMatrix;

        Matrix<double> A1;
        Matrix<double> A2;

        public Transform3D GetTransformation(Match m, VolumetricData d1, VolumetricData d2)
        {

            Point3D a = null; //match.point1
            Point3D b = null; //match.point2
            changeOfBasisMatrix = CalculateRotation(d1, d2, a, b);

            return null;
        }

        /// <summary>
        /// Overload for testing purposes
        /// </summary>
        /// <param name="m"></param>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public Transform3D GetTransformation(Point3D a, Point3D b, VolumetricData d1, VolumetricData d2)
        {

           
            changeOfBasisMatrix = CalculateRotation(d1, d2, a, b);

            return null;
        }
        /// <summary>
        /// Calculates the rotation matrix about a point
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public Matrix<double> CalculateRotation(VolumetricData d1, VolumetricData d2, Point3D point1, Point3D point2)
        {
            int count = 1000;

            A1 = getSymetricMatrixForEigenVectors(d1, point1, count);
            var evd1 = A1.Evd();//eigenvalues for d1

            A2 = getSymetricMatrixForEigenVectors(d2, point2, count);
            var evd2 = A2.Evd();//eigenvalues for d2

            //var svd = A2.Svd();

            Matrix<double> rotationMatrix = ComputeChangeOfBasisMatrixUsingTransposition(evd1.EigenVectors, evd2.EigenVectors); //eigenvectors make up an orthonormal basis    
            Console.WriteLine("Change of basis matrix:");
            Console.WriteLine(rotationMatrix.ToString());

            TestChangeOfBasisMatrixCorrectness(rotationMatrix, evd1.EigenVectors, evd2.EigenVectors);

            return rotationMatrix;

        }

        private void TestChangeOfBasisMatrixCorrectness(Matrix<double> transformMatrix, Matrix<double> base1, Matrix<double> base2)
        {
            Vector<double> v1t = transformMatrix.Multiply(base1.Column(0));
            Vector<double> v2t = transformMatrix.Multiply(base1.Column(1));
            Vector<double> v3t = transformMatrix.Multiply(base1.Column(2));
            Console.Write("v1: " + base2.Column(0));
            Console.WriteLine("v1t: " + v1t.ToString());

            Console.Write("v2: " + base2.Column(1));
            Console.WriteLine("v2t: " + v2t.ToString());

            Console.Write("v3: " + base2.Column(2));
            Console.WriteLine("v3t: " + v3t.ToString());


            return;
        }

        /// <summary>
        /// PCA works as intended
        /// </summary>
        public void testPCA()
        {
            PCATester pcatester = new PCATester(1000);
            Matrix<double> baseMatrix = getSymetricMatrixForEigenVectors(pcatester.Qs);
            var evd = baseMatrix.Evd();
            Matrix<double> output = evd.EigenVectors;

            Console.WriteLine("Input vectors:");
            Console.WriteLine(String.Format("{0} {1} {2}", pcatester.v1[0], pcatester.v2[0], pcatester.v3[0]));
            Console.WriteLine(String.Format("{0} {1} {2}", pcatester.v1[1], pcatester.v2[1], pcatester.v3[1]));
            Console.WriteLine(String.Format("{0} {1} {2}", pcatester.v1[2], pcatester.v2[2], pcatester.v3[2]));
      
           

            Console.WriteLine("PCA output:");
            Console.WriteLine(output.ToMatrixString());
        }

        

        /// <summary>
        /// if v1 x v2 != v3
        /// then v3 = -v3
        /// </summary>
        /// <param name="matrix"></param>
        private void MakeBaseRightHanded(Matrix<double> matrix)
        {
            if (IsBaseRightHanded(matrix))
                return;
            matrix.SetRow(2, matrix.Row(2).Multiply(-1));
            
        }

        /// <summary>
        /// Tests whether v1 x v2 = v3
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private bool IsBaseRightHanded(Matrix<double> matrix)
        {
            double epsilon = 0.000001;
            Vector<double> cross = CrossProduct(matrix.Column(0), matrix.Column(1));

            if ((Math.Abs(cross[0] - matrix.Column(2)[0]) < epsilon) && (Math.Abs(cross[1] - matrix.Column(2)[1]) < epsilon) && (Math.Abs(cross[2] - matrix.Column(2)[2]) < epsilon)){
                return true;
            }
            return false;
        }
        

        /// <summary>
        /// ForTesting
        /// Computes the crossProduct of vectors size 3
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector<double> CrossProduct(Vector<double> u, Vector<double> v)
        {

            Vector<double> w = Vector<double>.Build.Dense(3);
            /*
            uvi = u2 * v3 - v2 * u3;

            uvj = v1 * u3 - u1 * v3;

            uvk = u1 * v2 - v1 * u2;
            */
            w[0] = u[1] * v[2] - v[1] * u[2];
            w[1] = v[0] * u[2] - u[0] * v[2];
            w[2] = u[0] * v[1] - v[0] * u[1];
            return w;
        }

        /// <summary>
        /// ForTesting
        /// Sets the norm of column vectors in a matrix to 1
        /// </summary>
        /// <param name="matrix"></param>
        private void NormalizeCollumnVectorsInMatrix(Matrix<double> matrix)
        {

            for(int i = 0; i < matrix.ColumnCount; i++)
            {
                Vector<double> v = matrix.Column(i);
                v.Divide(Math.Sqrt(PCATester.ScalarProduct(v, v)));
                matrix.SetColumn(i, v);
            }
            
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
            //NormalizeCollumnVectorsInMatrix(base1); //base1 and base2 are already orthonormal
            //NormalizeCollumnVectorsInMatrix(base2);


            MakeBaseRightHanded(base1);
            MakeBaseRightHanded(base2);

            if (!IsBaseRightHanded(base1))
                Console.WriteLine("base1 is not righthanded :( ");
            if (!IsBaseRightHanded(base2))
                Console.WriteLine("base2 is not righthanded :( ");
            

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

        private Matrix<double> ComputeChangeOfBasisMatrixUsingTransposition(Matrix<double> base1, Matrix<double> base2)
        {
            //NormalizeCollumnVectorsInMatrix(base1); //base1 and base2 are already orthonormal
            //NormalizeCollumnVectorsInMatrix(base2);


            MakeBaseRightHanded(base1);
            MakeBaseRightHanded(base2);

            if (!IsBaseRightHanded(base1))
                Console.WriteLine("base1 is not righthanded :( ");
            if (!IsBaseRightHanded(base2))
                Console.WriteLine("base2 is not righthanded :( ");




            Matrix<double> changeOfBasisMatrix = base1.Multiply(base2.Transpose());

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
        private Matrix<double> getSymetricMatrixForEigenVectors(VolumetricData d, Point3D point, int count)
        {
            Sampler sampler = new Sampler(); //this is where ISampler used to be 
            Point3D[] sample1 = sampler.SampleSphereAroundPoint(d, point, 50, count);

            Matrix<double> sampleMatrix = Point3DArrayToMatrix(sample1); //matrix D
            double[] averageCoordinates = getAverageCoordinate(sampleMatrix); // vector overline{x}
            Matrix<double> sampleMatrix1Subtracted = subtractVectorFromMatrix(sampleMatrix, averageCoordinates); //matrix D*
            Matrix<double> A = sampleMatrix1Subtracted.TransposeAndMultiply(sampleMatrix1Subtracted); // D* times transpose(D*)
            return A;
        }

        
        private Matrix<double> getSymetricMatrixForEigenVectors(Point3D[] points)
        {

            Matrix<double> sampleMatrix = Point3DArrayToMatrix(points); //matrix D
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
