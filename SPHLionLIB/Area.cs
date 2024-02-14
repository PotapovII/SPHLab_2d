using System;
using System.Numerics;


namespace SPHLionLIB
{
    /// <summary>
    /// Область определения задачи/тела
    /// </summary>
    public class Area
    {
        /// <summary>
        /// Нижний левый угол области
        /// </summary>
        public Vector2 a;
        /// <summary>
        /// Верхний правый угол области
        /// </summary>
        public Vector2 b;
        /// <summary>
        /// Параметр уплотнения среды 0 ~ DensityScale ~ 2
        /// </summary>
        public static float DensityScale = 1;
        /// <summary>
        /// Размер частицы
        /// </summary>
        /// 
        float ps;

        public float Pmax = 0;

        public float ParticelSize { get { return this.ps; } set { ps = value; } }
        /// <summary>
        /// Параметр разделения области  0 ~ alpha ~ 1.1 по вертикали
        /// </summary>
        public static float alpha = 0;

        public float Width { get { return b.X - a.X; } }
        public float Heigh { get { return b.Y - a.Y; } }
        public Area(Vector2 a,Vector2 b)
        {
            this.a = a; this.b = b;
        }

        public Area(Vector2 a, Vector2 b, float pSize)
        {
            this.a = a;
            this.b = b;
            this.ps = pSize;

        }
    }
}
