using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Aoc2018
{
    [TestFixture]
    public class Day5
    {
        [TestCase("aA", "")]
        [TestCase("abBA", "")]
        [TestCase("abAB", "abAB")]
        [TestCase("aabAAB", "aabAAB")]
        public void Rules(string input, string expectation)
        {
            var chain = input;
            var reduced = React(chain);
            Assert.That(reduced, Is.EqualTo(expectation));
        }

        [Test]
        public void Example()
        {
            var chain = "dabAcCaCBAcCcaDA";

            var reduced = React(chain);

            Assert.That(reduced, Is.EqualTo("dabCBAcaDA"));
        }

        [Test]
        public void Example2()
        {
            var chain = "dabAcCaCBAcCcaDA";

            var shortest = CalculateShortestPolymer(chain);

            Assert.That(shortest, Is.EqualTo(4));
        }

        [Test]
        public void Test1()
        {
            var chain = File.ReadAllText("Day5.txt");

            var reduced = React(chain);

            Assert.That(reduced.Length, Is.EqualTo(10250));
        }

        [Test]
        public void Test2()
        {
            var chain = File.ReadAllText("Day5.txt");

            var shortest = CalculateShortestPolymer(chain);

            Assert.That(shortest, Is.EqualTo(6188));
        }

        public int CalculateShortestPolymer(string chain)
        {
            var letters = Enumerable.Range(65, 26).Select(i => (char) i).ToList();

            var shortest = int.MaxValue;

            foreach (var letter in letters)
            {
                var newChain = chain.Replace(letter.ToString(), "", StringComparison.CurrentCultureIgnoreCase);
                var reduced = React(newChain);

                if (reduced.Length < shortest)
                {
                    shortest = reduced.Length;
                }
            }

            return shortest;
        }


        private string React(string chain)
        {
            var modified = false;
            for (var index = 0; index < chain.Length - 1; index++)
            {
                var character = chain[index].ToString();
                var next = chain[index + 1].ToString();

                if (string.Equals(character, next, StringComparison.CurrentCultureIgnoreCase)
                    && !string.Equals(character, next, StringComparison.CurrentCulture))
                {
                    chain = chain.Remove(index + 1, 1);
                    chain = chain.Remove(index, 1);
                    modified = true;
                }
            }

            return modified ? React(chain) : chain;
        }
    }
}
