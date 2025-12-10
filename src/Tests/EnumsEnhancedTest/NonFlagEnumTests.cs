namespace EnumsEnhancedTest;

/// <summary>
/// Comprehensive tests for non-flag enums focusing on ToString and Parse behavior
/// </summary>
public class NonFlagEnumTests
{
    [Fact]
    public void SequentialEnum_ToString_AllValues_MatchDefault()
    {
        var values = SequentialEnumEnhanced.GetValuesFast();
        foreach (var value in values)
        {
            Assert.Equal(value.ToString(), value.ToStringFast());
        }
    }

    [Fact]
    public void SequentialEnum_Parse_AllNames_WorkCorrectly()
    {
        string[] names = SequentialEnumEnhanced.GetNamesFast();
        foreach (string name in names)
        {
            var expectedValue = (SequentialEnum)Enum.Parse(typeof(SequentialEnum), name);
            var actualValue = SequentialEnumEnhanced.ParseFast(name);
            Assert.Equal(expectedValue, actualValue);
        }
    }

    [Fact]
    public void SequentialEnum_ParseNumeric_WorksForAllValues()
    {
        for (int i = 0; i <= 5; i++)
        {
            var expected = (SequentialEnum)Enum.Parse(typeof(SequentialEnum), i.ToString());
            var actual = SequentialEnumEnhanced.ParseFast(i.ToString());
            Assert.Equal(expected, actual);
        }
    }

    [Fact]
    public void SequentialEnum_UndefinedValue_ReturnsNumeric()
    {
        var undefined = (SequentialEnum)99;
        Assert.Equal("99", undefined.ToStringFast());
        Assert.Equal(undefined.ToString(), undefined.ToStringFast());
    }

    [Fact]
    public void SequentialEnum_Parse_UndefinedNumeric_CreatesValue()
    {
        var parsed = SequentialEnumEnhanced.ParseFast("99");
        Assert.Equal((SequentialEnum)99, parsed);
        Assert.Equal(99, (int)parsed);
    }

    [Fact]
    public void SequentialEnum_Zero_ReturnsName()
    {
        var zero = SequentialEnum.None;
        Assert.Equal(nameof(SequentialEnum.None), zero.ToStringFast());
        Assert.Equal(zero.ToString(), zero.ToStringFast());
    }

    [Fact]
    public void GappedEnum_AllDefinedValues_ReturnNames()
    {
        Assert.Equal(nameof(GappedEnum.Min), GappedEnum.Min.ToStringFast());
        Assert.Equal(nameof(GappedEnum.Small), GappedEnum.Small.ToStringFast());
        Assert.Equal(nameof(GappedEnum.Medium), GappedEnum.Medium.ToStringFast());
        Assert.Equal(nameof(GappedEnum.Large), GappedEnum.Large.ToStringFast());
        Assert.Equal(nameof(GappedEnum.Max), GappedEnum.Max.ToStringFast());
    }

    [Fact]
    public void GappedEnum_GapValues_ReturnNumeric()
    {
        int[] testCases = new[] { 1, 5, 15, 25, 75, 150, 200, 254 };

        foreach (int testValue in testCases)
        {
            var enumValue = (GappedEnum)testValue;
            Assert.Equal(testValue.ToString(), enumValue.ToStringFast());
            Assert.Equal(enumValue.ToString(), enumValue.ToStringFast());
        }
    }

    [Fact]
    public void GappedEnum_Parse_DefinedNames_WorkCorrectly()
    {
        Assert.Equal(GappedEnum.Min, GappedEnumEnhanced.ParseFast(nameof(GappedEnum.Min)));
        Assert.Equal(GappedEnum.Small, GappedEnumEnhanced.ParseFast(nameof(GappedEnum.Small)));
        Assert.Equal(GappedEnum.Medium, GappedEnumEnhanced.ParseFast(nameof(GappedEnum.Medium)));
        Assert.Equal(GappedEnum.Large, GappedEnumEnhanced.ParseFast(nameof(GappedEnum.Large)));
        Assert.Equal(GappedEnum.Max, GappedEnumEnhanced.ParseFast(nameof(GappedEnum.Max)));
    }

