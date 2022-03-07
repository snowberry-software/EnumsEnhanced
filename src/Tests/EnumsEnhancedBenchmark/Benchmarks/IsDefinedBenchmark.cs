using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace EnumsEnhancedBenchmark.Benchmarks;

[MarkdownExporterAttribute.GitHub]
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net60)]
public class IsDefinedBenchmark
{
    [Params(nameof(TestEnum.Test1), nameof(TestEnum.Test6), "Invalid")]
    public string NameValue { get; set; } = "";

    [Benchmark(Baseline = true)]
    public bool IsDefined_StringName()
    {
        return Enum.IsDefined(typeof(TestEnum), NameValue);
    }

    [Benchmark]
    public bool IsDefinedFast_StringName()
    {
        return TestEnumEnhanced.IsDefinedFast(NameValue);
    }
}
