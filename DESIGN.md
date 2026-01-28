# Design Rationale

## Approach
The solution reads the log file line by line using a streaming approach (`File.ReadLines`) to avoid loading the entire file into memory. Each log entry is independent, which allows safe parallel processing. To utilize CPU efficiently, `Parallel.ForEach` is used to process multiple lines concurrently.

## Selection
- **File.ReadLines**: Provides memory-efficient streaming of large files.  
- **Parallel.ForEach**: Distributes the workload across all available CPU cores.  
- **ConcurrentDictionary**: Ensures thread-safe counting of IP addresses without manual locks, maintaining correctness under high concurrency.

## Trade-offs
**Advantages:**
- Scales efficiently to very large files (10GB+).  
- Maximizes CPU usage.  
- Clean and maintainable design.  

**Disadvantages:**
- Parallel overhead may be unnecessary for very small files.  
- Final sorting step requires all aggregated IPs in memory (acceptable since IP cardinality is low).

## Alternatives Considered
- **Single-threaded processing**: Rejected due to inefficient CPU utilization.  
- **Manual FileStream chunking**: Rejected due to added complexity and higher risk of errors.  
- **PLINQ**: Not used to maintain better control over parallelism and memory behavior.
