using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Pointillism_image_generator
{
    public partial class Form1 : Form
    {
        Pointillism pointillism;
        string imagesPath;
        bool saveProgress;
        static int progressInterval = 250;
        public Form1()
        {
            InitializeComponent();

            comboBoxPatternSize.Items.Add("7");
            comboBoxPatternSize.Items.Add("9");
            comboBoxPatternSize.Items.Add("11");
            comboBoxPatternSize.Items.Add("13");
            comboBoxPatternSize.SelectedItem = "9";

            string exePath = Path.GetDirectoryName(Application.ExecutablePath);
            imagesPath = Path.Combine(exePath, "images");
            Directory.CreateDirectory(imagesPath);

            btnStart.Enabled = false;
            btnAdd.Enabled = false;
            btnSave.Enabled = false;

            trackBar.Minimum = 0;
            trackBar.Maximum = 0;
            trackBar.Enabled = false;

            progressBar.Visible = false;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            // Load original image

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.JPG;*.PNG,*.BMP)|*.JPG;*.PNG;*.BMP|All files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filePath = ofd.FileName;
                try
                {
                    pbxOriginalImage.Image = Image.FromFile(filePath);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error. Select existing image file.");
                }
            }
            btnStart.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Save pointilism image

            Image outputImage = pbxOutputImage.Image;
            if (outputImage == null)
            {
                MessageBox.Show("Error. First generate an output image.");
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Image File(*.JPG)|*.jpg|Image File(*.PNG)|*.png|Image File (*.BMP)|*.bmp";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                pbxOutputImage.Image.Save(sfd.FileName);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // Initialize pointillism image generator

            if (pbxOriginalImage.Image == null)
            {
                MessageBox.Show("Error. First load an input image.");
                return;
            }

            labelProgress.Text = "Initialization... it may take a few minutes";
            labelProgress.Refresh();
            trackBar.Maximum = 0;
            saveProgress = checkBoxProgress.Checked;
            checkBoxProgress.Enabled = false;
            int patternSize = int.Parse(comboBoxPatternSize.SelectedItem.ToString());

            pointillism = new Pointillism();
            pointillism.Initialize(pbxOriginalImage.Image, patternSize);
            GenerateImage(500);
            btnStart.Enabled = false;
            btnAdd.Enabled = true;
            btnSave.Enabled = true;
        }

        private void GenerateImage(int patterns)
        {
            // Generate image and save image progress

            progressBar.Maximum = patterns;
            progressBar.Visible = true;
            labelProgress.Text = "Generating image...";
            labelProgress.Refresh();
            
            for (int i = 0; i < patterns; i++)
            {
                if (!pointillism.GeneratePointilismImage())
                {
                    return;
                }

                if (saveProgress)
                {
                    int numberOfPatterns = pointillism.GetNumberOfPatterns();
                    if (numberOfPatterns % progressInterval == 0)
                    {
                        pointillism.GetOutputImage().Save(Path.Combine(imagesPath, Convert.ToString(numberOfPatterns) + ".jpg"));
                        ++trackBar.Maximum;
                    }
                }
                progressBar.PerformStep();
            }
            progressBar.Value = 0;
            progressBar.Visible = false;
            labelProgress.Text = "";
            UpdateImage();
        }

        private void UpdateImage()
        {
            pbxOutputImage.Image = pointillism.GetOutputImage();
            labelNumberOfPatterns.Text = Convert.ToString(pointillism.GetNumberOfPatterns());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int numberOfPatterns = (int)numericUpDown.Value;
            GenerateImage(numberOfPatterns);
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            if (trackBar.Value == 0)
            {
                return;
            }
            int numberOfPatterns = trackBar.Value * progressInterval;
            string file = Convert.ToString(numberOfPatterns) + ".jpg";
            pbxOutputImage.Image = Image.FromFile(Path.Combine(imagesPath, file));
            labelNumberOfPatterns.Text = Convert.ToString(numberOfPatterns);
        }

        private void checkBoxProgress_CheckedChanged(object sender, EventArgs e)
        {
            trackBar.Enabled = checkBoxProgress.Checked;
        }
    }
}
