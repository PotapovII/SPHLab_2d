using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Drawing;
using ShowGraphic;
using System.Threading.Tasks;

namespace SPHLionLIB
{
    /// <summary>
    /// Определяет виды граничных условий
    /// </summary>
    public enum ВoundaryType
    {
        Wall       // создание буфера зеркальным движением частиц 
       ,Periodic   // создание буфера периодических зеркальных частиц
       ,Mirror     // создание буфера подвижных зеркальных частиц   - ГУ скольжение
       ,Solid      // создание буфера неподвижных зеркальных частиц - ГУ прилипание
       ,ShiftWall  // создание буфера сдвинутых движением частиц - ГУ прилипание
    }
    /// <summary>
    /// Класс HeshGrid предназначенн для хеширования 
    /// частиц и нахождения ближайших соседей
    /// </summary>
    public class HeshGrid
    {
        /// <summary>
        /// Расчетная область
        /// </summary>
        public Area area;
        /// <summary>
        /// Активные частицы
        /// </summary>
        public Particle[] Particles;
        /// <summary>
        /// Фиктивные частицы
        /// </summary>
        public Particle[] BufParicles;
        /// <summary>
        /// Ближайшие соседи в ячейках
        /// система координат
        /// ---------> y
        /// |
        /// |
        /// V X
        /// </summary>
        public List<Particle>[,] grid;
        /// <summary>
        /// Ширина области
        /// </summary>
        public float WidthArea;
        /// <summary>
        /// Высота области
        /// </summary>
        public float HiegthArea;
        /// <summary>
        /// Масштаб сетки
        /// </summary>
        public float HeshSize;
        /// <summary>
        /// Размер ячейки сетки хеша
        /// </summary>
        public float HeshSizeСell;
        /// <summary>
        /// Размер ячейки сетки
        /// </summary>
        public float HeshSizeСellX;
        /// <summary>
        /// Размер ячейки сетки
        /// </summary>
        public float HeshSizeСellY;
        /// <summary>
        /// Число ячеек по Х
        /// </summary>
        public int countX;
        /// <summary>
        /// Число ячеек по Y
        /// </summary>
        public int countY;
        /// <summary>
        /// Радиус ядра сглаживания
        /// </summary>
        // float ParticelSize;
        /// <summary>
        /// Граничные условия
        /// </summary>
        ВoundaryType[] Вoundary;
        /// <summary>
        /// Флаг типа частиц-стенок: 0 - жесткие физические, 1 - всякие разные витруальные 
        /// </summary>
        int ВoundaryFlag;
        /// <summary>
        /// Для периодического движения - начальный сдвиг от границы ддя обеспечения зазора
        /// </summary>
        //float periodicStart;
        /// <summary>
        /// Для периодического движения - коррекция размещения в буфере для исключения наложений на границах
        /// </summary>
        float periodicShift = 0;

        const int left = 0;
        const int right = 1;
        const int bottom = 2;
        const int top = 3;

        public float XA, YA;

        static float ErrEpsilon = Physics.ErrEpsilon;
        /// <summary>
        /// Создаем объект для хеширования частиц
        /// </summary>
        /// <param name="WidthArea">ширина области</param>
        /// <param name="HiegthArea">высота области</param>
        /// <param name="HeshSize">предварительный размер ячейки</param>
        /// <param name="RightPosition">усечение области</param>

        public HeshGrid(Particle[] Particles, Area area, float HeshSize, float Scale, bool Corr)
        {
            this.area = area;
            XA = area.a.X;
            YA = area.a.Y;
            this.WidthArea = area.Width;
            this.HiegthArea = area.Heigh;
            this.HeshSize = HeshSize;

            if (Corr) periodicShift = area.ParticelSize * 0.5f;

            // | буфер 0 | HeshSizeСell: 1 | HeshSizeСell: 2 | HeshSizeСell: 3 | HeshSizeСell: ... | буфер: countX -1 |
            // Округляем в меньшую сторону
            countX = (int)Math.Floor(WidthArea / HeshSize);
            HeshSizeСellX = (WidthArea + ErrEpsilon) / countX;

            //Это прямоугольный хэш только для трубы!!!
            countY = (int)Math.Floor(HiegthArea / (HeshSize * Scale));
            HeshSizeСellY = (HiegthArea + ErrEpsilon) / countY;

            //А это квадратный для всего остального
            //countY = (int)(HiegthArea / HeshSizeСellX);
            //HeshSizeСellY = HeshSizeСellX;


            // Плюс два буфера границ
            countX += 2;
            countY += 2;
            // Создание грид хеша
            grid = new List<Particle>[countX, countY];
            for (int xx = 0; xx < countX; xx++)
                for (int yy = 0; yy < countY; yy++)
                        grid[xx, yy] = new List<Particle>();
            this.Particles = Particles;
            this.BufParicles = new Particle[3 * Particles.Length];
            // Индексирование расчетных и фиктивных частиц
            for (int i = 0; i < Particles.Length; i++)
                Particles[i].idx = i;
            for (int i = 0; i < BufParicles.Length; i++)
            {
                BufParicles[i] = new Particle();
                BufParicles[i].idx = i;
            }
        }
        /// <summary>
        /// Чистим хеш таблицу области
        /// </summary>
        public void ClearGrid()
        {
            Parallel.For(0, grid.GetLength(0), xx =>
            {
                for (int yy = 0; yy < grid.GetLength(1); yy++)
                    grid[xx, yy].Clear();
            });
        }
        public void BuildHeshGrid(ВoundaryType[] Вoundary, int ВoundaryFlag = 0)
        {
            this.Вoundary = Вoundary;
            this.ВoundaryFlag = ВoundaryFlag;
            ReBuildHeshGrid();
        }
        public void ReBuildHeshGrid()
        {
            // Очистка хеша
            ClearGrid();
            //Parallel.For(0, Particles.Length, i =>
            //{
            // Постройка хеша активных частиц
            for (int i = 0; i < Particles.Length; i++)
            {
                Particle pi = Particles[i];
                // сдвиг на 1, в нулевых и последних ячейках храним хеш для ГУ
                int ix = (int)((pi.Position.X - XA) / HeshSizeСellX) + 1;
                int iy = (int)((pi.Position.Y - YA) / HeshSizeСellY) + 1;
                ix = ix < 0 ? 1 : ix;
                iy = iy < 0 ? 1 : iy;
                grid[ix, iy].Add(pi);
            }
            //});
            // Формирование буфера под граничные частицы
            AddPointToBuff();
        }
        public int CountBP = 0;

