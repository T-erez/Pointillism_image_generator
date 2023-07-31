using System;
using System.Drawing;

namespace Pointillism_image_generator;
#nullable enable

/// <summary>A structure for patterns, that will be pasted in the generated image. Patterns are squares.</summary>
public struct SquarePattern
{
    public readonly int XIndex;
    public readonly int YIndex;
    public ColorRgb ColorRgb;
    public int Angle;

    /// <summary>Initializes pattern properties.</summary>
    /// <param name="xIndex">x-coordinate of the centre of the pattern in the generated image</param>
    /// <param name="yIndex">y-coordinate of the centre of the pattern in the generated image</param>
    /// <param name="colorRgb">color in RGB format</param>
    /// <param name="angle">angle of rotation about the centre</param>
    public SquarePattern(int xIndex, int yIndex, ColorRgb colorRgb, int angle)
    {
        XIndex = xIndex;
        YIndex = yIndex;
        ColorRgb = colorRgb;
        Angle = angle;
    }
}

/// <summary>
/// Class PatternNode represents a pattern and its improvement. Improvement defines, which pattern is better. 
/// The pattern error must be stored to calculate the improvement.
/// The value backgroundContribution is not used in other parts of the program, but it could be used to cover the background first.
/// Class implements IComparable interface.
/// </summary>
public class PatternNode : IComparable<PatternNode>, IUpdatable<PatternNode>, IHasId<(int, int)>
{
    public readonly SquarePattern SquarePattern;
    public int Improvement; //improvement of generated image after pattern is added
    //public readonly int BackgroundContribution; //number of background pixels in the generated image covered by pattern
    public readonly Error Error;
    public (int, int) Id => (SquarePattern.XIndex, SquarePattern.YIndex);

    /// <summary>Initializes properties of Node.</summary>
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

    public override int GetHashCode() => (xIndex: SquarePattern.XIndex, yIndex: SquarePattern.YIndex, Improvement).GetHashCode();
}
