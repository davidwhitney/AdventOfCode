using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AoC.Infrastructure;
using NUnit.Framework;
using NUnit.Framework.Constraints;

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

        [TestCase("XYZ", 3)]
        [TestCase("(3x3)XYZ", 9)]
        [TestCase("X(8x2)(3x3)ABCY", 20)]
        public void Test2(string compressed, int length)
        {
            var lengthPrediction = _compressor.PredictLength2(compressed);

            Assert.That(lengthPrediction, Is.EqualTo(length));
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

        public int PredictLength2(string compressed)
        {
            var inputTokens = compressed.Split(new[] {'(', ')'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            var exRg = new Regex(@"([0-9]+)x([0-9]+)");
            var tokes = new List<Token>();

            foreach (var input in inputTokens)
            {
                var thisToke = exRg.IsMatch(input)
                    ? (Token) new ExpressionToken(input)
                    : new LiteralToken(input);

                var lastOrDefault = tokes.LastOrDefault();

                if (lastOrDefault is ExpressionToken
                    && thisToke is LiteralToken)
                {
                    var et = lastOrDefault as ExpressionToken;
                    var t1 = new LiteralToken(input.Substring(0, et.SequenceLength));
                    var t2 = new LiteralToken(input.Substring(et.SequenceLength, input.Length - et.SequenceLength));
                    lastOrDefault.SetNext(t1);
                    t1.SetNext(t2);
                    tokes.Add(t1);
                    tokes.Add(t2);
                }
                else
                {
                    lastOrDefault?.SetNext(thisToke);
                    tokes.Add(thisToke);
                }
            }

            int length = 0;
            foreach (var token in tokes)
            {
                length += token.ComputedLength();
            }
            
            return length;
        }
    }

    public class ExpressionToken : Token
    {
        public ExpressionToken(string value) : base(value)
        {
            var expanderRegex = new Regex(@"([0-9]+)x([0-9]+)");
            if (expanderRegex.IsMatch(value))
            {
                var matches = expanderRegex.Matches(value)[0];
                SequenceLength = int.Parse(matches.Groups[1].Value);
                PrintDuration = int.Parse(matches.Groups[2].Value);
            }
        }

        public int SequenceLength { get; }
        public int PrintDuration { get; }

        public override int ComputedLength() => ComputeLength();

        private int ComputeLength()
        {
            return Next.ComputedLength() * (PrintDuration - 1);
        }
    }


    public class LiteralToken : Token
    {
        public LiteralToken(string value) : base(value)
        {
        }

        public override int ComputedLength() => Value.Length;
    }

    public abstract class Token
    {
        public Token Next { get; set; }
        public string Value { get; set; }
        public abstract int ComputedLength();

        protected Token(string value)
        {
            Value = value;
        }

        public void SetNext(Token t)
        {
            Next = t;
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
