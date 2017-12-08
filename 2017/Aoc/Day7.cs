using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Aoc
{
    [TestFixture]
    public class Day7
    {
        [Test]
        public void Example()
        {
            var list = 
@"pbga (66)
xhth (57)
ebii (61)
havc (66)
ktlj (57)
fwft (72) -> ktlj, cntj, xhth
qoyq (66)
padx (45) -> pbga, havc, qoyq
tknk (41) -> ugml, padx, fwft
jptl (61)
ugml (68) -> gyxo, ebii, jptl
gyxo (61)
cntj (57)";

            var lines = list.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

            var programs = ParseLines(lines);
            var root = FindRoot(programs);

            var outlier = FindUnbalanced(root);
        }

        [Test]
        public void Part1()
        {
            var lines = File.ReadAllLines(@"C:\dev\AdventOfCode\2017\Aoc\Day7-1.txt");

            var programs = ParseLines(lines);
            var root = FindRoot(programs);

            Console.WriteLine(root.Name);

            var outlier = FindUnbalanced(root);
        }

        private static Program FindUnbalanced(Program root)
        {
            var outlierWeight = root.WeightGroups.SingleOrDefault(x => x.Count() == 1);
            return outlierWeight == null ? root : FindUnbalanced(outlierWeight.Single());
        }

        private static List<Program> ParseLines(string[] lines)
        {
            var match = new Regex(@"(?<name>\w+) \((?<weight>[0-9]+)\)( \s*(-> )?(?<supports>.*))?");

            var dictionary = new Dictionary<string, Tuple<Program, List<string>>>();
            var programs = new List<Program>();

            foreach (var captures in lines.Select(l => match.Match(l)))
            {
                var program = new Program
                {
                    Name = captures.Groups["name"].Value,
                    Weight = int.Parse(captures.Groups["weight"].Value)
                };

                var references = captures.Groups["supports"].Value.Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries).ToList();

                dictionary.Add(program.Name, new Tuple<Program, List<string>>(program, references));
                programs.Add(program);
            }

            foreach (var program in dictionary)
            {
                foreach (var dep in program.Value.Item2)
                {
                    program.Value.Item1.References.Add(dictionary[dep].Item1);
                }
            }

            return programs;
        }

        private static Program FindRoot(List<Program> programs)
        {
            var onlyWithChains = programs.Where(p => p.References.Count > 0).ToList();

            var allNames = onlyWithChains.Select(x => x.Name);
            var allDependencies = onlyWithChains.SelectMany(p => p.References).Select(x=>x.Name).ToList();

            foreach (var name in allNames)
            {
                if (!allDependencies.Contains(name))
                {
                    return programs.Single(x=>x.Name == name);
                }
            }

            throw new Exception("Nope.");
        }
    }

    public class Program
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public List<Program> References { get; set; } = new List<Program>();

        public int TotalWeight => Weight + References.Sum(i => i.TotalWeight);
        public IEnumerable<IGrouping<int, Program>> WeightGroups => References.GroupBy(x => x.TotalWeight).OrderBy(x=>x.Key);
        public bool ChildrenBalance => WeightGroups.Count() == 1;
    }
}
