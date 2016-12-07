using System.Drawing;

namespace kentekenherkenning
{
    //binding of point and text in one struct
    public struct FoundCharacter
    {
        public Point Point { get; set; }
        public string Text { get; set; }
        public int Height { get; set; }

        public FoundCharacter(Point point, string text, int height)
        {
            Point = point;
            Text = text;
            Height = height;
        }
    }
}