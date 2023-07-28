using System;
using System.Drawing;
using System.Drawing.Imaging;

#nullable enable

namespace Pointillism_image_generator
{
    /// <summary>
    /// PointillismImageGenerator is a class for generating a pointillistic image. 
    /// Square patterns are pasted in the generated image to make it look as close as possible to the original image.
    /// The patterns can be rotated and have different colors.
    /// </summary>
    public class PointillismImageGenerator
    {
        private readonly Bitmap _originalImage;
        private Bitmap _outputImage;
        private int _numberOfPatterns;

        private readonly int _patternSize;
        private readonly int _windowSize; // size where fits pattern with any rotation about centre
        private int HalfWindowSize => _windowSize / 2;
        private readonly SmartHeap<PatternNode, (int, int)> _patternsToAdd;
        
        private readonly Color _backgroundColor;
        private const int PixelMultiple = 4; // defines pixels on which to generate a pattern
        private Bitmap _windowSizeBitmap;

        /// <summary>
        /// Initializes the generator.
        /// </summary>
        /// <param name="originalImage">original (input) image</param>
        /// <param name="patternSize">size of a pattern</param>
        /// <param name="backgroundColor">color of background in the generated image</param>
        public PointillismImageGenerator(Image originalImage, int patternSize, Color backgroundColor)
        {
            _patternSize = patternSize;
            _windowSize = PatternSizeToWindowSize(_patternSize);
            _backgroundColor = backgroundColor;
            _windowSizeBitmap = new Bitmap(_windowSize, _windowSize);

            int padding = 2 * HalfWindowSize;
            _originalImage = new Bitmap(originalImage.Width + padding, originalImage.Height + padding, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(_originalImage))
            {
                g.Clear(backgroundColor);
                g.DrawImage(originalImage, HalfWindowSize, HalfWindowSize, originalImage.Width, originalImage.Height);
            }    // padding has color 'backgroundColor' 
            _outputImage = new Bitmap(_originalImage.Width, _originalImage.Height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(_outputImage)) { g.Clear(backgroundColor); }

            _patternsToAdd = new SmartHeap<PatternNode, (int, int)>(
                originalImage.Width * originalImage.Height / (PixelMultiple * PixelMultiple) + originalImage.Width + originalImage.Height);
            InitializePatterns();
        }

        /// <summary>
        /// Adds patterns to the heap. 
        /// </summary>
        private void InitializePatterns()
        {
            for (int j = HalfWindowSize; j < _originalImage.Height - HalfWindowSize; j += PixelMultiple)
            {
                for (int i = HalfWindowSize; i < _originalImage.Width - HalfWindowSize; i += PixelMultiple)
                {
                    PatternNode patternNode = GetBestPatternOnIndex(i, j);
                    Error backgroundError = ComputeBackgroundError(i, j);
                    patternNode.Improvement = (backgroundError - patternNode.Error).ToInt();
                    _patternsToAdd.Add(patternNode);
                }
            }
        }

        /// <returns>The generated image.</returns>
        public Bitmap GetOutputImage()
        {
            return _outputImage.Clone(new Rectangle(
                HalfWindowSize,
                HalfWindowSize,
                _originalImage.Width - _windowSize,
                _originalImage.Height - _windowSize),
                PixelFormat.Format24bppRgb); // remove padding
        }
        
        /// <returns>A number of patterns pasted in the generated image.</returns>
        public int GetNumberOfPatterns()
        {
            return _numberOfPatterns;
        }
        
        /// <summary>
        /// Adds the best possible pattern to the generated image.
        /// </summary>
        /// <returns>True if the pattern was added, otherwise false - the generated image can not be improved.</returns>
        public bool GeneratePointillismImage()
        {
            return AddBestOfBestPatterns();
        }

        /// <summary>Takes a pattern with the best improvement and adds it to the generated image. The pattern is
        /// added only if the improvement value is a positive number.</summary>
        /// <returns>True if the pattern was added, otherwise false.</returns>
        private bool AddBestOfBestPatterns()
        {
            PatternNode patternNode = _patternsToAdd.PeekMax()!;
            if (patternNode.Improvement <= 0)
                return false;

            AddPattern(_outputImage, patternNode.SquarePattern);
            ++_numberOfPatterns;
            UpdatePatterns(patternNode.SquarePattern.XIndex, patternNode.SquarePattern.YIndex);
            return true;
        }
        
