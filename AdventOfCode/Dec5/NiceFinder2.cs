using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Dec5
{
    public class NiceFinder2
    {
        public bool IsNice(string s)
        {
            if (new[] {"ab", "cd", "pq", "xy"}.Any(s.Contains))
            {
                return false;
            }

            return new List<Func<string, bool>>
            {
                ContainsSpacedRepeats,
                ContainsLetterPairs
            }
                .All(rule => rule(s));
        }

        public static bool ContainsLetterPairs(string s)
        {
            var pairs = IndexPairs(s);

            Tuple<char, char> lastPair = null;
            foreach (var pair in pairs)
            {
                if (MatchingPair(pair, lastPair) && OverlappingPair(lastPair, pair))
                {
                    return false;
                }

                lastPair = pair;
            }

            var distinctPairs = pairs.Distinct().Count();
            return pairs.Count != distinctPairs;
        }

        private static List<Tuple<char, char>> IndexPairs(string s)
        {
            var pairs1 = new List<Tuple<char, char>>();
            for (var index = 0; index < s.Length - 1; index = index + 2)
            {
                pairs1.Add(new Tuple<char, char>(s[index], s[index + 1]));
            }

            var pairs2 = new List<Tuple<char, char>>();
            for (int index = 1; index < s.Length - 1; index = index + 2)
            {
                pairs2.Add(new Tuple<char, char>(s[index], s[index + 1]));
            }

            var pairs = new List<Tuple<char, char>>();
            pairs.AddRange(pairs1);
            pairs.AddRange(pairs2);
            return pairs;
        }

        private static bool OverlappingPair(Tuple<char, char> lastPair, Tuple<char, char> pair)
        {
            return lastPair.Item2 == pair.Item1;
        }

        private static bool MatchingPair(Tuple<char, char> pair, Tuple<char, char> lastPair)
        {
            return pair.Item1 == lastPair?.Item1
                   && pair.Item2 == lastPair.Item2;
        }

        public static bool ContainsSpacedRepeats(string s)
        {
            for (int index = 0; index < s.Length; index++)
            {
                var letter = s[index];
            }

            return false;
        }
    }
}