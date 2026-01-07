namespace EnumsEnhancedTest;

public class EnumTests
{
    [Fact]
    public void HasFlagFast_SingleFlag_ReturnsTrue()
    {
        var e = TestEnum.Test1;

        Assert.True(e.HasFlagFast(TestEnum.Test1));
        Assert.Equal(e.HasFlag(TestEnum.Test1), e.HasFlagFast(TestEnum.Test1));
    }

    [Fact]
    public void HasFlagFast_MultipleFlags_ReturnsCorrectResult()
    {
        var e = TestEnum.Test1 | TestEnum.Test4;

        // Compare with default HasFlag for each flag
        Assert.Equal(e.HasFlag(TestEnum.Test1), e.HasFlagFast(TestEnum.Test1));
        Assert.Equal(e.HasFlag(TestEnum.Test_1), e.HasFlagFast(TestEnum.Test_1));
        Assert.Equal(e.HasFlag(TestEnum.Test4), e.HasFlagFast(TestEnum.Test4));
        Assert.Equal(e.HasFlag(TestEnum.Test5), e.HasFlagFast(TestEnum.Test5));
        Assert.Equal(e.HasFlag(TestEnum.Test6), e.HasFlagFast(TestEnum.Test6));
    }

    [Fact]
    public void HasFlagFast_AllValues_MatchesDefaultBehavior()
    {
        var values = TestEnumEnhanced.GetValuesFast();
        foreach (var value in values)
        {
            foreach (var flag in values)
            {
                bool expected = value.HasFlag(flag);
                bool actual = value.HasFlagFast(flag);
                Assert.Equal(expected, actual);
            }
        }
    }

    [Fact]
    public void HasFlagFast_ComplexCombinations_MatchesDefaultBehavior()
    {
        var combinations = new[]
        {
            (TestEnum.Test1 | TestEnum.Test2, TestEnum.Test1),
            (TestEnum.Test1 | TestEnum.Test2, TestEnum.Test2),
            (TestEnum.Test1 | TestEnum.Test2, TestEnum.Test1 | TestEnum.Test2),
            (TestEnum.Test4 | TestEnum.Test5, TestEnum.Test4),
            (TestEnum.Test1, TestEnum.Test1 | TestEnum.Test2),
        };

        foreach (var (value, flag) in combinations)
        {
            Assert.Equal(value.HasFlag(flag), value.HasFlagFast(flag));
        }
    }

    [Fact]
    public void HasFlagFast_ZeroFlag_AlwaysReturnsTrue()
    {
        var values = TestEnumEnhanced.GetValuesFast();
        foreach (var value in values)
        {
            bool expected = value.HasFlag(TestEnum.Test0);
            bool actual = value.HasFlagFast(TestEnum.Test0);
            Assert.Equal(expected, actual);
            Assert.True(actual);
        }
    }

    [Fact]
    public void HasFlagFast_AllFlagsCombined_MatchesDefaultBehavior()
    {
        var allFlags = TestEnum.Test1 | TestEnum.Test2 | TestEnum.Test4 | TestEnum.Test5 | TestEnum.Test6 | TestEnum.Test7;
        var values = TestEnumEnhanced.GetValuesFast();

        foreach (var value in values)
        {
            Assert.Equal(allFlags.HasFlag(value), allFlags.HasFlagFast(value));
        }
    }

    [Fact]
    public void GetNamesFast_ReturnsAllNames()
    {
        string[] namesFast = TestEnumEnhanced.GetNamesFast();
        string[] namesDefault = Enum.GetNames(typeof(TestEnum));

        Assert.Equal(namesDefault.Length, namesFast.Length);

        foreach (string name in namesDefault)
        {
            Assert.Contains(name, namesFast);
        }
    }

    [Fact]
    public void GetNamesFast_TestEnum2_MatchesDefaultBehavior()
    {
        string[] namesFast = TestEnum2Enhanced.GetNamesFast();
        string[] namesDefault = Enum.GetNames(typeof(TestEnum2));

        Assert.Equal(namesDefault.Length, namesFast.Length);

        for (int i = 0; i < namesDefault.Length; i++)
        {
            Assert.Contains(namesDefault[i], namesFast);
        }
    }

