using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.Infrastructure;
using NUnit.Framework;

namespace AoC
{
    [TestFixture]
    public class Day10 : Challenge
    {
        private Factory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new Factory();
        }

        [TestCase(5, 2)]
        [TestCase(6, 1)]
        public void Execute_GivenBotInstruction_StoresValue(int value, int botOwner)
        {
            _factory.Execute($"value {value} goes to bot {botOwner}");

            Assert.That(_factory.BotsById[botOwner].Low, Is.EqualTo(value));
        }

        [Test]
        public void Execute_GivenBotInstructionToSameBotTwice_HandlesCorrectly()
        {
            _factory.Execute("value 5 goes to bot 1", "value 5 goes to bot 1");

            Assert.That(_factory.BotsById[1].Low, Is.EqualTo(5));
        }

        [Test]
        public void Execute_GivenBotAHighAndALow_AssignsAccurately()
        {
            _factory.Execute("value 5 goes to bot 1", "value 6 goes to bot 1");

            Assert.That(_factory.BotsById[1].Low, Is.EqualTo(5));
            Assert.That(_factory.BotsById[1].High, Is.EqualTo(6));
        }

        [Test]
        public void Execute_GivenBotAHighAndALowInWrongOrder_AssignsAccurately()
        {
            _factory.Execute("value 6 goes to bot 1", "value 5 goes to bot 1");

            Assert.That(_factory.BotsById[1].Low, Is.EqualTo(5));
            Assert.That(_factory.BotsById[1].High, Is.EqualTo(6));
        }
        
        [Test]
        public void Execute_SecondItemArrives_FollowsAnyRules()
        {
            _factory.Execute("bot 1 gives low to bot 2 and high to bot 3");
            _factory.Execute("value 1 goes to bot 1", "value 2 goes to bot 1");

            Assert.That(_factory.BotsById[2].Low, Is.EqualTo(1));
            Assert.That(_factory.BotsById[3].High, Is.EqualTo(2));
        }

        [Test]
        public void Execute_AssignmentComesBeforeRoutes_ShouldStillWork()
        {
            _factory.Execute(
                "value 1 goes to bot 1",
                "value 2 goes to bot 1",
                "bot 1 gives low to bot 2 and high to bot 3");

            Assert.That(_factory.BotsById[2].Low, Is.EqualTo(1));
            Assert.That(_factory.BotsById[3].High, Is.EqualTo(2));
        }

        [Test]
        public void Execute_SecondItemArrivesCanSendToOutputs_FollowsAnyRules()
        {
            _factory.Execute("bot 1 gives low to output 1 and high to output 2");
            _factory.Execute("value 1 goes to bot 1", "value 2 goes to bot 1");

            Assert.That(_factory.OutputsById[1].Value, Is.EqualTo(1));
            Assert.That(_factory.OutputsById[2].Value, Is.EqualTo(2));
        }
        
