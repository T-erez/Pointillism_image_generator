using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Pointillism_image_generator;

/// <summary>
/// The Subimages class helps manage adding multiple patterns to an image at once.
/// It represents a set of subimages for an image.
/// Subimages are divided into Groups. It is safe to add patterns in parallel to all subimages of one group.
/// </summary>
internal class Subimages
{
    private Subimage[,] _data;
    private readonly int _subimageSize;
    private readonly Rectangle _region;
    private readonly int _subimagesPerRow;
    private readonly int _subimagesPerColumn;
    public readonly Group[] Groups;
    public int Count => _subimagesPerRow * _subimagesPerColumn;

    /// <summary>
    /// Initializes Subimages.
    /// </summary>
    /// <param name="region">region of an image for which subimages are to be created</param>
    /// <param name="subimageSize">width of subimage, subimage is a square</param>
    /// <param name="windowSize">window size of a pattern</param>
    /// <param name="patternsPerSubimage">maximum number of patterns in each subimage</param>
    /// <exception cref="ArgumentException">Exception is thrown when subimage size is smaller than window size.</exception>
    public Subimages(Rectangle region, int subimageSize, int windowSize, int patternsPerSubimage)
    {
        if (subimageSize < windowSize)
            throw new ArgumentException();
        
        _region = region;
        _subimageSize = subimageSize;
        _subimagesPerRow = DivideRoundingUp(region.Width, subimageSize);
        _subimagesPerColumn = DivideRoundingUp(region.Height, subimageSize);
        _data = new Subimage[_subimagesPerRow,_subimagesPerColumn];
        for (int i = 0; i < _subimagesPerRow; i++)
        {
            for (int j = 0; j < _subimagesPerColumn; j++)
            {
                _data[i, j] = new Subimage(patternsPerSubimage);
            }
        }
        Groups = new Group[] {
            new(GetGroupEnumerator(GroupNumber.One)),
            new(GetGroupEnumerator(GroupNumber.Two)),
            new(GetGroupEnumerator(GroupNumber.Three)),
            new(GetGroupEnumerator(GroupNumber.Four))
        };
    }
    
    /// <summary>
    /// Converts the image point to the subimage.
    /// </summary>
    /// <returns>Subimage where the point is located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Point is outside of the bounds of the image.</exception>
    public Subimage GetSubimage(Point point)
    {
        if (point.X < _region.X || point.X >= _region.X + _region.Width ||
            point.Y < _region.Y || point.Y >= _region.Y + _region.Height)
            throw new ArgumentOutOfRangeException();
        
        int xSubimageIndex = (point.X - _region.X) / _subimageSize;
        int ySubimageIndex = (point.Y - _region.Y) / _subimageSize;
        return _data[xSubimageIndex, ySubimageIndex];
    }

    /// <summary>
    /// Divides two positive integers rounding up.
    /// </summary>
    /// <returns></returns>
    private int DivideRoundingUp(int dividend, int divisor) => (dividend + divisor - 1) / divisor;

    /// <summary>
    /// Number of a group. There are four groups.
    /// </summary>
    private enum GroupNumber
    {
        /// <summary>
        /// Top left group.
        /// </summary>
        One,
        /// <summary>
        /// Top right group.
        /// </summary>
        Two,
        /// <summary>
        /// Bottom left group.
        /// </summary>
        Three,
        /// <summary>
        /// Bottom right group.
        /// </summary>
        Four
    }
    
    private IEnumerable<Subimage> GetGroupEnumerator(GroupNumber number)
    {
        int startRow = 0;
        int startColumn = 0;
        switch (number)
        {
            case GroupNumber.One:
                break;
            case GroupNumber.Two:
                startColumn = 1;
                break;
            case GroupNumber.Three:
                startRow = 1;
                break;
            case GroupNumber.Four:
                startRow = 1;
                startColumn = 1;
                break;
        }
        for (int j = startColumn; j < _subimagesPerColumn; j += 2)
        {
            for (int i = startRow; i < _subimagesPerRow; i += 2)
            {
                yield return _data[i, j];
            }
        }
    }

    /// <summary>
    /// Group of subimages. It is safe to add patterns in parallel to all subimages of one group.
    /// </summary>
    internal struct Group : IEnumerable<Subimage>
    {
        private IEnumerable<Subimage> _subimages;

        public Group(IEnumerable<Subimage> subimages) => _subimages = subimages;
        public IEnumerator<Subimage> GetEnumerator() => _subimages.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}


/// <summary>
/// The Subimage structure represents a square part of an image. It stores patterns that can be added to the subimage.
/// </summary>
internal struct Subimage
{
    public SmartHeap<PatternWithImprovement, Point> Patterns;

    public Subimage()
    {
        Patterns = new SmartHeap<PatternWithImprovement, Point>();
    }

    public Subimage(int numberOfPatterns)
    {
        Patterns = new SmartHeap<PatternWithImprovement, Point>(numberOfPatterns);
    }
}
