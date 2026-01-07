using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace EnumsEnhancedBenchmark.Benchmarks;

[MarkdownExporterAttribute.GitHub]
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class HasFlagBenchmark
{
    [Params(TestEnum.Test4 | TestEnum.Test1 | TestEnum.Test6)]
    public TestEnum TestEnumValue { get; set; }

    [Params(TestEnum.Test1 | TestEnum.Test2 | TestEnum.Test4 | TestEnum.Test5 | TestEnum.Test6)]
    public TestEnum TestEnumFlagCheck { get; set; }

    [Benchmark(Baseline = true)]
    public bool HasFlag()
    {
        return TestEnumValue.HasFlag(TestEnumFlagCheck);
    }

    [Benchmark]
    public bool HasFlagFast()
    {
        return TestEnumValue.HasFlagFast(TestEnumFlagCheck);
    }
}