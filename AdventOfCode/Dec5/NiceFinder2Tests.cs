using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode.Dec5
{
    [TestFixture]
    public class NiceFinder2Tests
    {
        private NiceFinder2 _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new NiceFinder2();
        }

        [TestCase("xyxy")]
        [TestCase("aabcdefgaa")]
        [TestCase("qjhvhtzxzqqjkmpb")]
        [TestCase("xxyxx")]
        public void IsNice_WithTwoPairs_IsNice(string s)
        {
            var count = NiceFinder2.ContainsLetterPairs(s);

            Assert.That(count, Is.True);
        }

        [TestCase("aaa")]
        public void IsNice_WithOverlappingTwoPairs_IsNotNice(string s)
        {
            var count = NiceFinder2.ContainsLetterPairs(s);

            Assert.That(count, Is.False);
        }

        [TestCase("xyx")]
        [TestCase("abcdefeghi")]
        [TestCase("aaa")]
        public void IsNice_WithSpacedRepeatingLetters_IsNice(string s)
        {
            var count = NiceFinder2.ContainsSpacedRepeats(s);

            Assert.That(count, Is.True);
        }
        
        [Test]
        public void IsNice_DoChallange()
        {
            //var contents = File.ReadLines("c:\\dev\\AdventOfCode\\AdventOfCode\\Dec5\\Test.txt").ToList();

            //Assert.That(contents.Count(s => _sut.IsNice(s)), Is.EqualTo(236));
        }
    }
}