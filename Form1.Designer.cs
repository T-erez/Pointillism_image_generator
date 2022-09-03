
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
            this.comboBoxPatternSize = new System.Windows.Forms.ComboBox();
            this.btnStart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbxOriginalImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxOutputImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pbxOriginalImage
            // 
            this.pbxOriginalImage.Location = new System.Drawing.Point(67, 43);
            this.pbxOriginalImage.Name = "pbxOriginalImage";
            this.pbxOriginalImage.Size = new System.Drawing.Size(492, 432);
            this.pbxOriginalImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxOriginalImage.TabIndex = 0;
            this.pbxOriginalImage.TabStop = false;
            // 
            // pbxOutputImage
            // 
            this.pbxOutputImage.Location = new System.Drawing.Point(661, 43);
            this.pbxOutputImage.Name = "pbxOutputImage";
            this.pbxOutputImage.Size = new System.Drawing.Size(492, 432);
            this.pbxOutputImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxOutputImage.TabIndex = 1;
            this.pbxOutputImage.TabStop = false;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(67, 597);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(94, 29);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(1059, 597);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 29);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // comboBoxPatternSize
            // 
            this.comboBoxPatternSize.FormattingEnabled = true;
            this.comboBoxPatternSize.Location = new System.Drawing.Point(260, 597);
            this.comboBoxPatternSize.Name = "comboBoxPatternSize";
            this.comboBoxPatternSize.Size = new System.Drawing.Size(151, 28);
            this.comboBoxPatternSize.TabIndex = 4;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(554, 596);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(94, 29);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1230, 741);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.comboBoxPatternSize);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.pbxOutputImage);
            this.Controls.Add(this.pbxOriginalImage);
            this.Name = "Form1";
            this.Text = "Pointillism Image Generator";
            ((System.ComponentModel.ISupportInitialize)(this.pbxOriginalImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxOutputImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbxOriginalImage;
        private System.Windows.Forms.PictureBox pbxOutputImage;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox comboBoxPatternSize;
        private System.Windows.Forms.Button btnStart;
    }
}

