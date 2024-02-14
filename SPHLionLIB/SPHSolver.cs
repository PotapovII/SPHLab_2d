using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using System.IO;
using ShowGraphic;

namespace SPHLionLIB
{
    public class SPHSolver
    {
        /// <summary>
        /// Расчетная область
        /// </summary>
        Area area;
        /// <summary>
        /// Начальная область для тела
        /// </summary>
        Area areaBody;
        /// <summary>
        /// Объект для хеширования частиц и нахождения ближайших соседей
        /// </summary>
        HeshGrid hesh;
        /// <summary>
        /// Активные частицы
        /// </summary>
        public Particle[] Particles;
        /// <summary>
        /// Сглаживающая длина h 
        /// </summary>
        public float hSize;
        /// <summary>
        /// Набор используемых ядер
        /// </summary>
        public BaseKernel SKGeneral;
        public BaseKernel[] KernelsDuo = new BaseKernel[3];

        /// <summary>
        /// Диаметр частицы
        /// </summary>
        public float pSize;


        public float limit;


        /// <summary>
        /// Получение среднего кинетической энергии системы относительно исходного среднего потенциальной
        /// </summary>
        public float EnergyKinetic
        {
            get
            {
                float Ek = 0;
                foreach (Particle p in Particles)
                {
                    Ek += p.Velocity.Length();
                    //Ek += 0.5f * (p.Mass * areaBody.ParticelSize) * p.Velocity.LengthSquared();//для статики и столба
                }
                return 
                    Ek / Particles.Count();
                    //Ek / Particles.Count() / EnergyPotential0; //для статики и столба
            }
        }
        /// <summary>
        /// Получение среднего потенциальной энергии системы относительно исходного 
        /// </summary>
        public float EnergyPotential
        {
            get
            {
                float Ep = 0;
                foreach (Particle p in Particles)
                    Ep += ((p.Mass * areaBody.ParticelSize) * areaBody.ParticelSize) * Particle.G.Y * (area.a.Y - p.Position.Y);
                return Ep / EnergyPotential0;
            }
        }
        /// <summary>
        /// Получение средней скорости системы
        /// </summary>
        public float MidVelosity
        {
            get
            {
                float V = 0;
                foreach (Particle p in Particles)
                    V += p.Velocity.Length();
                return V / Particles.Count();
            }
        }

        /// <summary>
        /// Прогешность плотности для слоя (строки хэша)
        /// </summary>
        public float DensityErr
        {
            get
            {
                float e = 0;
                //int yy = hesh.countY / 2;
                int yy = 1;
                int cnt = 0;
                foreach (Particle pi in Particles)
                {
                    int iy = (int)((pi.Position.Y - hesh.YA) / hesh.HeshSizeСellY) + 1;
                    int ix = (int)((pi.Position.X - hesh.XA) / hesh.HeshSizeСellX);
                    if (yy == iy && (ix > 0 || ix < hesh.countX))
                    {
                        e += (pi.density - pi.NaturalDensety) / pi.NaturalDensety;
                        cnt++;
                    }
                }

                return (e / cnt);
            }
        }
        /// <summary>
        /// Полгрешность по скорости для слоя (строки хэша)
        /// </summary>
        public float VelocityErr
        {
            get
            {
                float e = 0;
                //int yy = hesh.countY / 2;
                int yy = 1;
                int cnt = 0;
                foreach (Particle pi in Particles)
                {
                    int iy = (int)((pi.Position.Y - hesh.YA) / hesh.HeshSizeСellY) + 1;
                    int ix = (int)((pi.Position.X - hesh.XA) / hesh.HeshSizeСellX);
                    if (yy == iy && (ix > 0 || ix < hesh.countX))
                    {
                        e += (pi.ddW.X);
                        cnt++;
                    }
                }
                return (-e / (cnt * Physics.NaturalDensetyF * Particle.G.X) * Physics.NaturalViscosity - 1.0f);
            }
        }
        /// <summary>
        /// Изменения различных свойств индивидуально для маркированных частиц
        /// </summary>
        public List<float> Tracking
        {
            get
            {
                List<float> yList = new List<float>();

                foreach (Particle pi in Particles)
                {
                    if (pi.Color == 1)
                    {
                        yList.Add(pi.Position.Y);
                        //
                       /* yList.Add(pi.Velocity.X);
                        yList.Add(pi.Velocity.Y);*/
                        yList.Add((pi.density - pi.NaturalDensety) / pi.NaturalDensety);
                    }
  
                }

                return yList;
            }
        }
        /// <summary>
        /// Исходное среднее потенциальной энергии системы
        /// </summary>
        float EnergyPotential0 = 0;
        /// <summary>
        /// Характерный для задачи период времени
        /// </summary>
        float TimeScale = 1;
        /// <summary>
        /// Текущее относительное время
        /// </summary>
        public float relativeScale
        {
            get
            {
                return time / TimeScale;
            }
        }
        /// <summary>
        /// Настройка масштаба времени
        /// </summary>
        /// <returns></returns>
        public float SetTimeScale( int Tsk)
        {
            /* float hmax = Particles[0].Position.Y;
             float hmin = Particles[0].Position.Y;
             foreach (Particle p in Particles)
             {
                 if(hmax < p.Position.Y)
                     hmax = p.Position.Y;
                 if (hmin > p.Position.Y)
                     hmin = p.Position.Y;
             }
             float h = hmax - hmin;*/
            //исходная высота расчетного объема среды
            float hh = areaBody.b.Y - areaBody.a.Y;
            if (Tsk > 1)
            {
                //это по физике - период развития установления стационарного течения из статики 
                //TimeScale = (float)(0.5f * hh) * (0.5f * hh) / Physics.Viscosity * 5.8081f;

                //это период полной смены соседей в области сглаживания
                TimeScale = (float)(Physics.NaturalViscosity * 8f / (3f * Physics.NaturalDensetyF * areaBody.ParticelSize * Particle.G.X));
            }
            else
            {
                TimeScale = (float)Math.Sqrt(2 * hh / 9.81f);
            }
                float Ep = 0;
                foreach (Particle p in Particles)
                    Ep += (p.Mass * areaBody.ParticelSize) * Particle.G.Y * (area.a.Y - p.Position.Y);
                EnergyPotential0 = Ep;
                return TimeScale;
        }

