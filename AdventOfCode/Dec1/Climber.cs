using System.Collections.Generic;

namespace AdventOfCode.Dec1
{
    public class Climber
    {
        public ExhaustedSantaReport Climb(string floorPlan)
        {
            var floor = 0;
            int? basementAt = null;

            var valueMap = new Dictionary<char, int> {{'(', 1}, {')', -1}};

            for (var i = 0; i < floorPlan.Length; i++)
            {
                floor += valueMap[floorPlan[i]];

                if (floor < 0 && !basementAt.HasValue)
                {
                    basementAt = i + 1;
                }
            }

            return new ExhaustedSantaReport {FinalFloor = floor, FirstBasementTrip = basementAt};
        }
    }
}