using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoC.Infrastructure;
using NUnit.Framework;

namespace AoC
{
    [TestFixture]
    public class Day2 : Challenge
    {
        [Test]
        public void Sample()
        {
            var result = PhonePad.Default.Translate(new List<string>
            {
                "ULL",
                "RRDDD",
                "LURDL",
                "UUUUD"
            });

            Assert.That(result, Is.EqualTo("1985"));
        }

        [Test]
        public void Sample2()
        {
            var result = PhonePad.Radial.Translate(new List<string>
            {
                "ULL",
                "RRDDD",
                "LURDL",
                "UUUUD"
            });

            Assert.That(result, Is.EqualTo("5DB3"));
        }

        [Test]
        public void Test()
        {
            var result = PhonePad.Default.Translate(Input);

            Assert.That(result, Is.EqualTo("35749"));
        }

        [Test]
        public void Test2()
        {
            var result = PhonePad.Radial.Translate(Input);

            Assert.That(result, Is.EqualTo("9365C"));
        }

        public class PhonePad : List<List<string>>
        {
            private readonly int[] _finger;
            public string Current => ValueAt(_finger[0],_finger[1]);
            public string ValueAt(int x, int y) => this[y][x];

            public static PhonePad Default => new PhonePad(new List<List<string>>
            {
                new List<string> {"1", "2", "3"},
                new List<string> {"4", "5", "6"},
                new List<string> {"7", "8", "9"}
            }, new[] {1, 1});

            public static PhonePad Radial => new PhonePad(new List<List<string>>
            {
                new List<string> {"-", "-", "1", "-", "-"},
                new List<string> {"-", "2", "3", "4", "-"},
                new List<string> {"5", "6", "7", "8", "9"},
                new List<string> {"-", "A", "B", "C", "-"},
                new List<string> {"-", "-", "D", "-", "-"},
            }, new[] {0, 2});

            private PhonePad(List<List<string>> pattern, int[] start)
            {
                AddRange(pattern);
                _finger = start;
            }

            public string Translate(IEnumerable<string> sequence)
            {
                var buffer = new List<string>();
                foreach (var line in sequence)
                {
                    line.ToList().ForEach(Process);
                    buffer.Add(Current);
                }

                return string.Join("", buffer);
            }

            private void Process(char move)
            {
                var delta = new Dictionary<char, int> {{'U', -1}, {'L', -1}, {'D', 1}, {'R', 1}}[move];
                var axis = new Dictionary<char, int> {{'U', 1}, {'L', 0}, {'D', 1}, {'R', 0}}[move];
                var preview = _finger[axis] + delta;

                var boundsCheckLength = axis == 0 ? this.First().Count : Count;
                boundsCheckLength--;

                if (preview <= boundsCheckLength 
                    && preview >= 0)
                {
                    _finger[axis] += delta;

                    if (Current == "-")
                    {
                        _finger[axis] -= delta;
                    }
                }
            }
        }
    }
}
