using System;
namespace ShowGraphic
{
    /// <summary>
    /// точка графика двойной точности
    /// </summary>
    public struct DPoint
    {
        /// <summary>
        /// значение аргумента в точке
        /// </summary>
        public double X;
        /// <summary>
        /// значение функции в точке
        /// </summary>
        public double Y;
        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public DPoint(double X,double Y) { this.X = X; this.Y = Y; }
        public static DPoint Zero { get { return new DPoint(0,0); } }
        /// <summary>
        /// вычисление интевала для точек мини-макс
        /// </summary>
        public double Length  { get {return Y-X; }}
        /// <summary>
        /// вычисление максимума
        /// </summary>
        public double Max { get { return  Math.Max(Math.Abs(X),Math.Abs(Y)); } }
        public string ToString() { return "X= " + X.ToString() + " Y = " + Y.ToString(); }
    }
}
