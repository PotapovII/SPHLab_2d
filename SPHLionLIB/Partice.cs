using System;
using System.Numerics;

namespace SPHLionLIB
{
    /// <summary>
    /// Частица
    /// </summary>
    public class Particle
    {
        public int KernelIdx = 3;
        /// <summary>
        /// Номер частицы
        /// </summary>
        public int idx;
        /// <summary>
        /// Позиция
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// Старая позиция
        /// </summary>
        public Vector2 PositionOld;
        /// <summary>
        /// Скорость
        /// </summary>
        public Vector2 Velocity;
        /// <summary>
        /// Ускорение
        /// </summary>
        public Vector2 Acceleration;
        /// <summary>
        /// Плотность
        /// </summary>
        public float density;
        /// <summary>
        /// Плотность частицы
        /// </summary>
        public float Density
        {
            get { return density; }
            set
            {
                density = value;
                if (density < NaturalDensety)
                    Pressure = 0;
                else
                    Pressure = Rigidity * (density - NaturalDensety);
            }
        }
        /// <summary>
        /// Натуральная плотность
        /// </summary>
        public float NaturalDensety; 
        /// <summary>
        /// Давление
        /// </summary>
        public float Pressure = 0;
        /// <summary>
        /// Цвет
        /// </summary>
        public int Color = 0;
        /// <summary>
        /// Масса
        /// </summary>
        public float Mass;
        /// <summary>
        /// Коэффициент объемной упругости  
        /// </summary>
        public static float Rigidity;
        /// <summary>
        /// Жесткость среды
        /// </summary>
       // public float Cp = Rigidity * NaturalDensety;
        /// <summary>
        /// Размер частицы
        /// </summary>
        public float ParticelSize;
        /// <summary>
        /// Порядок сглаживания ядра
        /// </summary>
        public static int SmoothingOrder;
        /// <summary>
        /// Ускорение от силы выталкивания
        /// </summary
        public  Vector2 Archi;
        /// <summary>
        /// Вектор тяги
        /// </summary>
        public static Vector2 G = new Vector2(0,0);

        //Хранилища под переменные свойства частицы
        public int Count = 0;
        public const int MaxCount = 300;

        public Vector2 ddW = new Vector2();
        public float[] R_ij = new float[MaxCount];
        public float[] W_ij = new float[MaxCount];
        public Vector2[] Distance_ij = new Vector2[MaxCount];
        public Vector2[] GradientW_ij = new Vector2[MaxCount];
        public Vector2[] LGradientW_ij = new Vector2[MaxCount];
        public Particle[] particle_ij = new Particle[MaxCount];



