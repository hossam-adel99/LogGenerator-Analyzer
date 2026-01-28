# Log Analyzer

## Overview
This project generates and analyzes large server log files (`server_logs.txt`) to identify the top 5 IP addresses by occurrence count.  
It is designed to handle very large files efficiently (10GB+), utilize multiple CPU cores, and ensure thread-safe counting.

---

## Project Structure

Solution
<br>│
<br>├── LogGenerator/ # Generates the log file
<br>│ └── Program.cs
<br>│ └── Generator.cs
<br>│
<br>├── LogAnalyzer/ # Analyzes the log file
<br>│ └── Program.cs
<br>│ └── LogAnalyzer.cs
<br>│
<br>├── DESIGN.md
<br>│
<br>└── README.md # Instructions, Output, Design Rationale


---

## How to Run

1. Generate the Log File
Run the `LogGenerator` project to create `server_logs.txt`:

 ```csharp
Generator.Generate("server_logs.txt", 500, 65536);
```
- This will generate a 500MB log file.
- You can increase FileSizeInMB to test larger datasets (e.g., 5GB+).
<br><br>
2. Copy the Log File
- Copy server_logs.txt to the output folder of the LogAnalyzer project, usually:
 LogAnalyzer/bin/Debug/net8.0/
<br><br>
3. Run LogAnalyzer
- Open Visual Studio.
- Set LogAnalyzer as the startup project.
- Press Ctrl + F5 to run without debugging.
- The program will print the top 5 IP addresses and their occurrence count along with execution time.

## Example Output
### Top 5 IP Addresses:
 ```csharp
192.168.1.0 -> 1455994
192.168.1.1 -> 378578
192.168.1.2 -> 265774
192.168.1.3 -> 211203
192.168.1.4 -> 178735

Execution Time: 00:00:02.7978981
```
## Features
- Generates large log files (500MB+ or more)
- Streaming file processing to handle very large files efficiently
- Multi-threaded parallel processing for high performance
- Thread-safe counting using ConcurrentDictionary
- Clean separation of concerns between generation and analysis

## Design Rationale
### Approach
The solution reads the log file line by line using a streaming approach (`File.ReadLines`) to avoid loading the entire file into memory. Each log entry is independent, which allows safe parallel processing. To utilize CPU efficiently, `Parallel.ForEach` is used to process multiple lines concurrently.

### Selection
- **File.ReadLines**: Provides memory-efficient streaming of large files.  
- **Parallel.ForEach**: Distributes the workload across all available CPU cores.  
- **ConcurrentDictionary**: Ensures thread-safe counting of IP addresses without manual locks, maintaining correctness under high concurrency.

### Trade-offs
**Advantages:**
- Scales efficiently to very large files (10GB+).  
- Maximizes CPU usage.  
- Clean and maintainable design.  

**Disadvantages:**
- Parallel overhead may be unnecessary for very small files.  
- Final sorting step requires all aggregated IPs in memory (acceptable since IP cardinality is low).

### Alternatives Considered
- **Single-threaded processing**: Rejected due to inefficient CPU utilization.  
- **Manual FileStream chunking**: Rejected due to added complexity and higher risk of errors.  
- **PLINQ**: Not used to maintain better control over parallelism and memory behavior.

