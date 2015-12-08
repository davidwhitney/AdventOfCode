using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
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
    }

    public class PaperCalculator
    {
        public int WrappingPaperRequired(int length, int width, int height)
        {
            var surfaceArea = SurfaceAreaOf(length, width, height);
            var sidesOrdered = new[] {length, width, height}.OrderBy(x => x).ToList();
            var smallestSideArea = sidesOrdered.First()*sidesOrdered.Skip(1).First();
            return surfaceArea + smallestSideArea;
        }

        public int SurfaceAreaOf(int length, int width, int height)
        {
            return 2*length*width + 2*width*height + 2*height*length;
        }
    }
}
