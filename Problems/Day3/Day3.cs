using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Problems.Day3
{
    class Day3 : ICodingProblem
    {
        struct Point
        {
            public readonly int X;
            public readonly int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        struct Square
        {
            public readonly string Id;
            public readonly Point TopLeft;
            public readonly Point BottomRight;

            public Square(string id, Point topLeft, Point bottomRight)
            {
                Id = id;
                TopLeft = topLeft;
                BottomRight = bottomRight;
            }
        }

        public void Run()
        {
            var counts = new Dictionary<(int,int),int>();
            
            Square ParseSquare(string s)
            {
                var match = Regex.Match(s, @"#(?<id>\d+) @ (?<x>\d+),(?<y>\d+): (?<w>\d+)x(?<h>\d+)", RegexOptions.Compiled);
                var x = int.Parse(match.Groups["x"].Value);
                var y = int.Parse(match.Groups["y"].Value);
                var w = int.Parse(match.Groups["w"].Value);
                var h = int.Parse(match.Groups["h"].Value);
                var topLeft = new Point(x, y + h - 1);
                var bottomRight = new Point(x + w - 1, y);

                for (var i = topLeft.X; i <= bottomRight.X; i++)
                {
                    for (var j = bottomRight.Y; j <= topLeft.Y; j++)
                    {
                        if (!counts.ContainsKey((i, j)))
                            counts[(i, j)] = 0;
                        counts[(i, j)] += 1;
                    }
                }

                return new Square(match.Groups["id"].Value, topLeft, bottomRight);
            }
            
            var data = File.ReadAllLines("Problems\\Day3\\Day3.data").Select(ParseSquare).ToArray();

            var result = counts.Count(kv => kv.Value >= 2); 
            Console.WriteLine($"Part 1: {result}");

            foreach (var square in data)
            {
                var allOnes = true;
                for (var i = square.TopLeft.X; i <= square.BottomRight.X; i++)
                {
                    for (var j = square.BottomRight.Y; j <= square.TopLeft.Y; j++)
                        allOnes &= counts[(i, j)] == 1;
                }

                if (allOnes)
                {
                    Console.WriteLine($"Part 2: {square.Id}");
                    return;
                }
            }
        }
    }
}