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
            var input = "3, 4, 1, 5";
            
            var list = new CircularList<int>(4).Process(input);

            Assert.That(list.Checksum, Is.EqualTo(12));
        }

        [Test]
        public void Part1()
        {
            var input = "18,1,0,161,255,137,254,252,14,95,165,33,181,168,2,188";

            var list = new CircularList<int>(255).Process(input);

            Assert.That(list.Checksum, Is.EqualTo(46600));
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
                
                foreach (var value in inputParsed)
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
                => CurrentPosition = CurrentPosition + value >= Count
                    ? CurrentPosition + value - Count
                    : CurrentPosition + value;

            public override string ToString()
            {
                var stringOutput = this.Select(x => x.ToString()).ToList();
                stringOutput[CurrentPosition] = "[" + stringOutput[CurrentPosition] + "]";
                return string.Join(" ", stringOutput);
            }
        }
    }
}
