using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode.Dec5
{
    [TestFixture]
    public class NiceFinderTests
    {
        private NiceFinder _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new NiceFinder();
        }

        [TestCase("aei", "At least three vowels")]
        public void ContainsThreeVowels_ValidCases_IsNice(string s, string msg)
        {
            var count = NiceFinder.ContainsThreeVowels(s);

            Assert.That(count, Is.True, "Strings must contain: " + msg);
        }

        [TestCase("ab")]
        [TestCase("cd")]
        [TestCase("pq")]
        [TestCase("xy")]
        [TestCase("xaax")]
        [TestCase("xxxy")]
        [TestCase("aeiouxy")]
        public void IsNice_ValidCases_IsNotNice(string s)
        {
            var count = _sut.IsNice(s);

            Assert.That(count, Is.False);
        }

        [TestCase("ugknbfddgicrmopn", true)]
        [TestCase("aaa", true)]
        [TestCase("jchzalrnumimnmhp", false)]
        [TestCase("haegwjzuvuyypxyu", false)]
        [TestCase("dvszwmarrgswjxmb", false)]
        public void IsNice_ProvidedTestCases(string s, bool status)
        {
            var count = _sut.IsNice(s);

            Assert.That(count, Is.EqualTo(status));
        }

        [Test]
        public void IsNice_DoChallange()
        {
            var contents = File.ReadLines("c:\\dev\\AdventOfCode\\AdventOfCode\\Dec5\\Test.txt").ToList();

            Assert.That(contents.Count(s => _sut.IsNice(s)), Is.EqualTo(236));
        }
    }
}
