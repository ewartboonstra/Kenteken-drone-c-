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
using Emgu.CV;

namespace kentekenherkenning
{
    //form made by Julian
    public partial class ShowLicensePlates : Form
    {
        public List<LicensePlate> LicensePlates { get; set; }= new List<LicensePlate>();
        private MainForm Main;

        public ShowLicensePlates()
        {
            InitializeComponent();

            Main = new MainForm(this);
            Main.Show();

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


        private void ShowLicensePlates_FormClosing(object sender, FormClosingEventArgs e)
        {
            Main.Visible = false;
            Visible = false;
            Environment.Exit(0);
        }

        private void Write(string description, string data)
        {
            InformationLabel.Text += description + ": " + data + "\r\n";
        }

        //show image of selected item.
        private void VisualList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedItem = (string) VisualList.SelectedItems[0];
                LicensePlate designatedLicensePlate = LicensePlates.Find(lp => lp.Text == selectedItem);
                IImage pictureToShow = designatedLicensePlate.Image;
                PlatePictureBox.Image = pictureToShow;
                Write("Time", designatedLicensePlate.TimeStamp);
                Write("Gps", designatedLicensePlate.Gps);
            }
            catch (IndexOutOfRangeException)
            {
                PlatePictureBox.Image = null;
            }
        }

        private void Advanced_Click(object sender, EventArgs e)
        {
            if (Main.IsDisposed)
            {
                Main = new MainForm(this);
            }
            Main.Visible = true;
        }
    }
}
