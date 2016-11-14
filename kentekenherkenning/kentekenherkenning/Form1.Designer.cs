namespace kentekenherkenning
{
    partial class Form1
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
            this.Fotobutton = new System.Windows.Forms.Button();
            this.FotoLinktextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Fotobutton
            // 
            this.Fotobutton.Location = new System.Drawing.Point(197, 199);
            this.Fotobutton.Name = "Fotobutton";
            this.Fotobutton.Size = new System.Drawing.Size(75, 23);
            this.Fotobutton.TabIndex = 0;
            this.Fotobutton.Text = "Sla op";
            this.Fotobutton.UseVisualStyleBackColor = true;
            this.Fotobutton.Click += new System.EventHandler(this.FotoButton_Click);
            // 
            // FotoLinktextBox
            // 
            this.FotoLinktextBox.Location = new System.Drawing.Point(13, 228);
            this.FotoLinktextBox.Name = "FotoLinktextBox";
            this.FotoLinktextBox.Size = new System.Drawing.Size(259, 20);
            this.FotoLinktextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 209);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "foto link";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FotoLinktextBox);
            this.Controls.Add(this.Fotobutton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Fotobutton;
        private System.Windows.Forms.TextBox FotoLinktextBox;
        private System.Windows.Forms.Label label1;
    }
}

