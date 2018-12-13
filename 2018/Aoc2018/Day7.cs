using System;
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

            var ordered = Prioritise(steps);

            Assert.That(ordered, Is.EqualTo("CABDFE"));
        }

        [Test]
        public void Example2()
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
            var steps = GetSteps(dependencies, 0);

            var ordered = Thread(steps, 2);

            Assert.That(ordered, Is.EqualTo("CABFDE"));
        }

        [Test]
        public void Test()
        {
            var instructions = File.ReadAllLines("Day7.txt");

            var dependencies = instructions.Select(Parse).ToList();
            var steps = GetSteps(dependencies);

            var ordered = Prioritise(steps);

            Assert.That(ordered, Is.EqualTo("BKCJMSDVGHQRXFYZOAULPIEWTN"));
        }

        [Test]
        public void Test2()
        {
            var instructions = File.ReadAllLines("Day7.txt");

            var dependencies = instructions.Select(Parse).ToList();
            var steps = GetSteps(dependencies);

            var ordered = Thread(steps);

            Assert.That(ordered, Is.EqualTo("BKVCMSGHJDQXZRFYOAULPIEWTN"));
        }

        private static string Prioritise(List<Step> steps)
        {
            var candidates = new List<Step>(steps);
            var scheduledOrder = new List<string>();
            
            while (candidates.Any())
            {
                var freeOfDeps = candidates.Where(x => x.Dependencies.All(dep => scheduledOrder.Contains(dep))).OrderBy(x => x.Id).ToList();

                var firstAvailable = freeOfDeps.First();

                scheduledOrder.Add(firstAvailable.Id);
                candidates.RemoveAll(x => x.Id == firstAvailable.Id);
            }

            return string.Join("", scheduledOrder);
        }

        private static string Thread(List<Step> steps, int poolsize = 5)
        {
            var candidates = new List<Step>(steps);
            var scheduledOrder = new List<string>();

            var threads = new ThreadPool(poolsize);

            while (scheduledOrder.Count < steps.Count)
            {
                var completed = threads.Step();
                scheduledOrder.AddRange(completed.Select(x=>x.Id));

                if (!threads.ThreadsAvailable)
                {
                    continue;
                }

                var freeOfDeps = candidates.Where(x => x.Dependencies.All(dep => scheduledOrder.Contains(dep))).OrderBy(x => x.Id).ToList();
                var available = freeOfDeps.Take(threads.AvailableThreadCount).ToList();

                foreach (var task in available)
                {
                    threads.Queue(task);
                }

                candidates.RemoveAll(x => available.Contains(x));
            }

            return string.Join("", scheduledOrder);
        }

        public class ThreadPool
        {
            private readonly int _size;
            private readonly Dictionary<Step, int> _active;
            public bool ThreadsAvailable => _active.Count < _size;
            public int AvailableThreadCount => _size - _active.Count;
            public int Elapsed { get; private set; } = -1;

            public ThreadPool(int size)
            {
                _size = size;
                _active = new Dictionary<Step, int>();
            }

            public void Queue(Step step)
            {
                if(_active.Count >= _size) throw new Exception("Nope");
                _active.Add(step, 0);
            }

            public List<Step> Step()
            {
                Elapsed++;
                var completed = new List<Step>();

                var keys = _active.Keys.ToList();

                foreach (var key in keys)
                {
                    _active[key]++;

                    if (key.Cost == _active[key])
                    {
                        completed.Add(key);
                    }
                }

                completed.ForEach(t => _active.Remove(t));

                return completed;
            }
        }

        private static List<Step> GetSteps(List<Dependency> dependencies, int costFactor = 60)
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

            steps.ForEach(s => s.CostFactor = costFactor);

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
        public override string ToString() => Id;
        public int Cost => CostFactor + (((int) Id[0]) - 64);
        public int CostFactor { get; set; } = 60;
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
