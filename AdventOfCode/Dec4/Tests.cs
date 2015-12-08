using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
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

    public class AdventCoinMiner
    {
        public int Mine(string stub, int leadingZeroCount = 5)
        {
            for (var i = 0; i < int.MaxValue; i++)
            {
                var hash = Hash(stub + i);

                if (hash.StartsWith(string.Join("", Enumerable.Repeat("0", leadingZeroCount))))
                {
                    return i;
                }
            }

            return 0;
        }

        private static string Hash(string stub)
        {
            var encodedPassword = new UTF8Encoding().GetBytes(stub);
            var hash = ((HashAlgorithm) CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            return BitConverter.ToString(hash)
                .Replace("-", string.Empty)
                .ToLower();
        }
    }
}
