using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Aoc
{
    [TestFixture]
    public class Day9
    {
        [Test]
        public void Samples()
        {
            Assert.That(Score("{}"), Is.EqualTo(1));
            Assert.That(Score("{{{}}}"), Is.EqualTo(6));
            Assert.That(Score("{{},{}}"), Is.EqualTo(5));
            Assert.That(Score("{{{},{},{{}}}}"), Is.EqualTo(16));
            Assert.That(Score("{<a>,<a>,<a>,<a>}"), Is.EqualTo(1));
            Assert.That(Score("{{<ab>},{<ab>},{<ab>},{<ab>}}"), Is.EqualTo(9));
            Assert.That(Score("{{<!!>},{<!!>},{<!!>},{<!!>}}"), Is.EqualTo(9));
        }

        [Test]
        public void Samples2_Erasure()
        {
            Assert.That(Score("{{<a!>},{<a!>},{<a!>},{<ab>}}"), Is.EqualTo(3));
        }

        [Test]
        public void Samples3_Garbage()
        {
            Assert.That(Parse("<>").GarbageCount, Is.EqualTo(0));
            Assert.That(Parse("<random characters>").GarbageCount, Is.EqualTo(17));
            Assert.That(Parse("<<<<>").GarbageCount, Is.EqualTo(3));
            Assert.That(Parse("<{!>}>").GarbageCount, Is.EqualTo(2));
            Assert.That(Parse("<!!>").GarbageCount, Is.EqualTo(0));
            Assert.That(Parse("<!!!>>").GarbageCount, Is.EqualTo(0));
            Assert.That(Parse("<{o\"i!a,<{i<a>").GarbageCount, Is.EqualTo(10));
        }

        [Test]
        public void Part1()
        {
            var contents = File.ReadAllText(@"C:\dev\AdventOfCode\2017\Aoc\Day9.txt");

            Assert.That(Score(contents), Is.EqualTo(17537));
            Assert.That(Parse(contents).GarbageCount, Is.EqualTo(8658));
        }

        public int Score(string input)
        {
            var root = Parse(input);
            return root.Inner.Any() ? GetBlocksAsList(root).Sum(x => x.Score) : 1;
        }

        private static List<Block> GetBlocksAsList(Block root, List<Block> currentList = null)
        {
            currentList = currentList ?? new List<Block>();

            currentList.Add(root);
            foreach (var block in root.Inner)
            {
                GetBlocksAsList(block, currentList);
            }

            return currentList;
        }

        public Block Parse(string input)
        {
            var root = input[0] == '{' ? (Block) new Group() : new Garbage();
            var inGarbage = root is Garbage;
            var currentBlock = root;

            var garbage = new StringBuilder();

            var tokens = input.Skip(1).ToArray();
            for (var index = 0; index < tokens.Length; index++)
            {
                var character = tokens[index];

                switch (character)
                {
                    case '\r':
                    case '\n':
                        break;
                    case '{':
                        if (inGarbage)
                        {
                            garbage.Append(character);
                            continue;
                        }
                        currentBlock.Inner.Add(new Group {Parent = (Group) currentBlock});
                        currentBlock = currentBlock.Inner.Last();
                        break;
                    case '}':
                        if (inGarbage)
                        {
                            garbage.Append(character);
                            continue;
                        }
                        currentBlock = currentBlock?.Parent;
                        break;
                    case '<':
                        if (inGarbage)
                        {
                            garbage.Append(character);
                            continue;
                        }
                        inGarbage = true;
                        break;
                    case '>':
                        inGarbage = false;
                        break;
                    case '!':
                        tokens[index + 1] = '\0';
                        break;
                    default:
                        if (character != '\0')
                        {
                            garbage.Append(character);
                        }
                        break;
                }
            }

            root.GarbageCount = garbage.Length;
            return root;
        }

    }

    public abstract class Block
    {
        public Group Parent { get; set; }
        public List<Group> Inner { get; set; } = new List<Group>();
        public abstract int Score { get; }

        public int GarbageCount { get; set; }
    }

    public class Group : Block
    {
        public override int Score => Parent?.Score + 1 ?? 1;
    }

    public class Garbage : Block
    {
        public string Contents { get; set; }
        public override int Score => 0;
    }
}
