using System;
using System.Collections.Generic;

using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kentekenherkenning
{

    /// <summary>
    /// Input: string in constructor
    /// Output: doubles in 'toPoint';
    /// </summary>
    public class GpsPoint 
    {
        private string text;
        private DoublePoint toPoint;

        public GpsPoint(string Gps) 
        {
            this.text = Gps;

        }


        //format example: N 50.91721° E 5.91775°
        private DoublePoint ToGPSPoint(string gpsString) //string to real point
        {
            
            var coordinates = new string[2];

            gpsString = gpsString.Trim();
            var latitudeIndex = gpsString.IndexOf('N');
            if (latitudeIndex == -1)
            {
                latitudeIndex = gpsString.IndexOf('Z');
            }

            var longitudeIndex = gpsString.IndexOf('E');
            if (longitudeIndex == -1)
            {
                longitudeIndex = gpsString.IndexOf('W');
            }

            //get the number after the 'N' and 'E' (or 'S' and 'W')
            for (var i = latitudeIndex; i < gpsString.Length;i++)
            {
                if (char.IsNumber(gpsString[i]))
                {
                    coordinates[0] += gpsString[i];
                }
                else
                {
                    if (gpsString[i] != '.')
                    {
                        break;
                    }
                }
            }

            for (var i = longitudeIndex; i <= gpsString.Length; i++)
            {
                if (char.IsNumber(gpsString[i]))
                {
                    coordinates[1] += gpsString[i];
                }
                else
                {
                    if (gpsString[i] != '.')
                    {
                        break;
                    }
                }
            }


            



            return new DoublePoint(double.Parse(coordinates[0]), double.Parse(coordinates[1]));
        }


         
    }
}

