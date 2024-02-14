namespace TForm
{
    partial class TForm_2d
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.numericUpDownIndex = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownCount = new System.Windows.Forms.NumericUpDown();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.button8 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown_k = new System.Windows.Forms.NumericUpDown();
            this.cbValue = new System.Windows.Forms.ComboBox();
            this.numericUpDownCol = new System.Windows.Forms.NumericUpDown();
            this.button12 = new System.Windows.Forms.Button();
            this.cbModel = new System.Windows.Forms.ComboBox();
            this.cbValueAll = new System.Windows.Forms.ComboBox();
            this.cbBoundary = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.numericUpDownRow = new System.Windows.Forms.NumericUpDown();
            this.comboBoxKernel = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonKernel = new System.Windows.Forms.Button();
            this.numericUpDownDim = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonTKernel2 = new System.Windows.Forms.Button();
            this.numericUpDownLF = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDownBT = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numericUpDownSC = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.cbTasks = new System.Windows.Forms.ComboBox();
            this.cbBoundType = new System.Windows.Forms.ComboBox();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.cbStartFromBuff = new System.Windows.Forms.CheckBox();
            this.cbShowBoud = new System.Windows.Forms.CheckBox();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbShowRho = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.radioDouble = new System.Windows.Forms.RadioButton();
            this.radioMono = new System.Windows.Forms.RadioButton();
            this.cb_PressModel = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_k)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDim)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSC)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(214, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "1. Тест заполнения области";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(3, 28);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(213, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "2. Тест заполнения хеша";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button6.Location = new System.Drawing.Point(70, 55);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(241, 23);
            this.button6.TabIndex = 5;
            this.button6.Text = "2.1. Тест выборки из хеша";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button2_1_Click);
            // 
            // numericUpDownIndex
            // 
            this.numericUpDownIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownIndex.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numericUpDownIndex.Location = new System.Drawing.Point(27, 55);
            this.numericUpDownIndex.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownIndex.Name = "numericUpDownIndex";
            this.numericUpDownIndex.Size = new System.Drawing.Size(42, 23);
            this.numericUpDownIndex.TabIndex = 6;
            this.numericUpDownIndex.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDownCount
            // 
            this.numericUpDownCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numericUpDownCount.Location = new System.Drawing.Point(257, 74);
            this.numericUpDownCount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownCount.Name = "numericUpDownCount";
            this.numericUpDownCount.Size = new System.Drawing.Size(53, 23);
            this.numericUpDownCount.TabIndex = 7;
            this.numericUpDownCount.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button8.Location = new System.Drawing.Point(98, 238);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(76, 46);
            this.button8.TabIndex = 10;
            this.button8.Text = "Стоп";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.NoPause_Click);
            // 
            // button11
            // 
            this.button11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button11.Location = new System.Drawing.Point(3, 145);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(139, 23);
            this.button11.TabIndex = 13;
            this.button11.Text = "Жидкость";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.EulerTask_Click);
            // 
            // button10
            // 
            this.button10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button10.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button10.Location = new System.Drawing.Point(3, 177);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(139, 53);
            this.button10.TabIndex = 14;
            this.button10.Text = "Песок";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.SandTask_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(165, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 17);
            this.label1.TabIndex = 15;
            this.label1.Text = "Частиц по Х";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(237, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 17);
            this.label2.TabIndex = 16;
            this.label2.Text = "k";
            // 
            // numericUpDown_k
            // 
            this.numericUpDown_k.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown_k.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numericUpDown_k.Location = new System.Drawing.Point(255, 32);
            this.numericUpDown_k.Maximum = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            this.numericUpDown_k.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDown_k.Name = "numericUpDown_k";
            this.numericUpDown_k.ReadOnly = true;
            this.numericUpDown_k.Size = new System.Drawing.Size(55, 23);
            this.numericUpDown_k.TabIndex = 17;
            this.numericUpDown_k.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // cbValue
            // 
            this.cbValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbValue.Cursor = System.Windows.Forms.Cursors.Default;
            this.cbValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbValue.Items.AddRange(new object[] {
            "Скорость",
            "Плотность",
            "Давление"});
            this.cbValue.Location = new System.Drawing.Point(151, 146);
            this.cbValue.Name = "cbValue";
            this.cbValue.Size = new System.Drawing.Size(159, 24);
            this.cbValue.TabIndex = 18;
            this.cbValue.SelectedIndexChanged += new System.EventHandler(this.cbValue_SelectedIndexChanged);
            // 
            // numericUpDownCol
            // 
            this.numericUpDownCol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownCol.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numericUpDownCol.Location = new System.Drawing.Point(27, 83);
            this.numericUpDownCol.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownCol.Name = "numericUpDownCol";
            this.numericUpDownCol.Size = new System.Drawing.Size(42, 23);
            this.numericUpDownCol.TabIndex = 20;
            this.numericUpDownCol.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // button12
            // 
            this.button12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button12.Location = new System.Drawing.Point(70, 83);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(241, 23);
            this.button12.TabIndex = 19;
            this.button12.Text = "2.2. Тест выборки столбца";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button2_2_Click);
            // 
            // cbModel
            // 
            this.cbModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbModel.Cursor = System.Windows.Forms.Cursors.Default;
            this.cbModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbModel.Items.AddRange(new object[] {
            "ГПМ с1",
            "ГПМ с12",
            "ГПМ с123",
            "ГПМ с1234",
            "ГПМ c0"});
            this.cbModel.Location = new System.Drawing.Point(151, 206);
            this.cbModel.Name = "cbModel";
            this.cbModel.Size = new System.Drawing.Size(159, 24);
            this.cbModel.TabIndex = 21;
            // 
            // cbValueAll
            // 
            this.cbValueAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbValueAll.Cursor = System.Windows.Forms.Cursors.Default;
            this.cbValueAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbValueAll.Items.AddRange(new object[] {
            "Скорость",
            "Плотность",
            "Давление",
            "Eps_ij->J1",
            "Eps_ij->J2",
            "Eps_11",
            "Eps_22",
            "Eps_12",
            "S_ij->J2",
            "S_xx",
            "S_xy",
            "S_yy",
            "Sigma_xx",
            "Sigma_yy"});
            this.cbValueAll.Location = new System.Drawing.Point(151, 178);
            this.cbValueAll.Name = "cbValueAll";
            this.cbValueAll.Size = new System.Drawing.Size(159, 24);
            this.cbValueAll.TabIndex = 22;
            this.cbValueAll.SelectedIndexChanged += new System.EventHandler(this.cbValueAll_SelectedIndexChanged);
            // 
            // cbBoundary
            // 
            this.cbBoundary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbBoundary.Cursor = System.Windows.Forms.Cursors.Default;
            this.cbBoundary.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbBoundary.Items.AddRange(new object[] {
            "Зеркало",
            "Стенка",
            "Вязкая",
            "Периодик",
            "Сдвиг"});
            this.cbBoundary.Location = new System.Drawing.Point(222, 28);
            this.cbBoundary.Name = "cbBoundary";
            this.cbBoundary.Size = new System.Drawing.Size(88, 24);
            this.cbBoundary.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(5, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 17);
            this.label3.TabIndex = 24;
            this.label3.Text = "Тип границ";
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button3.Location = new System.Drawing.Point(2, 238);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(76, 46);
            this.button3.TabIndex = 25;
            this.button3.Text = "Пауза";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Pause_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button4.Location = new System.Drawing.Point(70, 111);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(241, 23);
            this.button4.TabIndex = 26;
            this.button4.Text = "2.3. Тест выборки строки";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button2_3_Click);
            // 
            // numericUpDownRow
            // 
            this.numericUpDownRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownRow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numericUpDownRow.Location = new System.Drawing.Point(27, 111);
            this.numericUpDownRow.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownRow.Name = "numericUpDownRow";
            this.numericUpDownRow.Size = new System.Drawing.Size(42, 23);
            this.numericUpDownRow.TabIndex = 27;
            this.numericUpDownRow.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // comboBoxKernel
            // 
            this.comboBoxKernel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxKernel.Cursor = System.Windows.Forms.Cursors.Default;
            this.comboBoxKernel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxKernel.Location = new System.Drawing.Point(120, 3);
            this.comboBoxKernel.Name = "comboBoxKernel";
            this.comboBoxKernel.Size = new System.Drawing.Size(190, 24);
            this.comboBoxKernel.TabIndex = 29;
            this.comboBoxKernel.SelectedIndexChanged += new System.EventHandler(this.comboBoxKernel_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(3, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 17);
            this.label4.TabIndex = 30;
            this.label4.Text = "Функция ядра";
            // 
            // buttonKernel
            // 
            this.buttonKernel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonKernel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonKernel.Location = new System.Drawing.Point(3, 32);
            this.buttonKernel.Name = "buttonKernel";
            this.buttonKernel.Size = new System.Drawing.Size(150, 25);
            this.buttonKernel.TabIndex = 31;
            this.buttonKernel.Text = "Графики ядра";
            this.buttonKernel.UseVisualStyleBackColor = true;
            this.buttonKernel.Click += new System.EventHandler(this.buttonKernel_Click);
            // 
            // numericUpDownDim
            // 
            this.numericUpDownDim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownDim.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numericUpDownDim.Location = new System.Drawing.Point(200, 32);
            this.numericUpDownDim.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownDim.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownDim.Name = "numericUpDownDim";
            this.numericUpDownDim.ReadOnly = true;
            this.numericUpDownDim.Size = new System.Drawing.Size(31, 23);
            this.numericUpDownDim.TabIndex = 32;
            this.numericUpDownDim.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(179, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(18, 17);
            this.label5.TabIndex = 33;
            this.label5.Text = "R";
            // 
            // buttonTKernel2
            // 
            this.buttonTKernel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTKernel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonTKernel2.Location = new System.Drawing.Point(3, 59);
            this.buttonTKernel2.Name = "buttonTKernel2";
            this.buttonTKernel2.Size = new System.Drawing.Size(150, 50);
            this.buttonTKernel2.TabIndex = 38;
            this.buttonTKernel2.Text = "Распределение плотностей";
            this.buttonTKernel2.UseVisualStyleBackColor = true;
            this.buttonTKernel2.Click += new System.EventHandler(this.DensityTest_Click);
            // 
            // numericUpDownLF
            // 
            this.numericUpDownLF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownLF.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numericUpDownLF.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownLF.Location = new System.Drawing.Point(1141, 13);
            this.numericUpDownLF.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numericUpDownLF.Name = "numericUpDownLF";
            this.numericUpDownLF.Size = new System.Drawing.Size(55, 23);
            this.numericUpDownLF.TabIndex = 39;
            this.numericUpDownLF.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.numericUpDownLF.ValueChanged += new System.EventHandler(this.numericUpDownSC_ValueChanged);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(1057, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 17);
            this.label8.TabIndex = 40;
            this.label8.Text = "Сдвиг по X";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(1216, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 17);
            this.label9.TabIndex = 41;
            this.label9.Text = "Сдвиг по Y";
            // 
            // numericUpDownBT
            // 
            this.numericUpDownBT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownBT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numericUpDownBT.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownBT.Location = new System.Drawing.Point(1299, 15);
            this.numericUpDownBT.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownBT.Name = "numericUpDownBT";
            this.numericUpDownBT.Size = new System.Drawing.Size(64, 23);
            this.numericUpDownBT.TabIndex = 42;
            this.numericUpDownBT.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numericUpDownBT.ValueChanged += new System.EventHandler(this.numericUpDownSC_ValueChanged);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(1218, 46);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 17);
            this.label10.TabIndex = 43;
            this.label10.Text = "Масштаб";
            // 
            // numericUpDownSC
            // 
            this.numericUpDownSC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownSC.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numericUpDownSC.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownSC.Location = new System.Drawing.Point(1299, 44);
            this.numericUpDownSC.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownSC.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownSC.Name = "numericUpDownSC";
            this.numericUpDownSC.Size = new System.Drawing.Size(64, 23);
            this.numericUpDownSC.TabIndex = 44;
            this.numericUpDownSC.Value = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.numericUpDownSC.ValueChanged += new System.EventHandler(this.numericUpDownSC_ValueChanged);
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label13.Location = new System.Drawing.Point(7, 33);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(57, 17);
            this.label13.TabIndex = 53;
            this.label13.Text = "Задача";
            // 
            // cbTasks
            // 
            this.cbTasks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbTasks.Cursor = System.Windows.Forms.Cursors.Default;
            this.cbTasks.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbTasks.Location = new System.Drawing.Point(121, 30);
            this.cbTasks.Name = "cbTasks";
            this.cbTasks.Size = new System.Drawing.Size(190, 24);
            this.cbTasks.TabIndex = 54;
            // 
            // cbBoundType
            // 
            this.cbBoundType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbBoundType.Cursor = System.Windows.Forms.Cursors.Default;
            this.cbBoundType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbBoundType.Location = new System.Drawing.Point(120, 3);
            this.cbBoundType.Name = "cbBoundType";
            this.cbBoundType.Size = new System.Drawing.Size(190, 24);
            this.cbBoundType.TabIndex = 61;
            // 
            // button15
            // 
            this.button15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button15.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button15.Location = new System.Drawing.Point(3, 299);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(137, 32);
            this.button15.TabIndex = 62;
            this.button15.Text = "График свойства";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button16
            // 
            this.button16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button16.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button16.Location = new System.Drawing.Point(149, 288);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(162, 43);
            this.button16.TabIndex = 63;
            this.button16.Text = "Графики состяний во времени";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.SystemDynamic_Click);
            // 
            // cbStartFromBuff
            // 
            this.cbStartFromBuff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbStartFromBuff.AutoSize = true;
            this.cbStartFromBuff.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbStartFromBuff.Location = new System.Drawing.Point(3, 116);
            this.cbStartFromBuff.Name = "cbStartFromBuff";
            this.cbStartFromBuff.Size = new System.Drawing.Size(131, 21);
            this.cbStartFromBuff.TabIndex = 64;
            this.cbStartFromBuff.Text = "Старт с буфера";
            this.cbStartFromBuff.UseVisualStyleBackColor = true;
            // 
            // cbShowBoud
            // 
            this.cbShowBoud.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbShowBoud.AutoSize = true;
            this.cbShowBoud.Checked = true;
            this.cbShowBoud.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowBoud.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbShowBoud.Location = new System.Drawing.Point(1060, 45);
            this.cbShowBoud.Name = "cbShowBoud";
            this.cbShowBoud.Size = new System.Drawing.Size(140, 21);
            this.cbShowBoud.TabIndex = 65;
            this.cbShowBoud.Text = "Орисовка границ";
            this.cbShowBoud.UseVisualStyleBackColor = true;
            this.cbShowBoud.CheckedChanged += new System.EventHandler(this.cbShowBoud_CheckedChanged);
            // 
            // button17
            // 
            this.button17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button17.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button17.Location = new System.Drawing.Point(151, 115);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(159, 24);
            this.button17.TabIndex = 67;
            this.button17.Text = "Загрузить в буфер";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.CopyBuffer_Click);
            // 
            // button18
            // 
            this.button18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button18.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button18.Location = new System.Drawing.Point(191, 238);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(119, 46);
            this.button18.TabIndex = 66;
            this.button18.Text = "Сохранить как";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.SaveBuffer_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.buttonKernel);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.comboBoxKernel);
            this.panel1.Controls.Add(this.numericUpDownDim);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.numericUpDown_k);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.buttonTKernel2);
            this.panel1.Controls.Add(this.numericUpDownCount);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(1051, 72);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(315, 118);
            this.panel1.TabIndex = 71;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.cbShowRho);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.cbBoundary);
            this.panel2.Controls.Add(this.button12);
            this.panel2.Controls.Add(this.button6);
            this.panel2.Controls.Add(this.numericUpDownIndex);
            this.panel2.Controls.Add(this.numericUpDownCol);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.numericUpDownRow);
            this.panel2.Location = new System.Drawing.Point(1051, 196);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(315, 172);
            this.panel2.TabIndex = 72;
            // 
            // cbShowRho
            // 
            this.cbShowRho.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbShowRho.AutoSize = true;
            this.cbShowRho.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbShowRho.Location = new System.Drawing.Point(10, 141);
            this.cbShowRho.Name = "cbShowRho";
            this.cbShowRho.Size = new System.Drawing.Size(171, 21);
            this.cbShowRho.TabIndex = 70;
            this.cbShowRho.Text = "Отрисовка плотности";
            this.cbShowRho.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.cb_PressModel);
            this.panel3.Controls.Add(this.radioDouble);
            this.panel3.Controls.Add(this.radioMono);
            this.panel3.Controls.Add(this.button16);
            this.panel3.Controls.Add(this.label13);
            this.panel3.Controls.Add(this.button15);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.cbBoundType);
            this.panel3.Controls.Add(this.button11);
            this.panel3.Controls.Add(this.cbTasks);
            this.panel3.Controls.Add(this.button18);
            this.panel3.Controls.Add(this.button17);
            this.panel3.Controls.Add(this.button10);
            this.panel3.Controls.Add(this.cbValue);
            this.panel3.Controls.Add(this.cbModel);
            this.panel3.Controls.Add(this.cbStartFromBuff);
            this.panel3.Controls.Add(this.cbValueAll);
            this.panel3.Controls.Add(this.button3);
            this.panel3.Controls.Add(this.button8);
            this.panel3.Location = new System.Drawing.Point(1051, 374);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(315, 350);
            this.panel3.TabIndex = 73;
            // 
            // radioDouble
            // 
            this.radioDouble.AutoSize = true;
            this.radioDouble.Location = new System.Drawing.Point(229, 92);
            this.radioDouble.Name = "radioDouble";
            this.radioDouble.Size = new System.Drawing.Size(77, 17);
            this.radioDouble.TabIndex = 69;
            this.radioDouble.Text = "Две фазы";
            this.radioDouble.UseVisualStyleBackColor = true;
            // 
            // radioMono
            // 
            this.radioMono.AutoSize = true;
            this.radioMono.Checked = true;
            this.radioMono.Location = new System.Drawing.Point(124, 92);
            this.radioMono.Name = "radioMono";
            this.radioMono.Size = new System.Drawing.Size(87, 17);
            this.radioMono.TabIndex = 68;
            this.radioMono.TabStop = true;
            this.radioMono.Text = "Однородная";
            this.radioMono.UseVisualStyleBackColor = true;
            // 
            // cb_PressModel
            // 
            this.cb_PressModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_PressModel.Cursor = System.Windows.Forms.Cursors.Default;
            this.cb_PressModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cb_PressModel.Items.AddRange(new object[] {
            "Давление (P_i/rho_i^2 + P_j/rho_j^2)  m_i",
            "Давление (P_i+P_j)/(rho_i+rho_j) m_i",
            "Давление (P_i+P_j)/(rho_i+rho_j) m_i + R_ij"});
            this.cb_PressModel.Location = new System.Drawing.Point(6, 58);
            this.cb_PressModel.Name = "cb_PressModel";
            this.cb_PressModel.Size = new System.Drawing.Size(305, 24);
            this.cb_PressModel.TabIndex = 70;
            // 
            // TForm_2d
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1374, 861);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cbShowBoud);
            this.Controls.Add(this.numericUpDownSC);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.numericUpDownBT);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.numericUpDownLF);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.DoubleBuffered = true;
            this.Name = "TForm_2d";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.TForm_2d_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TFormLion_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_k)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDim)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSC)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.NumericUpDown numericUpDownIndex;
        private System.Windows.Forms.NumericUpDown numericUpDownCount;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown_k;
        private System.Windows.Forms.ComboBox cbValue;
        private System.Windows.Forms.NumericUpDown numericUpDownCol;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.ComboBox cbModel;
        private System.Windows.Forms.ComboBox cbValueAll;
        private System.Windows.Forms.ComboBox cbBoundary;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.NumericUpDown numericUpDownRow;
        private System.Windows.Forms.ComboBox comboBoxKernel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonKernel;
        private System.Windows.Forms.NumericUpDown numericUpDownDim;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonTKernel2;
        private System.Windows.Forms.NumericUpDown numericUpDownLF;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numericUpDownBT;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numericUpDownSC;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cbTasks;
        private System.Windows.Forms.ComboBox cbBoundType;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.CheckBox cbStartFromBuff;
        private System.Windows.Forms.CheckBox cbShowBoud;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton radioDouble;
        private System.Windows.Forms.RadioButton radioMono;
        private System.Windows.Forms.CheckBox cbShowRho;
        private System.Windows.Forms.ComboBox cb_PressModel;
    }
}