        /// <summary>
        /// Потенциальная энергия системы в начальный момент времени
        /// </summary>
        public float EnergyPotentialStart;
        /// <summary>
        /// Шаг по времени/ половинный шаг по времени
        /// </summary>
        public float dt,dt05,time;
        /// <summary>
        /// Масштаб столба жидкости
        /// </summary>
        public float Hmax;
        /// <summary>
        /// Модель расчета давления
        /// </summary>
        public int IndexPressModel;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="hesh"></param>
        /// <param name="physics"></param>
        public SPHSolver(HeshGrid hesh, Area areaBody, int IndexPressModel, int IndexKernel, float kh)
        {
            this.hesh = hesh;
            this.area = hesh.area;
            this.areaBody = areaBody;
            this.Particles = hesh.Particles;
            EnergyPotentialStart = EnergyPotential;
            this.hSize =kh;
            this.pSize = areaBody.ParticelSize;
            this.IndexPressModel = IndexPressModel;
            // Определение ядер

            SKGeneral = KernelManager.CreateKernel(IndexKernel, kh);

            List<string> kL = KernelManager.GetNamesKernels();
                for (int i = 0; i < kL.Count; i++) KernelsDuo[i] =  KernelManager.CreateKernel(i, kh);

            sum = new float[hesh.countY];
            sumShow = new float[hesh.countY - 2];
            sumShowCol = new float[hesh.countY - 2];
            argSumShow = new float[hesh.countY - 2];
            sumCont = new int[hesh.countY];
            sumContCol = new int[hesh.countY - 2];
            // Индексация частиц на случай параллельной обработки массива частиц
            for (int i = 0; i < Particles.Length; i++)
                Particles[i].idx = i;
          
            // Определение текущего максимума высоты объема среды
            Hmax = Particles[0].Position.Y;
            for (int i = 1; i < Particles.Length; i++)
                if (Particles[i].Position.Y > Hmax)
                    Hmax = Particles[i].Position.Y;
            Hmax -= hesh.area.a.Y;
            time = 0;

            //Шаг времени по Куранту
            dt = Physics.Cr * areaBody.ParticelSize / Physics.SoundSpeed;
            dt05 = 0.5f * dt;
        }
        /// <summary>
        /// Коррекция шага по времени от состояния системы
        /// </summary>
        public void TimeStepCalculation()
        {
            float MaxVelocity = Particles.Select(p => p.Velocity.Length()).ToArray().Max();
            //float MaxAcceleration = Particles.Select(p => p.Acceleration.Length()).ToArray().Max();
            if (Physics.SoundSpeed < MaxVelocity)
            {
                //dt = Physics.Cr * Particle.ParticelSize / (Physics.SoundSpeed + MaxVelocity + MaxAcceleration * dt05);
                dt = Physics.Cr * areaBody.ParticelSize / (Physics.SoundSpeed + MaxVelocity);
                dt05 = 0.5f * dt;
            }
        }
        #region Вычисление параметров в точке
        ///// <summary>
        ///// Вычисление приведенных градиентов в точке
        ///// </summary>
        //public void CalculateGradient()
        //{
        //    float KernelRadius = SKGeneral.GetKernelRadius();
        //    float KernelRadius2 = KernelRadius * KernelRadius;
        //    // Вектор растояния межу двумя частицами
        //    Vector2 distance;
        //    // Список ближайших соседей
        //    List<Particle> adjacent;
        //    // Цикл по частицам
        //    foreach (Particle pi in Particles)
        //    {
        //        int Count = 0;
        //        // Вычисление списка ближайших соседей
        //        adjacent = hesh.FindNeighboringParticles(pi);
        //        // Расчет плотности частицы и дивергенции скорстей в смеси
        //        foreach (Particle pj in adjacent)
        //        {
        //            // Вычисление расстояния
        //            distance = pi.Position - pj.Position;
        //            float LengthSquared = distance.LengthSquared();
        //            //if (LengthSquared > hesh.HeshSize)
        //            //    continue;

        //            if (LengthSquared > KernelRadius2)
        //                continue;

        //            pi.Distance_ij[Count] = distance;
        //            pi.R_ij[Count] = (float)Math.Sqrt(LengthSquared);
        //            // Можно опримизировать

        //            pi.W_ij[Count] = SKGeneral.Calculate(ref distance);

        //            pi.GradientW_ij[Count] = SKGeneral.CalculateGradient(ref distance);

        //            pi.particle_ij[Count] = pj;
        //            Count++;
        //        }
        //        pi.Count = Count;
        //    }
        //}

