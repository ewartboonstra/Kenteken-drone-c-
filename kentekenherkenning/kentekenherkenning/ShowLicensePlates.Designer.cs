namespace kentekenherkenning
{
    partial class ShowLicensePlates
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
            this.VisualList = new System.Windows.Forms.CheckedListBox();
            this.SelectAllBtn = new System.Windows.Forms.Button();
            this.DeleteBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // VisualList
            // 
            this.VisualList.FormattingEnabled = true;
            this.VisualList.Location = new System.Drawing.Point(13, 26);
            this.VisualList.Name = "VisualList";
            this.VisualList.Size = new System.Drawing.Size(248, 154);
            this.VisualList.TabIndex = 0;
            // 
            // SelectAllBtn
            // 
            this.SelectAllBtn.Location = new System.Drawing.Point(13, 200);
            this.SelectAllBtn.Name = "SelectAllBtn";
            this.SelectAllBtn.Size = new System.Drawing.Size(75, 23);
            this.SelectAllBtn.TabIndex = 1;
            this.SelectAllBtn.Text = "Select all";
            this.SelectAllBtn.UseVisualStyleBackColor = true;
            this.SelectAllBtn.Click += new System.EventHandler(this.SelectAllBtn_Click);
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Location = new System.Drawing.Point(95, 199);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(75, 23);
            this.DeleteBtn.TabIndex = 2;
            this.DeleteBtn.Text = "Delete";
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // ShowLicensePlates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.DeleteBtn);
            this.Controls.Add(this.SelectAllBtn);
            this.Controls.Add(this.VisualList);
            this.Name = "ShowLicensePlates";
            this.Text = "Discovered License Plates";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShowLicensePlates_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox VisualList;
        private System.Windows.Forms.Button SelectAllBtn;
        private System.Windows.Forms.Button DeleteBtn;
    }
}