using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AdventOfCode.Dec3
{
    [TestFixture]
    public class Tests
    {
        private SantaGps _gps;

        [SetUp]
        public void SetUp()
        {
            _gps = new SantaGps();
        }

        [TestCase(">", 2)]
        [TestCase("^>v<", 4)]
        [TestCase("^v^v^v^v^v", 2)]
        public void DeliverTo_GivenDirections_CountsDistinctLocationsVisited(string directions, int uniques)
        {
            _gps.DeliverTo(directions);

            Assert.That(_gps.DistinctLocationsDelivered, Is.EqualTo(uniques));
        }
    }

    public class SantaGps
    {
        private readonly Location _location = new Location();
        private readonly List<Location> _visited = new List<Location>();
        public int DistinctLocationsDelivered => _visited.Distinct().Count();

        private readonly Dictionary<char, Action<Location>> _nav = new Dictionary<char, Action<Location>>
        {
            {'>', l => l.X++},
            {'<', l => l.X--},
            {'^', l => l.Y++},
            {'v', l => l.Y--}
        };

        public void DeliverTo(IEnumerable<char> directions)
        {
            _visited.Add(_location.Clone());

            foreach (var dir in directions)
            {
                _nav[dir](_location);
                _visited.Add(_location.Clone());
            }
        }
    }

    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Location))
            {
                return false;
            }

            return Equals((Location) obj);
        }

        protected bool Equals(Location other)
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

        public Location Clone()
        {
            return new Location {X = X, Y = Y};
        }
    }
}