        /// <summary>
        /// Поиск соседей и вычисление функций сглаживания
        /// </summary>
        public void CalculateKernels()
        {
            float KernelRadius = SKGeneral.GetKernelRadius();
            // Цикл по частицам
            Parallel.For(0, Particles.Length, i =>
            {
                Particle pi = Particles[i];
                int Count = 0;
                float piKernelRadius = KernelsDuo[pi.KernelIdx].GetKernelRadius();
                // Вычисление списка ближайших соседей
                List<Particle> adjacent = hesh.FindNeighboringParticles(pi);
                foreach (Particle pj in adjacent)
                {
                    if(pj==null) continue;
                    // Вектор растояния межу двумя частицами
                    Vector2 distance = pj.Position - pi.Position;
                    Vector2 rSpeed = pj.Velocity - pi.Velocity;
                    float R_ij = distance.Length();
                    if(R_ij > piKernelRadius)
                        continue;
                    pi.Distance_ij[Count] = distance;
                    pi.R_ij[Count] = R_ij;
                    // Можно оптимизировать
                    /* pi.W_ij[Count] = SKGeneral.Calculate(ref distance);
                     pi.GradientW_ij[Count] = SKGeneral.CalculateGradient(ref distance);
                     pi.LGradientW_ij[Count] = SKGeneral.CalculateLaplacian(ref distance) * rSpeed;
                     //pi.LGradientW_ij[Count].X = SKGeneral.CalculateLaplacian(ref distance) * rSpeed.Y;
                     //pi.LGradientW_ij[Count].Y = SKGeneral.CalculateLaplacian(ref distance) * rSpeed.X;*/
                    pi.W_ij[Count] = KernelsDuo[pi.KernelIdx].Calculate(ref distance);
                    pi.GradientW_ij[Count] = KernelsDuo[pi.KernelIdx].CalculateGradient(ref distance);
                    pi.LGradientW_ij[Count] = KernelsDuo[pi.KernelIdx].CalculateLaplacian(ref distance) * rSpeed;
                    pi.particle_ij[Count] = pj;
                    Count++;
                }
                pi.Count = Count;
            });
        }
        public void CalculateDensities()
        {

            // Цикл по частицам
            Parallel.For(0, Particles.Length, i =>
            {
                Particle pi = Particles[i];
                //Vector2 distance;
                // float Density = 0;
                //float WKernel = 0;
                //float norm = 0;
                //Vector2 deltaUx = new Vector2();
                //Vector2 deltaUy = new Vector2();
                //Vector2 Vel = new Vector2();
                //foreach (Particle pi in Particles)
                //{
                // Density = 0;
                //norm = 0;
                // Вычисление плотности частиц 
                //for (int j = 0; j < pi.Count; j++)
                //{
                //    Density += pi.W_ij[j];
                //}
                //pi.Density = pi.Mass * Density;
                pi.Density = 0;
                for (int j = 0; j < pi.Count; j++)
                {
                    Particle pj = pi.particle_ij[j];
                    pi.Density += pj.Mass * pi.W_ij[j];
                }
                //pi.Density = pi.Mass * Density;

                //for (int j = 0; j < pi.Count; j++)
                //{
                //    Particle pj = pi.particle_ij[j];
                //    // Вектор растояния межу двумя частицами
                //    distance = pi.Distance_ij[j];
                //    WKernel = pi.W_ij[j];
                //    /*
                //        // компоненты градиента скоростей
                //        Vel = pj.Velocity - pi.Velocity;
                //        deltaUx += 2 / (pj.Density + pi.Density) * Vel * distance.X * WKernel;
                //        deltaUy += 2 / (pj.Density + pi.Density) * Vel * distance.Y * WKernel;
                //        norm += distance.LengthSquared() * WKernel / pj.Density;*/
                //}

                /*  deltaUx *= Mass2D;
                  deltaUy *= Mass2D;
                  norm *= Mass2D;
                  //Тензор скоростей деформаций (девиатор формируется автоматически как поле объекта)
                  if (norm != 0)
                  {
                      deltaUx /= norm;
                      deltaUy /= norm;
                      //if (float.IsNaN(deltaUx.X) || float.IsNaN(deltaUy.X) ||
                      //    float.IsNaN(deltaUx.Y) || float.IsNaN(deltaUy.Y))
                      //    norm = norm;
                      pi.GradVelocity = new Matrix2x2
                              (deltaUx.X, deltaUx.Y,
                               deltaUy.X, deltaUy.Y);
                  }
                  else pi.GradVelocity = Matrix2x2.Zero;
                  /// Вычисление тензора скоростей деформаций 
                  pi.ComputeStrainRateTensor();
                  /// Вычисление вихревого тензора
                  pi.ComputeEddyDeformationTensor();*/
                //}
            });
        }
        /// <summary>
        /// Пресчет плотности после смещения частиц
        /// </summary>
        /// <param name="particles">частицы</param>
        //public void CalculateDensities()
        //{

        //    // Цикл по частицам
        //    Parallel.For(0, Particles.Length, i =>
        //    {
        //        Particle pi = Particles[i];
        //        Vector2 distance;
        //        float Density = 0;
        //        float WKernel = 0;
        //        float norm = 0;
        //        Vector2 deltaUx = new Vector2();
        //        Vector2 deltaUy = new Vector2();
        //        Vector2 Vel = new Vector2();
        //        //foreach (Particle pi in Particles)
        //        //{
        //            Density = 0;
        //            norm = 0;
        //        // Вычисление плотности частиц 
        //        for (int j = 0; j < pi.Count; j++)
        //        {
        //            Density += pi.W_ij[j];
        //        }

        //        pi.Density = pi.Mass * Density ;

        //        for (int j = 0; j < pi.Count; j++)
        //        {
        //            Particle pj = pi.particle_ij[j];
        //                // Вектор растояния межу двумя частицами
        //                distance = pi.Distance_ij[j];
        //                WKernel = pi.W_ij[j];
        //            /*
        //                // компоненты градиента скоростей
        //                Vel = pj.Velocity - pi.Velocity;
        //                deltaUx += 2 / (pj.Density + pi.Density) * Vel * distance.X * WKernel;
        //                deltaUy += 2 / (pj.Density + pi.Density) * Vel * distance.Y * WKernel;
        //                norm += distance.LengthSquared() * WKernel / pj.Density;*/
        //            }

        //          /*  deltaUx *= Mass2D;
        //            deltaUy *= Mass2D;
        //            norm *= Mass2D;
        //            //Тензор скоростей деформаций (девиатор формируется автоматически как поле объекта)
        //            if (norm != 0)
        //            {
        //                deltaUx /= norm;
        //                deltaUy /= norm;
        //                //if (float.IsNaN(deltaUx.X) || float.IsNaN(deltaUy.X) ||
        //                //    float.IsNaN(deltaUx.Y) || float.IsNaN(deltaUy.Y))
        //                //    norm = norm;
        //                pi.GradVelocity = new Matrix2x2
        //                        (deltaUx.X, deltaUx.Y,
        //                         deltaUy.X, deltaUy.Y);
        //            }
        //            else pi.GradVelocity = Matrix2x2.Zero;
        //            /// Вычисление тензора скоростей деформаций 
        //            pi.ComputeStrainRateTensor();
        //            /// Вычисление вихревого тензора
        //            pi.ComputeEddyDeformationTensor();*/
        //       //}
        //    });
        //}

