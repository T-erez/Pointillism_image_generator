using System;
using System.Drawing;

namespace Pointillism_image_generator;

/// <summary>
/// The ColorRgb structure represents a color in RGB format. Values for each channel are stored in fields 'Red', 'Green' and 'Blue'.
/// </summary>
public struct ColorRgb
{
    public byte Red;
    public byte Green;
    public byte Blue;

    public ColorRgb(byte red, byte green, byte blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }

    /// <summary>
    /// Converts this ColorRgb structure to equivalent Color structure. 
    /// </summary>
    /// <returns>Color</returns>
    public Color ToColor() => Color.FromArgb(Red, Green, Blue);
}


/// <summary>
/// The ErrorRgb structure represents an error (difference) between the original and the generated image.
/// Error of each RGB channels is stored separately.
/// </summary>
internal struct ErrorRgb
{
    public int Red;
    public int Green;
    public int Blue;

    /// <summary>
    /// Converts this ErrorRgb structure to Error structure.
    /// </summary>
    /// <returns>Sum of errors in each RGB channel.</returns>
    public Error ToError() => new(Red + Green + Blue);
}


/// <summary>
/// The Error structure represents an error between the original and the generated image.
/// It is a sum of differences between individual RGB channels.
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


/// <summary>The RgbColorRange structure represents RGB color and contains values to find the best color for a pattern using a binary search.
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
    /// <summary>
    /// Maximum of the first half of the range.
    /// </summary>
    public ColorRgb FirstHalfMax => new (_red.FirstHalfMax, _green.FirstHalfMax, _blue.FirstHalfMax);
    /// <summary>
    /// Minimum of the second half of the range.
    /// </summary>
    public ColorRgb SecondHalfMin => new (_red.SecondHalfMin, _green.SecondHalfMin, _blue.SecondHalfMin);
    /// <summary>
    /// The best color has not been found yet.
    /// </summary>
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

        (bestColor.Red, error.Red) = _red.GetBestValue(errorFirstHalfMax.Red, errorSecondHalfMin.Red);
        (bestColor.Green, error.Green) = _green.GetBestValue(errorFirstHalfMax.Green, errorSecondHalfMin.Green);
        (bestColor.Blue, error.Blue) = _blue.GetBestValue(errorFirstHalfMax.Blue, errorSecondHalfMin.Blue);

        return (bestColor, new Error(error.Red + error.Green + error.Blue));
    }
}

/// <summary>The RgbChannelRange structure represents one RGB channel and contains values to find the best color
/// for a pattern using a binary search.
/// FirstHalfMax and SecondHalfMin are two values in the middle of the range.
/// Call method UpdateRange() to shorten the range. Finally call GetBestValue().
/// </summary>
internal struct RgbChannelRange
{
    private byte _min;
    private byte _max;

    public RgbChannelRange()
    {
        _min = 0;
        _max = 255;
    }

    /// <summary>
    /// The best value has not been found yet.
    /// </summary>
    public bool NotDone => _max - _min > 2;
    /// <summary>
    /// Maximum of the first half of the range.
    /// </summary>
    public byte FirstHalfMax => (byte) (_min + (_max - _min) / 2);
    /// <summary>
    /// Minimum of the second half of the range.
    /// </summary>
    public byte SecondHalfMin => (byte) (FirstHalfMax + 1);

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
    /// Selects the better value from a range of two values. An exception is thrown for a larger range. 
    /// </summary>
    /// <param name="errorFirstHalfMax">error of 'FirstHalfMax'</param>
    /// <param name="errorSecondHalfMin">error of 'SecondHalfMin'</param>
    /// <returns>The best value for the color channel and its error.</returns>
    /// <exception cref="InvalidOperationException">'NotDone' is true.</exception>
    public (byte, int) GetBestValue(int errorFirstHalfMax, int errorSecondHalfMin)
    {
        if (NotDone) throw new InvalidOperationException();
        
        return errorFirstHalfMax <= errorSecondHalfMin ? (_min, errorFirstHalfMax) : (_max, errorSecondHalfMin);
    }
}