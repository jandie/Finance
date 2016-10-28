using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Utils
{
    public static class RanUtil
    {
        /// <summary>
        /// The static random that is used in this utility.
        /// </summary>
        private static Random _random;

        /// <summary>
        /// Generates a random string of lowercase letters and numbers.
        /// </summary>
        /// <param name="size">The length of the string.</param>
        /// <returns>The random string.</returns>
        public static string RandomString(int size)
        {
            if (_random == null) _random = new Random();

            const string input = "abcdefghijklmnopqrstuvwxyz0123456789";

            IEnumerable<char> chars = Enumerable.Range(0, size)
                                   .Select(x => input[_random.Next(0, input.Length)]);

            return new string(chars.ToArray());
        }
    }
}
