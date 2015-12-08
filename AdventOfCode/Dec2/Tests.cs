using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode.Dec2
{
    [TestFixture]
    public class Tests
    {
        private PaperCalculator _calc;

        [SetUp]
        public void SetUp()
        {
            _calc = new PaperCalculator();
        }

        [TestCase(2, 3, 4, 52)]
        public void SurfaceAreaOf_CalcsCorrectly(int length, int width, int height, int expectedSa)
        {
            var sa = _calc.SurfaceAreaOf(length, width, height);

            Assert.That(sa, Is.EqualTo(expectedSa));
        }

        [TestCase(2, 3, 4, 58)]
        public void WrappingPaperRequired_CalcsCorrectly(int length, int width, int height, int expectedSa)
        {
            var sa = _calc.WrappingPaperRequired(length, width, height);

            Assert.That(sa, Is.EqualTo(expectedSa));
        }

        [Test]
        public void WrappingPaperRequired_WithStringRepresentation_CalcsCorrectly()
        {
            var sides = "2x3x4".Split('x').Select(int.Parse).ToArray();
            var sa = _calc.WrappingPaperRequired(sides);

            Assert.That(sa, Is.EqualTo(58));
        }

        [TestCase(2, 3, 4, 34)]
        [TestCase(1, 1, 10, 14)]
        public void RibbonRequired_CalcsCorrectly(int w, int l, int h, int expected)
        {
            var ribbonLength = _calc.RibbonRequred(w, l, h);

            Assert.That(ribbonLength, Is.EqualTo(expected));
        }

        [Test]
        public void WrappingPaperRequired_Works()
        {
            var contents = File.ReadAllText("c:\\dev\\AdventOfCode\\AdventOfCode\\Dec2\\Test.txt")
                               .Split(new [] {Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                               .ToList();

            var total = _calc.WrappingPaperRequired(contents);
            var totalRibbon = _calc.RibbonRequred(contents);

            Console.WriteLine(total.ToString());
            Console.WriteLine(totalRibbon.ToString());
        }
    }
}
