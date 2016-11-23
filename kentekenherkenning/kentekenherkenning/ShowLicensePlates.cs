using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kentekenherkenning
{
    //form made by Julian
    public partial class ShowLicensePlates : Form
    {
        private List<LicensePlate> _licensePlates = new List<LicensePlate>();


        public ShowLicensePlates()
        {
            InitializeComponent();
        }


        public void AddLicensePlate(LicensePlate lp)
        {
            //get (string representation of) last added license plate
            string lastAdded;
            try
            {
                lastAdded = _licensePlates[_licensePlates.Count - 1].Text;
            }
            catch (ArgumentOutOfRangeException)
            {
                lastAdded = "";
            }
                

            /*check: 
             * - if it is not the same as the previous added one
             * - if the length of the license plate is smaller than 10 characters
             * */
            if (lastAdded != lp.Text && lp.Text.Length < 10)
            {
                _licensePlates.Add(lp);

                //visual part
                VisualList.Items.Add(lp.Text);

                
            }
                
           
            

        }



        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
