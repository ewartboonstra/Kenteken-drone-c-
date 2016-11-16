using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace kentekenherkenning
{
    public class Letterherkenning
    {
        private Dictionary<char,bool[,]>  BaseLetters = new Dictionary<char, bool[,]>();

        public Letterherkenning()
        {
            FillDictionary();
        }
        public char ReadChar(Bitmap source)
        {
            bool[,] charArray = WriteCharArray(source);

            return ' ';
        }

        private bool CompareBoolArray(bool[,] source)
        {

            return true;
        }

        /// <summary>
        /// turn the source bitmap into a chararray for comparison
        /// </summary>
        /// <param name="source">source bitmap</param>
        /// <returns>double bool array that shows blackness</returns>
        private bool[,] WriteCharArray(Bitmap source)
        {
            //empty array to store data in
            bool[,] character = new bool[5,8];

            //resize image to fixed size
            source = ResizeImage(source, 50, 80);

        
            //loop every section and fill boolArray
            for (int x = 0; x < 5; x++)
                for (int y = 0; y < 8; y++)
                {
                    Rectangle section = new Rectangle(new Point(x*10, y*10), new Size(10, 10));
                    character[x, y] = isBlack(CropImage(source, section));
                }

            return character;
        }

        /// <summary>
        /// check if blackness of bitmap reach trashhold
        /// </summary>
        /// <param name="source">source bitmap to check</param>
        /// <returns>bool that checks if the bitmap blackness is above trashhold</returns>
        private bool isBlack(Bitmap source)
        {
            int black = 0;

            //make in seperate section for configuring later.
            int trashHold = 70;

            for(int x = 0;x<source.Height;x++)
                for (int y = 0; y < source.Width; y++)
                    if (source.GetPixel(x, y) == Color.Black)
                        black++;

            int blackPercentage = (int)((double)black / source.Height*source.Width* 100);

            return blackPercentage > trashHold;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        /// <summary>
        /// crop the source image into a smaller image
        /// </summary>
        /// <param name="source">The source image</param>
        /// <param name="section">the section to crop to</param>
        /// <returns>cropped image</returns>
        private Bitmap CropImage(Bitmap source, Rectangle section)
        {
            // An empty bitmap which will hold the cropped image
            Bitmap bmp = new Bitmap(section.Width, section.Height);

            Graphics g = Graphics.FromImage(bmp);

            // Draw the given area (section) of the source image
            // at location 0,0 on the empty bitmap (bmp)
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);

            return bmp;
        }

        private void FillDictionary()
        {
            bool[,] b =
            {
                {true, true, true, false, false},
                {true, false, false, true, false},
                {true, false, false, true, true},
                {true, true, true, true, false},
                {true, true, true, true, true},
                {true, false, false, false, true},
                {true, false, false, true, true},
                {true, true, true, true, false}
            };
            BaseLetters.Add('b',b);
        }

    }
}