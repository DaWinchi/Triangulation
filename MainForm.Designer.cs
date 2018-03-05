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
            ((System.ComponentModel.ISupportInitialize)(this.GraphicsBox)).BeginInit();
            this.SuspendLayout();
            // 
            // GraphicsBox
            // 
            this.GraphicsBox.Location = new System.Drawing.Point(24, 14);
            this.GraphicsBox.Name = "GraphicsBox";
            this.GraphicsBox.Size = new System.Drawing.Size(600, 600);
            this.GraphicsBox.TabIndex = 0;
            this.GraphicsBox.TabStop = false;
            // 
            // DrawBtn
            // 
            this.DrawBtn.Location = new System.Drawing.Point(809, 419);
            this.DrawBtn.Name = "DrawBtn";
            this.DrawBtn.Size = new System.Drawing.Size(136, 63);
            this.DrawBtn.TabIndex = 1;
            this.DrawBtn.Text = "button1";
            this.DrawBtn.UseVisualStyleBackColor = true;
            this.DrawBtn.Click += new System.EventHandler(this.DrawBtn_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 633);
            this.Controls.Add(this.DrawBtn);
            this.Controls.Add(this.GraphicsBox);
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.GraphicsBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox GraphicsBox;
        private System.Windows.Forms.Button DrawBtn;
        private System.Windows.Forms.Timer timer1;
    }
}

