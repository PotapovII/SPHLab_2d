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
    /// Короткое ядро LOSSE (я скомпоновала)
    /// </summary>
    public class LosseKernel : BaseKernel
    {
        #region Contructors
        /// <summary>
        /// Инициализируем новый экземпляр класса
        /// </summary>
        public LosseKernel(int dimension = 2)
            : base(dimension)
        { }

        /// <summary>
        /// Инициализируем новый экземпляр класса
        /// </summary>
        /// <param name="kernelSize">Радиус сглаживающего ядра</param>
        public LosseKernel(float kernelSize, int dimension = 2) : base(kernelSize, dimension) { }
        #endregion

        #region Methods
        /// <summary>
        /// Расчет  коэффициента для сглаживающего ядра
        /// </summary>
        protected override void CalculateFactor()
        {
            if (dimension == 2)
            {
                CoeffCore = (float)(15f / 13f / h2);
                DCoeffCore = (float)(-264f / (655f * h4 * Math.PI));
                //D2CoeffCore = (float)(1000f / (827f * h4 * Math.PI));
                D2CoeffCore = (float)(400f / (331f * h4 * Math.PI));
                //D2CoeffCore = (float)(264f / (655f * h4 * Math.PI)); //оригинал
            }


        }
        /// <summary>
        /// Вычисление значения сглаживающего ядра 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override float Calculate(ref Vector2 r)
        {
            float Radius2 = r.LengthSquared();
            if (Radius2 >= h2)
                return 0.0f;
            float q2 = Radius2 / h2;
            float diff = (1 - q2) * (1 - q2) * (1 - 0.689577f * q2);
            return CoeffCore * diff;
        }
        /// <summary>
        /// Вычисление градиента для ядра 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override Vector2 CalculateGradient(ref Vector2 r)
        {
            float Radius = r.Length();
            if (Radius == 0) return Vector2.Zero;
            if  (Radius > h)
                return Vector2.Zero;
            float diff = DCoeffCore * (25f * h - 22f * Radius) / (Radius + ErrEpsilon);
            return diff * r;

        }
        /// <summary>
        /// Вычисление лапласиана от сглаживающего ядра 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override float CalculateLaplacian(ref Vector2 r)
        {
            float Radius = r.Length();
            if (Radius > h)
                return 0f;
           float diff = 1f;
           return D2CoeffCore * diff;

           /* if (Radius == 0) return 0f;
            if (Radius > h)
                return 0f;
            float diff = D2CoeffCore * (25f * h - 22f * Radius);
            return diff;*/
        }
        /// <summary>
        /// Получить масштабный коэффициент ядра
        /// </summary>
        /// <returns></returns>
        public override float GetHeshScale()
        {
                return 1.00f;
        }
        /// <summary>
        /// Получить длину сглаживания
        /// </summary>
        /// <returns></returns>
        public override float GetKernelRadius()
        {
            return h;
        }
        #endregion
    }
}
