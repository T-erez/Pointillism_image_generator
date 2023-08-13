using System;
using System.Drawing;

namespace Pointillism_image_generator;
#nullable enable

/// <summary>A structure for patterns, that will be pasted in the generated image. Patterns are squares.</summary>
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
/// Class *** represents a pattern and its improvement. Improvement defines, which pattern is better. 
/// The pattern error must be stored to calculate the improvement.
/// Class implements IComparable interface.
/// </summary>
public class PatternNode : IComparable<PatternNode>, IUpdatable<PatternNode>, IHasId<Point>
{
    public readonly SquarePattern SquarePattern;
    /// <summary>
    /// Improvement of generated image after pattern is added.
    /// </summary>
    public int Improvement;
    /// The value backgroundContribution is not used in other parts of the program, but it could be used to cover the background first.
    //public readonly int BackgroundContribution; //number of background pixels in the generated image covered by pattern
    public readonly Error Error;
    public Point Id => SquarePattern.Centre;

    /// <summary>Initializes ***.</summary>
    /// <param name="squarePattern">a pattern stored in a node</param>
    /// <param name="error">difference between the window size region in the output image with pattern and
    /// the corresponding region in the original image</param>
    public PatternNode(SquarePattern squarePattern, Error error)
    {
        SquarePattern = squarePattern;
        Error = error;
    }
    
    public void Update(PatternNode oldOne)
    {
        Improvement = (oldOne.Error - Error).ToInt();
    }

    public int CompareTo(PatternNode? otherNode)
    {
        if (otherNode is null)
            return 1;

        // int higherPriority = BackgroundContribution.CompareTo(otherNode.BackgroundContribution);
        // switch (higherPriority)
        // {
        //     case -1:
        //         return -1;
        //     case 0:
        //         return Improvement.CompareTo(otherNode.Improvement);
        //     default:
        //         return 1;
        // }
        return Improvement.CompareTo(otherNode.Improvement);
    }

    public override bool Equals(object? obj) => Equals(obj as PatternNode);

    private bool Equals(PatternNode? otherNode)
    {
        if (otherNode is null)
            return false;

        if (GetType() != otherNode.GetType())
            return false;

        return (Improvement == otherNode.Improvement) /*&& (BackgroundContribution == otherNode.BackgroundContribution)*/;
    }


    public static bool operator ==(PatternNode? leftNode, PatternNode? rightNode)
    {
        if (leftNode is not null) 
            return leftNode.Equals(rightNode);
        
        return rightNode is null;
    }

    public static bool operator !=(PatternNode? leftNode, PatternNode? rightNode) => !(leftNode == rightNode);

    public static bool operator <(PatternNode? leftNode, PatternNode? rightNode)
    {
        if (leftNode == null || rightNode == null)
            return false;
        
        // if (leftNode.BackgroundContribution < rightNode.BackgroundContribution)
        //     return true;
        //
        // if (leftNode.BackgroundContribution != rightNode.BackgroundContribution) 
        //     return false;
        
        return leftNode.Improvement < rightNode.Improvement;
    }

    public static bool operator >(PatternNode leftNode, PatternNode rightNode) => !(leftNode < rightNode | leftNode == rightNode);

    public override int GetHashCode() => (Index: SquarePattern.Centre, Improvement).GetHashCode();
}
