using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

#nullable enable

namespace Pointillism_image_generator
{
    /// <summary>
    /// PointillismImageGenerator is a class for generating a pointillistic image. 
    /// Square patterns are pasted in the generated image to make it look as close as possible to the original image.
    /// The patterns can be rotated and have different colors.
    /// Add patterns to the generated image by calling GeneratePointillismImage() repeatedly. 
    /// </summary>
    public class PointillismImageGenerator
    {
        private readonly Bitmap _originalImage;
        private Bitmap _generatedImage;
        private int _numberOfPatterns;

        private readonly int _patternSize;
        private readonly int _windowSize; // size where fits pattern with any rotation about centre
        private int HalfWindowSize => _windowSize / 2;
        private SmartHeap<PatternNode, (int, int)> _patternsToAdd;
        private const int RotationsCount = 3;
        private const int RotationalSymmetry = 4;
        
        private readonly Color _backgroundColor;
        private bool[][,] _isBackground; //the field is used to determine which pixels of window size region
                                         //are covered by pattern depending on its rotation
        private const int PixelMultiple = 2; // defines pixels on which to generate a pattern

        /// <summary>
        /// Initializes the generator.
        /// </summary>
        /// <param name="originalImage">original (input) image</param>
        /// <param name="patternSize">size of a pattern</param>
        /// <param name="backgroundColor">background color in the generated image</param>
        public PointillismImageGenerator(Image originalImage, int patternSize, Color backgroundColor)
        {
            _patternSize = patternSize;
            _windowSize = PatternSizeToWindowSize(_patternSize);
            _backgroundColor = backgroundColor;

            int padding = 2 * HalfWindowSize;
            _originalImage = new Bitmap(originalImage.Width + padding, originalImage.Height + padding, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(_originalImage))
            {
                g.Clear(backgroundColor);
                g.DrawImage(originalImage, HalfWindowSize, HalfWindowSize, originalImage.Width, originalImage.Height);
            }    // padding has color 'backgroundColor' 
            _generatedImage = new Bitmap(_originalImage.Width, _originalImage.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(_generatedImage)) { g.Clear(backgroundColor); }

            InitializeIsBackground();
            _patternsToAdd = new SmartHeap<PatternNode, (int, int)>(
                originalImage.Width * originalImage.Height / (PixelMultiple * PixelMultiple) + originalImage.Width + originalImage.Height);
            InitializePatterns();
        }
        
