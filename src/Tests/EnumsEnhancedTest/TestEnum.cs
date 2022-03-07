namespace EnumsEnhancedTest;

[Flags]
internal enum TestEnum
{
    Test1 = 1 << 0,
    Test_1 = 1 << 0,

    Test2 = 1 << 1,
    Test4 = 1 << 2,
    Test5 = 1 << 4,
    Test6 = 1 << 6,
    Test7
}

public enum TestEnum2 : short
{
    Test1 = -2,
    Test2 = +1,
}
