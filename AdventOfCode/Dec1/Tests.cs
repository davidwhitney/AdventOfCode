using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace AdventOfCode.Dec1
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Climber_Process_GivenOneOpenBracket_CountsUp()
        {
            var climber = new Climber();

            var report = climber.Climb("(");

            Assert.That(report.FinalFloor, Is.EqualTo(1));
        }

        [Test]
        public void Climber_Process_GivenOneCloseBracket_CountsDown()
        {
            var climber = new Climber();

            var report = climber.Climb(")");

            Assert.That(report.FinalFloor, Is.EqualTo(-1));
        }

        [Test]
        public void Climber_DoChallange()
        {
            var contents = File.ReadAllText("c:\\dev\\AdventOfCode\\AdventOfCode\\Dec1\\Test.txt");

            var report = new Climber().Climb(contents);

            Console.WriteLine(report.ToString());
        }
    }

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
                if (floor < 0) basementAt = i + 1;
            }

            return new ExhaustedSantaReport(floor, basementAt);
        }
    }

    public class ExhaustedSantaReport
    {
        public int FinalFloor { get; }
        public int? FirstBasementTrip { get; }

        public ExhaustedSantaReport(int floor, int? basementVisit)
        {
            FinalFloor = floor;
            FirstBasementTrip = basementVisit;
        }

        public override string ToString()
        {
            return $"Final floor: {FinalFloor}{Environment.NewLine}" +
                   $"First basement visit at: {FirstBasementTrip}";
        }
    }
}
