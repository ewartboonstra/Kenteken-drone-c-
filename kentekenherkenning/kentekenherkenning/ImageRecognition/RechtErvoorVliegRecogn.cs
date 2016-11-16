using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kentekenherkenning.ImageRecognition
{
    public class RechtErvoorVliegRecogn
    {

        private const int AllowedColorDistance = 10;

        //assumes the image is filled with the licence plate
        public static Bitmap[] giveCharacterCuttings(Bitmap BaseBitmap)
        {
            var pivotColor = Color.Yellow; //background of license plate


            //check first occurence of a yellow area of 3x3 pixels
            var first3X3Area = new Point(0,0);


            for (var x = 0; x < BaseBitmap.Width; x++)
            {
                var found = false;

                for (var y = 0; y < BaseBitmap.Height; y++)
                {
                    var colorDistance = getDistanceBetweenColors(BaseBitmap.GetPixel(x, y), pivotColor);

                    if (colorDistance < AllowedColorDistance && is3x3Area(x, y, BaseBitmap))
                    {
                        first3X3Area.X = x;
                        first3X3Area.Y = y;
                        found = true;
                        break;
                    };
                    
                    
                }

                if (found)
                {
                    break;
                }
            }

            //from that 3x3 area, scan down for the first time to check when to reach te border (y-length license plate)
            var height_LicensePlate = 0;
            for (var y = first3X3Area.Y + 1; y < BaseBitmap.Height; y++)
            {
                var colorDistance = getDistanceBetweenColors(BaseBitmap.GetPixel(first3X3Area.X, y), pivotColor);
                
                if (colorDistance > AllowedColorDistance)
                {
                    height_LicensePlate = y - first3X3Area.Y;
                }
            }

            //since we have retrieved the height of the license plate, now we continue by scanning the next column


        }

        //check whether the selected pixel is in the upper-left corner of a 3x3 area of nearly the same color
        private static bool is3x3Area(int x, int y, Bitmap BaseBitmap)
        {
            var pivotColor = (Color) BaseBitmap.GetPixel(x,y);

            var rightPixelHas = getDistanceBetweenColors(pivotColor, (Color) BaseBitmap.GetPixel(x + 1, y)) <= AllowedColorDistance;
            var pixelDownHas = getDistanceBetweenColors(pivotColor, (Color) BaseBitmap.GetPixel(x, y + 1)) <= AllowedColorDistance;
            var thirdPixelHas = getDistanceBetweenColors(pivotColor, (Color) BaseBitmap.GetPixel(x + 1, y + 1)) <= AllowedColorDistance;

            return (rightPixelHas && pixelDownHas && thirdPixelHas);
        }

        private static int getDistanceBetweenColors(Color a, Color b)
        {
            var aR = a.R;
            var aG = a.G;
            var aB = a.B;

            return (aR + aG + aB) / 3;
        }
    }
}
