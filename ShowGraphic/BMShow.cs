using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Globalization;

namespace ShowGraphic
{
    public partial class BMShow : Form
    {
        string TName = "Построитель функций";
        GData gdata = null;
        DPoint MinMax_X;
        DPoint MinMax_Y;
        /// <summary>
        /// начало отрезка по Х
        /// </summary>
        double X0, NX0;
        /// <summary>
        ///  конец отрезка по Х
        /// </summary>
        double X1, NX1;
        /// <summary>
        /// длина области Х
        /// </summary>
        double LY;
        double LY0;
        Bitmap img = null;
        Graphics g = null;
        bool Flag = false;
        bool FlagReShow = false;
        public BMShow()
        {
            InitializeComponent();
            this.Text = TName;
            saveFileDialog1.Filter = "graphic files(*.bmp)|*.bmp|All files(*.*)|*.*";
        }
        public void SetData(GData gdata)
        {
            img = new Bitmap(pBox.Width, pBox.Height);
            g = Graphics.FromImage(img);
            this.gdata = gdata;
            if(gdata!=null)
            {
                MinMax_X = gdata.MinMax_X();
                MinMax_Y = gdata.MinMax_Y();
                
                X0 = MinMax_X.X;
                X1 = MinMax_X.Y;
                NX0 = X0;
                NX1 = X1;
                LY0 = MinMax_Y.Max;
                LY = LY0;
                cListBoxFiltr.Items.Clear();
                // список кривых
                for (int i = 0; i < gdata.curves.Count; i++)
                    cListBoxFiltr.Items.Add(gdata.curves[i].Name, true);
                textBoxScale.Text = LY0.ToString();
                textBoxScaleXmin.Text = MinMax_X.X.ToString();
                textBoxScaleXmax.Text = MinMax_X.Y.ToString();
                if (MinMax_Y.X >= 0)
                    cbCoord2K.Checked = false;
                cbScale.Checked = false;
            }
        }
        private void BMShow_Paint(object sender, PaintEventArgs e)
        {
            try
            {
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

                if (g != null)
                {
                    if (pBox.Width != img.Width || pBox.Height != img.Height || FlagReShow == true)
                    {
                        img = new Bitmap(pBox.Width, pBox.Height);
                        g = Graphics.FromImage(img);
                        Flag = cbCoord2K.Checked;
                        FlagReShow = false;
                    }
                    //привязка системы координат
                    int W = pBox.Width - 120;
                    int H = pBox.Height -20;
                    if (cbCoord2K.Checked == true)
                        H = (pBox.Height) / 2;

                    int DWTop = 50;
                    int DWBotton = H - 50;
                    int DWTopBotton = 2 * DWBotton - DWTop;
                    int DWLeft = 90;
                    int DWRight = W - 50;
                    int CountStart = 0;

                    Point a = new Point(DWLeft, DWBotton);
                    Point b = new Point(DWRight + 10, DWBotton);
                    Point c = new Point(DWLeft, DWTop - 10);
                    Point mc = new Point(DWLeft, DWTopBotton);
                    // координатные оси
                    g.DrawLine(penBlack, a, b);

                    if (cbCoord2K.Checked == true)
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
                        string SLength = ScaleGraph.ScaleX0X1(X0, X1, i).ToString();
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
                        string SLength = ScaleGraph.Scale(LY, i);
                        g.DrawString(SLength, new Font("Arial", 12), Brushes.Black, DWLeft - 65, y - 10);
                    }
                    if (cbCoord2K.Checked == true)
                    {
                        for (int i = 1; i < 11; i++)
                        {
                            int y = DWBotton + DeltY * i;
                            a = new Point(DWLeft - 5, y);
                            b = new Point(DWLeft, y);
                            g.DrawLine(penBlack, a, b);
                            string SLength = ScaleGraph.Scale(LY, i);
                            g.DrawString("-" + SLength, new Font("Arial", 12), Brushes.Black, DWLeft - 65, y - 10);
                        }
                    }

                    //g.DrawLine(new Pen(Color.Black, 2), new Point(10, 20), new Point(333, 444));
                    //отрисовка графиков
                    if (gdata != null)
                    {
                        
                        //количество кривых
                        for (int idx = 0; idx < gdata.curves.Count; idx++)
                        {
                            GCurve curve = gdata.curves[idx];
                            List<DPoint> pps = curve.points;
                            int Count = curve.Count;
                            //
                            double WLengthX = X1-X0;
                            double WLengthY = LY;

                            double dx = curve.Get_dx;
                            // Отрисока кривой
                            CheckState flag = cListBoxFiltr.GetItemCheckState(idx);
                            if (flag == CheckState.Unchecked)
                                continue;
                            //количество узлов
                            for (int i = 0; i < Count - 1; i++)
                            {
                                var pp = pps[i];
                                var pe = pps[i+1];

                                int xa = DWLeft + (int)(LengthX * (pp.X - X0) / WLengthX);
                                int xb = DWLeft + (int)(LengthX * (pe.X - X0) / WLengthX);

                                
                                int ya = DWTop + (int)(LengthY * (1 - pp.Y / WLengthY));
                                int yb = DWTop + (int)(LengthY * (1 - pe.Y / WLengthY));

                                a = new Point(xa, ya);
                                b = new Point(xb, yb);
                              
                                g.DrawLine(CPens[0], a, b);
                              
                            }
                        }
                    }

                    pBox.Image = img;
                }
                 //g.DrawString(GraphName, new Font("Arial", 14), Brushes.Black, 160, 15);

            }
            catch (Exception ee)
            {
                Name = ee.Message;
            }
        }

