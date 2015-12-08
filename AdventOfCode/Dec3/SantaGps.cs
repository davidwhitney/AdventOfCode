using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Dec3
{
    public class SantaGps
    {
        private readonly Queue<Location> _deliveryAgentLocations = new Queue<Location>();
        private readonly List<Location> _visited = new List<Location>();
        public int DistinctLocationsDelivered => _visited.Distinct().Count();

        private readonly Dictionary<char, Action<Location>> _nav = new Dictionary<char, Action<Location>>
        {
            {'>', l => l.X++},
            {'<', l => l.X--},
            {'^', l => l.Y++},
            {'v', l => l.Y--}
        };

        public SantaGps()
        {
            _deliveryAgentLocations.Enqueue(new Location());
        }

        public void AddDeliveryBot()
        {
            _deliveryAgentLocations.Enqueue(new Location());
        }

        public void DeliverTo(IEnumerable<char> directions)
        {
            foreach (var robot in _deliveryAgentLocations)
            {
                _visited.Add(robot.Clone());
            }

            foreach (var dir in directions)
            {
                var robot = _deliveryAgentLocations.Dequeue();

                _nav[dir](robot);
                _visited.Add(robot.Clone());

                _deliveryAgentLocations.Enqueue(robot);
            }
        }
    }
}