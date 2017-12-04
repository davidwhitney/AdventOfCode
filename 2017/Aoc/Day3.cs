using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Aoc
{
    [TestFixture]
    public class Day3
    {
        [TestCase(1, 0)]
        [TestCase(12, 3)]
        [TestCase(277678, 475)]
        public void Part1(int start, int expectedSteps)
        {
            var input = start;

            var grid = new Grid(input);
            var distance = grid.DistanceBetween(start, 1);

            Assert.That(distance, Is.EqualTo(expectedSteps));
        }

        public class Grid
        {
            private readonly int?[,] _array;

            public Grid(int upTo)
            {
                var targetBoxSize = GetBoundingBoxSize(upTo);
                _array = GenerateGrid(targetBoxSize);
            }

            public int DistanceBetween(int start, int target)
            {
                var startL = Find(start);
                var endL = Find(target);

                return Math.Abs(startL.X - endL.X) + Math.Abs(startL.Y - endL.Y);
            }

            private Location Find(int value)
            {
                var location = new Location();
                for (var y = 0; y < _array.GetLength(0); y++)
                for (var x = 0; x < _array.GetLength(1); x++)
                {
                    if (_array[y, x] == value)
                    {
                        return new Location {X = x, Y = y};
                    }
                }

                throw new Exception("Not found");
            }

            private class Location { public int X { get; set; } public int Y { get; set; } }

            private static int?[,] GenerateGrid(int targetBoxSize)
            {
                var dimension = targetBoxSize;
                var array = new int?[dimension, dimension];
                var x = dimension - 1;
                var y = dimension - 1;
                var remaining = dimension * dimension;

                var currentDirection = "W";

                while (remaining > 0)
                {
                    array[y, x] = remaining;

                    var nextX = x;
                    var nextY = y;
                    MoveNext(currentDirection, ref nextX, ref nextY);

                    try
                    {
                        var nextCell = array[nextY, nextX];
                        if (nextCell != null) throw new Exception("Ha");
                    }
                    catch (Exception ex)
                    {
                        currentDirection = ChangeDirection(currentDirection);

                        nextX = x;
                        nextY = y;
                        MoveNext(currentDirection, ref nextX, ref nextY);
                    }

                    x = nextX;
                    y = nextY;
                    remaining--;
                }
                return array;
            }

            private static int GetBoundingBoxSize(int input)
            {
                var targetBoxSize = Math.Sqrt(input);

                if (targetBoxSize % 1 != 0)
                {
                    var nextNumber = (int)targetBoxSize + 1;
                    targetBoxSize = nextNumber;
                }
                return (int)targetBoxSize;
            }

            private static string ChangeDirection(string currentDirection)
            {
                const string walkDirections = "WNES";
                var nextDirection = walkDirections.IndexOf(currentDirection) + 1;
                nextDirection = nextDirection == walkDirections.Length ? 0 : nextDirection;
                return walkDirections[nextDirection].ToString();
            }

            private static void MoveNext(string currentDirection, ref int nextX, ref int nextY)
            {
                if (currentDirection == "E") nextX++;
                if (currentDirection == "N") nextY--;
                if (currentDirection == "W") nextX--;
                if (currentDirection == "S") nextY++;
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                for (var index0 = 0; index0 < _array.GetLength(0); index0++)
                {
                    for (var index1 = 0; index1 < _array.GetLength(1); index1++)
                    {
                        sb.Append(_array[index0, index1] + "  ");
                    }
                    sb.Append(Environment.NewLine);
                }

                return sb.ToString();
            }
        }
    }
}
