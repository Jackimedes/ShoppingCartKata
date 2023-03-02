namespace ShoppingCartKata.Common
{
    public static class Extensions
    {
        /// <summary>
        /// Determines whether a sequence is null or contains any values
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns>True is the sequence is null or empty, otherwise false</returns>
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
            => source == null || !source.Any();

        /// <summary>
        /// Determines whether a single element of a sequence satisfies a condition and returns that value
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source</typeparam>
        /// <param name="source">A System.Collections.Generic.IEnumerable whose elements to apply the predicate to</param>
        /// <param name="predicate"></param>
        /// <param name="result">A single value of satisfied by the predicate</param>
        /// <exception cref="ArgumentNullException">Source or predicate is null</exception>
        /// <returns>True if only one element in the source sequence matches specified predicate, otherwise false</returns>
        public static bool AnySingleOut<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, out TSource result)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#pragma warning disable CS8601 // Possible null reference assignment.
            result = default;

            foreach (TSource item in source)
            {
                if (predicate(item))
                {
                    if (!result.EqualToDefault())
                    {

                        result = default;
#pragma warning restore CS8601 // Possible null reference assignment.
                        return false;
                    }

                    result = item;
                }
            }

            return !result.EqualToDefault();
        }

        /// <summary>
        /// Determines whether an object is equal to the object types default value
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source</typeparam>
        /// <param name="obj">The object to be compared to the default value</param>
        /// <returns>True if the object is equal to the default value, otherwise false</returns>
        public static bool EqualToDefault<TSource>(this TSource obj)
        {
            if (default(TSource) == null)
                return obj == null;
            else
                return default(TSource)!.Equals(obj);
        }

        /// <summary>
        /// Remove a list of items from a list
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="itemsToRemove"></param>
        public static void RemoveRange<TSource>(this List<TSource> source, IEnumerable<TSource> itemsToRemove)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (itemsToRemove == null)
                throw new ArgumentNullException(nameof(itemsToRemove));

            foreach (TSource item in itemsToRemove)
            {
                if (source.Contains(item))
                {
                    source.Remove(item);
                }
            }
        }
    }
}