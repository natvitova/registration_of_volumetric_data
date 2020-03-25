using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.Extensions.Configuration;

namespace DataView
{
    class Transformer3D : ITransformer
    {
        public Transform3D GetTransformation(Match m,  IData d1, IData d2, IConfiguration configuration)
        {
            //int count = 1000;
            int count = Convert.ToInt32(configuration["transform3D:count"]);


            Point3D p1 = m.F1.Point; //micro
            Point3D p2 = m.F2.Point; //macro
            Vector<double> translationVector = Vector<double>.Build.Dense(3);


            //Matrix<double> rotationMatrix = RotationComputer.CalculateRotation(d1, d2, p1, p2, count);

            double[] ax = new double[3];
            ax[0] = 0;
            ax[1] = 0;
            ax[2] = 1;

            double fi = 12; //degrees
            fi = Math.PI * fi / 180;

            Matrix<double> rotationMatrix = Matrix<double>.Build.Dense(3, 3);
            rotationMatrix[0, 0] = Math.Cos(fi) + ax[0] * ax[0] * (1 - Math.Cos(fi));
            rotationMatrix[0, 1] = ax[0] * ax[1] * (1 - Math.Cos(fi)) - ax[2] * Math.Sin(fi);
            rotationMatrix[0, 2] = ax[0] * ax[2] * (1 - Math.Cos(fi)) + ax[1] * Math.Sin(fi);
            rotationMatrix[1, 0] = ax[0] * ax[1] * (1 - Math.Cos(fi)) + ax[2] * Math.Sin(fi);
            rotationMatrix[1, 1] = Math.Cos(fi) + ax[1] * ax[1] * (1 - Math.Cos(fi));
            rotationMatrix[1, 2] = ax[1] * ax[2] * (1 - Math.Cos(fi)) - ax[0] * Math.Sin(fi);
            rotationMatrix[2, 0] = ax[0] * ax[2] * (1 - Math.Cos(fi)) - ax[1] * Math.Sin(fi);
            rotationMatrix[2, 1] = ax[1] * ax[2] * (1 - Math.Cos(fi)) + ax[0] * Math.Sin(fi);
            rotationMatrix[2, 2] = Math.Cos(fi) + ax[2] * ax[2] * (1 - Math.Cos(fi));

            p1 = p1.Rotate(rotationMatrix);

            translationVector[0] = p2.X - p1.X; // real coordinates
            translationVector[1] = p2.Y - p1.Y;
            translationVector[2] = p2.Z - p1.Z;

            return new Transform3D(rotationMatrix, translationVector);
        }

        public Transform3D GetTransformationA(Match m, ArtificialData ad, int[] t1, int[] t2, IConfiguration configuration)
        {
            //int count = 1000;
            int count = Convert.ToInt32(configuration["transform3D:count"]); 

            Point3D p1 = m.F1.Point; //micro
            Point3D p2 = m.F2.Point; //macro

            Matrix<double> rotationMatrix = RotationComputer.CalculateRotationA(ad, t2, t1, p1, p2, count);
            Vector<double> translationVector = Vector<double>.Build.Dense(3);
            //translationVector[0] = p2.GetCoordinate(0) / d2.XSpacing - p1.GetCoordinate(0) / d1.XSpacing;
            //translationVector[1] = p2.GetCoordinate(1) / d2.YSpacing - p1.GetCoordinate(1) / d1.YSpacing;
            //translationVector[2] = p2.GetCoordinate(2) / d2.ZSpacing - p1.GetCoordinate(2) / d1.ZSpacing;

            translationVector[0] = p2.X - p1.X; // real coordinates
            translationVector[1] = p2.Y - p1.Y;
            translationVector[2] = p2.Z - p1.Z;

            return new Transform3D(rotationMatrix, translationVector);
        }
    }
}