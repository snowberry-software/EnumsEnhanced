using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace EnumsEnhancedBenchmark.Benchmarks;

[MarkdownExporterAttribute.GitHub]
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net60)]
public class GetNameBenchmark
{
    [Params(TestEnum.Test1, TestEnum.Test6)]
    public TestEnum NameValue { get; set; }

    [Benchmark(Baseline = true)]
    public string? GetName()
    {
        return Enum.GetName(typeof(TestEnum), NameValue);
    }

    [Benchmark]
    public string? GetNameFast()
    {
        return NameValue.GetNameFast(false);
    }
}
