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
        
        private const int GripSize = 16;
        private readonly int _captionBarHeight;
        private readonly Rectangle _originalFormSize;
        private readonly (Control, Rectangle)[] _originalSizesAndLocations;
        private readonly (Control, Point)[] _originalLocations;
        
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
            _captionBarHeight = btnClose.Height;
            _originalFormSize = new Rectangle(Location, Size);
            _originalSizesAndLocations = new (Control, Rectangle)[]
            {
                (panelSettings, new Rectangle(panelSettings.Location, panelSettings.Size)), 
                (panelAddPatterns, new Rectangle(panelAddPatterns.Location, panelAddPatterns.Size)),
                (panelCancle, new Rectangle(panelCancle.Location, panelCancle.Size)), 
                (panelGeneratedImage, new Rectangle(panelGeneratedImage.Location, panelGeneratedImage.Size)),
                (pbxOutputImage, new Rectangle(pbxOutputImage.Location, pbxOutputImage.Size)),
                (pbxOriginalImage, new Rectangle(pbxOriginalImage.Location, pbxOriginalImage.Size)),
                
                (btnLoad, new Rectangle(btnLoad.Location, btnLoad.Size)),
                (comboBoxPatternSize, new Rectangle(comboBoxPatternSize.Location, comboBoxPatternSize.Size)),
                (btnBackgroundColor, new Rectangle(btnBackgroundColor.Location, btnBackgroundColor.Size)),
                (btnStart, new Rectangle(btnStart.Location, btnStart.Size)),
                (btnAdd, new Rectangle(btnAdd.Location, btnAdd.Size)),
                (numericUpDown, new Rectangle(numericUpDown.Location, numericUpDown.Size)),
                (btnCancel, new Rectangle(btnCancel.Location, btnCancel.Size)),
                (progressBar, new Rectangle(progressBar.Location, progressBar.Size)),
                (trackBar, new Rectangle(trackBar.Location, trackBar.Size)),
                (btnSave, new Rectangle(btnSave.Location, btnSave.Size))
            };
            _originalLocations = new (Control, Point)[]
            {
                (labelOriginalImage, labelOriginalImage.Location),
                (labelOutputImage, labelOutputImage.Location),
                (labelNumberOfPatterns, labelNumberOfPatterns.Location),
                (labelInit, labelInit.Location),
                (labelCanNotBeImproved, labelCanNotBeImproved.Location),
                (labelPatternSize, labelPatternSize.Location),
                (checkBoxProgress, checkBoxProgress.Location),
                (labelScroll, labelScroll.Location)
            };

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
            var taskAddPatterns = _generator!.AddPatternsAsync(_patternsToAdd, imagesToSave, _tokenSource.Token);
            var (canBeImproved, generatedBitmaps) = taskAddPatterns.Result;
            
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
            var image = pbxOutputImage.Image;
            pbxOutputImage.Image = Image.FromFile(Path.Combine(_imagesPath, file));
            image?.Dispose();
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

        protected override void OnPaint(PaintEventArgs e) {
            Rectangle rectangle = new Rectangle(ClientSize.Width - GripSize, ClientSize.Height - GripSize, GripSize, GripSize);
            ControlPaint.DrawSizeGrip(e.Graphics, BackColor, rectangle);
        }
        
        protected override void WndProc(ref Message m) {
            if (m.Msg == 0x84) {  // Trap WM_NCHITTEST
                Point pos = new Point(m.LParam.ToInt32());
                pos = PointToClient(pos);
                if (pos.Y < _captionBarHeight) {
                    m.Result = (IntPtr)2;  // HTCAPTION
                    return;
                }
                if (pos.X >= ClientSize.Width - GripSize && pos.Y >= ClientSize.Height - GripSize) {
                    m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    return;
                }
            }
            base.WndProc(ref m);
        }

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
        
        /// <summary>Relocates and resizes controls.</summary>
        private void Form1_Resize(object sender, EventArgs e)
        {
            foreach (var (control, original) in _originalSizesAndLocations)
            {
                RelocateControl(control, original.Location);
                ResizeControl(control, original.Size);
            }

            foreach (var (control, point) in _originalLocations)
            {
                RelocateControl(control, point);
            }
        }

        /// <summary>Relocates control based on resizing the form.</summary>
        /// <param name="control">a control to relocate</param>
        /// <param name="originalLocation">a control's original location</param>
        private void RelocateControl(Control control, Point originalLocation)
        {
            float ratioX = (float) Width / _originalFormSize.Width;
            float ratioY = (float) (Height - _captionBarHeight) / (_originalFormSize.Height - _captionBarHeight);

            int newX = (int) (originalLocation.X * ratioX);
            int newY = (int) ((originalLocation.Y - _captionBarHeight)  * ratioY + _captionBarHeight);
            control.Location = new Point(newX, newY);
        }

        /// <summary>Resizes control based on resizing the form.</summary>
        /// <param name="control">a control to resize</param>
        /// <param name="originalSize">a control's original size</param>
        private void ResizeControl(Control control, Size originalSize)
        {
            float ratioX = (float) Width / _originalFormSize.Width;
            float ratioY = (float) (Height - _captionBarHeight) / (_originalFormSize.Height - _captionBarHeight);

            int newWidth = (int) (originalSize.Width * ratioX);
            int newHeight = (int) (originalSize.Height * ratioY);
            control.Size = new Size(newWidth, newHeight);
        }

        /// <summary>Disposes images.</summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _generator?.Dispose();
            pbxOriginalImage.Dispose();
            var image = pbxOutputImage.Image;
            pbxOutputImage.Dispose();
            image?.Dispose();
            Directory.Delete(_imagesPath, true);
        }
        
        #endregion
    }
}
