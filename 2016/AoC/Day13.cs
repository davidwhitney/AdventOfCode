﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

            List<Area.Coord> pathTaken;
            var cost = _area.ShortestPathTo(7, 4, out pathTaken);

            var travelled = _area.ToString(pathTaken);
            Console.WriteLine(travelled);

            Assert.That(cost, Is.EqualTo(11));
        }

        [Test]
        public void Test()
        {
            _area = new Area(1364);
            _area.GenerateMap(45, 45);

            List<Area.Coord> pathTaken;
            var cost = _area.ShortestPathTo(31, 39, out pathTaken);
            var travelled = _area.ToString(pathTaken, ' ', '.', '█');
            Console.WriteLine(travelled);

            Assert.That(cost, Is.EqualTo(86));
        }

        [Test]
        public void Test2()
        {
            _area = new Area(1364);
            _area.GenerateMap(45, 45);

            List<Area.Coord> pathTaken;
            _area.ScanPaths(50);
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

        public override string ToString() => ToString(new List<Coord>());
        public string ToString(List<Coord> winningPath, char empty = '.', char path = 'x', char wall = '#')
        {
            winningPath = winningPath ?? new List<Coord>();

            var buffer = new StringBuilder();
            for (var y = 0; y < _storage.Count; y++)
            {
                for(var x = 0; x < _storage[y].Count; x++)
                {
                    var draw = _storage[y][x];
                    draw = draw == '.' ? empty : draw;
                    draw = draw == '#' ? wall : draw;
                    draw = winningPath.Contains(new Coord(x, y)) ? path : draw;

                    buffer.Append(draw);
                }
                buffer.AppendLine();
            }
            return buffer.ToString().Trim();
        }

        public int ShortestPathTo(int targetX, int targetY, out List<Coord> pathTaken, int giveUpAt = int.MaxValue)
        {
            var location = new Coord(1, 1);
            var costSoFar = 0;
            var costOfMovement = 1;

            var target = new Coord(targetX, targetY);
            var visited = new List<Coord>();
            var badPaths = new List<Coord>();
            var junctions = new Stack<Coord>();
            var locationsVisited = 1;

            pathTaken = visited;

            while (!Equals(location, target))
            {
                var choices = new[]
                {
                    location.Clone(c => c.Y++),
                    location.Clone(c => c.Y--),
                    location.Clone(c => c.X++),
                    location.Clone(c => c.X--),
                };

                var paths = choices.Where(c => DetectTerrain(c.X, c.Y) == '.').ToList();
                paths.RemoveAll(path => visited.Contains(path));
                paths.RemoveAll(path => badPaths.Contains(path));

                var costToPaths = new Dictionary<Coord, int>();
                foreach (var availablePath in paths)
                {
                    var costToTarget = (Math.Abs(targetX - availablePath.X)) + (Math.Abs(targetY - availablePath.Y));
                    var tileCost = costToTarget + costSoFar + costOfMovement;
                    costToPaths.Add(availablePath, tileCost);
                }

                if (costToPaths.Count > 1)
                {
                    junctions.Push(location.Clone());
                }
                
                var selectedPath = costToPaths.OrderBy(x => x.Value).Select(c => c.Key).FirstOrDefault();
                if (selectedPath == null)
                {
                    // Dead end - let's go back to last junction.
                    var lastJunction = junctions.Pop();
                    var backtrackDistance = BackUpTo(location, lastJunction, visited, badPaths);
                    costSoFar -= backtrackDistance;
                    continue;
                }
                
                visited.Add(selectedPath);
                location.MoveTo(selectedPath);
                locationsVisited++;
                costSoFar++;
            }

            return costSoFar;
        }

        public int ScanPaths(int moves)
        {
            var travelableLocations = new List<Coord>();
            var location = new Coord(1, 1);
            var choices = new[]
            {
                location.Clone(c => c.Y++),
                location.Clone(c => c.Y--),
                location.Clone(c => c.X++),
                location.Clone(c => c.X--),
            };

            return 0;
        }

        private char DetectTerrain(int x, int y) => Convert.ToString(x * x + 3 * x + 2 * x * y + y + y * y + Seed, 2).Count(_ => _ == '1') % 2 == 0 ? '.' : '#';

        private static int BackUpTo(Coord currentLocation, Coord targetLocation, List<Coord> visited, List<Coord> badPaths)
        {
            var reverseDistance = visited.LastIndexOf(targetLocation);
            var deadPath = visited.GetRange(reverseDistance + 1, visited.Count - reverseDistance - 1);

            visited.RemoveRange(reverseDistance + 1, deadPath.Count);
            badPaths.AddRange(deadPath);
            currentLocation.MoveTo(targetLocation);
            return deadPath.Count;
        }

        public class Coord
        {
            public int X { get; set; }
            public int Y { get; set; }
            public override bool Equals(object obj) => Equals((Coord)obj);
            protected bool Equals(Coord other) => X == other.X && Y == other.Y;

            public Coord(int x, int y)
            {
                X = x;
                Y = y;
            }
            
            public override int GetHashCode()
            {
                unchecked
                {
                    return (X*397) ^ Y;
                }
            }

            public Coord Clone(Action<Coord> delta = null)
            {
                var c = new Coord(X, Y);
                (delta ?? (_ => { }))(c);
                return c;
            }

            public void MoveTo(Coord c)
            {
                X = c.X;
                Y = c.Y;
            }
        }
    }
}
