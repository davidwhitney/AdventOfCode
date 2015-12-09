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
        [TestCase("qjhvhtzxzqqjkmpb")]
        [TestCase("xxyxx")]
        public void ContainsLetterPairs_WithTwoPairs_IsNice(string s)
        {
            var count = _sut.IsNice(s);

            Assert.That(count, Is.True);
        }

        [TestCase("aaa")]
        public void ContainsLetterPairs_WithOverlappingTwoPairs_IsNotNice(string s)
        {
            var count = _sut.IsNice(s);

            Assert.That(count, Is.False);
        }

        [TestCase("qjhvhtzxzqqjkmpb")]
        [TestCase("xxyxx")]
        public void IsNice_Examples_AreNice(string s)
        {
            var count = _sut.IsNice(s);

            Assert.That(count, Is.True);
        }

        [TestCase("uurcxstgmygtbstg")]
        [TestCase("ieodomkazucvgmuy")]
        public void IsNice_Examples_AreNotNice(string s)
        {
            var count = _sut.IsNice(s);

            Assert.That(count, Is.False);
        }
        
        [Test]
        public void IsNice_DoChallange()
        {
            var contents = File.ReadLines("c:\\dev\\AdventOfCode\\AdventOfCode\\Dec5\\Test.txt").ToList();

            Assert.That(contents.Count(s => _sut.IsNice(s)), Is.EqualTo(51));
        }
    }
}