using System;
using System.Numerics;

namespace SPHLionLIB
{
    /// <summary>
    /// Матрица 2х2 используется для поддержки тензовор механики сплошной среды
    /// </summary>
    public struct Matrix2x2 : IEquatable<Matrix2x2>
    {
        #region Поля
        /// <summary>
        /// Нормальная составляющая XX матрицы Matrix2x2
        /// </summary>
        public float XX;
        /// <summary>
        /// Нормальная составляющая YY матрицы Matrix2x2.
        /// </summary>
        public float YY;
        /// <summary>
        /// Касательная составляющая XY матрицы Matrix2x2.
        /// </summary>
        public float XY;
        /// <summary>
        /// Касательная составляющая  YX матрицы Matrix2x2.
        /// </summary>
        public float YX;
        /// <summary>
        /// Определяет нулевую матрицу Matrix2x2.
        /// </summary>
        public static readonly Matrix2x2 Zero = new Matrix2x2(0, 0, 0, 0);
        /// <summary>
        /// Определяет единичную матрицу Matrix2x2.
        /// </summary>
        public static readonly Matrix2x2 Unit = new Matrix2x2(1, 0, 0, 1);
        #endregion

        #region Конструкторы
        /// <summary>
        /// Создает новую Matrix2x2.
        /// </summary>
        public Matrix2x2(float xx, float xy, float yx, float yy)
        {
            XX = xx;
            YY = yy;
            XY = xy;
            YX = yx;
        }
        /// <summary>
        /// Создает новую Matrix2x2.
        /// </summary>
        public Matrix2x2(Vector2 x, Vector2 y)
        {
            XX = x.X; XY = x.Y;
            YX = y.X; YY = y.Y;
        }
        /// <summary>
        /// Создает новую Matrix2x2.
        /// </summary>
        public Matrix2x2(Matrix2x2 m)
        {
            XX = m.XX; XY = m.XY;
            YX = m.YX; YY = m.YY;
        }
        #endregion

        #region Свойства
        /// <summary>
        /// Первый инвариант тензора
        /// </summary>
        public float J1
        {
            get
            {
                return XX + YY;
            }
        }
        /// <summary>
        /// Второй инвариант для девиатора тензора !!!!!!!
        /// </summary>
        public float J2
        {
            get
            {
                return 0.25f * ((XX - YY) * (XX - YY)) + XY * XY;
            }
        }
        /// <summary>
        /// Девиатор тензора
        /// </summary>
        public Matrix2x2 Deviator
        {
            get
            {
                return new Matrix2x2(0.5f * (XX - YY), XY, YX, 0.5f * (YY - XX));
            }
        }
        /// <summary>
        /// Детерминант тензора
        /// </summary>
        public float Det
        {
            get { return XX * YY - XY * YX; }
        }
        public Vector2 Mx
        {
            set { XX = value.X; XY = value.Y; }
            get { return new Vector2(XX, XY); }
        }
        public Vector2 My
        {
            set { YX = value.X; YY = value.Y; }
            get { return new Vector2(YX, YY); }
        }
        #endregion

        #region Методы
        public static Matrix2x2 BackMatric(Matrix2x2 a)
        {
            float d = a.Det;
            //Matrix2x2 back = new Matrix2x2(a.YY/d, -a.XY/d, -a.YX/d, a.XX/d);
            return new Matrix2x2(a.YY / d, -a.XY / d, -a.YX / d, a.XX / d);
        }