    [Fact]
    public void GappedEnum_Parse_NumericInGaps_WorksCorrectly()
    {
        byte[] testCases = new byte[] { 1, 5, 15, 25, 75, 150, 200, 254 };

        foreach (byte testValue in testCases)
        {
            var parsed = GappedEnumEnhanced.ParseFast(testValue.ToString());
            Assert.Equal((GappedEnum)testValue, parsed);
            Assert.Equal(testValue, (byte)parsed);
        }
    }

    [Fact]
    public void GappedEnum_RoundTrip_GapValues_WorksCorrectly()
    {
        var gapValue = (GappedEnum)42;
        string? toString = gapValue.ToStringFast();
        var parsed = GappedEnumEnhanced.ParseFast(toString!);

        Assert.Equal(gapValue, parsed);
        Assert.Equal("42", toString);
    }

    [Fact]
    public void NegativeValuesEnum_AllValues_ToStringCorrectly()
    {
        Assert.Equal(nameof(NegativeValuesEnum.VeryNegative), NegativeValuesEnum.VeryNegative.ToStringFast());
        Assert.Equal(nameof(NegativeValuesEnum.Negative), NegativeValuesEnum.Negative.ToStringFast());
        Assert.Equal(nameof(NegativeValuesEnum.Zero), NegativeValuesEnum.Zero.ToStringFast());
        Assert.Equal(nameof(NegativeValuesEnum.Positive), NegativeValuesEnum.Positive.ToStringFast());
        Assert.Equal(nameof(NegativeValuesEnum.VeryPositive), NegativeValuesEnum.VeryPositive.ToStringFast());
    }

    [Fact]
    public void NegativeValuesEnum_Parse_NegativeNumbers_WorksCorrectly()
    {
        var parsed1 = NegativeValuesEnumEnhanced.ParseFast(int.MinValue.ToString());
        Assert.Equal(NegativeValuesEnum.VeryNegative, parsed1);

        var parsed2 = NegativeValuesEnumEnhanced.ParseFast("-100");
        Assert.Equal(NegativeValuesEnum.Negative, parsed2);

        var parsed3 = NegativeValuesEnumEnhanced.ParseFast("-50");
        Assert.Equal((NegativeValuesEnum)(-50), parsed3);
    }

    [Fact]
    public void NegativeValuesEnum_UndefinedNegative_ReturnsNumeric()
    {
        var undefined = (NegativeValuesEnum)(-999);
        Assert.Equal("-999", undefined.ToStringFast());
        Assert.Equal(undefined.ToString(), undefined.ToStringFast());
    }

    [Fact]
    public void DuplicateValuesEnum_DuplicatesSameValue_ToStringReturnsFirst()
    {
        // Value 2 has two names: Second (first), SecondAlias (last)
        var value2 = (DuplicateValuesEnum)2;
        Assert.Equal(value2.ToString(), value2.ToStringFast());
        Assert.Equal(nameof(DuplicateValuesEnum.Second), value2.ToStringFast());

        // Value 3 has three names: Third, ThirdAlias, ThirdAlias2 (last)
        var value3 = (DuplicateValuesEnum)3;
        Assert.Equal(value3.ToString(), value3.ToStringFast());
        Assert.Equal(nameof(DuplicateValuesEnum.Third), value3.ToStringFast());
    }

