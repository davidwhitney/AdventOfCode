using System;
using System.Linq;
using AoC.Infrastructure;
using NUnit.Framework;

namespace AoC
{
    [TestFixture]
    public class Day4 : Challenge
    {
        [TestCase("aaaaa-bbb-z-y-x-123[abxyz]", true)]
        [TestCase("a-b-c-d-e-f-g-h-987[abcde]", true)]
        [TestCase("not-a-real-room-404[oarel]", true)]
        [TestCase("totally-real-room-200[decoy]", false)]
        public void Sample(string code, bool real)
        {
            var room = new Room(code);
            Assert.That(room.IsValid(), Is.EqualTo(real));
        }

        [Test]
        public void Sample2()
        {
            var room = new Room("qzmt-zixmtkozy-ivhz-343[zimth]");
            Assert.That(room.DecryptedName, Is.EqualTo("very encrypted name"));
        }

        [Test]
        public void Test()
        {
            var sectorSum = Input.Select(line => new Room(line)).Where(room => room.IsValid()).Sum(room => room.SectorId);
            Assert.That(sectorSum, Is.EqualTo(361724));
        }

        [Test]
        public void Test2()
        {
            var rooms = Input.Select(x => new Room(x));
            var pole = rooms.First(r => r.DecryptedName.Contains("north"));
            Assert.That(pole.SectorId, Is.EqualTo(482));
        }

        public class Room
        {
            public string Id { get; set; }
            public string Checksum { get; set; }
            public string Name { get; set; }
            public int SectorId { get; set; }
            public string ParsedId { get; set; }

            public Room(string code)
            {
                var parts = code.Split(new[] { "[" }, StringSplitOptions.RemoveEmptyEntries);
                parts[1] = parts[1].TrimEnd(']');

                Id = parts[0];
                Checksum = parts[1];

                var lastHashInId = Id.LastIndexOf("-");
                Name = Id.Substring(0, lastHashInId);

                var sectorIdAsString = Id.Substring(lastHashInId + 1);
                SectorId = int.Parse(sectorIdAsString);

                var normalised = Id.Replace("-", "");
                ParsedId = normalised.Remove(normalised.Length - sectorIdAsString.Length, sectorIdAsString.Length);
            }

            public bool IsValid()
            {
                var group = ParsedId.GroupBy(x => x).OrderByDescending(g => g.Count()).ThenBy(x => x.Key).ToList();
                var computedChecksum = string.Join("", group.Take(5).Select(x => x.Key));
                return Checksum == computedChecksum;
            }

            public string DecryptedName => DecryptName();

            public string DecryptName()
            {
                var target = "";
                foreach (var ch in Name)
                {
                    if (ch == '-')
                    {
                        target += ' ';
                        continue;
                    }

                    var shift = SectorId%26;
                    var newLetter = (char) (ch + shift);
                    if (newLetter > 'z')
                    {
                        var diff = newLetter - 'z';
                        newLetter = (char) ('a' + diff - 1);
                    }
                    target += newLetter;
                }

                return target;
            }
        }
    }
}