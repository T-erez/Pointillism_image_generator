using System.Drawing;

namespace Pointillism_image_generator;

/// <summary>
/// The IntReference class stores one int value that can be shared in multiple threads. 
/// </summary>
public class IntReference
{
    public int Value;

    public IntReference(int value) => Value = value;
}


public static class IntExtensions
{
    /// <summary>
    /// Converts this int value to an equivalent instance of IntReference class.
    /// </summary>
    public static IntReference ToIntReference(this int value)
    {
        return new IntReference(value);
    }
}
