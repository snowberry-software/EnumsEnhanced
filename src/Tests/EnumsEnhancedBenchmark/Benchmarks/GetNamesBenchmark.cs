using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace EnumsEnhancedBenchmark.Benchmarks;

[MarkdownExporterAttribute.GitHub]
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class GetNamesBenchmark
{
    [Benchmark(Baseline = true)]
    public string[] GetNames()
    {
        return Enum.GetNames(typeof(TestEnum));
    }

#if NETCOREAPP
    [Benchmark]
    public string[] GetNames_Generic()
    {
        return Enum.GetNames<TestEnum>();
    }
#endif

    [Benchmark]
    public string[] GetNamesFast()
    {
        return TestEnumEnhanced.GetNamesFast();
    }
}
