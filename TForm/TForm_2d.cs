using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.IO;
using System.Reflection;
//---------------------------------------------
using SPHLionLIB;
using ShowGraphic;

namespace TForm
{
    public partial class TForm_2d : Form
    {

        /// <summary>
        /// Временняа отсечка (в периодах), когда требуется остановить расчет
        /// </summary>
        float[] stopTime = { 0.5f, 1.0f, 2.0f, 2.5f, 3.0f, 5.0f };
        int IndexStopTime = 0;
        /// <summary>
        /// Контроль системы во времени
        /// </summary>
        List<float> Ek = new List<float>();
        List<float> Ep = new List<float>();
        List<float> Tm = new List<float>();
        //List<float> Vm = new List<float>();
        List<float> DnsErr = new List<float>();
        List<List<float>> Track = new List<List<float>>();

        /// <summary>
        /// Список решаетмых задач
        /// </summary>
        public string[] TaskList
        {
            get {
                string[] t = { "Статика", "Обрушение столба", "Река", "Труба" };
                return t;
            }
        }
        /// <summary>
        /// Список возможных типов для твердых стенок расчетной области
        /// </summary>
        public string[] BorderList
        {
            get
            {
                string[] t = { "Жесткие частицы", "Виртуальные частицы", "Зеркала" };
                return t;
            }
        }

        private void TForm_2d_Load(object sender, EventArgs e)
        {
            cbTasks.Items.AddRange(TaskList);
            cbTasks.SelectedIndex = 0;
            cbBoundType.Items.AddRange(BorderList);
            cbBoundType.SelectedIndex = 0;
            cb_PressModel.SelectedIndex = 0;
        }

        /// <summary>
        /// Масштаб для h и следовательно для хэша
        /// </summary>
        public float HeshScale
        {
            get
            {
                float V = 1.0f;
                if (numericUpDown_k != null)
                    V = (float)numericUpDown_k.Value / 1000.0f;
                return V;
            }
           
        }

        public int ParticlesInLine
        {
            get
            {
                int n = 1;
                if (numericUpDownCount != null)
                    n = (int)numericUpDownCount.Value;
                return n;
            }
        }
        /// <summary>
        /// Получение идентификатора решаемой задачи
        /// </summary>
        public int TaskIndex
        {
            get
            {
                int i = 0;
                if (cbTasks.Items.Count != 0)
                    i = (int)cbTasks.SelectedIndex;
                return i;
            }

        }
        /// <summary>
        /// Расчетная область со всеми физическими частицами и ее параметры 
        /// </summary>
        SphArea sphArea;
        /// <summary>
        /// Сетка хэша на объекте sphArea и связанные с ней действия
        /// </summary>
        HeshGrid hesh;
        /// <summary>
        /// Решатель задачи, заданной объектом sphArea
        /// </summary>
        SPHSolver sphSolver;

        //габаритная ширина тела среды 50 мм, высота варьируется от типа задачи,
        //под это задем габариты емкости

        /// <summary>
        ///Ширина емкости
        /// </summary>
        static float wgt = 0.0525f;
        //высота емкости
        static float hgt = (float)(1.2 * wgt);
        //а это габариты емкости для трубы и реки соотвественно...
        static float wgttrube = (float)(1.0 * wgt);
        static float wgtriver = (float)(3.0 * wgt);

        //формируем расчетные области под габариты емкости
        static Area areaTrube = new Area(new Vector2(0.01f, 0.01f), new Vector2(0.01f + wgttrube, 0.01f + 0.8660254f * wgttrube * 2f));
        static Area areaStatic = new Area(new Vector2(0.01f, 0.01f), new Vector2(0.01f + wgt, 0.01f + hgt));
        static Area areaColumn = new Area(new Vector2(0.01f, 0.01f), new Vector2(0.01f + wgt, 0.01f + 2.2f * wgt));
        static Area areaRiver = new Area(new Vector2(0.01f, 0.01f), new Vector2(0.01f + wgtriver, 0.01f + 1.0f * wgt));

        Area BodyStatic = new Area(new Vector2(), new Vector2());
        Area BodyColumn = new Area(new Vector2(), new Vector2());
        Area BodyRiver = new Area(new Vector2(), new Vector2());
        Area BodyTrube = new Area(new Vector2(), new Vector2());

        public Area area
        {
            get
            {
                if (TaskIndex == 0)
                    return areaStatic;
                if (TaskIndex == 1)
                    return areaColumn;
                if (TaskIndex == 3)
                    return areaTrube;
                else return areaRiver;
            }
        }
        
