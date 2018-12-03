using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.Problems
{
    class Day2 : ICodingProblem
    {
        public void Run()
        {
            Part1();
            Part2();
        }
        
        private static void Part1()
        {
            var data = File.ReadAllLines("Problems\\Day2\\Day2.data").ToArray();

            var checksum = data.Aggregate(
                (0, 0),
                (current, s) =>
                {
                    var count = new Dictionary<char, int>();
                    foreach (var c in s)
                    {
                        if (!count.ContainsKey(c))
                            count[c] = 0;
                        count[c]++;
                    }

                    var two = false;
                    var three = false;
                    foreach (var value in count.Values)
                    {
                        two |= value == 2;
                        three |= value == 3;
                        if (two && three)
                            break;
                    }

                    return (current.Item1 + (two ? 1 : 0), current.Item2 + (three ? 1 : 0));
                },
                t => t.Item1 * t.Item2);

            Console.WriteLine($"Part 1: {checksum}");
        }

        private static void Part2()
        {
            var data = File.ReadAllLines("Problems\\Day2\\Day2.data").ToArray();

            (int,int) CalculateDistance(string x, string y)
            {
                var compare = x.Zip(y, (c1, c2) => c1 != c2 ? 1 : 0).ToArray();
                var distance = compare.Sum();
                var index = compare.TakeWhile(n => n == 0).Count();
                return (distance,index);
            }
            
            for (var i = 0; i < data.Length; i++)
            {
                for (var j = i + 1; j < data.Length; j++)
                {
                    var (distance,index) = CalculateDistance(data[i], data[j]);
                    if (distance == 1)
                        Console.WriteLine($"Part 2: {data[i].Remove(index,1)}");
                }
            }
        }
    }
}