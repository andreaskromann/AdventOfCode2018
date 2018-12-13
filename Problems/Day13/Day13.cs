using System;
using System.IO;
using System.Numerics;

namespace AdventOfCode2018.Problems.Day13
{
    public class Day13 : ICodingProblem
    {
        private static readonly Vector2 Left = new Vector2(-1, 0);
        private static readonly Vector2 Right = new Vector2(1, 0);
        private static readonly Vector2 Up = new Vector2(0, -1);
        private static readonly Vector2 Down = new Vector2(0, 1);
        
        enum IntersectionDirection
        {
            Left,
            Straight,
            Right
        }
        
        class Cart
        {
            public Vector2 Velocity { get; private set; }

            private readonly IntersectionDirection[] _directions =
            {
                IntersectionDirection.Left,
                IntersectionDirection.Straight,
                IntersectionDirection.Right
            };
            private int _index;
            private IntersectionDirection NextIntersection => _directions[_index];

            private long _lastUpdated = 0;

            public Cart(Vector2 initialVelocity)
            {
                Velocity = initialVelocity;
            }

            public bool HasMoved(long tick)
            {
                return _lastUpdated >= tick;
            }

            public void Turn(char track, long tick)
            {
                _lastUpdated = tick;
                
                // Set the new velocity based on the new track kind
                switch (track)
                {
                    case '/' when Velocity == Left:
                    case '\\' when Velocity == Right:
                        Velocity = Down;
                        break;
                    case '/' when Velocity == Up:
                    case '\\' when Velocity == Down:
                        Velocity = Right;
                        break;
                    case '/' when Velocity == Down:
                    case '\\' when Velocity == Up:
                        Velocity = Left;
                        break;
                    case '/' when Velocity == Right:
                    case '\\' when Velocity == Left:
                        Velocity = Up;
                        break;
                    case '+':
                        switch (NextIntersection)
                        {
                            case IntersectionDirection.Left when Velocity == Left:
                            case IntersectionDirection.Straight when Velocity == Down:
                            case IntersectionDirection.Right when Velocity == Right:
                                Velocity = Down;
                                break;
                            case IntersectionDirection.Left when Velocity == Right:
                            case IntersectionDirection.Straight when Velocity == Up:
                            case IntersectionDirection.Right when Velocity == Left:
                                Velocity = Up;
                                break;
                            case IntersectionDirection.Left when Velocity == Up:
                            case IntersectionDirection.Straight when Velocity == Left:
                            case IntersectionDirection.Right when Velocity == Down:
                                Velocity = Left;
                                break;
                            case IntersectionDirection.Left when Velocity == Down:
                            case IntersectionDirection.Straight when Velocity == Right:
                            case IntersectionDirection.Right when Velocity == Up:
                                Velocity = Right;
                                break;
                            default:
                                throw new Exception($"Turn failed going {Velocity} into an intersection and next is {NextIntersection}");
                        }
                        _index = (_index + 1) % 3;
                        break;
                    case '|':
                    case '-':
                        break;
                    default:
                        throw new Exception($"Turn failed going {Velocity} into a track with kind '{track}'");
                }
            }

            public override string ToString()
            {
                if (Velocity == Right)
                        return ">";
                if (Velocity == Left)
                        return "<";
                if (Velocity == Up)
                    return "^";
                if (Velocity == Down)
                    return "v";
                throw new NotImplementedException();
            }
        }

        class Track
        {
            public char Kind { get; }
            public Cart Cart { get; set; }

            public Track(char kind, Cart cart = null)
            {
                Kind = kind;
                Cart = cart;
            }

            public override string ToString()
            {
                return Cart?.ToString() ?? Kind.ToString();
            }
        }
        
        public void Run()
        {
            var data = File.ReadAllLines("Problems\\Day13\\Day13.data");
            var width = data[0].Length;
            var height = data.Length;

            var grid = new Track[width, height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var c = data[y][x];
                    var kind = c;
                    Cart cart = null;
                    switch (c)
                    {
                        case '<':
                            cart = new Cart(Left);
                            kind = '-';
                            break;
                        case '>':
                            cart = new Cart(Right);
                            kind = '-';
                            break;
                        case '^':
                            cart = new Cart(Up);
                            kind = '|';
                            break;
                        case 'v':
                            cart = new Cart(Down);
                            kind = '|';
                            break;
                    }
                    grid[x,y] = new Track(kind, cart);
                }
            }

            void PrintGrid()
            {
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        var s = grid[x, y].ToString();
                        var temp = Console.ForegroundColor;
                        if (s == "<" || s == ">" || s == "v" || s == "^")
                            Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(s);
                        Console.ForegroundColor = temp;
                    }

                    Console.WriteLine();
                }
            }

            long tick = -1;
            while (true)
            {
                tick++;
                //PrintGrid();

                var numberOfCarsLeft = 0;
                
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        var current = grid[x, y];
                        
                        // No cart? Nothing to do here.
                        if (current?.Cart == null)
                            continue;

                        var cart = current.Cart;
                        
                        if (cart.HasMoved(tick))
                            continue;
                        
                        numberOfCarsLeft++;
                        
                        // Move the cart
                        var position = new Vector2(x, y);
                        var newPosition = position + cart.Velocity;
                        var newTrack = grid[Convert.ToInt32(newPosition.X), Convert.ToInt32(newPosition.Y)];
                        
                        if (newTrack.Cart != null)
                        {
                            // CRASH!
                            //Console.WriteLine($"Part 1: {newPosition}");
                            //PrintGrid();
                            //return;

                            numberOfCarsLeft--;
                            if (newTrack.Cart.HasMoved(tick))
                                numberOfCarsLeft--;

                            newTrack.Cart = null;
                            current.Cart = null;
                            continue;
                        }

                        newTrack.Cart = cart;
                        current.Cart = null;
                        cart.Turn(newTrack.Kind, tick);
                    }
                }

                if (numberOfCarsLeft == 1)
                {
                    for (var y = 0; y < height; y++)
                    {
                        for (var x = 0; x < width; x++)
                        {
                            if (grid[x, y].Cart != null)
                            {
                                Console.WriteLine($"Part 2: {x},{y}");
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}