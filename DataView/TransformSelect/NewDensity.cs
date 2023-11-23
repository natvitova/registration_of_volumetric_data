using System;
using Framework;


namespace DataView
{
    class NewDensity
    {
        // configuration
        public static double radius = Double.NaN; // this has to be set for particular object roughly as average distance to centroid
        public static double spread = 25;

        NewDistanceTree dt;

        public Transform3D Find(Transform3D[] transforms)
        {
            int maxDensityIndex = -1;

            dt = new NewDistanceTree(transforms);

            double maxDensity = 0;
            maxDensityIndex = -1;

            double t = -radius * Math.Log(0.01) / spread;

            object[] teInput = new object[transforms.Length / 100+1];

            for (int i = 0; i < teInput.Length; i++)
                teInput[i] = i * 100;

            ThreadedExecution<object, double[]> te = new ThreadedExecution<object, double[]>(teInput, new ThreadedExecution<object, double[]>.ExecBody(this.density), -1, new object[] { transforms, t});

            double[][] density = te.Execute();

            for (int i = 0; i < teInput.Length; i++)
            {
                for (int j = 0; j < 100; j++)
                    if (density[i][j] > maxDensity)
                    {
                        maxDensity = density[i][j];
                        maxDensityIndex = i * 100 + j;
                    }
            }
            return transforms[maxDensityIndex];
        }

        private double[] density(object input, int threadId, object[] parameters)
        {
            double[] result = new double[100];

            Transform3D[] candidate = (Transform3D[])parameters[0];
            double t = (double)parameters[1];

            for (int i = 0; i < 100; i++)
            {
                int idx = (int)input + i;

                if (idx == candidate.Length)
                    break;

                var nbs = dt.findAllCloserThan(candidate[idx], t);
                double density = 0;
                foreach (int n in nbs)
                    density += Math.Exp(-spread * candidate[idx].DistanceTo(candidate[n]) / (radius* radius));
                result[i] = density;
            }

            return result;
        }        
    }
}