        public Area areaBody
        {
            get
            {
                if (TaskIndex == 0)
                    return BodyStatic;
                if (TaskIndex == 1)
                    return BodyColumn;
                if (TaskIndex == 3)
                    return BodyTrube;
                else return BodyRiver;
            }
        }

        static float NaturalDensety;
        float ParticelSize;
        static Vector2 Velocity0 = new Vector2(1f, 1f);
        static float HeshSize;
        static float Rigidity;
        static float Mass;

        static ВoundaryType[] ВoundaryM = { ВoundaryType.Mirror, 
                                            ВoundaryType.Mirror, 
                                            ВoundaryType.Mirror, 
                                            ВoundaryType.Mirror };
        static ВoundaryType[] ВoundaryW = { ВoundaryType.Wall, 
                                            ВoundaryType.Wall, 
                                            ВoundaryType.Wall, 
                                            ВoundaryType.Wall };
        static ВoundaryType[] ВoundaryS = { ВoundaryType.Solid, 
                                            ВoundaryType.Solid, 
                                            ВoundaryType.Solid, 
                                            ВoundaryType.Solid };
        static ВoundaryType[] ВoundaryP = { ВoundaryType.Periodic, 
                                            ВoundaryType.Periodic, 
                                            ВoundaryType.Periodic, 
                                            ВoundaryType.Periodic };
        static ВoundaryType[] ВoundarySW = { ВoundaryType.ShiftWall, 
                                            ВoundaryType.ShiftWall, 
                                            ВoundaryType.ShiftWall, 
                                            ВoundaryType.ShiftWall };
        // труба вертикальная
        static ВoundaryType[] ВoundaryTubeV  = { ВoundaryType.Solid, 
                                                ВoundaryType.Solid, 
                                                ВoundaryType.Periodic, 
                                                ВoundaryType.Periodic };
        static ВoundaryType[] ВoundaryTubeH = { ВoundaryType.Periodic,
                                                ВoundaryType.Periodic, 
                                                ВoundaryType.Wall, 
                                                ВoundaryType.Wall };
        // речной канал
        static ВoundaryType[] ВoundaryRiverS = {ВoundaryType.Periodic, 
                                                ВoundaryType.Periodic,
                                                ВoundaryType.Solid, 
                                                ВoundaryType.Solid};
        static ВoundaryType[] ВoundaryRiverW = {ВoundaryType.Periodic, 
                                                ВoundaryType.Periodic,
                                                ВoundaryType.Wall, 
                                                ВoundaryType.Wall};
        static ВoundaryType[] ВoundaryRiverSh = {ВoundaryType.Periodic, 
                                                ВoundaryType.Periodic,
                                                ВoundaryType.ShiftWall, 
                                                ВoundaryType.ShiftWall};

        public static ВoundaryType[] GetВoundary(int idx)
        {
            switch (idx)
            {
                case 0: // Статика
                    return ВoundaryW;
                case 1: // Обрушение
                    return ВoundaryS;
                case 2: // Река
                    return ВoundaryRiverS;
            }
            return ВoundaryTubeH;
        }

        int startx = 90;
        int bottomy = 890;
        int scaleVelocity = 20;
        int scale = 4000;

