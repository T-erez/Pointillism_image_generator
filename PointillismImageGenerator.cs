using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;


namespace Pointillism_image_generator
{
    /// <summary>
    /// PointillismImageGenerator generates pointillistic images. 
    /// Square patterns are pasted in the generated image to make it look as close as possible to the original image.
    /// The patterns can be rotated and have different colors.
    /// Add patterns to the generated image by calling AddPatterns().
    /// </summary>
    public abstract class PointillismImageGenerator :IDisposable
    {
        private readonly BitmapDataMultiThreads _originalBmpData;
        private BitmapDataMultiThreads _generatedBmpData;
        protected Bitmap OutputBitmap;
        protected Rectangle ImageWithoutPadding;
        private readonly Color _backgroundColor;
        protected int NumberOfPatterns;

        private readonly int _patternSize;
        /// <summary>
        /// Size where fits pattern with any rotation about centre.
        /// </summary>
        protected readonly int WindowSize;
        protected int HalfWindowSize => WindowSize / 2;
        private bool[][,] _isCovered = null!;
        private const int PatternRotationalSymmetry = 4;
        private const int PatternRotationsCount = 3;

        /// <summary>
        /// PixelMultiple defines pixels on which to generate a pattern. For value x, pattern will be generated on every (x^2)-th pixel.
        /// The smaller the value, the more precise the generated image is. 
        /// </summary>
        protected const int PixelMultiple = 2;
        protected bool Disposed;

        /// <summary>
        /// Initializes the generator.
        /// </summary>
        /// <param name="originalImage">original (input) image</param>
        /// <param name="patternSize">size of a pattern</param>
        /// <param name="backgroundColor">background color of the generated image</param>
        /// <exception cref="ArgumentOutOfRangeException">Exception is thrown if pattern size is a non-positive number.</exception>
        /// <exception cref="ArgumentException">Exception is thrown if original image is not in 24bpp or 32bpp pixel format.</exception>
        protected PointillismImageGenerator(Image originalImage, int patternSize, Color backgroundColor)
        {
            if (patternSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(patternSize), "Pattern size must be a positive number.");
            if (originalImage.PixelFormat is not (PixelFormat.Format24bppRgb or PixelFormat.Format32bppArgb
                or PixelFormat.Format32bppRgb or PixelFormat.Format32bppPArgb))
                throw new ArgumentException("Image pixel format is not supported.");
            
            _patternSize = patternSize;
            WindowSize = PatternSizeToWindowSize(_patternSize);
            _backgroundColor = backgroundColor;

            int padding = 2 * HalfWindowSize;
            _originalBmpData = new BitmapDataMultiThreads(AddPaddingToImage(originalImage, padding / 2, backgroundColor));

            OutputBitmap = new Bitmap(_originalBmpData.Width, _originalBmpData.Height, originalImage.PixelFormat);
            using (Graphics g = Graphics.FromImage(OutputBitmap)) { g.Clear(backgroundColor); }
            _generatedBmpData = new BitmapDataMultiThreads(OutputBitmap);

            ImageWithoutPadding = new Rectangle(HalfWindowSize, HalfWindowSize, _originalBmpData.Width - padding,
                _originalBmpData.Height - padding);

            InitializeIsCovered();
        }
        
