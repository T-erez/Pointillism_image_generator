using System.Drawing;

namespace Pointillism_image_generator;

/// <summary>
/// The GeneratedBitmap structure encapsulates bitmap and number of patterns that were added during generation.
/// </summary>
public struct GeneratedBitmap
{
    public Bitmap Bitmap;
    public int PatternsCount;

    public GeneratedBitmap(Bitmap bitmap, int patternsCount)
    {
        Bitmap = bitmap;
        PatternsCount = patternsCount;
    }
}