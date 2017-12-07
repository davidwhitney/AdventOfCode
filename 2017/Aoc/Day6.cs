using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Aoc
{
    [TestFixture]
    public class Day6
    {
        [Test]
        public void Sample()
        {
            var memory = new MemoryBanks("0\t2\t7\t0");

            var cycles = memory.ReallocateUntilRepeated();

            Assert.That(cycles, Is.EqualTo(5));
            Assert.That(memory.RepeatedAt, Is.EqualTo(4));
        }

        [Test]
        public void Part1()
        {
            var memory = new MemoryBanks("0\t5\t10\t0\t11\t14\t13\t4\t11\t8\t8\t7\t1\t4\t12\t11");

            var cycles = memory.ReallocateUntilRepeated();

            Assert.That(cycles, Is.EqualTo(7864));
            Assert.That(memory.RepeatedAt, Is.EqualTo(1695));
        }

    }

    public class MemoryBanks : List<MemoryBank>
    {
        public int RepeatedAt { get; set; }
        private readonly List<string> _previouslySeenBlockConfigurations = new List<string>();


        public MemoryBanks(string blockMap)
        {
            var splitBlockCounts = blockMap.Split('\t').Select(int.Parse).ToList();
            for (var index = 0; index < splitBlockCounts.Count; index++)
            {
                Add(new MemoryBank(index) {BlockCount = splitBlockCounts[index]});
            }

            _previouslySeenBlockConfigurations.Add(Hash());
        }

        public int ReallocateUntilRepeated()
        {
            var cycles = 0;
            while (true)
            {
                var hash = Reallocate();
                cycles++;

                if (_previouslySeenBlockConfigurations.Contains(hash))
                {
                    RepeatedAt = _previouslySeenBlockConfigurations.Count -
                                 _previouslySeenBlockConfigurations.IndexOf(hash);
                    break;
                }

                _previouslySeenBlockConfigurations.Add(hash);
                
            }

            return cycles;
        }

        public string Reallocate()
        {
            var mostFullItemWithLowestId =
                this.GroupBy(x => x.BlockCount)
                    .OrderBy(g => g.Key).Last()
                    .OrderBy(g => g.Id).First();
            
            var itemsToReallocate = mostFullItemWithLowestId.BlockCount;
            mostFullItemWithLowestId.BlockCount = 0;

            var nextBankToAllocateTo = IndexOf(mostFullItemWithLowestId) + 1;
            nextBankToAllocateTo = nextBankToAllocateTo == Count ? 0 : nextBankToAllocateTo;

            while (itemsToReallocate > 0)
            {
                this[nextBankToAllocateTo].BlockCount += 1;
                itemsToReallocate--;
                nextBankToAllocateTo++;
                nextBankToAllocateTo = nextBankToAllocateTo == Count ? 0 : nextBankToAllocateTo;
            }

            return Hash();
        }
        
        private string Hash() => string.Join(" ", this.Select(x => x.Hash()));
    }

    public class MemoryBank
    {
        public int Id { get; set; }
        public int BlockCount { get; set; }
        public MemoryBank(int id) => Id = id;
        public string Hash() => $"{BlockCount}";
    }

}
