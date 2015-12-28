using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace AdventOfCode.Dec8
{
    [TestFixture]
    public class Tests
    {
        private SpecialCounter _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new SpecialCounter();
        }

        [TestCase("\"abc\"", 3)]
        [TestCase("abc", 3)]
        [TestCase("\"\"", 0)]
        [TestCase("aaa\\\"aaa", 7)]
        [TestCase("\"", 0)]
        [TestCase("\\x27", 1)]
        [TestCase("a b", 2)]
        public void CountActualCharacters_GivenJustRegularChars_CountsCorrectly(string src, int lengthWithoutSpecials)
        {
            var specialCount = _sut.CountActualCharacters(src);

            Assert.That(specialCount, Is.EqualTo(lengthWithoutSpecials));
        }
    }

    public class SpecialCounter
    {
        public int CountActualCharacters(string original)
        {
            var copy = new string(original.ToCharArray());

            copy = ReplaceHex(copy);
            copy = copy.Replace("\"", "^");

            if (original.StartsWith("\""))
            {
                copy = copy.Insert(0, "^");
                copy = copy.Remove(1, 1);
            }

            if(original.Length > 1 && original.EndsWith("\""))
            {
                copy = copy.Insert(copy.Length - 1, "^");
                copy = copy.Remove(copy.Length - 2, 1);
            }

            var remainingSpecials = copy.Count(ch => ch == '^');
            var whitespace = copy.Count(ch => ch == ' ');

            return copy.Length - remainingSpecials  - whitespace;
        }

        private static string ReplaceHex(string copy)
        {
            var caps = Regex.Match(copy, @"(\\x[0-9]+)");
            if (!caps.Success)
            {
                return copy;
            }

            foreach (var match in caps.Groups)
            {
                if (!(match is Match))
                {
                    continue;
                }

                var matchString = ((Match) match).Value;
                var charCode = int.Parse(matchString.Replace("\\x", ""));
                var val = ((char) charCode).ToString();
                copy = copy.Replace(matchString, val);
            }
            return copy;
        }
    }
}
