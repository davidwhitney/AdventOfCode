using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Infrastructure;
using NUnit.Framework;

namespace AoC
{
    [TestFixture]
    public class Day3 : Challenge
    {
        [TestCase("5 10 25")]
        [TestCase("10 5 25")]
        [TestCase("5 25 10")]
        [TestCase("25 10 5")]
        public void Sample(string sides)
        {
            Assert.That(Triangulator.IsValid(sides), Is.False);
        }

        [Test]
        public void Sample2()
        {
            var input = 
@"101 301 501
102 302 502
103 303 503";

            var group = TextGrouper.Group(input.Split(new[] {Environment.NewLine}, StringSplitOptions.None));

            Assert.That(group[0], Is.EqualTo("101 102 103"));
            Assert.That(group[1], Is.EqualTo("301 302 303"));
            Assert.That(group[2], Is.EqualTo("501 502 503"));
        }

        [Test]
        public void Test()
        {
            var valid = Triangulator.CountValid(Input);
            Assert.That(valid, Is.EqualTo(983));
        }

        [Test]
        public void Test2()
        {
            var groups = TextGrouper.Group(Input);
            var valid = Triangulator.CountValid(groups);
            Assert.That(valid, Is.EqualTo(1836));
        }

        public class Triangulator
        {
            public static int CountValid(IEnumerable<string> inputLines) => inputLines.Count(IsValid);

            public static bool IsValid(string inputLine)
            {
                var sides = inputLine.Split(new[] {" ", "\t"}, StringSplitOptions.RemoveEmptyEntries).ToList().Select(int.Parse).ToList();
                return sides[0] + sides[1] > sides[2] && sides[0] + sides[2] > sides[1] && sides[1] + sides[2] > sides[0];
            }
        }

        public class TextGrouper
        {
            public static List<string> Group(string[] input)
            {
                var shapes = new List<string>();
                var separator = new[] { " ", "\t" };

                for (var index = 0; index < input.Length; index+=3)
                {
                    var workingSet = new List<string[]>
                    {
                        input[index].Split(separator, StringSplitOptions.RemoveEmptyEntries),
                        input[index + 1].Split(separator, StringSplitOptions.RemoveEmptyEntries),
                        input[index + 2].Split(separator, StringSplitOptions.RemoveEmptyEntries)
                    };

                    shapes.Add($"{workingSet[0][0]} {workingSet[1][0]} {workingSet[2][0]}");
                    shapes.Add($"{workingSet[0][1]} {workingSet[1][1]} {workingSet[2][1]}");
                    shapes.Add($"{workingSet[0][2]} {workingSet[1][2]} {workingSet[2][2]}");
                }

                return shapes;
            }
        }
    }
}