        /// <summary>
        /// Вычисление ускорений от сил гравитации и деформации
        /// </summary>
        /// <param name="particles">частицы</param>
        /// <param name="globalForce">объемные силы</param>
        private void CalculateForces()
        {
            float ErrEpsilon = Physics.ErrEpsilon;
            float Nu_monogan = hSize * hSize * ErrEpsilon;

            // Расчет сил для каждой частицы
            Parallel.For(0, Particles.Length, i =>
            {

                Vector2 distance;
                Vector2 Val_dW = Vector2.Zero;
                Vector2 Val_d2W = Vector2.Zero;
                // Силы деформации
                Vector2 fii = Vector2.Zero;

                //for (int i = 0; i < Particles.Length; i++)
                //{
                Particle pi = Particles[i];
                float F_mon;
                // буфер для расчета сил давления и вязкости
                Vector2 div_Sigma = Vector2.Zero;
                float F_R = 0, dP;
                // Вычисление сил 
                for (int jj = 0; jj < pi.Count; jj++)
                {
                    Particle pj = pi.particle_ij[jj];
                    float VR, distance2, Mu_monogan, alpha;
                    F_mon = 0;

                    if (pj.Density > ErrEpsilon)
                    {
                        distance = pi.Distance_ij[jj];
                        fii = Vector2.Zero;
                        Vector2 KernelGradient = pi.GradientW_ij[jj];

                        //Matrix2x2 S = pj.DevStressTensor.Deviator + pi.DevStressTensor.Deviator;

                        //***************************Вычисление общего напряжения**********************
                        
                        //Val_dW += (pj.Pressure + pi.Pressure) * Mass2D * pi.DensityScale / (pj.Density * pi.Density) * KernelGradient;

                        //P = (pj.Pressure + pi.Pressure);
                        //Matrix2x2 Sigma = -new Matrix2x2(P, 0, 0, P) + S;
                        //div_Sigma += Matrix2x2.Dot(Sigma, Val_dW);

                        // ********************************************************************
                        // фиктивная вязкость по Моноганану
                         Vector2 v = pj.Velocity - pi.Velocity;
                         distance2 = pi.R_ij[jj] * pi.R_ij[jj];
                         VR = Vector2.Dot(v, distance);
                         alpha = 0.5f;
                        if (VR < 0 && distance2 > 0)
                        {
                            Mu_monogan = hSize * VR / (distance2 + Nu_monogan);
                            F_mon = (-alpha * Mu_monogan * Physics.SoundSpeed);
                        }
                        switch(IndexPressModel)
                        {
                            case 0:
                                Val_dW += ((pj.Pressure / (pj.Density * pj.Density) + pi.Pressure / (pi.Density * pi.Density)) + F_mon / pi.Density) * KernelGradient;
                                break;
                            case 1:
                                Val_dW += ((pj.Pressure + pi.Pressure) / (pi.Density * pi.Density) + F_R + F_mon / pi.Density) * KernelGradient;
                                break;
                            case 2:
                                dP = (pj.Pressure + pi.Pressure) / (pi.Density * pi.Density);
                                F_R = 0.08f * Math.Abs((pi.NaturalDensety-pj.NaturalDensety)/ (pi.NaturalDensety + pj.NaturalDensety)* dP);
                                Val_dW += (dP + F_R + F_mon / pi.Density) * KernelGradient;
                                break;
                        }
                        //********************************************************************       
                        ///Val_dW += ((pj.Pressure + pi.Pressure) / pj.Density + F_mon) * KernelGradient;
                        Val_d2W += pi.LGradientW_ij[jj] / (pj.Density);
                    }
                  }
                //Val_dW = div_Sigma + F_monogan;
                //pi.Acceleration = (Val_dW + 2.0f * Physics.Viscosity * Val_d2W) * pi.Mass / pi.Density + Particle.G;// + pi.Archi;
                pi.Acceleration = (Val_dW + 2.0f * Physics.Viscosity * Val_d2W / pi.Density) * pi.Mass + Particle.G;// + pi.Archi;
                pi.ddW = 2 * Val_d2W * pi.Mass;
            });
           // }
        }

