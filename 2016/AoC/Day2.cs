using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AoC
{
    [TestFixture]
    public class Day2
    {
        [Test]
        public void Sample()
        {
            var phonePad = new PhonePad();

            var result = phonePad.Translate(new List<string>
            {
                "ULL",
                "RRDDD",
                "LURDL",
                "UUUUD"
            });

            Assert.That(result, Is.EqualTo(1985));
        }

        public class PhonePad : List<List<int>>
        {
            private int _fingerX = 1;
            private int _fingerY = 1;

            public PhonePad()
            {
                this.AddRange(new List<List<int>>
                {
                    new List<int> {1, 2, 3},
                    new List<int> {4, 5, 6},
                    new List<int> {7, 8, 9}
                });
            }

            public int Translate(List<string> sequence)
            {
                foreach (var line in sequence)
                {
                    foreach (var move in line)
                    {
                    }
                }

                return -1;
            }
        }
    }
}
