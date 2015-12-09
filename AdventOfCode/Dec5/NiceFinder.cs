using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Dec5
{
    public class NiceFinder
    {
        private static bool ContainsLetterPairs(string s) => Enumerable.Range(1, s.Length - 1).Any(i => s[i] == s[i - 1]);
        public static bool ContainsThreeVowels(string s) => "aeiou".Sum(vowel => s.Count(c => c == vowel)) >= 3;

        public bool IsNice(string s)
        {
            if (new[] {"ab", "cd", "pq", "xy"}.Any(s.Contains))
            {
                return false;
            }

            return new List<Func<string, bool>>
            {
                ContainsThreeVowels,
                ContainsLetterPairs
            }
                .All(rule => rule(s));
        }

    }
}