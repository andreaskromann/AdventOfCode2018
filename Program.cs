using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2018
{
    internal static class Program
    {
        private static void Main()
        {
            var type = typeof(ICodingProblem);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p != type)
                .ToDictionary(p => p.Name.Substring(3));
            
            Console.WriteLine("{0} solutions found", types.Count);
            Console.WriteLine();
            
            Console.Write("Solution to run? ");
            Console.Out.Flush();
            var number = Console.ReadLine();
            Console.WriteLine();

            if (string.IsNullOrEmpty(number))
                return;

            if(!int.TryParse(number, out var choice) || choice > types.Count || choice < 1 || !types.ContainsKey(number))
                Console.WriteLine("Invalid choice");

            var problem = types[number];
            
            var instance = (ICodingProblem)Activator.CreateInstance(problem);
            
            try
            {
                var sw = Stopwatch.StartNew();
                
                instance.Run();

                sw.Stop();
                
                Console.WriteLine();
                Console.WriteLine("Elapsed = {0}ms", sw.ElapsedMilliseconds);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
        }
    }
}