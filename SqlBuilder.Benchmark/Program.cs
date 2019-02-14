using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace SqlBuilder.Benchmark
{
  class Program
  {
    static void Main(string[] args)
    {
      Summary summary = BenchmarkRunner.Run<KeyBenchmarks>();
    }
  }
}
