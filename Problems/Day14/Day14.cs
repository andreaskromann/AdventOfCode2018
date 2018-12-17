using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Problems.Day14
{
    public class Day14 : ICodingProblem
    {
        public void Run()
        {
            const int input = 640441;

            Part1(input);
            Part2(input);
        }

        private static void Part1(int input)
        {
            var recipes = new List<int>(input + 10) {3, 7};
            var elf1 = 0;
            var elf2 = 1;

            while (recipes.Count < input + 10)
                (elf1,elf2) = Iterate(recipes, elf1, elf2);

            Console.Write("Part 1: ");
            foreach (var d in recipes.Skip(input).Take(10).Select(r => r.ToString()))
                Console.Write(d);
            Console.WriteLine();
        }

        private void Part2(int input)
        {
            var recipes = new List<int>(input + 10) {3, 7};
            var elf1 = 0;
            var elf2 = 1;
            var pattern = input.ToString().Select(c => int.Parse(c.ToString())).ToArray();
            var index = 0;
            
            while (true)
            {
                if (recipes.Count < pattern.Length + index)
                {
                    (elf1, elf2) = Iterate(recipes, elf1, elf2);
                    continue;
                }

                var match = true;
                for (var i = 0; i < pattern.Length; i++)
                {
                    if (recipes[index + i] != pattern[i])
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    Console.WriteLine($"Part 2: {index}");
                    return;
                }

                index++;
            }
        }
        
        private static (int,int) Iterate(List<int> recipes, int elf1, int elf2)
        {
            var combined = (recipes[elf1] + recipes[elf2]).ToString();
            foreach (var d in combined)
                recipes.Add(int.Parse(d.ToString()));
            elf1 = (elf1 + 1 + recipes[elf1]) % recipes.Count;
            elf2 = (elf2 + 1 + recipes[elf2]) % recipes.Count;
            return (elf1, elf2);
        }
    }
}