using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace EnumsEnhancedBenchmark.Benchmarks;

[MarkdownExporterAttribute.GitHub]
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class GetValuesBenchmark
{
    [Benchmark(Baseline = true)]
    public Array GetValues()
    {
        return Enum.GetValues(typeof(TestEnum));
    }

#if NETCOREAPP
    [Benchmark]
    public Array GetValues_Generic()
    {
        return Enum.GetValues<TestEnum>();
    }
#endif

    [Benchmark]
    public Array GetValuesFast()
    {
        return TestEnumEnhanced.GetValuesFast();
    }
}
