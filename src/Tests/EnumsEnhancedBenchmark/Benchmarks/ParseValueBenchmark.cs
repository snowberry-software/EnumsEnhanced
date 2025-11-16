using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace EnumsEnhancedBenchmark.Benchmarks;

[MarkdownExporterAttribute.GitHub]
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class ParseValueBenchmark
{
    [Params("1", "64")]
    public string Value { get; set; } = "";

    [Benchmark(Baseline = true)]
    public object Parse_Value()
    {
        return Enum.Parse(typeof(TestEnum), Value);
    }

#if NETCOREAPP
    [Benchmark]
    public object Parse_Value_Generic()
    {
        return Enum.Parse<TestEnum>(Value);
    }
#endif

    [Benchmark]
    public object ParseFast_Value()
    {
        return TestEnumEnhanced.ParseFast(Value);
    }
}