        public TForm_2d()
        {
            InitializeComponent();
            cbValue.SelectedIndex = 0;
            cbModel.SelectedIndex = 3;
            cbValueAll.SelectedIndex = 0;
            cbBoundary.SelectedIndex = 1;
            foreach (string s in KernelManager.GetNamesKernels())
                comboBoxKernel.Items.Add(s);

            int LosseInd = comboBoxKernel.FindString("MonaghanKernel");
            comboBoxKernel.SelectedIndex = LosseInd;

            int grafInd = cbValue.FindString("Плотность");
            cbValue.SelectedIndex = grafInd;

            scale = (int)numericUpDownSC.Value;
            bottomy = (int)numericUpDownBT.Value;
            startx = (int)numericUpDownLF.Value;
            CoScale.shiftX = 250;
            CoScale.shiftY = 300;
            CoScale.scale = 400;
            CoScale.xmax = area.b.X;
            CoScale.xmin = area.a.X;
            CoScale.ymax = area.b.Y;
            CoScale.ymin = area.a.Y;
            CoScale.SetInitSubArea();
        }
        /// <summary>
        /// Настройка графического отображения решения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDownSC_ValueChanged(object sender, EventArgs e)
        {
            CoScale.scale = (int)numericUpDownSC.Value;
            CoScale.shiftY = (int)numericUpDownBT.Value;
            CoScale.shiftX = (int)numericUpDownLF.Value;
            ReShow();
        }
        //не помню - накой это...
        int flag = 0;
        int TskInd;
        private void CreateArea(Vector2 Velocity0, bool fG, bool fV)
        {
            int SmoothingOrder = (int)numericUpDownDim.Value;
            if (flag == 0)
            {
                //Это задание зазоров между стенками и объемом среды
                int CountParticleLine = ParticlesInLine;
                float w = 0;
                switch (cbTasks.SelectedIndex)
                {
                    case 0: // Статика
                        w = (wgt * (1 - 0.02f / (CountParticleLine + 0.5f))); break;
                    case 1: // Столб
                        w = (wgt * (1 - 0.01f / (CountParticleLine + 0.5f))); break;
                    case 2: // Река
                        w = (wgt * (1 - 0.00f / (CountParticleLine + 0.5f))); break;
                    case 3: // Труба
                        w = (wgttrube * (1 - 0.00f / (CountParticleLine + 0.5f))); break;
                    default: break;
                } 
                ParticelSize =(w / (CountParticleLine + 0.5f));

                //Это объем среды для статики
                BodyStatic = new Area(new Vector2(0.01f + 0.01f * ParticelSize, 0.01f + 0.1f * ParticelSize), new Vector2(0.01f + w, 0.01f + wgt), ParticelSize);

                //Это объем среды для столба
                BodyColumn = new Area(new Vector2(0.01f + 0.01f * ParticelSize, 0.01f + 0.1f * ParticelSize), new Vector2(0.01f + w, 0.01f + 2*wgt), ParticelSize);

                //Это объем среды для реки
                BodyRiver = new Area(new Vector2(0.01f + 1.0f * ParticelSize, 0.01f + 0.1f * ParticelSize), new Vector2(0.01f + 0.95f * wgtriver, 0.01f + wgt * 0.8f), ParticelSize);

                //Это объем среды для трубы
                BodyTrube = new Area(new Vector2(0.01f, 0.01f), new Vector2(0.01f + w, 0.01f + (0.8660254f * wgttrube * 2f)), ParticelSize);

                // радиус сглаживания 
                HeshSize = ParticelSize * HeshScale * SmoothingOrder;

                NaturalDensety = Physics.NaturalDensetyF;
                Rigidity = Physics.SoundSpeed * Physics.SoundSpeed;

                //Из шестигранной формы частицы
                //Mass = (NaturalDensety * (ParticelSize * ParticelSize * ParticelSize) * 0.86602541f);
                //Из квадратной формы частицы
                //Mass = (NaturalDensety * (ParticelSize * ParticelSize * ParticelSize));

                List<string> kL = new List<string>();
                for (int i = 0; i < comboBoxKernel.Items.Count; i++) kL.Add((string)comboBoxKernel.Items[i]);
                sphArea = new SphArea(Mass, NaturalDensety, Rigidity, ParticelSize, kL, comboBoxKernel.SelectedIndex);
                if (radioMono.Checked == true)
                    sphArea.SetBobyMono(areaBody, Velocity0, CountParticleLine, fG, fV);
                else
                    sphArea.SetBobyDouble(areaBody, Velocity0, CountParticleLine, fG, fV);

                //Только длЯ трубы - корректируем область задачи под реальное число рядов частиц
                if (cbTasks.SelectedIndex == 3)
                {
                    area.b.Y = areaBody.b.Y;
                }

                area.ParticelSize = ParticelSize;
                area.Pmax = areaBody.Pmax;

                
                //SmoothingOrder =KernelManager.g sphArea.Particles[sphArea.Particles.Count() -1].KernelIdx
                //HeshSize = ParticelSize * HeshScale * SmoothingOrder;

            }
            else
            {
            }
        }
        /// <summary>
        /// Генерация расчетной области и связанных с ней объектов
        /// </summary>
        /// <param name="Вoundary"></param>
        /// <param name="Velocity0"></param>
        /// <param name="GradVelocity"></param>
        private void CreateAreaAndHesh(ВoundaryType[] Вoundary, Vector2 Velocity0)
        {
            bool fv = false, fg = false;
            if (TaskIndex == 3 || TaskIndex == 2) fg = true;

            CreateArea(Velocity0, fg, fv);
            if (cbTasks.SelectedIndex == 3)
                hesh = new HeshGrid(sphArea.Particles, area, HeshSize, 0.86602541f, false);
            else
                hesh = new HeshGrid(sphArea.Particles, area, HeshSize, 1, false);
            hesh.BuildHeshGrid(Вoundary, cbBoundType.SelectedIndex);
        }
        bool test0 = false;

