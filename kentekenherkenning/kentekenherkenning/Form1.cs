using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kentekenherkenning
{
    public partial class Form1 : Form
    {
        public Image image;
        public Form1()
        {
            InitializeComponent();

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if(image != null)
                e.Graphics.DrawImage(image, 0, 0, image.Width, image.Height);
        }
        //tijdelijke manier om foto's te uploaden
        private void FotoButton_Click(object sender, EventArgs e)
        {
            image = Image.FromFile(@FotoLinktextBox.Text, true);
            Invalidate();
        }
       
    }
}
