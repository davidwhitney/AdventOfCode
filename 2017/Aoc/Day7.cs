using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        }

        [Test]
        public void Part1()
        {
            var lines = File.ReadAllLines(@"C:\dev\AdventOfCode\2017\Aoc\Day7-1.txt");

            var programs = ParseLines(lines);
            var root = FindRoot(programs);

            Console.WriteLine(root);
        }

        private static List<Program> ParseLines(string[] lines)
        {
            var match = new Regex(@"(?<name>\w+) \((?<weight>[0-9]+)\)( \s*(-> )?(?<supports>.*))?");

            var programs = new List<Program>();

            foreach (var line in lines)
            {
                var captures = match.Match(line);
                if (!captures.Success)
                {
                    continue;
                }

                programs.Add(new Program
                {
                    Name = captures.Groups["name"].Value,
                    Weight = int.Parse(captures.Groups["weight"].Value),
                    Supports = new List<string>(captures.Groups["supports"].Value
                        .Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries))
                });
            }
            return programs;
        }

        private static string FindRoot(List<Program> programs)
        {
            var onlyWithChains = programs.Where(p => p.Supports.Count > 0).ToList();

            var allNames = onlyWithChains.Select(x => x.Name);
            var allDependencies = onlyWithChains.SelectMany(p => p.Supports).ToList();

            foreach (var name in allNames)
            {
                if (!allDependencies.Contains(name))
                {
                    return name;
                }
            }

            throw new Exception("Nope.");
        }
    }

    public class Program
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public List<string> Supports { get; set; }
    }
}
