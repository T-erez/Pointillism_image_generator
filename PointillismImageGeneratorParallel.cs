using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;

namespace Pointillism_image_generator;

/// <summary>
/// PointillismImageGeneratorParallel class generates pointillistic images. It implements parallel pasting algorithm. 
/// Add patterns to the generated image by calling AddPatterns().
/// </summary>
public class PointillismImageGeneratorParallel : PointillismImageGenerator
{
    private readonly object _numberOfPatternsLock = new();
    private Subimages _subimages = null!;
    private readonly int _subimageSize;
    /// <summary>The maximum number of threads is the square of the value.</summary>
    private const int SubimagesPerOneDimension = 10;
    /// <summary>The minimum number of threads that added a pattern. The smaller the number, the more accurate the generator.</summary>
    private const int MinimumPatternsAddedInIteration = 5;
    private int _improvementLevel;
    private readonly int _improvementLevelStep;

    /// <summary>
    /// Initializes the generator.
    /// </summary>
    /// <param name="originalImage">original (input) image</param>
    /// <param name="patternSize">size of a pattern</param>
    /// <param name="backgroundColor">background color of the generated image</param>
    /// <exception cref="ArgumentOutOfRangeException">Exception is thrown if pattern size is a non-positive number.</exception>
    /// <exception cref="ArgumentException">Exception is thrown if original image is not in 24bpp or 32bpp pixel format.</exception>
    public PointillismImageGeneratorParallel(Image originalImage, int patternSize, Color backgroundColor) : base(originalImage, patternSize, backgroundColor)
    {
        int maxImprovement = 255 * 3 * patternSize * patternSize;
        _improvementLevelStep = (int) (maxImprovement / 7.0);
        _improvementLevel = maxImprovement - _improvementLevelStep;
        
        _subimageSize = Math.Max(originalImage.Width / SubimagesPerOneDimension, originalImage.Height / SubimagesPerOneDimension);
        InitializePatterns();
    }
    
    /// <summary>
    /// Initializes patterns for each subimage.
    /// </summary>
    protected sealed override void InitializePatterns()
    {
        int indicesPerRow = DivideRoundingUp(ImageWithoutPadding.Width, PixelMultiple);
        int indicesPerColumn = DivideRoundingUp(ImageWithoutPadding.Height, PixelMultiple);
        
        Rectangle patternsRegion = new Rectangle(HalfWindowSize, HalfWindowSize, (indicesPerRow - 1) * PixelMultiple + 1,
            (indicesPerColumn - 1) * PixelMultiple + 1);
        int patternsPerRow = DivideRoundingUp(_subimageSize, PixelMultiple);
        int patternsPerSubimage = patternsPerRow * patternsPerRow;
        _subimages = new Subimages(patternsRegion, _subimageSize, WindowSize, patternsPerSubimage);
        Parallel.For(0, indicesPerRow * indicesPerColumn, i =>
        {
            Point centre = new Point(HalfWindowSize + (i % indicesPerRow) * PixelMultiple,
                HalfWindowSize + (i / indicesPerRow) * PixelMultiple);
            PatternWithImprovement pattern = InitializePattern(centre);
            lock (_subimages.GetSubimage(centre).Patterns)
            {
                _subimages.GetSubimage(centre).Patterns.Add(pattern);
            }
        });
    }
    
    public override (bool, IList<GeneratedBitmap>) AddPatterns(IntReference patternsToAddShared, int progressImages = 0, CancellationToken token = default)
    {
        if (Disposed)
            throw new ObjectDisposedException("Can not use disposed generator.");
        int patternsToAdd = patternsToAddShared.Value;
        if (patternsToAdd <= 0 || progressImages < 0) throw new ArgumentOutOfRangeException();
            
        List<GeneratedBitmap> generatedBitmaps = new(progressImages + 1);
        int step = progressImages == 0 ? patternsToAdd : patternsToAdd / progressImages;
        int nextToSave = step;
        int patternsAdded = 0;
        while (patternsToAdd > 0)
        {
            int patternsAddedInIteration = 0;
            #region OneIteration
    
            foreach (var group in _subimages.Groups)
            {
                var save = nextToSave;
                Parallel.ForEach(group, (subimage, state) =>
                {
                    if (AddBestPatternToSubimage(subimage))
                    {
                        lock (patternsToAddShared)
                        {
                            --patternsToAdd; 
                            patternsToAddShared.Value = patternsToAdd;
                            ++patternsAddedInIteration;
                            ++patternsAdded;
                            if (patternsToAdd <= 0 || patternsAdded >= save) state.Break();
                        }
                    }
                    if (!token.IsCancellationRequested) return;
                    state.Break();
                    nextToSave = patternsAdded;
                });
                if (patternsAdded >= nextToSave)
                {
                    generatedBitmaps.Add(new GeneratedBitmap(GetOutputImage(), NumberOfPatterns));
                    nextToSave += step;
                }
                if (token.IsCancellationRequested || patternsToAdd <= 0) return (true, generatedBitmaps);
            }
    
            #endregion

            if (_improvementLevel == 0 && patternsAddedInIteration == 0)
            {
                generatedBitmaps.Add(new GeneratedBitmap(GetOutputImage(), NumberOfPatterns));
                return (false, generatedBitmaps);
            }
            
            if (patternsAddedInIteration < MinimumPatternsAddedInIteration) UpdateImprovementLevel();
        }
        return (true, generatedBitmaps);
    }
    
    /// <summary>Takes a pattern with the best improvement for a subimage and adds it to the generated image. 
    /// The pattern is added only if the improvement value is greater than '_improvementLevel'.</summary>
    /// <returns>True if pattern was added, otherwise false.</returns>
    private bool AddBestPatternToSubimage(Subimage subimage)
    {
        PatternWithImprovement pattern = subimage.Patterns.PeekMax()!;
        if (pattern.Improvement <= _improvementLevel)
            return false;
        
        AddPattern(pattern.SquarePattern);
        lock (_numberOfPatternsLock)
        {
            ++NumberOfPatterns;
        }
        UpdatePatterns(pattern.SquarePattern.Centre);
        return true;
    }
    
    /// <summary>
    /// Lowers the level of improvement.
    /// </summary>
    private void UpdateImprovementLevel()
    {
        _improvementLevel = Math.Max(0, _improvementLevel - _improvementLevelStep);    
    }
    
    protected override void UpdatePattern(PatternWithImprovement newPattern)
    {
        _subimages.GetSubimage(newPattern.SquarePattern.Centre).Patterns.Update(newPattern);
    }
}