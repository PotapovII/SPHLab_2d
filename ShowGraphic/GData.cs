using System;
using System.Collections.Generic;
using System.Linq;

namespace ShowGraphic
{
    /// <summary>
    /// Контейнер кривых
    /// </summary>
    public class GData
    {
       
        /// <summary>
        /// Список кривых 
        /// </summary>
        public List<GCurve> curves = new List<GCurve>();
        /// <summary>
        /// Добавление кривой
        /// </summary>
        /// <param name="e"></param>
        public void Add(GCurve e) { curves.Add(e); }
        /// <summary>
        /// Очистка контейнера
        /// </summary>
        public void Clear() { curves.Clear(); }
        /// <summary>
        /// Получение точки мини-макс для аргумента всех кривых
        /// в параметре Х которой хранится минимум аргумента для всех кривых
        /// в параметре Y которой хранится максимум аргумента для всех кривых
        /// </summary>
        /// <returns>точка мини-макс </returns>
        public DPoint MinMax_X()
        {
            DPoint mm = curves[0].MinMax_X();
            foreach (GCurve c in curves)
            {
                DPoint cmm = c.MinMax_X();
                if (mm.X > cmm.X) mm.X = cmm.X;
                if (mm.Y < cmm.Y) mm.Y = cmm.Y;
            }
            return mm;
        }
        /// <summary>
        /// Получение точки мини-макс для значения функции всех кривых
        /// в параметре Х которой хранится минимум значения функции для всех кривых
        /// в параметре Y которой хранится максимум значения функции для всех кривых
        /// </summary>
        /// <returns>точка мини-макс </returns>
        public DPoint MinMax_Y()
        {
            DPoint mm = curves[0].MinMax_Y();
            foreach (GCurve c in curves)
            {
                DPoint cmm = c.MinMax_Y();
                if (mm.X > cmm.X) mm.X = cmm.X;
                if (mm.Y < cmm.Y) mm.Y = cmm.Y;
            }
            return mm;
        }
    }
}
