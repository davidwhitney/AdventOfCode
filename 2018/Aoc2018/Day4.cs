using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Aoc2018
{
    [TestFixture]
    public class Day4
    {
        [Test]
        public void Example()
        {
            var log = @"[1518-11-01 00:00] Guard #10 begins shift
[1518-11-01 00:05] falls asleep
[1518-11-01 00:25] wakes up
[1518-11-01 00:30] falls asleep
[1518-11-01 00:55] wakes up
[1518-11-01 23:58] Guard #99 begins shift
[1518-11-02 00:40] falls asleep
[1518-11-02 00:50] wakes up
[1518-11-03 00:05] Guard #10 begins shift
[1518-11-03 00:24] falls asleep
[1518-11-03 00:29] wakes up
[1518-11-04 00:02] Guard #99 begins shift
[1518-11-04 00:36] falls asleep
[1518-11-04 00:46] wakes up
[1518-11-05 00:03] Guard #99 begins shift
[1518-11-05 00:45] falls asleep
[1518-11-05 00:55] wakes up";

            var lines = log.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            var timetracker = new Timetracker(lines);

            Assert.That(timetracker.LongestSleeper, Is.EqualTo(10));
            Assert.That(timetracker.GenerateHash1(), Is.EqualTo(240));
            Assert.That(timetracker.GenerateHash2(), Is.EqualTo(4455));
        }

        [Test]
        public void Task()
        {
            var lines = File.ReadAllLines("Day4.txt");

            var timetracker = new Timetracker(lines);

            Assert.That(timetracker.LongestSleeper, Is.EqualTo(283));
            Assert.That(timetracker.GenerateHash1(), Is.EqualTo(12169));
            Assert.That(timetracker.GenerateHash2(), Is.EqualTo(16164));
        }

        public class Timetracker : Dictionary<int, GuardDiary>
        {
            public int LongestSleeper => this.OrderBy(x => x.Value.TotalSleepingTime).Last().Key;
            private readonly int? _activeGuard;

            public Timetracker(string[] lines)
            {
                var log = lines.Select(x => new Logline(x)).OrderBy(x => x.Timestamp);

                var sleepingSince = DateTime.MaxValue;
                foreach (var line in log)
                {
                    var guard = Regex.Match(line.Raw, "#([0-9]+)");
                    if (guard.Success)
                    {
                        _activeGuard = int.Parse(guard.Groups[0].Value.Remove(0, 1));
                        if (!ContainsKey(_activeGuard.Value))
                        {
                            Add(_activeGuard.Value, new GuardDiary());
                        }

                        this[_activeGuard.Value].Logs.Add(new ShiftLog());
                    }

                    if (!_activeGuard.HasValue) continue;

                    if (line.Message.Contains("asleep"))
                    {
                        sleepingSince = line.Timestamp;
                    }

                    if (line.Message.Contains("wakes"))
                    {
                        var currentDiary = this[_activeGuard.Value].Logs.Last();
                        for (var x = sleepingSince.Minute; x < line.Timestamp.Minute; x++)
                        {
                            currentDiary.SleepingLog[x] = true;
                        }
                    }
                }
            }

            public int GenerateHash1()
            {
                var longestSleeper = this.OrderBy(x => x.Value.TotalSleepingTime).Last();
                var mostSleepyMinute = longestSleeper.Value.FindMostSleepyMinute();
                return longestSleeper.Key * mostSleepyMinute.Minute;
            }

            public int GenerateHash2()
            {
                var longestSleeper = this.OrderBy(x => x.Value.FindMostSleepyMinute().NumberOfTimesSleptOn).Last();
                var mostSleepyMinute = longestSleeper.Value.FindMostSleepyMinute();
                return longestSleeper.Key * mostSleepyMinute.Minute;
            }
        }

        public class GuardDiary
        {
            public List<ShiftLog> Logs { get; set; } = new List<ShiftLog>();
            public int TotalSleepingTime => Logs.Sum(x => x.SleepingMinutes);

            public MostSleepyMinute FindMostSleepyMinute()
            {
                var sleepyMinute = new MostSleepyMinute();
                foreach (var minute in Enumerable.Range(0, 60))
                {
                    var asleepCount = 0;
                    foreach (var item in Logs)
                    {
                        if (item.SleepingLog[minute])
                        {
                            asleepCount++;
                        }
                    }

                    if (asleepCount > sleepyMinute.NumberOfTimesSleptOn)
                    {
                        sleepyMinute.Minute = minute;
                        sleepyMinute.NumberOfTimesSleptOn = asleepCount;
                    }
                }

                return sleepyMinute;
            }
        }

        public class MostSleepyMinute
        {
            public int Minute { get; set; }
            public int NumberOfTimesSleptOn { get; set; }
        }

        public class ShiftLog
        {
            public Dictionary<int, bool> SleepingLog { get; set; } = new Dictionary<int, bool>();
            public int SleepingMinutes => SleepingLog.Count(x => x.Value);

            public ShiftLog()
            {
                Enumerable.Range(0, 60).ToList().ForEach(x => SleepingLog.Add(x, false));
            }
        }

        [Test]
        public void LogLine_Ctor()
        {
            var line = new Logline("[1518-11-01 00:01] Guard #10 begins shift");

            Assert.That(line.Timestamp.Minute, Is.EqualTo(1));
        }

        public class Logline
        {
            public string Raw { get; }
            public DateTime Timestamp { get; set; }
            public string Message { get; set; }

            public Logline(string raw)
            {
                Raw = raw;

                var stamp = raw.Substring(1, 16);
                Timestamp = DateTime.Parse(stamp);
                Message = raw.Substring(19);
            }
        }
    }
}
