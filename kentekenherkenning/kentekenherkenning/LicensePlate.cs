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

namespace kentekenherkenning
{
    //class made by Julian
    public class LicensePlate
    {
        private const int AllowedDifference = 50;
        private Country _country;

        private List<FoundCharacter> characters = new List<FoundCharacter>();
        public string Text { get; set; }= "";

        public IImage BasePicture {get; private set;}

        public LicensePlate(Country country, IImage basePicture)
        {
            _country = country;
            BasePicture = basePicture;
        }

        public void Add(FoundCharacter foundCharacter)
        {
            characters.Add(foundCharacter);
            UpdateText();
        }

        public bool IsValid() 
        {
            //compare amount of characters to country standard
            if (characters.Count != _country.Characters)
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