        /// <summary>
        /// Проверка распределения плотности в области 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DensityTest_Click(object sender, EventArgs e)
        {
            CreateAreaAndHesh(GetВoundary(cbBoundary.SelectedIndex), new Vector2());
            sphSolver = new SPHSolver(hesh, areaBody,cb_PressModel.SelectedIndex, comboBoxKernel.SelectedIndex, ParticelSize * HeshScale);
            sphSolver.CalculateTestStaticDensity();
            test0 = true;
            Invalidate();
        }

        bool test1 = false;

        BMShow bmshow = new BMShow();
        /// <summary>
        /// Построение графиков функций ядра
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonKernel_Click(object sender, EventArgs e)
        {
            // контейнер графиков
            GData gd = new GData();
            List<string> names = KernelManager.GetNamesKernels();
            int Index = comboBoxKernel.SelectedIndex;
            // Заполнение данными
            float h = (float)(numericUpDown_k.Value)/1000f;
            int Dim = 2;
            // ядро
            BaseKernel Kernel = KernelManager.CreateKernel(Index, h, Dim);
            // Первый график
            GCurve W = new GCurve("W для " + names[Index]);
            // Второй график
            GCurve dW = new GCurve("dW для " + names[Index]);
            int Count = 45;
            float L = Kernel.GetKernelRadius();

            float dx = L / (Count - 1);
            for (int j = 0; j < Count; j++)
            {
                float x = j * dx;
                Vector2 r = new Vector2(x, 0);
                float w = Kernel.Calculate(ref r);
                Vector2 dw = Kernel.CalculateGradient(ref r);
                DPoint p = new DPoint(x, w);
                W.Add(p);
                DPoint dp = new DPoint(x, dw.X);
                Console.WriteLine(dp.ToString());
                dW.Add(dp);
            }
            // Добавление графиков в контейнер графиков
            gd.Add(W);
            gd.Add(dW);
            // Передача контейнера графиков в форму визуализации
            bmshow.SetData(gd);
            // отрисовка
            bmshow.ShowDialog();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // тестирование области частиц
            CreateArea(new Vector2(), false, false);
            test1 = true;
            Invalidate();
        }
        bool test2 = false;
        private void button2_Click(object sender, EventArgs e)
        {
            CreateAreaAndHesh(GetВoundary(cbBoundary.SelectedIndex), new Vector2(0, 0));
            test2 = true;
            Invalidate();
        }
        bool test23 = false;
        int Index = 0;
        private void button2_1_Click(object sender, EventArgs e)
        {
            Index = (int)numericUpDownIndex.Value;
            // тестирование области частиц и хеш сетки
            CreateAreaAndHesh(GetВoundary(cbBoundary.SelectedIndex), new Vector2(1, 1));
            test1 = true;
            test23 = true;
            Invalidate();
        }
        bool test24 = false;
        private void button2_2_Click(object sender, EventArgs e)
        {
            // тестирование области частиц и хеш сетки
            CreateAreaAndHesh(GetВoundary(cbBoundary.SelectedIndex), new Vector2(1, 1));
            test24 = true;
            Invalidate();
        }
        bool test25 = false;
        private void button2_3_Click(object sender, EventArgs e)
        {
            // тестирование области частиц и хеш сетки
            CreateAreaAndHesh(GetВoundary(cbBoundary.SelectedIndex), new Vector2(1, 1));
            test25 = true;
            Invalidate();
        }

        int SPH_task = 0;
        bool test3 = false;

        bool test4 = false;

        private void ClearEE()
        {
            Ek.Clear();
            Ep.Clear();
            Tm.Clear();
            //Vm.Clear();
            DnsErr.Clear();
            Track.Clear();
        }