        [Test]
        public void Sample()
        {
            _factory.Execute(
                "value 5 goes to bot 2",
                "bot 2 gives low to bot 1 and high to bot 0",
                "value 3 goes to bot 1",
                "bot 1 gives low to output 1 and high to bot 0",
                "bot 0 gives low to output 2 and high to output 0",
                "value 2 goes to bot 2");

            /*In the end, output bin 0 contains a value-5 microchip, 
             * output bin 1 contains a value-2 microchip, and output bin 2 contains a value-3 microchip. 
             * In this configuration, 
             * bot number 2 is responsible for comparing value-5 microchips with value-2 microchips.*/

            Assert.That(_factory.OutputsById[0].Value, Is.EqualTo(5));
            Assert.That(_factory.OutputsById[1].Value, Is.EqualTo(2));
            Assert.That(_factory.OutputsById[2].Value, Is.EqualTo(3));
        }
    }

    public class Factory
    {
        private readonly Dictionary<Regex, Action<MatchCollection>> _commands;
        public Dictionary<int, Bot> BotsById { get; set; } = new Dictionary<int, Bot>();
        public Dictionary<int, OutputBin> OutputsById { get; set; } = new Dictionary<int, OutputBin>();
        public Dictionary<int, Targets> DataRouting { get; set; } = new Dictionary<int, Targets>();

        public Factory()
        {
            _commands = new Dictionary<Regex, Action<MatchCollection>>
            {
                {
                    new Regex("value ([0-9]+) goes to bot ([0-9]+)"),
                    m => AssignValue(int.Parse(m[0].Groups[1].Value), int.Parse(m[0].Groups[2].Value))
                },
                {
                    new Regex("bot ([0-9]+) gives low to (bot|output) ([0-9]+) and high to (bot|output) ([0-9]+)"),
                    m => AddRoute(int.Parse(m[0].Groups[1].Value), int.Parse(m[0].Groups[3].Value),
                            m[0].Groups[2].Value, int.Parse(m[0].Groups[5].Value), m[0].Groups[4].Value)
                }
            };
        }

        public void Execute(params string[] operations)
        {
            var ordered = operations.OrderBy(x => x).ToList();

            foreach (var op in ordered)
            {
                var selectedCommand = _commands.SingleOrDefault(c => c.Key.IsMatch(op));
                var matches = selectedCommand.Key.Matches(op);

                selectedCommand.Value(matches);
            }
        }

        private void AddRoute(int sourceBot, int lowTarget, string lowTargetType, int highTarget, string highTargetType)
            => DataRouting.Add(sourceBot, new Targets(lowTarget, lowTargetType, highTarget, highTargetType));

        private void AssignValue(int val, int botId)
        {
            var bot = GetBot(botId);
            bot.SupplyValue(val, botId);

            var invoke = new Stack<int>();
            if (bot.HasHighAndLow)
            {
                invoke.Push(botId);
            }

            while (invoke.Any())
            {
                var id = invoke.Pop();
                var toProcess = RouteValues(id);
                toProcess.ForEach(i => { if (!invoke.Contains(i)) { invoke.Push(i); } });
            }

        }

        private List<int> RouteValues(int botId)
        {
            var route = DataRouting.SingleOrDefault(x => x.Key == botId);
            if (route.Value == null)
            {
                return new List<int>();
            }

            var bot = GetBot(botId);
            
            var highDest = route.Value.HighType == "output"
                ? (ITakeValues) GetOuput(route.Value.HighTarget)
                : GetBot(route.Value.HighTarget);

            highDest.SupplyValue(bot.High.Value, botId);
            bot.RemoveHigh();

            var lowDest = route.Value.LowType == "output"
                ? (ITakeValues) GetOuput(route.Value.LowTarget)
                : GetBot(route.Value.LowTarget);

            lowDest.SupplyValue(bot.Low.Value, botId);
            bot.RemoveLow();


            var processOthers = new List<int>();
            if (route.Value.HighType == "bot") processOthers.Add(route.Value.HighTarget);
            if (route.Value.LowType == "bot") processOthers.Add(route.Value.LowTarget);
            return processOthers;
        }

        private Bot GetBot(int botId)
        {
            if (!BotsById.ContainsKey(botId))
            {
                BotsById.Add(botId, new Bot());
            }
            return BotsById[botId];
        }

        private OutputBin GetOuput(int outputId)
        {
            if (!OutputsById.ContainsKey(outputId))
            {
                OutputsById.Add(outputId, new OutputBin());
            }
            return OutputsById[outputId];
        }
    }

    public class Targets
    {
        public int LowTarget { get; set; }
        public string LowType { get; set; }
        public int HighTarget { get; set; }
        public string HighType { get; set; }

        public Targets(int lowTarget, string lowType, int highTarget, string highType)
        {
            HighTarget = highTarget;
            HighType = highType;
            LowTarget = lowTarget;
            LowType = lowType;
        }
    }

    public interface ITakeValues
    {
        void SupplyValue(int val, int originBot);
    }
    public class OutputBin : ITakeValues
    {
        public int? Value { get; set; }
        public int? OriginBot { get; set; }

        public void SupplyValue(int val, int originBot)
        {
            Value = val;
            OriginBot = originBot;
        }
    }

    public class Bot : ITakeValues
    {
        private List<int> _values;
        public int? Low => _values.FirstOrDefault();
        public int? High => _values.LastOrDefault();
        public bool HasHighAndLow => _values.Count >= 2;

        public Bot()
        {
            _values = new List<int>();
        }

        public void SupplyValue(int val, int originBot)
        {
            _values.Add(val);
            _values = _values.OrderBy(x => x).ToList();
        }
        
        public void RemoveHigh()
        {
            if (_values.Count > 0)
                _values.RemoveAt(_values.Count - 1);
        }

        public void RemoveLow()
        {
            if (_values.Count > 0)
                _values.RemoveAt(0);
        }

    }
}