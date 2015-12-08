using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace AdventOfCode.Dec1
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Climber_Process_GivenOneOpenBracket_CountsUp()
        {
            var climber = new Climber();

            var floor = climber.Climb("(");

            Assert.That(floor, Is.EqualTo(1));
        }

        [Test]
        public void Climber_Process_GivenOneCloseBracket_CountsDown()
        {
            var climber = new Climber();

            var floor = climber.Climb(")");

            Assert.That(floor, Is.EqualTo(-1));
        }

        [Test]
        public void Climber_DoChallange()
        {
            var contents = File.ReadAllText("c:\\dev\\AdventOfCode\\AdventOfCode\\Dec1\\Test.txt");

            var floor = new Climber().Climb(contents);

            Console.WriteLine(floor);
        }
    }

    public class Climber
    {
        public int Climb(string plan)
        {
            return plan.Count(x => x == '(') - plan.Count(x => x == ')');
        }
    }
}
