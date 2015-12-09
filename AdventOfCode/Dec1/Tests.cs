using System;
using System.IO;
using System.Linq;
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

        [TestCase(")", 1)]
        [TestCase("()())", 5)]
        [TestCase("()())()())", 5)]
        public void Climber_Process_GivenOneCloseBracket_CorrectlyIdentifiesBasementEntry(string path, int basementIndex)
        {
            var climber = new Climber();

            var report = climber.Climb(path);

            Assert.That(report.FirstBasementTrip, Is.EqualTo(basementIndex));
        }

        [Test]
        public void Climber_DoChallange()
        {
            var contents = File.ReadAllText("c:\\dev\\AdventOfCode\\AdventOfCode\\Dec1\\Test.txt");

            var report = new Climber().Climb(contents);

            Console.WriteLine(report.ToString());
        }

        [Test]
        public void Climber_DoChallangeGolf()
        {
            var floor = 0;
            File.ReadAllText("c:\\dev\\AdventOfCode\\AdventOfCode\\Dec1\\Test.txt")
                .ToList()
                .ForEach(c => floor += c == '(' ? 1 : -1);
            Console.WriteLine(floor);
        }
    }
}
