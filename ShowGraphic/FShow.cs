using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShowGraphic
{
    public partial class FShow : Form
    {
        //вид графика
        bool FlagGraph = false;
        //количество узлов
        public int CountLine = 0;
        //количество кривых
        public int CountCurve = 0;
        //массив графиков
        public double[][] Data = null;
        //рабочий мсштаб по Х
        public double WLengthX = 1.0f;
        //рабочий мсштаб по Y
        public double WLengthY = 0.4f;
        //наименованеи графиков
        public string GraphName = "";
        string[] Names = null;
        
        public FShow()
        {
            InitializeComponent();
        }

        private string ScaleGraphL(double Length, int idx)
        {
            int i = idx % 11;
            //деления на координатных осях
            string[] scale = { "0", "0.1", "0.2", "0.3", "0.4", "0.5", "0.6", "0.7", "0.8", "0.9", "1.0" };
            string SLengthF = (double.Parse(scale[i]) * Length).ToString();
            string[] SS = SLengthF.Split('.');
            string SLength;
            if (SS.Length == 2)
            {
                int LS = SS[1].Length;
                if (LS > 4)
                    LS = 4;
                SLength = SS[0] + "." + SS[1].Substring(0, LS);
            }
            else
                SLength = SS[0];
            return SLength;
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    Invalidate();
        //}

        private void FShow_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                Pen penRed = new Pen(Color.Red, 2);
                Pen[] CPens = new Pen[13];
                Pen penGreen = new Pen(Color.Green, 2);
                Pen penBlack = new Pen(Color.Black, 2);
                Pen penGray = new Pen(Color.Gray, 2);

                CPens[0] = new Pen(Color.Black, 2);
                CPens[1] = new Pen(Color.Green, 2);
                CPens[2] = new Pen(Color.RoyalBlue, 2);
                CPens[3] = new Pen(Color.HotPink, 2);
                CPens[4] = new Pen(Color.Red, 2);
                CPens[5] = new Pen(Color.Plum, 2);
                CPens[6] = new Pen(Color.Aqua, 2);
                CPens[7] = new Pen(Color.Purple, 2);
                CPens[8] = new Pen(Color.Gray, 2);
                CPens[9] = new Pen(Color.Teal, 2);
                CPens[10] = new Pen(Color.Violet, 2);
                CPens[11] = new Pen(Color.Tan, 2);
                CPens[12] = new Pen(Color.Salmon, 2);

                Pen[] pens = new Pen[4];
                pens[0] = penBlack;
                pens[1] = penGreen;
                pens[2] = penRed;

                g.DrawString(GraphName, new Font("Arial", 14), Brushes.Black, 160, 15);

                //привязка системы координат
                int W = this.Width - 120;
                int H = this.Height - 50;
                if (FlagGraph == true)
                    H = (this.Height - 50) / 2;

                int DWTop = 50;
                int DWBotton = H - 50;
                int DWTopBotton = 2 * DWBotton - DWTop;
                int DWLeft = 70;
                int DWRight = W - 50;
                int CountStart = 0;

                //координатные оси
                Point a = new Point(DWLeft, DWBotton);
                Point b = new Point(DWRight + 10, DWBotton);
                Point c = new Point(DWLeft, DWTop - 10);
                Point mc = new Point(DWLeft, DWTopBotton);
                g.DrawLine(penBlack, a, b);
                if (FlagGraph == true)
                {
                    g.DrawLine(penBlack, c, mc);
                    CountStart = 1;
                }
                else
                    g.DrawLine(penBlack, a, c);

                //горизонтальные
                int LengthX = DWRight - DWLeft;
                int DeltX = (int)(LengthX / 4);
                int DeltXG = (int)(LengthX / 10);

                for (int i = CountStart; i < 11; i++)
                {
                    int x = DWLeft + DeltXG * i;
                    a = new Point(x, DWBotton);
                    b = new Point(x, DWBotton + 5);
                    g.DrawLine(penBlack, a, b);
                    string SLength = ScaleGraphL(WLengthX, i);
                    g.DrawString(SLength, new Font("Arial", 12), Brushes.Black, x - 10, DWBotton + 10);
                }

                //вертикальные
                int LengthY = DWBotton - DWTop;
                int DeltY = (int)(LengthY / 10);
                int CountPoint = 11;
                for (int i = 0; i < CountPoint; i++)
                {
                    int y = DWBotton - DeltY * i;
                    a = new Point(DWLeft - 5, y);
                    b = new Point(DWLeft, y);
                    g.DrawLine(penBlack, a, b);
                    string SLength = ScaleGraphL(WLengthY, i);
                    g.DrawString(SLength, new Font("Arial", 12), Brushes.Black, DWLeft - 65, y - 10);
                }
                if (FlagGraph == true)
                {
                    for (int i = 1; i < 11; i++)
                    {
                        int y = DWBotton + DeltY * i;
                        a = new Point(DWLeft - 5, y);
                        b = new Point(DWLeft, y);
                        g.DrawLine(penBlack, a, b);
                        string SLength = ScaleGraphL(WLengthY, i);
                        g.DrawString("-" + SLength, new Font("Arial", 12), Brushes.Black, DWLeft - 65, y - 10);
                    }
                }

                //отрисовка графиков
                if (Data != null)
                {
                    double dx = WLengthX / (CountLine - 1);
                    //количество кривых
                    for (int idx = 0; idx < CountCurve; idx++)
                    {
                        CheckState Flag = cListBoxFiltr.GetItemCheckState(idx);
                        if (Flag == CheckState.Unchecked)
                            continue;
                        //количество узлов
                        for (uint i = 0; i < CountLine - 1; i++)
                        {
                            int xa = DWLeft + (int)(LengthX * (dx * i) / WLengthX);
                            int xb = DWLeft + (int)(LengthX * (dx * i + dx) / WLengthX);

                            int ya = DWTop + (int)(LengthY * (1 - Data[idx][i] / WLengthY));
                            int yb = DWTop + (int)(LengthY * (1 - Data[idx][i + 1] / WLengthY));

                            a = new Point(xa, ya);
                            b = new Point(xb, yb);
                            if (checkBox1.Checked == true)
                                g.DrawLine(CPens[0], a, b);
                            else
                                g.DrawLine(CPens[idx % 13], a, b);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Name = ee.Message;
            }
        }

        public void SetcListBoxFiltr()
        {
            cListBoxFiltr.Items.Clear();
            for (int i = 0; i < CountCurve; i++)
            {
                cListBoxFiltr.Items.Add("Кривая" + i.ToString(), true);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                WLengthY = double.Parse(textBox1.Text);
                Invalidate();
            }
            catch (Exception ee)
            {
                Name = ee.Message;
            }
        }

        private void FShow_Load(object sender, EventArgs e)
        {
            textBox1.Text = WLengthY.ToString();
        }

        private void cListBoxFiltr_SelectedIndexChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void cb2K_CheckedChanged(object sender, EventArgs e)
        {
            FlagGraph = cb2K.Checked;
            Invalidate();
        }
        public void Set(double[][] data, string GName, double WLX, int CLine,int CCurve )
        {
            WLengthX = WLX;
            GraphName = GName;
            CountLine = CLine;
            CountCurve = CCurve;
            Data = data;
            SetcListBoxFiltr();
            WLengthY =  1.2f*Math.Abs( Data[CCurve - 1].Max() );
            textBox1.Text = WLengthY.ToString();
            WLengthY = double.Parse(textBox1.Text);
            Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

                        

    }
}