        /// <summary>
        /// Вычисление деформаций
        /// </summary>
        private void CalculateDevStress(int IndexModel)
        {
            // Цикл по частицам
            Parallel.For(0, Particles.Length, i =>
            {
                Particle pi = Particles[i];
                // Цикл по частицам
                //    foreach (Particle pi in Particles)
                //{

                float J_StressTensor_2 = 0;
                // тензор скоростей деформаций
                Matrix2x2 Strain = pi.StrainRateTensor;
                // девиатор от тензора скоростей деформаций
                Matrix2x2 DevStrine = Strain.Deviator;
                // девиатор от тензора напряжений
                Matrix2x2 DevStress = pi.DevStressTensor;
                // первый инвариант от светки девиатор напряжений на тензор скоростей деформаций
                float J_DevStressStrain_1 = Matrix2x2.MultTen(DevStress, Strain).J1;
                // первый инвариант от тензора скоростей деформаций 
                float J_Strain_1 = Strain.J1;
                // второй инвариант от тензора скоростей деформаций
                float J_Strain_2 = Strain.J2;
                // Выбор моделей
                float c0 = Physics.с1 / Physics.с2;
                float c1 = Physics.с1;
                float c2 = Physics.с2;
                float c3 = Physics.с3;
                float c4 = Physics.с4;
                float ErrEpsilon = Physics.ErrEpsilon;
                float alpha_phi = Physics.alpha_phi;
                float alpha_Coh = Physics.alpha_Coh;

                switch (IndexModel)
                {
                    case 0: // c1 и c2
                        {
                            // Определение коэффициента вязкости
                            float Mu_p = 2 * c1 * pi.Pressure;
                            pi.DevStressTensor += dt * Mu_p * DevStrine;
                        }
                        break;
                    case 1: // c1 и c2
                        {
                            c1 = 500f; c2 = 50;
                            // Определение коэффициента вязкости
                            float Mu_p = 2 * c1 * pi.Pressure;
                            // Определение коэффициента релаксации
                            //float Relax = c2 * (J_DevStressStrain_1 - 2 * J_Strain_1 * pi.Pressure) / (2 * pi.Pressure + ErrEpsilon);
                            //float Relax = c2 * (J_DevStressStrain_1 / (2 * pi.Pressure + ErrEpsilon) - J_Strain_1 );
                            float Relax = c2 * (J_DevStressStrain_1 / (2 * pi.Pressure + ErrEpsilon) - J_Strain_1);
                            pi.DevStressTensor += dt * (Mu_p * DevStrine + Relax * DevStress);
                        }
                        break;
                    case 2: // c1 с2 и c3 
                        {
                            // скалярные коэффициенты
                            // float c01=700, c02=1, c03 =0.5; // жидкость течет
                            // с1 аналогично модулю сдвига для упругопластического материала: 
                            // малые с1 делает гранулированный материал более похожим на сдвиговый поток. 
                            // с2 связан с углом трения: больший угол трения от большего с2. 
                            // с3 контролирует жесткость: если с3 меньше, внутреннее тело сыпучего материала жестче.
                            c1 = 10;
                            c2 = 180;
                            c3 = -180;
                            float A1 = c1 * pi.Pressure;
                            float B1 = (c2 * J_DevStressStrain_1 + J_Strain_1 * pi.Pressure) / (pi.Pressure * pi.Pressure + 1);
                            float C1 = c3 * J_Strain_2;
                            // на самом деле в DevStressTensor хранится девиатор напряжений
                            pi.DevStressTensor += dt * (A1 * DevStrine + (B1 + C1) * pi.DevStressTensor);
                            // второй инвариант от девиатора тензора напряжений
                        }
                        break;
                    case 3: // c1 с2 c3 и c4 
                        {
                            // скалярные коэффициенты
                            // float c01=700, c02=1, c03 =0.5; // жидкость течет
                            // с1 аналогично модулю сдвига для упругопластического материала: 
                            // малые с1 делает гранулированный материал более похожим на сдвиговый поток. 
                            // с2 связан с углом трения: больший угол трения от большего с2. 
                            // с3 контролирует жесткость: если с3 меньше, внутреннее тело сыпучего материала жестче.
                            c1 = 28;
                            c2 = 280;
                            c3 = -280;
                            float A1 = c1 * pi.Pressure;
                            float B1 = (c2 * J_DevStressStrain_1 + J_Strain_1 * pi.Pressure) / (pi.Pressure * pi.Pressure + 1);
                            float C1 = c3 * J_Strain_2;
                            // на самом деле в DevStressTensor хранится девиатор напряжений
                            pi.DevStressTensor += dt * (A1 * DevStrine + (B1 + C1) * pi.DevStressTensor);
                            // второй инвариант от девиатора тензора напряжений
                        }
                        break;
                    case 4:
                        {
                            // скалярные коэффициенты
                            // float c01=700, c02=1, c03 =0.5; // жидкость течет
                            // с1 аналогично модулю сдвига для упругопластического материала: 
                            // малые с1 делает гранулированный материал более похожим на сдвиговый поток. 
                            // с2 связан с углом трения: больший угол трения от большего с2. 
                            // с3 контролирует жесткость: если с3 меньше, внутреннее тело сыпучего материала жестче.
                            //float MuSand = 0.5f; // неплохо
                            //c0=0.01f;
                            float eta = 0.0001f;
                            eta = ErrEpsilon;
                            float MuSand = 0.5f;
                            float alphaS = 1;// 0.75f;
                            c0 = 0.1f;
                            // c0 = 0.005f;
                            // Определение коэффициента вязкости
                            float Mu_p = 2 * pi.Pressure;
                            // Определение коэффициента релаксации
                            float Relax = J_DevStressStrain_1 - 2 * J_Strain_1 * pi.Pressure + eta;
                            if (Math.Abs(Relax) < eta)
                                Relax = eta;
                            if (Mu_p > ErrEpsilon)
                                MuSand = 0.001f;
                            //MuSand = 0;
                            pi.DevStressTensor = (1 - alphaS) * pi.DevStressTensor + alphaS * (MuSand + c0 * (Mu_p * Mu_p / Relax)) * DevStrine;
                        }
                        break;
                }
                // второй инвариант от девиатора тензора напряжений
                J_StressTensor_2 = (float)Math.Sqrt(pi.DevStressTensor.J2);
                // Если среда течет, то напряжение равно пределу текучести, 
                // следовательно масштабируем девиатор тензора напряжений                
                if (J_StressTensor_2 > (alpha_phi * pi.Pressure + alpha_Coh))
                {
                    alpha_Coh = 0;
                    //pi.DevStressTensor = (alpha_phi * pi.Pressure + alpha_Coh) / (J_StressTensor_2) * pi.DevStressTensor;
                    float scale = (alpha_phi * pi.Pressure + alpha_Coh) / (J_StressTensor_2 + ErrEpsilon);
                    // float scale = (alpha_phi * pi.Pressure) / (J_StressTensor_2);

                    pi.DevStressTensor = scale * pi.DevStressTensor;
                    pi.Plastic = 1;
                }
                else
                    pi.Plastic = 0;
                if (float.IsNaN(pi.DevStressTensor.XX) == true ||
                               float.IsNaN(pi.DevStressTensor.XY) == true ||
                               float.IsNaN(pi.DevStressTensor.YX) == true ||
                               float.IsNaN(pi.DevStressTensor.YY) == true)
                {
                    return;
                }

            });
        }

        #endregion

        #region Движение частиц
        /// <summary>
        /// Шаг предиктора по методу скоростей Верле
        /// </summary>
        public void PredictorStepAccordingToVerletVelocityMethod()
        {
            // Цикл по частицам
            Parallel.For(0, Particles.Length, i =>
            {
                Particle pi = Particles[i];
                // Цикл по частицам
                //foreach (Particle pi in Particles)
                //{
                // Новое положение за половинный шаг по времени
                pi.Position += pi.Velocity * dt05;
                hesh.OutControl(pi);
                if (float.IsNaN(pi.Position.X) || float.IsNaN(pi.Position.Y))
                    pi.Position = pi.PositionOld;
                pi.Acceleration = Vector2.Zero;
            });
        }
        /// <summary>
        /// Шаг корректора по методу скоростей Верле
        /// </summary>
        public void VerleVelocityCorrectorStep()
        {
            Parallel.For(0, Particles.Length, i =>
            {
                Particle pi = Particles[i];
                // Обновить скорость + положение с помощью сил
                pi.Velocity = pi.Velocity + pi.Acceleration * dt;
                // Обновить положение 
                pi.Position += pi.Velocity * dt05;
                // контроль границ
                hesh.OutControl(pi);
                if (float.IsNaN(pi.Position.X) || float.IsNaN(pi.Position.Y))
                    pi.Position = pi.PositionOld;
            });
            time += dt;
        }
        #endregion

