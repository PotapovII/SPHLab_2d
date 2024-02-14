using System;
using System.Drawing;

namespace SPHLionLIB
{
    class RGBAdapter
    {
        /// <summary>
        /// Вычисление цвета заливки
        /// </summary>
        static public Color RGBBrash(float MaxV, float MinV, float arg, bool PType = true)
        {
            Color col = new Color();
            int R = 0, G = 0, B = 0;
            float t;
            try
            {
                if (PType == true)
                {
                    // черный цвет
                    if (Math.Abs(MaxV) < 0.0000001) MaxV = 1;
                    t = (arg - MinV) / (MaxV - MinV);
                    B = 255;
                    R = (int)(t * 255);
                    G = 0;
                }
                else
                {
                    // Определение цвета
                    int Base = 1;
                    float MaxColors = 255 - Base;
                    float mod = MaxV - MinV;
                    if (mod < 1e-12) mod = 1;
                    float _arg = 4 * (1 - (arg - MinV) / mod);
                    int level = (int)(_arg);
                    float value = _arg - level;
                    int Arg = (int)(value * MaxColors);
                    if (arg > MaxV) level = 4;
                    switch (level)
                    {
                        case -1:
                            B = 0;
                            R = 0;
                            G = 0;
                            break;
                        case 0:
                            R = 255;
                            G = Base + Arg;
                            B = Base;
                            break;
                        case 1:
                            R = 255 - Arg;
                            G = 255;
                            B = Base;
                            break;
                        case 2:
                            R = Base;
                            G = 255;
                            B = Base + Arg;
                            break;
                        case 3:
                            R = Base;
                            G = 255 - Arg;
                            B = 255;
                            break;
                        case 4:
                            B = 255;
                            R = 255;
                            G = 255;
                            break;
                        default:
                            B = 255;
                            R = 255;
                            G = 255;
                            break;
                    }
                }
            }
            catch
            {
                B = 125;
                R = 125;
                G = 125;
            }
            if (R < 0) R = 0;
            if (G < 0) G = 0;
            if (B < 0) B = 0;
            col = Color.FromArgb(R, G, B);
            return col;
        }
    }
}
