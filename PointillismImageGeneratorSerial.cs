using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

#nullable enable

namespace Pointillism_image_generator
{
    /// <summary>
    /// PointillismImageGeneratorSerial generates pointillistic images. It implements serial pasting algorithm.
    /// Add patterns to the generated image by calling AddPatterns(). 
    /// </summary>
    public class PointillismImageGeneratorSerial : PointillismImageGenerator
    {
        private SmartHeap<PatternWithImprovement, Point> _patterns = null!;

        /// <summary>
        /// Initializes the generator.
        /// </summary>
        /// <param name="originalImage">original (input) image</param>
        /// <param name="patternSize">size of a pattern</param>
        /// <param name="backgroundColor">background color in the generated image</param>
        /// <exception cref="ArgumentOutOfRangeException">Exception is thrown if pattern size is a non-positive number.</exception>
        /// <exception cref="ArgumentException">Exception is thrown if original image is not in 24bpp or 32bpp pixel format.</exception>
        public PointillismImageGeneratorSerial(Image originalImage, int patternSize, Color backgroundColor) : base(originalImage, patternSize, backgroundColor)
        {
            InitializePatterns();
        }
        
        protected sealed override void InitializePatterns()
        {
            int indicesPerRow = DivideRoundingUp(ImageWithoutPadding.Width, PixelMultiple);
            int indicesPerColumn = DivideRoundingUp(ImageWithoutPadding.Height, PixelMultiple);

            _patterns = new SmartHeap<PatternWithImprovement, Point>(indicesPerRow*indicesPerColumn);
            for (int y = HalfWindowSize; y < OutputBitmap.Height - HalfWindowSize; y += PixelMultiple)
            {
                for (int x = HalfWindowSize; x < OutputBitmap.Width - HalfWindowSize; x += PixelMultiple)
                {
                    PatternWithImprovement pattern = InitializePattern(new Point(x, y));
                    _patterns.Add(pattern);
                }
            }
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
                if (!AddBestPattern())
                {
                    generatedBitmaps.Add(new GeneratedBitmap(GetOutputImage(), NumberOfPatterns));
                    return (false, generatedBitmaps);
                }
                
                --patternsToAdd;
                patternsToAddShared.Value = patternsToAdd;
                ++patternsAdded;

                if (patternsAdded == nextToSave || token.IsCancellationRequested)
                {
                    generatedBitmaps.Add(new GeneratedBitmap(GetOutputImage(), NumberOfPatterns));
                    nextToSave += step;
                }

                if (patternsToAdd <= 0 || token.IsCancellationRequested)
                    return (true, generatedBitmaps);
            }

            return (true, generatedBitmaps);
        }

        /// <summary>Takes a pattern with the best improvement and adds it to the generated image. The pattern is
        /// added only if the improvement value is a positive number.</summary>
        /// <returns>True if the pattern was added, otherwise false.</returns>
        private bool AddBestPattern()
        {
            PatternWithImprovement pattern = _patterns.PeekMax()!;
            if (pattern.Improvement <= 0)
                return false;

            AddPattern(pattern.SquarePattern);
            ++NumberOfPatterns;
            UpdatePatterns(pattern.SquarePattern.Centre);
            return true;
        }

        protected override void UpdatePattern(PatternWithImprovement newPattern)
        {
            _patterns.Update(newPattern);
        }
    }
}