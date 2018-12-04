using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.Problems
{
    class Day1 : ICodingProblem
    {
        public void Run()
        {
            var data = File.ReadAllLines("Problems\\Day1\\Day1.data").Select(int.Parse).ToArray();
            
            var part1 = data.Sum();
            Console.WriteLine($"Part 1: {part1}");

            var frequency = 0;
            var seen = new HashSet<int>{0};
            for (var i = 0;;i++)
            {
                frequency += data[i % data.Length];
                
                if (seen.Contains(frequency))
                {
                    Console.WriteLine($"Part 2: {frequency}");
                    return;
                }

                seen.Add(frequency);
            }
        }
    }
}