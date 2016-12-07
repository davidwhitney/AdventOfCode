using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoC.Infrastructure;
using NUnit.Framework;

namespace AoC
{
    [TestFixture]
    public class Day7 : Challenge
    {
        [TestCase("abba[mnop]qrst", true)]
        [TestCase("abcd[bddb]xyyx", false)]
        [TestCase("aaaa[qwer]tyui", false)]
        [TestCase("ioxxoj[asdfgh]zxcvbn", true)]
        [TestCase("zxcvbn[asdfgh]ohxoij[asdfgh]zxcvbn[asdfgh]oxxoij", true)]
        public void Sample(string sequence, bool supportsAbba)
        {
            var supports = new Message(sequence).SuppportsAbba;
            Assert.That(supports, Is.EqualTo(supportsAbba));
        }

        [TestCase("aba[bab]xyz", true)]
        [TestCase("xyx[xyx]xyx", false)]
        [TestCase("aaa[kek]eke", true)]
        [TestCase("zazbz[bzb]cdb", true)]
        public void Sample2(string sequence, bool supportsSsl)
        {
            var supports = new Message(sequence).SupportsSsl;
            Assert.That(supports, Is.EqualTo(supportsSsl));
        }

        [Test]
        public void Test()
        {
            var containsAbba = Input.Count(s => new Message(s).SuppportsAbba);
            Assert.That(containsAbba, Is.EqualTo(110));
        }

        [Test]
        public void Test2()
        {
            var containsAbba = Input.Count(s => new Message(s).SupportsSsl);
            Assert.That(containsAbba, Is.EqualTo(242));
        }

        public class Message
        {
            public List<string> External { get; } = new List<string>();
            public List<string> Enclosed { get; } = new List<string>();
            public bool SuppportsAbba => !Enclosed.Any(ContainsAbba) && External.Any(ContainsAbba);
            public bool SupportsSsl => ContainsAbaAndBab();

            public Message(string sequence)
            {
                var buffer = new StringBuilder();
                foreach (var ch in sequence)
                {
                    if (ch == '[')
                    {
                        External.Add(buffer.ToString());
                        buffer.Clear();
                        continue;
                    }

                    if (ch == ']')
                    {
                        Enclosed.Add(buffer.ToString());
                        buffer.Clear();
                        continue;
                    }

                    buffer.Append(ch);
                }

                if (buffer.Length > 0)
                {
                    External.Add(buffer.ToString());
                }
            }

            private static bool ContainsAbba(string part)
            {
                foreach (var window in WindowsFrom(part, 4))
                {
                    var half = window.Substring(0, 2);
                    var predictedOtherHalff = string.Join("", half.Reverse());

                    if (window.All(x => x == window[0]))
                    {
                        return false;
                    }

                    if (half + predictedOtherHalff == window)
                    {
                        return true;
                    }
                }

                return false;
            }

            private bool ContainsAbaAndBab()
            {
                var abaFound = new List<string>();
                foreach (var part in External)
                {
                    abaFound.AddRange(WindowsFrom(part, 3).Where(window => window[0] == window[2]));
                }

                var babFound = new List<string>();
                foreach (var part in Enclosed)
                {
                    babFound.AddRange(WindowsFrom(part, 3).Where(window => window[0] == window[2]));
                }

                return abaFound.Select(aba => new string(new[] {aba[1], aba[0], aba[1]}))
                               .Any(expectedBab => babFound.Contains(expectedBab));
            }

            private static IEnumerable<string> WindowsFrom(string source, int characters)
            {
                for (var index = 0; index < source.Length; index++)
                {
                    if (index + characters <= source.Length)
                    {
                        yield return source.Substring(index, characters);
                    }
                }
            }
        }
    }
}