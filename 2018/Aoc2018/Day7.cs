using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Aoc2018
{
    [TestFixture]
    public class Day7
    {
        [Test]
        public void Example()
        {
            var instructions = new List<string>
            {
                "Step C must be finished before step A can begin.",
                "Step C must be finished before step F can begin.",
                "Step A must be finished before step B can begin.",
                "Step A must be finished before step D can begin.",
                "Step B must be finished before step E can begin.",
                "Step D must be finished before step E can begin.",
                "Step F must be finished before step E can begin."
            };

            var dependencies = instructions.Select(Parse).ToList();
            var steps = GetSteps(dependencies);
            Prioritise(steps);

            var rendered = string.Join("", steps.Select(x=>x.Id));
            Assert.That(rendered, Is.EqualTo("CABDFE"));
        }

        [Test]
        public void Test()
        {
            var instructions = File.ReadAllLines("Day7.txt");

            var dependencies = instructions.Select(Parse).ToList();
            var steps = GetSteps(dependencies);
            Prioritise(steps);

            var rendered = string.Join("", steps.Select(x=>x.Id));
            Assert.That(rendered, Is.EqualTo("CABDFE"));
        }

        private static void Prioritise(List<Step> steps)
        {
            for (var i = 0; i < steps.Count; i++)
            {
                var step = steps[i];
                var orderedDependencies = step.Dependencies.OrderBy(x => x).ToList();

                foreach (var dep in orderedDependencies)
                {
                    var stepsBeforeMe = steps.Take(i).Select(x => x.Id).ToList();
                    if (!stepsBeforeMe.Contains(dep))
                    {
                        var moving = steps.Single(x => x.Id == dep);
                        steps.Remove(moving);
                        steps.Insert(i, moving);
                        i = 0;
                    }
                }
            }
        }

        private static List<Step> GetSteps(List<Dependency> dependencies)
        {
            var allSteps = dependencies.SelectMany(d => d.Requires).ToList();
            allSteps.AddRange(dependencies.SelectMany(d => d.Target));
            allSteps = allSteps.Distinct().ToList();
            var steps = allSteps.OrderBy(x => x).Select(_ => new Step {Id = _.ToString()}).ToList();

            foreach (var dependency in dependencies)
            {
                var step = steps.First(x => x.Id == dependency.Target);
                step.Dependencies.Add(dependency.Requires);
            }

            return steps;
        }

        private Dependency Parse(string arg)
        {
            var matches = Regex.Match(arg, "Step ([A-Z]+) must be finished before step ([A-Z]+) can begin.");

            return new Dependency(matches.Groups[1].Value, matches.Groups[2].Value);
        }
    }

    public class Step
    {
        public string Id { get; set; }
        public List<string> Dependencies { get; set; } = new List<string>();
    }

    public class Dependency
    {
        public string Requires { get; }
        public string Target { get; }

        public Dependency(string requires, string target)
        {
            Requires = requires;
            Target = target;
        }
    }
}
