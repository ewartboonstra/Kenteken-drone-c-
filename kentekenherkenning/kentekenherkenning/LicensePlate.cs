using kentekenherkenning.MoreLinq;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Emgu.CV;
using Emgu.CV.Structure;
using Newtonsoft.Json;

namespace kentekenherkenning
{
    public class LicensePlate
    {
        private const int AllowedDifference = 50;
        public Country Country { get; set; }

        private List<FoundCharacter> characters = new List<FoundCharacter>();
        public string Text { get; set; }= "";

        public Image<Bgr, Byte> Image {get; private set;}
        public string Gps { get; set; }
        public string TimeStamp { get; set; }


        public LicensePlate(Image<Bgr, Byte> basePicture)
            : this("","",basePicture)
        {
        }
        public LicensePlate(string gps, string timestamp, Image<Bgr, Byte> image)
        {
            Gps = gps;
            TimeStamp = timestamp;
            Image = image;
        }

        public void Add(FoundCharacter foundCharacter)
        {
            characters.Add(foundCharacter);
            UpdateText();
        }

        public bool IsValid() 
        {
            //compare amount of characters to country standard
            if (characters.Count != Country.Characters)
                return false;


            //check difference in letterheight
            var maxHeight = characters.MaxBy(x => x.Height).Height;
            var minHeight = characters.MinBy(x => x.Height).Height;
            return (double) minHeight/maxHeight*100 >= AllowedDifference;
        }

        public void Sort()
        {
            InsertionSort();
        }

       
        private void InsertionSort()
        {
            for (int I = 1; I < characters.Count; I++)
            {
                var newElement = characters[I];
                while ((I - 1 >= 0) && (newElement.Point.X < characters[I - 1].Point.X))
                {
                    characters[I] = characters[I - 1];
                    I--;
                }
                characters[I] = newElement;
            }

            UpdateText();
        }

        private void UpdateText()
        {
            Text = "";
            foreach (var ch in characters)
            {
                Text += ch.Text;
            }
        }
    }
}
