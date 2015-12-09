using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace AdventOfCode.Dec6
{
    [TestFixture]
    public class Tests
    {
        private LightBox _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new LightBox();
        }

        [Test]
        public void Toggle_SingleCoord_LightsUpCorrectly()
        {
            _sut.Toggle(new Coord(0, 0), new Coord(0, 0), true);

            Assert.That(_sut.StatusAt(0,0), Is.EqualTo(true));
        }

        [Test]
        public void Toggle_SmallGroup_LightsUpCorrectly()
        {
            _sut.Toggle(new Coord(0, 0), new Coord(2, 2), true);

            Assert.That(_sut.StatusAt(0,0), Is.EqualTo(true));
            Assert.That(_sut.StatusAt(1,0), Is.EqualTo(true));
            Assert.That(_sut.StatusAt(2,0), Is.EqualTo(true));
                                        
            Assert.That(_sut.StatusAt(0,1), Is.EqualTo(true));
            Assert.That(_sut.StatusAt(1,1), Is.EqualTo(true));
            Assert.That(_sut.StatusAt(2,1), Is.EqualTo(true));
                                         
            Assert.That(_sut.StatusAt(0,2), Is.EqualTo(true));
            Assert.That(_sut.StatusAt(1,2), Is.EqualTo(true));
            Assert.That(_sut.StatusAt(2,2), Is.EqualTo(true));
        }

        [Test]
        public void Toggle_NaturalLanguage_ParsesCorrectly()
        {
            var instruction = "turn on 0,0 through 0,0";

            _sut.Toggle(instruction);

            Assert.That(_sut.StatusAt(0, 0), Is.EqualTo(true));
        }

        [Test]
        public void Toggle_DoChallange()
        {
            var contents = File.ReadLines("c:\\dev\\AdventOfCode\\AdventOfCode\\Dec6\\Test.txt").ToList();
            

        }

    }

    public class LightBox
    {
        private readonly bool[,] _box;

        public LightBox()
        {
            _box = new bool[1000, 1000];
        }

        public void Toggle(string instruction)
        {
            var results = Regex.Match(instruction, "turn (.*) ([0-9+]),([0-9+]) through ([0-9+]),([0-9+])");

            var bottomLeft = new Coord(int.Parse(results.Groups[2].Value), int.Parse(results.Groups[3].Value));
            var topRight = new Coord(int.Parse(results.Groups[4].Value), int.Parse(results.Groups[5].Value));
            var turnOn = results.Groups[1].Value == "on";

            Toggle(bottomLeft, topRight, turnOn);

        }

        public void Toggle(Coord bottomLeft, Coord topRight, bool turnOn)
        {
            for (int x = bottomLeft.X; x <= topRight.X; x++)
            {
                for (int y = bottomLeft.Y; y <= topRight.Y; y++)
                {
                    _box[y, x] = turnOn;
                }
            }
        }

        public bool StatusAt(int x, int y)
        {
            return _box[y, x];
        }
    }

    public struct Coord
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
