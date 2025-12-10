namespace EnumsEnhancedTest;

// Original flag enum
[Flags]
internal enum TestEnum
{
    Test0 = 0,
    Test1 = 1 << 0,
    Test_1 = 1 << 0,
    Test2 = 1 << 1,
    Test4 = 1 << 2,
    Test5 = 1 << 4,
    Test6 = 1 << 6,
    Test7
}

// Non-flag enum with various value patterns
public enum TestEnum2 : short
{
    Test1 = -2,
    Test2 = +1,
}

// Non-flag enum with sequential values
public enum SequentialEnum
{
    None = 0,
    First = 1,
    Second = 2,
    Third = 3,
    Fourth = 4,
    Fifth = 5
}

// Non-flag enum with gaps
public enum GappedEnum : byte
{
    Min = 0,
    Small = 10,
    Medium = 50,
    Large = 100,
    Max = 255
}

// Flag enum with all bits (byte)
[Flags]
public enum ByteFlagsEnum : byte
{
    None = 0,
    Bit0 = 1 << 0,
    Bit1 = 1 << 1,
    Bit2 = 1 << 2,
    Bit3 = 1 << 3,
    Bit4 = 1 << 4,
    Bit5 = 1 << 5,
    Bit6 = 1 << 6,
    Bit7 = 1 << 7,
    All = 0xFF,
    LowerNibble = Bit0 | Bit1 | Bit2 | Bit3,
    UpperNibble = Bit4 | Bit5 | Bit6 | Bit7
}

// Flag enum with signed byte (sbyte)
[Flags]
public enum SignedByteFlagsEnum : sbyte
{
    None = 0,
    Bit0 = 1 << 0,
    Bit1 = 1 << 1,
    Bit2 = 1 << 2,
    Bit3 = 1 << 3,
    Bit4 = 1 << 4,
    Bit5 = 1 << 5,
    Bit6 = 1 << 6,
    NegativeBit = -128 // sbyte.MinValue
}

// Flag enum with long
[Flags]
public enum LongFlagsEnum : long
{
    None = 0L,
    Bit0 = 1L << 0,
    Bit10 = 1L << 10,
    Bit20 = 1L << 20,
    Bit30 = 1L << 30,
    Bit40 = 1L << 40,
    Bit50 = 1L << 50,
    Bit62 = 1L << 62,
    HighBit = long.MinValue // Bit63 (sign bit)
}

// Non-flag enum with negative values
public enum NegativeValuesEnum : int
{
    VeryNegative = int.MinValue,
    Negative = -100,
    Zero = 0,
    Positive = 100,
    VeryPositive = int.MaxValue
}

// Flag enum with composite values
[Flags]
public enum CompositeFlagsEnum
{
    None = 0,
    Read = 1,
    Write = 2,
    Execute = 4,
    Delete = 8,

    // Composite values
    ReadWrite = Read | Write,
    ReadExecute = Read | Execute,
    ReadWriteExecute = Read | Write | Execute,
    All = Read | Write | Execute | Delete
}

// Non-flag enum with duplicate values
public enum DuplicateValuesEnum
{
    First = 1,
    Second = 2,
    SecondAlias = 2,
    Third = 3,
    ThirdAlias = 3,
    ThirdAlias2 = 3
}

// Flag enum with duplicate values
[Flags]
public enum DuplicateFlagsEnum
{
    None = 0,
    Flag1 = 1,
    Flag1Alias = 1,
    Flag2 = 2,
    Flag2Alias = 2,
    Flag4 = 4,
    Combined = Flag1 | Flag2,
    CombinedAlias = 3
}

// Enum with single value
public enum SingleValueEnum
{
    OnlyValue = 42
}

// Empty-like enum (only zero)
public enum EmptyEnum
{
    Nothing = 0
}

// Enum with extreme values (ulong)
public enum UlongEnum : ulong
{
    Zero = 0,
    Small = 1,
    Medium = ulong.MaxValue / 2,
    Large = ulong.MaxValue - 1,
    Max = ulong.MaxValue
}

// Flag enum with sparse bits
[Flags]
public enum SparseFlagsEnum
{
    None = 0,
    Bit0 = 1 << 0,
    Bit5 = 1 << 5,
    Bit10 = 1 << 10,
    Bit15 = 1 << 15,
    Bit20 = 1 << 20,
    Bit25 = 1 << 25,
    Bit30 = 1 << 30
}

// Non-flag enum with powers of 2 (looks like flags but isn't)
public enum PseudoFlagsEnum
{
    None = 0,
    One = 1,
    Two = 2,
    Four = 4,
    Eight = 8,
    Sixteen = 16,
    ThirtyTwo = 32,
    SixtyFour = 64
}

// Flag enum with overlapping composite values
[Flags]
public enum OverlappingFlagsEnum
{
    None = 0,
    A = 1,
    B = 2,
    C = 4,
    D = 8,

    AB = A | B,
    BC = B | C,
    CD = C | D,
    ABC = A | B | C,
    BCD = B | C | D,
    ABCD = A | B | C | D
}

internal class EnumContainingTypeTest
{
    public enum ContainingTypeEnum
    {
        Test1
    }
}