        #region Вставка для напряжений
        /// <summary>
        /// Тензор градиента скоростей
        /// </summary>
        public Matrix2x2 GradVelocity;
        /// <summary>
        /// Тензор скоростей деформаций 
        /// </summary>
        public Matrix2x2 StrainRateTensor;
        /// <summary>
        /// Вихривой тензор скоростей деформаций 1 строка
        /// </summary>
        public Matrix2x2 EddyDeformationTensor;
        /// <summary>
        /// Тензор напряжений 
        /// </summary>
        public Matrix2x2 DevStressTensor;
        /// <summary>
        /// Второй инвариант 
        /// </summary>
        public float DevStressTensor_J2 { get { return (float)Math.Sqrt(DevStressTensor.J2); }  }
        /// <summary>
        /// Маркер нахождения на поверхности тикучести
        /// </summary>
        public int Plastic = 0;
        #endregion 
        /// <summary>
        /// Конструктор для массивов
        /// </summary>
        public Particle()
        {
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Position">Позиция</param>
        /// <param name="Velocity">Скорость</param>
        public Particle(Vector2 Position, Vector2 Velocity, int Color,float Dens, float Ms, float Ps, int kIdx)
        {
            //this.DensityScale = DensityScale;
            this.Velocity = Velocity;
            this.Position = Position;
            this.Color = Color;
            PositionOld = Position;
            NaturalDensety = Dens;
            Density = NaturalDensety;
            ParticelSize = Ps;
            this.Mass = Ms;
            KernelIdx = kIdx;
        }
        /// <summary>
        /// Конструктор c копированием физики
        /// </summary>
        /// <param name="p">частица</param>
        public Particle(Particle p)
        {
            Velocity = p.Velocity;
            Position = p.Position;
            PositionOld = p.PositionOld;
            NaturalDensety = p.NaturalDensety;
            Density = p.Density;
            Mass = p.Mass;
            ParticelSize = p.ParticelSize;
            Color = p.Color;
            Acceleration = p.Acceleration;
            #region Вставка для напряжений
            GradVelocity=p.GradVelocity;
            StrainRateTensor=p.StrainRateTensor;
            EddyDeformationTensor=p.EddyDeformationTensor;
            DevStressTensor=p.EddyDeformationTensor;
            Plastic = p.Plastic;
            #endregion 
        }

        /// <summary>
        /// Конструктор для жестких границ
        /// </summary>
        /// <param name="Position"></param>
        /// <param name="Velocity"></param>
        /// <param name="Dns"></param>
        /// <param name="P"></param>
        public Particle(Vector2 Position, Vector2 Velocity, float Dns, float P, float M)
        {
            this.Velocity = Velocity;
            this.Position = Position;
            PositionOld = Position;
            Density = Dns;
            Pressure = P;
            Mass = M;
        }

        /// <summary>
        /// Копирование без сдвига
        /// </summary>
        /// <param name="p">источник</param>
        public void Copy(Particle p)
        {
            Velocity = p.Velocity;
            Position = p.Position;
            Color = p.Color;
            PositionOld = p.PositionOld;
            Acceleration = p.Acceleration;
            NaturalDensety = p.NaturalDensety;
            Density = p.Density;
            Mass = p.Mass;
            ParticelSize = p.ParticelSize;
            #region Вставка для напряжений
            GradVelocity = p.GradVelocity;
            StrainRateTensor = p.StrainRateTensor;
            EddyDeformationTensor = p.EddyDeformationTensor;
            DevStressTensor = p.EddyDeformationTensor;
            Plastic = p.Plastic;
            #endregion 
        }
        /// <summary>
        /// Копирование со сдвигом по Х
        /// </summary>
        /// <param name="p">источник</param>
        /// <param name="period">сдвиг</param>
        public void Copy(Particle p, float period)
        {
            Velocity = p.Velocity;
            Color = p.Color;
            NaturalDensety = p.NaturalDensety;
            Density = p.Density;
            Mass = p.Mass;
            Pressure = p.Pressure;
            ParticelSize = p.ParticelSize;
            // кинематика
            Position.X = p.Position.X + period;
            PositionOld.X = p.PositionOld.X + period;
            Position.Y = p.Position.Y;
            PositionOld.Y = p.PositionOld.Y;
            Acceleration = p.Acceleration;
            #region Вставка для напряжений
            GradVelocity = p.GradVelocity;
            StrainRateTensor = p.StrainRateTensor;
            EddyDeformationTensor = p.EddyDeformationTensor;
            DevStressTensor = p.EddyDeformationTensor;
            Plastic = p.Plastic;
            #endregion 
        }
        /// <summary>
        /// Вычисление тензора скоростей деформаций по тензору градиента скоростей
        /// </summary>
        public void ComputeStrainRateTensor()
        {
            StrainRateTensor = new Matrix2x2(
                GradVelocity.XX,
                0.5f * (GradVelocity.XY + GradVelocity.YX),
                0.5f * (GradVelocity.XY + GradVelocity.YX),
                GradVelocity.YY);
        }
        /// <summary>
        /// Вычисление вихривого тензора по тензору градиента скоростей
        /// </summary>
        public void ComputeEddyDeformationTensor()
        {
            EddyDeformationTensor = new Matrix2x2(
                0,
                0.5f * (GradVelocity.XY - GradVelocity.YX),
                0.5f * (GradVelocity.XY - GradVelocity.YX),
                0);
        }
        // Запись в файл
        public string Vector2ToString(Vector2 a)
        {
            return a.X.ToString() + " " + a.Y.ToString() + " ";
        }
        public string floatToString(float a)
        {
            return a.ToString() + " ";
        }
        public override string ToString()
        {
            string str = "";
            str += Vector2ToString(Velocity);
            str += Vector2ToString(Position);
            str += Vector2ToString(PositionOld);
            str += Vector2ToString(Acceleration);
            str += floatToString(Density);
            return str;
        }
        // чтение из файла
        public void StringToParticle(string str)
        {
            string s = str.Trim();
            string[] ss = s.Split(' ');
            float[] v = new float[ss.Length];
            for (int i = 0; i < v.Length; i++)
                v[i] = float.Parse(ss[i]);
            Velocity.X      = v[0];
            Velocity.Y      = v[1];
            Position.X      = v[2];
            Position.Y      = v[3];
            PositionOld.X   = v[4];
            PositionOld.Y   = v[5];
            Acceleration.X  = v[6];
            Acceleration.Y  = v[7];
            Density         = v[8];
        }
    }
}
