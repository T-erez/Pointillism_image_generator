using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pointillism_image_generator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
            // Generate pointillism image

            if (pbxOriginalImage.Image == null)
            {
                MessageBox.Show("Error. First load an input image.");
            }


        }
    }
}
