using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AoC
{
    [TestFixture]
    public class Day1
    {
        private string _input =
            "L2, L5, L5, R5, L2, L4, R1, R1, L4, R2, R1, L1, L4, R1, L4, L4, R5, R3, R1, L1, R1, L5, L1, R5, L4, R2, L5, L3, L3, R3, L3, R4, R4, L2, L5, R1, R2, L2, L1, R3, R4, L193, R3, L5, R45, L1, R4, R79, L5, L5, R5, R1, L4, R3, R3, L4, R185, L5, L3, L1, R5, L2, R1, R3, R2, L3, L4, L2, R2, L3, L2, L2, L3, L5, R3, R4, L5, R1, R2, L2, R4, R3, L4, L3, L1, R3, R2, R1, R1, L3, R4, L5, R2, R1, R3, L3, L2, L2, R2, R1, R2, R3, L3, L3, R4, L4, R4, R4, R4, L3, L1, L2, R5, R2, R2, R2, L4, L3, L4, R4, L5, L4, R2, L4, L4, R4, R1, R5, L2, L4, L5, L3, L2, L4, L4, R3, L3, L4, R1, L2, R3, L2, R1, R2, R5, L4, L2, L1, L3, R2, R3, L2, L1, L5, L2, L1, R4";

        [Test]
        public void Solve()
        {
            var instructions = _input.Split(',').ToList().Select(s=>s.Trim());

            var compasDirection = 0;
            var coords = new[] {0, 0};
            foreach (var instruction in instructions)
            {
                var direction =  new Dictionary<char, int> {{'L', -1}, {'R', 1}}[instruction[0]];
                compasDirection = compasDirection + direction;
                compasDirection = compasDirection > 3 ? 0 : compasDirection;
                compasDirection = compasDirection < 0 ? 3 : compasDirection;

                var distance = int.Parse(instruction.Trim('L', 'R'));
                distance = compasDirection > 1 ? distance*-1 : distance;
                var index = compasDirection%2 == 0 ? 1 : 0;

                coords[index] += distance;
            }

            var value = Math.Abs(coords[0]) + Math.Abs(coords[1]);
            Console.WriteLine(value);

            Assert.That(value, Is.EqualTo(181));
        }
    }
}
