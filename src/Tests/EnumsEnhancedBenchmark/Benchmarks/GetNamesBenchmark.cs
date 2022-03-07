using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace EnumsEnhancedBenchmark.Benchmarks;

[MarkdownExporterAttribute.GitHub]
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net60)]
public class GetNamesBenchmark
{
    [Benchmark(Baseline = true)]
    public string[] GetNames()
    {
        return Enum.GetNames(typeof(TestEnum));
    }

    [Benchmark]
    public string[] GetNamesFast()
    {
        return TestEnumEnhanced.GetNamesFast();
    }
}
