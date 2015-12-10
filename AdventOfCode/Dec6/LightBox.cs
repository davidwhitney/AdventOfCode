using System.Text.RegularExpressions;

namespace AdventOfCode.Dec6
{
    public class LightBox
    {
        private readonly bool[,] _box;
        private readonly int[,] _brightness;
        public int TotalLightsOn { get; private set; }
        public int BrightnessTracker { get; private set; }

        public LightBox()
        {
            _box = new bool[1000, 1000];
            _brightness = new int[1000, 1000];
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

                    TrackLumens(x,y, turnOn);

                    if (currentState != desiredState)
                    {
                        RecordState(y, x);
                    }
                }
            }
        }

        private void TrackLumens(int x, int y, bool? turnOn)
        {
            if (!turnOn.HasValue)
            {
                _brightness[y, x] += 2;
                BrightnessTracker += 2;
                return;
            }

            if (turnOn.Value)
            {
                _brightness[y, x]++;
                BrightnessTracker++;
                return;
            }

            if (_brightness[y, x] > 0)
            {
                _brightness[y, x]--;
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
}