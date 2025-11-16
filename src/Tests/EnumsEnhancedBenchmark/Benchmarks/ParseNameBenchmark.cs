using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace EnumsEnhancedBenchmark.Benchmarks;

[MarkdownExporterAttribute.GitHub]
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class ParseNameBenchmark
{
    [Params(nameof(TestEnum.Test1), nameof(TestEnum.Test6))]
    public string NameValue { get; set; } = "";

    [Params("TEST1", "TEST6")]
    public string NameValueUpper { get; set; } = "";

    [Benchmark]
    public object Parse_Name()
    {
        return Enum.Parse(typeof(TestEnum), NameValue);
    }

#if NETCOREAPP
    [Benchmark]
    public object Parse_Name_Generic()
    {
        return Enum.Parse<TestEnum>(NameValue);
    }
#endif

    [Benchmark]
    public object ParseFast_Name()
    {
        return TestEnumEnhanced.ParseFast(NameValue);
    }

    [Benchmark]
    public object Parse_NameIgnoreCase()
    {
        return Enum.Parse(typeof(TestEnum), NameValueUpper, true);
    }

#if NETCOREAPP
    [Benchmark]
    public object Parse_NameIgnoreCase_Generic()
    {
        return Enum.Parse<TestEnum>(NameValueUpper, true);
    }
#endif

    [Benchmark]
    public object ParseFast_NameIgnoreCase()
    {
        return TestEnumEnhanced.ParseFast(NameValueUpper, true);
    }
}
