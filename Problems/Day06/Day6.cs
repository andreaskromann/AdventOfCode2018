using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Math;

namespace AdventOfCode2018.Problems.Day6
{
    public class Day6 : ICodingProblem
    {
        struct Point
        {
            public readonly int X;
            public readonly int Y;
            public readonly int Index;

            public Point(int i, int x, int y)
            {
                X = x;
                Y = y;
                Index = i;
            }
        }
        
        public void Run()
        {
            var data = File.ReadAllLines("Problems\\Day06\\Day6.data")
                .Select(l => l.Split(','))
                .Select((a,i) => new Point(i, int.Parse(a[0].Trim()), int.Parse(a[1].Trim())))
                .ToArray();

            // Box around the input points
            var lowerLeft = (X: int.MaxValue, Y: int.MaxValue);
            var upperRight = (X: int.MinValue, Y: int.MinValue);
            foreach (var point in data)
            {
                if (lowerLeft.X > point.X)
                    lowerLeft.X = point.X;
                if (lowerLeft.Y > point.Y)
                    lowerLeft.Y = point.Y;
                if (upperRight.X < point.X)
                    upperRight.X = point.X;
                if (upperRight.Y < point.Y)
                    upperRight.Y = point.Y;
            }

            int CalcDistance(Point p1, Point p2)
            {
                return Abs(p1.X - p2.X) + Abs(p1.Y - p2.Y);
            }

            var counts = new Dictionary<int, int>();
            var infinite = new HashSet<int>();
            var regionSize = 0;
            for (var x = lowerLeft.X; x <= upperRight.X; x++)
            {
                for (var y = lowerLeft.Y; y <= upperRight.Y; y++)
                {
                    var current = new Point(-1, x, y);
                    var distances = data.Select(p => (p.Index, CalcDistance(current, p)))
                        .OrderBy(r => r.Item2)
                        .ToArray();
                    
                    if (distances.Sum(d => d.Item2) < 10000)
                        regionSize++;
                    
                    if (distances[0].Item2 == distances[1].Item2)
                        continue;
                    
                    var minIndex = distances[0].Item1;
                    if (!counts.ContainsKey(minIndex))
                        counts[minIndex] = 0;
                    counts[minIndex] += 1;

                    if (x == lowerLeft.X || x == upperRight.X || y == lowerLeft.Y || y == upperRight.Y)
                        infinite.Add(minIndex);
                }
            }

            var part1 = counts.Where(kv => !infinite.Contains(kv.Key)).Max(kv => kv.Value);
            Console.WriteLine($"Part 1: {part1}");
            
            Console.WriteLine($"Part 2: {regionSize}");
        }
    }
}