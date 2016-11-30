using kentekenherkenning.MoreLinq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace kentekenherkenning
{
    //class made by Julian
    public class LicensePlate
    {
        private const int CharactersInLicensePlate = 6; //the characters have to be six (as that is a requirement for recognizing a dutch licence plate)
        private const int AllowedDifferenceInAreasPercentage = 50;


        private List<FoundCharacter> _characterPlaces = new List<FoundCharacter>();
        public string Text = "";

        public void Add(FoundCharacter foundCharacter)
        {
            _characterPlaces.Add(foundCharacter);
            UpdateText();
        }

        /// <summary>
        /// Check if licenceplate is valid licenceplate
        /// </summary>
        /// <returns>valid checks</returns>
        public bool IsValid() 
        {
            if (_characterPlaces.Count != CharactersInLicensePlate)
                return false;

            //if the difference in areas is more than 50%, then return false
            var maxArea = _characterPlaces.MaxBy(x => x.Height).Height;
            var minArea = _characterPlaces.MinBy(x => x.Height).Height;
            return (double) minArea/maxArea*100 >= AllowedDifferenceInAreasPercentage;
        }

        public void Sort()
        {
            InsertionSort();
        }

        private void InsertionSort()
        {
            
            for (int I = 1; I < _characterPlaces.Count; I++)
            {
                var newElement = _characterPlaces[I];
                while ((I - 1 >= 0) && (newElement.Point.X < _characterPlaces[I - 1].Point.X))
                {
                    _characterPlaces[I] = _characterPlaces[I - 1];
                    I--;
                }
                _characterPlaces[I] = newElement;
            }

            UpdateText();
        }

        private void UpdateText()
        {
            Text = "";
            foreach (var ch in _characterPlaces)
            {
                Text += ch.Text;
            }
        }

    }
    
    //binding of point and text in one struct
    public struct FoundCharacter
    {
        public Point Point;
        public string Text;
        public int Height;

        public FoundCharacter(Point point, string text, int height)
        {
            Point = point;
            Text = text;
            Height = height;
        }
    }
}
