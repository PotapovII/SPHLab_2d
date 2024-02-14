/////////////////////////////////////////////////////
// дата: 16 03 2019
// автор: Потапов Игорь Иванович 
// библиотека: SPH_LibraryKernels
// лицензия: свободное распространение
/////////////////////////////////////////////////////
using System.Numerics;

namespace SPHLionLIB
{
    /// <summary>
    /// Абстрактный базовый класс сглаживающего ядра  
    /// используемый для моделирования жидкости на основе SPH метода
    /// </summary>
    public abstract class BaseKernel 
    {
        #region Members
        protected float ErrEpsilon;
        /// <summary>
        /// Коэффициент сглаживающего ядра
        /// </summary>
        protected float CoeffCore = 1f;
        protected float DCoeffCore = 1f;
        protected float D2CoeffCore = 1f;
        /// <summary>
        /// Радиус сглаживающего ядра 
        /// </summary>
        protected float h;
        /// <summary>
        /// Квадрат радиуса сглаживания для ядра 
        /// </summary>
        protected float h2Length;
        public float H { get { return h; } }
        /// <summary>
        /// Квадрат радиуса сглаживающего ядра 
        /// </summary>
        protected float h2;
        /// <summary>
        /// Куб радиуса сглаживающего ядра 
        /// </summary>
        protected float h3;
        /// <summary>
        /// 4 степень сглаживающего ядра  
        /// </summary>
        protected float h4;
        /// <summary>
        /// 5 степень сглаживающего ядра  
        /// </summary>
        protected float h5;
        /// <summary>
        /// Шестая степень сглаживающего ядра 
        /// </summary>
        protected float h6;
        /// <summary>
        /// 7 степень сглаживающего ядра 
        /// </summary>
        protected float h7;
        /// <summary>
        /// 8 степень сглаживающего ядра 
        /// </summary>
        protected float h8;
        /// <summary>
        /// 9 степень сглаживающего ядра 
        /// </summary>
        protected float h9;
        /// <summary>
        /// Размерность ядра
        /// </summary>
        protected int dimension;

        //protected float Radius2Length;
        //protected float Radius;
        //protected float Radius2;
        //protected float diff;
        //protected float val;
        //protected float q;
        /// <summary>
        /// Масштабирование диаметра частицы к масштабу h для ядра
        /// определяется в наследниках
        /// </summary>
        #endregion

        #region Properties

        /// <summary>
        /// Получаем или задаем размер ядра.
        /// </summary>
        public float KernelSize
        {
            get { return h; }
            set
            {
                h = value;
                h2Length = 2 * h;
                h2 = h * h;
                h3 = h * h2;
                h4 = h2 * h2;
                h5 = h2 * h3;
                h6 = h3 * h3;
                h7 = h * h6;
                h8 = h * h7;
                h9 = h * h8;
                ErrEpsilon = 0.000001f * h;
                CalculateFactor();
            }
        }
        #endregion

        #region Contructors

        /// <summary>
        /// Инициализируем новый экземпляр класса
        /// </summary>
        public BaseKernel(int dimension = 2)
        {
            this.dimension = dimension;
            this.KernelSize = 1;
        }

        /// <summary>
        /// Инициализируем новый экземпляр класса
        /// </summary>
        /// <param name="kernelSize">Радиус сглаживающего ядра.</param>
        public BaseKernel(float kernelSize, int dimension = 2)
        {
            this.dimension = dimension;
            this.KernelSize = kernelSize;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Расчет  коэффициента для сглаживающего ядра
        /// </summary>
        protected abstract void CalculateFactor();
        /// <summary>
        /// Вычисление значения сглаживающего ядра  Poly6 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public abstract float Calculate(ref Vector2 r);
        /// <summary>
        /// Вычисление градиента от сглаживающего ядра 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public abstract Vector2 CalculateGradient(ref Vector2 r);
        /// <summary>
        /// Вычисление лапласиана от сглаживающего ядра 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public abstract float CalculateLaplacian(ref Vector2 r);
        /// <summary>
        /// Получить масштабный коэффициент ядра
        /// </summary>
        /// <returns></returns>
        public abstract float GetHeshScale();
        /// <summary>
        /// Получить длину сглаживания
        /// </summary>
        /// <returns></returns>
        public abstract float GetKernelRadius();
        #endregion
    }
}
