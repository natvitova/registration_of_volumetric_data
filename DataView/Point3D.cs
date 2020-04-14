using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace DataView
{
    class Point3D
    {
        private double x;
        private double y;
        private double z;

        public Point3D()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }

        public Point3D(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Point3D Rotate(Matrix<double> m)
        {
            Vector<double> p = Vector<double>.Build.Dense(3);
            p[0] = this.x;
            p[1] = this.y;
            p[2] = this.z;
            Vector<double> newp = m.Multiply(p);

            return new Point3D(newp[0], newp[1], newp[2]);
        }

        public Point3D Move(double[] t)
        {
            Vector<double> p = Vector<double>.Build.Dense(3);
            p[0] = this.x + t[0];
            p[1] = this.y + t[1];
            p[2] = this.z + t[2];

            return new Point3D(p[0], p[1], p[2]);
        }

        public Point3D Copy()
        {
            return new Point3D(this.X, this.Y, this.Z);
        }

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Z { get => z; set => z = value; }

        public override string ToString()
        {
            return "x:" + Math.Round(X, 2) + " y:" + Math.Round(Y, 2) + " z:" + Math.Round(Z, 2);
        }
    }
}
