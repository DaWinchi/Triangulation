namespace TriangleDeloneWithMagnetic
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.GraphicsBox = new System.Windows.Forms.PictureBox();
            this.DrawBtn = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ScrollHeight = new System.Windows.Forms.TrackBar();
            this.ScrollWidth = new System.Windows.Forms.TrackBar();
            this.RadioMagnet1 = new System.Windows.Forms.RadioButton();
            this.RadioMagnet2 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Height2Box = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Width2Box = new System.Windows.Forms.TextBox();
            this.Height1Box = new System.Windows.Forms.TextBox();
            this.Width1Box = new System.Windows.Forms.TextBox();
            this.ScrollAngle = new System.Windows.Forms.TrackBar();
            this.StepXBox = new System.Windows.Forms.TextBox();
            this.StepYBox = new System.Windows.Forms.TextBox();
            this.UpdateBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioTriangle = new System.Windows.Forms.RadioButton();
            this.radioPotential = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.GraphicsBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScrollHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScrollWidth)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScrollAngle)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // GraphicsBox
            // 
            this.GraphicsBox.Location = new System.Drawing.Point(53, 12);
            this.GraphicsBox.Name = "GraphicsBox";
            this.GraphicsBox.Size = new System.Drawing.Size(600, 600);
            this.GraphicsBox.TabIndex = 0;
            this.GraphicsBox.TabStop = false;
            // 
            // DrawBtn
            // 
            this.DrawBtn.Location = new System.Drawing.Point(809, 558);
            this.DrawBtn.Name = "DrawBtn";
            this.DrawBtn.Size = new System.Drawing.Size(136, 63);
            this.DrawBtn.TabIndex = 1;
            this.DrawBtn.Text = "Начать";
            this.DrawBtn.UseVisualStyleBackColor = true;
            this.DrawBtn.Click += new System.EventHandler(this.DrawBtn_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ScrollHeight
            // 
            this.ScrollHeight.Location = new System.Drawing.Point(2, 12);
            this.ScrollHeight.Name = "ScrollHeight";
            this.ScrollHeight.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.ScrollHeight.Size = new System.Drawing.Size(45, 600);
            this.ScrollHeight.TabIndex = 2;
            this.ScrollHeight.Scroll += new System.EventHandler(this.ScrollHeight_Scroll);
            // 
            // ScrollWidth
            // 
            this.ScrollWidth.Location = new System.Drawing.Point(53, 618);
            this.ScrollWidth.Name = "ScrollWidth";
            this.ScrollWidth.Size = new System.Drawing.Size(600, 45);
            this.ScrollWidth.TabIndex = 3;
            this.ScrollWidth.Scroll += new System.EventHandler(this.ScrollWidth_Scroll);
            // 
            // RadioMagnet1
            // 
            this.RadioMagnet1.AutoSize = true;
            this.RadioMagnet1.Checked = true;
            this.RadioMagnet1.Location = new System.Drawing.Point(6, 19);
            this.RadioMagnet1.Name = "RadioMagnet1";
            this.RadioMagnet1.Size = new System.Drawing.Size(71, 17);
            this.RadioMagnet1.TabIndex = 4;
            this.RadioMagnet1.TabStop = true;
            this.RadioMagnet1.Text = "Магнит 1";
            this.RadioMagnet1.UseVisualStyleBackColor = true;
            this.RadioMagnet1.CheckedChanged += new System.EventHandler(this.RadioMagnet1_CheckedChanged);
            // 
            // RadioMagnet2
            // 
            this.RadioMagnet2.AutoSize = true;
            this.RadioMagnet2.Location = new System.Drawing.Point(6, 73);
            this.RadioMagnet2.Name = "RadioMagnet2";
            this.RadioMagnet2.Size = new System.Drawing.Size(71, 17);
            this.RadioMagnet2.TabIndex = 5;
            this.RadioMagnet2.Text = "Магнит 2";
            this.RadioMagnet2.UseVisualStyleBackColor = true;
            this.RadioMagnet2.CheckedChanged += new System.EventHandler(this.RadioMagnet2_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.RadioMagnet2);
            this.groupBox1.Controls.Add(this.Height2Box);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.Width2Box);
            this.groupBox1.Controls.Add(this.RadioMagnet1);
            this.groupBox1.Controls.Add(this.Height1Box);
            this.groupBox1.Controls.Add(this.Width1Box);
            this.groupBox1.Location = new System.Drawing.Point(710, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(205, 127);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Выберите магнит";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(96, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Высота 2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(96, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Высота 1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(96, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Ширина 2";
            // 
            // Height2Box
            // 
            this.Height2Box.Location = new System.Drawing.Point(157, 98);
            this.Height2Box.Name = "Height2Box";
            this.Height2Box.Size = new System.Drawing.Size(36, 20);
            this.Height2Box.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(96, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Ширина 1";
            // 
            // Width2Box
            // 
            this.Width2Box.Location = new System.Drawing.Point(157, 73);
            this.Width2Box.Name = "Width2Box";
            this.Width2Box.Size = new System.Drawing.Size(36, 20);
            this.Width2Box.TabIndex = 17;
            // 
            // Height1Box
            // 
            this.Height1Box.Location = new System.Drawing.Point(157, 46);
            this.Height1Box.Name = "Height1Box";
            this.Height1Box.Size = new System.Drawing.Size(36, 20);
            this.Height1Box.TabIndex = 14;
            // 
            // Width1Box
            // 
            this.Width1Box.Location = new System.Drawing.Point(157, 21);
            this.Width1Box.Name = "Width1Box";
            this.Width1Box.Size = new System.Drawing.Size(36, 20);
            this.Width1Box.TabIndex = 13;
            // 
            // ScrollAngle
            // 
            this.ScrollAngle.Location = new System.Drawing.Point(659, 12);
            this.ScrollAngle.Name = "ScrollAngle";
            this.ScrollAngle.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.ScrollAngle.Size = new System.Drawing.Size(45, 600);
            this.ScrollAngle.TabIndex = 7;
            this.ScrollAngle.Scroll += new System.EventHandler(this.ScrollAngle_Scroll);
            // 
            // StepXBox
            // 
            this.StepXBox.Location = new System.Drawing.Point(62, 19);
            this.StepXBox.Name = "StepXBox";
            this.StepXBox.Size = new System.Drawing.Size(36, 20);
            this.StepXBox.TabIndex = 8;
            // 
            // StepYBox
            // 
            this.StepYBox.Location = new System.Drawing.Point(160, 18);
            this.StepYBox.Name = "StepYBox";
            this.StepYBox.Size = new System.Drawing.Size(37, 20);
            this.StepYBox.TabIndex = 9;
            // 
            // UpdateBtn
            // 
            this.UpdateBtn.Location = new System.Drawing.Point(829, 196);
            this.UpdateBtn.Name = "UpdateBtn";
            this.UpdateBtn.Size = new System.Drawing.Size(86, 37);
            this.UpdateBtn.TabIndex = 10;
            this.UpdateBtn.Text = "Update";
            this.UpdateBtn.UseVisualStyleBackColor = true;
            this.UpdateBtn.Click += new System.EventHandler(this.UpdateBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Шаг по x";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(104, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Шаг по y";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.StepYBox);
            this.groupBox2.Controls.Add(this.StepXBox);
            this.groupBox2.Location = new System.Drawing.Point(710, 145);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(205, 45);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Дискретность сетки";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioPotential);
            this.groupBox3.Controls.Add(this.radioTriangle);
            this.groupBox3.Location = new System.Drawing.Point(952, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(163, 104);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Тип отображаемого содержимого";
            // 
            // radioTriangle
            // 
            this.radioTriangle.AutoSize = true;
            this.radioTriangle.Checked = true;
            this.radioTriangle.Location = new System.Drawing.Point(16, 35);
            this.radioTriangle.Name = "radioTriangle";
            this.radioTriangle.Size = new System.Drawing.Size(96, 17);
            this.radioTriangle.TabIndex = 0;
            this.radioTriangle.TabStop = true;
            this.radioTriangle.Text = "Триангуляция";
            this.radioTriangle.UseVisualStyleBackColor = true;
            this.radioTriangle.CheckedChanged += new System.EventHandler(this.radioTriangle_CheckedChanged);
            // 
            // radioPotential
            // 
            this.radioPotential.AutoSize = true;
            this.radioPotential.Location = new System.Drawing.Point(16, 62);
            this.radioPotential.Name = "radioPotential";
            this.radioPotential.Size = new System.Drawing.Size(80, 17);
            this.radioPotential.TabIndex = 1;
            this.radioPotential.Text = "Потенциал";
            this.radioPotential.UseVisualStyleBackColor = true;
            this.radioPotential.CheckedChanged += new System.EventHandler(this.radioPotential_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1127, 664);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.UpdateBtn);
            this.Controls.Add(this.ScrollAngle);
            this.Controls.Add(this.ScrollWidth);
            this.Controls.Add(this.ScrollHeight);
            this.Controls.Add(this.DrawBtn);
            this.Controls.Add(this.GraphicsBox);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "Триангуляция Делоне";
            ((System.ComponentModel.ISupportInitialize)(this.GraphicsBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScrollHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScrollWidth)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScrollAngle)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox GraphicsBox;
        private System.Windows.Forms.Button DrawBtn;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TrackBar ScrollHeight;
        private System.Windows.Forms.TrackBar ScrollWidth;
        private System.Windows.Forms.RadioButton RadioMagnet1;
        private System.Windows.Forms.RadioButton RadioMagnet2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar ScrollAngle;
        private System.Windows.Forms.TextBox StepXBox;
        private System.Windows.Forms.TextBox StepYBox;
        private System.Windows.Forms.Button UpdateBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Height1Box;
        private System.Windows.Forms.TextBox Width1Box;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Height2Box;
        private System.Windows.Forms.TextBox Width2Box;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioPotential;
        private System.Windows.Forms.RadioButton radioTriangle;
    }
}

