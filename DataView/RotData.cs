using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
namespace DataView
{
    class TransData : IData
    {
        private double xSpacing;
        private double ySpacing;
        private double zSpacing;
        private VolumetricData vData;
        private double[] translation;
        private double[] vecX;
        private double[] vecY;
        private double[] vecZ;
        private Matrix<double> rotationM;

        

        public TransData(VolumetricData vData, double[] t, int fi)
        {
            this.xSpacing = vData.XSpacing;
            this.ySpacing = vData.YSpacing;
            this.zSpacing = vData.ZSpacing;
            this.translation = t;

            Matrix<double> rot = Matrix<double>.Build.Dense(3,4);

            //z
            rot[0, 0] = Math.Cos(fi);
            rot[0, 1] = -Math.Sin(fi);
            rot[0, 2] = 0;
            rot[0, 3] = t[0];
            rot[1, 0] = Math.Sin(fi);
            rot[1, 1] = Math.Cos(fi);
            rot[1, 2] = 0;
            rot[1, 3] = t[1];
            rot[2, 0] = 0;
            rot[2, 1] = 0;
            rot[2, 2] = 1;
            rot[2, 3] = t[2];

            //z
            vecX = new double[] { Math.Cos(fi), -Math.Sin(fi), 0 };
            vecY = new double[] { Math.Sin(fi), Math.Cos(fi), 0 };
            vecZ = new double[] { 0, 0, 1 };

        }

        public int GetValue(double x, double y, double z)
        {
            double newx = translation[0] + vecX[0] * x + vecY[0] * y + vecZ[0] * z;
            double newy = translation[1] + vecX[1] * x + vecY[1] * y + vecZ[1] * z;
            double newz = translation[2] + vecX[2] * x + vecY[2] * y + vecZ[2] * z;
            Point3D Q = new Point3D(newx,newy,newz);            
            return vData.GetValue(Q);
        }

        public void Rotate(Matrix<double> r)
        {
            RotationM = RotationM.Multiply(r);
        }

        public void Move(double[] translation)
        {
            this.translation[0] += translation[0];
            this.translation[1] += translation[1];
            this.translation[2] += translation[2];
        }

        public double[] GetAngles()
        {
            double theta1 = -Math.Asin(RotationM[2, 0]);
            double theta2 = Math.PI + Math.Asin(RotationM[2, 0]);

            double psi1 = Math.Atan2(RotationM[2, 1] / Math.Cos(theta1), RotationM[2, 2] / Math.Cos(theta1));
            double psi2 = Math.Atan2(RotationM[2, 1] / Math.Cos(theta2), RotationM[2, 2] / Math.Cos(theta2));

            double phi1 = Math.Atan2(RotationM[1, 0] / Math.Cos(theta1), RotationM[0, 0] / Math.Cos(theta1));
            double phi2 = Math.Atan2(RotationM[1, 0] / Math.Cos(theta2), RotationM[0, 0] / Math.Cos(theta2));

             return new double[] { theta1, psi1, phi1 };
        }

        public Matrix<double> RotationM { get => rotationM; set => rotationM = value; }
    }
}
