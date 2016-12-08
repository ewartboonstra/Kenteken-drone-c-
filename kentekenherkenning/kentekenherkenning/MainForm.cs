using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using ContourAnalysisNS;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;


namespace kentekenherkenning
{
    public partial class MainForm : Form
    {
        private Image<Bgr, Byte> image;


        ImageProcessor processor;

        Dictionary<string, Image> AugmentedRealityImages = new Dictionary<string, Image>();

        private List<Country> CountryList; 

        bool showAngle;

        private ShowLicensePlates _licensePlateForm;
        private string templateFile;
        private Thread ServerThread;

        //Paint-method variables
        private Font font;
        Brush bgBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
        Brush foreBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
        Pen borderPen = new Pen(Color.FromArgb(150, 0, 255, 0));


        public MainForm()
        {
            InitializeComponent();

            font = new Font(Font.FontFamily, 24);//16
            
            processor = new ImageProcessor();

            ApplySettings();

            StartLicensePlateForm();

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
                Image message = s.WaitForImage();

                Image<Bgr, byte> receivedImage = new Image<Bgr, byte>((Bitmap)message);
                Invoke(new Action(() => image = receivedImage));
                Invoke(new Action(ProcessFrame));

            }
        }


        private void StartLicensePlateForm()
        {
            _licensePlateForm = new ShowLicensePlates();
            _licensePlateForm.Show();
        }

        private void LoadTemplates(string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                    processor.templates = (Templates)new BinaryFormatter().Deserialize(fs);
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
                    new BinaryFormatter().Serialize(fs, processor.templates);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //redraw image on changing settings
        private void SpecialView(object sender, EventArgs e)
        {
            if (cbShowBinarized.Checked)
                ibMain.Image = processor.binarizedFrame;
            else
                ibMain.Image = image;
            Invalidate();        
        }
        /// <summary>
        /// process most recent image saved
        /// </summary>
        private void ProcessFrame()
        {
            foreach (Country country in CountryList)
            {
                try
                {
                    LoadTemplates(country.TemplateLocation);
                    processor.ProcessImage(image);
                    ibMain.Image = image;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                LicensePlate licensePlate = ProcessLicensePlate(country);
               
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
                    Console.WriteLine("Reason: Licenseplate is not unique");
                    continue;
                }

                _licensePlateForm.AddLicensePlate(licensePlate);
                Console.WriteLine($"LicencePlate: {licensePlate.Text}, {country.Name} rejected.");
                break;
            }
        }

        /// <summary>
        /// draw contour box around each character
        /// </summary>
        /// <param name="g">graphics class</param>
        private void DrawContours(Graphics g)
        {
            if (cbShowContours.Checked)
                foreach (Contour<Point> contour in processor.contours)
                    if (contour.Total > 1)
                        g.DrawLines(Pens.Red, contour.ToArray());
        }

        /// <summary>
        /// Draw box around each character
        /// </summary>
        /// <param name="g">Graphics class</param>
        private void DrawBox(Graphics g)
        {
            foreach (FoundTemplateDesc found in processor.foundTemplates)
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

                g.DrawString(text, font, bgBrush, new PointF(p1.X + 1 - font.Height / 3, p1.Y + 1 - font.Height));
                g.DrawString(text, font, foreBrush, new PointF(p1.X - font.Height / 3, p1.Y - font.Height));
            }
        }

        /// <summary>
        /// make a licenceplate of image
        /// </summary>
        /// <param name="country">country to check licenceplate in</param>
        /// <returns> Licenceplate found</returns>
        private LicensePlate ProcessLicensePlate(Country country)
        {
            LicensePlate licensePlate = new LicensePlate(country);

            lock (processor.foundTemplates)
                foreach (FoundTemplateDesc found in processor.foundTemplates)
                {
                    Rectangle foundRect = found.sample.contour.SourceBoundingRect;
                    Point p1 = new Point((foundRect.Left + foundRect.Right) / 2, foundRect.Top);

                    string text = found.template.name;
                    int height = foundRect.Height;

                    FoundCharacter foundCharacter = new FoundCharacter(p1, text, height);

                    licensePlate.Add(foundCharacter);
                }
            return licensePlate;
        }

        /// <summary>
        /// Paint objects on screen after updating image.
        /// </summary>
        private void ibMain_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            DrawContours(g);
            DrawBox(g);
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

        private void cbAutoContrast_CheckedChanged(object sender, EventArgs e)
        {
            ApplySettings();
        }

        private void ApplySettings()
        {
            try
            {
                processor.equalizeHist = cbAutoContrast.Checked;
                showAngle = cbShowAngle.Checked;
                btLoadImage.Enabled = true;

                processor.finder.maxRotateAngle = cbAllowAngleMore45.Checked ? Math.PI : Math.PI / 4;
                processor.minContourArea = (int)nudMinContourArea.Value;
                processor.minContourLength = (int)nudMinContourLength.Value;
                processor.finder.maxACFDescriptorDeviation = (int)nudMaxACFdesc.Value;
                processor.finder.minACF = (double)nudMinACF.Value;
                processor.finder.minICF = (double)nudMinICF.Value;
                processor.blur = cbBlur.Checked;
                processor.noiseFilter = cbNoiseFilter.Checked;
                processor.cannyThreshold = (int)nudMinDefinition.Value;
                nudMinDefinition.Enabled = processor.noiseFilter;
                processor.adaptiveThresholdBlockSize = (int)nudAdaptiveThBlockSize.Value;
                processor.adaptiveThresholdParameter = cbAdaptiveNoiseFilter.Checked ? 1.5 : 0.5;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// import picture from disk for testing purposes 
        /// </summary>
        private void btLoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image|*.bmp;*.png;*.jpg;*.jpeg";
            if (ofd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                try
                {
                    image = new Image<Bgr, byte>((Bitmap)Bitmap.FromFile(ofd.FileName));
                    ProcessFrame();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}