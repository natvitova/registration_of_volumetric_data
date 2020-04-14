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

        public Transform3D GetTransformation(Match m, IData dataMicro, IData dataMacro)
        {
            int count = 1000;

            Point3D pMicro = m.F1.Point.Copy();
            Point3D pMacro = m.F2.Point.Copy();
            Vector<double> translationVector = Vector<double>.Build.Dense(3);
            Matrix<double> rotationMatrix = RotationComputer.CalculateRotation(dataMicro, dataMacro, pMicro, pMacro, count);

            pMicro = pMicro.Rotate(rotationMatrix);

            translationVector[0] = pMacro.X - pMicro.X; // real coordinates
            translationVector[1] = pMacro.Y - pMicro.Y;
            translationVector[2] = pMacro.Z - pMicro.Z;

            return new Transform3D(rotationMatrix, translationVector);
        }

        public Transform3D GetTransformation(Match m, IData dataMicro, IData dataMacro, IConfiguration configuration)
        {
            int count = Convert.ToInt32(configuration["transform3D:count"]);
            Point3D pMicro = m.F1.Point.Copy();
            Point3D pMacro = m.F2.Point.Copy();
            Vector<double> translationVector = Vector<double>.Build.Dense(3);
            Matrix<double> rotationMatrix = RotationComputer.CalculateRotation(dataMicro, dataMacro, pMicro, pMacro, count);

            pMicro = pMicro.Rotate(rotationMatrix);

            translationVector[0] = pMacro.X - pMicro.X; // real coordinates
            translationVector[1] = pMacro.Y - pMicro.Y;
            translationVector[2] = pMacro.Z - pMicro.Z;

            return new Transform3D(rotationMatrix, translationVector);
        }
    }
}