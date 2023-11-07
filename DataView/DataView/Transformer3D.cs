using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.Extensions.Configuration;

namespace DataView
{
    class Transformer3D : ITransformer
    {

        public Transform3D GetTransformation(Match m, IData dataMicro, IData dataMacro)
        {
            //int count = 1_000;

            Point3D pMicro = m.F1.Point.Copy();
            Point3D pMacro = m.F2.Point.Copy();

            Vector<double> translationVector = Vector<double>.Build.Dense(3);
            //Matrix<double> rotationMatrix = RotationComputer.CalculateRotation(dataMicro, dataMacro, pMicro, pMacro, count); original method
            //Matrix<double> rotationMatrix = TestRotationComputer.CalculateRotation(dataMicro, dataMacro, pMicro, pMacro, count);

            Matrix<double> rotationMatrix;

            //Select min spacing
            double[] spacings = new double[] { dataMicro.XSpacing, dataMicro.YSpacing, dataMicro.ZSpacing, dataMacro.XSpacing, dataMacro.YSpacing, dataMacro.ZSpacing };
            double minSpacing = spacings.Min();

            //Round the points to be matching the min spacing
            //pMicro = RoundPoint(pMicro, minSpacing);
            //pMacro = RoundPoint(pMacro, minSpacing);

            try { rotationMatrix = TestUniformRotationComputerPCA.CalculateRotation(dataMicro, dataMacro, pMicro, pMacro, minSpacing); }
            catch (Exception e) { throw e; }

            pMicro = pMicro.Rotate(rotationMatrix);

            translationVector[0] = pMacro.X - pMicro.X; // real coordinates
            translationVector[1] = pMacro.Y - pMicro.Y;
            translationVector[2] = pMacro.Z - pMicro.Z;

            return new Transform3D(rotationMatrix, translationVector);
        }

        private Point3D RoundPoint(Point3D point, double spacing)
        {
            double newPointX = roundNumber(point.X, spacing);
            double newPointY = roundNumber(point.Y, spacing);
            double newPointZ = roundNumber(point.Z, spacing);

            return new Point3D(newPointX, newPointY, newPointZ);
        }

        private double roundNumber(double number, double units)
        {
            double remainder = number % units;
            double midpoint = units / 2.0;

            if (remainder >= midpoint)
                return number + (units - remainder);

            return number - remainder;
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