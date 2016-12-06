using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AoC
{
    [TestFixture]
    public class Day1
    {
        private string _input =
            "L2, L5, L5, R5, L2, L4, R1, R1, L4, R2, R1, L1, L4, R1, L4, L4, R5, R3, R1, L1, R1, L5, L1, R5, L4, R2, L5, L3, L3, R3, L3, R4, R4, L2, L5, R1, R2, L2, L1, R3, R4, L193, R3, L5, R45, L1, R4, R79, L5, L5, R5, R1, L4, R3, R3, L4, R185, L5, L3, L1, R5, L2, R1, R3, R2, L3, L4, L2, R2, L3, L2, L2, L3, L5, R3, R4, L5, R1, R2, L2, R4, R3, L4, L3, L1, R3, R2, R1, R1, L3, R4, L5, R2, R1, R3, L3, L2, L2, R2, R1, R2, R3, L3, L3, R4, L4, R4, R4, R4, L3, L1, L2, R5, R2, R2, R2, L4, L3, L4, R4, L5, L4, R2, L4, L4, R4, R1, R5, L2, L4, L5, L3, L2, L4, L4, R3, L3, L4, R1, L2, R3, L2, R1, R2, R5, L4, L2, L1, L3, R2, R3, L2, L1, L5, L2, L1, R4";

        [Test]
        public void Solve()
        {
            var instructions = _input.Split(',').ToList().Select(s=>s.Trim());

            var distance = Navigate(instructions);

            Console.WriteLine(distance);
            Assert.That(distance, Is.EqualTo(181));
        }

        [Test]
        public void Solve2()
        {
            var instructions = _input.Split(',').ToList().Select(s=>s.Trim());
            var history = new List<Location>();

            Navigate(instructions, history);
            var loc = GetFirstDupe(history);
            Console.WriteLine("First double: " + loc + " at distance: " + loc.Distance );
        }

        private static Location GetFirstDupe(List<Location> history)
        {
            Location firstDupe = null;
            var visited = new List<string>();
            foreach (var item in history)
            {
                if (visited.Contains(item.ToString()))
                {
                    firstDupe = item;
                    break;
                }
                visited.Add(item.ToString());
            }
            return firstDupe;
        }

        private static int Navigate(IEnumerable<string> instructions, List<Location> history = null)
        {
            history = history ?? new List<Location>();

            var baring = 0;
            var coords = new[] {0, 0};
            foreach (var instruction in instructions)
            {
                var rotation = new Dictionary<char, int> {{'L', -1}, {'R', 1}}[instruction[0]];
                baring = baring + rotation;
                baring = baring > 3 ? 0 : baring;
                baring = baring < 0 ? 3 : baring;

                var distance = int.Parse(instruction.Trim('L', 'R'));
                distance = baring > 1 ? distance*-1 : distance;
                var index = baring%2 == 0 ? 1 : 0;

                RecordHistory(history, coords, baring, distance, index);
                coords[index] += distance;
            }

            return Math.Abs(coords[0]) + Math.Abs(coords[1]);
        }

        private static void RecordHistory(ICollection<Location> history, IReadOnlyList<int> coords, int baring, int distance, int index)
        {
            var snapshot = new[] {coords[0], coords[1]};
            var distanceDelta = baring > 1 ? -1 : 1;
            for (int i = 0; i < Math.Abs(distance); i++)
            {
                snapshot[index] += distanceDelta;
                history.Add(new Location(snapshot[0], snapshot[1]));
            }
        }

        public class Location
        {
            public int X { get; set; }
            public int Y { get; set; }

            public int Distance => Math.Abs(X) + Math.Abs(Y);

            public Location(int x, int y)
            {
                X = x;
                Y = y;
            }

            public override string ToString()
            {
                return $"X:{X},Y:{Y}";
            }

            public override bool Equals(object obj)
            {
                return ToString() == obj.ToString();
            }
        }
    }
}
