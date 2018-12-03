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
            Square ParseSquare(string s)
            {
                var match = Regex.Match(s, @"#(?<id>\d+) @ (?<x>\d+),(?<y>\d+): (?<w>\d+)x(?<h>\d+)", RegexOptions.Compiled);
                var x = int.Parse(match.Groups["x"].Value);
                var y = int.Parse(match.Groups["y"].Value);
                var w = int.Parse(match.Groups["w"].Value);
                var h = int.Parse(match.Groups["h"].Value);
                var topLeft = new Point(x, y + h - 1);
                var bottomRight = new Point(x + w - 1, y);
                return new Square(match.Groups["id"].Value, topLeft, bottomRight);
            }
            
            var data = File.ReadAllLines("Problems\\Day3\\Day3.data").Select(ParseSquare).ToArray();
            
            var maxX = data.Max(s => s.BottomRight.X);
            var maxY = data.Max(s => s.TopLeft.Y);

            // This is really slow (~10 sec) but gives the correct result.
            // I have a good idea how to make it much faster, but have to sleep now...
            var seen = new HashSet<string>();
            var result = 0;
            for (var x = 0; x <= maxX; x++)
            {
                for (var y = 0; y < maxY; y++)
                {
                    var hits = data.Where(s => s.TopLeft.Y >= y && s.BottomRight.Y <= y && s.TopLeft.X <= x && s.BottomRight.X >= x).ToArray();
                    if (hits.Length >= 2)
                    {
                        foreach (var square in hits)
                            seen.Add(square.Id);
                        result++;
                    }
                }
            }
            
            Console.WriteLine($"Part 1: {result}");
            Console.WriteLine($"Part 2: {data.Single(s => !seen.Contains(s.Id)).Id}");
        }
    }
}