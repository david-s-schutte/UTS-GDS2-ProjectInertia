using System;
using System.Collections.Generic;
using System.Linq;

namespace Surfer.Utilities
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Gets the lowest key value in a given dictionary O(log n)
        /// </summary>
        public static TKey GetLowestKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary) where TKey : IComparable
        {
            TKey key = dictionary.Aggregate((x, y) => IsLessThan(x.Key, y.Key) ? x : y).Key;
            return key;
        }

        /// <summary>
        /// Gets the highest key value in a given dictionary O(log n)
        /// </summary>
        public static TKey GetHighestKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
            where TKey : IComparable
        {
            TKey key = dictionary.Aggregate((x, y) => IsGreaterThan(x.Key, y.Key) ? x : y).Key;
            return key;
        }

        private static bool IsGreaterThan<T>(this T value, T other) where T : IComparable => value.CompareTo(other) > 0;

        private static bool IsLessThan<T>(this T value, T other) where T : IComparable => value.CompareTo(other) < 0;
    }
}