using System.Diagnostics;

namespace LogAnalyzer
{
    class Program
    {
        static void Main()
        {
            const string filePath = "server_logs.txt";

            var stopwatch = Stopwatch.StartNew();

            var analyzer = new LogAnalyzer();
            analyzer.Analyze(filePath);

            stopwatch.Stop();
            Console.WriteLine($"\nExecution Time: {stopwatch.Elapsed}");
        }
    }
}
