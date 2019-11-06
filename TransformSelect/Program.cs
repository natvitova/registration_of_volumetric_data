using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    class Program
    {
        static void Main(string[] args)
        {
            Transform3D[] trns = new Transform3D[4];
            MathNet.Numerics.LinearAlgebra.Matrix<double> identity = MathNet.Numerics.LinearAlgebra.Matrix<double>.Build.Dense(3, 3);
            identity[0, 0] = 1;
            identity[1, 1] = 1;
            identity[2, 2] = 1;

            MathNet.Numerics.LinearAlgebra.Vector<double> v1 = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense(3);
            v1[0] = 0;

            MathNet.Numerics.LinearAlgebra.Vector<double> v2 = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense(3);
            v2[0] = 9;

            MathNet.Numerics.LinearAlgebra.Vector<double> v3 = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense(3);
            v3[0] = 10;

            MathNet.Numerics.LinearAlgebra.Vector<double> v4 = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense(3);
            v4[0] = 11;

            trns[0] = new Transform3D(identity, v1);
            trns[1] = new Transform3D(identity, v2);
            trns[2] = new Transform3D(identity, v3);
            trns[3] = new Transform3D(identity, v4);
            Candidate.initSums(42, 3.14, 2.72);
            Density d = new Density(); // finder, we need an instance for certain complicated reason
            Transform3D solution = d.Find(trns);
            Console.WriteLine(solution);
        }
    }
}