        /// <summary>
        /// Initializes '_isBackground'.
        /// </summary>
        private void InitializeIsBackground()
        {
            _isBackground = new bool[RotationsCount][,];

            for (int i = 0; i < RotationsCount; i++)
            {
                _isBackground[i] = new bool[_windowSize, _windowSize];
                Bitmap bitmap = new Bitmap(_windowSize, _windowSize);
                var backgroundColor = Color.FromArgb(255,255,255);
                using (Graphics g = Graphics.FromImage(bitmap)) g.Clear(backgroundColor);

                int angle = i * (360 / RotationalSymmetry) / RotationsCount;
                AddPattern(bitmap, new SquarePattern(_windowSize / 2 + 1, _windowSize / 2 + 1, new ColorRgb(0,0,0), angle));

                BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, bitmap.PixelFormat);
                unsafe
                {
                    int pixelSize = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        byte* ptr = (byte*) data.Scan0 + y * data.Stride;

                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            Color color = Color.FromArgb(ptr[2], ptr[1], ptr[0]);
                            _isBackground[i][x, y] = color == backgroundColor;
                            ptr += pixelSize;
                        }
                    }
                }
                bitmap.UnlockBits(data);
            }
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
            return _generatedImage.Clone(new Rectangle(
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

            AddPattern(_generatedImage, patternNode.SquarePattern);
            ++_numberOfPatterns;
            UpdatePatterns(patternNode.SquarePattern.XIndex, patternNode.SquarePattern.YIndex);
            return true;
        }
        
        /// <summary>Adds pattern to a bitmap. Properties of the pattern define where to put it.</summary>
        /// <param name="bmp">a bitmap</param>
        /// <param name="squarePattern">a pattern to add</param>
        private void AddPattern(Bitmap bmp, SquarePattern squarePattern)
        {
            using Graphics graphics = Graphics.FromImage(bmp);
            graphics.TranslateTransform(squarePattern.XIndex, squarePattern.YIndex);
            graphics.RotateTransform(squarePattern.Angle);
            graphics.TranslateTransform((float) (-_patternSize / 2.0), (float) (-_patternSize / 2.0));

            using Brush brush = new SolidBrush(squarePattern.ColorRgb.ToColor());
            graphics.FillRectangle(brush, 0, 0, _patternSize, _patternSize);
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
                pattern.Angle = angle;
                var (color, error) = FindBestColorOfPattern(pattern);

                if (error >= smallestError) continue;
                smallestError = error;
                bestPattern.Angle = angle;
                bestPattern.ColorRgb = color;
            }
            return new PatternNode(bestPattern, smallestError);
        }
        
        /// <summary>Finds the best color of pattern using a binary search performed separately on each rgb channel.</summary>
        /// <param name="pattern">pattern whose color is being searched for</param>
        /// <returns>Color in RGB format and error of pattern.</returns>
        private (ColorRgb, Error) FindBestColorOfPattern(SquarePattern pattern)
        {
            SquarePattern pattern2 = pattern;
            RgbColorRange rgbColorRange = new RgbColorRange();
            ErrorRgb errorFirstHalf = default;
            ErrorRgb errorSecondHalf = default;
            
            while (rgbColorRange.NotDone)
            {
                pattern.ColorRgb = rgbColorRange.FirstHalfMax;
                pattern2.ColorRgb = rgbColorRange.SecondHalfMin;
                errorFirstHalf = ComputeError(pattern);
                errorSecondHalf = ComputeError(pattern2);
                rgbColorRange.UpdateRange(errorFirstHalf, errorSecondHalf);
            }
            var (bestRgb, error) = rgbColorRange.GetBestColor(errorFirstHalf, errorSecondHalf);
            return (bestRgb, error);
        }

        /// <summary>
        /// Computes an error of pattern.
        /// </summary>
        /// <param name="pattern">a pattern that does not have to be inserted into the generated image</param>
        /// <returns>Error in RGB format.</returns>
        private ErrorRgb ComputeError(SquarePattern pattern)
        {
            ErrorRgb error = new();
            Rectangle window = new Rectangle(pattern.XIndex - _patternSize / 2, pattern.YIndex - _patternSize / 2, _patternSize, _patternSize);
            BitmapData dataOriginal  = _originalImage.LockBits(window,ImageLockMode.ReadOnly, _originalImage.PixelFormat);
            BitmapData dataGenerated = _generatedImage.LockBits(window, ImageLockMode.ReadOnly, _generatedImage.PixelFormat);
            unsafe
            {
                int pixelSizeOriginal = Image.GetPixelFormatSize(_originalImage.PixelFormat) / 8;
                int pixelSizeGenerated = Image.GetPixelFormatSize(_generatedImage.PixelFormat) / 8;

                for (int y = 0; y < _patternSize; y++)
                {
                    byte* ptrOriginal = (byte*) dataOriginal.Scan0 + y * dataOriginal.Stride;
                    byte* ptrGenerated = (byte*) dataGenerated.Scan0 + y * dataGenerated.Stride;

                    for (int x = 0; x < _patternSize; x++)
                    {
                        ColorRgb colorOriginal = new ColorRgb(ptrOriginal[2], ptrOriginal[1], ptrOriginal[0]);

                        ColorRgb colorGenerated = pattern.ColorRgb;
                        if (_isBackground[AngleToIndex(pattern.Angle)][x,y])
                            colorGenerated = new ColorRgb(ptrGenerated[2], ptrGenerated[1], ptrGenerated[0]);

                        error.Red += Math.Abs(colorOriginal.Red - colorGenerated.Red);
                        error.Green += Math.Abs(colorOriginal.Green - colorGenerated.Green);
                        error.Blue += Math.Abs(colorOriginal.Blue - colorGenerated.Blue);
                        
                        ptrOriginal += pixelSizeOriginal;
                        ptrGenerated += pixelSizeGenerated;
                    }
                }
            }
            _originalImage.UnlockBits(dataOriginal);
            _generatedImage.UnlockBits(dataGenerated);
            return error;
        }

        /// <summary>
        /// Translates angle of rotation to index into 'isBackground' field
        /// </summary>
        /// <param name="angle"></param>
        /// <returns>Index.</returns>
        private static int AngleToIndex(int angle)
        {
            return angle / (360 / RotationalSymmetry / RotationsCount);
        }

        /// <summary>Computes an error between the original image and the background of generated image.</summary>
        /// <param name="centreX">x-coordinate of pattern in the generated image</param>
        /// <param name="centreY">y-coordinate of pattern in the generated image</param>
        /// <returns>Error in RGB format.</returns>
        private Error ComputeBackgroundError(int centreX, int centreY)
        {
            ErrorRgb backgroundError = new();
            Rectangle window = new Rectangle(centreX - _patternSize / 2, centreY - _patternSize / 2, _patternSize, _patternSize);
            BitmapData data  = _originalImage.LockBits(window,ImageLockMode.ReadOnly, _originalImage.PixelFormat);
            unsafe
            {
                int pixelSize = Image.GetPixelFormatSize(_originalImage.PixelFormat) / 8;

                for (int y = 0; y < _patternSize; y++)
                {
                    byte* ptr = (byte*) data.Scan0 + y * data.Stride;

                    for (int x = 0; x < _patternSize; x++)
                    {
                        ColorRgb color = new ColorRgb(ptr[2], ptr[1], ptr[0]);

                        backgroundError.Red += Math.Abs(color.Red - _backgroundColor.R);
                        backgroundError.Green += Math.Abs(color.Green - _backgroundColor.G);
                        backgroundError.Blue += Math.Abs(color.Blue - _backgroundColor.B);
                        
                        ptr += pixelSize;
                    }
                }
            }
            _originalImage.UnlockBits(data);
            return backgroundError.Red + backgroundError.Green + backgroundError.Blue;
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