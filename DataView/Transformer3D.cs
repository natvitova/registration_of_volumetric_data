using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace DataView
{
    class Transformer3D : ITransformer
    {
        public Transform3D GetTransformation(Match m, VolumetricData d1, VolumetricData d2)
        {
            int count = 1000;

            Point3D p1 = m.F1.Point; //micro
            Point3D p2 = m.F2.Point; //macro

            Matrix<double> rotationMatrix = RotationComputer.CalculateRotation(d2, d1, p1, p2, count);
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