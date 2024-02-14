using System;
//using System.Numerics;

namespace SPHLionLIB
{
    /// <summary>
    /// Физические параметры задачи
    /// </summary>
    public static class Physics
    {
        /// <summary>
        /// Граничные условия
        /// </summary>
        //public static ВoundaryType[] Вoundary = { ВoundaryType.Mirror, ВoundaryType.Mirror, ВoundaryType.Mirror, ВoundaryType.Mirror };
        public static ВoundaryType[] Вoundary = { ВoundaryType.Wall, 
                                            ВoundaryType.Wall, 
                                            ВoundaryType.Wall, 
                                            ВoundaryType.Wall };
        /// <summary>
        /// Число курранта
        /// </summary>
        public static float Cr = 0.5f;
        /// <summary>
        /// Индекс выбора модели Монагана
        /// </summary>
        public static int IndexMu = 2;
        /// <summary>
        /// Плотность жидкой фазы среды
        /// </summary>
        public static float NaturalDensetyF = 1000f;
        /// <summary>
        /// Плотность твердой фазы среды
        /// </summary>
        public static float NaturalDensetyS = 2500f;
        /// <summary>
        /// Натуральная молекулярная вязкость среды
        /// </summary>
        public static float NaturalViscosity = 0.2f;
        /// <summary>
        /// Скорость звука в среде
        /// </summary>
        public static float SoundSpeed = 25;//Проба

        //public static float SoundSpeed = 2.5f;//<-Mon 20 частиц - сдвиг
        //public static float SoundSpeed = 109;//<-Mon & Morr 50 частиц - сдвиг


        /// <summary>
        /// Вязкость среды
        /// </summary>

        public static float Viscosity = 0f;//Проба
       // public static float Viscosity = 0.000017f;//<-Mon 20 частиц - сдвиг
        //public static float Viscosity = 0.0000000024f;//0.000000001f;//<-Mon 50 частиц

        /// <summary>
        /// Точность расчетов
        /// </summary>
        public static float ErrEpsilon = 0.000001f;


        // Коэффициенты для критерия Дрюкера-Прагера
        public static float FrictionAngle = 30;
        public static float Cohesoin = 1.0f;
        public static float tan_phi = (float)Math.Tan(FrictionAngle * Math.PI / 180f);
        
        public static float alpha_phi = tan_phi /(float)Math.Sqrt(9 + 12 * tan_phi * tan_phi);
        public static float alpha_Coh = 3 * Cohesoin / (float)Math.Sqrt(9 + 12 * tan_phi * tan_phi);

        /// <summary>
        /// Коэффициенты гипопластической модели
        /// </summary>
        public static float с1=25;
        public static float с2=280;
        public static float с3=250;
        public static float с4=-500;
    }
}
