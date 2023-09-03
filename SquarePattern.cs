using System;
using System.Drawing;

namespace Pointillism_image_generator;
#nullable enable

/// <summary>
/// A structure for patterns, that will be pasted in the generated image.
/// Patterns are squares and can be rotated about the centre.
/// </summary>
public struct SquarePattern
{
    public readonly Point Centre;
    public ColorRgb ColorRgb;
    public int Angle;

    /// <summary>Initializes pattern.</summary>
    /// <param name="centre">the centre of the pattern in the generated image</param>
    /// <param name="colorRgb">color in RGB format</param>
    /// <param name="angle">angle of rotation about the centre</param>
    public SquarePattern(Point centre, ColorRgb colorRgb, int angle)
    {
        Centre = centre;
        ColorRgb = colorRgb;
        Angle = angle;
    }
}


/// <summary>
/// The PatternWithImprovement structure represents a pattern and its improvement. Improvement defines, which pattern is better. 
/// The pattern error must be stored to calculate the improvement.
/// Structure implements IComparable interface.
/// </summary>
public struct PatternWithImprovement : IComparable<PatternWithImprovement>, IUpdatable<PatternWithImprovement>, IHasId<Point>
{
    public readonly SquarePattern SquarePattern;
    /// <summary>
    /// Improvement of the generated image after pattern is added.
    /// </summary>
    public int Improvement;
    public readonly Error Error;
    public Point Id => SquarePattern.Centre;

    /// <summary>Initializes PatternWithImprovement.</summary>
    /// <param name="squarePattern">a square pattern</param>
    /// <param name="error">difference between the window size region in the output image with pattern and
    /// the corresponding region in the original image</param>
    public PatternWithImprovement(SquarePattern squarePattern, Error error)
    {
        SquarePattern = squarePattern;
        Error = error;
    }
    
    public void Update(PatternWithImprovement oldOne)
    {
        Improvement = (oldOne.Error - Error).ToInt();
    }

    public int CompareTo(PatternWithImprovement other)
    {
        return Improvement.CompareTo(other.Improvement);
    }
}
