using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AoC.Infrastructure;
using NUnit.Framework;

namespace AoC
{
    [TestFixture]
    public class Day9 : Challenge
    {
        private Compressor _compressor;

        [SetUp]
        public void Setup()
        {
            _compressor = new Compressor();
        }

        [TestCase("ADVENT", "ADVENT")]
        [TestCase("A(1x5)BC", "ABBBBBC")]
        [TestCase("(3x3)XYZ", "XYZXYZXYZ")]
        [TestCase("A(2x2)BCD(2x2)EFG", "ABCBCDEFEFG")]
        [TestCase("X(8x2)(3x3)ABCY", "X(3x3)ABC(3x3)ABCY")]
        public void Sample(string compressed, string decompressed)
        {
            var result = _compressor.Decompress(compressed);

            Assert.That(result.Decompressed, Is.EqualTo(decompressed));
        }

        [Test]
        public void Test()
        {
            var result = _compressor.Decompress(string.Join("", Input));

            Assert.That(result.Decompressed.Length, Is.EqualTo(74532));
        }
    }

    public class Compressor
    {
        public Body Decompress(string compressed)
        {
            var buffer = new StringBuilder();

            for (var offset = 0; offset < compressed.Length; offset++)
            {
                var printDuration = 1;
                var sequenceBuffer = new StringBuilder();
                sequenceBuffer.Append(compressed[offset]);

                if (sequenceBuffer.ToString() == "(")
                {
                    var next = "";
                    while (next != ")")
                    {
                        offset++;
                        next = compressed[offset].ToString();
                        sequenceBuffer.Append(next);
                    }
                }

                var expanderRegex = new Regex(@"\(([0-9]+)x([0-9]+)\)");
                if (expanderRegex.IsMatch(sequenceBuffer.ToString()))
                {
                    var matches = expanderRegex.Matches(sequenceBuffer.ToString())[0];
                    var sequenceLength = int.Parse(matches.Groups[1].Value);
                    printDuration = int.Parse(matches.Groups[2].Value);

                    sequenceBuffer.Clear();
                    for (var i = 0; i < sequenceLength; i++)
                    {
                        offset++;
                        sequenceBuffer.Append(compressed[offset]);
                    }
                }

                for (var i = 0; i < printDuration; i++)
                {
                    buffer.Append(sequenceBuffer);
                }
            }

            return new Body(compressed, buffer.ToString());
        }
    }

    public class Body
    {
        public string Compressed { get; set; }
        public string Decompressed { get; set; }

        public Body(string compressed, string decompressed)
        {
            Compressed = compressed;
            Decompressed = decompressed;
        }
    }
}
