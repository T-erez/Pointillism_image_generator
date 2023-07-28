using System;

namespace Pointillism_image_generator;

/// <summary>
/// ColorArgb represents a color in 32-bit ARGB format. 
/// </summary>
public struct ColorArgb
{
    public int Value;

    public ColorArgb(int value) => Value = value;
    public int ToInt() => Value;
}


/// <summary>
/// ColorRgb represents a color in RGB format. Values for each channel are stored in fields 'Red', 'Green' and 'Blue'.
/// </summary>
internal struct ColorRgb
{
    public int Red;
    public int Green;
    public int Blue;

    public ColorRgb(int red, int green, int blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }

    /// <summary>Returns a color in ARGB format. Alpha channel is set to 255.</summary>
    /// <returns>The color in ARGB format.</returns>
    public ColorArgb ToArgb() => new ColorArgb(255 << 24 | Red << 16 | Green << 8 | Blue);
}


/// <summary>
/// ErrorRgb represents an error between the original and the generated image.
/// The differences between individual RGB channels are stored separately.
/// </summary>
internal struct ErrorRgb
{
    public int Red;
    public int Green;
    public int Blue;
}


/// <summary>
/// Error represents an error between the original and the generated image.
/// It is the sum of differences between individual RGB channels.
/// </summary>
public struct Error : IComparable<Error>
{
    public int Value;

    public Error(int value) => Value = value;

    public static implicit operator Error(int value) => new (value);
    public int ToInt() => Value;

    public int CompareTo(Error other) => Value.CompareTo(other.Value);

    public static bool operator <(Error left, Error right) => left.CompareTo(right) < 0;

    public static bool operator >(Error left, Error right) => left.CompareTo(right) > 0;

    public static bool operator <=(Error left, Error right) => left.CompareTo(right) <= 0;

    public static bool operator >=(Error left, Error right) => left.CompareTo(right) >= 0;

    public static Error operator +(Error left, Error right) => new(left.Value + right.Value);

    public static Error operator -(Error left, Error right) => new(left.Value - right.Value);
}


/// <summary>A structure RgbColorRange represents rgb color and contains values to find the best color
/// for a pattern using a binary search.
/// FirstHalfMax and SecondHalfMin are two values in the middle of the range.
/// Call method UpdateRange() to shorten the range. Finally call GetBestColor().
/// </summary>
internal struct RgbColorRange
{
    private RgbChannelRange _red;
    private RgbChannelRange _green;
    private RgbChannelRange _blue;

    public RgbColorRange()
    {
        _red = new();
        _green = new();
        _blue = new();
    }
    public ColorRgb FirstHalfMax => new (_red.FirstHalfMax, _green.FirstHalfMax, _blue.FirstHalfMax);
    public ColorRgb SecondHalfMin => new (_red.SecondHalfMin, _green.SecondHalfMin, _blue.SecondHalfMin);
    public bool NotDone => _red.NotDone || _green.NotDone || _blue.NotDone;
    
    /// <summary>
    /// Halves the range of each color channel based on which half has the smaller error.
    /// </summary>
    /// <param name="errorFirstHalfMax">error of 'FirstHalfMax'</param>
    /// <param name="errorSecondHalfMin">error of 'SecondHalfMin'</param>
    public void UpdateRange(ErrorRgb errorFirstHalfMax, ErrorRgb errorSecondHalfMin)
    {
        _red.UpdateRange(errorFirstHalfMax.Red, errorSecondHalfMin.Red);
        _green.UpdateRange(errorFirstHalfMax.Green, errorSecondHalfMin.Green);
        _blue.UpdateRange(errorFirstHalfMax.Blue, errorSecondHalfMin.Blue);
    }

    /// <summary>
    /// Selects the better color from a range of two values for each color channel. An exception is thrown for a larger range. 
    /// </summary>
    /// <param name="errorFirstHalfMax">error of 'FirstHalfMax'</param>
    /// <param name="errorSecondHalfMin">error of 'SecondHalfMin'</param>
    /// <returns>The best color in RGB format and its error.</returns>
    /// <exception cref="InvalidOperationException">'NotDone' is true.</exception>
    public (ColorRgb,Error) GetBestColor(ErrorRgb errorFirstHalfMax, ErrorRgb errorSecondHalfMin)
    {
        ColorRgb bestColor = new();
        ErrorRgb error = new();

        (bestColor.Red, error.Red) = _red.GetBestColor(errorFirstHalfMax.Red, errorSecondHalfMin.Red);
        (bestColor.Green, error.Green) = _green.GetBestColor(errorFirstHalfMax.Green, errorSecondHalfMin.Green);
        (bestColor.Blue, error.Blue) = _blue.GetBestColor(errorFirstHalfMax.Blue, errorSecondHalfMin.Blue);

        return (bestColor, new Error(error.Red + error.Green + error.Blue));
    }
}

/// <summary>A structure RgbChannelRange represents one rgb channel and contains values to find the best color
/// for a pattern using a binary search.
/// FirstHalfMax and SecondHalfMin are two values in the middle of the range.
/// Call method UpdateRange() to shorten the range. Finally call GetBestColor().
/// </summary>
internal struct RgbChannelRange
{
    private int _min;
    private int _max;

    public RgbChannelRange()
    {
        _min = 0;
        _max = 255;
    }

    public bool NotDone => _max - _min > 2; 
    public int FirstHalfMax => _min + (_max - _min) / 2;
    public int SecondHalfMin => FirstHalfMax + 1;

    /// <summary>
    /// Halves the range based on which half has the smaller error.
    /// </summary>
    /// <param name="errorFirstHalfMax">error of 'FirstHalfMax'</param>
    /// <param name="errorSecondHalfMin">error of 'SecondHalfMin'</param>
    public void UpdateRange(int errorFirstHalfMax, int errorSecondHalfMin)
    {
        if (errorFirstHalfMax <= errorSecondHalfMin)
            _max = SecondHalfMin;

        if (errorFirstHalfMax >= errorSecondHalfMin)
            _min = FirstHalfMax;
    }

    /// <summary>
    /// Selects the better color from a range of two values. An exception is thrown for a larger range. 
    /// </summary>
    /// <param name="errorFirstHalfMax">error of 'FirstHalfMax'</param>
    /// <param name="errorSecondHalfMin">error of 'SecondHalfMin'</param>
    /// <returns>The best value for the color channel and its error.</returns>
    /// <exception cref="InvalidOperationException">'NotDone' is true.</exception>
    public (int, int) GetBestColor(int errorFirstHalfMax, int errorSecondHalfMin)
    {
        if (NotDone) throw new InvalidOperationException();
        
        return errorFirstHalfMax <= errorSecondHalfMin ? (_min, errorFirstHalfMax) : (_max, errorSecondHalfMin);
    }
}