        #region Тестовые методы
        /// <summary>
        /// Тест на линейное движение частиц
        /// </summary>
        public void CalculateTestFreeMove()
        {
            // Вычисление шага по времени
            TimeStepCalculation();
            //Предикция по Верле
            PredictorStepAccordingToVerletVelocityMethod();
            //// Создание хеша
            hesh.ReBuildHeshGrid();
            ////// Вычисление модифицированного градиента для частицы
            CalculateKernels();
            ////Окончательное обновление позиции частиц  
            VerleVelocityCorrectorStep();
        }
        /// <summary>
        /// Тест на расчет плотности для неподвижных частиц
        /// </summary>
        public void CalculateTestStaticDensity()
        {
            //// Создание хеша
            hesh.ReBuildHeshGrid();
            ////// Вычисление модифицированного градиента для частицы
            CalculateKernels();
            //// Вычисление плотности 
            CalculateDensities();
        }

        /// <summary>
        /// Тест на расчет плотности при линейном движеним частиц
        /// </summary>
        public void CalculateTestDensity()
        {
            // Вычисление шага по времени
            TimeStepCalculation();
            //Предикция по Верле
            PredictorStepAccordingToVerletVelocityMethod();
            //// Создание хеша
            hesh.ReBuildHeshGrid();
            ////// Вычисление модифицированного градиента для частицы
            CalculateKernels();
            //// Вычисление плотности 
            CalculateDensities();
            ////Окончательное обновление позиции частиц  
            VerleVelocityCorrectorStep();
        }
        /// <summary>
        /// Тест на движение жидких частиц по уравнению Эйлера
        /// </summary>
        public void CalculateGlobalForces()
        {
            // Вычисление шага по времени
            TimeStepCalculation();
            // Шаг предиктора по методу скоростей Верле
            PredictorStepAccordingToVerletVelocityMethod();
            // Создание хеша
            hesh.ReBuildHeshGrid();
            // Вычисление весов частиц в области сглаживания
            CalculateKernels();
            // Вычисление плотности 
            CalculateDensities();
            //Вычисление сил давления и вязкости
            CalculateForces();
            // Шаг корректора по методу скоростей Верле
            VerleVelocityCorrectorStep();
        }

        /// <summary>
        /// Тест на движеним сыпучей среды
        /// </summary>
        public void Calculate(int IndexModel)
        {
            // Вычисление шага по времени
            TimeStepCalculation();
            // Шаг предиктора по методу скоростей Верле
            PredictorStepAccordingToVerletVelocityMethod();
            // Создание хеша
            hesh.ReBuildHeshGrid();
            //// Вычисление модифицированного градиента для частицы
            CalculateKernels();
            //// Вычисление плотности 
            CalculateDensities();
            ////Вычисление скорстей деформации 
            CalculateDevStress(IndexModel);
            //Вычисление сил давления и вязкости
            CalculateForces();
            // Шаг корректора по методу скоростей Верле
            VerleVelocityCorrectorStep();
        }

        #endregion

