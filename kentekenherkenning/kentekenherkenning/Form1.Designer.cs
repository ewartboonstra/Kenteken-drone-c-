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
            this.SuspendLayout();
            // 
            // Fotobutton
            // 
            this.Fotobutton.Location = new System.Drawing.Point(197, 199);
            this.Fotobutton.Name = "Fotobutton";
            this.Fotobutton.Size = new System.Drawing.Size(75, 23);
            this.Fotobutton.TabIndex = 0;
            this.Fotobutton.Text = "Haal foto op";
            this.Fotobutton.UseVisualStyleBackColor = true;
            this.Fotobutton.Click += new System.EventHandler(this.FotoButton_Click);
            this.Fotobutton.Paint += new System.Windows.Forms.PaintEventHandler(this.Fotobutton_Paint);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.Fotobutton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Fotobutton;
    }
}

