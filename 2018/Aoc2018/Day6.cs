using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Aoc2018
{
    [TestFixture]
    public class Day6
    {
        [Test]
        public void Example()
        {
            var coords = new List<Map.Cordinate>
            {
                new Map.Cordinate(1, 1),
                new Map.Cordinate(1, 6),
                new Map.Cordinate(8, 3),
                new Map.Cordinate(3, 4),
                new Map.Cordinate(5, 5),
                new Map.Cordinate(8, 9)
            };

            var map = new Map(coords);
            Console.WriteLine(map.ToString());

            Assert.That(map.Bounds.Count, Is.EqualTo(4));
            Assert.That(map.BoundedOwnership.Count, Is.EqualTo(2));
            Assert.That(map.LargestArea, Is.EqualTo(17));
            Assert.That(map.CountSafeZone(32), Is.EqualTo(16));
        }

        [Test]
        public void Test()
        {
            var coords = TestInput();

            var map = new Map(coords);

            Assert.That(map.Bounds.Count, Is.EqualTo(4));
            Assert.That(map.BoundedOwnership.Count, Is.EqualTo(46));
            Assert.That(map.LargestArea, Is.EqualTo(5365));
            Assert.That(map.CountSafeZone(10000), Is.EqualTo(42513));
        }

        private static List<Map.Cordinate> TestInput()
        {
            var coords = new List<Map.Cordinate>
            {
                new Map.Cordinate(341, 330),
                new Map.Cordinate(85, 214),
                new Map.Cordinate(162, 234),
                new Map.Cordinate(218, 246),
                new Map.Cordinate(130, 67),
                new Map.Cordinate(340, 41),
                new Map.Cordinate(206, 342),
                new Map.Cordinate(232, 295),
                new Map.Cordinate(45, 118),
                new Map.Cordinate(93, 132),
                new Map.Cordinate(258, 355),
                new Map.Cordinate(187, 302),
                new Map.Cordinate(181, 261),
                new Map.Cordinate(324, 246),
                new Map.Cordinate(150, 203),
                new Map.Cordinate(121, 351),
                new Map.Cordinate(336, 195),
                new Map.Cordinate(44, 265),
                new Map.Cordinate(51, 160),
                new Map.Cordinate(63, 133),
                new Map.Cordinate(58, 117),
                new Map.Cordinate(109, 276),
                new Map.Cordinate(292, 241),
                new Map.Cordinate(81, 56),
                new Map.Cordinate(281, 284),
                new Map.Cordinate(226, 104),
                new Map.Cordinate(98, 121),
                new Map.Cordinate(178, 234),
                new Map.Cordinate(319, 332),
                new Map.Cordinate(279, 234),
                new Map.Cordinate(143, 163),
                new Map.Cordinate(109, 333),
                new Map.Cordinate(80, 188),
                new Map.Cordinate(106, 242),
                new Map.Cordinate(65, 59),
                new Map.Cordinate(253, 137),
                new Map.Cordinate(287, 317),
                new Map.Cordinate(185, 50),
                new Map.Cordinate(193, 132),
                new Map.Cordinate(96, 319),
                new Map.Cordinate(193, 169),
                new Map.Cordinate(100, 155),
                new Map.Cordinate(113, 161),
                new Map.Cordinate(182, 82),
                new Map.Cordinate(157, 148),
                new Map.Cordinate(132, 67),
                new Map.Cordinate(339, 296),
                new Map.Cordinate(243, 208),
                new Map.Cordinate(196, 234),
                new Map.Cordinate(87, 335)
            };
            return coords;
        }
    }

    public class Map
    {
        public int MinX { get; set; }
        public int MaxY { get; set; }
        public int MinY { get; set; }
        public int MaxX { get; set; }

        public List<Cordinate> Bounds { get; set; }
        public IReadOnlyCollection<Cordinate> Cordinates { get; set; }
        public Dictionary<Cordinate, int> OwnershipCounts { get; set; } = new Dictionary<Cordinate, int>();
        public Dictionary<Cordinate, Cordinate> ClosestToMap { get; set; } = new Dictionary<Cordinate, Cordinate>();

        public int LargestArea => BoundedOwnership.Max(x => x.Value) + 1;
        public Dictionary<Cordinate, int> BoundedOwnership
        {
            get
            {
                var set = new Dictionary<Cordinate, int>(OwnershipCounts);
                foreach (var bound in Bounds)
                {
                    set.Remove(bound);
                }

                return set;
            }
        }


        public Map(IReadOnlyCollection<Cordinate> coords)
        {
            Cordinates = coords;
            LabelCordinates();
            FindBounds(coords);
            FindOwners();
        }

        public int CountSafeZone(int size)
        {
            var count = 0;
            for (var x = MinX; x <= MaxX; x++)
            for (var y = MinY; y <= MaxY; y++)
            {
                var currentLocation = new Cordinate(x, y);

                var distances = Cordinates.ToDictionary(c => c, c => c.DistanceFrom(currentLocation));
                var sum = distances.Sum(_ => _.Value);
                if (sum < size) count++;
            }
            return count;
        }

        private void FindBounds(IReadOnlyCollection<Cordinate> coords)
        {
            MinY = coords.Min(_ => _.Y);
            MaxY = coords.Max(_ => _.Y);
            MinX = coords.Min(_ => _.X);
            MaxX = coords.Max(_ => _.X);

            Bounds = new List<Cordinate>();
            Bounds.AddRange(coords.Where(c => c.Y == MinY));
            Bounds.AddRange(coords.Where(c => c.Y == MaxY));
            Bounds.AddRange(coords.Where(c => c.X == MinX));
            Bounds.AddRange(coords.Where(c => c.X == MaxX));
            Bounds = Bounds.Distinct().ToList();
        }

        private void LabelCordinates()
        {
            var charCode = (int) 'A';
            foreach (var coord in Cordinates)
            {
                coord.Label = ((char) charCode).ToString();
                charCode++;
            }
        }

        private void FindOwners()
        {
            Cordinates.ToList().ForEach(c => OwnershipCounts.Add(c, 0));

            for (var x = MinX; x <= MaxX; x++)
            for (var y = MinY; y <= MaxY; y++)
            {
                var currentLocation = new Cordinate(x, y);
                if (Cordinates.Contains(currentLocation))
                {
                    continue;
                }

                var closest = FindOwner(currentLocation);
                if (closest == null)
                {
                    continue;
                }

                OwnershipCounts[closest]++;
                ClosestToMap[currentLocation] = closest;
            }

            foreach (var bound in Bounds)
            {
                OwnershipCounts.Remove(bound);
            }
        }

        private Cordinate FindOwner(Cordinate currentLocation)
        {
            var distances = Cordinates.ToDictionary(c => c, c => c.DistanceFrom(currentLocation));
            var rankedByDistance = distances
                .GroupBy(_ => _.Value)
                .OrderBy(_ => _.Key)
                .ToList();

            var closestDistance = rankedByDistance.First().ToList();
            return closestDistance.Count >= 2 ? null : closestDistance.First().Key;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var y = MinY; y <= MaxY; y++)
            {
                for (var x = MinX; x <= MaxX; x++)
                {
                    var location = new Cordinate(x, y);
                    var label = ".";
                    if (ClosestToMap.ContainsKey(location))
                    {
                        var owner = ClosestToMap[location];
                        label = owner.Label.ToLower();
                    }

                    if (Cordinates.Contains(location))
                    {
                        label = Cordinates.Single(_=>_.Equals(location)).Label;
                    }

                    sb.Append(label);
                }

                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public class Cordinate
        {
            public int X { get; }
            public int Y { get; }
            public string Label { get; set; }

            public Cordinate(int x, int y)
            {
                X = x;
                Y = y;
            }

            public override bool Equals(object obj) => obj is Cordinate coordinate && Equals(coordinate);
            protected bool Equals(Cordinate other) => X == other.X && Y == other.Y;

            public override int GetHashCode()
            {
                unchecked
                {
                    return (X * 397) ^ Y;
                }
            }



            public int DistanceFrom(Cordinate other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
            public override string ToString() => $"{X},{Y}";
        }
    }
}