using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
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

        [Test]
        public void Test2()
        {
            var result = _compressor.PredictLength2(string.Join("", Input));

            Assert.That(result, Is.EqualTo(11558231665));
        }

        [TestCase("XYZ", 3)]
        [TestCase("(3x3)XYZ", 9)]
        [TestCase("X(8x2)(3x3)ABCY", 20)]
        [TestCase("(27x12)(20x12)(13x14)(7x10)(1x12)A", 241920)]
        [TestCase("(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN", 445)]
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

        public long PredictLength2(string compressed)
        {
            var stackOfCharacters = new Stack<char>(compressed.Reverse());

            long count = CountSequence(stackOfCharacters);

            return count;
        }

        private static long CountSequence(Stack<char> stackOfCharacters, int? maxCapture = null)
        {
            long count = 0;
            var iterations = 0;
            var maxCharsToCount = maxCapture.GetValueOrDefault(int.MaxValue);

            while (stackOfCharacters.Any() && maxCharsToCount > iterations++)
            {
                var ch = stackOfCharacters.Pop();
                if (ch != '(')
                {
                    count++;
                    continue;
                }

                var capture = new ExpressionToken(CaptureExpression(stackOfCharacters));
                iterations += capture.ValueLength - 1;

                var multiplicationFactor = capture.MultiplyBy;
                var captureCharacters = capture.SequenceLength;

                var countSequence = CountSequence(stackOfCharacters, captureCharacters);
                iterations += captureCharacters;

                var expandedCount = countSequence*multiplicationFactor;
                count += expandedCount;
            }

            return count;
        }

        private static string CaptureExpression(Stack<char> stackOfCharacters)
        {
            var op = new StringBuilder();
            while (!op.ToString().EndsWith(")") && stackOfCharacters.Any())
            {
                op.Append(stackOfCharacters.Pop());
            }

            var token = op.ToString().Remove(op.ToString().Length - 1);
            return token;
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
                MultiplyBy = int.Parse(matches.Groups[2].Value);
            }
        }

        public int SequenceLength { get; }
        public int MultiplyBy { get; }
        
        public override int ValueLength => Value.Length + 2;
    }
    

    public abstract class Token
    {
        public Token Captured { get; set; }
        public string Value { get; set; }
        public abstract int ValueLength { get; }

        protected Token(string value)
        {
            Value = value;
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
