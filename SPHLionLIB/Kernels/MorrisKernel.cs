/////////////////////////////////////////////////////
// дата: 16 03 2019
// автор: Потапов Игорь Иванович 
// библиотека: SPH_LibraryKernels
// лицензия: свободное распространение
/////////////////////////////////////////////////////

using System;
using System.Numerics;

namespace SPHLionLIB
{
    /// <summary>
    /// Класс сглаживающего ядра
    /// Gaussian kernel: - классика
    /// </summary>
    class MorrisKernel : BaseKernel
    {
        #region Contructors
        /// <summary>
        /// Инициализируем новый экземпляр класса
        /// </summary>
        public MorrisKernel(int dimension = 2)
            : base(dimension) { }

        /// <summary>
        /// Инициализируем новый экземпляр класса
        /// </summary>
        /// <param name="kernelSize">Радиус сглаживающего ядра.</param>
        public MorrisKernel(float kernelSize, int dimension = 3) : base(kernelSize, dimension) { }
        #endregion

        #region Methods
        /// <summary>
        /// Расчет  коэффициента для сглаживающего ядра
        /// </summary>
        protected override void CalculateFactor()
        {
            if (dimension == 2)
            { 
                CoeffCore = (float)(7.0/(478 * Math.PI * h2));
                DCoeffCore = -CoeffCore / h;
                D2CoeffCore = (float)(1000f / (827f * h4 * Math.PI));
                //D2CoeffCore = -DCoeffCore; //Оригинал
            }

        }
        
        /// <summary>
        /// Вычисление значения сглаживающего ядра  
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override float Calculate(ref Vector2 r)
        {
            float Radius = r.Length();
            float q = (float)(Radius / h);
            if (q >= 3)
                return 0.0f;
            else
            {
                float a, b, c;
                if (q <= 1)
                {
                    a = (3 - q) * (3 - q); a *= a; a *= (3 - q);
                    b = (2 - q) * (2 - q); b *= b; b *= (2 - q);
                    c = (1 - q) * (1 - q); c *= c; c *= (1 - q);
                    float K = (float)(CoeffCore * (a - 6 * b + 15 * c));
                    return K;
                }
                else
                    if (q <= 2)
                    {
                        a = (3 - q) * (3 - q); a *= a; a *= (3 - q);
                        b = (2 - q) * (2 - q); b *= b; b *= (2 - q);
                        return (float)(CoeffCore * (a - 6 * b));
                    }
                    else
                    {
                        a = (3 - q) * (3 - q); a *= a; a *= (3 - q);
                        return (float)(CoeffCore * a);
                    }
            }
        }
        /// <summary>
        /// Вычисление градиента от сглаживающего ядра 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override Vector2 CalculateGradient(ref Vector2 r)
        {
            float Radius = r.Length();
            if (Radius >=  3 * h)
                return Vector2.Zero;
            else
            {
                float a, b, c;
                float q = Radius/h;
                Vector2 v = r / ((Radius + ErrEpsilon) );
                if (q <= 1)
                {
                    a = (3 - q) * (3 - q); a *= a;
                    b = (2 - q) * (2 - q); b *= b;
                    c = (1 - q) * (1 - q); c *= c;
                    return v * (float)(DCoeffCore * (5 * a - 30 * b + 75 * c));
                }
                else
                    if (q <= 2)
                    {
                        a = (3 - q) * (3 - q); a *= a;
                        b = (2 - q) * (2 - q); b *= b;
                        return v * (float)(DCoeffCore * (5 * a - 30 * b));
                    }
                    else
                    {
                        a = (3 - q) * (3 - q); a *= a;
                        return v * (float)(DCoeffCore * 5 * a);
                    }
            }
        }
        /// <summary>
        /// Вычисление лапласиана от сглаживающего ядра 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override float CalculateLaplacian(ref Vector2 r)
        {
            float Radius = r.Length();
           /* if (Radius >= 3 * h)
                return 0f;
            else
            {
                float a, b, c;
                float q = Radius / h;
                if (q <= 1)
                {
                    a = (3 - q) * (3 - q); a *= a;
                    b = (2 - q) * (2 - q); b *= b;
                    c = (1 - q) * (1 - q); c *= c;
                    return (D2CoeffCore * (5 * a - 30 * b + 75 * c)) / (Radius + ErrEpsilon);
                }
                else
                    if (q <= 2)
                {
                    a = (3 - q) * (3 - q); a *= a;
                    b = (2 - q) * (2 - q); b *= b;
                    return (D2CoeffCore * (5 * a - 30 * b)) / (Radius + ErrEpsilon);
                }
                else
                {
                    a = (3 - q) * (3 - q); a *= a;
                    return (D2CoeffCore * 5 * a) / (Radius + ErrEpsilon);
                }
            }*/

                if (Radius > h * 1.32288f)
                    return 0f;
                float diff = 1f;
                if (Radius == h * 1.32288f)
                    diff = 0.5f;

                return D2CoeffCore * diff;//Комбинация
         }
        /// <summary>
        /// Получить масштабный коэффициент ядра
        /// </summary>
        /// <returns></returns>
        public override float GetHeshScale()
        {
            if(dimension==1)
                return 1.0f;
            else
                return 1.0f;
        }
        /// <summary>
        /// Получить длину сглаживания
        /// </summary>
        /// <returns></returns>
        public override float GetKernelRadius()
        {
            return 3*h;
        }
        #endregion
    }
}
