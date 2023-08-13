using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Pointillism_image_generator;

/// <summary>
/// BitmapDataThreadSafe enables to edit different parts of Bitmap at the same time.
/// It stores a deep copy of BitmapData.
/// Call UpdateBitmap() to reflect the changes made to BitmapDataThreadSafe. 
/// </summary>
public class BitmapDataThreadSafe
{
    public readonly Bitmap Bitmap;
    private byte[] _data;
    public byte this[int i]
    {
        get => _data[i];
        set {_data[i] = value; 
            _updated = false; }
    }

    private bool _updated = true;
    public readonly int Width;
    public readonly int Height; 
    public readonly int PixelSize;
    public readonly int Stride;
    private readonly Rectangle _wholeBitmap;

    /// <summary>
    /// Initializes BitmapDataThreadSafe.
    /// </summary>
    /// <param name="bitmap">a bitmap to edit</param>
    public BitmapDataThreadSafe(Bitmap bitmap)
    {
        Bitmap = bitmap;
        Width = Bitmap.Width;
        Height = Bitmap.Height;
        _wholeBitmap = new Rectangle(0, 0, Width, Height);
        BitmapData data = bitmap.LockBits(_wholeBitmap, ImageLockMode.ReadOnly, bitmap.PixelFormat);
        Stride = data.Stride;
        PixelSize = Image.GetPixelFormatSize(data.PixelFormat) / 8;
        _data = new byte[Stride * Height];
        Marshal.Copy(data.Scan0, _data, 0, _data.Length);
        bitmap.UnlockBits(data);
    }
    
    /// <summary>
    /// Updates original bitmap to reflect the changes made to BitmapDataThreadSafe.
    /// </summary>
    public void UpdateBitmap()
    {
        if (_updated)
            return;

        BitmapData data = Bitmap.LockBits(_wholeBitmap, ImageLockMode.WriteOnly, Bitmap.PixelFormat);
        Marshal.Copy(_data, 0, data.Scan0, _data.Length);
        Bitmap.UnlockBits(data);
    }
}