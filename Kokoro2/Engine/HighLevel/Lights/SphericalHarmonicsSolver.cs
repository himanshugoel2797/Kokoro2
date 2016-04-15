using Kokoro2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Kokoro2.Engine.HighLevel.Lights
{
    public class SphericalHarmonicsSolver
    {
        #region Source
        //Most of the code here comes almost directly from http://www.research.scea.com/gdc2003/spherical-harmonic-lighting.pdf
        #endregion

        private struct SHSample
        {
            public Vector3 sph;
            public Vector3 vec;
            public double[] coeffs;
        }

        private double[] factorials;

        private void precalculateFactorials(int bandCnt)
        {
            factorials = new double[bandCnt * 2 + 1];
            factorials[0] = 1;  //Factorial(1)


            for (int i = 2; i < factorials.Length; i++)
            {
                factorials[i - 1] = factorials[i - 2] * i;
            }
        }

        private double P(int l, int m, double x)
        {
            double pmm = 1.0;
            if (m > 0)
            {
                double somx2 = Sqrt((1.0 - x) * (1.0 + x));
                double fact = 1.0;
                for (int i = 1; i < m; i++)
                {
                    pmm *= (-fact) * somx2;
                    fact += 2.0;
                }
            }

            if (l == m) return pmm;
            double pmmp1 = x * (2.0 * m + 1) * pmm;
            if (l == m + 1) return pmmp1;

            double pll = 0;
            for (int ll = m + 2; ll <= l; ++ll)
            {
                pll = ((2.0 * ll - 1) * x * pmmp1 - (ll + m - 1) * pmm) / (ll - m);
                pmm = pmmp1;
                pmmp1 = pll;
            }
            return pll;
        }

        private double K(int l, int m)
        {
            double temp = ((2.0 * l + 1.0) * factorials[l - m - 1]) / (4.0 * PI * factorials[l + m - 1]);
            return Sqrt(temp);
        }

        private double SH(int l, int m, double theta, double phi)
        {
            double sqrt2 = Sqrt(2);

            if (m == 0) return K(l, m) * P(l, m, Cos(theta));
            else if (m > 0) return sqrt2 * K(l, m) * Cos(m * phi) * P(l, m, Cos(theta));
            else return sqrt2 * K(l, -m) * Sin(-m * phi) * P(l, -m, Cos(theta));
        }

        private SHSample[] SetupSHSamples(int sampleCnt, int bandCnt)
        {
            precalculateFactorials(bandCnt);

            SHSample[] samples = new SHSample[sampleCnt * sampleCnt * 2];

            int i = 0;
            double invN = 1 / sampleCnt;

            Random rng = new Random(0);

            for (int a = 0; a < sampleCnt; a++)
            {
                for (int b = 0; b < sampleCnt; b++)
                {
                    double x = (a + rng.NextDouble()) * invN;
                    double y = (b + rng.NextDouble()) * invN;

                    double theta = 2.0 * Acos(Sqrt(1.0 - x));
                    double phi = 2.0 * PI * y;

                    samples[i].sph = new Vector3((float)theta, (float)phi, 1);
                    samples[i].vec = new Vector3((float)(Sin(theta) * Cos(phi)), (float)(Sin(theta) * Sin(phi)), (float)Cos(theta));

                    samples[i].coeffs = new double[bandCnt * (bandCnt + 1)];

                    for (int l = 0; l < bandCnt; l++)
                    {
                        for (int m = -l; m <= l; m++)
                        {
                            int index = l * (l + 1) + m;
                            samples[i].coeffs[index] = SH(l, m, theta, phi);
                        }
                    }


                    i++;

                }
            }


            return samples.ToArray();
        }

        private double[] ProjectSHFunction(Func<double, double, double> fn, SHSample[] samples)
        {
            double[] result = new double[samples[0].coeffs.Length];
            for(int i = 0; i < samples.Length; i++)
            {
                double theta = samples[i].sph.X;
                double phi = samples[i].sph.Y;
                for(int n = 0; n < result.Length; n++)
                {
                    result[n] += fn(theta, phi) * samples[i].coeffs[n];
                }
            }

            double factor = 4 * PI / samples.Length;
            for(int i = 0; i < result.Length; i++)
            {
                result[i] = result[i] * factor;
            }

            return result;
        }

    }
}
