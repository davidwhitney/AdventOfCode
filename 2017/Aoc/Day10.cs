using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Aoc
{
    [TestFixture]
    public class Day10
    {
        [Test]
        public void Sample1()
        {
            var list = new CircularList<int>(4).Process("3, 4, 1, 5");

            Assert.That(list.Checksum, Is.EqualTo(12));
        }

        [TestCase("", "a2582a3a0e66e6e86e3812dcb672a272")]
        [TestCase("AoC 2017", "33efeb34ea91902bb2f59c9920caa6cd")]
        [TestCase("1,2,3", "3efbe78a8d82f29979031a4aa0b16a9d")]
        [TestCase("1,2,4", "63960835bcdc130f0b66d7ff4f6a5a8e")]
        public void Sample2(string input, string expectedOutcome)
        {
            var list = new CircularList<int>(255);

            for (var i = 0; i < 64; i++)
            {
                list.ProcessAsChars(input);
            }
            
            Assert.That(list.DenseHash(), Is.EqualTo(expectedOutcome));
        }

        [Test]
        public void Part1()
        {
            var list = new CircularList<int>(255).Process("18,1,0,161,255,137,254,252,14,95,165,33,181,168,2,188");

            Assert.That(list.Checksum, Is.EqualTo(46600));
        }

        [Test]
        public void Part2()
        {
            var list = new CircularList<int>(255);

            for (var i = 0; i < 64; i++)
            {
                list.ProcessAsChars("18,1,0,161,255,137,254,252,14,95,165,33,181,168,2,188");
            }
            
            Assert.That(list.DenseHash(), Is.EqualTo("23234babdc6afa036749cfa9b597de1b"));
        }

        public class CircularList<T> : List<int>
        {
            public int CurrentPosition { get; set; }
            public int SkipSize { get; set; }
            public int Checksum => this[0] * this[1];

            public CircularList(int length) : base(Enumerable.Range(0, ++length)) { }

            public CircularList<T> Process(string sequence)
            {
                var inputParsed = sequence.Split(',').Select(x => int.Parse(x.Trim())).ToList();
                return Process(inputParsed);
            }

            public CircularList<T> ProcessAsChars(string sequence)
            {
                var inputParsed = sequence.Select(x => (int)x).ToList();
                inputParsed.AddRange(new[] { 17, 31, 73, 47, 23 });
                return Process(inputParsed);
            }

            public CircularList<T> Process(IEnumerable<int> sequence)
            {
                foreach (var value in sequence)
                {
                    Reverse(value);
                    AddToCurrentPosition(value);
                    AddToCurrentPosition(SkipSize);
                    SkipSize++;
                    Console.WriteLine(this);
                }
                return this;
            }

            public void Reverse(int numberToReverse)
            {
                var tempOffset1 = CurrentPosition;
                var selection = new List<int>();
                for (var takeCount = 0; takeCount < numberToReverse; takeCount++)
                {
                    selection.Add(this[tempOffset1]);
                    tempOffset1++;
                    tempOffset1 = tempOffset1 > Count - 1 ? 0 : tempOffset1;
                }

                selection.Reverse();

                var tempOffset = CurrentPosition;
                foreach (var item in selection)
                {
                    this[tempOffset] = item;

                    tempOffset++;
                    tempOffset = tempOffset > Count - 1 ? 0 : tempOffset;
                }
            }

            public void AddToCurrentPosition(int value)
            {
                var next = CurrentPosition + value;
                while (next >= Count)
                {
                    next = next - Count;
                }

                CurrentPosition = next;
            }

            public string DenseHash()
            {
                var hash = new List<int>();
                for (var i = 0; i < 16; i++)
                {
                    var numbers = this.Skip(i * 16).Take(16);
                    hash.Add(numbers.Aggregate(0, (current, number) => current ^ number));
                }

                return hash.Aggregate("", (current, hashVal) => current + hashVal.ToString("X2")).ToLower();
            }

            public override string ToString()
            {
                var stringOutput = this.Select(x => x.ToString()).ToList();
                stringOutput[CurrentPosition] = "[" + stringOutput[CurrentPosition] + "]";
                return string.Join(" ", stringOutput);
            }
        }
    }
}
