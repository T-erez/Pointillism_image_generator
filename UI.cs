using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pointillism_image_generator;

#nullable enable

public static class UI
{
    /// <summary>
    /// Loads image from the user.
    /// </summary>
    /// <param name="filePath">filepath is valid if image is not null</param>
    /// <returns>Returns null on failure, otherwise returns an image.</returns>
    public static Image? LoadImage(out string? filePath)
    {
        filePath = null;
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "Image Files(*.JPG;*.PNG,*.BMP)|*.JPG;*.PNG;*.BMP|All files (*.*)|*.*";

        if (ofd.ShowDialog() != DialogResult.OK)
            return null;
                
        filePath = ofd.FileName;
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
} 