        #region Методы отрисовки
        int psize = 4;
        int psizeM = 2;
        public void ShowParticle(Graphics g, int scaleVelocity)
        {
            hesh.ShowMeshHesh(g);
            SolidBrush R = new SolidBrush(Color.Red);
            Pen p = new Pen(R, 1);
            int x, y;

            
            for (int i = 0; i < Particles.Length; i++)
            {
                Particle pi = Particles[i];
                x = CoScale.ScaleX(pi.Position.X);
                y = CoScale.ScaleY(pi.Position.Y);
                int u = (int)(pi.Velocity.X * scaleVelocity);
                int v = (int)(pi.Velocity.Y * scaleVelocity);
                Point a = new Point(x + 1, y + 1);
                Point b = new Point(x, y);
                Point c = new Point(x + u, y - v);
                g.DrawString(pi.idx.ToString(), new Font("Arial", 10), R, a);
                g.FillRectangle(R, x - psizeM, y - psizeM, psize, psize);
                g.DrawLine(p, b, c);
            }
        }
        public void ShowParticleDensity(Graphics g)
        {
            hesh.ShowMeshHesh(g);
            SolidBrush R = new SolidBrush(Color.Red);
            SolidBrush P = new SolidBrush(Color.FromArgb(250 * 50 / 100, Color.Lime));
            SolidBrush PB = new SolidBrush(Color.FromArgb(250 * 25 / 100, Color.Lime));
            Pen p = new Pen(Color.Red, 1);
            SolidBrush B = new SolidBrush(Color.Black);
            int x, y;
            g.DrawString(Particles.Length.ToString(), new Font("Arial", 10), B, new Point(10, 30));
            //g.DrawString("Rho0 = " + Particle.NaturalDensety.ToString(), new Font("Arial", 10), B, new Point(10, 50));
            int psizeH = CoScale.ScaleX(pSize) - CoScale.ScaleX(0);
            int psizeH2 = psizeH / 2;
            for (int i = 0; i < Particles.Length; i++)
            {
                Particle pi = Particles[i];
                x = CoScale.ScaleX(pi.Position.X);
                y = CoScale.ScaleY(pi.Position.Y);
                g.FillEllipse(P, x - psizeH2, y - psizeH2, psizeH, psizeH);
            }
            for (int i = 0; i < Particles.Length; i++)
            {
                Particle pi = Particles[i];
                x = CoScale.ScaleX(pi.Position.X);
                y = CoScale.ScaleY(pi.Position.Y);
                Point a = new Point(x + 1, y + 1);
                g.DrawString(((int)pi.Density).ToString(), new Font("Arial", 10), B, a);
            }
            for (int i = 0; i < hesh.BufParicles.Length; i++)
            {
                Particle pi = hesh.BufParicles[i];
                if (pi == null) continue;
                x = CoScale.ScaleX(pi.Position.X);
                y = CoScale.ScaleY(pi.Position.Y);
                Point a = new Point(x + 1, y + 1);
                g.FillEllipse(PB, x - psizeH2, y - psizeH2, psizeH, psizeH);
                g.DrawString(((int)pi.Density).ToString(), new Font("Arial", 10), B, a);
            }
        }
        float[] ShowValue = null;
        float[] sum = null;
        int[] sumCont = null;
        int[] sumContCol = null;
        // массивы для внешней орисовки
        public float[] sumShow = null;
        public float[] sumShowCol = null;
        public float[] argSumShow = null;
        public int indexCol = -1;
        /// <summary>
        /// Отрисовка полей
        /// </summary>
        public void ShowParticleWater(Graphics g, int SelectedIndex, int scaleVelocity, int ck, bool flagBS = false)
        {
            hesh.ShowMeshHesh(g);

            if (ShowValue == null)
                ShowValue = new float[Particles.Length];
            switch (SelectedIndex)
            {
                case 0: // Модуль скорости
                    for (int n = 0; n < Particles.Length; n++)
                        ShowValue[n] = (Particles[n].Velocity.Length());
                    break;
                case 1: // Density   Плотность
                    for (int n = 0; n < Particles.Length; n++)
                        ShowValue[n] = Particles[n].Density;
                    break;
                case 2: // Press  Давление
                    for (int n = 0; n < Particles.Length; n++)
                        ShowValue[n] = Particles[n].Pressure;
                    break;
                case 3: // Eps_ij->J1  Первый инвариант тензора деформаций
                    for (int n = 0; n < Particles.Length; n++)
                        ShowValue[n] = Particles[n].StrainRateTensor.J1;
                    break;
                case 4: // Eps_ij->J2  Второй инвариант тензора деформаций
                    for (int n = 0; n < Particles.Length; n++)
                        ShowValue[n] = Particles[n].StrainRateTensor.J2;
                    break;
                case 5: // Eps_11  Деформации dU/dx
                    for (int n = 0; n < Particles.Length; n++)
                        ShowValue[n] = Particles[n].StrainRateTensor.XX;
                    break;
                case 6: // Eps_22  Деформации dW/dy
                    for (int n = 0; n < Particles.Length; n++)
                        ShowValue[n] = Particles[n].StrainRateTensor.YY;
                    break;
                case 7: // Eps_12  Деформации dU/dy + dW/dx
                    for (int n = 0; n < Particles.Length; n++)
                        ShowValue[n] = 0.5f * (Particles[n].StrainRateTensor.XY + Particles[n].StrainRateTensor.YX);
                    break;
                case 8: // S_ij->J2  Интенсивность напряжений
                    for (int n = 0; n < Particles.Length; n++)
                        ShowValue[n] = (float)Math.Sqrt(Particles[n].DevStressTensor.J2);
                    break;
                case 9: // S_xx
                    for (int n = 0; n < Particles.Length; n++)
                        ShowValue[n] = Particles[n].DevStressTensor.XX;
                    break;
                case 10: // S_xy
                    for (int n = 0; n < Particles.Length; n++)
                        ShowValue[n] = Particles[n].DevStressTensor.XY;
                    break;
                case 11: // S_yy
                    for (int n = 0; n < Particles.Length; n++)
                        ShowValue[n] = Particles[n].DevStressTensor.YY;
                    break;
                case 12: // Sigma_xx
                    for (int n = 0; n < Particles.Length; n++)
                        ShowValue[n] = Particles[n].DevStressTensor.XX - Particles[n].Pressure;
                    break;
                case 13: // Sigma_yy
                    for (int n = 0; n < Particles.Length; n++)
                        ShowValue[n] = Particles[n].DevStressTensor.YY - Particles[n].Pressure;
                    break;
            }
            float MaxValue = ShowValue.Max();
            float MinValue = ShowValue.Min();
            SolidBrush B = new SolidBrush(Color.Black);
            Pen pen = new Pen(Brushes.Black, 1);
            Font font = new Font("Arial", 12);
            // Количество частиц
            g.DrawString(Particles.Length.ToString(), new Font("Arial", 10), B, new Point(10, 30));
            g.DrawString(" Min: " + MinValue.ToString(), font, Brushes.Black, new Point(10, 50));
            g.DrawString(" Max: " + MaxValue.ToString(), font, Brushes.Black, new Point(10, 70));
            g.DrawString("time: " + time.ToString(), font, Brushes.Black, new Point(10, 90));
            g.DrawString("timeScale: " + relativeScale.ToString(), font, Brushes.Black, new Point(10, 120));
            // калибровка масштаба
            MaxValue = 1.1f * MaxValue;
            if (MinValue > 0)
                MinValue = 0.6f * MinValue;
            else
                MinValue = 1.1f * MinValue;
            if (SelectedIndex == 2) // давление
                MinValue = -10;

            for (int i = 0; i < sum.Length; i++)
            {
                sum[i] = 0; sumCont[i] = 0;
            }
            for (int i = 0; i < sumShowCol.Length; i++)
            {
                sumShowCol[i] = 0; sumContCol[i] = 0;
            }
            if (indexCol == -1)
                indexCol = hesh.countX / 2;
            int xx = indexCol % hesh.countX;
            foreach (Particle pi in Particles)
            {
                int iy = (int)((pi.Position.Y - hesh.YA) / hesh.HeshSizeСellY) + 1;
                int ix = (int)((pi.Position.X - hesh.XA) / hesh.HeshSizeСellX);
                sum[iy] += ShowValue[pi.idx];
                sumCont[iy]++;
                if (xx == ix && iy>0)
                {
                    sumShowCol[iy - 1] += ShowValue[pi.idx]; 
                    sumContCol[iy-1]++;
                }
            }
            for (int i = 0; i < sum.Length; i++)
            {
                if (sumCont[i] > 0)
                    sum[i] /= sumCont[i];
            }
            for (int i = 0; i < sumContCol.Length; i++)
            {
                if (sumContCol[i] > 0)
                    sumShowCol[i] /= sumContCol[i];
            }
            float y0 = 0;// 1.5f * hesh.HeshSizeСellY;
            for (int i = 0; i < sumShow.Length; i++)
            {
                sumShow[i] = sum[i + 1];
                argSumShow[i] = y0 + i * hesh.HeshSizeСellY;
            }
            if (ck == 1)
            {
                int pxLeft = CoScale.ScaleX(area.a.X - 1f * hesh.HeshSizeСellX) - 0;
                int pxRight = CoScale.ScaleX(area.a.X + area.Width + 1f * hesh.HeshSizeСellX);

                for (int i = 0; i < sum.Length; i++)
                {
                    int py = CoScale.ScaleY(area.a.Y + hesh.HeshSizeСellY * i);
                    Point a = new Point(pxLeft, py);
                    Point b = new Point(pxRight, py);
                    float coordy = hesh.HeshSizeСell * i;
                    g.DrawString(coordy.ToString(), new Font("Arial", 12), B, a);
                    g.DrawString(sum[i].ToString(), new Font("Arial", 12), B, b);
                }
                float ymax = Particles[0].Position.Y;
                float ymin = ymax;
                int iymax = 0, iymin = 0;
                float val;
                for (int i = 1; i < Particles.Length; i++)
                {
                    val = Particles[i].Position.Y;
                    if (ymax < val)
                    {
                        ymax = val; iymax = i;
                    }
                    if (ymin > val)
                    {
                        ymin = val; iymin = i;
                    }
                }
                float pxmax = Particles[iymin].Pressure;
                float pxmin = Particles[iymax].Pressure;
                var PP = Particles.Select(p => p.Pressure).ToArray();
                var Pxmax = PP.Max();
                var Pxmin = PP.Min();
                
                float bourder = ymin + (ymax - ymin) * Area.alpha;
                if (bourder > ymax)
                    bourder = ymax;

                int ya = CoScale.ScaleY(ymin);
                int yb = CoScale.ScaleY(ymax);
                int yc = CoScale.ScaleY(bourder);
                int sh = 120;
                Point p1 = new Point(pxRight + sh, ya);
                Point p2 = new Point(pxRight + sh, yb);
                Point pc = new Point(pxRight + sh, yc);
                Point pc1 = new Point(pxRight + sh, yc - 20);

                Point p3 = new Point(pxRight + sh, ya - 20);
                Point p4 = new Point(pxRight + sh, ya + 20);

                //float pressA = Particle.NaturalDensety * (9.81f) * (ymax - bourder); // верхняя часть
                //float press = Particle.NaturalDensety * Area.DensityScale * (9.81f) * (bourder - ymin) + pressA;

                //float pressA = Particle.NaturalDensety * (9.81f) * (ymax - bourder); // верхняя часть
                float press = SphArea.area.Pmax;

                g.DrawLine(pen, p1, p2);
                g.DrawString("H: " + (ymax - ymin).ToString(), font, Brushes.Black, new Point(10, 150));
                //float Ha = areaBody.Heigh - Particle.ParticelSize;
                float Ha = (float)Math.Sqrt(SphArea.area.Heigh * SphArea.area.Heigh - 0.25 * 0.02 * 0.02 * SphArea.area.Width * SphArea.area.Width) - SphArea.area.ParticelSize;
                g.DrawString("Ha: " + Ha.ToString(), font, Brushes.Black, new Point(10, 175));
                g.DrawString("H/Ha: " + ((ymax - ymin) / Ha).ToString(), font, Brushes.Black, new Point(10, 200));
                g.DrawString("P: " + press.ToString(), font, Brushes.Black, new Point(10, 225));
                g.DrawString("ParticelSize: " + SphArea.area.ParticelSize.ToString(), font, Brushes.Black, new Point(10, 250));

                g.DrawString("Pmin: " + pxmin.ToString(), font, Brushes.Black, p2);
                g.DrawString("Pmax: " + pxmax.ToString(), font, Brushes.Black, p1);
                //g.DrawString("Pa: " + pressA.ToString(), font, Brushes.Black, pc1);
                g.DrawString("Pa: " + press.ToString(), font, Brushes.Black, p3);
                g.DrawString("Err: " + (100 * (pxmax - press) / press).ToString() + " %", font, Brushes.Black, p4);
            }
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            int x, y;
            int psize = (CoScale.ScaleX(pSize) - CoScale.ScaleX(0));
            int psizeH = psize;

            int delta = (psizeH - psize) / 2;
            Pen pp = new Pen(Color.Black, 1);
            for (int i = 0; i < Particles.Length; i++)
            {
                Particle pi = Particles[i];
                x = CoScale.ScaleX(pi.Position.X);
                y = CoScale.ScaleY(pi.Position.Y);
                int bx = x - psizeH / 2;
                int by = y - psizeH / 2;
                float value = ShowValue[i];
                Color color = RGBAdapter.RGBBrash(MaxValue, MinValue, value, false);
                SolidBrush P = new SolidBrush(Color.FromArgb(125, color));
                g.FillEllipse(P, bx, by, psizeH, psizeH);
                if (ck == 1) // контур частицы
                    g.DrawEllipse(pen, bx + delta, by + delta, psize, psize);
            }
            if (flagBS == true)
            {
                for (int i = 0; i < hesh.CountBP; i++)
                {
                    Particle pi = hesh.BufParicles[i];
                    x = CoScale.ScaleX(pi.Position.X);
                    y = CoScale.ScaleY(pi.Position.Y);
                    if (ck == 1)
                        g.DrawEllipse(pen, x - psize / 2, y - psize / 2, psize, psize);
                }
            }
        }

