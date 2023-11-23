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
        private static ITransformationDistance transformationDistance;

        private Matrix<double> rotationMatrix;
        private Vector<double> translationVector;

        /// <summary>
        /// Set transformation distance for this object
        /// </summary>
        /// <param name="transformationDistance"></param>
        public static void SetTransformationDistance(ITransformationDistance transformationDistance)
        {
            Transform3D.transformationDistance = transformationDistance;
        }

        public Transform3D(Matrix<double> rotationMatrix, Vector<double> translationVector)
        {
            RotationMatrix = rotationMatrix;
            TranslationVector = translationVector;
        }

        public Matrix<double> RotationMatrix { get => rotationMatrix; set => rotationMatrix = value; }
        public Vector<double> TranslationVector { get => translationVector; set => translationVector = value; }

        public override string ToString()
        {
            return RotationMatrix.ToString() + TranslationVector.ToString();
        }

        public double DistanceTo(Transform3D anotherTransformation)
        {
            if (transformationDistance == null)
                throw new Exception("Transformation dustance needs to bet set before calling this method.");

            return transformationDistance.GetTransformationsDistance(this, anotherTransformation);
        }

        public double SqrtDistanceTo(Transform3D anotherTransformation)
        {
            return Math.Sqrt(Math.Abs(DistanceTo(anotherTransformation)));
        }
    }
}
