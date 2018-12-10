using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Problems.Day4
{
    public class Day4 : ICodingProblem
    {
        public void Run()
        {
            var sleepMap = new Dictionary<int, Dictionary<int, int>>();
            var currentGuardId = 0;
            var startSleep = 0;
            
            void ParseInput(string s)
            {
                var match = Regex.Match(s, @"\[(?<time>[\d\s-:]+)\] (?<action>Guard|falls|wakes) (#(?<id>\d+))?", RegexOptions.Compiled);
                var minute = DateTimeOffset.Parse(match.Groups["time"].Value).Minute;
                var action = match.Groups["action"].Value;

                switch (action)
                {
                    case "Guard":
                        currentGuardId = int.Parse(match.Groups["id"].Value);
                        break;
                    case "falls":
                        startSleep = minute;
                        break;
                    case "wakes":
                        if (!sleepMap.ContainsKey(currentGuardId))
                            sleepMap[currentGuardId] = new Dictionary<int, int>();
                        for (var i = startSleep; i < minute; i++)
                        {
                            if (!sleepMap[currentGuardId].ContainsKey(i))
                                sleepMap[currentGuardId][i] = 0;
                            sleepMap[currentGuardId][i] += 1;
                        }
                        break;
                }
            }

            var data = File.ReadAllLines("Problems\\Day04\\Day4.data");
            Array.Sort(data);
            foreach (var line in data)
                ParseInput(line);

            var maxSleep = (Id : 0, TotalSleep: 0, MaxMinute: 0);
            var mostFrequent = (Id : 0, Max: 0 , MaxMinute: 0);
            
            foreach (var guard in sleepMap)
            {
                var guardTotal = guard.Value.Sum(x => x.Value);
                var maxMinute = (Minute: -1, Max:0);
                
                foreach (var kv in guard.Value)
                {
                    if (kv.Value > maxMinute.Max)
                        maxMinute = (kv.Key, kv.Value);
                }
                
                if (guardTotal > maxSleep.TotalSleep)
                    maxSleep = (guard.Key, guardTotal, maxMinute.Minute);
                
                if (maxMinute.Max > mostFrequent.Max)
                    mostFrequent = (guard.Key, maxMinute.Max, maxMinute.Minute);
            }
            
            Console.WriteLine($"Part 1: {maxSleep.Id * maxSleep.MaxMinute}");
            Console.WriteLine($"Part 2: {mostFrequent.Id * mostFrequent.MaxMinute}");
        }
    }
}