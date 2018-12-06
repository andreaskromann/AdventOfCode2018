using System;
using System.IO;
using System.Linq;
using static System.Math;

namespace AdventOfCode2018.Problems.Day5
{
    class Day5 : ICodingProblem
    {
        public void Run()
        {
            var data = File.ReadAllLines("Problems\\Day5\\Day5.data")[0];

            int React(string polymer, int? remove = null)
            {
                var indexes = Enumerable.Range(0, polymer.Length).ToList();
                
                for (var i = 0; i < indexes.Count-1;)
                {
                    if (remove.HasValue && (remove == polymer[indexes[i]] || (remove + 32) == polymer[indexes[i]]))
                    {
                        indexes.RemoveAt(i);
                        i = Max(0, i - 1);
                    }
                    else if (Abs(polymer[indexes[i]] - polymer[indexes[i + 1]]) == 32) // |'a' - 'A'| == 32
                    {
                        indexes.RemoveRange(i, 2);
                        i = Max(0, i - 1);
                    }
                    else
                        i++;
                }

                return indexes.Count;
            }

            var result = React(data);
            Console.WriteLine($"Part 1: {result}");

            var min = int.MaxValue;
            for (var c = 65; c <= 90; c++)
            {
                var temp = React(data, c);
                if (temp < min)
                    min = temp;
            }
            Console.WriteLine($"Part 2: {min}");
        }
    }
}