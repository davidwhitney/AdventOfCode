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
            Assert.That(_sut.BrightnessTracker, Is.EqualTo(1));
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
            
            Assert.That(_sut.BrightnessTracker, Is.EqualTo(9));
        }

        [Test]
        public void Toggle_OneLight_AddsTwoToBrightness()
        {
            _sut.Toggle(new Coord(0, 0), new Coord(0, 0));
            
            Assert.That(_sut.BrightnessTracker, Is.EqualTo(2));
        }

        [Test]
        public void Toggle_SmallGrid_WorksOutBrightness()
        {
            _sut.Toggle(new Coord(1, 1), new Coord(1, 5), true);
            
            Assert.That(_sut.BrightnessTracker, Is.EqualTo(5));
        }

        [Test]
        public void Toggle_OneLightOff_StopsAtZero()
        {
            _sut.Toggle(new Coord(0, 0), new Coord(0, 0), false);
            
            Assert.That(_sut.BrightnessTracker, Is.EqualTo(0));
        }

        [Test]
        public void Toggle_OneOnLightOff_StopsAtZero()
        {
            _sut.Toggle(new Coord(0, 0), new Coord(0, 0)); // Brightness 2
            _sut.Toggle(new Coord(0, 0), new Coord(0, 0), false); // -1
            
            Assert.That(_sut.BrightnessTracker, Is.EqualTo(1));
        }

        [Test]
        public void Toggle_OneOnLightOffTwice_StopsAtZero()
        {
            _sut.Toggle(new Coord(0, 0), new Coord(0, 0)); // Brightness 2
            _sut.Toggle(new Coord(0, 0), new Coord(0, 0), false); // -1
            _sut.Toggle(new Coord(0, 0), new Coord(0, 0), false); // -1
            
            Assert.That(_sut.BrightnessTracker, Is.EqualTo(0));
        }

        [Test]
        public void Toggle_All_AddsTwoToBrightness()
        {
            _sut.Toggle(new Coord(0, 0), new Coord(999, 999));
            
            Assert.That(_sut.BrightnessTracker, Is.EqualTo(2000000));
        }

        [Test]
        public void Toggle_AllWithTurnOn_AddsTwoToBrightness()
        {
            _sut.Toggle(new Coord(0, 0), new Coord(999, 999), true);
            
            Assert.That(_sut.BrightnessTracker, Is.EqualTo(1000000));
        }

        [Test]
        public void Toggle_NaturalLanguage_ParsesCorrectly()
        {
            var instruction = "turn on 0,0 through 0,0";

            _sut.Toggle(instruction);

            Assert.That(_sut.StatusAt(0, 0), Is.EqualTo(true));
            Assert.That(_sut.BrightnessTracker, Is.EqualTo(1));
        }

        [Ignore("Slow!")]
        [Test]
        public void Toggle_DoChallange()
        {
            var contents = File.ReadLines("c:\\dev\\AdventOfCode\\AdventOfCode\\Dec6\\Test.txt").ToList();

            contents.ForEach(x => _sut.Toggle(x));

            Assert.That(_sut.TotalLightsOn, Is.EqualTo(569999));
            Assert.That(_sut.BrightnessTracker, Is.EqualTo(1));
        }

    }

    public class LightBox
    {
        private readonly bool[,] _box;
        public int TotalLightsOn { get; private set; }
        public int BrightnessTracker { get; private set; }

        public LightBox()
        {
            _box = new bool[1000, 1000];
        }

        public void Toggle(string instruction)
        {
            var results = Regex.Match(instruction.Trim(), "(.*) ([0-9]+),([0-9]+) through ([0-9]+),([0-9]+)");

            var bottomLeft = new Coord(int.Parse(results.Groups[2].Value), int.Parse(results.Groups[3].Value));
            var topRight = new Coord(int.Parse(results.Groups[4].Value), int.Parse(results.Groups[5].Value));
            bool? turnOn = results.Groups[1].Value == "turn on";

            if (results.Groups[1].Value == "toggle")
            {
                turnOn = null;
            }

            Toggle(bottomLeft, topRight, turnOn);

        }

        public void Toggle(Coord bottomLeft, Coord topRight, bool? turnOn = null)
        {
            for (int x = bottomLeft.X; x <= topRight.X; x++)
            {
                for (int y = bottomLeft.Y; y <= topRight.Y; y++)
                {
                    var currentState = _box[y, x];
                    var desiredState = turnOn ?? !_box[y, x];
                    _box[y, x] = desiredState;

                    TrackLumens(turnOn);

                    if (currentState != desiredState)
                    {
                        RecordState(y, x);
                    }
                }
            }
        }

        private void TrackLumens(bool? turnOn)
        {
            if (!turnOn.HasValue)
            {
                BrightnessTracker += 2;
                return;
            }

            if (turnOn.Value)
            {
                BrightnessTracker++;
                return;
            }

            if (BrightnessTracker > 0)
            {
                BrightnessTracker--;
            }
        }

        private void RecordState(int y, int x)
        {
            if (_box[y, x])
            {
                TotalLightsOn++;
            }
            else
            {
                TotalLightsOn--;
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
