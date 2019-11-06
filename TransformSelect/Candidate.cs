using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Framework;

namespace DataView
{
    public class Candidate
    {
        // properties of Q        
        static double[] tpSum; // sum of tensor products, diagonal only
        static double dpsum; // sum of dot products
        static double n; // number of points
        static double invn; // inverse number of points        
        static MathNet.Numerics.LinearAlgebra.Vector<double> t0;

        double[] rot; // rotation matrix
        double[] t; // translation

        /// <summary>
        /// initializes rotation and translation from dual quaternion representation
        /// </summary>
        public void initRT()
        {
            this.rot = new double[9];
            this.t = new double[3];
        }

        public static void initSums(double xSize, double ySize, double zSize)
        {
            t0 = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense(new double[] { xSize / 2, ySize / 2, zSize / 2 });
            tpSum = new double[3]; // we only need the diagonal 
            tpSum[0] = xSize * xSize * xSize * ySize * zSize / 12;
            tpSum[1] = xSize * ySize * ySize * ySize * zSize / 12;
            tpSum[2] = xSize * ySize * zSize * zSize * zSize / 12;
            dpsum = tpSum[0] + tpSum[1] + tpSum[2];            
            n = xSize*ySize*zSize;
            invn = 1.0 / n;
            Density.radius = Math.Sqrt(xSize * xSize + ySize * ySize + zSize * zSize)/4;
        }

        internal Transform3D toTransform3D()
        {            
            MathNet.Numerics.LinearAlgebra.Matrix<double> r = MathNet.Numerics.LinearAlgebra.Matrix<double>.Build.Dense(3, 3);
            MathNet.Numerics.LinearAlgebra.Vector<double> tr = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense(3);
            r[0, 0] = rot[0];
            r[0, 1] = rot[1];
            r[0, 2] = rot[2];
            r[1, 0] = rot[3];
            r[1, 1] = rot[4];
            r[1, 2] = rot[5];
            r[2, 0] = rot[6];
            r[2, 1] = rot[7];
            r[2, 2] = rot[8];
            var Rt0 = r * t0;
            tr[0] = t[0] - Rt0[0];
            tr[1] = t[1] - Rt0[1];
            tr[2] = t[2] - Rt0[2];
            return new Transform3D(r, tr);            
        }

        public Candidate(Transform3D tr)
        {
            var Rt0 = tr.RotationMatrix * t0;

            rot = new double[9];
            t = new double[3];
            rot[0] = tr.RotationMatrix[0, 0];
            rot[1] = tr.RotationMatrix[0, 1];
            rot[2] = tr.RotationMatrix[0, 2];
            rot[3] = tr.RotationMatrix[1, 0];
            rot[4] = tr.RotationMatrix[1, 1];
            rot[5] = tr.RotationMatrix[1, 2];
            rot[6] = tr.RotationMatrix[2, 0];
            rot[7] = tr.RotationMatrix[2, 1];
            rot[8] = tr.RotationMatrix[2, 2];
            t[0] = tr.TranslationVector[0] + Rt0[0];
            t[1] = tr.TranslationVector[1] + Rt0[1];
            t[2] = tr.TranslationVector[2] + Rt0[2];
        }

        public double SqrtDistanceTo(Candidate target)
        {
            double d = DistanceTo(target);
            if (d < 0)
                d = 0;
            return Math.Sqrt(d);
        }

        /// <summary>
        /// Caution! Only works if the mesh is centered and rotated correctly, otherwise use bfDistanceTo(...)!
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public double DistanceTo(Candidate target)
        {
            double[] r1 = this.rot;
            double[] r2 = target.rot;
            double[] t1 = this.t;
            double[] t2 = target.t;


            double r1tr20 = r1[0] * r2[0] + r1[3] * r2[3] + r1[6] * r2[6];
            double r1tr21 = r1[1] * r2[1] + r1[4] * r2[4] + r1[7] * r2[7];
            double r1tr22 = r1[2] * r2[2] + r1[5] * r2[5] + r1[8] * r2[8];

            double t1t2 = t1[0] * t2[0] + t1[1] * t2[1] + t1[2] * t2[2];
            double t2t2 = t2[0] * t2[0] + t2[1] * t2[1] + t2[2] * t2[2];
            double t1t1 = t1[0] * t1[0] + t1[1] * t1[1] + t1[2] * t1[2];

            double result = 2 * (dpsum) + n * (t2t2 + t1t1 - 2 * t1t2);

            result -= 2 * (r1tr20 * tpSum[0] + r1tr21 * tpSum[1] + r1tr22 * tpSum[2]);

            result *= invn;

            return (result);
        }        
    }
}
