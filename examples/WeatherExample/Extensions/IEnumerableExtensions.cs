using System;
using System.Collections.Generic;

namespace Phlank.Responder.WeatherExample.Extensions
{
    public static class IEnumerableExtensions
    {
        // Credit to Jon Skeet: https://stackoverflow.com/a/648240
        public static T RandomElement<T>(this IEnumerable<T> source,
                                 Random rng)
        {
            T current = default(T);
            int count = 0;
            foreach (T element in source)
            {
                count++;
                if (rng.Next(count) == 0)
                {
                    current = element;
                }
            }
            if (count == 0)
            {
                throw new InvalidOperationException("Sequence was empty");
            }
            return current;
        }
    }
}
