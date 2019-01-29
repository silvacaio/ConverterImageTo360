using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace _360Image
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Reading the image");

            var image = Image.FromFile(@"LOCAL IMAGE TO 360");
            var tools = new ImageTools(image);
            tools.ConvertImageTo360();

            tools.Image.Save(@"PATH OUTPUT IMAGE 360", ImageFormat.Jpeg);

            Console.WriteLine("Conversion of image finished");

            Console.ReadLine();

        }
    }
}
