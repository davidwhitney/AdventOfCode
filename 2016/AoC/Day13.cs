using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AoC
{
    [TestFixture]
    public class Day13
    {
        private Area _area;

        [SetUp]
        public void SetUp()
        {
            _area = new Area(10);
        }

        [Test]
        public void Sample()
        {
            _area.GenerateMap(10, 7);

            Assert.That(_area.ToString(), Is.EqualTo(@"
.#.####.##
..#..#...#
#....##...
###.#.###.
.##..#..#.
..##....#.
#...##.###".TrimStart()));

            var cost = _area.ShortestPathTo(7, 4);

            Assert.That(cost, Is.EqualTo(11));
        }

        [Test]
        public void Test()
        {
            _area = new Area(1364);
            var cost = _area.ShortestPathTo(31, 39);

            Assert.That(cost, Is.EqualTo(11));
        }
    }

    public class Area
    {
        public int Seed { get; set; }
        private List<List<char>> _storage;

        public Area(int seed)
        {
            Seed = seed;
        }

        public void GenerateMap(int maxX, int maxY)
        {
            _storage = new List<List<char>>();
            for (var i = 0; i < maxY; i++)
            {
                _storage.Add(Enumerable.Repeat('.', maxX).ToList());
            }

            for (var y = 0; y < maxY; y++)
            for (var x = 0; x < maxX; x++)
            {
                _storage[y][x] = DetectTerrain(x, y);
            }
        }

        private char DetectTerrain(int x, int y)
        {
            var number = x*x + 3*x + 2*x*y + y + y*y + Seed;
            var thatAreOne = Convert.ToString(number, 2).Count(_ => _ == '1');
            return thatAreOne%2 == 0 ? '.' : '#';
        }

        public override string ToString()
        {
            return ToString(".");
        }

        public string ToString(string empty)
        {
            var buffer = new StringBuilder();
            foreach (var row in _storage)
            {
                buffer.AppendLine(string.Join("", row).Replace(".", empty));
            }
            return buffer.ToString().Trim();
        }

        public int ShortestPathTo(int targetX, int targetY)
        {
            var location = new Coord(1, 1);
            var costSoFar = 0;
            var costOfMovement = 1;

            var visited = new List<Coord>();

            while (location.X != targetX || location.Y != targetY)
            {
                var choices = new[]
                {
                    new Coord(location.X, location.Y + 1),
                    new Coord(location.X, location.Y - 1),
                    new Coord(location.X + 1, location.Y),
                    new Coord(location.X - 1, location.Y),
                };

                var paths = choices.Where(c => DetectTerrain(c.X, c.Y) == '.').ToList();
                paths.RemoveAll(path => visited.Contains(path));

                var costToPaths = new Dictionary<Coord, int>();
                foreach (var availablePath in paths)
                {
                    var costToTarget = (Math.Abs(targetX - availablePath.X)) + (Math.Abs(targetY - availablePath.Y));
                    var tileCost = costToTarget + costSoFar + costOfMovement;
                    costToPaths.Add(availablePath, tileCost);
                }

                var selectedPath = costToPaths.OrderBy(x=>x.Value).Select(c=>c.Key).First();
                visited.Add(selectedPath);
                location.X = selectedPath.X;
                location.Y = selectedPath.Y;
                costSoFar++;
            }

            return costSoFar;
        }

        public class Coord
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Coord(int x, int y)
            {
                X = x;
                Y = y;
            }

            public override bool Equals(object obj)
            {
                return Equals((Coord)obj);
            }

            protected bool Equals(Coord other)
            {
                return X == other.X && Y == other.Y;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (X*397) ^ Y;
                }
            }
        }
    }
}
