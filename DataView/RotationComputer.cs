using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace DataView
{
    class RotationComputer
    {
        /// <summary>
        /// Calculates the rotation matrix from d1 to d2
        /// </summary>
        /// <param name="d1">Data1</param>
        /// <param name="d2">Data2</param>
        /// <param name="point1">Point in data1 to take samples around</param>
        /// <param name="point2">Point in data2 to take samples around</param>
        /// <param name="count">Number of samples taken</param>
        /// <returns></returns>
        public static Matrix<double> CalculateRotation(VolumetricData d1, VolumetricData d2, Point3D point1, Point3D point2, int count)
        {
            //Console.WriteLine("Calculating rotation between point {0} : {1} of value {2} in data: {3} and point {4} : {5} of value {6} in data {7} ",
            // nameof(point1), point1.ToString(), d1.GetValueRealCoordinates(point1), nameof(d1), nameof(point2), point2.ToString(), d2.GetValueRealCoordinates(point2), nameof(d2));

            Matrix<double> A1 = GetSymetricMatrixForEigenVectors(d1, point1, count);
            //Console.WriteLine("Matrix {0} : {1}", nameof(A1), A1.ToString());
            var evd1 = A1.Evd(); //eigenvalues for d1

            Matrix<double> A2 = GetSymetricMatrixForEigenVectors(d2, point2, count);
            //Console.WriteLine("Matrix {0} : {1}", nameof(A2), A2.ToString());
            var evd2 = A2.Evd(); //eigenvalues for d2
            //var svd = A2.Svd();

            Matrix<double> rotationMatrix = ComputeChangeOfBasisMatrixUsingTransposition(evd1.EigenVectors, evd2.EigenVectors); //eigenvectors make up an orthonormal basis    
            //Console.WriteLine("Change of basis matrix:");
            //Console.WriteLine(rotationMatrix.ToString());
            //TestChangeOfBasisMatrixCorrectness(rotationMatrix, evd1.EigenVectors, evd2.EigenVectors);

            return rotationMatrix;
        }

        public static Matrix<double> CalculateRotationA(ArtificialData ad, int[] t1, int[] t2, Point3D point1, Point3D point2, int count)
        {
            //Console.WriteLine("Calculating rotation between point {0} : {1} of value {2} in data: {3} and point {4} : {5} of value {6} in data {7} ",
            // nameof(point1), point1.ToString(), d1.GetValueRealCoordinates(point1), nameof(d1), nameof(point2), point2.ToString(), d2.GetValueRealCoordinates(point2), nameof(d2));

            Matrix<double> A1 = GetSymetricMatrixForEigenVectorsA(ad, t1, point1, count);
            //Console.WriteLine("Matrix {0} : {1}", nameof(A1), A1.ToString());
            var evd1 = A1.Evd(); //eigenvalues for d1

            Matrix<double> A2 = GetSymetricMatrixForEigenVectorsA(ad, t2, point2, count);
            //Console.WriteLine("Matrix {0} : {1}", nameof(A2), A2.ToString());
            var evd2 = A2.Evd(); //eigenvalues for d2
                                 //var svd = A2.Svd();


            Matrix<double> rotationMatrix = ComputeChangeOfBasisMatrixUsingTransposition(evd1.EigenVectors, evd2.EigenVectors); //eigenvectors make up an orthonormal basis    
            //Console.WriteLine("Change of basis matrix:");
            //Console.WriteLine(rotationMatrix.ToString());
            //TestChangeOfBasisMatrixCorrectness(rotationMatrix, evd1.EigenVectors, evd2.EigenVectors);

            return rotationMatrix;
        }

        private static void TestChangeOfBasisMatrixCorrectness(Matrix<double> transformMatrix, Matrix<double> base1, Matrix<double> base2)
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
        public void TestPCA()
        {
            PCATester pcatester = new PCATester(1000);
            Matrix<double> baseMatrix = GetSymetricMatrixForEigenVectors(pcatester.Qs);
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
        private static void MakeBaseRightHanded(Matrix<double> matrix)
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
        private static bool IsBaseRightHanded(Matrix<double> matrix)
        {
            double epsilon = 0.000001;
            Vector<double> cross = CrossProduct(matrix.Column(0), matrix.Column(1));

            if ((Math.Abs(cross[0] - matrix.Column(2)[0]) < epsilon) && (Math.Abs(cross[1] - matrix.Column(2)[1]) < epsilon) && (Math.Abs(cross[2] - matrix.Column(2)[2]) < epsilon))
            {
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
        private static void NormalizeCollumnVectorsInMatrix(Matrix<double> matrix)
        {
            for (int i = 0; i < matrix.ColumnCount; i++)
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
        private static Matrix<double> ComputeChangeOfBasisMatrix(Matrix<double> base1, Matrix<double> base2)
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

        private static Matrix<double> ComputeChangeOfBasisMatrixUsingTransposition(Matrix<double> base1, Matrix<double> base2)
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
        private static Matrix<double> GetSymetricMatrixForEigenVectors(VolumetricData d, Point3D point, int count)
        {
            Point3D[] sample1 = SampleSphereAroundPoint(d, point, 5, count);

            //for (int i = 0; i < sample1.Length; i++)
            //{
            //    Console.WriteLine("Point : {0} , Value: {1}", sample1[i], d.GetValue(sample1[i]));
            //}

            Matrix<double> sampleMatrix = Point3DArrayToMatrix(sample1); //matrix D
            double[] averageCoordinates = GetAverageCoordinate(sampleMatrix); // vector overline{x}
            //Console.WriteLine("Average coordinates: [{0}, {1}, {2}]", averageCoordinates[0], averageCoordinates[1], averageCoordinates[2]);
            Matrix<double> sampleMatrixSubtracted = SubtractVectorFromMatrix(sampleMatrix, averageCoordinates); //matrix D*
            Matrix<double> A = sampleMatrixSubtracted.TransposeAndMultiply(sampleMatrixSubtracted); // D* times transpose(D*)

            return A;
        }

        private static Matrix<double> GetSymetricMatrixForEigenVectorsA(ArtificialData ad, int[] t, Point3D point, int count)
        {
            Point3D[] sample1 = SampleSphereAroundPointA(ad, t, point, 2, count);

            //for (int i = 0; i < sample1.Length; i++)
            //{
            //    Console.WriteLine("Point : {0} , Value: {1}", sample1[i], d.GetValue(sample1[i]));
            //}

            Matrix<double> sampleMatrix = Point3DArrayToMatrix(sample1); //matrix D
            double[] averageCoordinates = GetAverageCoordinate(sampleMatrix); // vector overline{x}
            //Console.WriteLine("Average coordinates: [{0}, {1}, {2}]", averageCoordinates[0], averageCoordinates[1], averageCoordinates[2]);
            Matrix<double> sampleMatrixSubtracted = SubtractVectorFromMatrix(sampleMatrix, averageCoordinates); //matrix D*
            Matrix<double> A = sampleMatrixSubtracted.TransposeAndMultiply(sampleMatrixSubtracted); // D* times transpose(D*)

            return A;
        }

        private static Point3D[] SampleSphereAroundPoint(VolumetricData d, Point3D centerPoint, int radius, int count)
        {
            List<Point3D> survivingPoints = new List<Point3D>();
            List<Point3D> pointsInSphere = FeatureComputer.GetSphere(centerPoint, radius, 0.5); //gets all points in a given sphere

            Random rnd = new Random();
            int currentValue;
            int maxValue = 0;
            int minValue = int.MaxValue;

            foreach (Point3D point in pointsInSphere) //finds the minumum and maximum value in the sphere
            {
                currentValue = d.GetValueMatrixCoordinates(point);

                if (currentValue > maxValue)
                    maxValue = currentValue;

                if (currentValue < minValue)
                    minValue = currentValue;
            }
            bool NonTrivial = true;
            if (maxValue == minValue)
                NonTrivial = false;
            while (survivingPoints.Count < count && pointsInSphere.Count > 0)
            {
                int rndIndex = rnd.Next(0, pointsInSphere.Count); //random index in pointsInSphere
                if (NonTrivial)
                {
                    if (DecideFate(rnd, d.GetValueMatrixCoordinates(pointsInSphere[rndIndex]), minValue, maxValue)) //decides whether to keep the point or not
                    {
                        //the point is kept
                        survivingPoints.Add(pointsInSphere[rndIndex]); //add to result
                    }
                }
                else
                {
                    if (rnd.NextDouble() > 0.5)
                    {
                        survivingPoints.Add(pointsInSphere[rndIndex]); //add to result
                    }
                }
                pointsInSphere.RemoveAt(rndIndex); //removed from pointsInSphere
            }
            return survivingPoints.ToArray();
        }

        private static Point3D[] SampleSphereAroundPointA(ArtificialData ad, int[] t, Point3D centerPoint, int radius, int count)
        {
            List<Point3D> survivingPoints = new List<Point3D>();
            List<Point3D> pointsInSphere = FeatureComputer.GetSphere(centerPoint, radius, 0.5); //gets all points in a given sphere
            //Console.WriteLine(pointsInSphere.Count);

            Random rnd = new Random(0); // change rnd - index in count - BACHA VADA!!!!!
            Random rn = new Random(0); // change rnd - decide fate
            Random r = new Random(0); // change rnd - trivial
            int currentValue;
            int maxValue = 0;
            int minValue = int.MaxValue;

            foreach (Point3D point in pointsInSphere) //finds the minumum and maximum value in the sphere
            {
                currentValue = (int)ad.Fce.GetValue(point.X + t[0], point.Y + t[1], point.Z + t[2]); //Avalue

                if (currentValue > maxValue)
                    maxValue = currentValue;

                if (currentValue < minValue)
                    minValue = currentValue;
            }
            bool NonTrivial = true;
            if (maxValue == minValue)
                NonTrivial = false;

            int iii = 0;
            while (survivingPoints.Count < count && pointsInSphere.Count > 0)
            {
                int rndIndex = rnd.Next(0, pointsInSphere.Count); //random index in pointsInSphere
                if (iii < 20)
                {
                    //Console.Write(rndIndex + " ");
                    iii++;
                }


                if (NonTrivial)
                {
                    int v = (int)ad.Fce.GetValue(pointsInSphere[rndIndex].X + t[0], pointsInSphere[rndIndex].Y + t[1], pointsInSphere[rndIndex].Z + t[2]); //Avalue
                    if (DecideFate(rn, v, minValue, maxValue)) //decides whether to keep the point or not
                    {
                        //the point is kept
                        survivingPoints.Add(pointsInSphere[rndIndex]); //add to result
                    }
                }
                else
                {
                    if (r.NextDouble() > 0.5)
                    {
                        survivingPoints.Add(pointsInSphere[rndIndex]); //add to result
                    }
                }
                pointsInSphere.RemoveAt(rndIndex); //removed from pointsInSphere
            }
            //Console.WriteLine(survivingPoints.Count);
            return survivingPoints.ToArray();
        }

        private static Matrix<double> GetSymetricMatrixForEigenVectorsN(VolumetricData d, Point3D point, int count) //___________________________________________NATY
        {
            Point3D[] sample1 = SampleSphereAroundPointN(d, point, 5, count);

            Matrix<double> sampleMatrix = Point3DArrayToMatrix(sample1); //matrix D
            double[] averageCoordinates = GetAverageCoordinate(sampleMatrix); // vector overline{x}
            //Console.WriteLine("Average coordinates: [{0}, {1}, {2}]", averageCoordinates[0], averageCoordinates[1], averageCoordinates[2]);
            Matrix<double> sampleMatrixSubtracted = SubtractVectorFromMatrix(sampleMatrix, averageCoordinates); //matrix D*
            Matrix<double> A = sampleMatrixSubtracted.TransposeAndMultiply(sampleMatrixSubtracted); // D* times transpose(D*)

            return A;
        }

        private static Point3D[] SampleSphereAroundPointN(VolumetricData d, Point3D centerPoint, int radius, int count) //______________________________________NATY
        {
            List<Point3D> survivingPoints = new List<Point3D>();
            List<Point3D> pointsInSphere = FeatureComputer.GetSphereN(centerPoint, d, radius, 0.5); //gets all points in a given sphere

            Random rnd = new Random();
            int currentValue;
            int maxValue = 0;
            int minValue = int.MaxValue;

            foreach (Point3D point in pointsInSphere) //finds the minumum and maximum value in the sphere
            {
                Point3D ipoint = new Point3D(point.X / d.XSpacing, point.Y / d.YSpacing, point.Z / d.ZSpacing);
                currentValue = d.GetValueRealCoordinates(ipoint);

                if (currentValue > maxValue)
                    maxValue = currentValue;

                if (currentValue < minValue)
                    minValue = currentValue;
            }
            bool NonTrivial = true;
            if (maxValue == minValue)
                NonTrivial = false;
            while (survivingPoints.Count < count && pointsInSphere.Count > 0)
            {
                int rndIndex = rnd.Next(0, pointsInSphere.Count); //random index in pointsInSphere
                if (NonTrivial)
                {
                    if (DecideFate(rnd, d.GetValueRealCoordinates(pointsInSphere[rndIndex]), minValue, maxValue)) //decides whether to keep the point or not
                    {
                        //the point is kept
                        survivingPoints.Add(pointsInSphere[rndIndex]); //add to result
                    }
                }
                else
                {
                    if (rnd.NextDouble() > 0.5)
                    {
                        survivingPoints.Add(pointsInSphere[rndIndex]); //add to result
                    }
                }
                pointsInSphere.RemoveAt(rndIndex); //removed from pointsInSphere
            }

            return survivingPoints.ToArray();
        }

        private static bool DecideFate(Random rnd, int pointValue, int min, int max)
        {
            int rndValue = rnd.Next(min, max);
            if (pointValue > rndValue)
                return true;
            else
                return false;
        }

        //private static point3d[] samplespherearoundpoint(volumetricdata d, point3d centerpoint, int radius, int count)
        //{

        //    list<point3d> survivingpoints = new list<point3d>();

        //    random r = new random();
        //    point3d tmppoint;
        //    int randomindex;

        //    list<point3d> pointsinsphere = featurecomputer.getsphere(centerpoint, d, radius); //gets all points in a given sphere

        //    int currentvalue;
        //    int maxvalue = 0;
        //    int minvalue = int.maxvalue;
        //    int sumofvalues = 0;
        //    int meanvalue;
        //    int treshold;

        //    foreach (point3d point in pointsinsphere)
        //    {
        //        currentvalue = d.getvalue(point);

        //        if (currentvalue > maxvalue)
        //            maxvalue = currentvalue;

        //        if (currentvalue < minvalue)
        //            minvalue = currentvalue;

        //        //sumofvalues += currentvalue;
        //    }

        //    //todo tohle cely predelej
        //    treshold = (int)(maxvalue * 0.999);
        //    int i = 0;
        //    //meanvalue = sumofvalues / pointsinsphere.count;
        //    for (int j = 0; j < count && i < pointsinsphere.count;){//iterates the final points

        //        for (i = i; i < pointsinsphere.count; i++)//iterates the points in sphere
        //        {
        //            int randomnumber = r.next(minvalue, maxvalue); //random number between min and max
        //            //int randomnumber = minvalue;

        //            if (d.getvalue(pointsinsphere[i]) > treshold) //high values have a high chance of survival
        //            {
        //                survivingpoints.add(pointsinsphere[i]);
        //                j++;
        //                if (j >= count)
        //                    break;
        //            }

        //        }
        //    }

        //    return survivingpoints.toarray();
        //}

        /// <summary>
        /// FUJKY METODA NEPOUZIVEJ
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private static Matrix<double> GetSymetricMatrixForEigenVectors(Point3D[] points)
        {

            Matrix<double> sampleMatrix = Point3DArrayToMatrix(points); //matrix D
            double[] averageCoordinates = GetAverageCoordinate(sampleMatrix); // vector overline{x}
            Matrix<double> sampleMatrix1Subtracted = SubtractVectorFromMatrix(sampleMatrix, averageCoordinates); //matrix D*
            Matrix<double> A = sampleMatrix1Subtracted.TransposeAndMultiply(sampleMatrix1Subtracted); // D* times transpose(D*)
            return A;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m">3*n matrix</param>
        /// <param name="v">vector of size 3</param>
        /// <returns></returns>
        private static Matrix<double> SubtractVectorFromMatrix(Matrix<double> m, double[] v)
        {
            Matrix<double> output = Matrix<double>.Build.Dense(3, m.ColumnCount);
            for (int i = 0; i < m.ColumnCount; i++)
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
        private static Matrix<double> Point3DArrayToMatrix(Point3D[] points)
        {
            Matrix<double> m = Matrix<double>.Build.Dense(3, points.Length);
            for (int i = 0; i < points.Length; i++)
            {
                m[0, i] = points[i].X;
                m[1, i] = points[i].Y;
                m[2, i] = points[i].Z;
            }

            return m;
        }
        /// <summary>
        /// Computes a vector v of size 3 where v[0] = average(m[0,0], m[0,1], m[0,2], ..., m[0,m.CollumCount - 1])
        /// </summary>
        /// <param name="m">3 * n  matrix</param>
        /// <returns></returns>
        private static double[] GetAverageCoordinate(Matrix<double> m)
        {
            double[] averages = new double[3];
            double[] sums = new double[3];
            for (int i = 0; i < m.ColumnCount; i++)
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