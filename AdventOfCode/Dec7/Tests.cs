using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        }
    }

    public class Circuit
    {
        private Dictionary<char, int> _wires;

        public Circuit()
        {
            _wires = new Dictionary<char, int>();
        }

        public void Parse(string instruction)
        {
            var assignment = Regex.Match(instruction, "([0-9]+) -> ([a-z]+)");
            if (assignment.Success)
            {
                _wires[assignment.Groups[2].Value[0]] = int.Parse(assignment.Groups[1].Value);
            }

        }
    }
}
