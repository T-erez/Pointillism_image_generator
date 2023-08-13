using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Pointillism_image_generator;

internal class Subimages
{
    private Subimage[][] _data;
    /// <summary>
    /// Translates image indices to subimage.
    /// </summary>
    /// <param name="x">x-index of pixel in region</param>
    /// <param name="y">y-index of pixel in region</param>
    public Subimage this[int x, int y] => GetSubimage(x, y);
    private readonly int _subimageSize;
    private readonly Rectangle _region;
    public Group[] Groups;

    public Subimages(Rectangle region, int subimageSize)
    {
        _region = region;
        _subimageSize = subimageSize;
        int subimagesPerRow = DivideRoundingUp(region.Width, subimageSize);
        _data = new Subimage[subimagesPerRow][];
        int subimagesPerColumn = DivideRoundingUp(region.Height, subimageSize);
        for (int i = 0; i < subimagesPerRow; i++)
        {
            _data[i] = new Subimage[subimagesPerColumn];
            for (int j = 0; j < _data[i].Length; j++)
            {
                _data[i][j] = new Subimage();
            }
        }
        Groups = new Group[] {
            new Group1(_data, subimagesPerRow, subimagesPerColumn), 
            new Group2(_data, subimagesPerRow, subimagesPerColumn), 
            new Group3(_data, subimagesPerRow, subimagesPerColumn), 
            new Group4(_data, subimagesPerRow, subimagesPerColumn)};
    }
    
    public Subimage GetSubimage(int xImageIndex, int yImageIndex)
    {
        if (xImageIndex < _region.X || xImageIndex > _region.X + _region.Width ||
            yImageIndex < _region.Y || yImageIndex > _region.Y + _region.Height)
            throw new ArgumentOutOfRangeException();
        
        int xSubimageIndex = xImageIndex == _region.X ? 0 : DivideRoundingUp(xImageIndex - _region.X, _subimageSize) - 1;
        int ySubimageIndex = yImageIndex == _region.Y ? 0 : DivideRoundingUp(yImageIndex - _region.Y, _subimageSize) - 1;
        return _data[xSubimageIndex][ySubimageIndex];
    }

    /// <summary>
    /// Divides two positive integers rounding up.
    /// </summary>
    /// <param name="x">dividend</param>
    /// <param name="y">divisor</param>
    /// <returns></returns>
    private int DivideRoundingUp(int x, int y) => (x + y - 1) / y;
}


public struct Subimage
{
    public SmartHeap<PatternNode, Point> Patterns;

    public Subimage(int patternsPerSubimage)
    {
        Patterns = new SmartHeap<PatternNode, Point>(patternsPerSubimage);
    }

    public Subimage()
    {
        Patterns = new SmartHeap<PatternNode, Point>();
    }
}