        bool test5 = false; 
        /// <summary>
        /// Движение жидкости
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EulerTask_Click(object sender, EventArgs e)
        {
            TskInd = TaskIndex;
            
            SPH_task = 3;
            Vector2 v = new Vector2(0, 0);
            // Задачи
            switch (TaskIndex)
            {
                case 0: // статика
                    Particle.G.X =  0f; 
                    Particle.G.Y = -9.81f;
                    break;
                case 1: // Обрушение столба
                    Particle.G.X = 0f;
                    Particle.G.Y = -9.81f;
                    break;
                case 3: // Труба
                    Particle.G.X = 0.2f; //тянет только вдоль оси трубы ОХ
                    Particle.G.Y = 0f;
                    //v.Y = (Physics.NaturalDensety * Particle.G.X) / (2 * Physics.NaturalViscosity); //для установившегося течения
                    break;
                case 2: // Река
                    float a = 0.03f;
                    Particle.G.X = 9.81f * a; //даем уклон плоскости течения
                    Particle.G.Y = -9.81f * (float)Math.Sqrt(1 - a * a);
                    break;
            }
            CreateAreaAndHesh(GetВoundary(TaskIndex), v);

            ClearEE();
            // генерация решателя
            sphSolver = new SPHSolver(hesh, areaBody, cb_PressModel.SelectedIndex, comboBoxKernel.SelectedIndex, ParticelSize * HeshScale);
            // старт с буфера
            if (cbStartFromBuff.Checked == true)
                button14_Click(sender, e);
            //Вычисление характерного периода
            sphSolver.SetTimeScale(TaskIndex);
            //собственно запуск решения
            button11.Enabled = false;
            button10.Enabled = false;
            cbModel.Enabled = false;
            cbValueAll.Enabled = false;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerAsync();
        }
        bool test6 = false;
        int IndexModel = 0;
        /// <summary>
        /// Движение гранул
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SandTask_Click(object sender, EventArgs e)
        {
            SPH_task = 4;

            CreateAreaAndHesh(GetВoundary(cbBoundary.SelectedIndex), new Vector2(0, 0));
            ClearEE();
            // движение по Эйлеру
            sphSolver = new SPHSolver(hesh, areaBody,cb_PressModel.SelectedIndex, comboBoxKernel.SelectedIndex, ParticelSize * HeshScale);
            // старт с буфера
            if (cbStartFromBuff.Checked == true)
                button14_Click(sender, e);

            sphSolver.SetTimeScale(TaskIndex);

            button10.Enabled = false;
            button11.Enabled = false;
            cbValue.Enabled = false;

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerAsync();
            IndexModel = cbModel.SelectedIndex;
        }

        bool pausa = false;

        private void Pause_Click(object sender, EventArgs e)
        {
            string[] str = { "Пауза", "Отмена паузы" };
            pausa = !pausa;
            button3.Text = str[pausa == true ? 1 : 0];
        }
        private void NoPause_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();

            button10.Enabled = true;
            button11.Enabled = true;
            cbValue.Enabled = true;
            cbModel.Enabled = true;
            cbValueAll.Enabled = true;
            IndexStopTime = 0;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int k = 0;
            //float[] t;
            for (;;)
            {
                if (pausa == false)
                {
                    if (SPH_task == 1)
                        sphSolver.CalculateTestFreeMove();
                    if (SPH_task == 2)
                        sphSolver.CalculateTestDensity();
                    if ( SPH_task > 2 )
                    {
                        if (SPH_task == 3)
                            sphSolver.CalculateGlobalForces();
                        else
                            sphSolver.Calculate(IndexModel);
                    }

                    // Количество шагов по времени
                    if (k % 201 == 0)
                    {
                        //if (TaskIndex < 2)
                        {
                            Ek.Add(sphSolver.EnergyKinetic);
                            Ep.Add(sphSolver.EnergyPotential);
                        }
                       /* else
                        {
                            //Vm.Add(sphSolver.MidVelosity);
                            //DnsErr.Add(sphSolver.DensityErr);//это выводим погрешности плотности
                            //DnsErr.Add(sphSolver.VelocityErr);//это выводим погрешности производной скорости, пока так...

                            Track.Add(sphSolver.Tracking);//это выводим колебания частиц поперек течения...
                        }*/

                        Tm.Add(sphSolver.relativeScale);

                        backgroundWorker1.ReportProgress(k);
                        //if (SPH_task == 2) Thread.Sleep(50);
                        k = 1;
                    }
                    k++;
                }
                // Проверяем, есть ли запрос на отмену процесса
                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }
        string str = "";
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            str = e.ProgressPercentage.ToString();
            ReShow();
        }
        public void ReShow()
        {
            if (SPH_task == 1)
                test3 = true;
            if (SPH_task == 2)
                test4 = true;
            if(cbShowRho.Checked == true)
                test4 = true;
            if (SPH_task == 3)
                test5 = true;
            if (SPH_task == 4)
                test6 = true;
            Invalidate();
        }
        private void TFormLion_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawString(str, new Font("Arial", 10), new SolidBrush(Color.Green), 10, 10);

            int ck = cbShowBoud.Checked == true ? 1 : 0;

            if (sphArea != null && test1)
                sphArea.ShowBody(g, scaleVelocity);

