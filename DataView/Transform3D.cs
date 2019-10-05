using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace DataView
{
    class Transform3D
    {
        private Matrix<double> rotationMatrix;
        public Matrix<double> RotationMatrix
        {
            get { return rotationMatrix; }
        }

        private Vector<double> translationVector;
        public Vector<double> TranslationVector
        {
            get { return translationVector; }
        }

        public Transform3D(Matrix<double> rotationMatrix, Vector<double> translationVector)
        {
            this.rotationMatrix = rotationMatrix;
            this.translationVector = translationVector;
        }

        public override string ToString()
        {
            return RotationMatrix.ToString();
        }
    }
}
