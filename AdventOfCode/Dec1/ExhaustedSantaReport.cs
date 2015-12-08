using System;

namespace AdventOfCode.Dec1
{
    public class ExhaustedSantaReport
    {
        public int FinalFloor { get; set; }
        public int? FirstBasementTrip { get; set; }
        
        public override string ToString()
        {
            return $"Final floor: {FinalFloor}{Environment.NewLine}" +
                   $"First basement visit at: {FirstBasementTrip}";
        }
    }
}