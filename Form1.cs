using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable enable

namespace Pointillism_image_generator
{
    public partial class Form1 : Form
    {
        private PointillismImageGenerator _pointillismImageGenerator;
        private string _imagesPath;
        private bool _saveProgress;
        private static int _progressInterval = 250;
        public Form1()
        {
            InitializeComponent();

            comboBoxPatternSize.Items.Add("7");
            comboBoxPatternSize.Items.Add("9");
            comboBoxPatternSize.Items.Add("11");
            comboBoxPatternSize.Items.Add("13");
            comboBoxPatternSize.SelectedItem = "7";

            string? exePath = Path.GetDirectoryName(Application.ExecutablePath);
            _imagesPath = Path.Combine(exePath!, "images");
            Directory.CreateDirectory(_imagesPath);

            btnStart.Enabled = false;
            btnAdd.Enabled = false;
            btnSave.Enabled = false;

            trackBar.Minimum = 0;
            trackBar.Maximum = 0;
            trackBar.Enabled = false;

            progressBar.Visible = false;
        }

        /// <summary> Loads original image. Enables Start button and checkbox. Disables Add button. </summary>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.JPG;*.PNG,*.BMP)|*.JPG;*.PNG;*.BMP|All files (*.*)|*.*";

            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            
            string filePath = ofd.FileName;
            try
            {
                pbxOriginalImage.Image = Image.FromFile(filePath);
            }
            catch (Exception)
            {
                MessageBox.Show("Error. Select existing image file.");
                return;
            }

            btnStart.Enabled = true;
            checkBoxProgress.Enabled = true;
            btnAdd.Enabled = false;
        }

        /// <summary> Saves generated image. </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            Image outputImage = pbxOutputImage.Image;
            if (outputImage == null)
            {
                MessageBox.Show("Error. First generate an output image.");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Image File(*.JPG)|*.jpg|Image File(*.PNG)|*.png|Image File (*.BMP)|*.bmp";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                pbxOutputImage.Image.Save(sfd.FileName);
            }
        }

        /// <summary> Initializes pointillism image generator. </summary>
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (pbxOriginalImage.Image == null)
            {
                MessageBox.Show("Error. First load an input image.");
                return;
            }

            btnStart.Enabled = false;
            checkBoxProgress.Enabled = false;
            _saveProgress = checkBoxProgress.Checked;
            labelProgress.Text = "Initialization... it may take a few minutes";
            labelProgress.Refresh();
            trackBar.Maximum = 0;
            
            int patternSize = int.Parse(comboBoxPatternSize.SelectedItem.ToString()!);
            _pointillismImageGenerator = new PointillismImageGenerator(pbxOriginalImage.Image, patternSize, Color.White);
            GenerateImage(100);
            
            btnAdd.Enabled = true;
            btnSave.Enabled = true;
        }

        /// <summary> Generates image and saves image progress. </summary>
        /// <param name="patternsCount">number of patterns to add</param>
        private void GenerateImage(int patternsCount)
        {
            progressBar.Maximum = patternsCount;
            progressBar.Visible = true;
            labelProgress.Text = "Generating image...";
            labelProgress.Refresh();

            _pointillismImageGenerator.AddBestOfBestPatterns(patternsCount);
            // for (int i = 0; i < patternsCount; i++)
            // {
            //     if (!_pointillismImageGenerator.AddBestOfBestPatterns(patternsCount))
            //         return;
            //
            //     if (_saveProgress)
            //     {
            //         int numberOfPatterns = _pointillismImageGenerator.GetNumberOfPatterns();
            //         if (numberOfPatterns % _progressInterval == 0)
            //         {
            //             _pointillismImageGenerator.GetOutputImage().Save(Path.Combine(_imagesPath, Convert.ToString(numberOfPatterns) + ".jpg"));
            //             ++trackBar.Maximum;
            //         }
            //     }
            //     progressBar.PerformStep();
            // }

            progressBar.Value = 0;
            progressBar.Visible = false;
            labelProgress.Text = "";
            UpdateImage();
        }

        /// <summary> Updates generated image in the picture box. </summary>
        private void UpdateImage()
        {
            pbxOutputImage.Image = _pointillismImageGenerator.GetOutputImage();
            labelNumberOfPatterns.Text = Convert.ToString(_pointillismImageGenerator.GetNumberOfPatterns());
        }

        /// <summary> Adds patterns to the generated image. </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int patternsCount = (int)numericUpDown.Value;
            GenerateImage(patternsCount);
        }

        /// <summary>Changes the generated image displayed in the picture box. </summary>
        private void trackBar_Scroll(object sender, EventArgs e)
        {
            if (trackBar.Value == 0)
                return;
            
            int patternsCount = trackBar.Value * _progressInterval;
            string file = Convert.ToString(patternsCount) + ".jpg";
            pbxOutputImage.Image = Image.FromFile(Path.Combine(_imagesPath, file));
            labelNumberOfPatterns.Text = Convert.ToString(patternsCount);
        }

        /// <summary> Enable track bar when checkbox is checked and vice versa. </summary>
        private void checkBoxProgress_CheckedChanged(object sender, EventArgs e)
        {
            trackBar.Enabled = checkBoxProgress.Checked;
        }
    }
}
