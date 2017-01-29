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
        public DoublePoint toPoint { get; private set; }

        public GpsPoint(string Gps) 
        {
            this.text = Gps;

            toPoint = ToGPSPoint(Gps);
        }


        //format example: N 50.91721° E 5.91775°
        private DoublePoint ToGPSPoint(string gpsString) //string to real point
        {
            
            var coordinates = new string[2];
            coordinates[0] = "";
            coordinates[1] = "";
            gpsString = gpsString.Replace(" ", ""); //remove all spaces from the Gps string
            
            var latitudeIndex = gpsString.IndexOf('N');
            if (latitudeIndex == -1)
            {
                latitudeIndex = gpsString.IndexOf('Z');
                if (latitudeIndex == -1)
                {
                    throw new ArgumentException("Invalid GPS string given");
                }
            }

            var longitudeIndex = gpsString.IndexOf('E');
            if (longitudeIndex == -1)
            {
                longitudeIndex = gpsString.IndexOf('W');
                if (longitudeIndex == -1)
                {
                    throw new ArgumentException("Invalid GPS string given");
                }
            }

            //get the number after the 'N' and 'E' (or 'S' and 'W')
            //any '.' or ',' will be replaced by ',' otherwise the parse to double method won't work.
            for (var i = latitudeIndex + 1; i < gpsString.Length;i++)
            {
                if (char.IsNumber(gpsString[i]))
                {
                    coordinates[0] += gpsString[i];
                }
                else
                {
                    if (gpsString[i] == '.' || gpsString[i] == ',')
                    {
                        coordinates[0] += ",";
                    }
                    else if(gpsString[i] != '.' || gpsString[i] == ' ')
                    {
                        break;
                    }
                }
            }

            for (var i = longitudeIndex + 1; i <= gpsString.Length; i++)
            {                
                if (char.IsNumber(gpsString[i]))
                {
                    coordinates[1] += gpsString[i];
                }
                else
                {
                    if(gpsString[i] == '.' || gpsString[i] == ',')
                    {
                        coordinates[1] += ",";
                    }
                    else if (gpsString[i] != '.' || gpsString[i] == ' ')
                    {
                        break;
                    }
                }
            }
            
            return new DoublePoint(double.Parse(coordinates[0]), double.Parse(coordinates[1]));
        }


         
    }
}

