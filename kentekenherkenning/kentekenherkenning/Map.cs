using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace kentekenherkenning
{
    public partial class Map : Form
    {

        private ShowLicensePlates licensePlateform;
        private Dictionary<string, DoublePoint> licensePlatePoints = new Dictionary<string, DoublePoint>(); //via coördinates
        // highest and lowest coordinates for X and Y
        private double highestXcoord;
        private double lowestXcoord;
        private double highestYcoord;
        private double lowestYcoord;
        //dummydata voor GPS...
        private string[] gpscoordinates = new string[] { "N 51.91721° E 5.91775°", "N 51.91981° E 5.91799°", "N 49.91744° E 5.91735°", "N 48.91744° E 5.91735°" };
               
        public Map(ShowLicensePlates licensePlateform)
        {
            this.licensePlateform = licensePlateform;
            //update();
            InitializeComponent();
           

        }
       
        /// <summary>
        /// Deze methode wordt aangeroepen om de kaart te genereren.
        /// </summary>
        public void update()
        {
            string oldDir = Environment.CurrentDirectory;
            //template file is located 3 directories up from the debug folder this code will run in, \kentekenherkenning\bin\x86\Debug.
            //save old directory to set back as working directory after this method is done.
            Directory.SetCurrentDirectory(@"..\..\..\");
            string currentDir = Environment.CurrentDirectory;
            refresh();
            determineScale();
            //generate HTML document to show interactive map
            BuildJsonForMap BuildJsonForMap = new BuildJsonForMap();
            GenerateHTML GenerateHTML = new GenerateHTML(currentDir + "\\map.html");
            string ycoord = Math.Round((highestYcoord + lowestYcoord) / 2, 1).ToString().Replace(',', '.');
            string xcoord = Math.Round((highestXcoord + lowestXcoord) / 2, 1).ToString().Replace(',', '.');
            string HTMLpageOutput = GenerateHTML.Render(new
            {
                CENTER = ycoord + ", " + xcoord,
                JSONDATA = BuildJsonForMap.createJsonString(licensePlatePoints)
            });
            System.IO.File.WriteAllText(currentDir + "\\interactivemap.html", HTMLpageOutput);
            //start standaardbrowser met de htmlpagina die niet gegenereerd is.
            System.Diagnostics.Process.Start(currentDir + "\\interactivemap.html");
            Directory.SetCurrentDirectory(oldDir);

        }

        /// <summary>
        /// Voegt alle kentekens aan een dictionary toe.
        /// </summary>
        private void refresh()
        {
            var LicensePlates = licensePlateform.LicensePlates;
            int counter = 0;
            foreach (var licensePlate in LicensePlates)
            {
                licensePlate.Gps = gpscoordinates[counter];
                counter++; //dummycoordinates! gooit exceptie als er niks meer in array staat onder counter...
                GpsPoint GpsPoint = new GpsPoint(licensePlate.Gps);
                licensePlatePoints.Add(licensePlate.Text + " - " + licensePlate.Gps, GpsPoint.toPoint);                
            }           
        }

        /// <summary>
        /// selecteer uit de dictionary alle uiterste coordinaten, voor het genereren van een map.
        /// </summary>
        private void determineScale()
        {
            foreach (KeyValuePair<string, DoublePoint> entry in licensePlatePoints)
            {
                highestXcoord = (highestXcoord == 0) ? entry.Value.X : (entry.Value.X > highestXcoord) ? entry.Value.X : highestXcoord;
                highestYcoord = (highestYcoord == 0) ? entry.Value.Y : (entry.Value.Y > highestYcoord) ? entry.Value.Y : highestYcoord;
                lowestXcoord = (lowestXcoord == 0) ? entry.Value.X : (entry.Value.X < lowestXcoord) ? entry.Value.X : lowestXcoord;
                lowestYcoord = (lowestYcoord == 0) ? entry.Value.Y : (entry.Value.Y < lowestYcoord) ? entry.Value.Y : lowestYcoord;
            }            
        }                                                    
    }

    
}