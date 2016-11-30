//
//  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
//  PURPOSE.
//
//  License: GNU General Public License version 3 (GPLv3)
//
//  Email: pavel_torgashov@mail.ru.
//
//  Copyright (C) Pavel Torgashov, 2011. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
        private Image<Bgr, Byte> frame;
            
        ImageProcessor processor;
        Dictionary<string, Image> AugmentedRealityImages = new Dictionary<string, Image>();

        int frameCount = 0;
        int oldFrameCount = 0;
        bool showAngle;
      
        string templateFile;

        private ShowLicensePlates _licensePlateForm;
        private Thread _imagelistenerThread;

        private readonly ConsoleKeepTrackUtilities consoleUtilities = new ConsoleKeepTrackUtilities();

        //Paint-method variables
        private Font font;
        Brush bgBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
        Brush foreBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
        Pen borderPen = new Pen(Color.FromArgb(150, 0, 255, 0));


        public MainForm()
        {
            InitializeComponent();

             font = new Font(Font.FontFamily, 24);//16

            //create image preocessor
            processor = new ImageProcessor();
            //load default templates
            templateFile = "../../Kenteken.bin";
            LoadTemplates(templateFile);
            
            
            //apply settings
            ApplySettings();
            //
            Application.Idle += new EventHandler(Application_Idle);

            StartLicensePlateForm();

            _imagelistenerThread = new Thread(new ThreadStart(RunConnectionServer));
            _imagelistenerThread.Start();

        }

        /// <summary>
        /// Start connection to server on a new thread
        /// </summary>
        private void RunConnectionServer()
        {
            //start server klasse
            ServerConnection s = new ServerConnection();

            //maak verbinding
            s.SetConnection();
            while (true)
            {
                Image message = s.WaitForImage();

               var receivedImage = new Image<Bgr, byte>((Bitmap) message);
                Invoke(new Action( () => frame = receivedImage));
                
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
                using(FileStream fs = new FileStream(fileName, FileMode.Open))
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

        void Application_Idle(object sender, EventArgs e)
        {
            ProcessFrame();
        }

        private void ProcessFrame()
        {
            
            try
            {
                frameCount++;
                //
                if(frame!= null)
                    processor.ProcessImage(frame);
                //
                if(cbShowBinarized.Checked)
                    ibMain.Image = processor.binarizedFrame;
                else
                    ibMain.Image = frame;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private void tmUpdateState_Tick(object sender, EventArgs e)
        {
            lbFPS.Text = (frameCount - oldFrameCount) + " fps";
            oldFrameCount = frameCount;
            if (processor.contours != null)
                lbContoursCount.Text = "Contours: " + processor.contours.Count;
            if (processor.foundTemplates != null)
                lbRecognized.Text = "Recognized contours: " + processor.foundTemplates.Count;
        }

        //part of Paint-method
        private void DrawContours(Graphics g)
        {
            g.DrawString(lbFPS.Text, new Font(Font.FontFamily, 16), Brushes.Yellow, new PointF(1, 1));
            
            if (cbShowContours.Checked)
                foreach (var contour in processor.contours)
                    if (contour.Total > 1)
                        g.DrawLines(Pens.Red, contour.ToArray());
        }

        //part of ProcessLicensePlate, which gives te needed variables of the new found character.
        private void PerformDrawingsOfFoundCharacter(Graphics g, string text, int height, FoundTemplateDesc found, Rectangle foundRect, Point p1)
        {
            if (showAngle)
                text += string.Format("\r\nangle={0:000}°\r\nscale={1:0.0}", 180 * found.angle / Math.PI, found.scale);
            g.DrawRectangle(borderPen, foundRect);

            //text += "-" + testCounter++;    
            g.DrawString(text, font, bgBrush, new PointF(p1.X + 1 - font.Height / 3, p1.Y + 1 - font.Height));
            g.DrawString(text, font, foreBrush, new PointF(p1.X - font.Height / 3, p1.Y - font.Height));
        }

        private LicensePlate ProcessLicensePlate(Graphics g)
        {
            var processingLicensePlate = new LicensePlate();
            lock (processor.foundTemplates)
                foreach (FoundTemplateDesc found in processor.foundTemplates)
                {


                    if (found.template.name.EndsWith(".png") || found.template.name.EndsWith(".jpg"))
                    {
                        DrawAugmentedReality(found, g);
                        continue;
                    }

                    Rectangle foundRect = found.sample.contour.SourceBoundingRect;
                    Point p1 = new Point((foundRect.Left + foundRect.Right) / 2, foundRect.Top);

                    var text = found.template.name;
                    var height = foundRect.Height;
                    PerformDrawingsOfFoundCharacter(g,text, height, found, foundRect, p1);

                    //put it in the license plate (made by Julian)
                    var foundCharacter = new FoundCharacter(p1, text, height);
                    processingLicensePlate.Add(foundCharacter);

                }
            return processingLicensePlate;
        }


        private void ibMain_Paint(object sender, PaintEventArgs e)
        {
            if (frame == null) return;

            DrawContours(e.Graphics);
            var processingLicensePlate = ProcessLicensePlate(e.Graphics);

            //add the license plate to the list (made by Julian)
            if (processingLicensePlate.IsValid())
            {
                _licensePlateForm.AddLicensePlate(processingLicensePlate);
            }
            else
            {
                consoleUtilities.WriteOnce("Processed 'license plate' rejected: " + processingLicensePlate.Text);
                
            }

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
            var state = gr.Save();
            gr.TranslateTransform(p.X, p.Y);
            gr.RotateTransform((float)(180f * found.angle / Math.PI));
            gr.ScaleTransform((float)(found.scale), (float)(found.scale));
            gr.DrawImage(img, new Point(-img.Width/2, -img.Height/2));
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
                processor.adaptiveThresholdParameter = cbAdaptiveNoiseFilter.Checked?1.5:0.5;
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btLoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image|*.bmp;*.png;*.jpg;*.jpeg";
            if (ofd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                try
                {
                    frame = new Image<Bgr, byte>((Bitmap)Bitmap.FromFile(ofd.FileName));
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