        /// <summary>
        /// Initializes '_isCovered'.
        /// </summary>
        private void InitializeIsCovered()
        {
            _isCovered = new bool[PatternRotationsCount][,];

            for (int i = 0; i < PatternRotationsCount; i++)
            {
                _isCovered[i] = new bool[WindowSize, WindowSize];
                Bitmap bitmap = new Bitmap(WindowSize, WindowSize);
                var backgroundColor = Color.FromArgb(255,255,255);
                using (Graphics g = Graphics.FromImage(bitmap)) g.Clear(backgroundColor);

                int angle = i * (360 / PatternRotationalSymmetry) / PatternRotationsCount;
                Point centre = new Point(HalfWindowSize, HalfWindowSize);
                AddPattern(bitmap, new SquarePattern(centre, new ColorRgb(0,0,0), angle));

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
                            _isCovered[i][x, y] = color != backgroundColor;
                            ptr += pixelSize;
                        }
                    }
                }
                bitmap.UnlockBits(data);
            }
        }

        /// <summary>
        /// Returns true if pixel in window size region is covered by pattern depending on its rotation.
        /// </summary>
        /// <param name="angle">angle of pattern rotation</param>
        /// <param name="x">x-coordinate of pixel in window size region</param>
        /// <param name="y">y-coordinate of pixel in window size region</param>
        /// <returns>True if pixel is covered by the pattern.</returns>
        private bool IsCovered(int angle, int x, int y)
        {
            int angleIndex = angle / (360 / PatternRotationalSymmetry / PatternRotationsCount);
            return _isCovered[angleIndex][x, y];
        }

        /// <summary>
        /// Initializes all patterns.
        /// </summary>
        protected abstract void InitializePatterns();

        /// <summary>
        /// Initializes a pattern.
        /// </summary>
        /// <param name="centre">centre of a pattern</param>
        /// <returns>Initialized pattern.</returns>
        protected PatternWithImprovement InitializePattern(Point centre)
        {
            PatternWithImprovement pattern = GetBestPatternOnIndex(centre);
            Error backgroundError = ComputeBackgroundError(centre.X, centre.Y);
            pattern.Improvement = (backgroundError - pattern.Error).ToInt();
            return pattern;
        }

        /// <returns>The generated image.</returns>
        protected Bitmap GetOutputImage()
        {
            _generatedBmpData.UpdateBitmap();
            return OutputBitmap.Clone(ImageWithoutPadding, OutputBitmap.PixelFormat);
        }

        /// <summary>
        /// Adds the best possible patterns to the generated image. To see how many patterns are left to add, look at the patternsToAddShared value.
        /// </summary>
        /// <param name="patternsToAddShared">number of patterns to be added</param>
        /// <param name="progressImages">number of images to save during generation</param>
        /// <param name="token">token to cancel execution</param>
        /// <exception cref="ArgumentOutOfRangeException">Exception is thrown if number of patterns to add is non-positive
        /// or if number of images to save is negative.</exception>
        /// <returns>bool: True if all the patterns were added or the execution was canceled. False means that the generated
        /// image can not be improved.
        /// IList of generated bitmaps: Contains progress images and final generated image sorted by number of patterns in ascending order.</returns>
        public abstract Task<(bool canBeImproved, List<GeneratedBitmap> generatedBitmaps)> AddPatternsAsync(
            IntReference patternsToAddShared, int progressImages = 0,
            CancellationToken token = default);
        
        /// <summary>Adds a pattern to bitmap. Properties of the pattern define where to put it.</summary>
        /// <param name="bmp">a bitmap</param>
        /// <param name="squarePattern">a pattern to add</param>
        private void AddPattern(Bitmap bmp, SquarePattern squarePattern)
        {
            using Graphics graphics = Graphics.FromImage(bmp);
            graphics.TranslateTransform(squarePattern.Centre.X, squarePattern.Centre.Y);
            graphics.RotateTransform(squarePattern.Angle);
            graphics.TranslateTransform((float) (-_patternSize / 2.0), (float) (-_patternSize / 2.0));

            using Brush brush = new SolidBrush(squarePattern.ColorRgb.ToColor());
            graphics.FillRectangle(brush, 0, 0, _patternSize, _patternSize);
        }

        /// <summary>Adds pattern to the generated image.</summary>
        /// <param name="pattern">a pattern to add</param>
        protected void AddPattern(SquarePattern pattern)
        {
            Point leftUpperCorner = new Point(pattern.Centre.X - HalfWindowSize, pattern.Centre.Y - HalfWindowSize);
            int scan0 = leftUpperCorner.Y * _generatedBmpData.Stride + leftUpperCorner.X * _generatedBmpData.PixelSize;

            for (int y = 0; y < WindowSize; y++)
            {
                int index = scan0 + y * _generatedBmpData.Stride;
                for (int x = 0; x < WindowSize; x++)
                {
                    if (IsCovered(pattern.Angle, x, y))
                    {
                        _generatedBmpData[index+2] = pattern.ColorRgb.Red;
                        _generatedBmpData[index+1] = pattern.ColorRgb.Green;
                        _generatedBmpData[index] = pattern.ColorRgb.Blue;
                    }
                    index += _generatedBmpData.PixelSize;
                }
            }
        }

        /// <summary>Updates patterns in the window size region of newly added pattern.</summary>
        /// <param name="centre">centre of the newly added pattern</param>
        protected void UpdatePatterns(Point centre)
        {
            (int xStart, int xEnd) = GetIndicesOfRegion(centre.X, WindowSize, true);
            (int yStart, int yEnd) = GetIndicesOfRegion(centre.Y, WindowSize, false);

            for (int y = yStart; y < yEnd; y++)
            {
                for (int x = xStart; x < xEnd; x++)
                {
                    if ((x - HalfWindowSize) % PixelMultiple == 0 && (y - HalfWindowSize) % PixelMultiple == 0)
                    {
                        UpdatePattern(GetBestPatternOnIndex(new Point(x, y)));
                    }
                }
            }
        }

        /// <summary>Replaces an old pattern with a new pattern.</summary>
        /// <param name="newPattern">updated pattern</param>
        protected abstract void UpdatePattern(PatternWithImprovement newPattern);

        /// <summary>
        /// Finds a pattern on the given index that the best reflects the original image.
        /// It requires to try different combinations of rotation and color.
        /// </summary>
        /// <param name="centre">centre of the searched pattern</param>
        /// <returns>A node with the best pattern and its error.</returns>
        private PatternWithImprovement GetBestPatternOnIndex(Point centre)
        {
            // if (centre.X < ImageWithoutPadding.X || centre.X > ImageWithoutPadding.X + ImageWithoutPadding.Width ||
            //     centre.Y < ImageWithoutPadding.Y || centre.Y > ImageWithoutPadding.Y + ImageWithoutPadding.Height)
            //     throw new ArgumentException();
            
            SquarePattern pattern = new SquarePattern(centre, default, default);
            SquarePattern bestPattern = pattern;
            Error smallestError = int.MaxValue;

            int max = 360 / PatternRotationalSymmetry;
            int increment = max / PatternRotationsCount;
            for (int angle = 0; angle < max; angle += increment)
            {
                pattern.Angle = angle;
                var (color, error) = FindBestColorOfPattern(pattern);

                if (error >= smallestError) continue;
                smallestError = error;
                bestPattern.Angle = angle;
                bestPattern.ColorRgb = color;
            }
            return new PatternWithImprovement(bestPattern, smallestError);
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
        /// Computes an error of a pattern.
        /// </summary>
        /// <param name="pattern">a pattern that does not have to be inserted into the generated image</param>
        /// <returns>Error in RGB format.</returns>
        private ErrorRgb ComputeError(SquarePattern pattern)
        {
            ErrorRgb error = new();
            Point leftUpperCorner = new Point(pattern.Centre.X - HalfWindowSize, pattern.Centre.Y - HalfWindowSize);
            int scan0Original = leftUpperCorner.Y * _originalBmpData.Stride + leftUpperCorner.X * _originalBmpData.PixelSize;
            int scan0Generated = leftUpperCorner.Y * _generatedBmpData.Stride + leftUpperCorner.X * _generatedBmpData.PixelSize;

            for (int y = 0; y < WindowSize; y++)
            {
                int indexOriginal = scan0Original + y * _originalBmpData.Stride;
                int indexGenerated = scan0Generated + y * _generatedBmpData.Stride;
                
                for (int x = 0; x < WindowSize; x++)
                {
                    ColorRgb colorOriginal = new ColorRgb(_originalBmpData[indexOriginal + 2],
                        _originalBmpData[indexOriginal + 1], _originalBmpData[indexOriginal]);

                    ColorRgb colorGenerated = new ColorRgb(_generatedBmpData[indexGenerated + 2],
                        _generatedBmpData[indexGenerated + 1], _generatedBmpData[indexGenerated]);
                    if (IsCovered(pattern.Angle, x, y))
                        colorGenerated = pattern.ColorRgb;
                    
                    error.Red += Math.Abs(colorOriginal.Red - colorGenerated.Red);
                    error.Green += Math.Abs(colorOriginal.Green - colorGenerated.Green);
                    error.Blue += Math.Abs(colorOriginal.Blue - colorGenerated.Blue);

                    indexOriginal += _originalBmpData.PixelSize;
                    indexGenerated += _generatedBmpData.PixelSize;
                }
            }
            
            return error;
        }

        /// <summary>Computes an error between the original image and the background of generated image.</summary>
        /// <param name="centreX">x-coordinate of pattern in the generated image</param>
        /// <param name="centreY">y-coordinate of pattern in the generated image</param>
        /// <returns>Error in RGB format.</returns>
        private Error ComputeBackgroundError(int centreX, int centreY)
        {
            Error backgroundError = new();
            Point leftUpperCorner = new Point(centreX - HalfWindowSize, centreY - HalfWindowSize);
            int scan0Original = leftUpperCorner.Y * _originalBmpData.Stride + leftUpperCorner.X * _originalBmpData.PixelSize;

            for (int y = 0; y < WindowSize; y++)
            {
                int index = scan0Original + y * _originalBmpData.Stride;
                for (int x = 0; x < WindowSize; x++)
                {
                    ColorRgb color = new ColorRgb(_originalBmpData[index+2], _originalBmpData[index+1], _originalBmpData[index]);

                    backgroundError += Math.Abs(color.Red - _backgroundColor.R);
                    backgroundError += Math.Abs(color.Green - _backgroundColor.G);
                    backgroundError += Math.Abs(color.Blue - _backgroundColor.B);

                    index += _originalBmpData.PixelSize;
                }
            }
            
            return backgroundError;
        }

        /// <summary> Returns region indices that do not overlap the side padding of generated image.</summary>
        /// <param name="centre">index of the centre of the region</param>
        /// <param name="regionSize">size of region</param>
        /// <param name="xCoord">indicates whether it is x- or y-coordinate</param>
        /// <returns>x- or y-coordinates of region.</returns>
        private (int, int) GetIndicesOfRegion(int centre, int regionSize, bool xCoord)
        {
            int start = centre - regionSize / 2;
            int end = start + regionSize;
            if (start <= HalfWindowSize)
                return (HalfWindowSize, end);

            int size = xCoord ? _originalBmpData.Width : _originalBmpData.Height;
            if (end >= size - HalfWindowSize)
                return (start, size - HalfWindowSize);
            
            return (start, end);
        }

        /// <summary> Translates pattern size to window size.
        /// Window is a square where fits pattern with any rotation about the centre.</summary>
        /// <param name="patternSize">size of a pattern</param>
        /// <returns>Window size.</returns>
        private static int PatternSizeToWindowSize(int patternSize)
        {
            switch (patternSize)
            {
                case 7:
                    return 11;
                case 9:
                    return 13;
                case 11:
                    return 17;
                case 13:
                    return 19;
                case 15:
                    return 23;
                case 17:
                    return 25;
                case 19:
                    return 27;
                case 21:
                    return 31;
                case 23:
                    return 33;
                default:
                    throw new ArgumentException();
            }
        }
        
        /// <summary>
        /// Divides two positive integers rounding up.
        /// </summary>
        /// <param name="x">dividend</param>
        /// <param name="y">divisor</param>
        /// <returns></returns>
        protected static int DivideRoundingUp(int x, int y) => (x + y - 1) / y;
        
        /// <summary>
        /// Adds padding to an image. Padding is added to all four sides of the image.
        /// </summary>
        private static Bitmap AddPaddingToImage(Image image, int padding, Color paddingColor)
        {
            var bitmapWithPadding = new Bitmap(image.Width + 2 * padding, image.Height + 2 * padding, image.PixelFormat);
            using Graphics g = Graphics.FromImage(bitmapWithPadding);
            g.Clear(paddingColor);
            g.DrawImage(image, padding, padding, image.Width, image.Height);

            return bitmapWithPadding;
        }

        public void Dispose()
        {
            if (Disposed)
                return;
            
            OutputBitmap.Dispose();
            _originalBmpData.Dispose();
            _generatedBmpData.Dispose();
            Disposed = true;
        }
    }
}