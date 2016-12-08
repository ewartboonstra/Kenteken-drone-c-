using System;

namespace kentekenherkenning
{
    public class LicensePlateException : Exception
    {
        public LicensePlateException(string message)
            :base(message)
        {
            
        }

    }
}