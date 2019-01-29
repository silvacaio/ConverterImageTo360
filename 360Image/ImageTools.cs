using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace _360Image
{
    //Reference: https://facebook360.fb.com/editing-360-photos-injecting-metadata/
    public class ImageTools
    {
        private const string _make = "RICOH";
        private const string _model = "RICOH THETA S";

        public Image Image { get; private set; }

        public ImageTools(Image image)
        {
            this.Image = image;
        }

        /// <summary>
        /// Add  properties values image to facebook recognize with 360º 
        /// </summary>
        public void ConvertImageTo360()
        {
            ResizeImageTo360();

            //Add make decorator
            var prop = GetPropertyImage(EnumDecoratorImage.make);
            SetValuePropertyImage(_make, prop, 2);

            //Add model decorator
            prop = GetPropertyImage(EnumDecoratorImage.model);
            SetValuePropertyImage(_model, prop, 2);
        }

        /// <summary>
        /// Sets value to property that decor image how 360º
        /// </summary>
        /// <param name="valueText"> Value of property: this value will be changed to byte array, to decor a image </param>
        /// <param name="prop">Property of image</param>    
        /// <param name="type">https://docs.microsoft.com/pt-br/dotnet/api/system.drawing.imaging.propertyitem.type?view=netframework-4.7.2</param> 
        private void SetValuePropertyImage(string valueText, PropertyItem prop, short type)
        {
            var value = GetValue(valueText);

            prop.Value = value.ToArray();
            prop.Len = prop.Value.Length;
            prop.Type = type;

            Image.SetPropertyItem(prop);
        }

        /// <summary>
        /// Convert value to byte array to set in property of image. Add in the ends of value the null byte
        /// </summary>
        /// <param name="value"> value to convert in byte array </param>
        /// <returns> byte array from value string </returns>
        private byte[] GetValue(string value)
        {
            var ret = Encoding.ASCII.GetBytes(value);
            return ret.Concat(new byte[] { 0 }).ToArray();

        }

        private PropertyItem GetPropertyImage(EnumDecoratorImage prop)
        {
            var property = (PropertyItem)Activator.CreateInstance(typeof(PropertyItem), BindingFlags.Instance | BindingFlags.NonPublic, null, new object[0], CultureInfo.InvariantCulture);
            property.Id = (int)prop;

            return property;
        }

        /// <summary>
        /// Resize image to 2:1 ratio, to show how 360º
        /// </summary>
        private void ResizeImageTo360()
        {
            if (Image.With2X1Ratio())
                return;

            var width = Image.Size.Width;
            var height = width / 2;

            var target = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(target))
            {
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(Image, new Rectangle(0, 0, width, height), 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, wrapMode);
                }

                Image = target;
            }
        }

        public enum EnumDecoratorImage
        {
            make = 0x010F,
            model = 0x0110
        }
    }
}
