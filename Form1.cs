using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable enable

namespace Pointillism_image_generator
{
    public sealed partial class Form1 : Form
    {
        private PointillismImageGeneratorParallel? _generator;
        private CancellationTokenSource _tokenSource = null!;
        private string _imagesPath;
        private string _imageName = null!;
        private int _imagesToSave = 5;
        private List<int> _trackBarIndexToPatternsCount = null!;
        private IntReference _patternsToAdd = null!;
        
        public Form1()
        {
            InitializeComponent();
            
            // directory
            string? exePath = Path.GetDirectoryName(Application.ExecutablePath);
            _imagesPath = Path.Combine(exePath!, "working-directory-images");
            Directory.CreateDirectory(_imagesPath);
            
            // form
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);

            // combo box
            for (int item = 7; item <= 23; item += 2)
            {
                comboBoxPatternSize.Items.Add(item);
            }
            comboBoxPatternSize.SelectedItem = 7;

            // buttons
            btnStart.Enabled = false;
            btnBackgroundColor.BackColor = Color.White;
            btnAdd.Enabled = false;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            
            // file dialogs
            saveFileDialog.Filter = "Image File(*.JPG)|*.jpg|Image File(*.PNG)|*.png|Image File (*.BMP)|*.bmp";
            openFileDialog.Filter = "Image Files(*.JPG;*.PNG,*.BMP)|*.JPG;*.PNG;*.BMP|All files (*.*)|*.*";

            // trackbar
            trackBar.Maximum = -1;

            // progressBar
            progressBar.Visible = false;
            progressBar.Maximum = 100; // percent
            backgroundWorkerProgressBar.WorkerReportsProgress = true;
            
            // labels
            labelCanNotBeImproved.Visible = false;
            labelCanNotBeImproved.Text = "Generated image can not be improved.";
            labelInit.Visible = false;
            labelInit.Text = "Initialization... it may take a while.";
        }

        #region SETUP

        /// <summary> Loads original image. Enables Start button. Disables Add and Save buttons. </summary>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            var image = LoadImage(out var filePath);
            if (image is null)
                return;
            
            pbxOriginalImage.Image = image;
            _imageName = Path.GetFileName(filePath!);
            pbxOutputImage.Image = null;
            labelCanNotBeImproved.Visible = false;
            labelNumberOfPatterns.Text = 0.ToString();

