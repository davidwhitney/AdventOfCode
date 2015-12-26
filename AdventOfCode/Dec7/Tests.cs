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
    }

    public class Circuit
    {
        public Dictionary<char, int> Wires { get; }

        public Circuit(Dictionary<char, int> wires = null)
        {
            Wires = wires ?? new Dictionary<char, int>();
        }

        public void Parse(string instruction)
        {
            var assignment = Regex.Match(instruction, "([0-9]+) -> ([a-z]+)");
            if (assignment.Success)
            {
                Wires[assignment.Groups[2].Value[0]] = int.Parse(assignment.Groups[1].Value);
            }

            var bitwiseAnd = Regex.Match(instruction, "([a-z]+) (AND|OR) ([a-z]+) -> ([a-z]+)");
            var op = bitwiseAnd.Groups[2].Value;

            if (bitwiseAnd.Success)
            {
                var firstRegister = bitwiseAnd.Groups[1].Value[0];
                var secondRegister = bitwiseAnd.Groups[3].Value[0];
                var targetRegister = bitwiseAnd.Groups[4].Value[0];

                if (op == "AND")
                {
                    Wires[targetRegister] = Wires[firstRegister] & Wires[secondRegister];
                }

                if (op == "OR")
                {
                    Wires[targetRegister] = Wires[firstRegister] | Wires[secondRegister];
                }
            }
        }
    }
}