        /// <summary>Adds pattern to a bitmap. Properties of the pattern define where to put it.</summary>
        /// <param name="bmp">a bitmap</param>
        /// <param name="squarePattern">a pattern to add</param>
        /// <returns>The bitmap with added pattern.</returns>
        private Bitmap AddPattern(Bitmap bmp, SquarePattern squarePattern)
        {
            using Graphics graphics = Graphics.FromImage(bmp);
            graphics.TranslateTransform(squarePattern.XIndex, squarePattern.YIndex);
            graphics.RotateTransform(squarePattern.Angle);
            graphics.TranslateTransform(-_patternSize / 2, -_patternSize / 2);

            using Brush brush = new SolidBrush(Color.FromArgb(squarePattern.ColorArgb.ToInt()));
            graphics.FillRectangle(brush, 0, 0, _patternSize, _patternSize);

            return bmp;
        }

        /// <summary>Updates patterns in the window size region of newly added pattern.</summary>
        /// <param name="xIndex">x-coordinate of the newly added pattern</param>
        /// <param name="yIndex">y-coordinate of the newly added pattern</param>
        private void UpdatePatterns(int xIndex, int yIndex)
        {
            (int iStart, int iEnd) = GetIndicesOfRegion(xIndex, _windowSize, true);
            (int jStart, int jEnd) = GetIndicesOfRegion(yIndex, _windowSize, false);

            for (int j = jStart; j < jEnd; j++)
            {
                for (int i = iStart; i < iEnd; i++)
                {
                    if ((i - HalfWindowSize) % PixelMultiple == 0 && (j - HalfWindowSize) % PixelMultiple == 0)
                    {
                        _patternsToAdd.Update(GetBestPatternOnIndex(i, j));
                    }
                }
            }
        }

        /// <summary>
        /// Finds a pattern on the given index that the best reflects the original image.
        /// It requires to try different combinations of rotation and color.
        /// </summary>
        /// <param name="xIndex">x-coordinate of the searched pattern</param>
        /// <param name="yIndex">y-coordinate of the searched pattern</param>
        /// <returns>A node with the best pattern and its error.</returns>
        private PatternNode GetBestPatternOnIndex(int xIndex, int yIndex)
        {
            SquarePattern pattern = new SquarePattern(xIndex, yIndex, default, default);
            SquarePattern bestPattern = pattern;
            Error smallestError = int.MaxValue;

            for (int angle = 0; angle < 90; angle += 30)
            {
                _windowSizeBitmap = _outputImage.Clone(
                    new Rectangle(xIndex - HalfWindowSize, yIndex - HalfWindowSize, _windowSize, _windowSize),
                    _outputImage.PixelFormat);
                pattern.Angle = angle;
                var (color, error) = FindBestColorOfPattern(_windowSizeBitmap, pattern);

                if (error >= smallestError) continue;
                smallestError = error;
                bestPattern.Angle = angle;
                bestPattern.ColorArgb = color;
            }
            return new PatternNode(bestPattern, smallestError);
        }
        
        /// <summary>Finds the best color of pattern using a binary search performed separately on each rgb channel.</summary>
        /// <param name="windowSizeBmp">window size bitmap cropped from the generated image</param>
        /// <param name="pattern">pattern whose color is being searched for</param>
        /// <returns>Color in ARGB format and error of pattern.</returns>
        private (ColorArgb, Error) FindBestColorOfPattern(Bitmap windowSizeBmp, SquarePattern pattern)
        {
            SquarePattern pattern1 = new SquarePattern(HalfWindowSize, HalfWindowSize, pattern.ColorArgb, pattern.Angle);
            SquarePattern pattern2 = pattern1;
            RgbColorRange rgbColorRange = new RgbColorRange();
            ErrorRgb errorFirstHalf = default;
            ErrorRgb errorSecondHalf = default;
            
            while (rgbColorRange.NotDone)
            {
                pattern1.ColorArgb = rgbColorRange.FirstHalfMax.ToArgb();
                pattern2.ColorArgb = rgbColorRange.SecondHalfMin.ToArgb();
                errorFirstHalf = ComputeError(AddPattern(windowSizeBmp, pattern1), pattern.XIndex, pattern.YIndex);
                errorSecondHalf = ComputeError(AddPattern(windowSizeBmp, pattern2), pattern.XIndex, pattern.YIndex);
                rgbColorRange.UpdateRange(errorFirstHalf, errorSecondHalf);
            }
            var (bestRgb, error) = rgbColorRange.GetBestColor(errorFirstHalf, errorSecondHalf);
            return (bestRgb.ToArgb(), error);
        }

