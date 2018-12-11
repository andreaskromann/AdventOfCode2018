using System;

namespace AdventOfCode2018.Problems.Day11
{
    public class Day11 : ICodingProblem
    {
        public void Run()
        {
            const int serialNumber = 1309;

            var grid = new int[300, 300];
            for (var x = 0; x < 300; x++)
            {
                for (var y = 0; y < 300; y++)
                {
                    var number = ((x + 1 + 10) * ((x + 1 + 10) * (y + 1) + serialNumber));
                    var formatted = number.ToString("000");
                    var digit = formatted.Substring(formatted.Length - 3, 1);
                    grid[x, y] = int.Parse(digit) - 5;
                }
            }

            var max = (Level: int.MinValue, X: -1, Y: -1);
            for (var x = 0; x < 300 - 2; x++)
            {
                for (var y = 0; y < 300 - 2; y++)
                {
                    var square = 0;
                    for (var i = 0; i < 3; i++)
                    {
                        for (var j = 0; j < 3; j++)
                        {
                            square += grid[x + i, y + j];
                        }
                    }

                    if (square > max.Level)
                        max = (square, x+1, y+1);
                }
            }
            
            Console.WriteLine($"Part 1: {max}");

            int SumSquare(int x, int y, int size, int lastSum)
            {
                var sum = lastSum;
                
                // Add bottom row of new square
                for (var i = 0; i < size; i++)
                    sum += grid[x + i, y + size - 1];

                // Add rightmost column of new square except bottom field
                for (var j = 0; j < size - 1; j++)
                    sum += grid[x + size - 1, y + j];

                return sum;
            }
            
            var max2 = (Level: int.MinValue, X: -1, Y: -1, Size: -1);
            for (var x = 0; x < 300; x++)
            {
                for (var y = 0; y < 300; y++)
                {
                    var square = grid[x,y];
                    
                    if (square > max2.Level)
                        max2 = (square, x+1, y+1, 1);

                    for (var i = 2; y + i - 1 < 300 && x + i - 1 < 300; i++)
                    {
                        square = SumSquare(x, y, i, square);
                        if (square > max2.Level)
                            max2 = (square, x+1, y+1, i);
                    }
                }
            }
            
            Console.WriteLine($"Part 2: {max2}");
        }
    }
}