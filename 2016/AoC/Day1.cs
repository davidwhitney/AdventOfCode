using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AoC
{
    [TestFixture]
    public class Day1
    {
        private string _input = "L2, L5, L5, R5, L2, L4, R1, R1, L4, R2, R1, L1, L4, R1, L4, L4, R5, R3, R1, L1, R1, L5, L1, R5, L4, R2, L5, L3, L3, R3, L3, R4, R4, L2, L5, R1, R2, L2, L1, R3, R4, L193, R3, L5, R45, L1, R4, R79, L5, L5, R5, R1, L4, R3, R3, L4, R185, L5, L3, L1, R5, L2, R1, R3, R2, L3, L4, L2, R2, L3, L2, L2, L3, L5, R3, R4, L5, R1, R2, L2, R4, R3, L4, L3, L1, R3, R2, R1, R1, L3, R4, L5, R2, R1, R3, L3, L2, L2, R2, R1, R2, R3, L3, L3, R4, L4, R4, R4, R4, L3, L1, L2, R5, R2, R2, R2, L4, L3, L4, R4, L5, L4, R2, L4, L4, R4, R1, R5, L2, L4, L5, L3, L2, L4, L4, R3, L3, L4, R1, L2, R3, L2, R1, R2, R5, L4, L2, L1, L3, R2, R3, L2, L1, L5, L2, L1, R4";
        private IEnumerable<string> Instructions => _input.Split(',').ToList().Select(s => s.Trim());

        private Pathfinder _pathfinder;

        [SetUp]
        public void SetUp()
        {
            _pathfinder = new Pathfinder();
        }

        [Test]
        public void Solve()
        {
            var distance = _pathfinder.Navigate(Instructions).Distance;
            
            Assert.That(distance, Is.EqualTo(181));
        }

        [Test]
        public void Solve2()
        {
            _pathfinder = new Pathfinder(true);

            var loc = _pathfinder.Navigate(Instructions);

            Assert.That(loc.ToString(), Is.EqualTo("X:28,Y:112"));
            Assert.That(loc.Distance, Is.EqualTo(140));
        }
    }

    public class Pathfinder
    {
        private readonly bool _returnOnRevisit;
        public List<Loc> History { get; } = new List<Loc>();

        public Pathfinder(bool returnOnRevisit = false)
        {
            _returnOnRevisit = returnOnRevisit;
        }

        public Loc Navigate(IEnumerable<string> instructions)
        {
            var baring = 0;
            var coords = new Loc(0, 0);
            foreach (var instruction in instructions)
            {
                var rotation = new Dictionary<char, int> { { 'L', -1 }, { 'R', 1 } }[instruction[0]];
                baring = baring + rotation;
                baring = baring > 3 ? 0 : baring;
                baring = baring < 0 ? 3 : baring;

                var distance = int.Parse(instruction.Trim('L', 'R'));
                var directionalDistance = baring > 1 ? distance * -1 : distance;
                var xOrY = baring % 2 == 0 ? 1 : 0;

                var detectedRevisit = RecordHistory(coords, baring, directionalDistance, xOrY);
                coords[xOrY] += directionalDistance;

                if (_returnOnRevisit && detectedRevisit != null)
                {
                    return detectedRevisit;
                }
            }

            return Loc.Copy(coords);
        }

        private Loc RecordHistory(IList<int> coords, int baring, int distance, int xOrY)
        {
            var snapshot = Loc.Copy(coords);
            var distanceDelta = baring > 1 ? -1 : 1;
            for (var i = 0; i < Math.Abs(distance); i++)
            {
                snapshot[xOrY] += distanceDelta;
                var historyEntry = Loc.Copy(snapshot);
                if (_returnOnRevisit && History.Any(x => x.ToString() == historyEntry.ToString()))
                {
                    return historyEntry;
                }

                History.Add(historyEntry);
            }

            return null;
        }
    }

    public class Loc : List<int>
    {
        public int X { get { return this[0]; } set { this[0] = value; } }
        public int Y { get { return this[1]; } set { this[1] = value; } }
        public int Distance => Math.Abs(X) + Math.Abs(Y);
        public override string ToString() => $"X:{X},Y:{Y}";
        public static Loc Copy(IList<int> x) => new Loc(x[0], x[1]);
        public Loc(int x, int y) { AddRange(new[] { x, y }); }
    }
}
