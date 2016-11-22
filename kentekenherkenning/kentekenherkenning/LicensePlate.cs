using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kentekenherkenning
{
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

        public void InsertionSort(int[] Tabel)
        {
            int X;
            for (int I = 1; I < Tabel.Length; I++)
            {
                X = Tabel[I];
                while ((I - 1 >= 0) && (X < Tabel[I - 1]))
                {
                    Tabel[I] = Tabel[I - 1];
                    I--;
                }
                Tabel[I] = X;
            }
        }/*InsertionSort*/

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