        /// <summary>
        /// Метод буферизации частиц из правого и левого столбцов хеша на области
        /// </summary>
        public void AddPointToBuff()
        {
            CountBP = 0;
            float X0;
            float Y0;
            //float delta;
            if (Вoundary[left] == ВoundaryType.Periodic || Вoundary[top] == ВoundaryType.Periodic)
            {
                #region  Периодические условия есть!!!
                if (Вoundary[left] == ВoundaryType.Periodic)
                {
                    if (ВoundaryFlag != 0)
                    {
                        // генерация фиктивных частиц крышки и дна
                        #region не периодические граничные условия на дне
                        Y0 = 2 * area.a.Y;
                        float G = -Particle.G.Y;
                        if (Вoundary[bottom] == ВoundaryType.Wall)
                            for (int xx = 0; xx < countX; xx++)
                            //for (int xx = 1; xx < countX - 1; xx++)
                                foreach (Particle p in grid[xx, 1])
                                {
                                    Particle b = BufParicles[CountBP++];
                                    b.Copy(p);
                                    b.Velocity = -b.Velocity;
                                    b.Position.Y = Y0 - b.Position.Y;
                                    b.PositionOld.Y = Y0 - b.PositionOld.Y;
                                    grid[xx, 0].Add(b);
                                }
                        if (Вoundary[bottom] == ВoundaryType.ShiftWall)
                            for (int xx = 0; xx < countX; xx++)
                            //for (int xx = 1; xx < countX - 1; xx++)
                                foreach (Particle p in grid[xx, 1])
                                {
                                    Particle b = BufParicles[CountBP++];
                                    b.Copy(p);
                                    b.Velocity = -b.Velocity;
                                    b.Position.Y = b.Position.Y - HeshSizeСellY;
                                    b.PositionOld.Y = b.PositionOld.Y - HeshSizeСellY;
                                    grid[xx, 0].Add(b);
                                }
                        if (Вoundary[bottom] == ВoundaryType.Mirror)
                            //for (int xx = 0; xx < countX; xx++)
                            for (int xx = 1; xx < countX - 1; xx++)
                                foreach (Particle p in grid[xx, 1])
                                {
                                    Particle b = BufParicles[CountBP++];
                                    b.Copy(p);
                                    b.Velocity.Y = -b.Velocity.Y;
                                    b.Position.Y = Y0 - b.Position.Y;
                                    b.PositionOld.Y = Y0 - b.PositionOld.Y;
                                    b.density = p.NaturalDensety + (b.Pressure + 2 * (p.Position.Y - area.a.Y) * G * p.Density) / Particle.Rigidity;
                                    grid[xx, 0].Add(b);
                                }
                        if (Вoundary[bottom] == ВoundaryType.Solid)
                            //for (int xx = 0; xx < countX; xx++)
                            for (int xx = 1; xx < countX - 1; xx++)
                                foreach (Particle p in grid[xx, 1])
                                {
                                    Particle b = BufParicles[CountBP++];
                                    b.Copy(p);
                                    b.Velocity = Vector2.Zero;
                                    b.Position.Y = Y0 - b.Position.Y;
                                    b.PositionOld.Y = Y0 - b.PositionOld.Y;
                                    grid[xx, 0].Add(b);
                                }
                        #endregion
                    }
                    else
                    {
                        SetBottom();
                    }
                    if (ВoundaryFlag != 0)
                    {
                        #region не периодические граничные условия на крышке
                        Y0 = 2 * area.b.Y;
                        if (Вoundary[top] == ВoundaryType.Wall)
                            //for (int xx = 0; xx < countX; xx++)
                                for (int xx = 1; xx < countX - 1; xx++)
                                foreach (Particle p in grid[xx, countY - 2])
                                {
                                    Particle b = BufParicles[CountBP++];
                                    b.Copy(p);
                                    b.Velocity = -b.Velocity;
                                    b.Position.Y = Y0 - b.Position.Y;
                                    b.PositionOld.Y = Y0 - b.PositionOld.Y;
                                    grid[xx, countY - 1].Add(b);
                                }
                        if (Вoundary[top] == ВoundaryType.ShiftWall)
                            //for (int xx = 0; xx < countX; xx++)
                            for (int xx = 1; xx < countX - 1; xx++)
                                foreach (Particle p in grid[xx, countY - 2])
                                {
                                    Particle b = BufParicles[CountBP++];
                                    b.Copy(p);
                                    b.Velocity = -b.Velocity;
                                    b.Position.Y = b.Position.Y + HeshSizeСellY;
                                    b.PositionOld.Y = b.PositionOld.Y + HeshSizeСellY;
                                    grid[xx, countY - 1].Add(b);
                                }
                        if (Вoundary[top] == ВoundaryType.Mirror)
                            //for (int xx = 0; xx < countX; xx++)
                            for (int xx = 1; xx < countX - 1; xx++)
                                foreach (Particle p in grid[xx, countY - 2])
                                {
                                    Particle b = BufParicles[CountBP++];
                                    b.Copy(p);
                                    b.Velocity.Y = -b.Velocity.Y;
                                    b.Position.Y = Y0 - b.Position.Y;
                                    b.PositionOld.Y = Y0 - b.PositionOld.Y;
                                    grid[xx, countY - 1].Add(b);
                                }
                        if (Вoundary[top] == ВoundaryType.Solid)
                            //for (int xx = 0; xx < countX; xx++)
                            for (int xx = 1; xx < countX - 1; xx++)
                                foreach (Particle p in grid[xx, countY - 2])
                                {
                                    Particle b = BufParicles[CountBP++];
                                    b.Copy(p);
                                    b.Velocity = Vector2.Zero;
                                    b.Position.Y = Y0 - b.Position.Y;
                                    b.PositionOld.Y = Y0 - b.PositionOld.Y;
                                    grid[xx, countY - 1].Add(b);
                                }
                        #endregion
                    }
                    else
                    {
                        SetTop();
                    }
                    // Периодика с боков
                    // Здесь обеспечивается гладкость стыковки за счет корректровки скоростей
                    // Минусы при alpha и beta ускоряют поток, но плохо сглаживают; плюсы - тормозят и хорошо гладят
                    #region периодические  граничные условия на боковых стенках
                    if (Вoundary[left] == Вoundary[right])
                    {
                        float d;

                        float W = area.Width;
                        float alpha, beta, v;

                        for (int yy = 1; yy < countY-1; yy++)
                        {
                            // левая периодическая граница (берем из правого столбца области)
                            foreach (Particle p in grid[countX - 2, yy])
                                {
                                d = p.Position.X - (WidthArea + periodicShift);
                                if (d > (area.a.X - HeshSizeСellX))
                                {
                                   Particle b = BufParicles[CountBP++];
                                    //alpha = (area.b.X - p.Position.X) * 6.283185f / W;
                                    //beta = (area.a.X - d) * 6.283185f / (W /*+ periodicShift*/);
                                    b.Copy(p, -(WidthArea + periodicShift));
                                    //v = b.Velocity.X;
                                    // b.Velocity.X = -v / (1 - (float)Math.Sin(alpha) * (1 - (float)Math.Sin(beta)));
                                    grid[0, yy].Add(b);
                                }
                           }
                            // правая периодическая граница (берем из левого столбца области)
                            foreach (Particle p in grid[1, yy])
                            {
                                d = p.Position.X + (WidthArea + periodicShift);
                                if (d < (area.b.X + HeshSizeСellX))
                                {
                                    Particle b = BufParicles[CountBP++];
                                    //alpha = (area.a.X - p.Position.X) * 6.283185f / W;
                                    //beta = (area.b.X - d) * 6.283185f / (W /*+ periodicShift*/);
                                    b.Copy(p, (WidthArea + periodicShift));
                                    //v = b.Velocity.X;
                                    //b.Velocity.X = -v / (1 - (float)Math.Sin(alpha) * (1 - (float)Math.Sin(beta)));
                                    grid[countX - 1, yy].Add(b);
                                }
                            }
                        }

                    }
                    else
                        throw new Exception("Ошибка в горизонтальных ГУ");
                    #endregion
                }
                
                if (Вoundary[top] == ВoundaryType.Periodic)
                {
                    #region не периодические граничные условия на левой стенке
                    X0 = 2 * area.a.X;
                    if (Вoundary[left] == ВoundaryType.Wall)
                        for (int yy = 0; yy < countY; yy++)
                            foreach (Particle p in grid[1, yy])
                            {
                                Particle b = BufParicles[CountBP++];
                                b.Copy(p,-HeshSizeСellX);
                                //b.Position.X = X0 - b.Position.X;
                                //b.PositionOld.X = X0 - b.PositionOld.X;
                                b.Velocity = -b.Velocity;
                                grid[0, yy].Add(b);
                            }
                    if (Вoundary[left] == ВoundaryType.ShiftWall)
                        for (int yy = 0; yy < countY; yy++)
                            foreach (Particle p in grid[1, yy])
                            {
                                Particle b = BufParicles[CountBP++];
                                b.Copy(p, -HeshSizeСellX);
                                //b.Position.X = b.Position.X - HeshSizeСellX;
                                //b.PositionOld.X = b.PositionOld.X - HeshSizeСellX;
                                b.Velocity = -b.Velocity;
                                grid[0, yy].Add(b);
                            }
                    if (Вoundary[left] == ВoundaryType.Mirror)
                        for (int yy = 0; yy < countY; yy++)
                            foreach (Particle p in grid[1, yy])
                            {
                                Particle b = BufParicles[CountBP++];
                                b.Copy(p, -HeshSizeСellX);
                                //b.Position.X = X0 - b.Position.X;
                                //b.PositionOld.X = X0 - b.PositionOld.X;
                                b.Velocity.X = -b.Velocity.X;
                                grid[0, yy].Add(b);
                            }
                    if (Вoundary[left] == ВoundaryType.Solid)
                        for (int yy = 0; yy < countY; yy++)
                            foreach (Particle p in grid[1, yy])
                            {
                                Particle b = BufParicles[CountBP++];
                                b.Copy(p, -HeshSizeСellX);
                                //.Position.X = X0 - b.Position.X;
                                //b.PositionOld.X = X0 - b.PositionOld.X;
                                b.Velocity = Vector2.Zero;
                                grid[0, yy].Add(b);
                            }
                    #endregion
                    #region не периодические граничные условия на правой стенке
                    // координаты правой стенки области считаются от нуля 
                    X0 = 2 * area.b.X;
                    if (Вoundary[right] == ВoundaryType.Wall)
                        for (int yy = 0; yy < countY; yy++)
                            foreach (Particle p in grid[countX - 2, yy])
                            {
                                Particle b = BufParicles[CountBP++];
                                b.Copy(p, +HeshSizeСellX);
                                /*b.Copy(p);
                                b.Position.X = X0 - b.Position.X;
                                b.PositionOld.X = X0 - b.PositionOld.X;*/
                                b.Velocity = -b.Velocity;
                                grid[countX - 1, yy].Add(b);
                            }
                    if (Вoundary[right] == ВoundaryType.ShiftWall)
                        for (int yy = 0; yy < countY; yy++)
                            foreach (Particle p in grid[countX - 2, yy])
                            {
                                Particle b = BufParicles[CountBP++];
                                b.Copy(p, +HeshSizeСellX);
                                /*b.Copy(p);
                                b.Position.X = b.Position.X + HeshSizeСellX;
                                b.PositionOld.X = b.PositionOld.X + HeshSizeСellX;*/
                                b.Velocity = -b.Velocity;
                                grid[countX - 1, yy].Add(b);
                            }
                    if (Вoundary[right] == ВoundaryType.Mirror)
                        for (int yy = 0; yy < countY; yy++)
                            foreach (Particle p in grid[countX - 2, yy])
                            {
                                Particle b = BufParicles[CountBP++];
                                b.Copy(p, +HeshSizeСellX);
                                /*b.Copy(p);
                                b.Position.X = X0 - b.Position.X;
                                b.PositionOld.X = X0 - b.PositionOld.X;*/
                                b.Velocity.X = -b.Velocity.X;
                                grid[countX - 1, yy].Add(b);
                            }
                    if (Вoundary[right] == ВoundaryType.Solid)
                        for (int yy = 0; yy < countY; yy++)
                            foreach (Particle p in grid[countX - 2, yy])
                            {
                                Particle b = BufParicles[CountBP++];
                                b.Copy(p, +HeshSizeСellX);
                                /*b.Copy(p);
                                b.Position.X = X0 - b.Position.X;
                                b.PositionOld.X = X0 - b.PositionOld.X;*/
                                b.Velocity = Vector2.Zero;
                                grid[countX - 1, yy].Add(b);
                            }
                    #endregion
                    // периодика 
                    #region периодические граничные условия по вритикали!!!
                    if (Вoundary[top] == Вoundary[bottom])
                    {
                        for (int xx = 0; xx < countX; xx++)
                        {
                            // верхняя стенка
                            foreach (Particle p in grid[xx, countY - 2])
                            {
                                Particle b = BufParicles[CountBP++];
                                b.Copy(p);
                                b.Position.Y = b.Position.Y - HiegthArea;
                                b.PositionOld.Y = b.PositionOld.Y - HiegthArea;
                                grid[xx, 0].Add(b);
                            }
                            // нижняя стенка
                            foreach (Particle p in grid[xx, 1])
                            {
                                Particle b = BufParicles[CountBP++];
                                b.Copy(p);
                                b.Position.Y = b.Position.Y + HiegthArea;
                                b.PositionOld.Y = b.PositionOld.Y + HiegthArea;
                                grid[xx, countY - 1].Add(b);
                            }

                        }
                    }
                    else
                        throw new Exception("Ошибка в горизонтальных ГУ");
                    #endregion
                }
                #endregion
            }
            else
            {
                #region  Периодических условий нет
                #region не периодические граничные условия на левой стенке
                X0 = 2 * area.a.X;
                if (Вoundary[left] == ВoundaryType.Wall)
                    for (int yy = 0; yy < countY; yy++)
                        foreach (Particle p in grid[1, yy])
                        {
                            Particle b = BufParicles[CountBP++];
                            b.Copy(p);
                            b.Position.X = X0 - b.Position.X;
                            b.PositionOld.X = X0 - b.PositionOld.X;
                            b.Velocity = -b.Velocity;
                            grid[0, yy].Add(b);
                        }
                if (Вoundary[left] == ВoundaryType.ShiftWall)
                    for (int yy = 0; yy < countY; yy++)
                        foreach (Particle p in grid[1, yy])
                        {
                            Particle b = BufParicles[CountBP++];
                            b.Copy(p);
                            b.Position.X = b.Position.X - HeshSizeСellX;
                            b.PositionOld.X = b.PositionOld.X - HeshSizeСellX;
                            b.Velocity = -b.Velocity;
                            grid[0, yy].Add(b);
                        }
                if (Вoundary[left] == ВoundaryType.Mirror)
                    for (int yy = 0; yy < countY; yy++)
                        foreach (Particle p in grid[1, yy])
                        {
                            Particle b = BufParicles[CountBP++];
                            b.Copy(p);
                            b.Position.X = X0 - b.Position.X;
                            b.PositionOld.X = X0 - b.PositionOld.X;
                            b.Velocity.X = -b.Velocity.X;
                            grid[0, yy].Add(b);
                        }
                if (Вoundary[left] == ВoundaryType.Solid)
                    for (int yy = 0; yy < countY; yy++)
                        foreach (Particle p in grid[1, yy])
                        {
                            Particle b = BufParicles[CountBP++];
                            b.Copy(p);
                            b.Position.X = X0 - b.Position.X;
                            b.PositionOld.X = X0 - b.PositionOld.X;
                            b.Velocity = Vector2.Zero;
                            grid[0, yy].Add(b);
                        }
                #endregion
                #region не периодические граничные условия на правой стенке
                // координаты правой стенки области считаются от нуля 
                X0 = 2 * area.b.X;
                if (Вoundary[right] == ВoundaryType.Wall)
                    for (int yy = 0; yy < countY; yy++)
                        foreach (Particle p in grid[countX - 2, yy])
                        {
                            Particle b = BufParicles[CountBP++];
                            b.Copy(p);
                            b.Position.X = X0 - b.Position.X;
                            b.PositionOld.X = X0 - b.PositionOld.X;
                            b.Velocity = Vector2.Zero;
                            grid[countX - 1, yy].Add(b);
                        }
                if (Вoundary[right] == ВoundaryType.ShiftWall)
                    for (int yy = 0; yy < countY; yy++)
                        foreach (Particle p in grid[countX - 2, yy])
                        {
                            Particle b = BufParicles[CountBP++];
                            b.Copy(p);
                            b.Position.X = b.Position.X + HeshSizeСellX;
                            b.PositionOld.X = b.PositionOld.X + HeshSizeСellX;
                            b.Velocity = -b.Velocity;
                            grid[countX - 1, yy].Add(b);
                        }
                if (Вoundary[right] == ВoundaryType.Mirror)
                    for (int yy = 0; yy < countY; yy++)
                        foreach (Particle p in grid[countX - 2, yy])
                        {
                            Particle b = BufParicles[CountBP++];
                            b.Copy(p);
                            b.Position.X = X0 - b.Position.X;
                            b.PositionOld.X = X0 - b.PositionOld.X;
                            b.Velocity.X = -b.Velocity.X;
                            grid[countX - 1, yy].Add(b);
                        }
                if (Вoundary[right] == ВoundaryType.Solid)
                    for (int yy = 0; yy < countY; yy++)
                        foreach (Particle p in grid[countX - 2, yy])
                        {
                            Particle b = BufParicles[CountBP++];
                            b.Copy(p);
                            b.Position.X = X0 - b.Position.X;
                            b.PositionOld.X = X0 - b.PositionOld.X;
                            b.Velocity = Vector2.Zero;
                            grid[countX - 1, yy].Add(b);
                        }
                #endregion
                            // фиктивные частицы
                if (ВoundaryFlag != 0)
                {
                    #region не периодические граничные условия на дне
                    Y0 = 2 * area.a.Y;
                    float G = -Particle.G.Y;
                    if (Вoundary[bottom] == ВoundaryType.Wall)
                        for (int xx = 1; xx < countX - 1; xx++)
                            foreach (Particle p in grid[xx, 1])
                            {
                                Particle b = BufParicles[CountBP++];
                                b.Copy(p);
                                b.Velocity = -b.Velocity;
                                b.Position.Y = Y0 - b.Position.Y;
                                b.PositionOld.Y = Y0 - b.PositionOld.Y;
                                grid[xx, 0].Add(b);
                            }
                    if (Вoundary[bottom] == ВoundaryType.ShiftWall)
                        for (int xx = 1; xx < countX - 1; xx++)
                            foreach (Particle p in grid[xx, 1])
                            {
                                Particle b = BufParicles[CountBP++];
                                b.Copy(p);
                                b.Velocity = -b.Velocity;
                                b.Position.Y = b.Position.Y - HeshSizeСellY;
                                b.PositionOld.Y = b.PositionOld.Y - HeshSizeСellY;
                                grid[xx, 0].Add(b);
                            }
                    if (Вoundary[bottom] == ВoundaryType.Mirror)
                        for (int xx = 1; xx < countX - 1; xx++)
                            foreach (Particle p in grid[xx, 1])
                            {
                                Particle b = BufParicles[CountBP++];
                                b.Copy(p);
                                b.Velocity.Y = -b.Velocity.Y;
                                b.Position.Y = Y0 - b.Position.Y;
                                b.PositionOld.Y = Y0 - b.PositionOld.Y;
                                b.density = p.NaturalDensety + (b.Pressure + 2 * (p.Position.Y - area.a.Y) * G * p.Density) / Particle.Rigidity;
                                grid[xx, 0].Add(b);
                            }
                    if (Вoundary[bottom] == ВoundaryType.Solid)
                        for (int xx = 1; xx < countX - 1; xx++)
                            foreach (Particle p in grid[xx, 1])
                            {
                                Particle b = BufParicles[CountBP++];
                                b.Copy(p);
                                b.Velocity = Vector2.Zero;
                                b.Position.Y = Y0 - b.Position.Y;
                                b.PositionOld.Y = Y0 - b.PositionOld.Y;
                                grid[xx, 0].Add(b);
                            }
                    #endregion
                }
                else
                    SetBottom();
                if (ВoundaryFlag != 0)
                {
                    #region не периодические граничные условия на крышке
                    Y0 = 2 * area.b.Y;
                if (Вoundary[top] == ВoundaryType.Wall)
                    for (int xx = 1; xx < countX - 1; xx++)
                        foreach (Particle p in grid[xx, countY - 2])
                        {
                            Particle b = BufParicles[CountBP++];
                            b.Copy(p);
                            b.Velocity = -b.Velocity;
                            b.Position.Y = Y0 - b.Position.Y;
                            b.PositionOld.Y = Y0 - b.PositionOld.Y;
                            grid[xx, countY - 1].Add(b);
                        }
                if (Вoundary[top] == ВoundaryType.ShiftWall)
                    for (int xx = 1; xx < countX - 2; xx++)
                        foreach (Particle p in grid[xx, countY - 2])
                        {
                            Particle b = BufParicles[CountBP++];
                            b.Copy(p);
                            b.Velocity = -b.Velocity;
                            b.Position.Y = b.Position.Y + HeshSizeСellY;
                            b.PositionOld.Y = b.PositionOld.Y + HeshSizeСellY;
                            grid[xx, countY - 1].Add(b);
                        }
                if (Вoundary[top] == ВoundaryType.Mirror)
                    for (int xx = 1; xx < countX - 1; xx++)
                        foreach (Particle p in grid[xx, countY - 2])
                        {
                            Particle b = BufParicles[CountBP++];
                            b.Copy(p);
                            b.Velocity.Y = -b.Velocity.Y;
                            b.Position.Y = Y0 - b.Position.Y;
                            b.PositionOld.Y = Y0 - b.PositionOld.Y;
                            grid[xx, countY - 1].Add(b);
                        }
                if (Вoundary[top] == ВoundaryType.Solid)
                    for (int xx = 1; xx < countX - 1; xx++)
                        foreach (Particle p in grid[xx, countY - 2])
                        {
                            Particle b = BufParicles[CountBP++];
                            b.Copy(p);
                            b.Velocity = Vector2.Zero;
                            b.Position.Y = Y0 - b.Position.Y;
                            b.PositionOld.Y = Y0 - b.PositionOld.Y;
                            grid[xx, countY - 1].Add(b);
                        }
                #endregion
                }
                else
                    SetTop();
            #endregion
            }
        }

