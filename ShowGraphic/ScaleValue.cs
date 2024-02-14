using System;

namespace ShowGraphic
{
    
    class ScaleValue
    {
        /// <summary>
        /// Авто округление чисел до второго значащего числа
        /// </summary>
        public static double ScaleConvert(double a)
        {
            double res = 0;
            int s = Math.Sign(a);
            a = Math.Abs(a);
            int n = (int)Math.Log10(a);
            if (a > 1)
            {
                double val = (double)Math.Pow(10, n);
                double dv = (double)Math.Pow(10, n - 1);
                for (int i = 1; i <= 1000; i++)
                {
                    res = val + dv * i;
                    if (res > a)
                        break;
                }
            }
            else
            {
                double val = (double)Math.Pow(10, n - 1);
                double dv = (double)Math.Pow(10, n - 2);
                for (int i = 1; i <= 10; i++)
                {
                    res = val + dv * i;
                    if (res > a)
                        break;
                }
            }
            return s * res;
        }
    }
}
