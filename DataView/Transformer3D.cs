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
            Point3D a = m.GetF1().GetPoint();
            Point3D b = m.GetF2().GetPoint();

            Matrix<double> rotationMatrix = RotationComputer.CalculateRotation(d2, d1, a, b, count);
            Console.WriteLine("Jsem tady hahahaha");
            Vector<double> translationVector = Vector<double>.Build.Dense(3);
            translationVector[0] = b.GetCoordinate(0) / d1.XSpacing - a.GetCoordinate(0) / d2.XSpacing;
            translationVector[1] = b.GetCoordinate(1) / d1.YSpacing - a.GetCoordinate(1) / d2.YSpacing;
            translationVector[2] = b.GetCoordinate(2) / d1.ZSpacing - a.GetCoordinate(2) / d2.ZSpacing;

            return new Transform3D(rotationMatrix, translationVector);
        }


    }
}