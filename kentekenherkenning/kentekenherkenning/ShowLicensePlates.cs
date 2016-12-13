using System;
using System.CodeDom;
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
        public List<LicensePlate> LicensePlates { get; set; }= new List<LicensePlate>();


        public ShowLicensePlates()
        {
            InitializeComponent();
        }


        public bool IsUnique(LicensePlate licensePlate)
        {
            return LicensePlates.All(plate => plate.Text != licensePlate.Text);
        }

        public void AddLicensePlate(LicensePlate licensePlate)
        {
           LicensePlates.Add(licensePlate);
            //visual list
            VisualList.Items.Add(licensePlate.Text);
        }

        private void SelectAllBtn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < VisualList.Items.Count; i++)
            {
                 VisualList.SetItemChecked(i, true);
            }
               
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            
               for (var i = 0; i < VisualList.CheckedItems.Count;)
                {
                    var checkedItem = VisualList.CheckedItems[i];
                    var text = (string) checkedItem;

                    

                    LicensePlates.RemoveAll(lp => lp.Text == text);


                    VisualList.Items.Remove(checkedItem);
                } 
            
                
          }
    }
}