        private void cbCoord2K_CheckedChanged(object sender, EventArgs e)
        {
            FlagReShow = true; 
            Invalidate();
        }

        private void cListBoxFiltr_SelectedIndexChanged(object sender, EventArgs e)
        {
            FlagReShow = true;
            Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FlagReShow = true;
            Invalidate();
        }
        private void rbAuto_CheckedChanged(object sender, EventArgs e)
        {
            this.Text = TName;
            if (rbAuto.Checked == true)
            {
                LY = LY0;
                X0 = NX0;
                X1 = NX1;
                textBoxScaleXmin.Text = MinMax_X.X.ToString();
                textBoxScaleXmax.Text = MinMax_X.Y.ToString();
            }
            else 
            {
                try
                {
                    if (cbScale.Checked == true)
                    {
                        LY = ScaleValue.ScaleConvert(LY0);
                        X1 = ScaleValue.ScaleConvert(NX1);
                        X0 = ScaleValue.ScaleConvert(NX0);
                        textBoxScaleXmin.Text = X0.ToString();
                        textBoxScaleXmax.Text = X1.ToString();
                        textBoxScale.Text = LY.ToString();
                    }
                    else
                    {
                        LY = double.Parse(textBoxScale.Text, NumberStyles.AllowDecimalPoint);
                        X0 = double.Parse(textBoxScaleXmin.Text, NumberStyles.AllowDecimalPoint);
                        X1 = double.Parse(textBoxScaleXmax.Text, NumberStyles.AllowDecimalPoint);
                    }
                }
                catch (Exception eee)
                {
                    this.Text = eee.Message;
                    textBoxScale.Text = LY0.ToString();
                    LY = LY0;
                }
            }
            FlagReShow = true;
            Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = new Bitmap(pBox.Width, pBox.Height);
                pBox.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                bmp.Save(saveFileDialog1.FileName, ImageFormat.Bmp);
            }
        }

        private void textBoxScale_MouseClick(object sender, MouseEventArgs e)
        {
            rbManual.Checked = true;
        }
        private void cbScale_CheckedChanged(object sender, EventArgs e)
        {
            if (cbScale.Checked == true)
            {
                rbManual.Checked = true;
                rbAuto_CheckedChanged(sender, e);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < cListBoxFiltr.Items.Count; i++)
                cListBoxFiltr.SetItemChecked(i, checkBox1.Checked);
            Invalidate();
        }
    }
}
