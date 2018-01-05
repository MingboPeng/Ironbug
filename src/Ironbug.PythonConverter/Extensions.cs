using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.PythonConverter
{
    public static class Extensions
    {
        
        public static string UpperInitial(string name)
        {
            return name[0].ToString().ToUpperInvariant() + name.Substring(1);
        }

        public static string CheckNamingForCS(this string Name)
        {
            if (Name.Contains('.')) //check for namespaces
            {
                var names = Name.Split('.').Select(name => UpperInitial(name));
                return string.Join(".", names);
            }
            else
            {
                return UpperInitial(Name);
            }
        }
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
            if (enumerable is ICollection<T> collection)
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

    }
}
