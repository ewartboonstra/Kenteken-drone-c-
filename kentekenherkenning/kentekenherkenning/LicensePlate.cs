using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kentekenherkenning
{
    //class made by Julian
    public class LicensePlate
    {
        private List<FoundCharacter> _characterPlaces = new List<FoundCharacter>();
        public string Text = "";

        public void Add(FoundCharacter foundCharacter)
        {
            _characterPlaces.Add(foundCharacter);
        }


        public void Sort()
        {
            InsertionSort();
        }

        public void InsertionSort()
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

            updateText();
        }

        private void updateText()
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

        public FoundCharacter(Point point, string text)
        {
            Point = point;
            Text = text;
        }
    }
}
