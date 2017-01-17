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
        private ShowLicensePlates licensePlateform;
        private Dictionary<string, DoublePoint> licensePlatePoints = new Dictionary<string, DoublePoint>(); //via coördinates
        // highest and lowest coordinates for X and Y
        private double highestX;
        private double lowestX;
        private double highestY;
        private double lowestY;
        private double Xside;
        private double Yside;
        //dummydata voor GPS...
        private string[] gpscoordinates = new string[] { "N 50.91721° E 5.91775°", "N 50.91981° E 5.91799°", "N 50.91744° E 5.91735°" };
        
        
        public Map(ShowLicensePlates licensePlateform)
        {
            this.licensePlateform = licensePlateform;
            InitializeComponent();

            update();
        }

        /// <summary>
        /// Deze methode wordt aangeroepen om de kaart te vernieuwen.
        /// </summary>
        public void update()
        {
            refresh();
            determineScale();
            Console.WriteLine("X highest: {0}, Y highest: {1}, X Lowest: {2}, Y lowest: {3}, Xside: {4}, Yside {5}"
                , highestX, highestY, lowestX, lowestY, Xside, Yside);
        }

        private void refresh()
        {
            var LicensePlates = licensePlateform.LicensePlates;
            int counter = 0;
            foreach (var licensePlate in LicensePlates)
            {
                licensePlate.Gps = gpscoordinates[counter];
                counter++; //dummycoordinates! gooit exceptie als er niks meer in array staat onder counter...
                GpsPoint GpsPoint = new GpsPoint(licensePlate.Gps);
                licensePlatePoints.Add(licensePlate.Gps, GpsPoint.toPoint);
                System.Console.WriteLine(licensePlate.Gps + " " + GpsPoint.toPoint.X + " : " + GpsPoint.toPoint.Y);
            }           
        }

        private void determineScale()
        {
            foreach (KeyValuePair<string, DoublePoint> entry in licensePlatePoints)
            {
                highestX = (highestX == 0) ? entry.Value.X : (entry.Value.X > highestX) ? entry.Value.X : highestX;
                highestY = (highestY == 0) ? entry.Value.Y : (entry.Value.Y > highestY) ? entry.Value.Y : highestY;
                lowestX = (lowestX == 0) ? entry.Value.X : (entry.Value.X < lowestX) ? entry.Value.X : lowestX;
                lowestY = (lowestY == 0) ? entry.Value.Y : (entry.Value.Y < lowestY) ? entry.Value.Y : lowestY;
            }
            Xside = (highestX - lowestX);
            Yside = (highestY - lowestY);
        }

        

        //bepaal de schaal

        //breng in kaart waar op de kaart het moet staan

        //teken het

        //zorg ervoor dat het klikken erop werkt
                                                    
    }
}