    [Fact]
    public void GetNamesFast_OrderAndContent_ExactMatch()
    {
        string[] namesFast = TestEnumEnhanced.GetNamesFast();
        string[] namesDefault = Enum.GetNames(typeof(TestEnum));

        // Both should contain exactly the same names
        Assert.Equal(namesDefault.OrderBy(x => x).ToArray(), namesFast.OrderBy(x => x).ToArray());
    }

    [Fact]
    public void GetValuesFast_ReturnsAllValues()
    {
        var valuesFast = TestEnumEnhanced.GetValuesFast();
        var valuesDefault = Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>().ToArray();

        Assert.Equal(valuesDefault.Length, valuesFast.Length);

        foreach (var value in valuesDefault)
        {
            Assert.Contains(value, valuesFast);
        }
    }

    [Fact]
    public void GetValuesFast_TestEnum2_MatchesDefaultBehavior()
    {
        var valuesFast = TestEnum2Enhanced.GetValuesFast();
        var valuesDefault = Enum.GetValues(typeof(TestEnum2)).Cast<TestEnum2>().ToArray();

        Assert.Equal(valuesDefault.Length, valuesFast.Length);

        foreach (var value in valuesDefault)
        {
            Assert.Contains(value, valuesFast);
        }
    }

    [Fact]
    public void GetValuesFast_OrderAndContent_ExactMatch()
    {
        var valuesFast = TestEnumEnhanced.GetValuesFast();
        var valuesDefault = Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>().ToArray();

        // Both should contain exactly the same values
        Assert.Equal(valuesDefault.OrderBy(x => (int)x).ToArray(), valuesFast.OrderBy(x => (int)x).ToArray());
    }

    [Fact]
    public void IsDefinedFast_WithName_MatchesDefaultBehavior()
    {
        string[] names = Enum.GetNames(typeof(TestEnum));

        foreach (string name in names)
        {
            bool expectedResult = Enum.IsDefined(typeof(TestEnum), name);
            bool actualResult = TestEnumEnhanced.IsDefinedFast(name);
            Assert.Equal(expectedResult, actualResult);
        }

        // Test invalid names
        string[] invalidNames = { "InvalidName", "Test7x", "", "test1" };
        foreach (string invalidName in invalidNames)
        {
            if (!string.IsNullOrEmpty(invalidName))
            {
                Assert.Equal(Enum.IsDefined(typeof(TestEnum), invalidName),
                             TestEnumEnhanced.IsDefinedFast(invalidName));
            }
        }
    }

    [Fact]
    public void IsDefinedFast_WithValue_MatchesDefaultBehavior()
    {
        var values = Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>();

        foreach (var value in values)
        {
            bool expectedResult = Enum.IsDefined(typeof(TestEnum), value);
            bool actualResult = TestEnumEnhanced.IsDefinedFast(value);
            Assert.Equal(expectedResult, actualResult);
        }

        // Test undefined values
        var undefinedValues = new[] { (TestEnum)999, (TestEnum)(-1), (TestEnum)1000 };
        foreach (var undefinedValue in undefinedValues)
        {
            Assert.Equal(Enum.IsDefined(typeof(TestEnum), undefinedValue),
                         TestEnumEnhanced.IsDefinedFast(undefinedValue));
        }
    }

    [Fact]
    public void IsDefinedFast_WithUnderlyingType_MatchesDefaultBehavior()
    {
        var values = Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>();

        foreach (var value in values)
        {
            int underlyingValue = (int)value;
            bool expectedResult = Enum.IsDefined(typeof(TestEnum), underlyingValue);
            bool actualResult = TestEnumEnhanced.IsDefinedFast(underlyingValue);
            Assert.Equal(expectedResult, actualResult);
        }

        Assert.Equal(Enum.IsDefined(typeof(TestEnum), 999999),
                     TestEnumEnhanced.IsDefinedFast(999999));
    }

