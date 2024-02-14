using System;
using System.Numerics;
using System.Drawing;
using ShowGraphic;
using System.Collections.Generic;

namespace SPHLionLIB
{
    /// <summary>
    /// Установка начального состояния тела из частиц
    /// </summary>
    public class SphArea
    {
        public static Area area;
        /// <summary>
        /// Максимально возможное количество частиц
        /// </summary>
        public const int CountMaxParticles = 128000;
        /// <summary>
        /// Вычисляемое количество частиц
        /// </summary>
        int Count = 0;
        /// <summary>
        /// Количество частиц в линии
        /// </summary>
        public int CountParticleLine;
        /// <summary>
        /// Частицы
        /// </summary>
        public Particle[] Particles = new Particle[CountMaxParticles];
        /// <summary>
        /// Характерный размер частиц для задачи
        /// </summary>
        public float ParticelSize;
        /// <summary>
        /// Коэффициент объемной упругости  
        /// </summary>
        public float Rigidity;

        public List<string> Kernels;
        public int kInd;
        public int kSize;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Mass">Масса частицы</param>
        /// <param name="NaturalDensety">Натуральная плотность</param>
        /// <param name="Rigidity">Коэффициент жесткости (сжатия)</param>
        /// <param name="ParticelSize">Размер частицы</param>
        /// <param name="SmoothingOrder">Порядок сглаживания ядра</param>
        public SphArea(float Mass, float NaturalDensety, float Rigidity, float ParticelSize, List<string> k, int ind)
        {
            //Particle.Mass = Mass;
            //Particle.NaturalDensety = NaturalDensety;
            this.Rigidity = Rigidity;
            Particle.Rigidity = Rigidity;
            //Particle.ParticelSize = ParticelSize;
            // Particle.SmoothingOrder = SmoothingOrder;
            this.ParticelSize = ParticelSize;
            Kernels = k;
            kInd = ind;
        }
        /// <summary>
        /// Распределение частиц грунта в начальный момент времени в прямоугольнике
        /// </summary>
        /// <param name="WidthBody">ширина</param>
        /// <param name="HeightBody">высота</param>
        /// <param name="CountParticleLine">количество частиц в линии</param>
        /// <param name="ParticelSize">размер частицы</param>
        /// <param name="ShiftX">сдвиг прямоугольника по оси Х</param>
        public void SetBobyMono(Area rarea, Vector2 Velocity0, int CountParticleLine, bool flagPeriodityG, bool flagPeriodityV)
        {
            area = rarea;
            float ShiftX = area.a.X;
            float ShiftY = area.a.Y;
            float WidthBody = area.Width;
            float HeightBody = area.Heigh;
            float Mass = (Physics.NaturalDensetyF * (ParticelSize * ParticelSize /* ParticelSize*/) * 0.86602541f);
            float ParticelSizeY = (0.8660254f * ParticelSize); //Для треугольной сетки
            //float ParticelSizeY = ParticelSize; // Для квадратной сетки
            float ErrEpsilon = Physics.ErrEpsilon;
            Vector2 Position;
            Vector2 Velocity = Velocity0;
            // шаг заполнения объема
            float shiftX = ParticelSize / 2f;
            float shiftY = ParticelSizeY / 2f;
            float shift2 = shiftX;
            //краевые точки заполнения области
            float startX = area.a.X + ParticelSize;
            float startY = area.a.Y + shiftY;
            float endX = area.b.X + ErrEpsilon;
            float endY = area.b.Y + ErrEpsilon;
            int sign = 1;
            //Для периодических границ уравновешиваем зазоры у них: 
            //общий зазор устанавливается в половину диаметра, без этого получается неустранимый коллапс
            if (flagPeriodityG)
            {
                float a = 0.25f;
                //a = 0f;
                startX = area.a.X + a * ParticelSize;
                sign = -1;
            }
            // это маркеры на частицах (типа сетки) чтобы видеть их рассыпание (для задачи обрушения нужно)
            int mark = (int)(CountParticleLine / 9.6f) + 1;
            int markX =  (int)(CountParticleLine / 2.0f);//это условно маркируем частицы, для которых записываются свойства в процессе  
            int color = 1;

            int yi = 0;
            float y;
            float b2 = (area.b.Y - area.a.Y) * (area.b.Y - area.a.Y) * 0.25f;
            float axis = area.a.Y + (area.b.Y - area.a.Y) * 0.5f;
            for (y = startY; y < endY; y += ParticelSizeY)
            {
                int xi = 0;
                for (float x = startX; x < endX; x += ParticelSize)
                {
                    if  //(x > endX) //!!!Для квадратной сетки
                        (x - sign * shift2 * (yi % 2) > endX)
                        {
                        break;
                        }

                    //if (xi % mark == 0 || yi % mark == 0)//это рисуем визуальную сетку на среде
                    //if (xi == markX && (y - axis > 0)) // это выделяем столбец частиц на полуширине потока
                    if (xi == markX && ((y - axis > 0.1 * ParticelSizeY) && y - axis < 1.1f * ParticelSizeY))  // это метим отдельную частицу в потоке
                            color = 1;
                    else
                        color = 0;
                    Position = new Vector2(x - sign * shift2 * (yi % 2), y);
                    Vector2 vY = new Vector2(0);
                    vY = Velocity * (b2 - (axis - y) * (axis - y));
                    Particle p = new Particle(Position, vY, color, Physics.NaturalDensetyF, Mass, ParticelSize, kInd);
                    //p.Density = Physics.NaturalDensetyF;
                    Particles[Count++] = p;
                    xi++;
                }
                yi++;
            }
            rarea.b.Y = y - shiftY;
            rarea.Pmax = Physics.NaturalDensetyF * Math.Abs(Particle.G.Y) * ((rarea.b.Y-rarea.a.Y) + ParticelSizeY);


            // Это обрезаем массив до реального числа частиц в нем 
            //*********************
            Particle[] tmp = new Particle[Count];
            for (int i = 0; i < Count; i++)
                tmp[i] = Particles[i];
            Particles = tmp;
            //*********************
        }

