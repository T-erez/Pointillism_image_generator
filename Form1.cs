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

        /// <summary> Load original image. Enable Start button and checkbox. Disable Add button. </summary>
        private void btnLoad_Click(object sender, EventArgs e)
        {
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
            checkBoxProgress.Enabled = true;
            btnAdd.Enabled = false;
        }

        /// <summary> Save generated image. </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
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

        /// <summary> Initialize pointillism image generator. </summary>
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (pbxOriginalImage.Image == null)
            {
                MessageBox.Show("Error. First load an input image.");
                return;
            }

            btnStart.Enabled = false;
            checkBoxProgress.Enabled = false;
            labelProgress.Text = "Initialization... it may take a few minutes";
            labelProgress.Refresh();
            trackBar.Maximum = 0;
            
            saveProgress = checkBoxProgress.Checked;
            int patternSize = int.Parse(comboBoxPatternSize.SelectedItem.ToString());
            pointillism = new Pointillism();
            pointillism.Initialize(pbxOriginalImage.Image, patternSize);
            GenerateImage(500);
            
            btnAdd.Enabled = true;
            btnSave.Enabled = true;
        }

        /// <summary> Generate image and save image progress. </summary>
        /// <param name="patterns">number of patterns to add</param>
        private void GenerateImage(int patterns)
        {
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

        /// <summary> Update generated image in the picture box. </summary>
        private void UpdateImage()
        {
            pbxOutputImage.Image = pointillism.GetOutputImage();
            labelNumberOfPatterns.Text = Convert.ToString(pointillism.GetNumberOfPatterns());
        }

        /// <summary> Add more patterns to generated image. </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int numberOfPatterns = (int)numericUpDown.Value;
            GenerateImage(numberOfPatterns);
        }

        /// <summary>Change generated image displayed in picture box. </summary>
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

        /// <summary> Enable track bar when checkbox checked and vice versa. </summary>
        private void checkBoxProgress_CheckedChanged(object sender, EventArgs e)
        {
            trackBar.Enabled = checkBoxProgress.Checked;
        }
    }
}