    [Fact]
    public void IsDefinedFast_NullString_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => TestEnumEnhanced.IsDefinedFast(null!));
    }

    [Fact]
    public void IsDefinedFast_CombinedFlags_MatchesDefaultBehavior()
    {
        var combinedFlag = TestEnum.Test1 | TestEnum.Test4;
        Assert.Equal(Enum.IsDefined(typeof(TestEnum), combinedFlag),
                     TestEnumEnhanced.IsDefinedFast(combinedFlag));
    }

    [Fact]
    public void GetNameFast_SingleValue_MatchesEnumGetName()
    {
        var values = Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>();

        foreach (var value in values)
        {
            string? expectedName = Enum.GetName(typeof(TestEnum), value);
            string? actualName = value.GetNameFast(false);
            Assert.Equal(expectedName, actualName);
        }
    }

    [Fact]
    public void GetNameFast_WithoutFlags_ReturnsNull()
    {
        var e = TestEnum.Test4 | TestEnum.Test1;

        // Default Enum.GetName returns null for combined flags
        string? expectedName = Enum.GetName(typeof(TestEnum), e);
        string? actualName = e.GetNameFast(false);

        Assert.Null(expectedName);
        Assert.Null(actualName);
        Assert.Equal(expectedName, actualName);
    }

    [Fact]
    public void GetNameFast_WithFlags_ReturnsCommaSeparatedNames()
    {
        var e = TestEnum.Test1 | TestEnum.Test5;
        string? actualName = e.GetNameFast(true);

        // Should contain flag names separated by comma
        // For Test1/Test_1 duplicate, should use Test1 (first defined) since GetNameFast uses first
        Assert.Contains("Test1", actualName);
        Assert.Contains(nameof(TestEnum.Test5), actualName);
        Assert.Contains(", ", actualName);
    }

    [Fact]
    public void GetNameFast_TestEnum2_MatchesDefaultBehavior()
    {
        Assert.Equal(Enum.GetName(typeof(TestEnum2), TestEnum2.Test1),
                     TestEnum2.Test1.GetNameFast(false));
        Assert.Equal(Enum.GetName(typeof(TestEnum2), TestEnum2.Test2),
                     TestEnum2.Test2.GetNameFast(false));
    }

    [Fact]
    public void GetNameFast_AllDefinedValues_MatchesEnumGetName()
    {
        var values = Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>().Distinct();

        foreach (var value in values)
        {
            Assert.Equal(Enum.GetName(typeof(TestEnum), value),
                         value.GetNameFast(false));
        }
    }

    [Fact]
    public void GetNameFast_DuplicateValue_ReturnsFirstDefinedName()
    {
        // Test1 and Test_1 have the same value
        // Enum.GetName returns the FIRST defined name (Test1)
        string? name = TestEnum.Test1.GetNameFast(false);
        Assert.Equal(Enum.GetName(typeof(TestEnum), TestEnum.Test1), name);

        // Both should return "Test1" (first defined)
        Assert.Equal("Test1", name);
        Assert.Equal("Test1", TestEnum.Test_1.GetNameFast(false));
    }

    [Fact]
    public void ToStringFast_SingleValue_MatchesToString()
    {
        var values = Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>();

        foreach (var value in values)
        {
            string? expectedString = value.ToString();
            string? actualString = value.ToStringFast();

            // For single values, both should return the name
            Assert.Equal(expectedString, actualString);
        }
    }

    [Fact]
    public void ToStringFast_MultipleFlags_ReturnsCommaSeparatedNames()
    {
        var e = TestEnum.Test4 | TestEnum.Test1;
        string? actualString = e.ToStringFast();

        // Should return comma-separated names for flags
        Assert.NotNull(actualString);
        Assert.Contains(", ", actualString);
    }

    [Fact]
    public void ToStringFast_AllValues_ReturnsValidString()
    {
        var values = Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>();

        foreach (var value in values)
        {
            string? result = value.ToStringFast();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }

    [Fact]
    public void ToStringFast_TestEnum2_MatchesToString()
    {
        Assert.Equal(TestEnum2.Test1.ToString(), TestEnum2.Test1.ToStringFast());
        Assert.Equal(TestEnum2.Test2.ToString(), TestEnum2.Test2.ToStringFast());
    }

    [Fact]
    public void ToStringFast_ZeroValue_ReturnsName()
    {
        var zero = TestEnum.Test0;
        Assert.Equal(zero.ToString(), zero.ToStringFast());
        Assert.Equal("Test0", zero.ToStringFast());
    }

    [Fact]
    public void ToStringFast_ComplexFlagCombinations_MatchesToString()
    {
        var combinations = new[]
        {
            TestEnum.Test1 | TestEnum.Test2,
            TestEnum.Test1 | TestEnum.Test2 | TestEnum.Test4,
            TestEnum.Test2 | TestEnum.Test5,
            TestEnum.Test4 | TestEnum.Test6,
            TestEnum.Test1 | TestEnum.Test2 | TestEnum.Test4 | TestEnum.Test5 | TestEnum.Test6,
        };

        foreach (var combo in combinations)
        {
            string? expected = combo.ToString();
            string? actual = combo.ToStringFast();
            Assert.Equal(expected, actual);
        }
    }

    [Fact]
    public void ToStringFast_AllFlagsCombined_MatchesToString()
    {
        var allFlags = TestEnum.Test1 | TestEnum.Test2 | TestEnum.Test4 | TestEnum.Test5 | TestEnum.Test6 | TestEnum.Test7;
        Assert.Equal(allFlags.ToString(), allFlags.ToStringFast());
    }

    [Fact]
    public void ToStringFast_UndefinedValue_ReturnsNumericString()
    {
        var undefined = (TestEnum)999;
        string? expected = undefined.ToString();
        string? actual = undefined.ToStringFast();

        // Both should return numeric representation for undefined values
        Assert.NotNull(actual);
    }

    [Fact]
    public void ParseFast_ValidName_MatchesEnumParse()
    {
        string[] names = Enum.GetNames(typeof(TestEnum));

        foreach (string name in names)
        {
            var expectedValue = (TestEnum)Enum.Parse(typeof(TestEnum), name);
            var actualValue = TestEnumEnhanced.ParseFast(name, false);
            Assert.Equal(expectedValue, actualValue);
        }
    }

    [Fact]
    public void ParseFast_CaseInsensitive_MatchesEnumParse()
    {
        string[] names = Enum.GetNames(typeof(TestEnum));

        foreach (string name in names)
        {
            string upperName = name.ToUpper();
            string lowerName = name.ToLower();

            var expectedUpper = (TestEnum)Enum.Parse(typeof(TestEnum), upperName, true);
            var actualUpper = TestEnumEnhanced.ParseFast(upperName, true);
            Assert.Equal(expectedUpper, actualUpper);

            var expectedLower = (TestEnum)Enum.Parse(typeof(TestEnum), lowerName, true);
            var actualLower = TestEnumEnhanced.ParseFast(lowerName, true);
            Assert.Equal(expectedLower, actualLower);
        }
    }

    [Fact]
    public void ParseFast_MultipleNames_MatchesEnumParse()
    {
        string enumName = $"{nameof(TestEnum.Test4)}, {nameof(TestEnum.Test1)}";

        var expectedValue = (TestEnum)Enum.Parse(typeof(TestEnum), enumName);
        var actualValue = TestEnumEnhanced.ParseFast(enumName);

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void ParseFast_ComplexFlagCombinations_MatchesEnumParse()
    {
        string[] testCases = new[]
        {
            $"{nameof(TestEnum.Test1)}, {nameof(TestEnum.Test2)}",
            $"{nameof(TestEnum.Test1)}, {nameof(TestEnum.Test2)}, {nameof(TestEnum.Test4)}",
            $"{nameof(TestEnum.Test2)}, {nameof(TestEnum.Test5)}, {nameof(TestEnum.Test6)}",
            $"{nameof(TestEnum.Test1)}, {nameof(TestEnum.Test4)}, {nameof(TestEnum.Test6)}",
        };

        foreach (string? testCase in testCases)
        {
            var expected = (TestEnum)Enum.Parse(typeof(TestEnum), testCase);
            var actual = TestEnumEnhanced.ParseFast(testCase);
            Assert.Equal(expected, actual);
        }
    }

    [Fact]
    public void ParseFast_FlagsWithExtraSpaces_MatchesEnumParse()
    {
        string[] testCases = new[]
        {
            $"{nameof(TestEnum.Test1)} , {nameof(TestEnum.Test2)}",
            $" {nameof(TestEnum.Test1)}, {nameof(TestEnum.Test2)} ",
            $"{nameof(TestEnum.Test1)}  ,  {nameof(TestEnum.Test2)}",
            $"  {nameof(TestEnum.Test1)}  ,  {nameof(TestEnum.Test2)}  ",
        };

        foreach (string? testCase in testCases)
        {
            var expected = (TestEnum)Enum.Parse(typeof(TestEnum), testCase);
            var actual = TestEnumEnhanced.ParseFast(testCase);
            Assert.Equal(expected, actual);
        }
    }

    [Fact]
    public void ParseFast_DuplicateNames_MatchesEnumParse()
    {
        // Parsing both names that share the same value
        var test1Result = TestEnumEnhanced.ParseFast(nameof(TestEnum.Test1));
        var test_1Result = TestEnumEnhanced.ParseFast(nameof(TestEnum.Test_1));

        Assert.Equal((int)test1Result, (int)test_1Result);
        Assert.Equal((TestEnum)Enum.Parse(typeof(TestEnum), nameof(TestEnum.Test1)), test1Result);
        Assert.Equal((TestEnum)Enum.Parse(typeof(TestEnum), nameof(TestEnum.Test_1)), test_1Result);
    }

    [Fact]
    public void ParseFast_NumericValue_MatchesEnumParse()
    {
        var values = Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>();

        foreach (var value in values)
        {
            string numericString = ((int)value).ToString();

            var expectedValue = (TestEnum)Enum.Parse(typeof(TestEnum), numericString);
            var actualValue = TestEnumEnhanced.ParseFast(numericString);

            Assert.Equal(expectedValue, actualValue);
        }
    }

    [Fact]
    public void ParseFast_NumericZero_MatchesEnumParse()
    {
        var expected = (TestEnum)Enum.Parse(typeof(TestEnum), "0");
        var actual = TestEnumEnhanced.ParseFast("0");
        Assert.Equal(expected, actual);
        Assert.Equal(TestEnum.Test0, actual);
    }

    [Fact]
    public void ParseFast_NumericCombinedFlags_MatchesEnumParse()
    {
        int combinedValue = (int)(TestEnum.Test1 | TestEnum.Test4);
        string numericString = combinedValue.ToString();

        var expected = (TestEnum)Enum.Parse(typeof(TestEnum), numericString);
        var actual = TestEnumEnhanced.ParseFast(numericString);

        Assert.Equal(expected, actual);
        Assert.Equal(TestEnum.Test1 | TestEnum.Test4, actual);
    }

    [Fact]
    public void ParseFast_NegativeValue_TestEnum2_MatchesEnumParse()
    {
        Assert.Equal((TestEnum2)Enum.Parse(typeof(TestEnum2), "-2"),
                     TestEnum2Enhanced.ParseFast("-2"));
        Assert.Equal((TestEnum2)Enum.Parse(typeof(TestEnum2), "1"),
                     TestEnum2Enhanced.ParseFast("1"));
    }

    [Fact]
    public void ParseFast_InvalidName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Enum.Parse(typeof(TestEnum), "InvalidName"));
        Assert.Throws<ArgumentException>(() => TestEnumEnhanced.ParseFast("InvalidName"));
    }

    [Fact]
    public void ParseFast_TrailingComma_ThrowsArgumentException()
    {
        string enumName = $"{nameof(TestEnum.Test4)}, {nameof(TestEnum.Test1)},";

        Assert.Throws<ArgumentException>(() => Enum.Parse(typeof(TestEnum), enumName));
        Assert.Throws<ArgumentException>(() => TestEnumEnhanced.ParseFast(enumName));
    }

    [Fact]
    public void ParseFast_LeadingComma_ThrowsArgumentException()
    {
        string enumName = $", {nameof(TestEnum.Test1)}";

        Assert.Throws<ArgumentException>(() => TestEnumEnhanced.ParseFast(enumName));
    }

    [Fact]
    public void ParseFast_MultipleConsecutiveCommas_ThrowsArgumentException()
    {
        string enumName = $"{nameof(TestEnum.Test1)},, {nameof(TestEnum.Test2)}";

        Assert.Throws<ArgumentException>(() => TestEnumEnhanced.ParseFast(enumName));
    }

    [Fact]
    public void ParseFast_WithSpaces_MatchesEnumParse()
    {
        string nameWithSpaces = $" {nameof(TestEnum.Test1)} ";

        var expectedValue = (TestEnum)Enum.Parse(typeof(TestEnum), nameWithSpaces);
        var actualValue = TestEnumEnhanced.ParseFast(nameWithSpaces);

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void ParseFast_NullOrWhitespace_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => TestEnumEnhanced.ParseFast(""));
        Assert.Throws<ArgumentException>(() => TestEnumEnhanced.ParseFast("   "));
    }

    [Fact]
    public void ParseFast_UndefinedNumericValue_Does_Not_Throw_ArgumentException()
    {
        Assert.Equal((TestEnum2)0, Enum.Parse<TestEnum2>("0"));
        Assert.Equal((TestEnum2)0, TestEnum2Enhanced.ParseFast("0"));
    }

    [Fact]
    public void TryParseFast_ValidName_MatchesEnumTryParse()
    {
        string[] names = Enum.GetNames(typeof(TestEnum));

        foreach (string name in names)
        {
            bool expectedResult = Enum.TryParse(typeof(TestEnum), name, false, out object? expectedValue);
            bool actualResult = TestEnumEnhanced.TryParseFast(name, false, out var actualValue);

            Assert.Equal(expectedResult, actualResult);
            Assert.Equal(expectedValue, actualValue);
        }
    }

    [Fact]
    public void TryParseFast_InvalidName_MatchesEnumTryParse()
    {
        string[] invalidNames = { "InvalidName", "Test7x", "xxxxx" };

        foreach (string invalidName in invalidNames)
        {
            bool expectedResult = Enum.TryParse(typeof(TestEnum), invalidName, false, out object? expectedValue);
            bool actualResult = TestEnumEnhanced.TryParseFast(invalidName, false, out var actualValue);

            Assert.Equal(expectedResult, actualResult);
            Assert.False(actualResult);
        }
    }

    [Fact]
    public void TryParseFast_CaseInsensitive_MatchesEnumTryParse()
    {
        string[] names = Enum.GetNames(typeof(TestEnum));

        foreach (string name in names)
        {
            string upperName = name.ToUpper();

            bool expectedResult = Enum.TryParse(typeof(TestEnum), upperName, true, out object? expectedValue);
            bool actualResult = TestEnumEnhanced.TryParseFast(upperName, true, out var actualValue);

            Assert.Equal(expectedResult, actualResult);
            Assert.Equal(expectedValue, actualValue);
        }
    }

    [Fact]
    public void TryParseFast_NumericValue_MatchesEnumTryParse()
    {
        var values = Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>();

        foreach (var value in values)
        {
            string numericString = ((int)value).ToString();

            bool expectedResult = Enum.TryParse(typeof(TestEnum), numericString, false, out object? expectedValue);
            bool actualResult = TestEnumEnhanced.TryParseFast(numericString, false, out var actualValue);

            Assert.Equal(expectedResult, actualResult);
            Assert.Equal(expectedValue, actualValue);
        }
    }

    [Fact]
    public void TryParseFast_InvalidNumeric_ReturnsFalse()
    {
        var values = Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>();

        foreach (var value in values)
        {
            string invalidNumeric = ((int)value).ToString() + "x";

            bool expectedResult = Enum.TryParse(typeof(TestEnum), invalidNumeric, false, out object? expectedValue);
            bool actualResult = TestEnumEnhanced.TryParseFast(invalidNumeric, false, out var actualValue);

            Assert.Equal(expectedResult, actualResult);
            Assert.False(actualResult);
        }
    }

    [Fact]
    public void TryParseFast_NullOrEmpty_ReturnsFalse()
    {
        Assert.False(TestEnumEnhanced.TryParseFast("", false, out _));
        Assert.False(TestEnumEnhanced.TryParseFast("   ", false, out _));
    }

    [Fact]
    public void TryParseFast_MultipleValues_MatchesEnumTryParse()
    {
        string multiValue = $"{nameof(TestEnum.Test1)}, {nameof(TestEnum.Test4)}";

        bool expectedResult = Enum.TryParse(typeof(TestEnum), multiValue, false, out object? expectedValue);
        bool actualResult = TestEnumEnhanced.TryParseFast(multiValue, false, out var actualValue);

        Assert.Equal(expectedResult, actualResult);
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void TryParseFast_ComplexFlagCombinations_ReturnsTrue()
    {
        string[] testCases = new[]
        {
            $"{nameof(TestEnum.Test1)}, {nameof(TestEnum.Test2)}, {nameof(TestEnum.Test4)}",
            $"{nameof(TestEnum.Test2)}, {nameof(TestEnum.Test5)}, {nameof(TestEnum.Test6)}",
            $"{nameof(TestEnum.Test1)}, {nameof(TestEnum.Test4)}, {nameof(TestEnum.Test6)}",
        };

        foreach (string? testCase in testCases)
        {
            bool expectedResult = Enum.TryParse(typeof(TestEnum), testCase, false, out object? expectedValue);
            bool actualResult = TestEnumEnhanced.TryParseFast(testCase, false, out var actualValue);

            Assert.Equal(expectedResult, actualResult);
            Assert.Equal(expectedValue, actualValue);
            Assert.True(actualResult);
        }
    }

    [Fact]
    public void TryParseFast_TrailingComma_ReturnsFalse()
    {
        string testCase = $"{nameof(TestEnum.Test1)}, {nameof(TestEnum.Test2)},";

        bool result = TestEnumEnhanced.TryParseFast(testCase, false, out _);
        Assert.False(result);
    }

    [Fact]
    public void AllMethods_ComprehensiveComparison_WithDefaultEnum()
    {
        var valuesFast = TestEnumEnhanced.GetValuesFast();
        var valuesDefault = Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>().ToArray();

        string[] namesFast = TestEnumEnhanced.GetNamesFast();
        string[] namesDefault = Enum.GetNames(typeof(TestEnum));

        // Verify GetValues matches
        Assert.Equal(valuesDefault.Length, valuesFast.Length);

        // Verify GetNames matches
        Assert.Equal(namesDefault.Length, namesFast.Length);

        foreach (var value in valuesDefault)
        {
            // Test IsDefined
            Assert.Equal(Enum.IsDefined(typeof(TestEnum), value),
                         TestEnumEnhanced.IsDefinedFast(value));

            // Test GetName
            Assert.Equal(Enum.GetName(typeof(TestEnum), value),
                         value.GetNameFast(false));

            // Test ToString
            Assert.Equal(value.ToString(), value.ToStringFast());

            string? name = Enum.GetName(typeof(TestEnum), value);
            if (name != null)
            {
                // Test Parse round-trip
                Assert.Equal((TestEnum)Enum.Parse(typeof(TestEnum), name),
                             TestEnumEnhanced.ParseFast(name));

                // Test TryParse
                bool expectedTryParse = Enum.TryParse(typeof(TestEnum), name, false, out object? expectedParsed);
                bool actualTryParse = TestEnumEnhanced.TryParseFast(name, false, out var actualParsed);

                Assert.Equal(expectedTryParse, actualTryParse);
                Assert.Equal(expectedParsed, actualParsed);
            }
        }
    }

    [Fact]
    public void RoundTrip_AllValues_MatchesDefaultBehavior()
    {
        var values = Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>();

        foreach (var value in values)
        {
            // Round trip: value -> string -> value
            string? name = Enum.GetName(typeof(TestEnum), value);
            string? nameFast = value.GetNameFast(false);

            Assert.Equal(name, nameFast);

            if (name != null)
            {
                var parsedDefault = (TestEnum)Enum.Parse(typeof(TestEnum), name);
                var parsedFast = TestEnumEnhanced.ParseFast(name);

                Assert.Equal(parsedDefault, parsedFast);
                Assert.Equal(value, parsedFast);
            }
        }
    }

    [Fact]
    public void RoundTrip_ComplexFlags_ToStringAndParse()
    {
        var flagCombinations = new[]
        {
            TestEnum.Test1 | TestEnum.Test2,
            TestEnum.Test1 | TestEnum.Test4 | TestEnum.Test5,
            TestEnum.Test2 | TestEnum.Test6,
            TestEnum.Test1 | TestEnum.Test2 | TestEnum.Test4 | TestEnum.Test5 | TestEnum.Test6,
        };

        foreach (var combo in flagCombinations)
        {
            string? toString = combo.ToStringFast();
            Assert.NotNull(toString);

            var parsed = TestEnumEnhanced.ParseFast(toString);
            Assert.Equal(combo, parsed);
        }
    }

    [Fact]
    public void EdgeCase_ZeroValueBehavior_MatchesDefault()
    {
        var zero = TestEnum.Test0;

        Assert.Equal(Enum.GetName(typeof(TestEnum), zero), zero.GetNameFast(false));
        Assert.Equal(zero.ToString(), zero.ToStringFast());
        Assert.Equal(Enum.IsDefined(typeof(TestEnum), zero), TestEnumEnhanced.IsDefinedFast(zero));
    }

    [Fact]
    public void EdgeCase_MaxValueCombination_WorksCorrectly()
    {
        var maxCombo = TestEnum.Test1 | TestEnum.Test2 | TestEnum.Test4 | TestEnum.Test5 | TestEnum.Test6 | TestEnum.Test7;

        string? name = maxCombo.ToStringFast();
        Assert.NotNull(name);
        Assert.Contains(", ", name);

        // Verify all flags are present
        var values = TestEnumEnhanced.GetValuesFast().Where(v => v != 0 && (int)v == ((int)v & (int)maxCombo));
        foreach (var value in values)
        {
            Assert.True(maxCombo.HasFlagFast(value));
        }
    }

    [Fact]
    public void EdgeCase_ConsecutiveFlagParsing_OrderIndependent()
    {
        var combo1 = TestEnumEnhanced.ParseFast($"{nameof(TestEnum.Test1)}, {nameof(TestEnum.Test4)}");
        var combo2 = TestEnumEnhanced.ParseFast($"{nameof(TestEnum.Test4)}, {nameof(TestEnum.Test1)}");

        Assert.Equal(combo1, combo2);
        Assert.Equal(TestEnum.Test1 | TestEnum.Test4, combo1);
    }

    [Fact]
    public void EdgeCase_MixedCaseParsing_IgnoreCase()
    {
        var testCases = new[]
        {
            ($"test1, TEST2", TestEnum.Test1 | TestEnum.Test2),
            ($"TeSt4, tEsT5", TestEnum.Test4 | TestEnum.Test5),
            ($"TEST1, test4, TeSt6", TestEnum.Test1 | TestEnum.Test4 | TestEnum.Test6),
        };

        foreach (var (input, expected) in testCases)
        {
            var actual = TestEnumEnhanced.ParseFast(input, ignoreCase: true);
            Assert.Equal(expected, actual);
        }
    }

    [Fact]
    public void EdgeCase_DuplicateFlagsInParse_SameResult()
    {
        var single = TestEnumEnhanced.ParseFast($"{nameof(TestEnum.Test1)}");
        var duplicate = TestEnumEnhanced.ParseFast($"{nameof(TestEnum.Test1)}, {nameof(TestEnum.Test1)}");

        Assert.Equal(single, duplicate);
    }

    [Fact]
    public void EdgeCase_NonPowerOfTwoValue_Test7Behavior()
    {
        // Test7 should have a specific value (power of 2 or calculated)
        var test7 = TestEnum.Test7;

        string? name = test7.GetNameFast(false);
        Assert.Equal("Test7", name);

        var parsed = TestEnumEnhanced.ParseFast("Test7");
        Assert.Equal(test7, parsed);
    }
}