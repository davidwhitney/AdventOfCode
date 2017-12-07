using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;

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

        private static bool IsValid(string line)
        {
            var allTokens = new Queue<string>(line.Split(' '));
            var seen = new List<string>();

            while (allTokens.Count > 0)
            {
                var current = allTokens.Dequeue();
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
