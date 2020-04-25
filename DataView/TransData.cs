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
        private Matrix<double> rotationM;
        private Vector<double> translation;
        private int[] measures;

        public TransData(VolumetricData vData, int[] t, double fi, double[] ax)
        {
            this.vData = vData;
            this.Measures = new int[3];
            this.Measures[0] = 100; //vData.Measures[0] - t[0] - 400;
            this.Measures[1] = 80; //vData.Measures[1] - t[1] - 400;
            this.Measures[2] = 120; //vData.Measures[2] - t[2] - 400;

            fi = Math.PI * fi / 180; //degrees to radians
            this.xSpacing = vData.XSpacing;
            this.ySpacing = vData.YSpacing;
            this.zSpacing = vData.ZSpacing;

            this.RotationM = Matrix<double>.Build.Dense(3, 3);
            this.Translation = Vector<double>.Build.Dense(3);
            this.Translation[0] = t[0] * XSpacing;
            this.Translation[1] = t[1] * YSpacing;
            this.Translation[2] = t[2] * ZSpacing;

            this.RotationM[0, 0] = Math.Cos(fi) + ax[0] * ax[0] * (1 - Math.Cos(fi));
            this.RotationM[0, 1] = ax[0] * ax[1] * (1 - Math.Cos(fi)) - ax[2] * Math.Sin(fi);
            this.RotationM[0, 2] = ax[0] * ax[2] * (1 - Math.Cos(fi)) + ax[1] * Math.Sin(fi);
            this.RotationM[1, 0] = ax[0] * ax[1] * (1 - Math.Cos(fi)) + ax[2] * Math.Sin(fi);
            this.RotationM[1, 1] = Math.Cos(fi) + ax[1] * ax[1] * (1 - Math.Cos(fi));
            this.RotationM[1, 2] = ax[1] * ax[2] * (1 - Math.Cos(fi)) - ax[0] * Math.Sin(fi);
            this.RotationM[2, 0] = ax[0] * ax[2] * (1 - Math.Cos(fi)) - ax[1] * Math.Sin(fi);
            this.RotationM[2, 1] = ax[1] * ax[2] * (1 - Math.Cos(fi)) + ax[0] * Math.Sin(fi);
            this.RotationM[2, 2] = Math.Cos(fi) + ax[2] * ax[2] * (1 - Math.Cos(fi));
        }

        public int GetValue(double x, double y, double z)
        {
            Vector<double> v = Vector<double>.Build.Dense(3);
            v[0] = x;
            v[1] = y;
            v[2] = z;

            Vector<double> u = RotationM.Multiply(v);
            u += Translation;
            Point3D Q = new Point3D(u[0], u[1], u[2]);
            if (u[0] > Measures[0] && u[1] > Measures[1] && u[2] > Measures[2] && u[0] <= 0 && u[1] <= 0 && u[2] <= 0)
            {
                Console.WriteLine();
            }
            return vData.GetValue(Q);
        }

        public void Rotate(Matrix<double> r)
        {
            RotationM = RotationM.Multiply(r);
        }

        public void Move(double[] translation)
        {
            this.RotationM[0, 3] += translation[0];
            this.RotationM[1, 3] += translation[1];
            this.RotationM[2, 3] += translation[2];
        }

        public double[] GetAxis(Matrix<double> r)
        {
            double[] u = new double[3];
            u[0] = r[2, 1] - r[1, 2];
            u[1] = r[0, 2] - r[2, 0];
            u[2] = r[1, 0] - r[0, 1];

            return u;
        }

        public double[] GetAxis()
        {
            return GetAxis(RotationM);
        }

        public double GetAngle(Matrix<double> r)
        {
            double tr = r.Trace();//r[0, 0] + r[1, 1] + r[2, 2];
            tr = Math.Min(tr, 3);
            tr = Math.Max(tr, -1);

            double phi = Math.Acos((tr - 1) / 2);
            phi = 180*phi/Math.PI;
            return phi;
        }

        public double GetAngle()
        {
            return GetAngle(RotationM);
        }

        public double GetAlpha(Matrix<double> r2)
        {
            Matrix<double> r = r2.Transpose();
            Matrix<double> r3 = RotationM.Multiply(r);
            double alpha = GetAngle(r3);
            return alpha;
        }

        public int GetValue(Point3D p)
        {
            return GetValue(p.X, p.Y, p.Z);
        }

        public Point3D[] Sample(Point3D[] points) // from micro to macro
        {
            Vector<double> v = Vector<double>.Build.Dense(3);
            
            Point3D[] pointsReturn = new Point3D[points.Length];
            for (int i = 0; i<points.Length;i++)
            {
                v[0] = points[i].X;
                v[1] = points[i].Y;
                v[2] = points[i].Z;

                Vector<double> u = RotationM.Multiply(v);
                u += Translation;
                Point3D newPoint = new Point3D(u[0], u[1], u[2]);
                pointsReturn[i] = newPoint;
            }
            return pointsReturn;
        }

        public int[,] Cut(double[] point, double[] v1, double[] v2, int xRes, int yRes, double spacing)
        {
            throw new NotImplementedException();
        }

        public Matrix<double> RotationM { get => rotationM; set => rotationM = value; }
        public double XSpacing { get => xSpacing; set => xSpacing = value; }
        public double YSpacing { get => ySpacing; set => ySpacing = value; }
        public double ZSpacing { get => zSpacing; set => zSpacing = value; }
        public int[] Measures { get => measures; set => measures = value; }
        public Vector<double> Translation { get => translation; set => translation = value; }
    }
}
