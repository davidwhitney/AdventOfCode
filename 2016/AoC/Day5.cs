using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace AoC
{
    [TestFixture]
    public class Day5
    {
        [Test]
        [Explicit]
        public void Sample()
        {
            var input = "abc";

            var password = GetPassword(input);

            Assert.That(password, Is.EqualTo("18f47a30"));
        }

        [Test]
        [Explicit]
        public void Test()
        {
            var password = GetPassword("reyedfim");
            
            Assert.That(password, Is.EqualTo("f97c354d"));
        }

        [Test]
        [Explicit]
        public void Test2()
        {
            var password = GetPassword("reyedfim", true);
            
            Assert.That(password, Is.EqualTo("863dde27"));
        }

        private static string GetPassword(string input, bool positional = false)
        {
            var filled = 0;
            var buffer = new string[8];
            for (var counter = 0; filled < 8; counter++)
            {
                var hash = Md5(input + counter);
                if (hash.StartsWith("00000"))
                {
                    var character = positional ? hash.Substring(6, 1) : hash.Substring(5, 1);

                    int parsedPositionl;
                    var success = int.TryParse(hash.Substring(5, 1), out parsedPositionl);
                    var position = positional ? parsedPositionl : filled;

                    if (positional && !success)
                    {
                        continue;
                    }
                    if (position > 7)
                    {
                        continue;
                    }
                    if (buffer[position] != null)
                    {
                        continue;
                    }
                    
                    buffer[position] = character;
                    filled++;
                }
            }

            return string.Join("", buffer);
        }

        private static string Md5(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);
            var hex = BitConverter.ToString(hash);
            return hex.Replace("-", "").ToLower();
        }
    }
}
