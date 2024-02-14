using System;
using System.Collections.Generic;
using System.Linq;

namespace ShowGraphic
{
    /// <summary>
    /// Данные о кривой
    /// </summary>
    public class GCurve 
    {
        /// <summary>
        /// название кривой - легенда
        /// </summary>
        public string Name;
        /// <summary>
        /// Список точек кривой
        /// </summary>
        public List<DPoint> points = new List<DPoint>();
        public GCurve() { Name = "без имени";  }
        public GCurve(string Name = "без имени") 
        { 
            this.Name = Name; 
            points.Clear(); 
        }
        /// <summary>
        /// количество кривых
        /// </summary>
        public int Count { get { return points.Count; } }
        /// <summary>
        /// постоянный шаг дискретизации интервала кривой
        /// </summary>
        public double Get_dx { get { return MinMax_X().Length / (Count - 1); } }
        /// <summary>
        /// добавить точку в кривую
        /// </summary>
        /// <param name="e">точка</param>
        public void Add(DPoint e) { points.Add(e); }
        /// <summary>
        /// добавить точку в кривую
        /// </summary>
        /// <param name="x">координата X</param>
        /// <param name="y">координата Y</param>
        public void Add(double x, double y) { points.Add(new DPoint(x,y)); }
        /// <summary>
        /// очистка точек кривой из списка
        /// </summary>
        public void Clear() { points.Clear(); }
        /// <summary>
        /// Получение точки мини-макс для аргумента кривой
        /// в параметре Х которой хранится минимум аргумента для кривой
        /// в параметре Y которой хранится максимум аргумента для кривой
        /// </summary>
        /// <returns>точка мини-макс </returns>
        public DPoint MinMax_X()
        {
            DPoint mm = new DPoint( points[0].X, points[0].X);
            foreach (DPoint p in points)
            {
                double a = p.X;
                if (mm.X > a) mm.X = a;
                if (mm.Y < a) mm.Y = a;
            }
            return mm;
        }
        /// <summary>
        /// Получение точки мини-макс для значения кривой
        /// в параметре Х которой хранится минимум значения для кривой
        /// в параметре Y которой хранится максимум значения для кривой
        /// </summary>
        /// <returns>точка мини-макс </returns>
        public DPoint MinMax_Y()
        {
            DPoint mm = new DPoint(points[0].Y, points[0].Y);
            foreach (DPoint p in points)
            {
                double a = p.Y;
                if (mm.X > a) mm.X = a;
                if (mm.Y < a) mm.Y = a;
            }
            return mm;
        }
    }
}
