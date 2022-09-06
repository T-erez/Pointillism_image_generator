using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Pointillism_image_generator
{
    /// <summary>Structure for patterns, that will be pasted in the generated image. Patterns are squares.</summary>
    public struct Pattern
    {
        public int xIndex;
        public int yIndex;
        public int colorARGB;
        public int angle;

        /// <summary>Initialize pattern properties</summary>
        /// <param name="xIndex">x-coordinate of the centre of the pattern in the generated image</param>
        /// <param name="yIndex">y-coordinate of the centre of the pattern in the generated image</param>
        /// <param name="color">color of pattern in ARGB</param>
        /// <param name="angle">angle of rotation about the centre</param>
        public Pattern(int xIndex, int yIndex, int color, int angle)
        {
            this.xIndex = xIndex;
            this.yIndex = yIndex;
            this.colorARGB = color;
            this.angle = angle;
        }
    }

    /// <summary>RGBchannel structure represents one rgb channel and contains values to find the best color for a pattern.</summary>
    public struct RGBchannel
    {
        public int min;
        public int max;
        public int firstHalfMax;
        public int secondHalfMin;
        public int error1;
        public int error2;

        /// <param name="min">the lowest intestity</param>
        /// <param name="max">the highest intestity</param>
        /// <param name="firstHalfMax">the maximum value in the first half of the intensity range</param>
        /// <param name="secondHalfMin">the minimum value in the second half of the intensity range</param>
        /// <param name="error1">error of a pattern with color equal to firstHalfMax</param>
        /// <param name="error2">error of a pattern with color equal to secondHalfMin</param>
        public RGBchannel(int min, int max, int firstHalfMax = 0, int secondHalfMin = 0, int error1 = 0, int error2 = 0)
        {
            this.min = min;
            this.max = max;
            this.firstHalfMax = 0;
            this.secondHalfMin = 0;
            this.error1 = 0;
            this.error2 = 0;
        }
    }

    /// <summary>
    /// Pointillism is a class for generating pointillistic image. 
    /// Square patterns are inserted into the output image to make it look as close as possible to the original image.
    /// The patterns can be rotated and have different colors.
    ///     WindowSize:  size where fits pattern with any rotation about centre
    ///     PatternsToAdd:  heap with patterns to add 
    ///     PixelMultiple:  defines pixels on which to generate a pattern
    /// </summary>
    public class Pointillism
    {
        private Bitmap OriginalImage;
        private Bitmap OutputImage;
        private int NumberOfPatterns;

        private int PatternSize;
        private int WindowSize;
        private int HalfWindowSize;
        private SmartHeap PatternsToAdd;

        static int PixelMultiple = 4;

        private Bitmap WindowSizeBitmap;

        /// <summary>
        /// Set pattern size and get appropriate window size. 
        /// Add padding to the original image to unify the error calculation for edge pixels.
        /// Initialize white output image.
        /// Initialize patterns.
        /// </summary>
        /// <param name="originalImage">input image</param>
        /// <param name="patternSize">size of square patterns</param>
        public void Initialize(Image originalImage, int patternSize)
        {
            PatternSize = patternSize;
            WindowSize = PatternSizeToWindowSize(PatternSize);
            HalfWindowSize = WindowSize / 2;

            WindowSizeBitmap = new Bitmap(WindowSize, WindowSize);

            OriginalImage = new Bitmap(originalImage.Width + 2 * HalfWindowSize, originalImage.Height + 2 * HalfWindowSize, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(OriginalImage))
            {
                g.Clear(Color.White);
                g.DrawImage(originalImage, HalfWindowSize, HalfWindowSize, originalImage.Width, originalImage.Height);
            }    // padding will stay white
            OutputImage = new Bitmap(OriginalImage.Width, OriginalImage.Height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(OutputImage)) { g.Clear(Color.White); }

            PatternsToAdd = new SmartHeap(originalImage.Width * originalImage.Height / (PixelMultiple * PixelMultiple) + originalImage.Width + originalImage.Height);
            InitializePatterns();
        }

        /// <summary>
        /// Initialize patterns. Count error0. It is an error before inserting the pattern into the output image.
        /// Since the background color of the output image was set to white, it is a white canvas error with the original image.
        /// </summary>
        private void InitializePatterns()
        {
            Bitmap whiteCanvas = new Bitmap(WindowSize, WindowSize, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(whiteCanvas)) { g.Clear(Color.White); }
            for (int j = HalfWindowSize; j < OriginalImage.Height - HalfWindowSize; j += PixelMultiple)
            {
                for (int i = HalfWindowSize; i < OriginalImage.Width - HalfWindowSize; i += PixelMultiple)
                {
                    (int redError, int greenError, int blueError) = GetError(whiteCanvas, i, j);
                    int error0 = redError + greenError + blueError;
                    PatternsToAdd.Add(GetBestPatternOnIndex(i, j), error0);
                }
            }
        }

        /// <summary>
        /// Add the best possible pattern to the output image.
        /// </summary>
        /// <returns>If exists a pattern which improves the output image.</returns>
        public bool GeneratePointilismImage()
        {
            return AddBestOfBestPatterns();
        }

        /// <returns>The output image without padding.</returns>
        public Bitmap GetOutputImage()
        {
            return OutputImage.Clone(new Rectangle(
                HalfWindowSize,
                HalfWindowSize,
                OriginalImage.Width - WindowSize,
                OriginalImage.Height - WindowSize),
                PixelFormat.Format24bppRgb);
        }


        /// <returns>Number of patterns pasted in the output image.</returns>
        public int GetNumberOfPatterns()
        {
            return NumberOfPatterns;
        }

        /// <summary>Add pattern to bitmap. Properties of pattern define where to put it.</summary>
        /// <param name="bmp">bitmap</param>
        /// <param name="pattern">pattern to add</param>
        /// <returns>Bitmap with added pattern.</returns>
        private Bitmap AddPattern(Bitmap bmp, Pattern pattern)
        {
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.TranslateTransform(pattern.xIndex, pattern.yIndex);
                graphics.RotateTransform(pattern.angle);
                graphics.TranslateTransform(-PatternSize / 2, -PatternSize / 2);

                using (Brush brush = new SolidBrush(Color.FromArgb(pattern.colorARGB)))
                {
                    graphics.FillRectangle(brush, 0, 0, PatternSize, PatternSize);
                }
            }
            return bmp;
        }

        /// <summary>Get a pattern with the best improvement and add it to the output image.</summary>
        /// <returns>If the pattern was added or not.</returns>
        private bool AddBestOfBestPatterns()
        {
            Node node = PatternsToAdd.GetMax();
            if (node.improvement <= 0)
            {
                return false;
            }

            OutputImage = AddPattern(OutputImage, node.pattern);
            ++NumberOfPatterns;
            UpdatePatterns(node.pattern.xIndex, node.pattern.yIndex);
            return true;
        }

        /// <summary>Update patterns in the window size region of the changed pattern.</summary>
        /// <param name="xIndex">x-coordinate of the changed pattern</param>
        /// <param name="yIndex">y-coordinate of the changed pattern</param>
        private void UpdatePatterns(int xIndex, int yIndex)
        {
            (int iStart, int iEnd) = GetIndexesOfRegion(xIndex, WindowSize, true);
            (int jStart, int jEnd) = GetIndexesOfRegion(yIndex, WindowSize, false);

            for (int j = jStart; j < jEnd; j++)
            {
                for (int i = iStart; i < iEnd; i++)
                {
                    if ((i - HalfWindowSize) % PixelMultiple == 0 && (j - HalfWindowSize) % PixelMultiple == 0)
                    {
                        PatternsToAdd.Change(GetBestPatternOnIndex(i, j));
                    }
                }
            }
        }

        /// <summary>
        /// Find a pattern on given index that the best reflects the original image.
        /// It requiers to try different combinations of rotation and color.
        /// </summary>
        /// <param name="xIndex">x-coordinate of searched pattern</param>
        /// <param name="yIndex">y-coordinate of searched pattern</param>
        /// <returns>Node with best pattern and its error</returns>
        private Node GetBestPatternOnIndex(int xIndex, int yIndex)
        {
            int smallestError = int.MaxValue;
            int bestAngle = 0;
            int bestColor = 0;
            Pattern pattern = new Pattern(xIndex, yIndex, 0, 0);

            for (int angle = 0; angle < 90; angle += 30)
            {
                WindowSizeBitmap = OutputImage.Clone(
                    new Rectangle(xIndex - HalfWindowSize, yIndex - HalfWindowSize, WindowSize, WindowSize),
                    OutputImage.PixelFormat);
                pattern.angle = angle;

                (int color, int error) = GetBestColorOfPattern(WindowSizeBitmap, pattern);

                if (error < smallestError)
                {
                    smallestError = error;
                    bestAngle = angle;
                    bestColor = color;
                }
            }
            pattern.angle = bestAngle;
            pattern.colorARGB = bestColor;
            return new Node(pattern, 0, 0, smallestError);
        }

        /// <summary>A binary search performed seperately on each rgb channel.</summary>
        /// <param name="windowSizeBmp">window size bitmap cropped from the output image</param>
        /// <param name="pattern1">pattern whose best color is sought</param>
        /// <returns>color in ARGB format</returns>
        private (int, int) GetBestColorOfPattern(Bitmap windowSizeBmp, Pattern pattern1)
        {
            int x = pattern1.xIndex;
            int y = pattern1.yIndex;

            pattern1.xIndex = HalfWindowSize;
            pattern1.yIndex = HalfWindowSize;
            Pattern pattern2 = pattern1;

            RGBchannel[] rgb = new RGBchannel[] { new RGBchannel(0, 255), new RGBchannel(0, 255), new RGBchannel(0, 255) };

            while (rgb[0].max - rgb[0].min > 2 || rgb[1].max - rgb[1].min > 2 || rgb[2].max - rgb[2].min > 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    rgb[i].firstHalfMax = GetFirstHalfMax(rgb[i].min, rgb[i].max);
                    rgb[i].secondHalfMin = rgb[i].firstHalfMax + 1;
                }

                pattern1.colorARGB = GenerateColorARGB(rgb[0].firstHalfMax, rgb[1].firstHalfMax, rgb[2].firstHalfMax);
                pattern2.colorARGB = GenerateColorARGB(rgb[0].secondHalfMin, rgb[1].secondHalfMin, rgb[2].secondHalfMin);

                (rgb[0].error1, rgb[1].error1, rgb[2].error1) = GetError(AddPattern(windowSizeBmp, pattern1), x, y);
                (rgb[0].error2, rgb[1].error2, rgb[2].error2) = GetError(AddPattern(windowSizeBmp, pattern2), x, y);

                for (int i = 0; i < 3; i++)
                {
                    if (rgb[i].error1 <= rgb[i].error2)
                    {
                        rgb[i].max = rgb[i].secondHalfMin;
                    }
                    if (rgb[i].error1 >= rgb[i].error2)
                    {
                        rgb[i].min = rgb[i].firstHalfMax;
                    }
                }
            }

            int[] rgbBest = new int[3];
            int error = 0;

            for (int i = 0; i < 3; i++)
            {
                if (rgb[i].error1 <= rgb[i].error2)
                {
                    rgbBest[i] = rgb[i].min;
                    error += rgb[i].error1;
                }
                else
                {
                    rgbBest[i] = rgb[i].max;
                    error += rgb[i].error2;
                }
            }
            return (GenerateColorARGB(rgbBest[0], rgbBest[1], rgbBest[2]), error);
        }

        /// <summary> Returns maximum of first half from given range. </summary>
        /// <param name="min">start of range</param>
        /// <param name="max">end of range</param>
        /// <returns>maximum of first half</returns>
        private int GetFirstHalfMax(int min, int max)
        {
            return min + (max - min) / 2;
        }

        /// <summary>Returns a color in ARGB format. Alpha channel is set to 255.</summary>
        /// <param name="red">value of red channel</param>
        /// <param name="green">value of green channel</param>
        /// <param name="blue">value of blue channel</param>
        /// <returns>color in ARGB format</returns>
        private int GenerateColorARGB(int red, int green, int blue)
        {
            return 255 << 24 | red << 16 | green << 8 | blue;
        }

        /// <summary>Compute an error between given bitmap and the original image.</summary>
        /// <param name="windowSizeBmp">window size bitmap cropped from the output image and with added pattern</param>
        /// <param name="x">x-coordinate of pattern in the output image</param>
        /// <param name="y">y-coordinate of pattern in the output image</param>
        /// <returns>error values for each rgb channel</returns>
        private (int, int, int) GetError(Bitmap windowSizeBmp, int x, int y)
        {
            int redError = 0;
            int greenError = 0;
            int blueError = 0;

            for (int j = y - HalfWindowSize; j < y + HalfWindowSize; j++)
            {
                for (int i = x - HalfWindowSize; i < x + HalfWindowSize; i++)
                {
                    Color originalPixel = OriginalImage.GetPixel(i, j);
                    Color pixel2 = windowSizeBmp.GetPixel(i - x + HalfWindowSize, j - y + HalfWindowSize);
                    redError += Math.Abs(originalPixel.R - pixel2.R);
                    greenError += Math.Abs(originalPixel.G - pixel2.G);
                    blueError += Math.Abs(originalPixel.B - pixel2.B);
                }
            }

            return (redError, greenError, blueError);
        }

        /// <summary> Returns the region indexes that do not overlap the side padding. </summary>
        /// <param name="index">centre of region</param>
        /// <param name="region">size of region</param>
        /// <param name="xCoord">indicates whether it is x- or y-coordinate</param>
        /// <returns>x- or y-coordinates of region</returns>
        private (int, int) GetIndexesOfRegion(int index, int region, bool xCoord)
        {
            int start = index - region / 2;
            int end = start + region;
            if (start <= HalfWindowSize)
            {
                return (HalfWindowSize, end);
            }
            else
            {
                int size = OriginalImage.Height;
                if (xCoord)
                {
                    size = OriginalImage.Width;
                }
                if (end >= size - HalfWindowSize)
                {
                    return (start, size - HalfWindowSize);
                }
                return (start, end);
            }
        }

        /// <summary> WindowSize is size where fits pattern with any rotation about centre. </summary>
        /// <param name="patternSize">size of pattern</param>
        /// <returns>appropriate size of window</returns>
        private int PatternSizeToWindowSize(int patternSize)
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