            btnStart.Text = "Start";
            btnStart.Enabled = true;
            checkBoxProgress.Checked = false;
            btnAdd.Enabled = false;
            btnSave.Enabled = false;
        }
        
        /// <summary>
        /// Loads image from the user.
        /// </summary>
        /// <param name="filePath">filepath is valid if image is not null</param>
        /// <returns>Returns null on failure, otherwise returns an image.</returns>
        private Image? LoadImage(out string? filePath)
        {
            filePath = null;
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return null;
                
            filePath = openFileDialog.FileName;
            try
            {
                return Image.FromFile(filePath);
            }
            catch (Exception)
            {
                MessageBox.Show("Error. Select existing image file.");
                return null;
            }
        }
        
        /// <summary>Sets background color of the generated image.</summary>
        private void btnBackgroundColor_Click(object sender, EventArgs e)
        {
            if (backgroundColorDialog.ShowDialog() == DialogResult.OK)
            {
                btnBackgroundColor.BackColor = backgroundColorDialog.Color;
                btnBackgroundColor.ForeColor = backgroundColorDialog.Color.GetBrightness() > 0.5 ? Color.Black : Color.White;
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
            labelInit.Visible = true;
            labelInit.Refresh();
            trackBar.Maximum = -1;
            _trackBarIndexToPatternsCount = new();
            
            int patternSize = int.Parse(comboBoxPatternSize.SelectedItem.ToString()!);
            _generator?.Dispose();
            _generator = new PointillismImageGeneratorParallel(pbxOriginalImage.Image, patternSize, btnBackgroundColor.BackColor);
            labelInit.Visible = false;
            AddPatterns(100);
            
            btnSave.Enabled = true;
        }

        #endregion


        #region ADD_PATTERNS
        
        /// <summary> Adds patterns to the generated image. </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int patternsCount = (int)numericUpDown.Value;
            AddPatterns(patternsCount);
        }

        /// <summary> Adds patterns to the generated image. </summary>
        /// <param name="patternsCount">number of patterns to add</param>
        private void AddPatterns(int patternsCount)
        {
            btnAdd.Enabled = false;
            checkBoxProgress.Enabled = false;
            progressBar.Visible = true;
            btnCancel.Enabled = true;

            _patternsToAdd = patternsCount.ToIntReference();
            _tokenSource = new CancellationTokenSource();

            backgroundWorkerAddPatterns.RunWorkerAsync();
            backgroundWorkerProgressBar.RunWorkerAsync(patternsCount);
        }
        
        /// <summary>Calls the generator and saves generated images.</summary>
        private void backgroundWorkerAddPatterns_DoWork(object sender, DoWorkEventArgs e)
        {
            int imagesToSave = checkBoxProgress.Checked ? _imagesToSave : 0;
            var (canBeImproved, generatedBitmaps) = _generator!.AddPatterns(_patternsToAdd, imagesToSave, _tokenSource.Token);
            
            foreach (var generatedBitmap in generatedBitmaps)
            {
                generatedBitmap.Bitmap.Save(Path.Combine(_imagesPath, Convert.ToString(generatedBitmap.PatternsCount) + ".jpg"));
                _trackBarIndexToPatternsCount.Add(generatedBitmap.PatternsCount);
                generatedBitmap.Bitmap.Dispose();
            }

            e.Result = new ResultObject {CanBeImproved = canBeImproved, GeneratedBitmapsCount = generatedBitmaps.Count};
        }

        /// <summary>Updates the track bar and other controls.</summary>
        /// <exception cref="ArgumentException">Exception is thrown if no valid result is set.</exception>
        private void backgroundWorkerAddPatterns_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ResultObject result = (ResultObject) (e.Result ?? throw new ArgumentException());
            UpdateTrackBar(result.GeneratedBitmapsCount);
            btnCancel.Enabled = false;
            if (!result.CanBeImproved)
            {
                _patternsToAdd.Value = 0;
                labelCanNotBeImproved.Visible = true;
            }
            else
            {
                btnAdd.Enabled = true;
                checkBoxProgress.Enabled = true;
            }
        }
        
        /// <summary>
        /// Result of background worker for adding patterns.
        /// </summary>
        private record struct ResultObject
        {
            public bool CanBeImproved;
            public int GeneratedBitmapsCount;
        }
        
        /// <summary>Updates the percentage of progress during image generation.</summary>
        /// <exception cref="ArgumentException">Exception is thrown if no valid argument is passed.</exception>
        private void backgroundWorkerProgressBar_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (sender as BackgroundWorker)!;
            int patternsTotal = (int) (e.Argument ?? throw new ArgumentException());
            while (_patternsToAdd.Value > 0 && !_tokenSource.Token.IsCancellationRequested)
            {
                worker.ReportProgress((int) ((patternsTotal - _patternsToAdd.Value) / (double) patternsTotal * 100));
                Thread.Sleep(100);
            }
        }
        
        /// <summary>Sets progress bar value to the percentage of progress.</summary>
        private void backgroundWorkerProgressBar_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        /// <summary>Makes progress bar invisible.</summary>
        private void backgroundWorkerProgressBar_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Visible = false;
            progressBar.Value = 0;
        }

        #endregion


        #region CANCEL

        /// <summary>Cancels adding patterns.</summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _tokenSource.Cancel();
            btnCancel.Enabled = false;
        }

        #endregion

        
        #region GENERATED_IMAGE
        
        /// <summary>Changes the displayed image in the picture box for generated images. </summary>
        private void trackBar_Scroll(object sender, EventArgs e)
        {
            UpdatePbx();
        }
        
        /// <summary>
        /// Increments trackbar maximum, sets trackbar value to its maximum and updates picture box.
        /// </summary>
        /// <param name="countAdded"></param>
        private void UpdateTrackBar(int countAdded)
        {
            trackBar.Maximum += countAdded;
            trackBar.Minimum = 0;
            trackBar.Value = trackBar.Maximum;
            UpdatePbx();
        }

        /// <summary> Updates the displayed image in the picture box for generated images based on the trackbar value. </summary>
        private void UpdatePbx()
        {
            int patternsCount = _trackBarIndexToPatternsCount[trackBar.Value];
            string file = Convert.ToString(patternsCount) + ".jpg";
            var image = Image.FromFile(Path.Combine(_imagesPath, file));
            pbxOutputImage.Image?.Dispose();
            pbxOutputImage.Image = image;
            labelNumberOfPatterns.Text = Convert.ToString(patternsCount);
        }

        /// <summary> Saves displayed generated image. </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            Image outputImage = pbxOutputImage.Image;
            if (outputImage is null)
            {
                MessageBox.Show("Error. First generate an output image.");
                return;
            }

            saveFileDialog.FileName = $"pointillism_{_imageName}_{labelNumberOfPatterns.Text}";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                pbxOutputImage.Image.Save(saveFileDialog.FileName);
            }
        }

        #endregion

        #region FORM

        /// <summary>Minimizes form.</summary>
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        
        /// <summary>Maximizes form.</summary>
        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Maximized)
                WindowState = FormWindowState.Maximized;
            else
                WindowState = FormWindowState.Normal;
        }
        
        /// <summary>Closes form.</summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _generator?.Dispose();
            backgroundWorkerAddPatterns.Dispose();
            backgroundWorkerProgressBar.Dispose();
            pbxOriginalImage.Dispose();
            pbxOutputImage.Image?.Dispose();
            pbxOutputImage.Dispose();
            Directory.Delete(_imagesPath, true);
        }
        
        #endregion
    }
}
