using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Aoc
{
    [TestFixture]
    public class Day4
    {
        [Test]
        public void Part1()
        {
            var allData = File.ReadAllLines(@"C:\dev\AdventOfCode\2017\Aoc\Day4-1.txt");

            var valid = allData.Count(IsValid);

            Console.WriteLine(valid);
        }

        [Test]
        public void Part2()
        {
            var allData = File.ReadAllLines(@"C:\dev\AdventOfCode\2017\Aoc\Day4-1.txt");

            var valid = allData.Count(l => IsValid(l, true));

            Console.WriteLine(valid);
        }

        private static bool IsValid(string line) => IsValid(line, false);
        private static bool IsValid(string line, bool normaliseAnagrams)
        {
            var allTokens = new Queue<string>(line.Split(' '));
            var seen = new List<string>();

            while (allTokens.Count > 0)
            {
                var current = allTokens.Dequeue();

                if (normaliseAnagrams)
                {
                    current = string.Join("", current.OrderBy(c => c));
                }

                if (!seen.Contains(current))
                {
                    seen.Add(current);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}
