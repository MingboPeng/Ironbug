using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Ironbug
{
    public static class Extensions
    {
        /// <summary>
        /// Determines whether the collection is null or contains no elements.
        /// </summary>
        /// <typeparam name="T">The IEnumerable type.</typeparam>
        /// <param name="enumerable">The enumerable, which may be null or empty.</param>
        /// <returns>
        ///     <c>true</c> if the IEnumerable is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }
            /* If this is a list, use the Count property. 
             * The Count property is O(1) while IEnumerable.Count() is O(N). */
            var collection = enumerable as ICollection<T>;
            if (collection != null)
            {
                return collection.Count < 1;
            }
            return enumerable.Any();
        }

        /// <summary>
        /// Determines whether the collection is null or contains no elements.
        /// </summary>
        /// <typeparam name="T">The IEnumerable type.</typeparam>
        /// <param name="collection">The collection, which may be null or empty.</param>
        /// <returns>
        ///     <c>true</c> if the IEnumerable is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            if (collection == null)
            {
                return true;
            }
            return collection.Count < 1;
        }

        /// <summary>
        /// Gets all connected components.
        /// </summary>
        /// <typeparam name="IGH_Param">The IGH_Param type.</typeparam>
        /// <param name="outputParam">The outputParam</param>
        /// <returns>
        ///     <c>List<GH_Component></c>.
        /// </returns>
        public static List<IGH_DocumentObject> DirectConnectedComponents(this IGH_Param outputParam)
        {
            var connectedComponent = new List<IGH_DocumentObject>();

            var recipients = outputParam.Recipients;
            if (recipients.IsNullOrEmpty()) return connectedComponent;

            foreach (var item in recipients)
            {
                var recipientComponent = item.Attributes.GetTopLevel.DocObject;
                connectedComponent.Add(recipientComponent);


            }
            return connectedComponent;
        }

        /// <summary>
        /// Draws a circle. https://rosettacode.org/wiki/Bitmap/Midpoint_circle_algorithm#C.23
        /// </summary>
        /// <param name="image">
        /// The destination image.
        /// </param>
        /// <param name="centerX">
        /// The x center position of the circle.
        /// </param>
        /// <param name="centerY">
        /// The y center position of the circle.
        /// </param>
        /// <param name="radius">
        /// The radius of the circle.
        /// </param>  
        /// <param name="color">
        /// The color to use.
        /// </param>    
        public static void DrawCircle(this Bitmap image, int centerX, int centerY, int radius, Color color)
        {
            int d = (5 - radius * 4) / 4;
            int x = 0;
            int y = radius;
            
            do
            {
                // ensure index is in range before setting (depends on your image implementation)
                // in this case we check if the pixel location is within the bounds of the image before setting the pixel
                if (centerX + x >= 0 && centerX + x <= image.Width - 1 && centerY + y >= 0 && centerY + y <= image.Height - 1) image.SetPixel(centerX + x, centerY + y, color);
                if (centerX + x >= 0 && centerX + x <= image.Width - 1 && centerY - y >= 0 && centerY - y <= image.Height - 1) image.SetPixel(centerX + x, centerY - y, color);
                if (centerX - x >= 0 && centerX - x <= image.Width - 1 && centerY + y >= 0 && centerY + y <= image.Height - 1) image.SetPixel(centerX - x, centerY + y, color);
                if (centerX - x >= 0 && centerX - x <= image.Width - 1 && centerY - y >= 0 && centerY - y <= image.Height - 1) image.SetPixel(centerX - x, centerY - y, color);
                if (centerX + y >= 0 && centerX + y <= image.Width - 1 && centerY + x >= 0 && centerY + x <= image.Height - 1) image.SetPixel(centerX + y, centerY + x, color);
                if (centerX + y >= 0 && centerX + y <= image.Width - 1 && centerY - x >= 0 && centerY - x <= image.Height - 1) image.SetPixel(centerX + y, centerY - x, color);
                if (centerX - y >= 0 && centerX - y <= image.Width - 1 && centerY + x >= 0 && centerY + x <= image.Height - 1) image.SetPixel(centerX - y, centerY + x, color);
                if (centerX - y >= 0 && centerX - y <= image.Width - 1 && centerY - x >= 0 && centerY - x <= image.Height - 1) image.SetPixel(centerX - y, centerY - x, color);
                if (d < 0)
                {
                    d += 2 * x + 1;
                }
                else
                {
                    d += 2 * (x - y) + 1;
                    y--;
                }
                x++;
            } while (x <= y);
        }

        /// <summary>Saves the images as frames to an animated Gif Image.</summary>
        /// <param name="images">The images to save.</param>
        /// <param name="path">The path of the Gif file to create.</param>
        /// <param name="delay">The delay between frames, in milliseconds.</param>
        /// <param name="repeat">The number of times the animation should repeat. Leave this zero 
        /// for it to loop forever, or specify a value to limit the number of repetitions.</param>
        public static void SaveAnimatedGifImage(this IEnumerable<Bitmap> images, string path, int delay = 100, int repeat = 0)
        {
            var imageArray = images.ToArray();

            using (var stream = new MemoryStream())
            {
                using (var encoder = new BumpKit.GifEncoder(stream, null, null, repeat))
                {
                    for (int i = 0; i < imageArray.Length; i++)
                    {
                        encoder.AddFrame(imageArray[i].CopyImage(), 0, 0, TimeSpan.FromMilliseconds(delay));
                    }
                }

                stream.Position = 0;
                using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 0x2000, false))
                {
                    stream.WriteTo(fileStream);
                }
            }
        }

        #region CopyImage

        /// <summary>Creates a 24 bit-per-pixel copy of the source image.</summary>
        public static Image CopyImage(this Image image)
        {
            return CopyImage(image, PixelFormat.Format24bppRgb);
        }

        /// <summary>Creates a copy of the source image with the specified pixel format.</summary><remarks>
        /// This can also be achieved with the <see cref="System.Drawing.Bitmap.Clone(int, int, PixelFormat)"/>
        /// overload, but I have had issues with that method.</remarks>
        public static Image CopyImage(this Image image, PixelFormat format)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            // Don't try to draw a new Bitmap with an indexed pixel format.
            if (format == PixelFormat.Format1bppIndexed || format == PixelFormat.Format4bppIndexed || format == PixelFormat.Format8bppIndexed || format == PixelFormat.Indexed)
                return (image as Bitmap).Clone(new Rectangle(0, 0, image.Width, image.Height), format);

            Image result = null;
            try
            {
                result = new Bitmap(image.Width, image.Height, format);

                using (var graphics = Graphics.FromImage(result))
                {
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    graphics.DrawImage(image, 0, 0, result.Width, result.Height);
                }
            }
            catch
            {
                if (result != null)
                    result.Dispose();

                throw;
            }
            return result;
        }

        #endregion
        
    }
}