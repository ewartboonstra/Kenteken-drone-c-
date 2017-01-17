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
    public partial class Map : Form
    {
        private Dictionary<string, Point> licensePlatePoints; //via coördinates
        
        public Map(ShowLicensePlates licensePlateform)
        {
            InitializeComponent();

            var LicensePlates = licensePlateform.LicensePlates;

            //determine scale
            foreach (var licensePlate in LicensePlates)
            {
                                                                                                                 
            }
        }

        

        //bepaal de schaal

        //breng in kaart waar op de kaart het moet staan

        //teken het

        //zorg ervoor dat het klikken erop werkt
                                                    
    }
}