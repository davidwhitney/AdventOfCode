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
    public class Day8
    {
        [Test]
        public void Sample()
        {
            var stack = new List<string>
            {
                "b inc 5 if a > 1",
                "a inc 1 if b < 5",
                "c dec -10 if a >= 1",
                "c inc -20 if c == 10"
            };

            var state = Process(stack);

            Console.WriteLine(state.Max(x=>x.Value));
        }

        [Test]
        public void Part1()
        {
            var lines = File.ReadAllLines(@"C:\dev\AdventOfCode\2017\Aoc\Day8.txt");

            var state = Process(lines);

            Console.WriteLine(state.Max(x => x.Value));
        }

        private static Dictionary<string, int> Process(IEnumerable<string> stack)
        {
            var instructions = ParseStack(stack);
            var registers = new Dictionary<string, int>();
            instructions.Select(x => x.Target).Distinct().ToList().ForEach(target => registers.Add(target, 0));

            foreach (var instruction in instructions)
            {
                if (!instruction.Operator.Operator(registers[instruction.ConditionLeft], instruction.ConditionValue)) continue;

                if (instruction.Command == "inc")
                    registers[instruction.Target] += instruction.Value;
                else
                    registers[instruction.Target] -= instruction.Value;
            }

            return registers;
        }

        private static List<Instruction> ParseStack(IEnumerable<string> stack)
        {
            var match = new Regex(
                @"(?<target>\w+) (?<command>inc|dec) (?<Value>-?[0-9]+) if (?<ConditionLeft>\w+) (?<operator>>|<|==|<=|>=|!=) (?<predicateValue>-?[0-9]+)");

            return stack.Select(line => match.Match(line))
                .Where(captures => captures.Success)
                .Select(captures => new Instruction
                {
                    Target = captures.Groups["target"].Value,
                    Command = captures.Groups["command"].Value,
                    Value = int.Parse(captures.Groups["Value"].Value),
                    ConditionLeft = captures.Groups["ConditionLeft"].Value,
                    Operator = captures.Groups["operator"].Value,
                    ConditionValue = int.Parse(captures.Groups["predicateValue"].Value),
                }).ToList();
        }
    }

    public class Instruction
    {
        public string Target { get; set; }
        public string Command { get; set; }
        public int Value { get; set; }

        public string ConditionLeft { get; set; }
        public string Operator { get; set; }
        public int ConditionValue { get; set; }
    }

    public static class Extension
    {
        public static bool Operator(this string logic, int x, int y)
        {
            switch (logic)
            {
                case ">": return x > y;
                case "<": return x < y;
                case "==": return x == y;
                case "!=": return x != y;
                case "<=": return x <= y;
                case ">=": return x >= y;
                default: throw new Exception("invalid logic");
            }
        }
    }
}
