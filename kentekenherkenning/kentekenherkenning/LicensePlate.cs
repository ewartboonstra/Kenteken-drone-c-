﻿using kentekenherkenning.MoreLinq;
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
        private const int AllowedDifferenceInAreasPercentage = 50;
        
        private Country _country;

        private List<FoundCharacter> _characterPlaces = new List<FoundCharacter>();
        public string Text = "";

        public LicensePlate(Country country)
        {
            _country = country;
        }
        public void Add(FoundCharacter foundCharacter)
        {
            _characterPlaces.Add(foundCharacter);
            UpdateText();
        }


        /// <summary>
        /// Check if licenceplate is valid licenceplate
        /// </summary>
        /// <returns>valid checks</returns>
        public bool IsValid() //sorts and checks
        {
            if (_characterPlaces.Count != _country.Characters)
                return false;

            //if the difference in heights is more than 50%, then return false
            var maxHeight = _characterPlaces.MaxBy(x => x.Height).Height;
            var minHeight = _characterPlaces.MinBy(x => x.Height).Height;
            return (double) minHeight/maxHeight*100 >= AllowedDifferenceInAreasPercentage;
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
}
