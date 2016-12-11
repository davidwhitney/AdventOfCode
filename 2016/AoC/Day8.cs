using System;
using System.Collections.Generic;
using System.Linq;
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
        public void Process_RotateX_Shifts()
        {
            var screen = new Screen(7, 3);
            screen.Process(new List<string>
            {
                "rect 3x2",
                "rotate row y=0 by 1"
            });

            Assert.That(screen.ToString(), Is.EqualTo(@"
.###...
###....
.......".TrimStart()));
        }

        [Test]
        public void Process_RotateY_Shifts()
        {
            var screen = new Screen(7, 3);
            screen.Process(new List<string>
            {
                "rect 3x2",
                "rotate column x=0 by 1"
            });

            Assert.That(screen.ToString(), Is.EqualTo(@"
.##....
###....
#......".TrimStart()));
        }

        [Test]
        public void Test()
        {
            var screen = new Screen(50, 6);
        }
    }

    public class Screen
    {
        private readonly List<List<char>> _storage;

        public Screen(int x, int y)
        {
            _storage = new List<List<char>>();
            for (var i = 0; i < y; i++)
            {
                _storage.Add(Enumerable.Repeat('.', x).ToList());
            }
        }

        public void Process(List<string> instructions)
        {
            var ops = new Dictionary<string, Action<Match>>
            {
                {@"rect ([0-9]+)x([0-9]+)", m => Fill(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value))},
                {@"rotate row y=([0-9]+) by ([0-9]+)", m => ShiftRow(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value))},
                {@"rotate column x=([0-9]+) by ([0-9]+)", m => ShiftCol(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value))}
            };

            foreach (var instruction in instructions)
            {
                var op = ops.First(x => Regex.IsMatch(instruction, x.Key, RegexOptions.Compiled));
                var matches = Regex.Matches(instruction, op.Key, RegexOptions.Compiled);
                op.Value(matches[0]);
            }
        }

        private void ShiftCol(int x, int places)
        {
            var virtualRow = _storage.Select(row => row[x]).ToList();

            for (var i = 0; i < places; i++) virtualRow.Insert(0, '?');
            var wrap = virtualRow.Skip(_storage.Count).Reverse().ToList();
            for (var i = 0; i < places; i++) virtualRow.RemoveAt(0);

            foreach (var entry in wrap)
            {
                virtualRow.Insert(0, entry);
            }

            for (var y = 0; y < _storage.Count; y++)
            {
                _storage[y][x] = virtualRow[y];
            }
        }

        private void ShiftRow(int y, int places)
        {
            var targetMask = string.Join("", _storage[y]);
            var wrap = (new string(' ', places) + targetMask).Substring(_storage[y].Count, places);
            targetMask = wrap + targetMask;

            for (var x = 0; x < _storage[y].Count; x++)
            {
                var ch = targetMask[x];
                _storage[y][x] = ch;
            }
        }

        private void Fill(int xDimension, int yDimension)
        {
            for (var y = 0; y < yDimension; y++)
            for (var x = 0; x < xDimension; x++)
            {
                _storage[y][x] = '#';
            }
        }

        public override string ToString()
        {
            var buffer = new StringBuilder();
            foreach (var row in _storage)
            {
                buffer.AppendLine(string.Join("", row));
            }
            return buffer.ToString().Trim();
        }
    }
}
