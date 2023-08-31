
namespace Pointillism_image_generator
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.checkBoxProgress = new System.Windows.Forms.CheckBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelProgress = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnBackgroundColor = new System.Windows.Forms.Button();
            this.backgroundColorDialog = new System.Windows.Forms.ColorDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pbxOriginalImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxOutputImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // pbxOriginalImage
            // 
            this.pbxOriginalImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxOriginalImage.Location = new System.Drawing.Point(79, 43);
            this.pbxOriginalImage.Name = "pbxOriginalImage";
            this.pbxOriginalImage.Size = new System.Drawing.Size(492, 432);
            this.pbxOriginalImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxOriginalImage.TabIndex = 0;
            this.pbxOriginalImage.TabStop = false;
            // 
            // pbxOutputImage
            // 
            this.pbxOutputImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxOutputImage.Location = new System.Drawing.Point(750, 43);
            this.pbxOutputImage.Name = "pbxOutputImage";
            this.pbxOutputImage.Size = new System.Drawing.Size(492, 432);
            this.pbxOutputImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxOutputImage.TabIndex = 1;
            this.pbxOutputImage.TabStop = false;
            // 
            // btnLoad
            // 
            this.btnLoad.BackColor = System.Drawing.Color.RosyBrown;
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoad.Location = new System.Drawing.Point(62, 488);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(100, 50);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = false;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.RosyBrown;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(1187, 488);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 50);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.RosyBrown;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Location = new System.Drawing.Point(453, 487);
            this.btnStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 50);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // labelPatternSize
            // 
            this.labelPatternSize.AutoSize = true;
            this.labelPatternSize.Location = new System.Drawing.Point(222, 487);
            this.labelPatternSize.Name = "labelPatternSize";
            this.labelPatternSize.Size = new System.Drawing.Size(163, 21);
            this.labelPatternSize.TabIndex = 6;
            this.labelPatternSize.Text = "Select pattern size";
            // 
            // comboBoxPatternSize
            // 
            this.comboBoxPatternSize.BackColor = System.Drawing.Color.RosyBrown;
            this.comboBoxPatternSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPatternSize.FormattingEnabled = true;
            this.comboBoxPatternSize.Location = new System.Drawing.Point(222, 508);
            this.comboBoxPatternSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxPatternSize.Name = "comboBoxPatternSize";
            this.comboBoxPatternSize.Size = new System.Drawing.Size(178, 29);
            this.comboBoxPatternSize.TabIndex = 7;
            // 
            // labelOriginalImage
            // 
            this.labelOriginalImage.AutoSize = true;
            this.labelOriginalImage.Location = new System.Drawing.Point(79, 406);
            this.labelOriginalImage.Name = "labelOriginalImage";
            this.labelOriginalImage.Size = new System.Drawing.Size(129, 21);
            this.labelOriginalImage.TabIndex = 9;
            this.labelOriginalImage.Text = "Original image";
            // 
            // labelGeneratedImage
            // 
            this.labelGeneratedImage.AutoSize = true;
            this.labelGeneratedImage.Location = new System.Drawing.Point(1118, 406);
            this.labelGeneratedImage.Name = "labelGeneratedImage";
            this.labelGeneratedImage.Size = new System.Drawing.Size(161, 21);
            this.labelGeneratedImage.TabIndex = 10;
            this.labelGeneratedImage.Text = "Generated image";
            // 
            // labelOutputImage
            // 
            this.labelOutputImage.AutoSize = true;
            this.labelOutputImage.Location = new System.Drawing.Point(750, 406);
            this.labelOutputImage.Name = "labelOutputImage";
            this.labelOutputImage.Size = new System.Drawing.Size(183, 21);
            this.labelOutputImage.TabIndex = 11;
            this.labelOutputImage.Text = "Number of patterns: ";
            // 
            // labelNumberOfPatterns
            // 
            this.labelNumberOfPatterns.AutoSize = true;
            this.labelNumberOfPatterns.Location = new System.Drawing.Point(923, 406);
            this.labelNumberOfPatterns.Name = "labelNumberOfPatterns";
            this.labelNumberOfPatterns.Size = new System.Drawing.Size(19, 21);
            this.labelNumberOfPatterns.TabIndex = 12;
            this.labelNumberOfPatterns.Text = "0";
            // 
            // trackBar
            // 
            this.trackBar.BackColor = System.Drawing.Color.RosyBrown;
            this.trackBar.Location = new System.Drawing.Point(935, 488);
            this.trackBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackBar.Maximum = 20;
            this.trackBar.Minimum = 1;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(201, 56);
            this.trackBar.TabIndex = 13;
            this.trackBar.Value = 1;
            this.trackBar.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(725, 487);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 21);
            this.label1.TabIndex = 15;
            this.label1.Text = "Add patterns";
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.RosyBrown;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Location = new System.Drawing.Point(831, 487);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 50);
            this.btnAdd.TabIndex = 16;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(944, 470);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 21);
            this.label2.TabIndex = 17;
            this.label2.Text = "View progress";
            // 
            // numericUpDown
            // 
            this.numericUpDown.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown.Location = new System.Drawing.Point(725, 507);
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
            this.numericUpDown.Size = new System.Drawing.Size(101, 27);
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
            this.checkBoxProgress.Location = new System.Drawing.Point(697, 538);
            this.checkBoxProgress.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxProgress.Name = "checkBoxProgress";
            this.checkBoxProgress.Size = new System.Drawing.Size(148, 25);
            this.checkBoxProgress.TabIndex = 20;
            this.checkBoxProgress.Text = "Save progress";
            this.checkBoxProgress.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            this.progressBar.ForeColor = System.Drawing.Color.Coral;
            this.progressBar.Location = new System.Drawing.Point(419, 594);
            this.progressBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(501, 19);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 21;
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(597, 573);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(0, 21);
            this.labelProgress.TabIndex = 22;
            this.labelProgress.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.RosyBrown;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(831, 538);
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
            this.btnBackgroundColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackgroundColor.Location = new System.Drawing.Point(222, 542);
            this.btnBackgroundColor.Name = "btnBackgroundColor";
            this.btnBackgroundColor.Size = new System.Drawing.Size(178, 55);
            this.btnBackgroundColor.TabIndex = 24;
            this.btnBackgroundColor.Text = "Change background color";
            this.btnBackgroundColor.UseVisualStyleBackColor = true;
            this.btnBackgroundColor.Click += new System.EventHandler(this.btnBackgroundColor_Click);
            // 
            // backgroundColorDialog
            // 
            this.backgroundColorDialog.AnyColor = true;
            this.backgroundColorDialog.Color = System.Drawing.Color.White;
            this.backgroundColorDialog.FullOpen = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1307, 645);
            this.Controls.Add(this.btnBackgroundColor);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.labelProgress);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.checkBoxProgress);
            this.Controls.Add(this.numericUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.labelNumberOfPatterns);
            this.Controls.Add(this.labelOutputImage);
            this.Controls.Add(this.labelGeneratedImage);
            this.Controls.Add(this.labelOriginalImage);
            this.Controls.Add(this.comboBoxPatternSize);
            this.Controls.Add(this.labelPatternSize);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.pbxOutputImage);
            this.Controls.Add(this.pbxOriginalImage);
            this.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Pointillism Image Generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pbxOriginalImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxOutputImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown;
        private System.Windows.Forms.CheckBox checkBoxProgress;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnBackgroundColor;
        private System.Windows.Forms.ColorDialog backgroundColorDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}

