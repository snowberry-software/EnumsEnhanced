namespace EnumsEnhancedTest;

/// <summary>
/// Tests for complex edge cases including duplicates, composites, and unusual patterns
/// </summary>
public class ComplexEdgeCaseTests
{
    [Fact]
    public void DuplicateValues_NonFlag_GetNameReturnsFirst()
    {
        var value = DuplicateValuesEnum.Second; // Value = 2
        var alias = DuplicateValuesEnum.SecondAlias; // Also value = 2

        // GetName should return first defined name
        Assert.Equal(Enum.GetName(typeof(DuplicateValuesEnum), value),
                     value.GetNameFast(false));
        Assert.Equal(nameof(DuplicateValuesEnum.Second), value.GetNameFast(false));
        Assert.Equal(nameof(DuplicateValuesEnum.Second), alias.GetNameFast(false));
    }

    [Fact]
    public void DuplicateValues_NonFlag_ToStringReturnsFirst()
    {
        var value = DuplicateValuesEnum.Third; // Value = 3

        // ToString should return last defined name
        Assert.Equal(value.ToString(), value.ToStringFast());
        Assert.Equal(nameof(DuplicateValuesEnum.Third), value.ToStringFast());
    }

    [Fact]
    public void DuplicateValues_NonFlag_AllNamesParseable()
    {
        var parsed1 = DuplicateValuesEnumEnhanced.ParseFast(nameof(DuplicateValuesEnum.Second));
        var parsed2 = DuplicateValuesEnumEnhanced.ParseFast(nameof(DuplicateValuesEnum.SecondAlias));

        Assert.Equal(parsed1, parsed2);
        Assert.Equal(DuplicateValuesEnum.Second, parsed1);
        Assert.Equal((int)DuplicateValuesEnum.Second, (int)parsed2);
    }

    [Fact]
    public void DuplicateValues_Flags_GetNameReturnsFirst()
    {
        var value = DuplicateFlagsEnum.Flag1;
        var alias = DuplicateFlagsEnum.Flag1Alias;

        Assert.Equal(nameof(DuplicateFlagsEnum.Flag1), value.GetNameFast(false));
        Assert.Equal(nameof(DuplicateFlagsEnum.Flag1), alias.GetNameFast(false));
    }

    [Fact]
    public void DuplicateValues_Flags_CompositeWithAlias()
    {
        var combined = DuplicateFlagsEnum.Combined; // Explicit name for 3
        var alias = DuplicateFlagsEnum.CombinedAlias; // Also 3

        // Both should return the composite name (last defined)
        Assert.Equal(combined.ToString(), combined.ToStringFast());
        Assert.Equal(alias.ToString(), alias.ToStringFast());
        Assert.Equal(nameof(DuplicateFlagsEnum.CombinedAlias), combined.ToStringFast());
    }

    [Fact]
    public void SingleValue_Enum_WorksCorrectly()
    {
        var value = SingleValueEnum.OnlyValue;

        Assert.Equal(value.ToString(), value.ToStringFast());
        Assert.Equal(nameof(SingleValueEnum.OnlyValue), value.ToStringFast());
        Assert.Equal(Enum.GetName(typeof(SingleValueEnum), value), value.GetNameFast(false));
    }

    [Fact]
    public void SingleValue_GetValues_ReturnsOne()
    {
        var values = SingleValueEnumEnhanced.GetValuesFast();
        Assert.Single(values);
        Assert.Equal(SingleValueEnum.OnlyValue, values[0]);
    }

    [Fact]
    public void EmptyEnum_OnlyZero_WorksCorrectly()
    {
        var nothing = EmptyEnum.Nothing;

        Assert.Equal(nameof(EmptyEnum.Nothing), nothing.ToStringFast());
        Assert.Equal(nothing.ToString(), nothing.ToStringFast());
    }

    [Fact]
    public void PseudoFlags_NotActualFlags_NoDecomposition()
    {
        // This enum has power-of-2 values but no [Flags] attribute
        var combined = (PseudoFlagsEnum)((int)PseudoFlagsEnum.One | (int)PseudoFlagsEnum.Two);

        // Should return numeric, not decompose
        Assert.Equal(combined.ToString(), combined.ToStringFast());
        Assert.Equal("3", combined.ToStringFast());
    }

    [Fact]
    public void PseudoFlags_DefinedValues_ReturnNames()
    {
        Assert.Equal(nameof(PseudoFlagsEnum.Four), PseudoFlagsEnum.Four.ToStringFast());
        Assert.Equal(nameof(PseudoFlagsEnum.Sixteen), PseudoFlagsEnum.Sixteen.ToStringFast());
        Assert.Equal(PseudoFlagsEnum.Four.ToString(), PseudoFlagsEnum.Four.ToStringFast());
    }