        public void SetBobyDouble(Area rarea, Vector2 Velocity0, int CountParticleLine, bool flagPeriodityG, bool flagPeriodityV)
        {
            area = rarea;
            float ShiftX = area.a.X;
            float ShiftY = area.a.Y;
            float WidthBody = area.Width;
            float HeightBody = area.Heigh;
            float MassF = (Physics.NaturalDensetyF * (ParticelSize * ParticelSize /* ParticelSize*/) * 0.86602541f);
            float MassS = (Physics.NaturalDensetyS * (ParticelSize * ParticelSize /* ParticelSize*/) * 0.86602541f);
            float ParticelSizeY = (0.86602541f * ParticelSize); //Для треугольной сетки
            //float ParticelSizeY = ParticelSize; // Для квадратной сетки
            float ErrEpsilon = Physics.ErrEpsilon;
            Vector2 Position;
            Vector2 Velocity = Velocity0;
            // шаг заполнения объема
            float shiftX = ParticelSize / 2f;
            float shiftY = ParticelSizeY / 2f;
            float shift2 = shiftX;
            //краевые точки заполнения области
            float startX = area.a.X + ParticelSize;
            float startY = area.a.Y + shiftY;
            float endX = area.b.X + ErrEpsilon;
            float endY = area.b.Y + ErrEpsilon;
            float endYS = area.a.Y + 0.5f * HeightBody + ErrEpsilon;
            float eS = 0;
            int sign = 1;
            //Для периодических границ уравновешиваем зазоры у них: 
            //общий зазор устанавливается в половину диаметра, без этого получается неустранимый коллапс
            if (flagPeriodityG)
            {
                float a = 0.25f;
                //a = 0f;
                startX = area.a.X + a * ParticelSize;
                sign = -1;
            }
            // это маркеры на частицах (типа сетки) чтобы видеть их рассыпание (для задачи обрушения нужно)
            int mark = (int)(CountParticleLine / 9.6f) + 1;
            int markX = (int)(CountParticleLine / 2.0f);//это условно маркируем частицы, для которых записываются свойства в процессе  
            int color = 1;

            int yi = 0;
            float y;
            float b2 = (area.b.Y - area.a.Y) * (area.b.Y - area.a.Y) * 0.25f;
            float axis = area.a.Y + (area.b.Y - area.a.Y) * 0.5f;
            float m = 0, d = 0;
            int ii = Kernels.FindIndex(s => s == "LosseKernel");
            int iF = Kernels.FindIndex(s => s == "MonaghanKernel");

            //Для использования одинаковых ядер
            ii = kInd;
            iF = kInd;

            Vector2 arc = new Vector2(0, (Physics.NaturalDensetyS - Physics.NaturalDensetyF) / Physics.NaturalDensetyS * Particle.G.Y);
            for (y = startY; y < endY; y += ParticelSizeY)
            {
                int xi = 0;
                for (float x = startX; x < endX; x += ParticelSize)
                {
                    if  //(x > endX) //!!!Для квадратной сетки
                        (x - sign * shift2 * (yi % 2) > endX)
                    {
                        break;
                    }

                    //if (xi % mark == 0 || yi % mark == 0)//это рисуем визуальную сетку на среде
                    //if (xi == markX && (y - axis > 0)) // это выделяем столбец частиц на полуширине потока
                   /* if (xi == markX && ((y - axis > 0.1 * ParticelSizeY) && y - axis < 1.1f * ParticelSizeY))  // это метим отдельную частицу в потоке
                        color = 1;
                    else
                        color = 0;*/
                    Position = new Vector2(x - sign * shift2 * (yi % 2), y);
                    Vector2 vY = new Vector2();
                    vY = Velocity * (b2 - (axis - y) * (axis - y));
                    if (y <= endYS)
                    { 
                        m = MassS; 
                        d = Physics.NaturalDensetyS; 
                        eS = y; 
                    }
                    else
                    { 
                        m = MassF; 
                        d = Physics.NaturalDensetyF; 
                        ii = iF; 
                        arc = arc * 0f; 
                    }
                    Particle p = new Particle(Position, vY, color, d, m, ParticelSize, ii);
                    p.Archi = arc;
                    Particles[Count++] = p;
                    xi++;
                }
                yi++;
            }
            rarea.b.Y = y - shiftY;
            rarea.Pmax = Math.Abs(Particle.G.Y) * (Physics.NaturalDensetyF * ((rarea.b.Y - eS) + ParticelSizeY) + Physics.NaturalDensetyS * ((eS - rarea.a.Y) + ParticelSizeY));


            // Это обрезаем массив до реального числа частиц в нем 
            //*********************
            Particle[] tmp = new Particle[Count];
            for (int i = 0; i < Count; i++)
                tmp[i] = Particles[i];
            Particles = tmp;
            //*********************
        }




