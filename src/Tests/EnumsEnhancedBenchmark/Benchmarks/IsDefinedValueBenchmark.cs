using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace EnumsEnhancedBenchmark.Benchmarks;

[MarkdownExporterAttribute.GitHub]
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net60)]
public class IsDefinedValueBenchmark
{
    [Params((short)TestEnum.Test1, (short)TestEnum.Test7)]
    public short EnumValue { get; set; }

    [Benchmark(Baseline = true)]
    public bool IsDefined_Value()
    {
        return Enum.IsDefined(typeof(TestEnum), EnumValue);
    }

    [Benchmark]
    public bool IsDefinedFast_Value()
    {
        return TestEnumEnhanced.IsDefinedFast(EnumValue);
    }
}