using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace kentekenherkenning.ImageRecognition
{
    public class ImageRecognition
    {

        //returns the part suspected of being a numberplate
        public static Bitmap FindLicensePlate(Bitmap basePicture, Color backgroundColor, int maxWidthNumberplate)
        {
           throw new NotImplementedException();
        }

        //gives an image with the text in black
        public static Bitmap giveContrastedImage(Bitmap basePicture, Color backgroundPivot, Color textColorPivot)
        {
            //example: background = white, text = near fully black;
            


            
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
