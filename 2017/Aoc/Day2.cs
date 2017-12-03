using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Aoc
{
    [TestFixture]
    public class Day2
    {
        [Test]
        public void Part1()
        {
            var sheet = new[]
            {
                "5\t1\t9\t5",
                "7\t5\t3",
                "2\t4\t6\t8"
            };

            var checksum = new Spreadsheet(sheet).Checksum();

            Assert.That(checksum, Is.EqualTo(18));
        }

        [Test]
        public void Part1Sample()
        {
            var contents = File.ReadAllText("Day2-1.txt").Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var checksum = new Spreadsheet(contents).Checksum();
            Assert.That(checksum, Is.EqualTo(44216));
        }

        [Test]
        public void Part2()
        {
            var sheet = new[]
            {
                "5\t9\t2\t8",
                "9\t4\t7\t3",
                "3\t8\t6\t5"
            };

            var checksum = new Spreadsheet(sheet).AltChecksum();

            Assert.That(checksum, Is.EqualTo(9));
        }

        [Test]
        public void Part2Sample()
        {
            var contents = File.ReadAllText("Day2-2.txt").Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var checksum = new Spreadsheet(contents).AltChecksum();
            Assert.That(checksum, Is.EqualTo(320));
        }


        public class Spreadsheet : List<Row>
        {
            public Spreadsheet(IEnumerable<string> sheet) => AddRange(sheet
                .Select(line => line.Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries))
                .Select(values => new Row(values.Select(int.Parse).ToList()))
                .ToList());

            public int Checksum() => this.Sum(x => x.RowChecksum());
            public int AltChecksum() => this.Sum(x => x.AlternateChecksum());
        }

        public class Row : List<int>
        {
            public Row(IEnumerable<int> values) => AddRange(values);

            public int RowChecksum()
            {
                return this.Max() - this.Min();
            }

            public int AlternateChecksum()
            {
                var queue = new Queue<int>(this);

                while (queue.Count > 0)
                {
                    var option = queue.Dequeue();
                    foreach (var item in this)
                    {
                        if (item == option) continue;

                        if (option % item == 0)
                        {
                            return option / item;
                        }
                    }
                }

                return 0;
            }
        }
    }
}