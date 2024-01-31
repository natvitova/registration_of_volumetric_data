using System;
using MathNet.Numerics.LinearAlgebra;

namespace DataView
{
    class Point3D
    {
        private double x;
        private double y;
        private double z;

        /// <summary>
        /// Initializes a point with [0, 0, 0] coordinates
        /// </summary>
        public Point3D()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }

        /// <summary>
        /// Initializes a point with given [x, y z] coordinates
        /// </summary>
        /// <param name="x">Coordinate x</param>
        /// <param name="y">Coordinate y</param>
        /// <param name="z">Coordinate z</param>
        public Point3D(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Calculates coordinates for point rotated using given rotation matrix
        /// </summary>
        /// <param name="m">Rotation matrix</param>
        /// <returns>Returns new coordinates for the original point</returns>
        public Point3D Rotate(Matrix<double> m)
        {
            
            Vector<double> p = Vector<double>.Build.Dense(3);
            p[0] = this.x;
            p[1] = this.y;
            p[2] = this.z;
            Vector<double> newp = m.Multiply(p);

            return new Point3D(newp[0], newp[1], newp[2]);
        }

        /// <summary>
        /// Moves the point by coordinates in the passed array
        /// They need to be in the order bellow
        /// [offsetX, offsetY, offsetZ]
        /// </summary>
        /// <param name="t">Array with offsets</param>
        /// <returns>Returns the new coordinates of a point</returns>
        public Point3D Move(double[] t)
        {
            Vector<double> p = Vector<double>.Build.Dense(3);
            p[0] = this.x + t[0];
            p[1] = this.y + t[1];
            p[2] = this.z + t[2];

            return new Point3D(p[0], p[1], p[2]);
        }

        /// <summary>
        /// Creates a copy of this instance
        /// </summary>
        /// <returns>Returns instance of a coppied point</returns>
        public Point3D Copy()
        {
            return new Point3D(this.X, this.Y, this.Z);
        }

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Z { get => z; set => z = value; }

        /// <summary>
        /// ToString method shows basic information about the point
        /// </summary>
        /// <returns>Gives string with X, Y, Z coordinates for the given point</returns>
        public override string ToString()
        {
            return "x:" + Math.Round(X, 2) + " y:" + Math.Round(Y, 2) + " z:" + Math.Round(Z, 2);
        }

        /// <summary>
        /// Calculates the distance between this point and the passed one
        /// </summary>
        /// <param name="differentPoint">Point which is used to calculate the distance</param>
        /// <returns></returns>
        public double Distance(Point3D differentPoint)
        {
            double xSquared = Math.Pow((this.x - differentPoint.x), 2);
            double ySquared = Math.Pow((this.y - differentPoint.y), 2);
            double zSquared = Math.Pow((this.z - differentPoint.z), 2);

            return Math.Sqrt(xSquared + ySquared + zSquared);
        }
    }
}
