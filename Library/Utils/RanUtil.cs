using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Utils
{
    public static class RanUtil
    {
        private static Random _random;

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
