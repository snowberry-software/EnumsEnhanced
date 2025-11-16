using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace EnumsEnhancedBenchmark.Benchmarks;

[MarkdownExporterAttribute.GitHub]
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class IsDefinedValueBenchmark
{
    [Params(TestEnum.Test1, TestEnum.Test7)]
    public TestEnum EnumValue { get; set; }

    [Benchmark(Baseline = true)]
    public bool IsDefined_Value()
    {
        return Enum.IsDefined(typeof(TestEnum), EnumValue);
    }

#if NETCOREAPP
    [Benchmark]
    public bool IsDefined_Value_Generic()
    {
        return Enum.IsDefined(EnumValue);
    }
#endif

    [Benchmark]
    public bool IsDefinedFast_Value()
    {
        return TestEnumEnhanced.IsDefinedFast(EnumValue);
    }
}