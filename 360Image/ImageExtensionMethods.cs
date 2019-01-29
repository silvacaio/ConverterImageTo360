using System.Drawing;

namespace _360Image
{
    public static class ImageExtensionMethods
    {
        public static bool With2X1Ratio(this Image self)
        {
            if (self == null)
                return false;

            return (self.Size.Width / 2) == self.Size.Height;
        }
    }
}
