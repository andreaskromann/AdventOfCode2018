using System;
using System.IO;
using static System.Math;

namespace AdventOfCode2018.Problems.Day5
{
    class Day5 : ICodingProblem
    {
        public void Run()
        {
            var data = File.ReadAllLines("Problems\\Day5\\Day5.data")[0];

            string React(string polymer)
            {
                for (var i = 0; i < polymer.Length-1;)
                {
                    if (Abs(polymer[i] - polymer[i + 1]) == 32) // |'a' - 'A'| == 32
                    {
                        polymer = polymer.Remove(i, 2);
                        i = Max(0, i - 1);
                    }
                    else
                        i++;
                }

                return polymer;
            }

            var result = React(data);
            Console.WriteLine($"Part 1: {result.Length}");

            var min = int.MaxValue;
            for (var c = 65; c <= 90; c++)
            {
                var temp = data.Replace(((char) c).ToString(), "", StringComparison.OrdinalIgnoreCase);
                temp = React(temp);
                if (temp.Length < min)
                    min = temp.Length;
            }
            Console.WriteLine($"Part 2: {min}");
        }
    }
}