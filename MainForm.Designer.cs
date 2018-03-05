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
            this.ScrollAngle = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.GraphicsBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScrollHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScrollWidth)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScrollAngle)).BeginInit();
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
            this.RadioMagnet2.Location = new System.Drawing.Point(6, 42);
            this.RadioMagnet2.Name = "RadioMagnet2";
            this.RadioMagnet2.Size = new System.Drawing.Size(71, 17);
            this.RadioMagnet2.TabIndex = 5;
            this.RadioMagnet2.Text = "Магнит 2";
            this.RadioMagnet2.UseVisualStyleBackColor = true;
            this.RadioMagnet2.CheckedChanged += new System.EventHandler(this.RadioMagnet2_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RadioMagnet2);
            this.groupBox1.Controls.Add(this.RadioMagnet1);
            this.groupBox1.Location = new System.Drawing.Point(710, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(110, 68);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Выберите магнит";
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 664);
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
    }
}

