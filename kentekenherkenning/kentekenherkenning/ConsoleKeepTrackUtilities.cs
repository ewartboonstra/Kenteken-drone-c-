using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kentekenherkenning
{
    public class ConsoleKeepTrackUtilities
    {
       
        private string previousWritten = "";

        public void WriteOnce(string input)
        {
            if (input == previousWritten) return;
            Console.WriteLine(input);
            previousWritten = input;
        }
    }
}
