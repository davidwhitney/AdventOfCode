using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

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
            //Console.WriteLine(grid.ToString());
        }
        
        [Test]
        public void Part2()
        {
            var grid = new Grid(277678, true);
            
            Console.WriteLine(grid.NextBiggest);
        }

        public class Grid
        {
            private readonly int _upTo;
            private readonly bool _part2;
            private int?[,] _array;
            public int NextBiggest { get; set; }

            public Grid(int upTo, bool part2 = false)
            {
                _upTo = upTo;
                _part2 = part2;

                var targetBoxSize = _part2 ? GetBoundingBoxSize(upTo * 2) : GetBoundingBoxSize(upTo);

                GenerateGrid(targetBoxSize);
            }

            public int DistanceBetween(int start, int target)
            {
                var startL = Find(start);
                var endL = Find(target);

                return Math.Abs(startL.X - endL.X) + Math.Abs(startL.Y - endL.Y);
            }

            private Location Find(int value)
            {
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

            private void GenerateGrid(int targetBoxSize)
            {
                var dimension = targetBoxSize;
                _array = new int?[dimension + 1, dimension + 1];
                var location = new Location {X = dimension / 2, Y = dimension / 2};
                var remaining = dimension * dimension;
                var count = 1;

                var currentDirection = "E";

                while (remaining > 0)
                {
                    var value = GetValue(count, location);

                    if (_part2 && value > _upTo)
                    {
                        NextBiggest = value;
                        break;
                    }

                    _array[location.Y, location.X] = value;

                    Location next;

                    if (count == 1)
                    {
                        next = NextLocation(currentDirection, location);
                    }
                    else
                    {
                        var turnAttempt = ChangeDirection(currentDirection);
                        var previewnext = NextLocation(turnAttempt, location);
                        if (_array[previewnext.Y, previewnext.X] == null)
                        {
                            currentDirection = ChangeDirection(currentDirection);
                            next = previewnext;
                        }
                        else
                        {
                            next = NextLocation(currentDirection, location);
                        }
                    }

                    location = next;
                    count++;
                    remaining--;
                }
            }

            private int GetValue(int count, Location current)
            {
                if (!_part2) return count;

                var neighbours = new List<Location>
                {
                    new Location {X = current.X - 1, Y = current.Y - 1},
                    new Location {X = current.X, Y = current.Y - 1},
                    new Location {X = current.X + 1, Y = current.Y - 1},
                    new Location {X = current.X - 1, Y = current.Y},
                    new Location {X = current.X + 1, Y = current.Y},
                    new Location {X = current.X - 1, Y = current.Y + 1},
                    new Location {X = current.X, Y = current.Y + 1},
                    new Location {X = current.X + 1, Y = current.Y + 1},
                };

                neighbours.RemoveAll(n => n.X > _array.Length);
                neighbours.RemoveAll(n => n.Y > _array.Length);

                var val = neighbours.Sum(neighbour => _array[neighbour.Y, neighbour.X].GetValueOrDefault(0));

                return val == 0 ? 1 : val;
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
                const string walkDirections = "ENWS";
                var nextDirection = walkDirections.IndexOf(currentDirection) + 1;
                nextDirection = nextDirection == walkDirections.Length ? 0 : nextDirection;
                return walkDirections[nextDirection].ToString();
            }

            private static Location NextLocation(string currentDirection, Location current)
            {
                var location = new Location {X = current.X, Y = current.Y };

                if (currentDirection == "E") location.X++;
                if (currentDirection == "N") location.Y--;
                if (currentDirection == "W") location.X--;
                if (currentDirection == "S") location.Y++;

                return location;
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
