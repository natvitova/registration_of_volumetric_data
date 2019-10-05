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
            Point3D a = null; //match.point1
            Point3D b = null; //match.point2



            Matrix<double> rotationMatrix = RotationComputer.CalculateRotation(d1, d2, a, b, count);


            return null;
        }

       
    }
}