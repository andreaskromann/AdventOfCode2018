using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using static System.Math;

namespace AdventOfCode2018.Problems.Day10
{
    public class Day10 : ICodingProblem
    {
        class Point
        {
            public Vector2 Position { get; private set; }
            public Vector2 Velocity { get; }

            public Point(Vector2 position, Vector2 velocity)
            {
                Position = position;
                Velocity = velocity;
            }

            public void Move()
            {
                Position = Position + Velocity;
            }
        }
        
        public void Run()
        {
            Point ParsePoint(string l)
            {
                var matches = Regex.Match(
                    l,
                    @"position=<\s*(?<px>-?\d+),\s*(?<py>-?\d+)>\s*velocity=<\s*(?<vx>-?\d+),\s*(?<vy>-?\d+)>",
                    RegexOptions.Compiled);

                var px = matches.Groups["px"].Value;
                var py = matches.Groups["py"].Value;
                var p = new Vector2(float.Parse(px),float.Parse(py));
                
                var vx = matches.Groups["vx"].Value;
                var vy = matches.Groups["vy"].Value;
                var v = new Vector2(float.Parse(vx), float.Parse(vy));
                
                return new Point(p, v);
            }
            
            var data = File.ReadAllLines("Problems\\Day10\\Day10.data")
                .Select(ParsePoint)
                .ToArray();

            var seconds = 0;
            while (true)
            {
                seconds++;
                
                var minx = float.MaxValue;
                var maxx = float.MinValue;
                var miny = float.MaxValue;
                var maxy = float.MinValue;
                foreach (var point in data)
                {
                    point.Move();
                    minx = Min(minx, point.Position.X);
                    maxx = Max(maxx, point.Position.X);
                    miny = Min(miny, point.Position.Y);
                    maxy = Max(maxy, point.Position.Y);
                }

                // Assume it doesn't get interesting until they are close together
                if (maxy - miny > 25)
                    continue;
                
                var currentFormation = new HashSet<Vector2>();
                foreach (var point in data)
                    currentFormation.Add(point.Position);
                
                for (var y = miny; y <= maxy; y++)
                {
                    for (var x = minx; x <= maxx; x++)
                    {
                        var c = currentFormation.Contains(new Vector2(x, y)) ? '#' : '.';
                        Console.Write(c);
                    }
                    Console.WriteLine();
                }
                
                Console.WriteLine($"Seconds: {seconds}");
                Console.ReadLine();
            }
        }
    }
}