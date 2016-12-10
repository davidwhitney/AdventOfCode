using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using AoC.Infrastructure;
using NUnit.Framework;

namespace AoC
{
    [TestFixture]
    public class Day8 : Challenge
    {
        [Test]
        public void Sample()
        {
            var instructions = new List<string>
            {
                "rect 3x2",
                "rotate column x=1 by 1",
                "rotate row y=0 by 4",
                "rotate column x=1 by 1"
            };

            var screen = new Screen(7, 3);
            screen.Process(instructions);

            Assert.That(screen.ToString(), Is.EqualTo(@"
.#..#.#
#.#....
.#.....".TrimStart()));
        }

        [Test]
        public void ToString_PrintsGrid()
        {
            var screen = new Screen(4,4);

            Assert.That(screen.ToString(), Is.EqualTo(@"
....
....
....
....".TrimStart()));
        }

        [Test]
        public void Process_CreateInstruction_Creates()
        {
            var screen = new Screen(7, 3);
            screen.Process(new List<string>
            {
                "rect 3x2"
            });

            Assert.That(screen.ToString(), Is.EqualTo(@"
###....
###....
.......".TrimStart()));
        }

        [Test]
        public void Test()
        {
            var screen = new Screen(50, 6);
        }
    }

    public class Screen
    {
        private readonly List<string> _storage;

        public Screen(int x, int y)
        {
            _storage = new List<string>();
            for (var i = 0; i < y; i++)
            {
                _storage.Add(new string('.', x));
            }
        }

        public void Process(List<string> instructions)
        {
            var ops = new Dictionary<string, Action<Match>>
            {
                {@"rect ([0-9]+)x([0-9]+)", m => Fill(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value))}
            };

            foreach (var instruction in instructions)
            {
                foreach (var op in ops)
                {
                    var matches = Regex.Matches(instruction, op.Key, RegexOptions.Compiled);
                    if (matches.Count > 0)
                    {
                        op.Value(matches[0]);
                    }
                }
            }
        }

        private void Fill(int objGroup, int mGroup)
        {
        }

        public override string ToString()
        {
            var buffer = new StringBuilder();
            foreach (var row in _storage)
            {
                buffer.AppendLine(row);
            }
            return buffer.ToString().Trim();
        }
    }
}