        //float[] P1;
        //float[] P2;

        /// <summary>
        /// Создание неподвижных физических частиц дна
        /// </summary>
        public void SetBottom()
        {
          /*  if(P1==null)
            {
                P1 = new float[countX];
                P2 = new float[countX];
            }
            for (int xx = 0; xx < countX - 1; xx++)
            {
                int count1 = 0;
                P1[xx] = 0;
                
                foreach (Particle p in grid[xx, 1])
                {
                    P1[xx] += p.Density;
                    count1++;
                }
                if (count1 > 0)
                    P1[xx] /= count1;
                else
                    P1[xx] = Physics.NaturalDensetyF;// * area.ParticelSize; 
                P2[xx] = 0;
                int count2 = 0;
                foreach (Particle p in grid[xx, 2])
                {
                    P2[xx] += p.Density;
                    count2++;
                }
                if (count2 > 0) 
                    P2[xx] /= count2;
                else
                    P2[xx] = Physics.NaturalDensetyF;
            }*/

            float ParticelSize = area.ParticelSize;
            float ParticelSizeY = (float)Math.Cos(Math.PI / 6) * ParticelSize;
            float ErrEpsilon = Physics.ErrEpsilon;

            float shiftX = ParticelSize / 2f;
            float shiftY = ParticelSizeY / 2f;
            float startX = area.a.X - HeshSizeСellX + shiftX;
            float startY = area.a.Y - shiftY;
            float endX = area.b.X + HeshSizeСellX;
            float P = area.Pmax;

            // первый слой
            float y = startY;
            for (float x = startX; x < endX; x += ParticelSize)
            {
                Particle b = BufParicles[CountBP++];
                b.Velocity = Vector2.Zero;
                b.Position.X = x;
                b.Position.Y = y;
                b.Pressure = P + 0.5f * Math.Abs(Physics.NaturalDensetyF * Particle.G.Y * ParticelSizeY); ;
                b.density = b.Pressure / Particle.Rigidity + Physics.NaturalDensetyF;
                // сдвиг на 1, в нулевых и последних ячейках храним хеш для ГУ
                int xx = (int)((x - XA) / HeshSizeСellX) + 1;
                xx = xx < 0 ? 0 : xx;
               /* b.Density = P1[xx];
                if (b.Density < Physics.ErrEpsilon)
                    b.Density = Physics.NaturalDensetyF;// * area.ParticelSize;*/
                grid[xx, 0].Add(b);
            }

            //Второй слой
            y -= ParticelSizeY;
            for (float x = startX - shiftX; x < endX; x += ParticelSize)
            {

                Particle b = BufParicles[CountBP++];
                b.Velocity = Vector2.Zero;
                b.Position.X = x;
                b.Position.Y = y;
                b.Pressure = P + 1.5f * Math.Abs(Physics.NaturalDensetyF * Particle.G.Y * ParticelSizeY);
                b.density = b.Pressure / Particle.Rigidity + Physics.NaturalDensetyF;

                int xx = ((int)((x - XA) / HeshSizeСellX) + 1);
                xx = xx < 0 ? 0 : xx;
               /* b.Density = P2[xx];
                //b.Density = P1[xx];
                if (b.Density < Physics.ErrEpsilon)
                    b.Density = Physics.NaturalDensetyF;// * area.ParticelSize;*/
                grid[xx, 0].Add(b);
            }
        }
        /// <summary>
        /// Создание неподвижных физических частиц верхней крышки
        /// </summary>
        public void SetTop()
        {
           /* if (P1 == null)
            {
                P1 = new float[countX];
                P2 = new float[countX];
            }
            for (int xx = 0; xx < countX - 1; xx++)
            {
                int count1 = 0;
                P1[xx] = 0;

                foreach (Particle p in grid[xx, countY-2])
                {
                    P1[xx] += p.Density;
                    count1++;
                }
                if (count1 > 0)
                    P1[xx] /= count1;
                else
                    P1[xx] = Physics.NaturalDensetyF;// * area.ParticelSize;
                P2[xx] = 0;
                int count2 = 0;
                foreach (Particle p in grid[xx, countY-3])
                {
                    P2[xx] += p.Density;
                    count2++;
                }
                if (count2 > 0)
                    P2[xx] /= count2;
                else
                    P2[xx] = Physics.NaturalDensetyF; 
            }*/
            
            float ParticelSize = area.ParticelSize;
            float ParticelSizeY = (float)Math.Cos(Math.PI / 6) * ParticelSize;
            float ErrEpsilon = Physics.ErrEpsilon;

            float shiftX = ParticelSize / 2f;
            float shiftY = ParticelSizeY / 2f;
            int poz = 1;
            if ((area.b.Y - area.a.Y) % ParticelSizeY == 0) poz = 0;

            float startX = area.a.X - HeshSizeСellX + poz * shiftX;
            float startY = area.b.Y + shiftY;
            float endX = area.b.X + HeshSizeСellX;
            float P = area.Pmax;
            // первый слой
            float y = startY;
            for (float x = startX; x < endX; x += ParticelSize)
            {

                Particle b = BufParicles[CountBP++];
                b.Velocity = Vector2.Zero;
                b.Position.X = x;
                b.Position.Y = y;
                b.Pressure = P + 0.5f * Math.Abs(Physics.NaturalDensetyF * Particle.G.Y * ParticelSizeY); 
                b.density = b.Pressure / Particle.Rigidity + Physics.NaturalDensetyF;

                int xx = (int)((x - XA) / HeshSizeСellX) + 1;
                xx = xx < 0 ? 0 : xx;
               /* b.Density = P1[xx];
                if (b.Density < Physics.ErrEpsilon)
                    b.Density = Physics.NaturalDensetyF;// * area.ParticelSize;*/
                grid[xx, countY - 1].Add(b);
            }

            //Второй слой
            y += ParticelSizeY;
            for (float x = startX + shiftX; x < endX; x += ParticelSize)
            {

                Particle b = BufParicles[CountBP++];
                b.Velocity = Vector2.Zero;
                b.Position.X = x;
                b.Position.Y = y;
                b.Pressure = P + 1.5f * Math.Abs(Physics.NaturalDensetyF * Particle.G.Y * ParticelSizeY);
                b.density = b.Pressure / Particle.Rigidity + Physics.NaturalDensetyF;

                int xx = ((int)((x - XA) / HeshSizeСellX) + 1);
                xx = xx < 0 ? 0 : xx;
                /*b.Density = P2[xx];
                //b.Density = P1[xx];
                if (b.Density < Physics.ErrEpsilon)
                    b.Density = Physics.NaturalDensetyF;// * area.ParticelSize;*/
                grid[xx, countY - 1].Add(b);
            }
        }
        /// <summary>
        /// Найти соседей
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public List<Particle> FindNeighboringParticles(Particle point)
        {
            List<Particle> alist = new List<Particle>();
            int ix = (int)Math.Floor((point.Position.X - XA) / HeshSizeСellX) + 1;
            int iy = (int)Math.Floor((point.Position.Y - YA) / HeshSizeСellY) + 1;
            int bondx = 3;
            int bondy = 3;
            int stx = 1;
            int sty = 1;
            if (iy == 0) sty = 0;
            if (ix == 0) stx = 0;
            if ((iy == countY - 1) || (iy == 0)) bondy = 2;
            if ((ix == countX - 1) || (ix == 0)) bondx = 2;
            for (int x = 0; x < bondx; x++)
                for (int y = 0; y < bondy; y++)
                    alist.AddRange(grid[ix - stx + x, iy - sty + y]);
            return alist;
        }
        /// <summary>
        /// Ликвидация вылета частиц из расчетной области
        /// </summary>
        /// <param name="p"></param>
        public void OutControl(Particle p)
        {
            float ax = area.a.X;
            float ay = area.a.Y;
            float bx = area.b.X;
            float by = area.b.Y;
            // боковые границы
            // сначала не периодика
            if (Вoundary[left] != ВoundaryType.Periodic)
            {
                float Out = p.Position.X - ax;
                if (Out < 0)
                {
                    p.Position.X = ax + ErrEpsilon;
                    p.Velocity.X = - p.Velocity.X;
                }
                Out = bx - p.Position.X;
                if (Out < 0)
                {
                    p.Position.X = (bx - ErrEpsilon);
                    p.Velocity.X = -p.Velocity.X;
                }
            }
            //теперь периодика
            else 
            {
                //Обеспечение гладкости стыковки за счет корректриорвки скоростей
                float halfPs = 0;
                //float halfPs = 0.5f * area.ParticelSize;
                //float W = bx - ax;// + halfPs;
                // float alpha, beta; 
                // float v = p.Velocity.X;

                float Out = ax - p.Position.X;
                if (Out > halfPs)
                {
                   // alpha = -Out * 6.2832f / W;
                    //beta = -(Out - halfPs) * 6.2831f / (W + halfPs);
                    p.Position.X += (area.Width + halfPs);
                   // p.Velocity.X = v / (1 - (float)Math.Sin(alpha) * (1 - (float)Math.Sin(beta)));
                }

                Out = p.Position.X - bx;
                if (Out > halfPs)
                {
                   // alpha = Out * 6.2832f / W;
                   // beta = (Out - halfPs) * 6.2831f / (W + halfPs);
                    p.Position.X -= (area.Width + halfPs);
                    //p.Velocity.X = v / (1 - (float)Math.Sin(alpha) * (1 - (float)Math.Sin(beta)));
                }
            }
            // дно и крышка
            if (Вoundary[top] != ВoundaryType.Periodic)
            {
                if (p.Position.Y < ay)
                {
                    p.Position.Y = ay + ErrEpsilon;
                    p.Velocity.Y = -p.Velocity.Y;
                }
                if (p.Position.Y > by)
                {
                    p.Position.Y = by - ErrEpsilon;
                    p.Velocity.Y = -p.Velocity.Y;
                }
            }
            else 
            {
                if (p.Position.Y < ay)
                {
                    p.Position.Y += area.Heigh;

                }
                if (p.Position.Y > by)
                {
                    p.Position.Y -= area.Heigh;               
                }
            }
        }
        /// <summary>
        /// Тестовая отрисовка грида сетки
        /// </summary>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">Контекст</param>
        /// <param name="startx">левый край "экрана"</param>
        /// <param name="bottomy">дно "экрана"</param>
        /// <param name="scaleVelocity">масштаб скорости</param>
        /// <param name="scale">масштаб</param>
        public void ShowBody(Graphics g, int scaleVelocity)
        {
            SolidBrush Br;
            SolidBrush B = new SolidBrush(Color.Black);
            SolidBrush R = new SolidBrush(Color.Red);
            Pen p = new Pen(B, 3);
            Pen pr = new Pen(R, 1);
            int x, y;
            ShowMeshHesh(g);
            int psize = 4;
            for (int xx = 0; xx < countX; xx++)
            {
                for (int yy = 0; yy < countY; yy++)
                {
                    string str = " " + xx.ToString() + " " + yy.ToString();
                    for (int n = 0; n < grid[xx, yy].Count; n++)
                    {
                        Particle pi = grid[xx, yy][n];
                        x = CoScale.ScaleX(pi.Position.X);
                        y = CoScale.ScaleY(pi.Position.Y);
                        int u = (int)(pi.Velocity.X * scaleVelocity);
                        int v = (int)(pi.Velocity.Y * scaleVelocity);
                        if (pi.Color == 0)
                            Br = R;
                        else
                            Br = B;
                        Point a = new Point(x + 1, y + 1);
                        Point b = new Point(x, y);
                        Point c = new Point(x + u, y - v);
                        g.DrawString(str, new Font("Arial", 10), B, b);
                        g.FillRectangle(Br, x - psize / 2, y - psize / 2, psize, psize);
                        g.DrawLine(new Pen(Br, 1), b, c);
                    }
                }
            }
        }

