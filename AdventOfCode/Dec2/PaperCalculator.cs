using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Dec2
{
    public class PaperCalculator
    {
        public PaperCalculator()
        {
            var data = "data";
            var result = data is string a ? a : "Not a String";

            void TotesALocalFunction()
            {
                Debug.WriteLine("This works too");
            }

            TotesALocalFunction();

            var parsed = int.TryParse("1", out var numberParsed);

            var ints = GetInts();
            var aa = ints.a;
            var ab = ints.b;

            (int a, int b) GetInts()
            {
                // How about tuples!
                return (1, 2);
            }

            var ccc = (A: "a", B: "aa");
            var aaaaa = ccc.A;
        }

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
            int.TryParse("1", out var abc);
            Console.WriteLine(abc);

            void A()
            {
                
            }

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