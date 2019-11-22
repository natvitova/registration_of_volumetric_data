using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace DataView
{
    class ArtificialData
    {
        private VolumetricData vd; // macro
        private VolumetricData vd2; // micro
        private IFunction fce;
        private int[] cut;

        public ArtificialData(IFunction fce)
        {
            this.vd = new VolumetricData();
            this.vd2 = new VolumetricData();
            this.Fce = fce;

            int[] m = new int[] { 100, 100, 100 };
            VD.Measures = m;

            VD.VData = new int[m[2]][,];
            for (int k = 0; k < m[2]; k++)
            {
                VD.VData[k] = new int[m[0], m[1]];
                for (int i = 0; i < m[0]; i++)
                {
                    for (int j = 0; j < m[1]; j++)
                    {
                        VD.VData[k][i, j] = (int)fce.GetValue(i,j,k); //Avalue
                    }
                }
            }
            SetSmallerData();
        }

        public void SetSmallerData(int rotation, double[] translation, int[] cut)
        {
            double[] vx = new double[] { 1, 0, 0 };
            double[] vy = new double[] { 0, 1, 0 };
            double[] vz = new double[] { 0, 0, 1 };
            int[][,] d = VD.Cut3D2(translation, vx, vy, vz, cut[0], cut[1], cut[2], 1);
            VD2.Measures = cut;

            switch (rotation)
            {
                case 90:
                    {
                        VD2.VData = new int[cut[2]][,];
                        for (int k = 0; k < cut[2]; k++)
                        {
                            VD2.VData[k] = new int[cut[2], cut[1]];
                            for (int i = cut[2] - 1; i >= 0; i--)
                            {
                                int ii = Math.Abs(cut[2] - 1 - i);
                                for (int j = 0; j < cut[1]; j++)
                                {
                                    VD2.VData[k][ii, j] = d[k][i, j];
                                }
                            }
                        }
                    }
                    break;
                case 180:
                    {
                        VD2.VData = new int[cut[2]][,];
                        for (int k = 0; k < cut[2]; k++)
                        {
                            VD2.VData[k] = new int[cut[1], cut[2]];
                            for (int i = cut[1] - 1; i >= 0; i--)
                            {
                                int ii = Math.Abs(cut[1] - 1 - i);
                                for (int j = cut[2] - 1; j >= 0; j--)
                                {
                                    int jj = Math.Abs(cut[2] - 1 - j);
                                    VD2.VData[k][ii, jj] = d[k][i, j];
                                }
                            }
                        }
                    }
                    break;
                case 270:
                    {
                        VD2.VData = new int[cut[2]][,];
                        for (int k = 0; k < cut[2]; k++)
                        {
                            VD2.VData[k] = new int[cut[2], cut[1]];
                            for (int i = 0; i > cut[2]; i++)
                            {
                                for (int j = cut[1] - 1; j >= 0; j--)
                                {
                                    int jj = Math.Abs(cut[1] - 1 - j);
                                    VD2.VData[k][i, jj] = d[k][i, j];
                                }
                            }
                        }
                    }
                    break;
                default:
                    vd2.VData = d;
                    break;
            }

        }
        public void SetSmallerData(int rotation, double[] translation)
        {
            int m = 100;
            SetSmallerData(rotation, translation, new int[] { m, m, m });
        }
        public void SetSmallerData(int[] translation)
        {
            this.cut = translation;
            int m1 = 100 - cut[0];
            int m2 = 100 - cut[1];
            int m3 = 100 - cut[2];

            //int[] cut = new int[] { vd.Measures[0] - (int)translation[0], vd.Measures[1] - (int)translation[1], vd.Measures[2] - (int)translation[2] };
            //double[] vx = new double[] { 1, 0, 0 };
            //double[] vy = new double[] { 0, 1, 0 };
            //double[] vz = new double[] { 0, 0, 1 };
            //vd2.VData = VD.Cut3D2(translation, vx, vy, vz, cut[0], cut[1], cut[2], 1);
            //VD2.Measures = cut;

            vd2.VData = new int[m3][,];
            for (int k = 0; k < m3; k++)
            {
                vd2.VData[k] = new int[m1, m2];
                for (int i = 0; i < m1; i++)
                {
                    for (int j = 0; j < m2; j++)
                    {
                        vd2.VData[k][i, j] = (int)fce.GetValue(i + translation[0], j + translation[1], k + translation[2]);    //Avalue
                    }
                }
            }
            VD2.Measures = new int[] { m1, m2, m3 };
        }
        public void SetSmallerData()
        {
            int[] m = new int[] { 0, 0, 0 };
            SetSmallerData(m);
        }

        internal VolumetricData VD { get => vd; set => vd = value; }
        internal VolumetricData VD2 { get => vd2; set => vd2 = value; }
        public int[] Cut { get => cut; set => cut = value; }
        internal IFunction Fce { get => fce; set => fce = value; }
    }
}
