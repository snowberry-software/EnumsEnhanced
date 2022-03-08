using System.Diagnostics;

namespace EnumsEnhancedTest;

public class EnumTests
{
    [Fact]
    public void GetName()
    {
        var e = TestEnum.Test1;
        Assert.Equal(nameof(TestEnum.Test1), e.GetNameFast(false));
        e = TestEnum.Test5;
        Assert.Equal(nameof(TestEnum.Test5), e.GetNameFast(false));
        e = TestEnum.Test7;
        Assert.Equal(nameof(TestEnum.Test7), e.GetNameFast(false));
        Assert.Equal(nameof(TestEnum.Test7), e.ToStringFast());

        e = TestEnum.Test4 | TestEnum.Test1;
        Assert.NotEqual(nameof(TestEnum.Test7), e.GetNameFast(false));

        Assert.Equal($"{nameof(TestEnum.Test1)}, {nameof(TestEnum.Test4)}", e.ToStringFast());
    }

    [Fact]
    public void GetNamesTest()
    {
        string[]? names = TestEnumEnhanced.GetNamesFast();
        Assert.NotEmpty(names);

        string[] namesExpected = Enum.GetNames(typeof(TestEnum));

        Debug.Equals(namesExpected.Length, names.Length);

        for (int i = 0; i < namesExpected.Length; i++)
        {
            string? name = namesExpected[i];
            Assert.Contains(name, names);
        }
    }

    [Fact]
    public void GetNameWithFlags()
    {
        var e = TestEnum.Test1 | TestEnum.Test5;
        Assert.Null(e.GetNameFast(false));
        Assert.Null(e.GetNameFast(false));

        Assert.Equal($"{nameof(TestEnum.Test1)}, {nameof(TestEnum.Test5)}", e.GetNameFast(true));
    }

    [Fact]
    public void GetValuesTest()
    {
        var values = TestEnumEnhanced.GetValuesFast();
        Assert.NotEmpty(values);

        var valuesExpected = Enum.GetValues<TestEnum>();

        Debug.Equals(valuesExpected.Length, values.Length);

        for (int i = 0; i < valuesExpected.Length; i++)
        {
            var value = valuesExpected[i];
            Assert.Contains(value, values);
        }
    }

    [Fact]
    public void HasFlagFast()
    {
        var e = TestEnum.Test1 | TestEnum.Test4;
        Assert.True(e.HasFlagFast(TestEnum.Test1));
        Assert.True(e.HasFlagFast(TestEnum.Test_1));
        Assert.True(e.HasFlagFastUnsafe(TestEnum.Test4));

        Assert.False(e.HasFlagFast(TestEnum.Test5));
        Assert.False(e.HasFlagFast(TestEnum.Test6));
    }

    /// <summary>
    /// <see langword="bool"/>
    /// </summary>
    [Fact]
    public void IsDefinedNameTest()
    {
        Assert.True(TestEnumEnhanced.IsDefinedFast(nameof(TestEnum.Test1)));
        Assert.True(TestEnumEnhanced.IsDefinedFast(nameof(TestEnum.Test7)));
        Assert.False(TestEnumEnhanced.IsDefinedFast(nameof(TestEnum.Test7) + "1"));

        Assert.True(Enum.IsDefined(typeof(TestEnum), nameof(TestEnum.Test1)));
        Assert.True(Enum.IsDefined(typeof(TestEnum), nameof(TestEnum.Test7)));
        Assert.False(Enum.IsDefined(typeof(TestEnum), nameof(TestEnum.Test7) + "1"));
    }

    [Fact]
    public void IsDefinedValueTest()
    {
        foreach (var value in TestEnumEnhanced.GetValuesFast())
            Assert.True(TestEnumEnhanced.IsDefinedFast(value));

        foreach (var value in TestEnumEnhanced.GetValuesFast())
            Assert.False(TestEnumEnhanced.IsDefinedFast(((int)value) + int.MaxValue / 2));
    }

    [Fact]
    public void ParseMultipleNamesTest()
    {
        string enumName = $"{nameof(TestEnum.Test4)}, {nameof(TestEnum.Test1)}";

        Assert.Equal(TestEnum.Test4 | TestEnum.Test1, Enum.Parse(typeof(TestEnum), enumName));
        Assert.Equal(TestEnum.Test4 | TestEnum.Test1, TestEnumEnhanced.ParseFast(enumName));

        Assert.Throws<ArgumentException>(() =>
        {
            TestEnumEnhanced.ParseFast($"{enumName},");
        });

        Assert.Throws<ArgumentException>(() =>
        {
            TestEnumEnhanced.ParseFast($"{enumName}1");
        });

        Assert.Throws<ArgumentException>(() =>
        {
            TestEnumEnhanced.ParseFast($"Test");
        });

        Assert.Throws<ArgumentException>(() =>
        {
            TestEnum2Enhanced.ParseFast($"-2x");
        });
    }

    [Fact]
    public void ParseNameTest()
    {
        string enumName = nameof(TestEnum.Test4);

        Assert.Equal(TestEnum.Test4, Enum.Parse(typeof(TestEnum), enumName));

        foreach (var value in TestEnumEnhanced.GetValuesFast())
        {
            Assert.Equal(value, TestEnumEnhanced.ParseFast(value.GetNameFast()!, true));
            Assert.True(TestEnumEnhanced.TryParseFast(value.GetNameFast()!, true, out var outVal));
            Assert.Equal(value, outVal);

            Assert.False(TestEnumEnhanced.TryParseFast(value.GetNameFast()! + "xxxx", true, out outVal));
        }

        // Uppercase

        foreach (var value in TestEnumEnhanced.GetValuesFast())
        {
            Assert.Equal(value, TestEnumEnhanced.ParseFast(value.GetNameFast()!.ToUpper(), true));
            Assert.True(TestEnumEnhanced.TryParseFast(value.GetNameFast()!.ToUpper(), true, out var outVal));
            Assert.Equal(value, outVal);

            Assert.False(TestEnumEnhanced.TryParseFast(value.GetNameFast()!.ToUpper() + "xxxx", true, out outVal));
        }
    }

    [Fact]
    public void ParseValueTest()
    {
        string enumName = nameof(TestEnum.Test4);

        Assert.Equal(TestEnum.Test4, Enum.Parse(typeof(TestEnum), enumName));

        foreach (var value in TestEnumEnhanced.GetValuesFast())
        {
            Assert.Equal(value, Enum.Parse(typeof(TestEnum), ((int)value).ToString()));
            Assert.Equal(value, TestEnumEnhanced.ParseFast(((int)value).ToString()));

            Assert.True(TestEnumEnhanced.TryParseFast(((int)value).ToString(), false, out var outVal));
            Assert.Equal(value, outVal);

            Assert.False(TestEnumEnhanced.TryParseFast(((int)value).ToString() + "x", false, out outVal));
        }

        Assert.Throws<ArgumentException>(() =>
        {
            TestEnum2Enhanced.ParseFast("0");
        });

        Assert.Equal(TestEnum2.Test1, TestEnum2Enhanced.ParseFast("-2"));
        Assert.Equal(TestEnum2.Test2, TestEnum2Enhanced.ParseFast("1"));
    }
}