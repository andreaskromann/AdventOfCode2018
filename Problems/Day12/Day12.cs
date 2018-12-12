using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2018.Problems.Day12
{
    public class Day12 : ICodingProblem
    {
        class Pot
        {
            public int Index { get; }
            public bool[] HasPlant { get; }
            
            public Pot(int index, bool hasPlant)
            {
                Index = index;
                HasPlant = new bool[2] { hasPlant, hasPlant };
            }
        }
        
        public void Run()
        {
            var data = File.ReadAllLines("Problems\\Day12\\Day12.data");
            var initialLine = data[0];
            var patternLines = data.Skip(2);

            var pots = new LinkedList<Pot>();
            var firstPot = initialLine.IndexOf(':') + 2;
            for (var i = firstPot; i < initialLine.Length; i++)
            {
                var hasPlant = initialLine[i] == '#';
                var node = new LinkedListNode<Pot>(new Pot(i-firstPot, hasPlant));
                pots.AddLast(node);
            }

            var patterns = patternLines
                .Where(l => l[2] != l[9]) // Filter out those that don't do anything
                .ToDictionary(
                    l => (l[0] == '#', l[1] == '#', l[2] == '#', l[3] == '#', l[4] == '#'),
                    l => l[9] == '#');
            
            var generation = 0;
            
            // Keep just 4 empty spaces on each side
            void PadPots()
            {
                const int emptyPlants = 4;
                
                var emptyLeft = 0;
                var current = pots.First;
                while (current != null && !current.Value.HasPlant[generation])
                {
                    emptyLeft++;
                    current = current.Next;
                }

                if (emptyLeft < emptyPlants)
                {
                    for (var i = 0; i < emptyPlants - emptyLeft; i++)
                    {
                        var firstIndex = pots.First.Value.Index;
                        pots.AddFirst(new LinkedListNode<Pot>(new Pot(firstIndex - 1, false)));
                    }
                }
                else if (emptyLeft > emptyPlants)
                {
                    for (var i = 0; i < emptyLeft - emptyPlants; i++)
                        pots.RemoveFirst();
                }

                var emptyRight = 0;
                current = pots.Last;
                while (current != null && !current.Value.HasPlant[generation])
                {
                    emptyRight++;
                    current = current.Previous;
                }

                if (emptyRight < emptyPlants)
                {
                    for (var i = 0; i < emptyPlants - emptyRight; i++)
                    {
                        var lastIndex = pots.Last.Value.Index;
                        pots.AddLast(new LinkedListNode<Pot>(new Pot(lastIndex + 1, false)));
                    }
                }
                else if (emptyRight > emptyPlants)
                {
                    for (var i = 0; i < emptyRight - emptyPlants; i++)
                        pots.RemoveLast();
                }
            }

            void Iterate()
            {
                PadPots();

                var current = pots.First.Next.Next;
                var nextGeneration = generation == 0 ? 1 : 0;

                while (current?.Next?.Next != null)
                {
                    var pattern = (
                        current.Previous.Previous.Value.HasPlant[generation],
                        current.Previous.Value.HasPlant[generation],
                        current.Value.HasPlant[generation],
                        current.Next.Value.HasPlant[generation],
                        current.Next.Next.Value.HasPlant[generation]);

                    if (patterns.TryGetValue(pattern, out var newValue))
                        current.Value.HasPlant[nextGeneration] = newValue;
                    else
                        current.Value.HasPlant[nextGeneration] = current.Value.HasPlant[generation];

                    current = current.Next;
                }

                generation = nextGeneration;
            }

            var results = new Dictionary<string,long>();

            bool CheckResult(long index)
            {
                var lastPlant = pots.Last;
                while (!lastPlant.Value.HasPlant[generation])
                    lastPlant = lastPlant.Previous;
                
                var sb = new StringBuilder(pots.Count);
                foreach (var c in pots.SkipWhile(p => !p.HasPlant[generation])
                    .TakeWhile(p => p != lastPlant.Next?.Value).Select(p => p.HasPlant[generation] ? '#' : '.'))
                    sb.Append(c);
                var newResult = sb.ToString();
                
                if (results.ContainsKey(newResult))
                {
                    Console.WriteLine($"Found cycle at index {index} matching index {results[newResult]}");
                    return true;
                }

                results[newResult] = index;
                return false;
            }

            for (long i = 0; i < 20; i++)
            {
                if (CheckResult(i)) 
                    return;

                Iterate();
            }

            var result1 = pots.Sum(p => p.HasPlant[generation] ? p.Index : 0);
            Console.WriteLine($"Part 1: {result1}");

//            for (long i = 0; i < 50000000000L - 20; i++)
//            {
//                if (CheckResult(i + 20)) 
//                    return;
//                
//                Iterate();
//            }

            // Using the code above I found that after 120 iterations the pattern is the same.
            // It only moves higher and higher up increasing the result by 22 for each iteration.
            // The sum at iteration 120 is 3151.
            Console.WriteLine($"Part 2: {22*(50000000000L-120) + 3151}");
        }
    }
}