    [Fact]
    public void DuplicateValuesEnum_ParseAllAliases_ReturnsSameValue()
    {
        // Parse all aliases for value 2
        var second = DuplicateValuesEnumEnhanced.ParseFast(nameof(DuplicateValuesEnum.Second));
        var secondAlias = DuplicateValuesEnumEnhanced.ParseFast(nameof(DuplicateValuesEnum.SecondAlias));
        Assert.Equal(second, secondAlias);
        Assert.Equal(2, (int)second);

        // Parse all aliases for value 3
        var third = DuplicateValuesEnumEnhanced.ParseFast(nameof(DuplicateValuesEnum.Third));
        var thirdAlias = DuplicateValuesEnumEnhanced.ParseFast(nameof(DuplicateValuesEnum.ThirdAlias));
        var thirdAlias2 = DuplicateValuesEnumEnhanced.ParseFast(nameof(DuplicateValuesEnum.ThirdAlias2));
        Assert.Equal(third, thirdAlias);
        Assert.Equal(third, thirdAlias2);
        Assert.Equal(3, (int)third);
    }

    [Fact]
    public void DuplicateValuesEnum_RoundTrip_UsesLastName()
    {
        var value2 = DuplicateValuesEnum.Second;
        string? toString = value2.ToStringFast();
        var parsed = DuplicateValuesEnumEnhanced.ParseFast(toString!);

        // ToString returns first name (Second), parsing it should give same value
        Assert.Equal(value2.ToString(), toString);
        Assert.Equal(nameof(DuplicateValuesEnum.Second), toString);
        Assert.Equal(value2, parsed);
        Assert.Equal(2, (int)parsed);
    }

    [Fact]
    public void PseudoFlagsEnum_CombinedValues_NotDecomposed()
    {
        // Even though it has power-of-2 values, it's not [Flags]
        var combined = (PseudoFlagsEnum)(1 | 2 | 4); // 7
        Assert.Equal("7", combined.ToStringFast());
        Assert.Equal(combined.ToString(), combined.ToStringFast());

        var combined2 = (PseudoFlagsEnum)(8 | 16); // 24
        Assert.Equal("24", combined2.ToStringFast());
    }

    [Fact]
    public void PseudoFlagsEnum_DefinedPowerOfTwo_ReturnsName()
    {
        Assert.Equal(nameof(PseudoFlagsEnum.One), PseudoFlagsEnum.One.ToStringFast());
        Assert.Equal(nameof(PseudoFlagsEnum.Two), PseudoFlagsEnum.Two.ToStringFast());
        Assert.Equal(nameof(PseudoFlagsEnum.Four), PseudoFlagsEnum.Four.ToStringFast());
        Assert.Equal(nameof(PseudoFlagsEnum.Eight), PseudoFlagsEnum.Eight.ToStringFast());
        Assert.Equal(nameof(PseudoFlagsEnum.Sixteen), PseudoFlagsEnum.Sixteen.ToStringFast());
    }

    [Fact]
    public void PseudoFlagsEnum_Parse_NumericCombinations_WorksCorrectly()
    {
        var parsed1 = PseudoFlagsEnumEnhanced.ParseFast("3");
        Assert.Equal((PseudoFlagsEnum)3, parsed1);

        var parsed2 = PseudoFlagsEnumEnhanced.ParseFast("7");
        Assert.Equal((PseudoFlagsEnum)7, parsed2);

        var parsed3 = PseudoFlagsEnumEnhanced.ParseFast("24");
        Assert.Equal((PseudoFlagsEnum)24, parsed3);
    }

    [Fact]
    public void SingleValueEnum_OnlyValue_ToString()
    {
        var value = SingleValueEnum.OnlyValue;
        Assert.Equal(nameof(SingleValueEnum.OnlyValue), value.ToStringFast());
        Assert.Equal(value.ToString(), value.ToStringFast());
    }

    [Fact]
    public void SingleValueEnum_Parse_Name_Works()
    {
        var parsed = SingleValueEnumEnhanced.ParseFast(nameof(SingleValueEnum.OnlyValue));
        Assert.Equal(SingleValueEnum.OnlyValue, parsed);
        Assert.Equal(42, (int)parsed);
    }

    [Fact]
    public void SingleValueEnum_Parse_Numeric_Works()
    {
        var parsed = SingleValueEnumEnhanced.ParseFast("42");
        Assert.Equal(SingleValueEnum.OnlyValue, parsed);
    }

