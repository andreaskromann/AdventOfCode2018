using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.Problems.Day8
{
    public class Day8 : ICodingProblem
    {
        class Node
        {
            public List<Node> Children { get; }
            public List<int> MetaData { get; }
            public int Value 
            {
                get
                {
                    if (Children.Any())
                    {
                        return MetaData
                            .Select(x => x <= Children.Count && x > 0 ? Children[x - 1].Value : 0)
                            .Sum();
                    }
                    return MetaData.Sum();
                }
            }

            public Node(List<Node> children, List<int> metaData)
            {
                Children = children;
                MetaData = metaData;
            }
        }
        
        public void Run()
        {
            var data = File.ReadAllLines("Problems\\Day8\\Day8.data")[0]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            var metaDataSum = 0;
            
            Node ParseNode(ref Span<int> span)
            {
                if (span.Length < 2)
                    return null;

                var numberOfChildren = span[0];
                var numberOfMetadata = span[1];
                span = span.Length > 2 ? span.Slice(2) : Span<int>.Empty;

                var children = new List<Node>();
                var metaData = new List<int>();
                
                for (var i = 0; i < numberOfChildren; i++)
                {
                    var child = ParseNode(ref span);
                    children.Add(child);
                }
                
                for (var i = 0; i < numberOfMetadata; i++)
                {
                    var metadata = span[0];
                    metaData.Add(metadata);
                    metaDataSum += metadata;
                    span = span.Length > 1 ? span.Slice(1) : Span<int>.Empty;
                }
                
                return new Node(children, metaData);
            }

            var allData = new Span<int>(data);
            var root = ParseNode(ref allData);
            
            Console.WriteLine($"Part 1: {metaDataSum}");
            Console.WriteLine($"Part 2: {root.Value}");            
        }
    }
}