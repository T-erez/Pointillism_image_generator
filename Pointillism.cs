using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Pointillism_image_generator
{
    public struct Pattern
    {
        /// <summary>
        /// Structure for patterns. Patterns are squares.
        /// 
        /// int xIndex:  x-coordinate of the centre of the pattern in the generated image
        /// int yIndex:  y-coordinate of the centre of the pattern in the generated image
        /// int colorARGB:  color of pattern in ARGB
        /// int angle:  angle of rotation about the centre
        /// </summary>
        
        public int xIndex;
        public int yIndex;
        public int colorARGB;
        public int angle;
        public Pattern(int xIndex, int yIndex, int color, int angle)
        {
            this.xIndex = xIndex;
            this.yIndex = yIndex;
            this.colorARGB = color;
            this.angle = angle;
        }
    }

    public struct RGBchannel
    {
        /// <summary>
        /// RGBchannel structure represents one rgb channel and contains values to find the best color for a pattern.
        /// 
        /// int min:  the lowest intestity
        /// int max:  the highest intensity
        /// int firstHalfMax:  the maximum value in the first half of the intensity range
        /// int secondHalfMin:  the minimum value in the second half of the intensity range
        /// int error1:  error of a pattern with color equal to firstHalfMax
        /// int error2:  error of a pattern with color equal to secondHalfMin 
        /// </summary>
        
        public int min;
        public int max;
        public int firstHalfMax;
        public int secondHalfMin;
        public int error1;
        public int error2;

        public RGBchannel(int min, int max)
        {
            this.min = min;
            this.max = max;
            this.firstHalfMax = 0;
            this.secondHalfMin = 0;
            this.error1 = 0;
            this.error2 = 0;
        }
    }

    public class Pointillism
    {
        /// <summary>
        /// A class for generating a pointillistic image.
        /// 
        /// Bitmap OriginalImage:  input image
        /// Bitmap OutputImage:  generated image
        /// int NumberOfPatterns:  number of patterns pasted to OutputImage
        /// int PatternSize:  size of square patterns
        /// int WindowSize: size where fits pattern with any rotation about centre
        /// int HalfWindowSize: WindowSize / 2
        /// SmartHeap PatternsToAdd:  heap with patterns to add 
        /// int PixelMultiple:  defines pixels on which to generate a pattern
        /// Bitmap WindowSizeBitmap:  bitmap of maximum size that is affected by pasting a pattern 
        /// </summary>

        private Bitmap OriginalImage;
        private Bitmap OutputImage;
        private int NumberOfPatterns;

        private int PatternSize;
        private int WindowSize;
        private int HalfWindowSize;
        private SmartHeap PatternsToAdd;

        static int PixelMultiple = 4;

        private Bitmap WindowSizeBitmap;

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
            Bitmap whiteCanvas = new Bitmap(WindowSize, WindowSize, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(WindowSizeBitmap)) { g.Clear(Color.White); }
            for (int j = HalfWindowSize; j < originalImage.Height + HalfWindowSize; j += PixelMultiple)
            {
                for (int i = HalfWindowSize; i < originalImage.Width + HalfWindowSize; i += PixelMultiple)
                {
                    (int redError, int greenError, int blueError) = GetError(whiteCanvas, i, j);
                    int error0 = redError + greenError + blueError;
                    PatternsToAdd.Add(GetBestPatternOnIndex(i, j), error0);
                }
            }
        }

        public bool GeneratePointilismImage()
        {
            // Add the best possible pattern. If no pattern was added, return false. 
            return AddBestOfBestPatterns();
        }

        public Bitmap GetOutputImage()
        {
            return OutputImage.Clone(new Rectangle(
                HalfWindowSize,
                HalfWindowSize,
                OriginalImage.Width - WindowSize,
                OriginalImage.Height - WindowSize),
                PixelFormat.Format24bppRgb);
        }

        public int GetNumberOfPatterns()
        {
            return NumberOfPatterns;
        }

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

        private bool AddBestOfBestPatterns()
        {
            // Get a pattern with the best improvement and add it to an output image. 

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

        private Node GetBestPatternOnIndex(int xIndex, int yIndex)
        {
            // Find a pattern on given index that the best reflects the original image. 

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

        private (int, int) GetBestColorOfPattern(Bitmap windowSizeBmp, Pattern pattern1)
        {
            // A binary search performed seperately on each rgb channel.

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

        private int GetFirstHalfMax(int min, int max)
        {
            return min + (max - min) / 2;
        }

        private int GenerateColorARGB(int red, int green, int blue)
        {
            return 255 << 24 | red << 16 | green << 8 | blue;
        }

        private (int, int, int) GetError(Bitmap windowSizeBmp, int x, int y)
        {
            // Bitmap is a window size region of the output image with a new pattern.
            // The coordinates of a pattern in the output image are (x, y).

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

        private (int, int) GetIndexesOfRegion(int index, int region, bool xAxis)
        {
            // Returns the region indexes that do not overlap the side padding.

            int start = index - region / 2;
            int end = start + region;
            if (start <= HalfWindowSize)
            {
                return (HalfWindowSize, end);
            }
            else
            {
                int size = OriginalImage.Height;
                if (xAxis)
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