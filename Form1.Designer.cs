
namespace Pointillism_image_generator
{
    sealed partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pbxOriginalImage = new System.Windows.Forms.PictureBox();
            this.pbxOutputImage = new System.Windows.Forms.PictureBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.labelPatternSize = new System.Windows.Forms.Label();
            this.comboBoxPatternSize = new System.Windows.Forms.ComboBox();
            this.labelOriginalImage = new System.Windows.Forms.Label();
            this.labelGeneratedImage = new System.Windows.Forms.Label();
            this.labelOutputImage = new System.Windows.Forms.Label();
            this.labelNumberOfPatterns = new System.Windows.Forms.Label();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.btnAdd = new System.Windows.Forms.Button();
            this.labelScroll = new System.Windows.Forms.Label();
            this.numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.checkBoxProgress = new System.Windows.Forms.CheckBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelProgress = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnBackgroundColor = new System.Windows.Forms.Button();
            this.backgroundColorDialog = new System.Windows.Forms.ColorDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnMaximize = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.panelSettings = new System.Windows.Forms.Panel();
            this.panelHeadline = new System.Windows.Forms.Panel();
            this.labelSetup = new System.Windows.Forms.Label();
            this.panelAddPatterns = new System.Windows.Forms.Panel();
            this.panelHeadline2 = new System.Windows.Forms.Panel();
            this.labelAddPatterns = new System.Windows.Forms.Label();
            this.panelCancle = new System.Windows.Forms.Panel();
            this.panelHeadline3 = new System.Windows.Forms.Panel();
            this.labelCancle = new System.Windows.Forms.Label();
            this.panelGeneratedImage = new System.Windows.Forms.Panel();
            this.panelHeadline4 = new System.Windows.Forms.Panel();
            this.labelResult = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbxOriginalImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxOutputImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).BeginInit();
            this.panelSettings.SuspendLayout();
            this.panelHeadline.SuspendLayout();
            this.panelAddPatterns.SuspendLayout();
            this.panelHeadline2.SuspendLayout();
            this.panelCancle.SuspendLayout();
            this.panelHeadline3.SuspendLayout();
            this.panelGeneratedImage.SuspendLayout();
            this.panelHeadline4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbxOriginalImage
            // 
            this.pbxOriginalImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxOriginalImage.Location = new System.Drawing.Point(21, 74);
            this.pbxOriginalImage.Name = "pbxOriginalImage";
            this.pbxOriginalImage.Size = new System.Drawing.Size(268, 239);
            this.pbxOriginalImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxOriginalImage.TabIndex = 0;
            this.pbxOriginalImage.TabStop = false;
            // 
            // pbxOutputImage
            // 
            this.pbxOutputImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxOutputImage.Location = new System.Drawing.Point(35, 74);
            this.pbxOutputImage.Name = "pbxOutputImage";
            this.pbxOutputImage.Size = new System.Drawing.Size(546, 363);
            this.pbxOutputImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxOutputImage.TabIndex = 1;
            this.pbxOutputImage.TabStop = false;
            // 
            // btnLoad
            // 
            this.btnLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(109)))), ((int)(((byte)(125)))));
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoad.ForeColor = System.Drawing.Color.White;
            this.btnLoad.Location = new System.Drawing.Point(304, 74);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(178, 50);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = false;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(109)))), ((int)(((byte)(125)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(426, 471);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(145, 55);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(109)))), ((int)(((byte)(125)))));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(304, 283);
            this.btnStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(178, 50);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // labelPatternSize
            // 
            this.labelPatternSize.AutoSize = true;
            this.labelPatternSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(243)))), ((int)(((byte)(221)))));
            this.labelPatternSize.Location = new System.Drawing.Point(304, 147);
            this.labelPatternSize.Name = "labelPatternSize";
            this.labelPatternSize.Size = new System.Drawing.Size(124, 17);
            this.labelPatternSize.TabIndex = 6;
            this.labelPatternSize.Text = "Select pattern size";
            // 
            // comboBoxPatternSize
            // 
            this.comboBoxPatternSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(213)))), ((int)(((byte)(185)))));
            this.comboBoxPatternSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPatternSize.FormattingEnabled = true;
            this.comboBoxPatternSize.Location = new System.Drawing.Point(304, 166);
            this.comboBoxPatternSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxPatternSize.Name = "comboBoxPatternSize";
            this.comboBoxPatternSize.Size = new System.Drawing.Size(178, 25);
            this.comboBoxPatternSize.TabIndex = 7;
            // 
            // labelOriginalImage
            // 
            this.labelOriginalImage.AutoSize = true;
            this.labelOriginalImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(243)))), ((int)(((byte)(221)))));
            this.labelOriginalImage.Location = new System.Drawing.Point(21, 316);
            this.labelOriginalImage.Name = "labelOriginalImage";
            this.labelOriginalImage.Size = new System.Drawing.Size(104, 17);
            this.labelOriginalImage.TabIndex = 9;
            this.labelOriginalImage.Text = "Original image";
            // 
            // labelGeneratedImage
            // 
            this.labelGeneratedImage.AutoSize = true;
            this.labelGeneratedImage.Location = new System.Drawing.Point(457, 440);
            this.labelGeneratedImage.Name = "labelGeneratedImage";
            this.labelGeneratedImage.Size = new System.Drawing.Size(124, 17);
            this.labelGeneratedImage.TabIndex = 10;
            this.labelGeneratedImage.Text = "Generated image";
            // 
            // labelOutputImage
            // 
            this.labelOutputImage.AutoSize = true;
            this.labelOutputImage.Location = new System.Drawing.Point(35, 440);
            this.labelOutputImage.Name = "labelOutputImage";
            this.labelOutputImage.Size = new System.Drawing.Size(142, 17);
            this.labelOutputImage.TabIndex = 11;
            this.labelOutputImage.Text = "Number of patterns: ";
            // 
            // labelNumberOfPatterns
            // 
            this.labelNumberOfPatterns.AutoSize = true;
            this.labelNumberOfPatterns.Location = new System.Drawing.Point(179, 440);
            this.labelNumberOfPatterns.Name = "labelNumberOfPatterns";
            this.labelNumberOfPatterns.Size = new System.Drawing.Size(15, 17);
            this.labelNumberOfPatterns.TabIndex = 12;
            this.labelNumberOfPatterns.Text = "0";
            // 
            // trackBar
            // 
            this.trackBar.AutoSize = false;
            this.trackBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(213)))), ((int)(((byte)(185)))));
            this.trackBar.Location = new System.Drawing.Point(179, 476);
            this.trackBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackBar.Maximum = 20;
            this.trackBar.Minimum = 1;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(201, 42);
            this.trackBar.TabIndex = 13;
            this.trackBar.Value = 1;
            this.trackBar.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(109)))), ((int)(((byte)(125)))));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(174, 70);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(130, 50);
            this.btnAdd.TabIndex = 16;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // labelScroll
            // 
            this.labelScroll.AutoSize = true;
            this.labelScroll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(213)))), ((int)(((byte)(185)))));
            this.labelScroll.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelScroll.Location = new System.Drawing.Point(58, 476);
            this.labelScroll.Name = "labelScroll";
            this.labelScroll.Size = new System.Drawing.Size(115, 42);
            this.labelScroll.TabIndex = 17;
            this.labelScroll.Text = "Scroll to view \r\nprogress";
            // 
            // numericUpDown
            // 
            this.numericUpDown.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown.Location = new System.Drawing.Point(21, 70);
            this.numericUpDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown.Name = "numericUpDown";
            this.numericUpDown.Size = new System.Drawing.Size(115, 23);
            this.numericUpDown.TabIndex = 19;
            this.numericUpDown.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // checkBoxProgress
            // 
            this.checkBoxProgress.AutoSize = true;
            this.checkBoxProgress.Location = new System.Drawing.Point(21, 101);
            this.checkBoxProgress.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxProgress.Name = "checkBoxProgress";
            this.checkBoxProgress.Size = new System.Drawing.Size(115, 21);
            this.checkBoxProgress.TabIndex = 20;
            this.checkBoxProgress.Text = "Save progress";
            this.checkBoxProgress.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            this.progressBar.ForeColor = System.Drawing.Color.Coral;
            this.progressBar.Location = new System.Drawing.Point(58, 400);
            this.progressBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(492, 17);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 21;
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(179, 400);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(0, 17);
            this.labelProgress.TabIndex = 22;
            this.labelProgress.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(109)))), ((int)(((byte)(125)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(31, 70);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 50);
            this.btnCancel.TabIndex = 23;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnBackgroundColor
            // 
            this.btnBackgroundColor.BackColor = System.Drawing.Color.White;
            this.btnBackgroundColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackgroundColor.Location = new System.Drawing.Point(304, 210);
            this.btnBackgroundColor.Name = "btnBackgroundColor";
            this.btnBackgroundColor.Size = new System.Drawing.Size(178, 55);
            this.btnBackgroundColor.TabIndex = 24;
            this.btnBackgroundColor.Text = "Change background color";
            this.btnBackgroundColor.UseVisualStyleBackColor = false;
            this.btnBackgroundColor.Click += new System.EventHandler(this.btnBackgroundColor_Click);
            // 
            // backgroundColorDialog
            // 
            this.backgroundColorDialog.AnyColor = true;
            this.backgroundColorDialog.Color = System.Drawing.Color.White;
            this.backgroundColorDialog.FullOpen = true;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(1200, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(51, 35);
            this.btnClose.TabIndex = 25;
            this.btnClose.Text = "╳";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnMaximize
            // 
            this.btnMaximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximize.BackColor = System.Drawing.Color.Transparent;
            this.btnMaximize.FlatAppearance.BorderSize = 0;
            this.btnMaximize.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnMaximize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximize.Location = new System.Drawing.Point(1143, 0);
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.Size = new System.Drawing.Size(51, 35);
            this.btnMaximize.TabIndex = 26;
            this.btnMaximize.Text = "⬜";
            this.btnMaximize.UseVisualStyleBackColor = false;
            this.btnMaximize.Click += new System.EventHandler(this.btnMaximize_Click);
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnMinimize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Location = new System.Drawing.Point(1086, 0);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(51, 35);
            this.btnMinimize.TabIndex = 27;
            this.btnMinimize.Text = "⎯";
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // panelSettings
            // 
            this.panelSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(243)))), ((int)(((byte)(221)))));
            this.panelSettings.Controls.Add(this.pbxOriginalImage);
            this.panelSettings.Controls.Add(this.labelOriginalImage);
            this.panelSettings.Controls.Add(this.btnLoad);
            this.panelSettings.Controls.Add(this.labelPatternSize);
            this.panelSettings.Controls.Add(this.comboBoxPatternSize);
            this.panelSettings.Controls.Add(this.btnBackgroundColor);
            this.panelSettings.Controls.Add(this.btnStart);
            this.panelSettings.Controls.Add(this.panelHeadline);
            this.panelSettings.Location = new System.Drawing.Point(45, 78);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(510, 363);
            this.panelSettings.TabIndex = 28;
            // 
            // panelHeadline
            // 
            this.panelHeadline.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHeadline.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(143)))), ((int)(((byte)(192)))), ((int)(((byte)(169)))));
            this.panelHeadline.Controls.Add(this.labelSetup);
            this.panelHeadline.Location = new System.Drawing.Point(0, 0);
            this.panelHeadline.Name = "panelHeadline";
            this.panelHeadline.Size = new System.Drawing.Size(510, 51);
            this.panelHeadline.TabIndex = 0;
            // 
            // labelSetup
            // 
            this.labelSetup.AutoSize = true;
            this.labelSetup.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelSetup.ForeColor = System.Drawing.Color.White;
            this.labelSetup.Location = new System.Drawing.Point(21, 13);
            this.labelSetup.Name = "labelSetup";
            this.labelSetup.Size = new System.Drawing.Size(62, 23);
            this.labelSetup.TabIndex = 0;
            this.labelSetup.Text = "SETUP";
            // 
            // panelAddPatterns
            // 
            this.panelAddPatterns.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(243)))), ((int)(((byte)(221)))));
            this.panelAddPatterns.Controls.Add(this.panelHeadline2);
            this.panelAddPatterns.Controls.Add(this.numericUpDown);
            this.panelAddPatterns.Controls.Add(this.btnAdd);
            this.panelAddPatterns.Controls.Add(this.checkBoxProgress);
            this.panelAddPatterns.Location = new System.Drawing.Point(45, 484);
            this.panelAddPatterns.Name = "panelAddPatterns";
            this.panelAddPatterns.Size = new System.Drawing.Size(332, 137);
            this.panelAddPatterns.TabIndex = 29;
            // 
            // panelHeadline2
            // 
            this.panelHeadline2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHeadline2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(143)))), ((int)(((byte)(192)))), ((int)(((byte)(169)))));
            this.panelHeadline2.Controls.Add(this.labelAddPatterns);
            this.panelHeadline2.Location = new System.Drawing.Point(0, 0);
            this.panelHeadline2.Name = "panelHeadline2";
            this.panelHeadline2.Size = new System.Drawing.Size(332, 51);
            this.panelHeadline2.TabIndex = 1;
            // 
            // labelAddPatterns
            // 
            this.labelAddPatterns.AutoSize = true;
            this.labelAddPatterns.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelAddPatterns.ForeColor = System.Drawing.Color.White;
            this.labelAddPatterns.Location = new System.Drawing.Point(21, 13);
            this.labelAddPatterns.Name = "labelAddPatterns";
            this.labelAddPatterns.Size = new System.Drawing.Size(141, 23);
            this.labelAddPatterns.TabIndex = 0;
            this.labelAddPatterns.Text = "ADD PATTERNS";
            // 
            // panelCancle
            // 
            this.panelCancle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(243)))), ((int)(((byte)(221)))));
            this.panelCancle.Controls.Add(this.panelHeadline3);
            this.panelCancle.Controls.Add(this.btnCancel);
            this.panelCancle.Location = new System.Drawing.Point(396, 484);
            this.panelCancle.Name = "panelCancle";
            this.panelCancle.Size = new System.Drawing.Size(159, 137);
            this.panelCancle.TabIndex = 30;
            // 
            // panelHeadline3
            // 
            this.panelHeadline3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHeadline3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(143)))), ((int)(((byte)(192)))), ((int)(((byte)(169)))));
            this.panelHeadline3.Controls.Add(this.labelCancle);
            this.panelHeadline3.Location = new System.Drawing.Point(0, 0);
            this.panelHeadline3.Name = "panelHeadline3";
            this.panelHeadline3.Size = new System.Drawing.Size(159, 51);
            this.panelHeadline3.TabIndex = 2;
            // 
            // labelCancle
            // 
            this.labelCancle.AutoSize = true;
            this.labelCancle.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelCancle.ForeColor = System.Drawing.Color.White;
            this.labelCancle.Location = new System.Drawing.Point(21, 13);
            this.labelCancle.Name = "labelCancle";
            this.labelCancle.Size = new System.Drawing.Size(86, 23);
            this.labelCancle.TabIndex = 0;
            this.labelCancle.Text = "CANCEL";
            // 
            // panelGeneratedImage
            // 
            this.panelGeneratedImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(243)))), ((int)(((byte)(221)))));
            this.panelGeneratedImage.Controls.Add(this.labelProgress);
            this.panelGeneratedImage.Controls.Add(this.panelHeadline4);
            this.panelGeneratedImage.Controls.Add(this.labelNumberOfPatterns);
            this.panelGeneratedImage.Controls.Add(this.btnSave);
            this.panelGeneratedImage.Controls.Add(this.labelGeneratedImage);
            this.panelGeneratedImage.Controls.Add(this.labelOutputImage);
            this.panelGeneratedImage.Controls.Add(this.trackBar);
            this.panelGeneratedImage.Controls.Add(this.labelScroll);
            this.panelGeneratedImage.Controls.Add(this.progressBar);
            this.panelGeneratedImage.Controls.Add(this.pbxOutputImage);
            this.panelGeneratedImage.Location = new System.Drawing.Point(587, 78);
            this.panelGeneratedImage.Name = "panelGeneratedImage";
            this.panelGeneratedImage.Size = new System.Drawing.Size(617, 543);
            this.panelGeneratedImage.TabIndex = 31;
            // 
            // panelHeadline4
            // 
            this.panelHeadline4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHeadline4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(143)))), ((int)(((byte)(192)))), ((int)(((byte)(169)))));
            this.panelHeadline4.Controls.Add(this.labelResult);
            this.panelHeadline4.Location = new System.Drawing.Point(0, 0);
            this.panelHeadline4.Name = "panelHeadline4";
            this.panelHeadline4.Size = new System.Drawing.Size(617, 51);
            this.panelHeadline4.TabIndex = 22;
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelResult.ForeColor = System.Drawing.Color.White;
            this.labelResult.Location = new System.Drawing.Point(21, 13);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(183, 23);
            this.labelResult.TabIndex = 0;
            this.labelResult.Text = "GENERATED IMAGE";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(176)))), ((int)(((byte)(171)))));
            this.ClientSize = new System.Drawing.Size(1250, 675);
            this.Controls.Add(this.panelGeneratedImage);
            this.Controls.Add(this.panelCancle);
            this.Controls.Add(this.panelAddPatterns);
            this.Controls.Add(this.btnMinimize);
            this.Controls.Add(this.btnMaximize);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panelSettings);
            this.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Pointillism Image Generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pbxOriginalImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxOutputImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).EndInit();
            this.panelSettings.ResumeLayout(false);
            this.panelSettings.PerformLayout();
            this.panelHeadline.ResumeLayout(false);
            this.panelHeadline.PerformLayout();
            this.panelAddPatterns.ResumeLayout(false);
            this.panelAddPatterns.PerformLayout();
            this.panelHeadline2.ResumeLayout(false);
            this.panelHeadline2.PerformLayout();
            this.panelCancle.ResumeLayout(false);
            this.panelHeadline3.ResumeLayout(false);
            this.panelHeadline3.PerformLayout();
            this.panelGeneratedImage.ResumeLayout(false);
            this.panelGeneratedImage.PerformLayout();
            this.panelHeadline4.ResumeLayout(false);
            this.panelHeadline4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbxOriginalImage;
        private System.Windows.Forms.PictureBox pbxOutputImage;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label labelPatternSize;
        private System.Windows.Forms.ComboBox comboBoxPatternSize;
        private System.Windows.Forms.Label labelOriginalImage;
        private System.Windows.Forms.Label labelGeneratedImage;
        private System.Windows.Forms.Label labelOutputImage;
        private System.Windows.Forms.Label labelNumberOfPatterns;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label labelScroll;
        private System.Windows.Forms.NumericUpDown numericUpDown;
        private System.Windows.Forms.CheckBox checkBoxProgress;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnBackgroundColor;
        private System.Windows.Forms.ColorDialog backgroundColorDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnMaximize;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Panel panelSettings;
        private System.Windows.Forms.Panel panelHeadline;
        private System.Windows.Forms.Label labelSetup;
        private System.Windows.Forms.Panel panelAddPatterns;
        private System.Windows.Forms.Panel panelHeadline2;
        private System.Windows.Forms.Label labelAddPatterns;
        private System.Windows.Forms.Panel panelCancle;
        private System.Windows.Forms.Label labelCancle;
        private System.Windows.Forms.Panel panelHeadline3;
        private System.Windows.Forms.Panel panelGeneratedImage;
        private System.Windows.Forms.Panel panelHeadline4;
        private System.Windows.Forms.Label labelResult;
    }
}