        #endregion
        /// <summary>
        /// Запись данных в vtk формат
        /// </summary>
        /// <param name="file"></param>
        public void SaveDataToVtk(StreamWriter file)
        {
            file.WriteLine("# vtk DataFile Version 4.2");
            file.WriteLine("vtk output");
            file.WriteLine("ASCII");
            file.WriteLine("DATASET UNSTRUCTURED_GRID");
            file.WriteLine("POINTS " + Particles.Length + " float");
            foreach(var p in Particles)
                file.WriteLine( p.Position.ToString() );
        }
        #region Тесты на гидростатику
        float G = 9.81f;
        /// <summary>
        /// Расчет аналитического давления
        /// </summary>
        /// <param name="x">координата</param>
        /// <param name="bourder">граница среды</param>
        /// <param name="xmin">максимальная координата</param>
        /// <returns>давление</returns>
        public float GetPressure(float x, float bourder, float xmax)
        {
            float DenG = areaBody.Pmax;
            float PTop = DenG * (xmax - bourder);
            float Pressure;
            if (bourder <= x)
                Pressure = DenG * (xmax - x);
            else
                Pressure = DenG * Area.DensityScale * (bourder - x) + PTop;
            return Pressure;
        }
     
        /// <summary>
        /// Расчет аналитического давления
        /// </summary>
        public float GetPressure(float y, Area area)
        {
            float ymin = area.a.Y;
            float ymax = area.b.Y;
            float bourder = ymin + (ymax - ymin) * Area.alpha;
            if (bourder > ymax)
                bourder = ymax;
       
            float G = 9.81f;
            float DenG = areaBody.Pmax;
            float PTop = DenG * (ymax - bourder) * Area.DensityScale;
            float Pressure;
            if (bourder <= y)
                Pressure = DenG * Area.DensityScale * (ymax - y);
            else
                Pressure = DenG * (bourder - y) + PTop;
            return Pressure;
        }
        #endregion
    }
}
