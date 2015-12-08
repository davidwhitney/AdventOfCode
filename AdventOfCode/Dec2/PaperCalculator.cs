using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Dec2
{
    public class PaperCalculator
    {
        public int SurfaceAreaOf(int length, int width, int height)
        {
            return 2*length*width + 2*width*height + 2*height*length;
        }

        public int WrappingPaperRequired(IEnumerable<string> dimensions)
        {
            return dimensions.Sum(dim => WrappingPaperRequired(ToSides(dim)));
        }

        public int RibbonRequred(IEnumerable<string> dimensions)
        {
            return dimensions.Sum(dim => RibbonRequred(ToSides(dim)));
        }

        public int WrappingPaperRequired(params int[] sides)
        {
            var surfaceArea = SurfaceAreaOf(sides[0], sides[1], sides[2]);
            var sidesOrdered = sides.OrderBy(x => x).ToList();
            var smallestSideArea = sidesOrdered[0]*sidesOrdered[1];
            return surfaceArea + smallestSideArea;
        }

        public int RibbonRequred(params int[] sides)
        {
            var sidesOrdered = sides.OrderBy(x => x).ToList();
            var subTotal = sidesOrdered[0] + sidesOrdered[0] + sidesOrdered[1] + sidesOrdered[1];
            var ribbonLength = sidesOrdered[0]*sidesOrdered[1]*sidesOrdered[2];
            return subTotal + ribbonLength;
        }

        private static int[] ToSides(string dim)
        {
            return dim.Split('x').Select(int.Parse).ToArray();
        }
    }
}