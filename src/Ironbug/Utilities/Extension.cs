using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using System.Drawing;

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
    }
}