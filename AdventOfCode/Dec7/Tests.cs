using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;
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
            var parts = instruction.Split(new[] {"->"}, StringSplitOptions.RemoveEmptyEntries);
            var operation = parts[0].Trim();
            var target = parts[1].Trim();

            var assignment = Regex.Match(operation, "^([0-9a-z]+)$");
            if (assignment.Success)
            {
                Wires[target] = int.Parse(assignment.Groups[1].Value);
                return;
            }

            var negation = Regex.Match(operation, "^NOT (.+)$");
            if (negation.Success)
            {
                Wires[target] = 65535 - Wires[negation.Groups[1].Value];
                return;
            }

            var bitwiseOperation = Regex.Match(operation, "^(.+) (AND|OR|LSHIFT|RSHIFT) (.+)$");
            if (bitwiseOperation.Success)
            {
                var first = ValOrInt(Wires, bitwiseOperation.Groups[1].Value);
                var gate = bitwiseOperation.Groups[2].Value;
                var second = ValOrInt(Wires, bitwiseOperation.Groups[3].Value);

                Wires[target] = Bitwise[gate](first, second);
            }
        }

        private static int ValOrInt(IReadOnlyDictionary<string, int> w, string input)
        {
            int asInt;
            if (int.TryParse(input, out asInt))
            {
                return asInt;
            }
            return w[input];
        }
        

        public void Parse(IEnumerable<string> instructions)
        {
            foreach (var instruction in instructions)
            {
                Parse(instruction);
            }
        }
    }
}