using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoC.Infrastructure;
using NUnit.Framework;

namespace AoC
{
    [TestFixture]
    public class Day6 : Challenge
    {
        [Test]
        public void Sample()
        {
            var sample =
@"eedadn
drvtee
eandsr
raavrd
atevrs
tsrnev
sdttsa
rasrtv
nssdts
ntnada
svetve
tesnvt
vntsnd
vrdear
dvrsen
enarar";
            var word = GetWord(sample);

            Assert.That(word, Is.EqualTo("easter"));
        }

        [Test]
        public void Sample2()
        {
            var sample =
@"eedadn
drvtee
eandsr
raavrd
atevrs
tsrnev
sdttsa
rasrtv
nssdts
ntnada
svetve
tesnvt
vntsnd
vrdear
dvrsen
enarar";
            var word = GetWord(sample, true);

            Assert.That(word, Is.EqualTo("advent"));
        }

        [Test]
        public void Test()
        {
            var word = GetWord(Input);

            Assert.That(word, Is.EqualTo("mlncjgdg"));
        }

        [Test]
        public void Test2()
        {
            var word = GetWord(Input, true);

            Assert.That(word, Is.EqualTo("bipjaytb"));
        }

        private static string GetWord(string sample, bool leastLikely = false)
        {
            var lines = sample.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            return GetWord(lines, leastLikely);
        }

        private static string GetWord(string[] lines, bool leastLikely = false)
        {
            var cols = new List<string>();
            var wide = lines.Max(x=>x.Length);
            for (var x = 0; x < wide; x++)
            {
                var sb = new StringBuilder();
                foreach (var line in lines)
                {
                    sb.Append(line[x]);
                }
                cols.Add(sb.ToString());
            }

            var buffer = "";
            foreach (var line in cols)
            {
                var frequent = leastLikely
                    ? line.GroupBy(x => x).OrderBy(x => x.Count()).ToList()
                    : line.GroupBy(x => x).OrderByDescending(x => x.Count()).ToList();
                buffer += frequent.First().Key;
            }
            return buffer;
        }
    }
}
