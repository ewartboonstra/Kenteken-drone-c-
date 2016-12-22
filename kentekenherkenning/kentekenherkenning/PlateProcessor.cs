using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using ContourAnalysisNS;
using Emgu.CV;
using Emgu.CV.Structure;

namespace kentekenherkenning
{
    public class PlateProcessor
    {
        private LicensePlate CurrentPlate;

        /// <summary>
        /// current image with modified view.
        /// </summary>
        public IImage CurrentImage { get; set; }
        private ImageProcessor Processor;
        public delegate void ChangedEventHandler();
        public event ChangedEventHandler Changed;

        Dictionary<string, Image> AugmentedRealityImages = new Dictionary<string, Image>();

        private List<Country> CountryList;

        bool showAngle;

        private ShowLicensePlates _licensePlateForm;
        private string templateFile;
        private Thread ServerThread;

        //Paint-method variables
        private Font font = new Font(FontFamily.GenericSansSerif, 24);//16
        Brush bgBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
        Brush foreBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
        Pen borderPen = new Pen(Color.FromArgb(150, 0, 255, 0));

        public PlateProcessor(ImageProcessor processor, ShowLicensePlates licensePlateForm)
        {
            _licensePlateForm = licensePlateForm;
            Processor = processor;
            InitializeCountries();

            //start server
            ServerThread = new Thread(new ThreadStart(RunConnectionServer));
            ServerThread.Start();
        }
        /// <summary>
        /// load a list of default countries in the list
        /// </summary>
        public void InitializeCountries()
        {

            CountryList = new List<Country>();

            Country netherlands = new Country("Nederland", 6, "Nederlands");
            CountryList.Add(netherlands);
        }

        /// <summary>
        /// Start connection to server on a new thread
        /// </summary>
        private void RunConnectionServer()
        {
            ServerConnection s = new ServerConnection();
            s.SetConnection();
            while (true)
            {
                LicensePlate plate = s.WaitForImage();
//                Invoke(new Action(() => CurrentPlate = plate));
//                Invoke(new Action(ProcessFrame));

            }
        }

        private void LoadTemplates(string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                    Processor.templates = (Templates)new BinaryFormatter().Deserialize(fs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveTemplates(string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                    new BinaryFormatter().Serialize(fs, Processor.templates);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// process most recent image saved
        /// </summary>
        public void ProcessFrame()
        {
            foreach (Country country in CountryList)
            {
                try
                {

                    //change current country to check
                    CurrentPlate.Country = country;
                    LoadTemplates(country.TemplateLocation);

                    //process image
                    Processor.ProcessImage(CurrentPlate.Image);

                    if (Processor.showBinarized)
                        CurrentImage = Processor.binarizedFrame;
                    else
                        CurrentImage = CurrentPlate.Image;

                    //invoke to change settings
                    Changed?.Invoke();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                LicensePlate licensePlate = ProcessLicensePlate(CurrentPlate);

                //add the license plate to the list
                if (!licensePlate.IsValid())
                {
                    Console.WriteLine($"LicencePlate: {licensePlate.Text}, {country.Name} rejected.");
                    Console.WriteLine("Reason: Licenseplate is not valid");
                    continue;
                }

                licensePlate.Sort();

                if (!_licensePlateForm.IsUnique(licensePlate))
                {
                    Console.WriteLine($"LicencePlate: {licensePlate.Text}, {country.Name} rejected.");
                    Console.WriteLine("Reason: Licenseplate is not unique in list");
                    continue;
                }

                _licensePlateForm.AddLicensePlate(licensePlate);
                Console.WriteLine($"LicencePlate: {licensePlate.Text}, {country.Name} accepted .");

                Changed?.Invoke();
                break;

            }
        }
        /// <summary>
        /// draw contour box around each character
        /// </summary>
        /// <param name="g">graphics class</param>
        public void DrawContours(Graphics g)
        {
            foreach (Contour<Point> contour in Processor.contours)
                if (contour.Total > 1)
                    g.DrawLines(Pens.Red, contour.ToArray());
        }

        /// <summary>
        /// Draw box around each character
        /// </summary>
        /// <param name="g">Graphics class</param>
        public void DrawBox(Graphics g)
        {
            foreach (FoundTemplateDesc found in Processor.foundTemplates)
            {
                if (found.template.name.EndsWith(".png") || found.template.name.EndsWith(".jpg"))
                {
                    DrawAugmentedReality(found, g);
                    continue;
                }
                Rectangle foundRect = found.sample.contour.SourceBoundingRect;
                string text = found.template.name;
                Point p1 = new Point((foundRect.Left + foundRect.Right) / 2, foundRect.Top);

                if (showAngle)
                    text += $"\r\nangle={180 * found.angle / Math.PI:000}°\r\nscale={found.scale:0.0}";

                g.DrawRectangle(borderPen, foundRect);
                //
                    g.DrawString(text, font, bgBrush, new PointF(p1.X + 1 - font.Height/3, p1.Y + 1 - font.Height));
                    g.DrawString(text, font, foreBrush, new PointF(p1.X - font.Height/3, p1.Y - font.Height));

            }
        }

        /// <summary>
        /// check licenseplate for characters
        /// </summary>
        /// <param name="plate"></param>
        /// <returns> Licenceplate found</returns>
        private LicensePlate ProcessLicensePlate(LicensePlate plate)
        {
            lock (Processor.foundTemplates)
                foreach (FoundTemplateDesc found in Processor.foundTemplates)
                {
                    Rectangle foundRect = found.sample.contour.SourceBoundingRect;
                    Point p1 = new Point((foundRect.Left + foundRect.Right) / 2, foundRect.Top);

                    string text = found.template.name;
                    int height = foundRect.Height;

                    FoundCharacter foundCharacter = new FoundCharacter(p1, text, height);

                    plate.Add(foundCharacter);
                }
            return plate;
        }

        private void DrawAugmentedReality(FoundTemplateDesc found, Graphics gr)
        {
            string fileName = Path.GetDirectoryName(templateFile) + "\\" + found.template.name;
            if (!AugmentedRealityImages.ContainsKey(fileName))
            {
                if (!File.Exists(fileName)) return;
                AugmentedRealityImages[fileName] = Image.FromFile(fileName);
            }
            Image img = AugmentedRealityImages[fileName];
            Point p = found.sample.contour.SourceBoundingRect.Center();
            GraphicsState state = gr.Save();
            gr.TranslateTransform(p.X, p.Y);
            gr.RotateTransform((float)(180f * found.angle / Math.PI));
            gr.ScaleTransform((float)(found.scale), (float)(found.scale));
            gr.DrawImage(img, new Point(-img.Width / 2, -img.Height / 2));
            gr.Restore(state);
        }

        public void LoadCustomImage()
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Image|*.bmp;*.png;*.jpg;*.jpeg" };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            Image<Bgr, byte> image = new Image<Bgr, byte>((Bitmap)Image.FromFile(ofd.FileName));
            CurrentPlate = new LicensePlate(image);
            ProcessFrame();
        }
    }

}