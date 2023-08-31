using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Pointillism_image_generator;

/// <summary>
/// BitmapDataMultiThreads enables to edit different parts of Bitmap at the same time.
/// It stores a deep copy of BitmapData.
/// Call UpdateBitmap() to reflect the changes made to BitmapDataMultiThreads.
/// Editing the same part of BitmapDataThreadSafe is not thread safe, nor calling UpdateBitmap() while editing. 
/// </summary>
public class BitmapDataMultiThreads
{
    private readonly Bitmap _bitmap;
    private byte[] _data;
    /// <summary>
    /// Returns i-th byte of the bitmap.
    /// </summary>
    public byte this[int i]
    {
        get
        {
            if (_disposed) throw new ObjectDisposedException(GetType().ToString());
            return _data[i]; 
        }
        set 
        {
            if (_disposed) throw new ObjectDisposedException(GetType().ToString());
            _data[i] = value; 
            _updated = false; 
        }
    }

    private bool _updated = true;
    public readonly int Width;
    public readonly int Height; 
    public readonly int PixelSize;
    public readonly int Stride;
    private readonly Rectangle _wholeBitmap;

    private bool _disposed;

    /// <summary>
    /// Initializes BitmapDataMultiThreads.
    /// </summary>
    /// <param name="bitmap">a bitmap to edit</param>
    public BitmapDataMultiThreads(Bitmap bitmap)
    {
        _bitmap = bitmap;
        Width = _bitmap.Width;
        Height = _bitmap.Height;
        _wholeBitmap = new Rectangle(0, 0, Width, Height);
        BitmapData data = bitmap.LockBits(_wholeBitmap, ImageLockMode.ReadOnly, bitmap.PixelFormat);
        Stride = data.Stride;
        PixelSize = Image.GetPixelFormatSize(data.PixelFormat) / 8;
        _data = new byte[Stride * Height];
        Marshal.Copy(data.Scan0, _data, 0, _data.Length);
        bitmap.UnlockBits(data);
    }
    
    /// <summary>
    /// Updates original bitmap to reflect the changes made to BitmapDataMultiThreads.
    /// </summary>
    public void UpdateBitmap()
    {
        if (_disposed) throw new ObjectDisposedException(GetType().ToString());
        if (_updated)
            return;

        BitmapData data = _bitmap.LockBits(_wholeBitmap, ImageLockMode.WriteOnly, _bitmap.PixelFormat);
        Marshal.Copy(_data, 0, data.Scan0, _data.Length);
        _bitmap.UnlockBits(data);
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        _bitmap.Dispose();
        _disposed = true;
    }
}