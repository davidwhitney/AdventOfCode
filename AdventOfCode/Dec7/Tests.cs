using System;
using System.Collections.Generic;
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

            Assert.That(_sut.Wires['x'], Is.EqualTo(123));
        }

        [TestCase(1, 1, 1)]
        [TestCase(0, 1, 0)]
        public void AndProvidedInText_PerformsBitwiseAnd_SetsThirdValue(int bit1, int bit2, int result)
        {
            _sut = new Circuit(new Dictionary<char, int>
            {
                {'a', bit1},
                {'y', bit2}
            });

            _sut.Parse("a AND y -> z");

            Assert.That(_sut.Wires['z'], Is.EqualTo(result));
        }

        [TestCase(0, 0, 0)]
        [TestCase(0, 1, 1)]
        public void OrProvidedInText_PerformsBitwiseOr_SetsThirdValue(int bit1, int bit2, int result)
        {
            _sut = new Circuit(new Dictionary<char, int>
            {
                {'a', bit1},
                {'y', bit2}
            });

            _sut.Parse("a OR y -> z");

            Assert.That(_sut.Wires['z'], Is.EqualTo(result));
        }

        [TestCase(23, 1, 11)]
        public void RshiftProvidedInText_PerformsLeftshift_SetsThirdValue(int bit1, int shiftBy, int result)
        {
            _sut = new Circuit(new Dictionary<char, int>
            {
                {'a', bit1}
            });

            _sut.Parse($"a RSHIFT {shiftBy} -> z");

            Assert.That(_sut.Wires['z'], Is.EqualTo(result));
        }
    }

    public class Circuit
    {
        public Dictionary<char, int> Wires { get; }

        public Dictionary<string, Func<Dictionary<char, int>, GroupCollection, int>> Ops = new Dictionary
            <string, Func<Dictionary<char, int>, GroupCollection, int>>
        {
            {"([0-9]+)", (w, input) => int.Parse(input[1].Value)},
            {"([a-z]+) AND ([a-z]+)", (w, input) => w[input[1].Value[0]] & w[input[2].Value[0]]},
            {"([a-z]+) OR ([a-z]+)", (w, input) => w[input[1].Value[0]] | w[input[2].Value[0]]},
            {"([a-z]+) LSHIFT ([0-9]+)", (w, input) => w[input[1].Value[0]] << int.Parse(input[2].Value)},
            {"([a-z]+) RSHIFT ([0-9]+)", (w, input) => w[input[1].Value[0]] >> int.Parse(input[2].Value)},
        };

        public Circuit(Dictionary<char, int> wires = null)
        {
            Wires = wires ?? new Dictionary<char, int>();
        }

        public void Parse(string instruction)
        {
            var parts = instruction.Split(new[] {"->"}, StringSplitOptions.RemoveEmptyEntries);
            var operation = parts[0].Trim();
            var target = parts[1].Trim()[0];

            foreach (var pattern in Ops)
            {
                var supported = Regex.Match(operation, "^" + pattern.Key + "$");
                if (supported.Success)
                {
                    var val = pattern.Value(Wires, supported.Groups);
                    Wires[target] = val;
                    break;
                }
            }
        }
    }
}