        int psize = 4;
        int psizeM = 2;

        /// <summary>
        /// Тестовая отрисовка массива частиц
        /// </summary>
        /// <param name="g">Контекст</param>
        /// <param name="scale">масштаб</param>
        public void ShowBody(Graphics g, int scaleVelocity)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            SolidBrush Y = new SolidBrush(Color.Yellow);
            SolidBrush Br;
            Pen p = new Pen(B, 1);

            int xa = CoScale.ScaleX(area.a.X);
            int ya = CoScale.ScaleY(area.a.Y);
            int xb = CoScale.ScaleX(area.b.X);
            int yb = CoScale.ScaleY(area.b.Y);
            // расчетная область
            g.DrawRectangle(p, xa, yb, xb - xa, ya - yb);

            for (int n = 0; n < Count; n++)
            {
                Particle pi = Particles[n];

                int x = CoScale.ScaleX(pi.Position.X);
                int y = CoScale.ScaleY(pi.Position.Y);
                int u = (int)(pi.Velocity.X * scaleVelocity);
                int v = (int)(pi.Velocity.Y * scaleVelocity);
                    Br = B;
                Point a = new Point(x+1, y+1);
                Point b = new Point(x, y);
                Point c = new Point(x + u,  y + v);
                if (CountParticleLine < 18)
                    g.DrawString(n.ToString(), new Font("Arial", 10), B, a);
                g.FillRectangle(Br, x - psizeM, y - psizeM, psize, psize);
                g.DrawLine(new Pen(Br, 1), b, c);
            }
        }


    }
}
