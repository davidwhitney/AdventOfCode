using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AdventOfCode.Dec4
{
    [TestFixture]
    public class Tests
    {
        private AdventCoinMiner _miner;

        [SetUp]
        public void SetUp()
        {
            _miner = new AdventCoinMiner();
        }

        [Ignore("Slow!")]
        [TestCase("abcdef", 609043)]
        [TestCase("pqrstuv", 1048970)]
        [TestCase("yzbqklnj", 282749)] // Challenge
        public void Mine_WithKnownHash_ReturnsKnownValue(string stub, int expected)
        {
            var lowestStem = _miner.Mine(stub);

            Assert.That(lowestStem, Is.EqualTo(expected));
        }

        [Ignore("Slow!")]
        [TestCase("yzbqklnj", 9962624)] // Challenge
        public void Mine_WithKnownHashForSixZeros_ReturnsKnownValue(string stub, int expected)
        {
            var lowestStem = _miner.Mine(stub, 6);

            Assert.That(lowestStem, Is.EqualTo(expected));
        }
    }
}
