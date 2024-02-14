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
    /// Monaghan 2005 page 1710
    /// Smoothed Particles: A new paradigm for animating highly deformable bodies
    /// </summary>

    public class MonaghanKernel : BaseKernel
    {
        #region Contructors
        /// <summary>
        /// Инициализируем новый экземпляр класса
        /// </summary>
        public MonaghanKernel(int dimension = 2)
            : base(dimension) { }

        /// <summary>
        /// Инициализируем новый экземпляр класса
        /// </summary>
        /// <param name="kernelSize">Радиус сглаживающего ядра.</param>
        public MonaghanKernel(float kernelSize, int dimension = 2) : base(kernelSize, dimension) { }
        #endregion

        #region Methods
        /// <summary>
        /// Расчет  коэффициента для сглаживающего ядра
        /// </summary>
        protected override void CalculateFactor()
        {
            if (dimension == 2)
            {
                CoeffCore = (float)(15.0f / (7.0f * Math.PI * h2));
                DCoeffCore = (float)(-15.0f / (7.0f * Math.PI * h3));
                //D2CoeffCore = (float)(1000f / (827f * h4 * Math.PI)); //Комбинация
                //D2CoeffCore = (float)(400f / (331f * h4 * Math.PI));//Комбинация для неполной области
                D2CoeffCore = -DCoeffCore; //Оригинал
            }
        }

        /// <summary>
        /// Вычисление значения сглаживающего ядра  Monaghan 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override float Calculate(ref Vector2 r)
        {
            float Radius = r.Length();
            if 
                (Radius >= 2 * h)
                return 0.0f;
            float q = r.Length() / h;
            float q2 = q * q;
            if (q < 1)
            {
                return CoeffCore * (0.66667f- q2 + 0.5f * q2 * q);
            }
            else
                return CoeffCore * 0.16667f * (2 - q) * (2 - q) * (2 - q);
        }
        /// <summary>
        /// Вычисление градиента от сглаживающего ядра Monaghan
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override Vector2 CalculateGradient(ref Vector2 r)
        {
            float val;
            float Radius = r.Length();
            if (Radius >= h*2)
                return Vector2.Zero;
            float q = Radius / h;
            if (q < 1)
                val = 2f * q - 1.5f * q * q;
            else
                val = 0.5f * (2 - q) * (2 - q);
            val = DCoeffCore * val / (Radius + ErrEpsilon);
            return val * r;
        }
        /// <summary>
        /// Вычисление лапласиана от сглаживающего ядра Monaghan
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override float CalculateLaplacian(ref Vector2 r)
        {
            float val;
            float Radius = r.Length();
            if (Radius >= h * 2)
                return 0f;
            float q = Radius / h;
             if (q < 1)
                 val = 2f * q - 1.5f * q * q;
             else
                 val = 0.5f * (2 - q) * (2 - q);
             val = D2CoeffCore * val / (Radius + ErrEpsilon);
            
           /* if (Radius > h * 1.32288f)
                return 0f;
            float diff = 1f;
           // if (Radius == h * 1.32288f)
             //   diff = 0.5f;
            val = D2CoeffCore * diff;//Комбинация*/
            return val;
        }
        /// <summary>
        /// Получить масштабный коэффициент ядра
        /// </summary>
        /// <returns></returns>
        public override float GetHeshScale()
        {

                return 1.0f;  
        }
        /// <summary>
        /// Получить длину сглаживания
        /// </summary>
        /// <returns></returns>
        public override float GetKernelRadius()
        {
            return 2*h;
        }
        #endregion
    }

}
