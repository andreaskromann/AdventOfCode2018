using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Problems.Day7
{
    public class Day7 : ICodingProblem
    {
        private const int NumberOfWorkers = 5;
        private const int DurationCorrection = 4;
        
        struct Edge
        {
            public readonly char From;
            public readonly char To;

            public Edge(char @from, char to)
            {
                From = @from;
                To = to;
            }
        }

        class Job
        {
            public readonly char Id;
            private int _seconds;
            public bool IsDone => _seconds == 0;
            public void Work() => _seconds--;

            public Job(char id)
            {
                Id = id;
                _seconds = id - DurationCorrection;
            }
        }
        
        public void Run()
        {
            var nodes = new HashSet<char>();
            var jobs = new List<Job>();
            
            Edge ParseInput(string l)
            {
                var matches = Regex.Match(l, @"Step (?<from>\w) must be finished before step (?<to>\w) can begin", RegexOptions.Compiled);
                var @from = matches.Groups["from"].Value[0];
                var to = matches.Groups["to"].Value[0];
                
                var added = nodes.Add(@from);
                if (added)
                    jobs.Add(new Job(@from));
               
                added = nodes.Add(to);
                if (added)
                    jobs.Add(new Job(to));
                
                return new Edge(@from, to);
            }
            
            var data = File.ReadAllLines("Problems\\Day07\\Day7.data")
                .Select(ParseInput)
                .ToList();

            List<char> GetCandidates(IEnumerable<char> todo, IReadOnlyCollection<Edge> graph)
            {
                var candidates = todo
                    .Where(x => graph.All(e => e.To != x))
                    .ToList();
                candidates.Sort();
                return candidates;
            }

            Console.Write("Part 1: ");
            var edges = new List<Edge>(data);
            while (nodes.Count > 0)
            {
                var candidates = GetCandidates(nodes, edges);
                var winner = candidates[0];
                Console.Write(winner);
                nodes.Remove(winner);
                edges.RemoveAll(e => e.From == winner);
            }
            Console.WriteLine();
            
            // Part 2
            var workers = new Job[5];
            var seconds = 0;
            while (seconds < 100000) // for safety ;)
            {
                for (var i = 0; i < NumberOfWorkers; i++)
                {
                    var job = workers[i];

                    if (job?.IsDone ?? false)
                    {
                        var id = workers[i].Id;
                        data.RemoveAll(e => e.From == id);
                        workers[i] = null;
                    }
                }
                
                var candidates = GetCandidates(jobs.Select(j => j.Id), data);
                
                for (var i = 0; i < NumberOfWorkers; i++)
                {
                    var job = workers[i];
                    
                    if (job == null && candidates.Any())
                    {
                        workers[i] = jobs.First(x => x.Id == candidates[0]);
                        jobs.Remove(workers[i]);
                        candidates.Remove(candidates[0]);
                        job = workers[i];
                    }
                    
                    job?.Work();
                }

                if (!jobs.Any() && workers.All(w => w == null))
                {
                    Console.WriteLine($"Part 2: {seconds}");
                    return;
                }
                
                seconds++;
            }
        }
    }
}