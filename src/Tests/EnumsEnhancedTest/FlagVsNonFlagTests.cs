namespace EnumsEnhancedTest;

/// <summary>
/// Tests comparing flag enum behavior vs non-flag enum behavior
/// </summary>
public class FlagVsNonFlagTests
{
    [Fact]
    public void FlagEnum_CombinedValues_DecomposeToFlags()
    {
        var combined = ByteFlagsEnum.Bit0 | ByteFlagsEnum.Bit1 | ByteFlagsEnum.Bit2;

        string? resultFast = combined.ToStringFast();
        string? resultDefault = combined.ToString();

        Assert.Equal(resultDefault, resultFast);
        Assert.Contains(", ", resultFast); // Should have commas
    }

    [Fact]
    public void NonFlagEnum_CombinedValues_ReturnsNumeric()
    {
        // Sequential enum is not [Flags], so combined values return numeric
        var combined = (SequentialEnum)((int)SequentialEnum.First | (int)SequentialEnum.Second | (int)SequentialEnum.Fifth);

        string? resultFast = combined.ToStringFast();
        string? resultDefault = combined.ToString();

        Assert.Equal(resultDefault, resultFast);
    }

    [Fact]
    public void FlagEnum_UndefinedComposite_DecomposesKnownBits()
    {
        var composite = (ByteFlagsEnum)((byte)ByteFlagsEnum.Bit0 | (byte)ByteFlagsEnum.Bit1 | 128);

        string? resultFast = composite.ToStringFast();
        string? resultDefault = composite.ToString();

        Assert.Equal(resultDefault, resultFast);
    }

    [Fact]
    public void NonFlagEnum_DefinedValue_ReturnsName()
    {
        var value = SequentialEnum.Third;

        Assert.Equal(value.ToString(), value.ToStringFast());
        Assert.Equal("Third", value.ToStringFast());
    }

    [Fact]
    public void FlagEnum_NamedComposite_ReturnsCompositeNameNotDecomposition()
    {
        // When a composite value has an explicit name, that should be used
        var composite = CompositeFlagsEnum.ReadWrite;

        string? resultFast = composite.ToStringFast();
        string? resultDefault = composite.ToString();

        Assert.Equal(resultDefault, resultFast);
        // Should return "ReadWrite", not "Read, Write"
        Assert.Equal("ReadWrite", resultFast);
    }

    [Fact]
    public void FlagEnum_UnnamedComposite_DecomposesToComponents()
    {
        // When composite has no explicit name, decompose it
        var composite = CompositeFlagsEnum.Read | CompositeFlagsEnum.Delete;

        string? resultFast = composite.ToStringFast();
        string? resultDefault = composite.ToString();

        Assert.Equal(resultDefault, resultFast);
        Assert.Contains(", ", resultFast);
    }

    [Fact]
    public void FlagEnum_AllFlag_ReturnsAllNameIfDefined()
    {
        var all = CompositeFlagsEnum.All;

        string? resultFast = all.ToStringFast();
        string? resultDefault = all.ToString();

        Assert.Equal(resultDefault, resultFast);
        Assert.Equal("All", resultFast);
    }

    [Fact]
    public void FlagEnum_ZeroValue_ReturnsNoneName()
    {
        var none = ByteFlagsEnum.None;

        Assert.Equal(none.ToString(), none.ToStringFast());
        Assert.Equal("None", none.ToStringFast());
    }

    [Fact]
    public void NonFlagEnum_ZeroValue_ReturnsDefinedName()
    {
        var none = SequentialEnum.None;

        Assert.Equal(none.ToString(), none.ToStringFast());
        Assert.Equal("None", none.ToStringFast());
    }

    [Fact]
    public void FlagEnum_Parse_SupportsMultipleNames()
    {
        var parsed = CompositeFlagsEnumEnhanced.ParseFast("Read, Write, Execute");
        var expected = CompositeFlagsEnum.Read | CompositeFlagsEnum.Write | CompositeFlagsEnum.Execute;

        Assert.Equal(expected, parsed);
    }

    [Fact]
    public void NonFlagEnum_Parse_DoesNotSupportMultipleNames()
    {
        var original = Enum.Parse<SequentialEnum>("First, Second");
        var actual = SequentialEnumEnhanced.ParseFast("First, Second");

        Assert.Equal(original, actual);
    }

    [Fact]
    public void FlagEnum_HasFlag_WorksCorrectly()
    {
        var value = ByteFlagsEnum.Bit0 | ByteFlagsEnum.Bit1;

        Assert.True(value.HasFlagFast(ByteFlagsEnum.Bit0));
        Assert.True(value.HasFlagFast(ByteFlagsEnum.Bit1));
        Assert.False(value.HasFlagFast(ByteFlagsEnum.Bit2));

        Assert.Equal(value.HasFlag(ByteFlagsEnum.Bit0), value.HasFlagFast(ByteFlagsEnum.Bit0));
    }

    [Fact]
    public void NonFlagEnum_HasFlag_StillWorks()
    {
        // HasFlag works on non-flag enums but behavior is unusual
        var value = SequentialEnum.Third; // 3
        var flag = SequentialEnum.First;  // 1

        Assert.Equal(value.HasFlag(flag), value.HasFlagFast(flag));
        // 3 & 1 == 1, so technically "has flag"
        Assert.True(value.HasFlagFast(flag));
    }

    [Fact]
    public void FlagEnum_IsDefined_FalseForCombinations()
    {
        var combined = ByteFlagsEnum.Bit0 | ByteFlagsEnum.Bit1;

        Assert.Equal(Enum.IsDefined(typeof(ByteFlagsEnum), combined),
                     ByteFlagsEnumEnhanced.IsDefinedFast(combined));
        Assert.False(ByteFlagsEnumEnhanced.IsDefinedFast(combined));
    }

    [Fact]
    public void NonFlagEnum_IsDefined_FalseForInvalid()
    {
        var invalid = (SequentialEnum)99;

        Assert.Equal(Enum.IsDefined(typeof(SequentialEnum), invalid),
                     SequentialEnumEnhanced.IsDefinedFast(invalid));
        Assert.False(SequentialEnumEnhanced.IsDefinedFast(invalid));
    }

    [Fact]
    public void FlagEnum_CompositeWithExplicitName_IsDefinedTrue()
    {
        var composite = CompositeFlagsEnum.ReadWrite;

        Assert.Equal(Enum.IsDefined(typeof(CompositeFlagsEnum), composite),
                     CompositeFlagsEnumEnhanced.IsDefinedFast(composite));
        Assert.True(CompositeFlagsEnumEnhanced.IsDefinedFast(composite));
    }
}
