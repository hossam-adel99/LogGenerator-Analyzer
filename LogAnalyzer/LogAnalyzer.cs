using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace LogAnalyzer
{
    public class LogAnalyzer
    {
        private readonly ConcurrentDictionary<string, long> _ipCounts =
            new ConcurrentDictionary<string, long>();

        public void Analyze(string filePath)
        {
            IEnumerable<string> lines = File.ReadLines(filePath);

            Parallel.ForEach(
                lines,
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                },
                ProcessLine
            );

            PrintTop5();
        }

        private void ProcessLine(string line)
        {
            var parts = line.Split(';');

            foreach (var part in parts)
            {
                if (part.StartsWith("ip="))
                {
                    string ip = part.Substring(3);
                    _ipCounts.AddOrUpdate(ip, 1, (_, current) => current + 1);
                    break;
                }
            }
        }

        private void PrintTop5()
        {
            Console.WriteLine("Top 5 IP Addresses:");

            foreach (var item in _ipCounts
                .OrderByDescending(x => x.Value)
                .Take(5))
            {
                Console.WriteLine($"{item.Key} -> {item.Value}");
            }
        }
    }

}