        /// <summary>
        /// Вычислить точечное (скалярное) произведение матрицы на вектор
        /// </summary>
        public static Vector2 Dot(Matrix2x2 a, Vector2 Col)
        {
            return new Vector2(a.XX * Col.X + a.XY * Col.Y, a.YX * Col.X + a.YY * Col.Y);
        }
        /// <summary>
        /// Полная тензорная светка
        /// </summary>
        public static float Dot(Matrix2x2 a, Matrix2x2 b)
        {
            return a.XX * b.XX + a.XY * b.XY + a.YX * b.YX + a.YY * b.YY;
        }
        /// <summary>
        /// Тензорная свертка по внутренним индексам
        /// </summary>
        public static Matrix2x2 MultTen(Matrix2x2 a, Matrix2x2 b)
        {
            return new Matrix2x2(a.XX * b.XX + a.XY * b.YX,
                                  a.XX * b.XY + a.XY * b.YY,
                                  a.YX * b.XX + a.YY * b.YX,
                                  a.YX * b.XY + a.YY * b.YY);
        }
        /// <summary>
        /// Указывает, равен ли текущий вектор другому вектору.
        /// </summary>
        public bool Equals(Matrix2x2 other)
        {
            return XX == other.XX && YY == other.YY &&
                   XY == other.XY && YX == other.YX;
        }
        #endregion

        #region Операции

        /// <summary>
        /// Сложение 
        /// </summary>
        public static Matrix2x2 operator +(Matrix2x2 ma, Matrix2x2 mb)
        {
            return new Matrix2x2(ma.XX + mb.XX, ma.XY + mb.XY, ma.YX + mb.YX, ma.YY + mb.YY);
        }
        /// <summary>
        /// Вычитание
        /// </summary>
        public static Matrix2x2 operator -(Matrix2x2 ma, Matrix2x2 mb)
        {
            return new Matrix2x2(ma.XX - mb.XX, ma.XY - mb.XY, ma.YX - mb.YX, ma.YY - mb.YY);
        }
        /// <summary>
        /// Смена знака
        /// </summary>
        public static Matrix2x2 operator -(Matrix2x2 m)
        {
            m.XX = -m.XX;
            m.YY = -m.YY;
            m.XY = -m.XY;
            m.YX = -m.YX;
            return m;
        }
        /// <summary>
        /// Умножение на число с лева
        /// </summary>
        public static Matrix2x2 operator *(float scale, Matrix2x2 m)
        {
            return new Matrix2x2(m.XX * scale, m.XY * scale, m.YX * scale, m.YY * scale);
        }
        /// <summary>
        /// Умножение на число с права
        /// </summary>
        public static Matrix2x2 operator *(Matrix2x2 m, float scale)
        {
            return new Matrix2x2(m.XX * scale, m.XY * scale, m.YX * scale, m.YY * scale);
        }
        /// <summary>
        /// Деление на число
        /// </summary>
        public static Matrix2x2 operator /(Matrix2x2 m, float scale)
        {
            float mult = 1.0f / scale;
            return new Matrix2x2(m.XX * mult, m.XY * mult, m.YX * mult, m.YY * mult);
        }
        /// <summary>
        /// Сравнивает указанные экземпляры на равенство.
        /// </summary>
        public static bool operator ==(Matrix2x2 ma, Matrix2x2 mb)
        {
            return ma.Equals(mb);
        }
        /// <summary>
        /// Сравнивает указанные экземпляры на неравенство.
        /// </summary>
        public static bool operator !=(Matrix2x2 ma, Matrix2x2 mb)
        {
            return !ma.Equals(mb);
        }

        #endregion

        #region Перегрузка интерфейсов
        /// <summary>
        /// Возвращает строку
        /// </summary>
        public override string ToString()
        {
            return String.Format("({0}, {1}, {2}, {3)", XX, XY, YX, YY);
        }
        /// <summary>
        /// Возвращает хеш-код для этого экземпляра.
        /// </summary>
        /// <returns>System.Int32, содержащий уникальный хэш-код для этого экземпляра.</returns>
        public override int GetHashCode()
        {
            return XX.GetHashCode() ^ YY.GetHashCode();
        }
        /// <summary>
        /// Указывает, равны ли этот экземпляр и указанный объект.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Matrix2x2))
                return false;
            return this.Equals((Matrix2x2)obj);
        }
        #endregion
    }
}
