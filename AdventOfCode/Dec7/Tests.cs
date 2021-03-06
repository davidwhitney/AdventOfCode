﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace AdventOfCode.Dec7
{
    [TestFixture]
    public class Tests
    {
        private Circuit _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new Circuit();
        }

        [Test]
        public void SignalProvidedInText_SetsWire_SetsWireValue()
        {
            _sut.Parse("123 -> x");

            Assert.That(_sut.Wires["x"], Is.EqualTo(123));
        }

        [TestCase(1, 1, 1)]
        [TestCase(0, 1, 0)]
        public void AndProvidedInText_PerformsBitwiseAnd_SetsThirdValue(int bit1, int bit2, int result)
        {
            _sut = new Circuit(new Dictionary<string, int>
            {
                {"a", bit1},
                {"y", bit2}
            });

            _sut.Parse("a AND y -> z");

            Assert.That(_sut.Wires["z"], Is.EqualTo(result));
        }

        [TestCase(0, 0, 0)]
        [TestCase(0, 1, 1)]
        public void OrProvidedInText_PerformsBitwiseOr_SetsThirdValue(int bit1, int bit2, int result)
        {
            _sut = new Circuit(new Dictionary<string, int>
            {
                {"a", bit1},
                {"y", bit2}
            });

            _sut.Parse("a OR y -> z");

            Assert.That(_sut.Wires["z"], Is.EqualTo(result));
        }

        [TestCase(23, 1, 11)]
        public void RshiftProvidedInText_PerformsRightshift_SetsThirdValue(int bit1, int shiftBy, int result)
        {
            _sut = new Circuit(new Dictionary<string, int>
            {
                {"a", bit1}
            });

            _sut.Parse($"a RSHIFT {shiftBy} -> z");

            Assert.That(_sut.Wires["z"], Is.EqualTo(result));
        }

        [TestCase(14, 2, 56)]
        public void LshiftProvidedInText_PerformsLeftshift_SetsThirdValue(int bit1, int shiftBy, int result)
        {
            _sut = new Circuit(new Dictionary<string, int>
            {
                {"a", bit1}
            });

            _sut.Parse($"a LSHIFT {shiftBy} -> z");

            Assert.That(_sut.Wires["z"], Is.EqualTo(result));
        }

        [Test]
        public void Not_PerformsInversion_SetsThirdValue()
        {
            _sut = new Circuit(new Dictionary<string, int>
            {
                {"a", 7}
            });

            _sut.Parse("NOT a -> z");
            
            Assert.That(_sut.Wires["z"], Is.EqualTo(65528));
        }

        [Test]
        public void Example1()
        {
            var instructions = new List<string>
            {
                "123 -> x",
                "456 -> y",
                "x AND y -> d",
                "x OR y -> e",
                "x LSHIFT 2 -> f",
                "y RSHIFT 2 -> g",
                "NOT x -> h",
                "NOT y -> i"
            };

            _sut.Parse(instructions);

            Assert.That(_sut.Wires["d"], Is.EqualTo(72));
            Assert.That(_sut.Wires["e"], Is.EqualTo(507));
            Assert.That(_sut.Wires["f"], Is.EqualTo(492));
            Assert.That(_sut.Wires["g"], Is.EqualTo(114));
            Assert.That(_sut.Wires["h"], Is.EqualTo(65412));
            Assert.That(_sut.Wires["i"], Is.EqualTo(65079));
            Assert.That(_sut.Wires["x"], Is.EqualTo(123));
            Assert.That(_sut.Wires["y"], Is.EqualTo(456));
        }

        [Test]
        [Ignore("Slow")]
        public void DoChallange()
        {
            var contents = File.ReadLines("c:\\dev\\AdventOfCode\\AdventOfCode\\Dec7\\Tests.txt").ToList();

            _sut.Parse(contents);

            Assert.That(_sut.Wires["a"], Is.EqualTo(16076));
        }

        [Test]
        [Ignore("Slow")]
        public void DoChallange2()
        {
            var contents = File.ReadLines("c:\\dev\\AdventOfCode\\AdventOfCode\\Dec7\\Tests2.txt").ToList();

            _sut.Parse(contents);

            Assert.That(_sut.Wires["a"], Is.EqualTo(2797));
        }
    }

    public class Circuit
    {
        public Dictionary<string, int> Wires { get; }

        private static readonly Dictionary<string, Func<int, int, int>> Bitwise = new Dictionary
            <string, Func<int, int, int>>
        {
            {"AND", (x, y) => x & y},
            {"OR", (x, y) => x | y},
            {"LSHIFT", (x, y) => x << y},
            {"RSHIFT", (x, y) => x >> y},
        };

        public Circuit(Dictionary<string, int> wires = null)
        {
            Wires = wires ?? new Dictionary<string, int>();
        }

        public void Parse(string instruction)
        {
            Parse(new[] {instruction});
        }

        public void Parse(IEnumerable<string> instructions)
        {
            var instructionQueue = new Queue<string>(instructions);

            do
            {
                var ins = instructionQueue.Dequeue();

                var parts = ins.Split(new[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
                var operation = parts[0].Trim();
                var target = parts[1].Trim();

                if (!Assigment(operation, instructionQueue, ins, target))
                {
                    continue;
                }

                if (!Negation(operation, instructionQueue, ins, target))
                {
                    continue;
                }

                BitwiseOp(operation, instructionQueue, ins, target);

            } while (instructionQueue.Count > 0);

        }

        private void BitwiseOp(string operation, Queue<string> instructionQueue, string ins, string target)
        {
            var bitwiseOperation = Regex.Match(operation, "^(.+) (AND|OR|LSHIFT|RSHIFT) (.+)$");
            if (bitwiseOperation.Success)
            {
                var source1 = RegisteredValueOrInteger(bitwiseOperation.Groups[1].Value);
                var gate = bitwiseOperation.Groups[2].Value;
                var source2 = RegisteredValueOrInteger(bitwiseOperation.Groups[3].Value);

                if (source1 == null || source2 == null)
                {
                    // Dependencies not satisfied requeue
                    instructionQueue.Enqueue(ins);
                    return;
                }

                Wires[target] = Bitwise[gate](source1.Value, source2.Value);
            }
        }

        private bool Negation(string operation, Queue<string> instructionQueue, string ins, string target)
        {
            var negation = Regex.Match(operation, "^NOT (.+)$");
            if (negation.Success)
            {
                var source = RegisteredValueOrInteger(negation.Groups[1].Value);
                if (source == null)
                {
                    // Dependencies not satisfied requeue
                    instructionQueue.Enqueue(ins);
                    return false;
                }

                Wires[target] = 65535 - source.Value;
                return false;
            }
            return true;
        }

        private bool Assigment(string operation, Queue<string> instructionQueue, string ins, string target)
        {
            var assignment = Regex.Match(operation, "^([0-9a-z]+)$");
            if (assignment.Success)
            {
                var source = RegisteredValueOrInteger(assignment.Groups[1].Value);
                if (source == null)
                {
                    // Dependencies not satisfied requeue
                    instructionQueue.Enqueue(ins);
                    return false;
                }

                Wires[target] = source.Value;
                return false;
            }
            return true;
        }

        private int? RegisteredValueOrInteger(string input)
        {
            int asInt;
            if (int.TryParse(input, out asInt))
            {
                return asInt;
            }

            if (Wires.ContainsKey(input))
            {
                return Wires[input];
            }

            return null;
        }
    }
}