        /// <summary>
        /// Отрисовка сетки хеширования
        /// </summary>
        /// <param name="g"></param>
        /// <param name="startx"></param>
        /// <param name="bottomy"></param>
        /// <param name="scaleVelocity"></param>
        /// <param name="scale"></param>
        public void ShowMeshHesh(Graphics g)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            SolidBrush R = new SolidBrush(Color.Red);
            Pen p = new Pen(B, 3);
            Pen pr = new Pen(R, 1);
            int xa = CoScale.ScaleX(area.a.X);
            int ya = CoScale.ScaleY(area.a.Y);
            int xb = CoScale.ScaleX(area.b.X);
            int yb = CoScale.ScaleY(area.b.Y);
            // расчетная область
            g.DrawRectangle(p, xa, yb, xb-xa, ya-yb);
            // хеш сетка
            xa = CoScale.ScaleX(area.a.X - HeshSizeСellX);
            xb = CoScale.ScaleX(area.b.X + HeshSizeСellX);
            ya = CoScale.ScaleY(area.a.Y - HeshSizeСellY);
            yb = CoScale.ScaleY(area.b.Y + HeshSizeСellY);

            for (int xx = 0; xx <= countX; xx++)
            {
                int gx = CoScale.ScaleX(area.a.X + HeshSizeСellX * (xx-1));
                Point a = new Point(gx, ya);
                Point b = new Point(gx, yb);
                g.DrawLine(pr, a, b);
                for (int yy = 0; yy <= countY; yy++)
                {
                    int gy = CoScale.ScaleY(area.a.Y + HeshSizeСellY * (yy-1));
                    Point c = new Point(xa, gy);
                    Point d = new Point(xb, gy);
                    string str = xx.ToString() + " " + yy.ToString();
                    g.DrawLine(pr, c, d);
                }
            }
        }
        public void ShowSelectBody(int ix, Graphics g, int scaleVelocity)
        {
            ShowMeshHesh(g);
            Particle point = Particles[ix];
            List<Particle> list = FindNeighboringParticles(point);
            SolidBrush R = new SolidBrush(Color.Red);
            Pen p = new Pen(R, 1);
            int x, y;
            int psize = 4;
            for (int i = 0; i < list.Count; i++)
            {
                Particle pi = list[i];
                x = CoScale.ScaleX(pi.Position.X);
                y = CoScale.ScaleY(pi.Position.Y);
                int u = (int)(pi.Velocity.X * scaleVelocity);
                int v = (int)(pi.Velocity.Y * scaleVelocity);
                Point a = new Point(x + 1, y + 1);
                Point b = new Point(x,  y);
                Point c = new Point(x + u, y - v);
                g.DrawString(pi.idx.ToString(), new Font("Arial", 10), R, a);
                g.FillRectangle(R, x - psize / 2, y - psize / 2, psize, psize);
                g.DrawLine(p, b, c);
            }
        }
        public void ShowSelectRow(int yy, Graphics g, int scaleVelocity)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            SolidBrush R = new SolidBrush(Color.Red);
            ShowMeshHesh(g);
            int x, y, psize = 4;
            yy = yy % countY;
            for (int xx = 0; xx < countX; xx++)
            {
                for (int n = 0; n < grid[xx, yy].Count; n++)
                {
                    string str = " " + xx.ToString() + " " + yy.ToString();
                    Particle pi = grid[xx, yy][n];
                    x = CoScale.ScaleX(pi.Position.X);
                    y = CoScale.ScaleY(pi.Position.Y);
                    int u = (int)(pi.Velocity.X * scaleVelocity);
                    int v = (int)(pi.Velocity.Y * scaleVelocity);
                    Point a = new Point(x + 1, y + 1);
                    Point b = new Point(x,  y);
                    Point c = new Point(x + u, y - v);
                    g.DrawString(str, new Font("Arial", 10), B, b);
                    g.FillRectangle(B, x - psize / 2, y - psize / 2, psize, psize);
                    g.DrawLine(new Pen(B, 1), b, c);
                }
            }
        }
        public void ShowSelectCol(int xx, Graphics g, int scaleVelocity)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            SolidBrush R = new SolidBrush(Color.Red);
            ShowMeshHesh(g);
            int x, y, psize = 4;
            xx = xx % countX;
            for (int yy = 0; yy < countY; yy++)
            {
                for (int n = 0; n < grid[xx, yy].Count; n++)
                {
                    string str = " " + xx.ToString() + " " + yy.ToString();
                    Particle pi = grid[xx, yy][n];
                    x = CoScale.ScaleX(pi.Position.X);
                    y = CoScale.ScaleY(pi.Position.Y);
                    int u = (int)(pi.Velocity.X * scaleVelocity);
                    int v = (int)(pi.Velocity.Y * scaleVelocity);
                    Point a = new Point(x + 1, y + 1);
                    Point b = new Point(x, y);
                    Point c = new Point(x + u, y - v);
                    g.DrawString(str, new Font("Arial", 10), B, b);
                    g.FillRectangle(B, x - psize / 2, y - psize / 2, psize, psize);
                    g.DrawLine(new Pen(B, 1), b, c);
                }
            }
        }
    }
}
