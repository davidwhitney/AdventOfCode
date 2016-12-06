using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC
{
    [TestFixture]
    public class Day2
    {
        [Test]
        public void Sample()
        {
            var result = new PhonePad().Translate(new List<string>
            {
                "ULL",
                "RRDDD",
                "LURDL",
                "UUUUD"
            });

            Assert.That(result, Is.EqualTo(1985));
        }

        [Test]
        public void Test()
        {
            var contents = File.ReadAllLines("day2.txt");

            var result = new PhonePad().Translate(contents);

            Assert.That(result, Is.EqualTo(35749));
        }

        public class PhonePad : List<List<string>>
        {
            private readonly int[] _finger = {1, 1};
            public string Current => this[_finger[1]][_finger[0]];

            public PhonePad()
            {
                AddRange(new List<List<string>>
                {
                    new List<string> {"1", "2", "3"},
                    new List<string> {"4", "5", "6"},
                    new List<string> {"7", "8", "9"}
                });


                AddRange(new List<List<string>>
                {
                    new List<string> {"-", "-", "1", "-", "-"},
                    new List<string> {"-", "2", "3", "4", "-"},
                    new List<string> {"5", "6", "7", "8", "9"},
                    new List<string> {"-", "A", "B", "C", "-"},
                    new List<string> {"-", "-", "D", "-", "-"},
                });
            }

            public int Translate(IEnumerable<string> sequence)
            {
                var buffer = new List<string>();
                foreach (var line in sequence)
                {
                    line.ToList().ForEach(Process);
                    buffer.Add(Current);
                }

                var elements = string.Join("", buffer);
                return int.Parse(elements);
            }

            private void Process(char move)
            {
                var delta = new Dictionary<char, int> {{'U', -1}, {'L', -1}, {'D', 1}, {'R', 1}}[move];
                var axis = new Dictionary<char, int> {{'U', 1}, {'L', 0}, {'D', 1}, {'R', 0}}[move];
                var preview = _finger[axis] + delta;
                if (preview > 2 || preview < 0) return;
                _finger[axis] += delta;
            }
        }
    }
}
