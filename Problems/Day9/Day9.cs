using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Problems.Day9
{
    public class Day9 : ICodingProblem
    {
        public void Run()
        {
            const int numberOfPlayers = 447;
            const int lastMarble = 71510 * 100;

            var nextMultipleOf23 = 23;
            var playerScores = new long[numberOfPlayers];
            var currentPlayer = 0;
            var circle = new LinkedList<int>(); // O(1) add and remove
            var currentMarble = circle.AddFirst(0);

            LinkedListNode<int> CounterClockWise(LinkedListNode<int> current, int positions)
            {
                for (var n = 0; n < positions; n++)
                {
                    if (current == current.List.First)
                        current = current.List.Last;
                    else
                        current = current.Previous;
                }

                return current;
            }
            
            LinkedListNode<int> ClockWise(LinkedListNode<int> current, int positions)
            {
                for (var n = 0; n < positions; n++)
                {
                    if (current == current.List.Last)
                        current = current.List.First;
                    else
                        current = current.Next;
                }

                return current;
            }
            
            for (var marble = 1; marble <= lastMarble; marble++)
            {
                if (marble == nextMultipleOf23)
                {
                    playerScores[currentPlayer] += marble;
                    var toBeRemoved = CounterClockWise(currentMarble, 7);
                    playerScores[currentPlayer] += toBeRemoved.Value;
                    currentMarble = ClockWise(toBeRemoved, 1);
                    circle.Remove(toBeRemoved);
                    nextMultipleOf23 += 23;
                }
                else
                {
                    var addAfterThis = ClockWise(currentMarble, 1);
                    var newNode = new LinkedListNode<int>(marble);
                    circle.AddAfter(addAfterThis, newNode);
                    currentMarble = newNode;
                }

                currentPlayer = (currentPlayer + 1) % numberOfPlayers;
            }
            
            Console.WriteLine($"Part 1: {playerScores.Max()}");
        }
    }
}