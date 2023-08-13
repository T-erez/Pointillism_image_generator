using System.Collections;
using System.Collections.Generic;

namespace Pointillism_image_generator;

internal abstract class Group :IEnumerable<Subimage>
{
    private readonly Subimage[][] _subimages;
    private readonly int _subimagesPerRow;
    private readonly int _subimagesPerColumn;
    protected int StartRow;
    protected int StartColumn;

    protected Group(Subimage[][] subimages, int subimagesPerRow, int subimagesPerColumn)
    {
        _subimages = subimages;
        _subimagesPerRow = subimagesPerRow;
        _subimagesPerColumn = subimagesPerColumn;
    }

    public IEnumerator<Subimage> GetEnumerator()
    {
        for (int j = StartColumn; j < _subimagesPerColumn; j += 2)
        {
            for (int i = StartRow; i < _subimagesPerRow; i += 2)
            {
                yield return _subimages[i][j];
            }
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

internal class Group1 : Group
{
    public Group1(Subimage[][] subimages, int subimagesPerRow, int subimagesPerColumn) :base(subimages, subimagesPerRow, subimagesPerColumn)
    {
        StartRow = 0;
        StartColumn = 0;
    }
}

internal class Group2 : Group
{
    public Group2(Subimage[][] subimages, int subimagesPerRow, int subimagesPerColumn) :base(subimages, subimagesPerRow, subimagesPerColumn)
    {
        StartRow = 0;
        StartColumn = 1;
    }
}

internal class Group3 : Group
{
    public Group3(Subimage[][] subimages, int subimagesPerRow, int subimagesPerColumn) :base(subimages, subimagesPerRow, subimagesPerColumn)
    {
        StartRow = 1;
        StartColumn = 0;
    }
}

internal class Group4 : Group
{
    public Group4(Subimage[][] subimages, int subimagesPerRow, int subimagesPerColumn) :base(subimages, subimagesPerRow, subimagesPerColumn)
    {
        StartRow = 1;
        StartColumn = 1;
    }
}