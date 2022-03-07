using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace EnumsEnhancedBenchmark.Benchmarks;

[MarkdownExporterAttribute.GitHub]
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net60)]
public class GetValuesBenchmark
{
    [Benchmark(Baseline = true)]
    public Array GetValues()
    {
        return Enum.GetValues(typeof(TestEnum));
    }

    [Benchmark]
    public Array GetValuesFast()
    {
        return TestEnumEnhanced.GetValuesFast();
    }
}
