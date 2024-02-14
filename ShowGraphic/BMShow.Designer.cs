namespace ShowGraphic
{
    partial class BMShow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pBox = new System.Windows.Forms.PictureBox();
            this.cbCoord2K = new System.Windows.Forms.CheckBox();
            this.cListBoxFiltr = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxScale = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbScale = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxScaleXmax = new System.Windows.Forms.TextBox();
            this.textBoxScaleXmin = new System.Windows.Forms.TextBox();
            this.btActive = new System.Windows.Forms.Button();
            this.rbManual = new System.Windows.Forms.RadioButton();
            this.rbAuto = new System.Windows.Forms.RadioButton();
            this.button2 = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pBox
            // 
            this.pBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pBox.BackColor = System.Drawing.SystemColors.Window;
            this.pBox.Location = new System.Drawing.Point(-2, -1);
            this.pBox.Name = "pBox";
            this.pBox.Size = new System.Drawing.Size(838, 789);
            this.pBox.TabIndex = 0;
            this.pBox.TabStop = false;
            // 
            // cbCoord2K
            // 
            this.cbCoord2K.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCoord2K.AutoSize = true;
            this.cbCoord2K.Checked = true;
            this.cbCoord2K.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCoord2K.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbCoord2K.Location = new System.Drawing.Point(849, 26);
            this.cbCoord2K.Name = "cbCoord2K";
            this.cbCoord2K.Size = new System.Drawing.Size(149, 24);
            this.cbCoord2K.TabIndex = 6;
            this.cbCoord2K.Text = "Полный график";
            this.cbCoord2K.UseVisualStyleBackColor = true;
            this.cbCoord2K.CheckedChanged += new System.EventHandler(this.cbCoord2K_CheckedChanged);
            // 
            // cListBoxFiltr
            // 
            this.cListBoxFiltr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cListBoxFiltr.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.cListBoxFiltr.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cListBoxFiltr.FormattingEnabled = true;
            this.cListBoxFiltr.Location = new System.Drawing.Point(845, 96);
            this.cListBoxFiltr.Name = "cListBoxFiltr";
            this.cListBoxFiltr.Size = new System.Drawing.Size(225, 319);
            this.cListBoxFiltr.TabIndex = 7;
            this.cListBoxFiltr.SelectedIndexChanged += new System.EventHandler(this.cListBoxFiltr_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(950, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Список кривых";
            // 
            // textBoxScale
            // 
            this.textBoxScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxScale.Location = new System.Drawing.Point(87, 83);
            this.textBoxScale.Name = "textBoxScale";
            this.textBoxScale.Size = new System.Drawing.Size(86, 26);
            this.textBoxScale.TabIndex = 10;
            this.textBoxScale.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBoxScale_MouseClick);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(33, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "Масштаб";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel1.Controls.Add(this.cbScale);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.textBoxScaleXmax);
            this.panel1.Controls.Add(this.textBoxScaleXmin);
            this.panel1.Controls.Add(this.btActive);
            this.panel1.Controls.Add(this.rbManual);
            this.panel1.Controls.Add(this.textBoxScale);
            this.panel1.Controls.Add(this.rbAuto);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(845, 454);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(225, 208);
            this.panel1.TabIndex = 12;
            // 
            // cbScale
            // 
            this.cbScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbScale.AutoSize = true;
            this.cbScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbScale.Location = new System.Drawing.Point(12, 181);
            this.cbScale.Name = "cbScale";
            this.cbScale.Size = new System.Drawing.Size(138, 24);
            this.cbScale.TabIndex = 15;
            this.cbScale.Text = "Авто масштаб";
            this.cbScale.UseVisualStyleBackColor = true;
            this.cbScale.CheckedChanged += new System.EventHandler(this.cbScale_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(8, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 20);
            this.label4.TabIndex = 19;
            this.label4.Text = "xb";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(8, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 20);
            this.label3.TabIndex = 18;
            this.label3.Text = "xa";
            // 
            // textBoxScaleXmax
            // 
            this.textBoxScaleXmax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxScaleXmax.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxScaleXmax.Location = new System.Drawing.Point(36, 140);
            this.textBoxScaleXmax.Name = "textBoxScaleXmax";
            this.textBoxScaleXmax.Size = new System.Drawing.Size(137, 26);
            this.textBoxScaleXmax.TabIndex = 17;
            this.textBoxScaleXmax.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBoxScale_MouseClick);
            // 
            // textBoxScaleXmin
            // 
            this.textBoxScaleXmin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxScaleXmin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxScaleXmin.Location = new System.Drawing.Point(36, 112);
            this.textBoxScaleXmin.Name = "textBoxScaleXmin";
            this.textBoxScaleXmin.Size = new System.Drawing.Size(137, 26);
            this.textBoxScaleXmin.TabIndex = 16;
            this.textBoxScaleXmin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBoxScale_MouseClick);
            // 
            // btActive
            // 
            this.btActive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btActive.BackColor = System.Drawing.SystemColors.Control;
            this.btActive.Location = new System.Drawing.Point(179, 83);
            this.btActive.Name = "btActive";
            this.btActive.Size = new System.Drawing.Size(35, 83);
            this.btActive.TabIndex = 15;
            this.btActive.Text = "Ок";
            this.btActive.UseVisualStyleBackColor = false;
            this.btActive.Click += new System.EventHandler(this.rbAuto_CheckedChanged);
            // 
            // rbManual
            // 
            this.rbManual.AutoSize = true;
            this.rbManual.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbManual.Location = new System.Drawing.Point(12, 83);
            this.rbManual.Name = "rbManual";
            this.rbManual.Size = new System.Drawing.Size(69, 21);
            this.rbManual.TabIndex = 13;
            this.rbManual.Text = "Выбор";
            this.rbManual.UseVisualStyleBackColor = true;
            this.rbManual.CheckedChanged += new System.EventHandler(this.rbAuto_CheckedChanged);
            // 
            // rbAuto
            // 
            this.rbAuto.AutoSize = true;
            this.rbAuto.Checked = true;
            this.rbAuto.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbAuto.Location = new System.Drawing.Point(12, 48);
            this.rbAuto.Name = "rbAuto";
            this.rbAuto.Size = new System.Drawing.Size(57, 21);
            this.rbAuto.TabIndex = 12;
            this.rbAuto.TabStop = true;
            this.rbAuto.Text = "Авто";
            this.rbAuto.UseVisualStyleBackColor = true;
            this.rbAuto.CheckedChanged += new System.EventHandler(this.rbAuto_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(955, 737);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(115, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Закрыть";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "Bmp";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(955, 691);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Сохранить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox1.Location = new System.Drawing.Point(848, 68);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(51, 21);
            this.checkBox1.TabIndex = 78;
            this.checkBox1.Text = "Все";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // BMShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1082, 789);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cListBoxFiltr);
            this.Controls.Add(this.cbCoord2K);
            this.Controls.Add(this.pBox);
            this.Name = "BMShow";
            this.Text = "BMShow";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.BMShow_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pBox;
        private System.Windows.Forms.CheckBox cbCoord2K;
        private System.Windows.Forms.CheckedListBox cListBoxFiltr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxScale;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbManual;
        private System.Windows.Forms.RadioButton rbAuto;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btActive;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxScaleXmax;
        private System.Windows.Forms.TextBox textBoxScaleXmin;
        private System.Windows.Forms.CheckBox cbScale;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}