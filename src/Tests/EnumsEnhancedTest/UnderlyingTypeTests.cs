namespace EnumsEnhancedTest;

/// <summary>
/// Tests for enums with different underlying types (byte, sbyte, short, int, long, ulong)
/// </summary>
public class UnderlyingTypeTests
{
    [Fact]
    public void ByteEnum_MaxValue_HandledCorrectly()
    {
        var max = GappedEnum.Max; // 255

        Assert.Equal(byte.MaxValue, (byte)max);
        Assert.Equal(max.ToString(), max.ToStringFast());
        Assert.Equal("Max", max.ToStringFast());
    }

    [Fact]
    public void ByteEnum_NumericParsing_WorksWithinRange()
    {
        var parsed = GappedEnumEnhanced.ParseFast("255");
        Assert.Equal(GappedEnum.Max, parsed);

        var parsed2 = GappedEnumEnhanced.ParseFast("0");
        Assert.Equal(GappedEnum.Min, parsed2);
    }

    [Fact]
    public void SByteEnum_NegativeValues_WorkCorrectly()
    {
        var neg = SignedByteFlagsEnum.NegativeBit; // -128

        Assert.Equal(sbyte.MinValue, (sbyte)neg);
        Assert.Equal(neg.ToString(), neg.ToStringFast());
    }

    [Fact]
    public void SByteEnum_NumericParsing_HandlesNegative()
    {
        var parsed = SignedByteFlagsEnumEnhanced.ParseFast("-128");
        Assert.Equal(SignedByteFlagsEnum.NegativeBit, parsed);
    }

    [Fact]
    public void ShortEnum_NegativeValues_WorkCorrectly()
    {
        var neg = TestEnum2.Test1; // -2

        Assert.Equal((short)-2, (short)neg);
        Assert.Equal(neg.ToString(), neg.ToStringFast());
        Assert.Equal("Test1", neg.ToStringFast());
    }

    [Fact]
    public void ShortEnum_Parse_NegativeWithPlus()
    {
        var pos = TestEnum2Enhanced.ParseFast("+1");
        Assert.Equal(TestEnum2.Test2, pos);

        var neg = TestEnum2Enhanced.ParseFast("-2");
        Assert.Equal(TestEnum2.Test1, neg);
    }

    [Fact]
    public void IntEnum_ExtremeValues_WorkCorrectly()
    {
        var min = NegativeValuesEnum.VeryNegative;
        var max = NegativeValuesEnum.VeryPositive;

        Assert.Equal(int.MinValue, (int)min);
        Assert.Equal(int.MaxValue, (int)max);

        Assert.Equal(min.ToString(), min.ToStringFast());
        Assert.Equal(max.ToString(), max.ToStringFast());
    }

    [Fact]
    public void IntEnum_Parse_ExtremeValues()
    {
        var min = NegativeValuesEnumEnhanced.ParseFast(int.MinValue.ToString());
        Assert.Equal(NegativeValuesEnum.VeryNegative, min);

        var max = NegativeValuesEnumEnhanced.ParseFast(int.MaxValue.ToString());
        Assert.Equal(NegativeValuesEnum.VeryPositive, max);
    }

    [Fact]
    public void LongEnum_HighBits_WorkCorrectly()
    {
        var highBit = LongFlagsEnum.Bit62;

        Assert.Equal(1L << 62, (long)highBit);
        Assert.Equal(highBit.ToString(), highBit.ToStringFast());
    }

    [Fact]
    public void LongEnum_NegativeBit_WorksCorrectly()
    {
        var negBit = LongFlagsEnum.HighBit; // long.MinValue

        Assert.Equal(long.MinValue, (long)negBit);
        Assert.Equal(negBit.ToString(), negBit.ToStringFast());
    }

    [Fact]
    public void LongEnum_Parse_LargeValues()
    {
        var large = LongFlagsEnumEnhanced.ParseFast((1L << 50).ToString());
        Assert.Equal(LongFlagsEnum.Bit50, large);
    }