        /// <summary>Computes an error between given bitmap and the corresponding part of the original image.</summary>
        /// <param name="windowSizeBmp">window size bitmap with added pattern (cropped from the output image)</param>
        /// <param name="x">x-coordinate of pattern in the generated image</param>
        /// <param name="y">y-coordinate of pattern in the generated image</param>
        /// <returns>Error in RGB format.</returns>
        private ErrorRgb ComputeError(Bitmap windowSizeBmp, int x, int y)
        {
            ErrorRgb error = new();

            for (int j = y - HalfWindowSize; j < y + HalfWindowSize; j++)
            {
                for (int i = x - HalfWindowSize; i < x + HalfWindowSize; i++)
                {
                    Color originalPixel = _originalImage.GetPixel(i, j);
                    Color newPixel = windowSizeBmp.GetPixel(i - x + HalfWindowSize, j - y + HalfWindowSize);
                    error.Red += Math.Abs(originalPixel.R - newPixel.R);
                    error.Green += Math.Abs(originalPixel.G - newPixel.G);
                    error.Blue += Math.Abs(originalPixel.B - newPixel.B);
                }
            }
            return error;
        }

        /// <summary>Computes an error between the original image and the background of generated image.</summary>
        /// <param name="x">x-coordinate of pattern in the generated image</param>
        /// <param name="y">y-coordinate of pattern in the generated image</param>
        /// <returns>Error in RGB format.</returns>
        private Error ComputeBackgroundError(int x, int y)
        {
            Error backgroundError = 0;

            for (int j = y - HalfWindowSize; j < y + HalfWindowSize; j++)
            {
                for (int i = x - HalfWindowSize; i < x + HalfWindowSize; i++)
                {
                    Color originalPixel = _originalImage.GetPixel(i, j);
                    backgroundError += Math.Abs(originalPixel.R - _backgroundColor.R);
                    backgroundError += Math.Abs(originalPixel.G - _backgroundColor.G);
                    backgroundError += Math.Abs(originalPixel.B - _backgroundColor.B);
                }
            }
            return backgroundError;
        }

        /// <summary> Returns the region indices that do not overlap the side padding. </summary>
        /// <param name="index">centre of region</param>
        /// <param name="region">size of region</param>
        /// <param name="xCoord">indicates whether it is x- or y-coordinate</param>
        /// <returns>x- or y-coordinates of region.</returns>
        private (int, int) GetIndicesOfRegion(int index, int region, bool xCoord)
        {
            int start = index - region / 2;
            int end = start + region;
            if (start <= HalfWindowSize)
                return (HalfWindowSize, end);

            int size = xCoord ? _originalImage.Width : _originalImage.Height;
            if (end >= size - HalfWindowSize)
                return (start, size - HalfWindowSize);
            
            return (start, end);
        }

        /// <summary> Translates pattern size to window size.
        /// Window is a square where fits pattern with any rotation about centre.</summary>
        /// <param name="patternSize">size of pattern</param>
        /// <returns>Window size.</returns>
        private static int PatternSizeToWindowSize(int patternSize)
        {
            switch (patternSize)
            {
                case 9:
                case 13:
                    return patternSize / 2 + patternSize;
                case 7:
                case 11:
                    return patternSize / 2 + patternSize + 1;
                default:
                    throw new ArgumentException();
            }
        }
    }
}