    [Fact]
    public void SingleValueEnum_UndefinedValue_ReturnsNumeric()
    {
        var undefined = (SingleValueEnum)100;
        Assert.Equal("100", undefined.ToStringFast());
        Assert.Equal(undefined.ToString(), undefined.ToStringFast());
    }

    [Fact]
    public void EmptyEnum_Zero_ToString()
    {
        var nothing = EmptyEnum.Nothing;
        Assert.Equal(nameof(EmptyEnum.Nothing), nothing.ToStringFast());
        Assert.Equal(nothing.ToString(), nothing.ToStringFast());
    }

    [Fact]
    public void EmptyEnum_Parse_Name_Works()
    {
        var parsed = EmptyEnumEnhanced.ParseFast(nameof(EmptyEnum.Nothing));
        Assert.Equal(EmptyEnum.Nothing, parsed);
        Assert.Equal(0, (int)parsed);
    }

    [Fact]
    public void EmptyEnum_Parse_Zero_Works()
    {
        var parsed = EmptyEnumEnhanced.ParseFast("0");
        Assert.Equal(EmptyEnum.Nothing, parsed);
    }

    [Fact]
    public void AllNonFlagEnums_ParseCaseInsensitive_WorksCorrectly()
    {
        // Sequential
        var seq = SequentialEnumEnhanced.ParseFast("third", ignoreCase: true);
        Assert.Equal(SequentialEnum.Third, seq);

        // Gapped
        var gap = GappedEnumEnhanced.ParseFast("MEDIUM", ignoreCase: true);
        Assert.Equal(GappedEnum.Medium, gap);

        // Negative
        var neg = NegativeValuesEnumEnhanced.ParseFast("positive", ignoreCase: true);
        Assert.Equal(NegativeValuesEnum.Positive, neg);

        // Pseudo
        var pseudo = PseudoFlagsEnumEnhanced.ParseFast("EIGHT", ignoreCase: true);
        Assert.Equal(PseudoFlagsEnum.Eight, pseudo);
    }

    [Fact]
    public void AllNonFlagEnums_TryParse_ValidNames_ReturnsTrue()
    {
        bool success1 = SequentialEnumEnhanced.TryParseFast(nameof(SequentialEnum.Fourth), false, out var result1);
        Assert.True(success1);
        Assert.Equal(SequentialEnum.Fourth, result1);

        bool success2 = GappedEnumEnhanced.TryParseFast(nameof(GappedEnum.Large), false, out var result2);
        Assert.True(success2);
        Assert.Equal(GappedEnum.Large, result2);

        bool success3 = DuplicateValuesEnumEnhanced.TryParseFast(nameof(DuplicateValuesEnum.ThirdAlias), false, out var result3);
        Assert.True(success3);
        Assert.Equal(DuplicateValuesEnum.Third, result3);
    }

    [Fact]
    public void AllNonFlagEnums_TryParse_InvalidNames_ReturnsFalse()
    {
        bool success1 = SequentialEnumEnhanced.TryParseFast("InvalidName", false, out _);
        Assert.False(success1);

        bool success2 = GappedEnumEnhanced.TryParseFast("NotAValue", false, out _);
        Assert.False(success2);

        bool success3 = NegativeValuesEnumEnhanced.TryParseFast("FakeValue", false, out _);
        Assert.False(success3);
    }

    [Fact]
    public void AllNonFlagEnums_TryParse_NumericValid_ReturnsTrue()
    {
        bool success1 = SequentialEnumEnhanced.TryParseFast("3", false, out var result1);
        Assert.True(success1);
        Assert.Equal(SequentialEnum.Third, result1);

        bool success2 = GappedEnumEnhanced.TryParseFast("100", false, out var result2);
        Assert.True(success2);
        Assert.Equal(GappedEnum.Large, result2);

        bool success3 = NegativeValuesEnumEnhanced.TryParseFast("-100", false, out var result3);
        Assert.True(success3);
        Assert.Equal(NegativeValuesEnum.Negative, result3);
    }

