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
        private PlateProcessor plateProcessor;
        private ImageProcessor processor;
        private Thread ServerThread;
        private Font font;

        public MainForm(ShowLicensePlates licensePlateForm)
        {
            InitializeComponent();
            processor = new ImageProcessor();
            ApplySettings();
            plateProcessor = new PlateProcessor(processor,licensePlateForm);

            
            plateProcessor.Changed += UpdateImage;


            //start server
            ServerThread = new Thread(new ThreadStart(RunConnectionServer));
            ServerThread.Start();
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
                Invoke(new Action(() => plateProcessor.CurrentPlate = plate));
                Invoke(new Action(plateProcessor.ProcessFrame));

            }
        }

        public void UpdateImage()
        {
            ibMain.Image = plateProcessor.CurrentImage;
            Invalidate();
        }

        /// <summary>
        /// Paint objects on screen after updating image.
        /// </summary>
        private void ibMain_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (cbShowContours.Checked)
                plateProcessor.DrawContours(g);

            plateProcessor.DrawBox(g);
        }
        

        private void cbAutoContrast_CheckedChanged(object sender, EventArgs e)
        {
            ApplySettings();
            plateProcessor.ProcessFrame();
        }

        private void ApplySettings()
        {
            try
            {
                processor.equalizeHist = cbAutoContrast.Checked;
                btLoadImage.Enabled = true;

                processor.showBinarized = cbShowBinarized.Checked;
                processor.showContours = cbShowContours.Checked;
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
                MessageBox.Show("Fout in functie: ApplySettings()" + ex.Message);
            }
        }

        /// <summary>
        /// import picture from disk for testing purposes 
        /// </summary>
        private void btLoadImage_Click(object sender, EventArgs e)
        {
            plateProcessor.LoadCustomImage();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Visible = false;
            }
    }
}