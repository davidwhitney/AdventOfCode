using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Dec3
{
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
}