    [Fact]
    public void LongEnum_FlagCombination_WorksCorrectly()
    {
        var combo = LongFlagsEnum.Bit0 | LongFlagsEnum.Bit10 | LongFlagsEnum.Bit20;

        string? result = combo.ToStringFast();
        string? expected = combo.ToString();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ULongEnum_MaxValue_WorksCorrectly()
    {
        var max = UlongEnum.Max;

        Assert.Equal(ulong.MaxValue, (ulong)max);
        Assert.Equal(max.ToString(), max.ToStringFast());
    }

    [Fact]
    public void ULongEnum_Parse_VeryLargeValue()
    {
        var parsed = UlongEnumEnhanced.ParseFast(ulong.MaxValue.ToString());
        Assert.Equal(UlongEnum.Max, parsed);
    }

    [Fact]
    public void ByteFlags_AllBitsSet_DecomposesCorrectly()
    {
        var all = ByteFlagsEnum.All; // 0xFF

        string? result = all.ToStringFast();
        string? expected = all.ToString();

        Assert.Equal(expected, result);
        Assert.Equal("All", result);
    }

    [Fact]
    public void ByteFlags_NibbleComposites_WorkCorrectly()
    {
        var lower = ByteFlagsEnum.LowerNibble;
        var upper = ByteFlagsEnum.UpperNibble;

        Assert.Equal("LowerNibble", lower.ToStringFast());
        Assert.Equal("UpperNibble", upper.ToStringFast());
        Assert.Equal(lower.ToString(), lower.ToStringFast());
        Assert.Equal(upper.ToString(), upper.ToStringFast());
    }

    [Fact]
    public void AllUnderlyingTypes_IsDefined_WorksCorrectly()
    {
        Assert.True(GappedEnumEnhanced.IsDefinedFast(GappedEnum.Large));
        Assert.True(SignedByteFlagsEnumEnhanced.IsDefinedFast(SignedByteFlagsEnum.Bit0));
        Assert.True(LongFlagsEnumEnhanced.IsDefinedFast(LongFlagsEnum.Bit40));
        Assert.True(UlongEnumEnhanced.IsDefinedFast(UlongEnum.Medium));
        Assert.True(TestEnum2Enhanced.IsDefinedFast(TestEnum2.Test1));

        Assert.False(GappedEnumEnhanced.IsDefinedFast((GappedEnum)99));
        Assert.False(TestEnum2Enhanced.IsDefinedFast((TestEnum2)50));
    }

    [Fact]
    public void AllUnderlyingTypes_GetValues_ReturnsCorrectCount()
    {
        var byteValues = GappedEnumEnhanced.GetValuesFast();
        var sbyteValues = SignedByteFlagsEnumEnhanced.GetValuesFast();
        var shortValues = TestEnum2Enhanced.GetValuesFast();
        var longValues = LongFlagsEnumEnhanced.GetValuesFast();
        var ulongValues = UlongEnumEnhanced.GetValuesFast();

        Assert.Equal(Enum.GetValues(typeof(GappedEnum)).Length, byteValues.Length);
        Assert.Equal(Enum.GetValues(typeof(SignedByteFlagsEnum)).Length, sbyteValues.Length);
        Assert.Equal(Enum.GetValues(typeof(TestEnum2)).Length, shortValues.Length);
        Assert.Equal(Enum.GetValues(typeof(LongFlagsEnum)).Length, longValues.Length);
        Assert.Equal(Enum.GetValues(typeof(UlongEnum)).Length, ulongValues.Length);
    }

    [Fact]
    public void AllUnderlyingTypes_RoundTrip_WorksCorrectly()
    {
        // Byte
        var byteVal = GappedEnum.Medium;
        var byteParsed = GappedEnumEnhanced.ParseFast(byteVal.ToStringFast()!);
        Assert.Equal(byteVal, byteParsed);

        // SByte
        var sbyteVal = SignedByteFlagsEnum.Bit3;
        var sbyteParsed = SignedByteFlagsEnumEnhanced.ParseFast(sbyteVal.ToStringFast()!);
        Assert.Equal(sbyteVal, sbyteParsed);

        // Short
        var shortVal = TestEnum2.Test1;
        var shortParsed = TestEnum2Enhanced.ParseFast(shortVal.ToStringFast()!);
        Assert.Equal(shortVal, shortParsed);

        // Long
        var longVal = LongFlagsEnum.Bit30;
        var longParsed = LongFlagsEnumEnhanced.ParseFast(longVal.ToStringFast()!);
        Assert.Equal(longVal, longParsed);

        // ULong
        var ulongVal = UlongEnum.Large;
        var ulongParsed = UlongEnumEnhanced.ParseFast(ulongVal.ToStringFast()!);
        Assert.Equal(ulongVal, ulongParsed);
    }
}