            if (hesh != null && test2)
                hesh.ShowBody(g,scaleVelocity);

            if (hesh != null && test23)
                hesh.ShowSelectBody(Index, g, scaleVelocity);

            if (hesh != null && test24)
            {
                int idx = (int)numericUpDownCol.Value;
                hesh.ShowSelectCol(idx, g, scaleVelocity);
            }
            if (hesh != null && test25)
            {
                int idx = (int)numericUpDownRow.Value;
                hesh.ShowSelectRow(idx, g, scaleVelocity);
            }
            if (sphSolver != null && test3)
                sphSolver.ShowParticle(g, scaleVelocity);

            if (sphSolver != null && test4)
                sphSolver.ShowParticleDensity(g);

            if (sphSolver != null && test0)
                sphSolver.ShowParticleDensity(g);

            if (sphSolver != null && test5)
            {
                int idx = cbValue.SelectedIndex;
                //bool flagBS = cbBufferShow.Checked;
                sphSolver.ShowParticleWater(g, idx, scaleVelocity, ck, false);
            }
            if (sphSolver != null && test6)
            {
                //int idx = cbValueAll.SelectedIndex;
                int idx = cbValue.SelectedIndex;
                //bool flagBS = cbBufferShow.Checked;
                sphSolver.ShowParticleWater(g, idx, scaleVelocity, ck, false);
            }
            if (testErr == true)
            {
                Font font = new Font("Arial", 12);
                g.DrawString("maxErrL2Press: " + EPress.ToString(), font, Brushes.Black, new Point(10, 220));
                g.DrawString("max Err Press: " + EPressMax.ToString(), font, Brushes.Black, new Point(10, 250));
            }
            test0 = false;
            test1 = false;
            test2 = false;
            test23 = false;
            test24 = false;
            test25 = false;
            test3 = false;
            test4 = false;
            test5 = false;
            test6 = false;
        }

        private void comboBoxKernel_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Index = comboBoxKernel.SelectedIndex;
            // ядро
            BaseKernel Kernel = KernelManager.CreateKernel(Index, 2);
            numericUpDown_k.Value = (int)(Kernel.GetHeshScale() * 1000);
            numericUpDownDim.Value = (int)Kernel.GetKernelRadius();
        }

        float EPress = 0;
        float EPressMax = 0;
        float AEPress = 0;
        float AEPressMax = 0;
        bool testErr = false;

        private void button15_Click(object sender, EventArgs e)
        {
            string name;
            // контейнер графиков
            GData gd = new GData();
            int index;
            if (SPH_task == 3)
            {
                name = cbValue.Items[cbValue.SelectedIndex].ToString();
                index = cbValue.SelectedIndex;
            }
            else
            {
                name = cbValueAll.Items[cbValueAll.SelectedIndex].ToString();
                index = cbValueAll.SelectedIndex;
            }
            if (index == 2)
            {
                // Первый график
                GCurve Press = new GCurve(name);
                // Второй график
                GCurve PressA = new GCurve("Аналитическое " + name);
                // Второй график
                GCurve PressB = new GCurve("Аналитическое эталон " + name);
                // Численное решение
                var YY = sphSolver.Particles.Select(p => p.Position.Y).ToArray();
                float ymax = YY.Max();
                float ymin = YY.Min();
               
                // ________________ Первый график _________________
                GCurve W = new GCurve(name);
                for (int i = 0; i < sphSolver.sumShow.Length; i++)
                {
                    DPoint p = new DPoint(sphSolver.argSumShow[i] + ymin, sphSolver.sumShow[i]);
                    W.Add(p);
                }
                // _____________ Второй график на расчетном размере области _____________
                float bourder = ymin + (ymax - ymin) * Area.alpha;
                if (bourder > ymax)
                    bourder = ymax;
                float Pressure;
                // текущий размер области
                float HY = ymax - ymin;
                int CountP = (int)(HY / ParticelSize);
                float dy = HY / (CountP - 1);
                float y = ymin;
                // аналитика
                for (int i = 0; i < CountP; i++)
                {
                    Pressure = sphSolver.GetPressure(y, bourder, ymax);
                    DPoint pa = new DPoint(y, Pressure);
                    PressA.Add(pa);
                    y += dy;
                }
                // _____________ Третий график на исходном размере области _____________
                int Count = (int)Math.Sqrt( sphSolver.Particles.Length );
                dy = areaBody.Heigh / (Count - 1);
                for (int i = 0; i < Count; i++)
                {
                    y = areaBody.a.Y + i * dy;
                    // аналитика
                    Pressure = sphSolver.GetPressure(y, areaBody);
                    DPoint pa = new DPoint(y, Pressure);
                    PressB.Add(pa);
                }

                // Добавление графиков в контейнер графиков
                gd.Add(W);
                gd.Add(PressA);
                gd.Add(PressB);
            }
            else
            {
                // Первый график
                GCurve W = new GCurve(name);
                for (int i = 0; i < sphSolver.sumShow.Length; i++)
                {
                    DPoint p = new DPoint(sphSolver.argSumShow[i], sphSolver.sumShow[i]);
                    W.Add(p);
                }
                // Добавление графиков в контейнер графиков
                gd.Add(W);
            }
            //gd.Add(dW);
            // Передача контейнера графиков в форму визуализации
            bmshow.SetData(gd);
            // отрисовка
            bmshow.ShowDialog();
            //string name;
            //// контейнер графиков
            //GData gd = new GData();
            //int idxValue = 0;
            //if (SPH_task == 3)
            //{
            //    name = cbValue.Items[cbValue.SelectedIndex].ToString();
            //    idxValue = cbValue.SelectedIndex;
            //}
            //else
            //{
            //    name = cbValueAll.Items[cbValueAll.SelectedIndex].ToString();
            //    idxValue = cbValueAll.SelectedIndex;
            //}
            //var YY = sphSolver.Particles.Select(p => p.Position.Y).ToArray();
            //float ymax = YY.Max();
            //float ymin = YY.Min();
            //// Первый график
            //GCurve W = new GCurve(name);
            //for (int i = 0; i < sphSolver.sumShow.Length; i++)
            //{
            //    DPoint p = new DPoint(sphSolver.argSumShow[i] + ymin, sphSolver.sumShow[i]);
            //    W.Add(p);
            //}
            //// Добавление графиков в контейнер графиков
            //gd.Add(W);
            //if (idxValue == 2) // давление
            //{
            //    // Второй график
            //    GCurve PressA = new GCurve("Аналитическое " + name);
            //    float bourder = ymin + (ymax - ymin) * Area1D.alpha;
            //    if (bourder > ymax)
            //        bourder = ymax;
            //    float Pressure;
            //    float HY = ymax - ymin;
            //    int CountP = (int)(HY / ParticelSize);
            //    float dy = HY / (CountP - 1);
            //    float y = ymin;
            //    // аналитика
            //    for (int i = 0; i < CountP; i++)
            //    {
            //        Pressure = sphSolver.GetPressure(y, bourder, ymax);
            //        DPoint pa = new DPoint(y, Pressure);
            //        PressA.Add(pa);
            //        y += dy;
            //    }
            //    // Добавление графиков в контейнер графиков
            //    gd.Add(PressA);

            //    EPress = 0;
            //    EPressMax = 0;
            //    //float PMax = sphSolver.Particles.Select(p => p.Pressure).ToArray().Max();
            //    //for (int i = 0; i < sphSolver.Particles.Length; i++)
            //    //{
            //    //        float x = sphSolver.Particles[i].Position.X;
            //    //        float P = sphSolver.Particles[i].Pressure;
            //    //        // аналитика
            //    //        Pressure = sphSolver.GetPressure(x, bourder, ymin);
            //    //        float err = (P - Pressure) / (PMax + 0.0000001f);
            //    //        if (Math.Abs(err) > EPressMax)
            //    //            EPressMax = Math.Abs(err);
            //    //    }

            //    //    // Расчет ошибок давления 
            //    //    for (int i = 0; i < sphSolver.Particles.Length - 1; i++)
            //    //    {
            //    //        float xA = sphSolver.Particles[i].Position.X;
            //    //        float xB = sphSolver.Particles[i + 1].Position.X;
            //    //        float h = xB - xA;
            //    //        float x = (xB + xA) / 2f;
            //    //        float PA = sphSolver.Particles[i].Pressure;
            //    //        float PB = sphSolver.Particles[i + 1].Pressure;
            //    //        float P = (PB + PA) / 2f;
            //    //        // аналитика
            //    //        Pressure = sphSolver.GetPressure(x, bourder, ymin);
            //    //        float err = (P - Pressure) / (PMax + 0.0000001f);
            //    //        EPress += err * err * h;
            //    //    }
            //    //    EPress = (float)Math.Sqrt(EPress);
            //    //    testErr = true;

            //}
            //// Передача контейнера графиков в форму визуализации
            //bmshow.SetData(gd);
            //// отрисовка
            //bmshow.ShowDialog();
        }
        /// <summary>
        /// Отрисовка энергий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemDynamic_Click(object sender, EventArgs e)
        {
            // контейнер графиков
            GData gd = new GData();

            bool b = false; 

            if (b) // это для получения графиков изменений свойств для отдельных частиц 
            {
                if (Track.Count < 2) return;
                int countT = Track.Count;
                int countC = Track[0].Count();
                List<GCurve> Tr = new List<GCurve>();
                for (int k = 0; k < countC; k++)
                {
                    Tr.Add(new GCurve("N="+k));
                }

                for (int i = 0; i < countT; i++)
                   
                {
                    for (int k = 0; k < countC; k++)
                    {
                        DPoint p = new DPoint(Tm[i], Track[i][k]);
                        Tr[k].Add(p);
                    }
                }
                // Добавление графиков в контейнер графиков
                for (int k = 0; k < countC; k++)
                {
                    gd.Add(Tr[k]);
                }
                // Передача контейнера графиков в форму визуализации
                bmshow.SetData(gd);
                // отрисовка
                bmshow.ShowDialog();
                return;
            }

            //а это для графиков применительно к объему целиком
            if (TaskIndex < 2)
            {
                if (Ek.Count < 2) return;
                // Первый график
                GCurve EK = new GCurve("Кинетическая энергия");
                // Второй график
                GCurve EP = new GCurve("Потенциальная энергия");
                for (int i = 0; i < Ek.Count; i++)
                {
                    DPoint p = new DPoint(Tm[i], Ek[i]);
                    EK.Add(p);
                    p.Y = Ep[i];
                    EP.Add(p);
                }
                // Добавление графиков в контейнер графиков
                gd.Add(EK);
                gd.Add(EP);
             }
            else
            {
                /* if (Vm.Count < 2) return;
                 GCurve VM = new GCurve("Скорость потока");
                 for (int i = 0; i < Vm.Count; i++)
                 {
                     DPoint p = new DPoint(Tm[i], Vm[i]);
                     VM.Add(p);
                 }
                 gd.Add(VM);*/

                if (DnsErr.Count < 2) return;
                GCurve DNS = new GCurve("Погрешность плотности");
                for (int i = 0; i < DnsErr.Count; i++)
                {
                    DPoint p = new DPoint(Tm[i], DnsErr[i]);
                    DNS.Add(p);
                }
                gd.Add(DNS);
            }
            // Передача контейнера графиков в форму визуализации
            bmshow.SetData(gd);
            // отрисовка
            bmshow.ShowDialog();
        }

        private void cbValueAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReShow();
        }

        private void cbValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReShow();
        }

        #region Работа с буферами обмена
        private void button14_Click(object sender, EventArgs e)
        {
            using (StreamReader file = new StreamReader("buffer.txt"))
            {
                int Length = int.Parse(file.ReadLine());
                if (Length == sphSolver.Particles.Length)
                {
                    for (int i = 0; i < sphSolver.Particles.Length; i++)
                        sphSolver.Particles[i].StringToParticle(file.ReadLine());
                }
                file.Close();
            }
        }

        private void SaveBuffer_Click(object sender, EventArgs e)
        {
            // находим текущую папку приложения
            string sourceActive = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // настраиваем окно диалога на текущую папку приложения
            saveFileDialog1.InitialDirectory = sourceActive;
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|vtk files (*.vtk)|*.vtk|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = saveFileDialog1.FileName;
                using (StreamWriter file = new StreamWriter(FileName))
                {
                    if (file == null) return;
                    if (saveFileDialog1.FilterIndex == 1)
                    {
                        file.WriteLine(sphSolver.Particles.Length);
                        for (int i = 0; i < sphSolver.Particles.Length; i++)
                            file.WriteLine(sphSolver.Particles[i].ToString());
                    }
                    else 
                    {
                        sphSolver.SaveDataToVtk(file);
                    }
                    file.Close();
                }
            }
        }
        private void CopyBuffer_Click(object sender, EventArgs e)
        {
            // находим текущую папку приложения
            string sourceActive = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // настраиваем окно диалога на текущую папку приложения
            openFileDialog1.InitialDirectory = sourceActive;
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            string saveFile = sourceActive + "\\buffer.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string sourceFile = openFileDialog1.FileName;
                if (saveFile == sourceFile) 
                    return;
                if (File.Exists(saveFile) == true)
                    File.Delete(saveFile);
                File.Copy(sourceFile, saveFile);
            }
        }
        #endregion

        private void cbShowBoud_CheckedChanged(object sender, EventArgs e)
        {

        }

     }
}
