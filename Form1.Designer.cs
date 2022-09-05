
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
            this.btnLoad.Location = new System.Drawing.Point(62, 575);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(94, 51);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = false;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.RosyBrown;
            this.btnSave.Location = new System.Drawing.Point(1187, 575);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 51);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.RosyBrown;
            this.btnStart.Location = new System.Drawing.Point(453, 572);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(94, 51);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // labelPatternSize
            // 
            this.labelPatternSize.AutoSize = true;
            this.labelPatternSize.Location = new System.Drawing.Point(222, 574);
            this.labelPatternSize.Name = "labelPatternSize";
            this.labelPatternSize.Size = new System.Drawing.Size(130, 20);
            this.labelPatternSize.TabIndex = 6;
            this.labelPatternSize.Text = "Select pattern size";
            // 
            // comboBoxPatternSize
            // 
            this.comboBoxPatternSize.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.comboBoxPatternSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPatternSize.FormattingEnabled = true;
            this.comboBoxPatternSize.Location = new System.Drawing.Point(222, 597);
            this.comboBoxPatternSize.Name = "comboBoxPatternSize";
            this.comboBoxPatternSize.Size = new System.Drawing.Size(151, 28);
            this.comboBoxPatternSize.TabIndex = 7;
            // 
            // labelOriginalImage
            // 
            this.labelOriginalImage.AutoSize = true;
            this.labelOriginalImage.Location = new System.Drawing.Point(79, 478);
            this.labelOriginalImage.Name = "labelOriginalImage";
            this.labelOriginalImage.Size = new System.Drawing.Size(108, 20);
            this.labelOriginalImage.TabIndex = 9;
            this.labelOriginalImage.Text = "Original image";
            // 
            // labelGeneratedImage
            // 
            this.labelGeneratedImage.AutoSize = true;
            this.labelGeneratedImage.Location = new System.Drawing.Point(1118, 478);
            this.labelGeneratedImage.Name = "labelGeneratedImage";
            this.labelGeneratedImage.Size = new System.Drawing.Size(124, 20);
            this.labelGeneratedImage.TabIndex = 10;
            this.labelGeneratedImage.Text = "Generated image";
            // 
            // labelOutputImage
            // 
            this.labelOutputImage.AutoSize = true;
            this.labelOutputImage.Location = new System.Drawing.Point(750, 478);
            this.labelOutputImage.Name = "labelOutputImage";
            this.labelOutputImage.Size = new System.Drawing.Size(146, 20);
            this.labelOutputImage.TabIndex = 11;
            this.labelOutputImage.Text = "Number of patterns: ";
            // 
            // labelNumberOfPatterns
            // 
            this.labelNumberOfPatterns.AutoSize = true;
            this.labelNumberOfPatterns.Location = new System.Drawing.Point(902, 478);
            this.labelNumberOfPatterns.Name = "labelNumberOfPatterns";
            this.labelNumberOfPatterns.Size = new System.Drawing.Size(17, 20);
            this.labelNumberOfPatterns.TabIndex = 12;
            this.labelNumberOfPatterns.Text = "0";
            // 
            // trackBar
            // 
            this.trackBar.BackColor = System.Drawing.Color.RosyBrown;
            this.trackBar.Location = new System.Drawing.Point(935, 575);
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
            this.label1.Location = new System.Drawing.Point(725, 573);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 20);
            this.label1.TabIndex = 15;
            this.label1.Text = "Add patterns";
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.RosyBrown;
            this.btnAdd.Location = new System.Drawing.Point(831, 572);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(60, 54);
            this.btnAdd.TabIndex = 16;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(944, 552);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 20);
            this.label2.TabIndex = 17;
            this.label2.Text = "View progress";
            // 
            // numericUpDown
            // 
            this.numericUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown.Location = new System.Drawing.Point(725, 596);
            this.numericUpDown.Maximum = new decimal(new int[] {
            9500,
            0,
            0,
            0});
            this.numericUpDown.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown.Name = "numericUpDown";
            this.numericUpDown.Size = new System.Drawing.Size(100, 27);
            this.numericUpDown.TabIndex = 19;
            this.numericUpDown.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // checkBoxProgress
            // 
            this.checkBoxProgress.AutoSize = true;
            this.checkBoxProgress.Location = new System.Drawing.Point(443, 629);
            this.checkBoxProgress.Name = "checkBoxProgress";
            this.checkBoxProgress.Size = new System.Drawing.Size(123, 24);
            this.checkBoxProgress.TabIndex = 20;
            this.checkBoxProgress.Text = "Save progress";
            this.checkBoxProgress.UseVisualStyleBackColor = true;
            this.checkBoxProgress.CheckedChanged += new System.EventHandler(this.checkBoxProgress_CheckedChanged);
            // 
            // progressBar
            // 
            this.progressBar.ForeColor = System.Drawing.Color.Coral;
            this.progressBar.Location = new System.Drawing.Point(419, 698);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(500, 23);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 21;
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(597, 675);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(0, 20);
            this.labelProgress.TabIndex = 22;
            this.labelProgress.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1326, 733);
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
            this.Name = "Form1";
            this.Text = "Pointillism Image Generator";
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
    }
}

