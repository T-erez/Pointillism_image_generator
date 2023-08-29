﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable enable

namespace Pointillism_image_generator
{
    public partial class Form1 : Form
    {
        private PointillismImageGeneratorParallel _generator;
        private CancellationTokenSource _tokenSource = new();
        private string _imagesPath;
        private string _imageName;
        private List<int> _trackBarIndexToPatternsCount;
        
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            for (int item = 7; item <= 23; item += 2)
            {
                comboBoxPatternSize.Items.Add(item);
            }
            comboBoxPatternSize.SelectedItem = 7;

            string? exePath = Path.GetDirectoryName(Application.ExecutablePath);
            _imagesPath = Path.Combine(exePath!, "images");
            Directory.CreateDirectory(_imagesPath);

            btnStart.Enabled = false;
            btnAdd.Enabled = false;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;

            trackBar.Maximum = -1;
            trackBar.Enabled = true;

            progressBar.Visible = false;
        }

        /// <summary> Loads original image. Enables Start button. Disables Add button. </summary>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            var image = UI.LoadImage(out var filePath);
            if (image is null)
                return;
            pbxOriginalImage.Image = image;
            pbxOutputImage.Image = null;
            _imageName = Path.GetFileName(filePath!);

            btnStart.Enabled = true;
            btnStart.Text = "Start";
            checkBoxProgress.Checked = false;
            btnAdd.Enabled = false;
            btnSave.Enabled = false;
        }

        /// <summary> Saves generated image. </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            Image outputImage = pbxOutputImage.Image;
            if (outputImage is null)
            {
                MessageBox.Show("Error. First generate an output image.");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Image File(*.JPG)|*.jpg|Image File(*.PNG)|*.png|Image File (*.BMP)|*.bmp";
            sfd.FileName = $"pointillism_{_imageName}";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                pbxOutputImage.Image.Save(sfd.FileName);
            }
        }

        /// <summary> Initializes pointillism image generator. </summary>
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (pbxOriginalImage.Image is null)
            {
                MessageBox.Show("Error. First load an input image.");
                return;
            }

            btnStart.Text = "Restart";
            labelProgress.Text = "Initialization... it may take a while.";
            labelProgress.Refresh();
            trackBar.Maximum = -1;
            _trackBarIndexToPatternsCount = new();
            
            int patternSize = int.Parse(comboBoxPatternSize.SelectedItem.ToString()!);
            Color backgroundColor = Color.White;
            _generator = new PointillismImageGeneratorParallel(pbxOriginalImage.Image, patternSize, backgroundColor);
            AddPatterns(100);
            
            btnAdd.Enabled = true;
            btnSave.Enabled = true;
        }

        /// <summary> Adds patterns to the generated image. </summary>
        /// <param name="patternsCount">number of patterns to add</param>
        private void AddPatterns(int patternsCount)
        {
            btnAdd.Enabled = false;
            checkBoxProgress.Enabled = false;
            progressBar.Maximum = patternsCount;
            progressBar.Visible = true;
            labelProgress.Text = "Generating image...";

            IntReference patternsToAdd = patternsCount.ToIntReference();
            _tokenSource = new CancellationTokenSource();
            
            Task.Run(() =>
            {
                btnCancel.Enabled = true;
                int imagesToSave = checkBoxProgress.Checked ? 5 : 0;
                var (canBeImproved, generatedBitmaps) = _generator.AddPatterns(patternsToAdd, _tokenSource.Token, imagesToSave);
                btnCancel.Enabled = false;
                UpdateTrackBar(generatedBitmaps);
                if (!canBeImproved)
                    labelProgress.Text = "Generated image can not be improved.";
                else
                {
                    btnAdd.Enabled = true;
                    labelProgress.Text = "";
                    checkBoxProgress.Enabled = true;
                }
            }, _tokenSource.Token);

            Task.Run(() => UpdateProgressBar(patternsCount, patternsToAdd), _tokenSource.Token);
        }

        /// <summary>
        /// Updates progress bar during generation of image.
        /// </summary>
        /// <param name="patternsTotal">total number of patterns to add</param>
        /// <param name="patternsLeft">number of patterns left to add</param>
        private void UpdateProgressBar(int patternsTotal, IntReference patternsLeft)
        {
            while (patternsLeft.Value > 0 && !_tokenSource.Token.IsCancellationRequested)
            {
                progressBar.Value = patternsTotal - patternsLeft.Value;
                Thread.Sleep(100);
            }
            progressBar.Visible = false;
            progressBar.Value = 0;
        }

        /// <summary>
        /// Saves generated bitmaps, sets trackbar value to its maximum and updates picture box with generated image.
        /// </summary>
        /// <param name="generatedBitmaps"></param>
        private void UpdateTrackBar(IList<GeneratedBitmap> generatedBitmaps)
        {
            foreach (var generatedBitmap in generatedBitmaps)
            {
                generatedBitmap.Bitmap.Save(Path.Combine(_imagesPath, Convert.ToString(generatedBitmap.PatternsCount) + ".jpg"));
                _trackBarIndexToPatternsCount.Add(generatedBitmap.PatternsCount);
            }
            trackBar.Maximum += generatedBitmaps.Count;
            trackBar.Minimum = 0;
            trackBar.Value = trackBar.Maximum;
            UpdatePbx(generatedBitmaps[^1].Bitmap, generatedBitmaps[^1].PatternsCount);
        }

        /// <summary> Updates generated image in the picture box. </summary>
        /// <param name="image">image to display</param>
        /// <param name="patternsCount">number of patterns in the image</param>
        private void UpdatePbx(Image image, int patternsCount)
        {
            pbxOutputImage.Image = image;
            labelNumberOfPatterns.Text = Convert.ToString(patternsCount);
        }

        /// <summary> Adds patterns to the generated image. </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int patternsCount = (int)numericUpDown.Value;
            AddPatterns(patternsCount);
        }

        /// <summary>Changes the generated image displayed in the picture box. </summary>
        private void trackBar_Scroll(object sender, EventArgs e)
        {
            int patternsCount = _trackBarIndexToPatternsCount[trackBar.Value];
            string file = Convert.ToString(patternsCount) + ".jpg";
            UpdatePbx(Image.FromFile(Path.Combine(_imagesPath, file)), patternsCount);
        }

        /// <summary>Cancels adding patterns.</summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _tokenSource.Cancel();
            btnCancel.Enabled = false;
        }
    }
}
