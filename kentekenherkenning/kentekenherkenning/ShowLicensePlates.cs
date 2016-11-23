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
            
                //check if the license plate is added already
                var AlreadyAdded = _licensePlates.Find(recordLp => recordLp.Text == lp.Text) != null;


                /*check: 
                 * - if not already added
                 * - if the length of the license plate is smaller than 10 characters
                 * */
                if (!AlreadyAdded && lp.Text.Length < 10)
                {
                    _licensePlates.Add(lp);

                    //visual part
                    VisualList.Items.Add(lp.Text);


                }
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

                    

                    _licensePlates.RemoveAll(lp => lp.Text == text);


                    VisualList.Items.Remove(checkedItem);
                } 
            
                
          }

       
        
    }
}