    [Fact]
    public void AllNonFlagEnums_TryParse_NumericUndefined_ReturnsTrue()
    {
        bool success1 = SequentialEnumEnhanced.TryParseFast("999", false, out var result1);
        Assert.True(success1);
        Assert.Equal((SequentialEnum)999, result1);

        bool success2 = GappedEnumEnhanced.TryParseFast("42", false, out var result2);
        Assert.True(success2);
        Assert.Equal((GappedEnum)42, result2);
    }

    [Fact]
    public void AllNonFlagEnums_RoundTrip_AllDefinedValues()
    {
        // Sequential
        foreach (var value in SequentialEnumEnhanced.GetValuesFast())
        {
            string? toString = value.ToStringFast();
            Assert.NotNull(toString);

            if (Enum.IsDefined(typeof(SequentialEnum), value))
            {
                var parsed = SequentialEnumEnhanced.ParseFast(toString);
                Assert.Equal(value, parsed);
            }
        }

        // Gapped
        foreach (var value in GappedEnumEnhanced.GetValuesFast())
        {
            string? toString = value.ToStringFast();
            Assert.NotNull(toString);

            if (Enum.IsDefined(typeof(GappedEnum), value))
            {
                var parsed = GappedEnumEnhanced.ParseFast(toString);
                Assert.Equal(value, parsed);
            }
        }
    }

    [Fact]
    public void NonFlagEnum_ParseWithWhitespace_WorksCorrectly()
    {
        var parsed1 = SequentialEnumEnhanced.ParseFast(" First ");
        Assert.Equal(SequentialEnum.First, parsed1);

        var parsed2 = GappedEnumEnhanced.ParseFast("  Medium  ");
        Assert.Equal(GappedEnum.Medium, parsed2);

        var parsed3 = NegativeValuesEnumEnhanced.ParseFast("\tPositive\t");
        Assert.Equal(NegativeValuesEnum.Positive, parsed3);
    }

    [Fact]
    public void NonFlagEnum_ParseNumericWithWhitespace_WorksCorrectly()
    {
        var parsed1 = SequentialEnumEnhanced.ParseFast(" 2 ");
        Assert.Equal(SequentialEnum.Second, parsed1);

        var parsed2 = GappedEnumEnhanced.ParseFast("  50  ");
        Assert.Equal(GappedEnum.Medium, parsed2);
    }

    [Fact]
    public void NonFlagEnum_ParseWithPlusSign_WorksForPositive()
    {
        var parsed1 = SequentialEnumEnhanced.ParseFast("+3");
        Assert.Equal(SequentialEnum.Third, parsed1);

        var parsed2 = NegativeValuesEnumEnhanced.ParseFast("+100");
        Assert.Equal(NegativeValuesEnum.Positive, parsed2);
    }

    [Fact]
    public void NonFlagEnum_ParseInvalidFormat_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => SequentialEnumEnhanced.ParseFast(""));
        Assert.Throws<ArgumentException>(() => SequentialEnumEnhanced.ParseFast("   "));
        Assert.Throws<ArgumentException>(() => SequentialEnumEnhanced.ParseFast("Not, A, Valid, Enum"));
        Assert.Throws<ArgumentException>(() => GappedEnumEnhanced.ParseFast("InvalidValue"));
    }

    [Fact]
    public void NonFlagEnum_TryParseInvalidFormat_ReturnsFalse()
    {
        Assert.False(SequentialEnumEnhanced.TryParseFast("", false, out _));
        Assert.False(SequentialEnumEnhanced.TryParseFast("   ", false, out _));
        Assert.False(GappedEnumEnhanced.TryParseFast("1.5", false, out _));
        Assert.False(NegativeValuesEnumEnhanced.TryParseFast("not_a_number", false, out _));
    }
}
