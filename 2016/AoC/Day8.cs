using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            var instructions = new List<string>();
            instructions.Add("rect 3x2");
            instructions.Add("rotate column x=1 by 1");
            instructions.Add("rotate row y=0 by 4");
            instructions.Add("rotate column x=1 by 1");

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
            throw new NotImplementedException();}

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
