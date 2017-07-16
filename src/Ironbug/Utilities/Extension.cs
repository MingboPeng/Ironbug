using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;

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


    }
}