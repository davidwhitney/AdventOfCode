using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;

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
            var sa = _calc.WrappingPaperRequired("2x3x4");

            Assert.That(sa, Is.EqualTo(58));
        }

        [Test]
        public void WrappingPaperRequired_Works()
        {
            var contents = File.ReadAllText("c:\\dev\\AdventOfCode\\AdventOfCode\\Dec2\\Test.txt")
                               .Split(new [] {Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                               .ToList();

            var total = _calc.WrappingPaperRequired(contents);

            Console.WriteLine(total.ToString());
        }
    }

    public class PaperCalculator
    {
        public int WrappingPaperRequired(IEnumerable<string> dimensions)
        {
            return dimensions.Sum(WrappingPaperRequired);
        }

        public int WrappingPaperRequired(string dimensions)
        {
            var sides = dimensions.Split('x').Select(int.Parse).ToArray();
            return WrappingPaperRequired(sides);
        }

        public int WrappingPaperRequired(params int[] sides)
        {
            var surfaceArea = SurfaceAreaOf(sides[0], sides[1], sides[2]);
            var sidesOrdered = sides.OrderBy(x => x).ToList();
            var smallestSideArea = sidesOrdered.First()*sidesOrdered.Skip(1).First();
            return surfaceArea + smallestSideArea;
        }

        public int SurfaceAreaOf(int length, int width, int height)
        {
            return 2*length*width + 2*width*height + 2*height*length;
        }
    }
}