    [Fact]
    public void SparseFlags_OnlyDefinedBits_Decompose()
    {
        var combo = SparseFlagsEnum.Bit0 | SparseFlagsEnum.Bit10 | SparseFlagsEnum.Bit20;

        string? result = combo.ToStringFast();
        string? expected = combo.ToString();

        Assert.Equal(expected, result);
        Assert.Contains(", ", result);
    }

    [Fact]
    public void SparseFlags_IntermediateBits_IncludeNumeric()
    {
        // Set a bit that's not defined (e.g., Bit7)
        var withExtra = (SparseFlagsEnum)((int)SparseFlagsEnum.Bit0 | (1 << 7));

        string? result = withExtra.ToStringFast();
        string? expected = withExtra.ToString();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void OverlappingFlags_NamedComposite_PrefersCompositeName()
    {
        var abc = OverlappingFlagsEnum.ABC;

        // Should return "ABC", not "A, B, C"
        Assert.Equal(nameof(OverlappingFlagsEnum.ABC), abc.ToStringFast());
        Assert.Equal(abc.ToString(), abc.ToStringFast());
    }

    [Fact]
    public void OverlappingFlags_Parse_CanUseCompositeOrComponents()
    {
        var parsed1 = OverlappingFlagsEnumEnhanced.ParseFast(nameof(OverlappingFlagsEnum.ABC));
        var parsed2 = OverlappingFlagsEnumEnhanced.ParseFast($"{nameof(OverlappingFlagsEnum.A)}, {nameof(OverlappingFlagsEnum.B)}, {nameof(OverlappingFlagsEnum.C)}");

        Assert.Equal(parsed1, parsed2);
        Assert.Equal(OverlappingFlagsEnum.ABC, parsed1);
    }

    [Fact]
    public void OverlappingFlags_MultipleComposites_UsesCorrectName()
    {
        // Both ABC and BCD are defined
        var abc = OverlappingFlagsEnum.ABC;
        var bcd = OverlappingFlagsEnum.BCD;

        Assert.NotEqual(abc.ToStringFast(), bcd.ToStringFast());
        Assert.Equal(nameof(OverlappingFlagsEnum.ABC), abc.ToStringFast());
        Assert.Equal(nameof(OverlappingFlagsEnum.BCD), bcd.ToStringFast());
    }

    [Fact]
    public void GappedEnum_UndefinedInGap_ReturnsNumeric()
    {
        var gapped = (GappedEnum)25; // Between Small (10) and Medium (50)

        Assert.Equal(gapped.ToString(), gapped.ToStringFast());
        Assert.Equal("25", gapped.ToStringFast());
    }

    [Fact]
    public void GappedEnum_Parse_WorksForGaps()
    {
        var parsed = GappedEnumEnhanced.ParseFast("25");
        Assert.Equal((GappedEnum)25, parsed);
    }

    [Fact]
    public void MultipleDuplicates_TripleValue_ReturnsFirst()
    {
        // Third, ThirdAlias, and ThirdAlias2 all equal 3
        var value = DuplicateValuesEnum.Third;

        Assert.Equal(nameof(DuplicateValuesEnum.Third), value.ToStringFast());
        Assert.Equal(value.ToString(), value.ToStringFast());
    }

    [Fact]
    public void MultipleDuplicates_AllNamesParse()
    {
        string[] names = new[]
        {
            nameof(DuplicateValuesEnum.Third),
            nameof(DuplicateValuesEnum.ThirdAlias),
            nameof(DuplicateValuesEnum.ThirdAlias2)
        };

        foreach (string? name in names)
        {
            var parsed = DuplicateValuesEnumEnhanced.ParseFast(name);
            Assert.Equal(3, (int)parsed);
        }
    }

    [Fact]
    public void GetNames_IncludesAllDuplicates()
    {
        string[] names = DuplicateValuesEnumEnhanced.GetNamesFast();

        Assert.Contains(nameof(DuplicateValuesEnum.Second), names);
        Assert.Contains(nameof(DuplicateValuesEnum.SecondAlias), names);
        Assert.Contains(nameof(DuplicateValuesEnum.Third), names);
        Assert.Contains(nameof(DuplicateValuesEnum.ThirdAlias), names);
        Assert.Contains(nameof(DuplicateValuesEnum.ThirdAlias2), names);
    }

    [Fact]
    public void GetValues_IncludesAllDuplicates()
    {
        var values = DuplicateValuesEnumEnhanced.GetValuesFast();

        // Should have 6 entries even though some have duplicate values
        Assert.Equal(6, values.Length);
    }

    [Fact]
    public void IsDefined_Name_TrueForAllDuplicates()
    {
        Assert.True(DuplicateValuesEnumEnhanced.IsDefinedFast(nameof(DuplicateValuesEnum.Second)));
        Assert.True(DuplicateValuesEnumEnhanced.IsDefinedFast(nameof(DuplicateValuesEnum.SecondAlias)));
        Assert.True(DuplicateValuesEnumEnhanced.IsDefinedFast(nameof(DuplicateValuesEnum.Third)));
        Assert.True(DuplicateValuesEnumEnhanced.IsDefinedFast(nameof(DuplicateValuesEnum.ThirdAlias)));
        Assert.True(DuplicateValuesEnumEnhanced.IsDefinedFast(nameof(DuplicateValuesEnum.ThirdAlias2)));
    }

    [Fact]
    public void IsDefined_Value_TrueForDefinedValues()
    {
        // Even though value 2 has two names, IsDefined(2) is true
        Assert.True(DuplicateValuesEnumEnhanced.IsDefinedFast((DuplicateValuesEnum)2));
        Assert.True(DuplicateValuesEnumEnhanced.IsDefinedFast((DuplicateValuesEnum)3));
        Assert.False(DuplicateValuesEnumEnhanced.IsDefinedFast((DuplicateValuesEnum)99));
    }

    [Fact]
    public void CompositeFlags_HasFlag_WorksForComponents()
    {
        var composite = CompositeFlagsEnum.ReadWriteExecute;

        Assert.True(composite.HasFlagFast(CompositeFlagsEnum.Read));
        Assert.True(composite.HasFlagFast(CompositeFlagsEnum.Write));
        Assert.True(composite.HasFlagFast(CompositeFlagsEnum.Execute));
        Assert.False(composite.HasFlagFast(CompositeFlagsEnum.Delete));

        // Also works for other composites
        Assert.True(composite.HasFlagFast(CompositeFlagsEnum.ReadWrite));
        Assert.True(composite.HasFlagFast(CompositeFlagsEnum.ReadExecute));
    }

    [Fact]
    public void ZeroValueInFlags_NeverAppearsInDecomposition()
    {
        var combo = ByteFlagsEnum.None | ByteFlagsEnum.Bit1 | ByteFlagsEnum.Bit2;
        string? result = combo.ToStringFast();

        Assert.DoesNotContain(nameof(ByteFlagsEnum.None), result);
        Assert.Contains(nameof(ByteFlagsEnum.Bit1), result);
        Assert.Contains(nameof(ByteFlagsEnum.Bit2), result);
    }

    [Fact]
    public void AllEnums_CaseInsensitiveParsing_Works()
    {
        var sequential = SequentialEnumEnhanced.ParseFast("FIRST", ignoreCase: true);
        Assert.Equal(SequentialEnum.First, sequential);

        var gapped = GappedEnumEnhanced.ParseFast("large", ignoreCase: true);
        Assert.Equal(GappedEnum.Large, gapped);

        var flags = ByteFlagsEnumEnhanced.ParseFast("bit0, BIT1", ignoreCase: true);
        Assert.Equal(ByteFlagsEnum.Bit0 | ByteFlagsEnum.Bit1, flags);
    }

    [Fact]
    public void AllEnums_TryParse_MatchesEnumTryParse()
    {
        // Sequential
        bool success1 = SequentialEnumEnhanced.TryParseFast(nameof(SequentialEnum.Third), false, out var parsed1);
        bool expected1 = Enum.TryParse(typeof(SequentialEnum), nameof(SequentialEnum.Third), false, out object? exp1);
        Assert.Equal(expected1, success1);
        Assert.Equal(exp1, parsed1);

        // Gapped
        bool success2 = GappedEnumEnhanced.TryParseFast("50", false, out var parsed2);
        bool expected2 = Enum.TryParse(typeof(GappedEnum), "50", false, out object? exp2);
        Assert.Equal(expected2, success2);
        Assert.Equal(exp2, parsed2);

        // Flags
        bool success3 = ByteFlagsEnumEnhanced.TryParseFast("InvalidName", false, out var parsed3);
        bool expected3 = Enum.TryParse(typeof(ByteFlagsEnum), "InvalidName", false, out object? exp3);
        Assert.Equal(expected3, success3);
    }
}
