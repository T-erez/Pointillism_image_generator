using System;

namespace Pointillism_image_generator
{
    public struct Pattern
    {
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
}