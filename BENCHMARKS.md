# Benchmarks

## HasFlag

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22567
Unknown processor
.NET SDK=7.0.100-preview.1.22110.4
  [Host]             : .NET 7.0.0 (7.0.22.7608), X64 RyuJIT
  .NET 6.0           : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT
  .NET Framework 4.8 : .NET Framework 4.8 (4.8.9022.1), X64 RyuJIT


```
|            Method |                Job |            Runtime |        TestEnumValue |    TestEnumFlagCheck |       Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 | Allocated |
|------------------ |------------------- |------------------- |--------------------- |--------------------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|
|           **HasFlag** |           **.NET 6.0** |           **.NET 6.0** |                **Test1** |                **Test1** |  **0.1633 ns** | **0.0063 ns** | **0.0059 ns** |  **1.00** |    **0.00** |      **-** |         **-** |
|       HasFlagFast |           .NET 6.0 |           .NET 6.0 |                Test1 |                Test1 |  0.1544 ns | 0.0037 ns | 0.0031 ns |  0.94 |    0.04 |      - |         - |
| HasFlagFastUnsafe |           .NET 6.0 |           .NET 6.0 |                Test1 |                Test1 |  0.1665 ns | 0.0040 ns | 0.0037 ns |  1.02 |    0.03 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           HasFlag | .NET Framework 4.8 | .NET Framework 4.8 |                Test1 |                Test1 | 10.7145 ns | 0.0853 ns | 0.0756 ns |  1.00 |    0.00 | 0.0076 |      48 B |
|       HasFlagFast | .NET Framework 4.8 | .NET Framework 4.8 |                Test1 |                Test1 |  0.1926 ns | 0.0079 ns | 0.0070 ns |  0.02 |    0.00 |      - |         - |
| HasFlagFastUnsafe | .NET Framework 4.8 | .NET Framework 4.8 |                Test1 |                Test1 |  1.0656 ns | 0.0051 ns | 0.0045 ns |  0.10 |    0.00 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           **HasFlag** |           **.NET 6.0** |           **.NET 6.0** |                **Test1** |         **Test1, Test4** |  **0.0876 ns** | **0.0056 ns** | **0.0052 ns** |  **1.00** |    **0.00** |      **-** |         **-** |
|       HasFlagFast |           .NET 6.0 |           .NET 6.0 |                Test1 |         Test1, Test4 |  0.1595 ns | 0.0056 ns | 0.0053 ns |  1.83 |    0.11 |      - |         - |
| HasFlagFastUnsafe |           .NET 6.0 |           .NET 6.0 |                Test1 |         Test1, Test4 |  0.1614 ns | 0.0041 ns | 0.0039 ns |  1.85 |    0.10 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           HasFlag | .NET Framework 4.8 | .NET Framework 4.8 |                Test1 |         Test1, Test4 | 11.1008 ns | 0.1036 ns | 0.0969 ns |  1.00 |    0.00 | 0.0076 |      48 B |
|       HasFlagFast | .NET Framework 4.8 | .NET Framework 4.8 |                Test1 |         Test1, Test4 |  0.1863 ns | 0.0074 ns | 0.0069 ns |  0.02 |    0.00 |      - |         - |
| HasFlagFastUnsafe | .NET Framework 4.8 | .NET Framework 4.8 |                Test1 |         Test1, Test4 |  1.0776 ns | 0.0050 ns | 0.0042 ns |  0.10 |    0.00 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           **HasFlag** |           **.NET 6.0** |           **.NET 6.0** |                **Test1** |         **Test4, Test7** |  **0.1300 ns** | **0.0068 ns** | **0.0063 ns** |  **1.00** |    **0.00** |      **-** |         **-** |
|       HasFlagFast |           .NET 6.0 |           .NET 6.0 |                Test1 |         Test4, Test7 |  0.1606 ns | 0.0071 ns | 0.0063 ns |  1.23 |    0.08 |      - |         - |
| HasFlagFastUnsafe |           .NET 6.0 |           .NET 6.0 |                Test1 |         Test4, Test7 |  0.1566 ns | 0.0035 ns | 0.0031 ns |  1.20 |    0.05 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           HasFlag | .NET Framework 4.8 | .NET Framework 4.8 |                Test1 |         Test4, Test7 | 11.1997 ns | 0.2393 ns | 0.2660 ns |  1.00 |    0.00 | 0.0076 |      48 B |
|       HasFlagFast | .NET Framework 4.8 | .NET Framework 4.8 |                Test1 |         Test4, Test7 |  0.1886 ns | 0.0087 ns | 0.0081 ns |  0.02 |    0.00 |      - |         - |
| HasFlagFastUnsafe | .NET Framework 4.8 | .NET Framework 4.8 |                Test1 |         Test4, Test7 |  1.0776 ns | 0.0036 ns | 0.0030 ns |  0.10 |    0.00 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           **HasFlag** |           **.NET 6.0** |           **.NET 6.0** |                **Test1** | **Test2(...)Test7 [26]** |  **0.1697 ns** | **0.0074 ns** | **0.0069 ns** |  **1.00** |    **0.00** |      **-** |         **-** |
|       HasFlagFast |           .NET 6.0 |           .NET 6.0 |                Test1 | Test2(...)Test7 [26] |  0.1607 ns | 0.0063 ns | 0.0056 ns |  0.95 |    0.06 |      - |         - |
| HasFlagFastUnsafe |           .NET 6.0 |           .NET 6.0 |                Test1 | Test2(...)Test7 [26] |  0.1593 ns | 0.0065 ns | 0.0057 ns |  0.94 |    0.05 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           HasFlag | .NET Framework 4.8 | .NET Framework 4.8 |                Test1 | Test2(...)Test7 [26] | 11.0349 ns | 0.1069 ns | 0.0948 ns |  1.00 |    0.00 | 0.0076 |      48 B |
|       HasFlagFast | .NET Framework 4.8 | .NET Framework 4.8 |                Test1 | Test2(...)Test7 [26] |  0.2040 ns | 0.0081 ns | 0.0072 ns |  0.02 |    0.00 |      - |         - |
| HasFlagFastUnsafe | .NET Framework 4.8 | .NET Framework 4.8 |                Test1 | Test2(...)Test7 [26] |  1.0784 ns | 0.0048 ns | 0.0040 ns |  0.10 |    0.00 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           **HasFlag** |           **.NET 6.0** |           **.NET 6.0** |         **Test1, Test4** |                **Test1** |  **0.1626 ns** | **0.0056 ns** | **0.0053 ns** |  **1.00** |    **0.00** |      **-** |         **-** |
|       HasFlagFast |           .NET 6.0 |           .NET 6.0 |         Test1, Test4 |                Test1 |  0.1620 ns | 0.0034 ns | 0.0032 ns |  1.00 |    0.04 |      - |         - |
| HasFlagFastUnsafe |           .NET 6.0 |           .NET 6.0 |         Test1, Test4 |                Test1 |  0.1303 ns | 0.0063 ns | 0.0056 ns |  0.80 |    0.04 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           HasFlag | .NET Framework 4.8 | .NET Framework 4.8 |         Test1, Test4 |                Test1 | 10.7468 ns | 0.0922 ns | 0.0863 ns |  1.00 |    0.00 | 0.0076 |      48 B |
|       HasFlagFast | .NET Framework 4.8 | .NET Framework 4.8 |         Test1, Test4 |                Test1 |  0.1912 ns | 0.0090 ns | 0.0084 ns |  0.02 |    0.00 |      - |         - |
| HasFlagFastUnsafe | .NET Framework 4.8 | .NET Framework 4.8 |         Test1, Test4 |                Test1 |  1.0665 ns | 0.0023 ns | 0.0021 ns |  0.10 |    0.00 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           **HasFlag** |           **.NET 6.0** |           **.NET 6.0** |         **Test1, Test4** |         **Test1, Test4** |  **0.1560 ns** | **0.0053 ns** | **0.0049 ns** |  **1.00** |    **0.00** |      **-** |         **-** |
|       HasFlagFast |           .NET 6.0 |           .NET 6.0 |         Test1, Test4 |         Test1, Test4 |  0.1589 ns | 0.0059 ns | 0.0055 ns |  1.02 |    0.05 |      - |         - |
| HasFlagFastUnsafe |           .NET 6.0 |           .NET 6.0 |         Test1, Test4 |         Test1, Test4 |  0.1562 ns | 0.0053 ns | 0.0049 ns |  1.00 |    0.05 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           HasFlag | .NET Framework 4.8 | .NET Framework 4.8 |         Test1, Test4 |         Test1, Test4 | 10.5692 ns | 0.0710 ns | 0.0630 ns |  1.00 |    0.00 | 0.0076 |      48 B |
|       HasFlagFast | .NET Framework 4.8 | .NET Framework 4.8 |         Test1, Test4 |         Test1, Test4 |  0.1971 ns | 0.0081 ns | 0.0075 ns |  0.02 |    0.00 |      - |         - |
| HasFlagFastUnsafe | .NET Framework 4.8 | .NET Framework 4.8 |         Test1, Test4 |         Test1, Test4 |  1.0816 ns | 0.0064 ns | 0.0057 ns |  0.10 |    0.00 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           **HasFlag** |           **.NET 6.0** |           **.NET 6.0** |         **Test1, Test4** |         **Test4, Test7** |  **0.1582 ns** | **0.0051 ns** | **0.0047 ns** |  **1.00** |    **0.00** |      **-** |         **-** |
|       HasFlagFast |           .NET 6.0 |           .NET 6.0 |         Test1, Test4 |         Test4, Test7 |  0.1647 ns | 0.0039 ns | 0.0034 ns |  1.04 |    0.04 |      - |         - |
| HasFlagFastUnsafe |           .NET 6.0 |           .NET 6.0 |         Test1, Test4 |         Test4, Test7 |  0.1149 ns | 0.0056 ns | 0.0053 ns |  0.73 |    0.04 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           HasFlag | .NET Framework 4.8 | .NET Framework 4.8 |         Test1, Test4 |         Test4, Test7 | 11.0612 ns | 0.2354 ns | 0.2202 ns |  1.00 |    0.00 | 0.0076 |      48 B |
|       HasFlagFast | .NET Framework 4.8 | .NET Framework 4.8 |         Test1, Test4 |         Test4, Test7 |  0.1887 ns | 0.0068 ns | 0.0056 ns |  0.02 |    0.00 |      - |         - |
| HasFlagFastUnsafe | .NET Framework 4.8 | .NET Framework 4.8 |         Test1, Test4 |         Test4, Test7 |  1.0721 ns | 0.0039 ns | 0.0036 ns |  0.10 |    0.00 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           **HasFlag** |           **.NET 6.0** |           **.NET 6.0** |         **Test1, Test4** | **Test2(...)Test7 [26]** |  **0.1290 ns** | **0.0095 ns** | **0.0089 ns** |  **1.00** |    **0.00** |      **-** |         **-** |
|       HasFlagFast |           .NET 6.0 |           .NET 6.0 |         Test1, Test4 | Test2(...)Test7 [26] |  0.1343 ns | 0.0061 ns | 0.0058 ns |  1.05 |    0.08 |      - |         - |
| HasFlagFastUnsafe |           .NET 6.0 |           .NET 6.0 |         Test1, Test4 | Test2(...)Test7 [26] |  0.1588 ns | 0.0044 ns | 0.0041 ns |  1.24 |    0.09 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           HasFlag | .NET Framework 4.8 | .NET Framework 4.8 |         Test1, Test4 | Test2(...)Test7 [26] | 10.8127 ns | 0.1024 ns | 0.0958 ns |  1.00 |    0.00 | 0.0076 |      48 B |
|       HasFlagFast | .NET Framework 4.8 | .NET Framework 4.8 |         Test1, Test4 | Test2(...)Test7 [26] |  0.2015 ns | 0.0087 ns | 0.0081 ns |  0.02 |    0.00 |      - |         - |
| HasFlagFastUnsafe | .NET Framework 4.8 | .NET Framework 4.8 |         Test1, Test4 | Test2(...)Test7 [26] |  1.0796 ns | 0.0098 ns | 0.0092 ns |  0.10 |    0.00 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           **HasFlag** |           **.NET 6.0** |           **.NET 6.0** |         **Test4, Test7** |                **Test1** |  **0.1358 ns** | **0.0076 ns** | **0.0067 ns** |  **1.00** |    **0.00** |      **-** |         **-** |
|       HasFlagFast |           .NET 6.0 |           .NET 6.0 |         Test4, Test7 |                Test1 |  0.1534 ns | 0.0036 ns | 0.0032 ns |  1.13 |    0.06 |      - |         - |
| HasFlagFastUnsafe |           .NET 6.0 |           .NET 6.0 |         Test4, Test7 |                Test1 |  0.1617 ns | 0.0045 ns | 0.0042 ns |  1.19 |    0.06 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           HasFlag | .NET Framework 4.8 | .NET Framework 4.8 |         Test4, Test7 |                Test1 | 10.6501 ns | 0.0936 ns | 0.0875 ns |  1.00 |    0.00 | 0.0076 |      48 B |
|       HasFlagFast | .NET Framework 4.8 | .NET Framework 4.8 |         Test4, Test7 |                Test1 |  0.1917 ns | 0.0076 ns | 0.0067 ns |  0.02 |    0.00 |      - |         - |
| HasFlagFastUnsafe | .NET Framework 4.8 | .NET Framework 4.8 |         Test4, Test7 |                Test1 |  1.0729 ns | 0.0099 ns | 0.0092 ns |  0.10 |    0.00 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           **HasFlag** |           **.NET 6.0** |           **.NET 6.0** |         **Test4, Test7** |         **Test1, Test4** |  **0.1276 ns** | **0.0089 ns** | **0.0084 ns** |  **1.00** |    **0.00** |      **-** |         **-** |
|       HasFlagFast |           .NET 6.0 |           .NET 6.0 |         Test4, Test7 |         Test1, Test4 |  0.1578 ns | 0.0023 ns | 0.0020 ns |  1.24 |    0.09 |      - |         - |
| HasFlagFastUnsafe |           .NET 6.0 |           .NET 6.0 |         Test4, Test7 |         Test1, Test4 |  0.1280 ns | 0.0078 ns | 0.0069 ns |  1.01 |    0.09 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           HasFlag | .NET Framework 4.8 | .NET Framework 4.8 |         Test4, Test7 |         Test1, Test4 | 10.6058 ns | 0.0948 ns | 0.0887 ns |  1.00 |    0.00 | 0.0076 |      48 B |
|       HasFlagFast | .NET Framework 4.8 | .NET Framework 4.8 |         Test4, Test7 |         Test1, Test4 |  0.1894 ns | 0.0046 ns | 0.0040 ns |  0.02 |    0.00 |      - |         - |
| HasFlagFastUnsafe | .NET Framework 4.8 | .NET Framework 4.8 |         Test4, Test7 |         Test1, Test4 |  1.0831 ns | 0.0066 ns | 0.0055 ns |  0.10 |    0.00 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           **HasFlag** |           **.NET 6.0** |           **.NET 6.0** |         **Test4, Test7** |         **Test4, Test7** |  **0.1249 ns** | **0.0094 ns** | **0.0088 ns** |  **1.00** |    **0.00** |      **-** |         **-** |
|       HasFlagFast |           .NET 6.0 |           .NET 6.0 |         Test4, Test7 |         Test4, Test7 |  0.1603 ns | 0.0063 ns | 0.0055 ns |  1.29 |    0.10 |      - |         - |
| HasFlagFastUnsafe |           .NET 6.0 |           .NET 6.0 |         Test4, Test7 |         Test4, Test7 |  0.1305 ns | 0.0063 ns | 0.0059 ns |  1.05 |    0.10 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           HasFlag | .NET Framework 4.8 | .NET Framework 4.8 |         Test4, Test7 |         Test4, Test7 | 10.6546 ns | 0.0897 ns | 0.0795 ns |  1.00 |    0.00 | 0.0076 |      48 B |
|       HasFlagFast | .NET Framework 4.8 | .NET Framework 4.8 |         Test4, Test7 |         Test4, Test7 |  0.1963 ns | 0.0052 ns | 0.0046 ns |  0.02 |    0.00 |      - |         - |
| HasFlagFastUnsafe | .NET Framework 4.8 | .NET Framework 4.8 |         Test4, Test7 |         Test4, Test7 |  1.0822 ns | 0.0022 ns | 0.0018 ns |  0.10 |    0.00 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           **HasFlag** |           **.NET 6.0** |           **.NET 6.0** |         **Test4, Test7** | **Test2(...)Test7 [26]** |  **0.1284 ns** | **0.0049 ns** | **0.0046 ns** |  **1.00** |    **0.00** |      **-** |         **-** |
|       HasFlagFast |           .NET 6.0 |           .NET 6.0 |         Test4, Test7 | Test2(...)Test7 [26] |  0.1603 ns | 0.0046 ns | 0.0043 ns |  1.25 |    0.05 |      - |         - |
| HasFlagFastUnsafe |           .NET 6.0 |           .NET 6.0 |         Test4, Test7 | Test2(...)Test7 [26] |  0.1658 ns | 0.0064 ns | 0.0060 ns |  1.29 |    0.08 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           HasFlag | .NET Framework 4.8 | .NET Framework 4.8 |         Test4, Test7 | Test2(...)Test7 [26] | 10.9188 ns | 0.1691 ns | 0.1582 ns |  1.00 |    0.00 | 0.0076 |      48 B |
|       HasFlagFast | .NET Framework 4.8 | .NET Framework 4.8 |         Test4, Test7 | Test2(...)Test7 [26] |  0.1895 ns | 0.0056 ns | 0.0052 ns |  0.02 |    0.00 |      - |         - |
| HasFlagFastUnsafe | .NET Framework 4.8 | .NET Framework 4.8 |         Test4, Test7 | Test2(...)Test7 [26] |  1.0757 ns | 0.0033 ns | 0.0029 ns |  0.10 |    0.00 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           **HasFlag** |           **.NET 6.0** |           **.NET 6.0** | **Test2(...)Test7 [26]** |                **Test1** |  **0.1632 ns** | **0.0063 ns** | **0.0056 ns** |  **1.00** |    **0.00** |      **-** |         **-** |
|       HasFlagFast |           .NET 6.0 |           .NET 6.0 | Test2(...)Test7 [26] |                Test1 |  0.1648 ns | 0.0072 ns | 0.0068 ns |  1.01 |    0.03 |      - |         - |
| HasFlagFastUnsafe |           .NET 6.0 |           .NET 6.0 | Test2(...)Test7 [26] |                Test1 |  0.1555 ns | 0.0048 ns | 0.0045 ns |  0.95 |    0.03 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           HasFlag | .NET Framework 4.8 | .NET Framework 4.8 | Test2(...)Test7 [26] |                Test1 | 10.6570 ns | 0.0919 ns | 0.0814 ns |  1.00 |    0.00 | 0.0076 |      48 B |
|       HasFlagFast | .NET Framework 4.8 | .NET Framework 4.8 | Test2(...)Test7 [26] |                Test1 |  0.1971 ns | 0.0058 ns | 0.0049 ns |  0.02 |    0.00 |      - |         - |
| HasFlagFastUnsafe | .NET Framework 4.8 | .NET Framework 4.8 | Test2(...)Test7 [26] |                Test1 |  1.0785 ns | 0.0050 ns | 0.0041 ns |  0.10 |    0.00 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           **HasFlag** |           **.NET 6.0** |           **.NET 6.0** | **Test2(...)Test7 [26]** |         **Test1, Test4** |  **0.1334 ns** | **0.0153 ns** | **0.0143 ns** |  **1.00** |    **0.00** |      **-** |         **-** |
|       HasFlagFast |           .NET 6.0 |           .NET 6.0 | Test2(...)Test7 [26] |         Test1, Test4 |  0.1573 ns | 0.0070 ns | 0.0065 ns |  1.19 |    0.14 |      - |         - |
| HasFlagFastUnsafe |           .NET 6.0 |           .NET 6.0 | Test2(...)Test7 [26] |         Test1, Test4 |  0.1625 ns | 0.0052 ns | 0.0046 ns |  1.23 |    0.12 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           HasFlag | .NET Framework 4.8 | .NET Framework 4.8 | Test2(...)Test7 [26] |         Test1, Test4 | 10.6389 ns | 0.0826 ns | 0.0732 ns |  1.00 |    0.00 | 0.0076 |      48 B |
|       HasFlagFast | .NET Framework 4.8 | .NET Framework 4.8 | Test2(...)Test7 [26] |         Test1, Test4 |  0.1929 ns | 0.0092 ns | 0.0086 ns |  0.02 |    0.00 |      - |         - |
| HasFlagFastUnsafe | .NET Framework 4.8 | .NET Framework 4.8 | Test2(...)Test7 [26] |         Test1, Test4 |  1.0717 ns | 0.0042 ns | 0.0033 ns |  0.10 |    0.00 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           **HasFlag** |           **.NET 6.0** |           **.NET 6.0** | **Test2(...)Test7 [26]** |         **Test4, Test7** |  **0.1586 ns** | **0.0051 ns** | **0.0048 ns** |  **1.00** |    **0.00** |      **-** |         **-** |
|       HasFlagFast |           .NET 6.0 |           .NET 6.0 | Test2(...)Test7 [26] |         Test4, Test7 |  0.1506 ns | 0.0048 ns | 0.0045 ns |  0.95 |    0.04 |      - |         - |
| HasFlagFastUnsafe |           .NET 6.0 |           .NET 6.0 | Test2(...)Test7 [26] |         Test4, Test7 |  0.0812 ns | 0.0078 ns | 0.0069 ns |  0.51 |    0.05 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           HasFlag | .NET Framework 4.8 | .NET Framework 4.8 | Test2(...)Test7 [26] |         Test4, Test7 | 10.7054 ns | 0.0689 ns | 0.0645 ns |  1.00 |    0.00 | 0.0076 |      48 B |
|       HasFlagFast | .NET Framework 4.8 | .NET Framework 4.8 | Test2(...)Test7 [26] |         Test4, Test7 |  0.1952 ns | 0.0071 ns | 0.0063 ns |  0.02 |    0.00 |      - |         - |
| HasFlagFastUnsafe | .NET Framework 4.8 | .NET Framework 4.8 | Test2(...)Test7 [26] |         Test4, Test7 |  1.0666 ns | 0.0054 ns | 0.0045 ns |  0.10 |    0.00 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           **HasFlag** |           **.NET 6.0** |           **.NET 6.0** | **Test2(...)Test7 [26]** | **Test2(...)Test7 [26]** |  **0.1563 ns** | **0.0068 ns** | **0.0064 ns** |  **1.00** |    **0.00** |      **-** |         **-** |
|       HasFlagFast |           .NET 6.0 |           .NET 6.0 | Test2(...)Test7 [26] | Test2(...)Test7 [26] |  0.1587 ns | 0.0048 ns | 0.0045 ns |  1.02 |    0.05 |      - |         - |
| HasFlagFastUnsafe |           .NET 6.0 |           .NET 6.0 | Test2(...)Test7 [26] | Test2(...)Test7 [26] |  0.1554 ns | 0.0048 ns | 0.0043 ns |  1.00 |    0.04 |      - |         - |
|                   |                    |                    |                      |                      |            |           |           |       |         |        |           |
|           HasFlag | .NET Framework 4.8 | .NET Framework 4.8 | Test2(...)Test7 [26] | Test2(...)Test7 [26] | 10.5093 ns | 0.1013 ns | 0.0898 ns |  1.00 |    0.00 | 0.0076 |      48 B |
|       HasFlagFast | .NET Framework 4.8 | .NET Framework 4.8 | Test2(...)Test7 [26] | Test2(...)Test7 [26] |  0.1990 ns | 0.0048 ns | 0.0045 ns |  0.02 |    0.00 |      - |         - |
| HasFlagFastUnsafe | .NET Framework 4.8 | .NET Framework 4.8 | Test2(...)Test7 [26] | Test2(...)Test7 [26] |  1.0820 ns | 0.0032 ns | 0.0027 ns |  0.10 |    0.00 |      - |         - |


## GetNames

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22567
Unknown processor
.NET SDK=7.0.100-preview.1.22110.4
  [Host]             : .NET 7.0.0 (7.0.22.7608), X64 RyuJIT
  .NET 6.0           : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT
  .NET Framework 4.8 : .NET Framework 4.8 (4.8.9022.1), X64 RyuJIT


```
|       Method |                Job |            Runtime |     Mean |    Error |   StdDev | Ratio | RatioSD |  Gen 0 | Allocated |
|------------- |------------------- |------------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|
|     GetNames |           .NET 6.0 |           .NET 6.0 | 19.85 ns | 0.151 ns | 0.141 ns |  1.00 |    0.00 | 0.0043 |      72 B |
| GetNamesFast |           .NET 6.0 |           .NET 6.0 | 10.38 ns | 0.252 ns | 0.414 ns |  0.54 |    0.02 | 0.0043 |      72 B |
|              |                    |                    |          |          |          |       |         |        |           |
|     GetNames | .NET Framework 4.8 | .NET Framework 4.8 | 35.16 ns | 0.457 ns | 0.428 ns |  1.00 |    0.00 | 0.0113 |      72 B |
| GetNamesFast | .NET Framework 4.8 | .NET Framework 4.8 | 13.59 ns | 0.321 ns | 0.499 ns |  0.39 |    0.01 | 0.0114 |      72 B |


# GetName

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22567
Unknown processor
.NET SDK=7.0.100-preview.1.22110.4
  [Host]             : .NET 7.0.0 (7.0.22.7608), X64 RyuJIT
  .NET 6.0           : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT
  .NET Framework 4.8 : .NET Framework 4.8 (4.8.9022.1), X64 RyuJIT


```
|      Method |                Job |            Runtime | NameValue |      Mean |     Error |    StdDev | Ratio |  Gen 0 | Allocated |
|------------ |------------------- |------------------- |---------- |----------:|----------:|----------:|------:|-------:|----------:|
|     **GetName** |           **.NET 6.0** |           **.NET 6.0** |     **Test1** | **33.480 ns** | **0.4676 ns** | **0.4374 ns** |  **1.00** | **0.0014** |      **24 B** |
| GetNameFast |           .NET 6.0 |           .NET 6.0 |     Test1 |  1.077 ns | 0.0216 ns | 0.0192 ns |  0.03 |      - |         - |
|             |                    |                    |           |           |           |           |       |        |           |
|     GetName | .NET Framework 4.8 | .NET Framework 4.8 |     Test1 | 99.949 ns | 0.7970 ns | 0.7065 ns |  1.00 | 0.0074 |      48 B |
| GetNameFast | .NET Framework 4.8 | .NET Framework 4.8 |     Test1 |  1.386 ns | 0.0719 ns | 0.0672 ns |  0.01 |      - |         - |
|             |                    |                    |           |           |           |           |       |        |           |
|     **GetName** |           **.NET 6.0** |           **.NET 6.0** |     **Test6** | **33.767 ns** | **0.7213 ns** | **0.9378 ns** |  **1.00** | **0.0014** |      **24 B** |
| GetNameFast |           .NET 6.0 |           .NET 6.0 |     Test6 |  1.253 ns | 0.0312 ns | 0.0292 ns |  0.04 |      - |         - |
|             |                    |                    |           |           |           |           |       |        |           |
|     GetName | .NET Framework 4.8 | .NET Framework 4.8 |     Test6 | 98.044 ns | 0.8165 ns | 0.7238 ns |  1.00 | 0.0074 |      48 B |
| GetNameFast | .NET Framework 4.8 | .NET Framework 4.8 |     Test6 |  1.314 ns | 0.0536 ns | 0.0448 ns |  0.01 |      - |         - |


# GetValues

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22567
Unknown processor
.NET SDK=7.0.100-preview.1.22110.4
  [Host]             : .NET 7.0.0 (7.0.22.7608), X64 RyuJIT
  .NET 6.0           : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT
  .NET Framework 4.8 : .NET Framework 4.8 (4.8.9022.1), X64 RyuJIT


```
|        Method |                Job |            Runtime |       Mean |     Error |    StdDev | Ratio |  Gen 0 | Allocated |
|-------------- |------------------- |------------------- |-----------:|----------:|----------:|------:|-------:|----------:|
|     GetValues |           .NET 6.0 |           .NET 6.0 | 376.091 ns | 2.8219 ns | 2.6396 ns | 1.000 | 0.0110 |     184 B |
| GetValuesFast |           .NET 6.0 |           .NET 6.0 |   3.447 ns | 0.1131 ns | 0.1162 ns | 0.009 | 0.0024 |      40 B |
|               |                    |                    |            |           |           |       |        |           |
|     GetValues | .NET Framework 4.8 | .NET Framework 4.8 | 541.702 ns | 6.6001 ns | 6.1737 ns | 1.000 | 0.0277 |     185 B |
| GetValuesFast | .NET Framework 4.8 | .NET Framework 4.8 |   2.790 ns | 0.1078 ns | 0.1008 ns | 0.005 | 0.0064 |      40 B |

# IsDefined Name

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22567
Unknown processor
.NET SDK=7.0.100-preview.1.22110.4
  [Host]             : .NET 7.0.0 (7.0.22.7608), X64 RyuJIT
  .NET 6.0           : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT
  .NET Framework 4.8 : .NET Framework 4.8 (4.8.9022.1), X64 RyuJIT


```
|                   Method |                Job |            Runtime | NameValue |      Mean |     Error |    StdDev | Ratio | Allocated |
|------------------------- |------------------- |------------------- |---------- |----------:|----------:|----------:|------:|----------:|
|     **IsDefined_StringName** |           **.NET 6.0** |           **.NET 6.0** |   **Invalid** | **42.731 ns** | **0.2840 ns** | **0.2517 ns** |  **1.00** |         **-** |
| IsDefinedFast_StringName |           .NET 6.0 |           .NET 6.0 |   Invalid |  2.272 ns | 0.0493 ns | 0.0461 ns |  0.05 |         - |
|                          |                    |                    |           |           |           |           |       |           |
|     IsDefined_StringName | .NET Framework 4.8 | .NET Framework 4.8 |   Invalid | 40.955 ns | 0.3580 ns | 0.2990 ns |  1.00 |         - |
| IsDefinedFast_StringName | .NET Framework 4.8 | .NET Framework 4.8 |   Invalid |  7.274 ns | 0.0337 ns | 0.0315 ns |  0.18 |         - |
|                          |                    |                    |           |           |           |           |       |           |
|     **IsDefined_StringName** |           **.NET 6.0** |           **.NET 6.0** |     **Test1** | **36.646 ns** | **0.3325 ns** | **0.2948 ns** |  **1.00** |         **-** |
| IsDefinedFast_StringName |           .NET 6.0 |           .NET 6.0 |     Test1 |  1.025 ns | 0.0178 ns | 0.0158 ns |  0.03 |         - |
|                          |                    |                    |           |           |           |           |       |           |
|     IsDefined_StringName | .NET Framework 4.8 | .NET Framework 4.8 |     Test1 | 34.242 ns | 0.6956 ns | 0.7443 ns |  1.00 |         - |
| IsDefinedFast_StringName | .NET Framework 4.8 | .NET Framework 4.8 |     Test1 |  1.635 ns | 0.0172 ns | 0.0153 ns |  0.05 |         - |
|                          |                    |                    |           |           |           |           |       |           |
|     **IsDefined_StringName** |           **.NET 6.0** |           **.NET 6.0** |     **Test6** | **49.736 ns** | **0.5576 ns** | **0.5216 ns** |  **1.00** |         **-** |
| IsDefinedFast_StringName |           .NET 6.0 |           .NET 6.0 |     Test6 |  9.277 ns | 0.0679 ns | 0.0635 ns |  0.19 |         - |
|                          |                    |                    |           |           |           |           |       |           |
|     IsDefined_StringName | .NET Framework 4.8 | .NET Framework 4.8 |     Test6 | 48.675 ns | 0.5386 ns | 0.5038 ns |  1.00 |         - |
| IsDefinedFast_StringName | .NET Framework 4.8 | .NET Framework 4.8 |     Test6 | 12.916 ns | 0.1933 ns | 0.1808 ns |  0.27 |         - |

# IsDefined Value

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22567
Unknown processor
.NET SDK=7.0.100-preview.1.22110.4
  [Host]             : .NET 7.0.0 (7.0.22.7608), X64 RyuJIT
  .NET 6.0           : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT
  .NET Framework 4.8 : .NET Framework 4.8 (4.8.9022.1), X64 RyuJIT


```
|              Method |                Job |            Runtime | EnumValue |       Mean |     Error |    StdDev | Ratio |  Gen 0 | Allocated |
|-------------------- |------------------- |------------------- |---------- |-----------:|----------:|----------:|------:|-------:|----------:|
|     **IsDefined_Value** |           **.NET 6.0** |           **.NET 6.0** |         **1** | **59.8654 ns** | **0.8602 ns** | **0.8047 ns** | **1.000** | **0.0014** |      **24 B** |
| IsDefinedFast_Value |           .NET 6.0 |           .NET 6.0 |         1 |  0.3752 ns | 0.0131 ns | 0.0122 ns | 0.006 |      - |         - |
|                     |                    |                    |           |            |           |           |       |        |           |
|     IsDefined_Value | .NET Framework 4.8 | .NET Framework 4.8 |         1 | 62.5326 ns | 0.5182 ns | 0.4594 ns |  1.00 | 0.0036 |      24 B |
| IsDefinedFast_Value | .NET Framework 4.8 | .NET Framework 4.8 |         1 |  1.6330 ns | 0.0160 ns | 0.0142 ns |  0.03 |      - |         - |
|                     |                    |                    |           |            |           |           |       |        |           |
|     **IsDefined_Value** |           **.NET 6.0** |           **.NET 6.0** |        **65** | **62.2328 ns** | **0.8922 ns** | **0.8346 ns** | **1.000** | **0.0014** |      **24 B** |
| IsDefinedFast_Value |           .NET 6.0 |           .NET 6.0 |        65 |  0.3883 ns | 0.0138 ns | 0.0129 ns | 0.006 |      - |         - |
|                     |                    |                    |           |            |           |           |       |        |           |
|     IsDefined_Value | .NET Framework 4.8 | .NET Framework 4.8 |        65 | 63.4696 ns | 0.3412 ns | 0.3024 ns |  1.00 | 0.0036 |      24 B |
| IsDefinedFast_Value | .NET Framework 4.8 | .NET Framework 4.8 |        65 |  1.4296 ns | 0.0160 ns | 0.0133 ns |  0.02 |      - |         - |


# Parse Name

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22567
Unknown processor
.NET SDK=7.0.100-preview.1.22110.4
  [Host]             : .NET 7.0.0 (7.0.22.7608), X64 RyuJIT
  .NET 6.0           : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT
  .NET Framework 4.8 : .NET Framework 4.8 (4.8.9022.1), X64 RyuJIT


```
|                   Method |                Job |            Runtime | NameValue | NameValueUpper |      Mean |    Error |   StdDev |  Gen 0 | Allocated |
|------------------------- |------------------- |------------------- |---------- |--------------- |----------:|---------:|---------:|-------:|----------:|
|               **Parse_Name** |           **.NET 6.0** |           **.NET 6.0** |     **Test1** |          **TEST1** |  **69.07 ns** | **0.188 ns** | **0.166 ns** | **0.0014** |      **24 B** |
|           ParseFast_Name |           .NET 6.0 |           .NET 6.0 |     Test1 |          TEST1 |  21.61 ns | 0.077 ns | 0.068 ns | 0.0014 |      24 B |
|     Parse_NameIgnoreCase |           .NET 6.0 |           .NET 6.0 |     Test1 |          TEST1 |  68.64 ns | 0.450 ns | 0.399 ns | 0.0014 |      24 B |
| ParseFast_NameIgnoreCase |           .NET 6.0 |           .NET 6.0 |     Test1 |          TEST1 |  37.84 ns | 0.250 ns | 0.222 ns | 0.0014 |      24 B |
|               Parse_Name | .NET Framework 4.8 | .NET Framework 4.8 |     Test1 |          TEST1 | 122.54 ns | 1.021 ns | 0.905 ns | 0.0198 |     128 B |
|           ParseFast_Name | .NET Framework 4.8 | .NET Framework 4.8 |     Test1 |          TEST1 |  15.42 ns | 0.292 ns | 0.259 ns | 0.0038 |      24 B |
|     Parse_NameIgnoreCase | .NET Framework 4.8 | .NET Framework 4.8 |     Test1 |          TEST1 | 127.91 ns | 0.848 ns | 0.751 ns | 0.0198 |     128 B |
| ParseFast_NameIgnoreCase | .NET Framework 4.8 | .NET Framework 4.8 |     Test1 |          TEST1 |  58.28 ns | 0.876 ns | 0.777 ns | 0.0036 |      24 B |
|               **Parse_Name** |           **.NET 6.0** |           **.NET 6.0** |     **Test1** |          **TEST6** |  **72.80 ns** | **1.004 ns** | **0.939 ns** | **0.0014** |      **24 B** |
|           ParseFast_Name |           .NET 6.0 |           .NET 6.0 |     Test1 |          TEST6 |  21.86 ns | 0.248 ns | 0.207 ns | 0.0014 |      24 B |
|     Parse_NameIgnoreCase |           .NET 6.0 |           .NET 6.0 |     Test1 |          TEST6 |  82.41 ns | 0.716 ns | 0.670 ns | 0.0014 |      24 B |
| ParseFast_NameIgnoreCase |           .NET 6.0 |           .NET 6.0 |     Test1 |          TEST6 |  38.04 ns | 0.796 ns | 0.744 ns | 0.0014 |      24 B |
|               Parse_Name | .NET Framework 4.8 | .NET Framework 4.8 |     Test1 |          TEST6 | 123.40 ns | 1.271 ns | 1.189 ns | 0.0198 |     128 B |
|           ParseFast_Name | .NET Framework 4.8 | .NET Framework 4.8 |     Test1 |          TEST6 |  15.53 ns | 0.261 ns | 0.244 ns | 0.0038 |      24 B |
|     Parse_NameIgnoreCase | .NET Framework 4.8 | .NET Framework 4.8 |     Test1 |          TEST6 | 162.46 ns | 1.320 ns | 1.235 ns | 0.0198 |     128 B |
| ParseFast_NameIgnoreCase | .NET Framework 4.8 | .NET Framework 4.8 |     Test1 |          TEST6 |  56.89 ns | 0.406 ns | 0.339 ns | 0.0036 |      24 B |
|               **Parse_Name** |           **.NET 6.0** |           **.NET 6.0** |     **Test6** |          **TEST1** |  **83.65 ns** | **0.511 ns** | **0.453 ns** | **0.0014** |      **24 B** |
|           ParseFast_Name |           .NET 6.0 |           .NET 6.0 |     Test6 |          TEST1 |  20.19 ns | 0.151 ns | 0.126 ns | 0.0014 |      24 B |
|     Parse_NameIgnoreCase |           .NET 6.0 |           .NET 6.0 |     Test6 |          TEST1 |  71.06 ns | 0.498 ns | 0.442 ns | 0.0014 |      24 B |
| ParseFast_NameIgnoreCase |           .NET 6.0 |           .NET 6.0 |     Test6 |          TEST1 |  37.69 ns | 0.631 ns | 0.559 ns | 0.0014 |      24 B |
|               Parse_Name | .NET Framework 4.8 | .NET Framework 4.8 |     Test6 |          TEST1 | 139.40 ns | 1.458 ns | 1.292 ns | 0.0198 |     128 B |
|           ParseFast_Name | .NET Framework 4.8 | .NET Framework 4.8 |     Test6 |          TEST1 |  26.18 ns | 0.158 ns | 0.140 ns | 0.0038 |      24 B |
|     Parse_NameIgnoreCase | .NET Framework 4.8 | .NET Framework 4.8 |     Test6 |          TEST1 | 126.82 ns | 0.983 ns | 0.821 ns | 0.0198 |     128 B |
| ParseFast_NameIgnoreCase | .NET Framework 4.8 | .NET Framework 4.8 |     Test6 |          TEST1 |  57.83 ns | 0.626 ns | 0.586 ns | 0.0036 |      24 B |
|               **Parse_Name** |           **.NET 6.0** |           **.NET 6.0** |     **Test6** |          **TEST6** |  **84.76 ns** | **0.707 ns** | **0.661 ns** | **0.0014** |      **24 B** |
|           ParseFast_Name |           .NET 6.0 |           .NET 6.0 |     Test6 |          TEST6 |  20.45 ns | 0.155 ns | 0.129 ns | 0.0014 |      24 B |
|     Parse_NameIgnoreCase |           .NET 6.0 |           .NET 6.0 |     Test6 |          TEST6 |  81.73 ns | 0.299 ns | 0.250 ns | 0.0014 |      24 B |
| ParseFast_NameIgnoreCase |           .NET 6.0 |           .NET 6.0 |     Test6 |          TEST6 |  37.42 ns | 0.284 ns | 0.252 ns | 0.0014 |      24 B |
|               Parse_Name | .NET Framework 4.8 | .NET Framework 4.8 |     Test6 |          TEST6 | 141.61 ns | 2.169 ns | 2.029 ns | 0.0198 |     128 B |
|           ParseFast_Name | .NET Framework 4.8 | .NET Framework 4.8 |     Test6 |          TEST6 |  26.12 ns | 0.160 ns | 0.142 ns | 0.0038 |      24 B |
|     Parse_NameIgnoreCase | .NET Framework 4.8 | .NET Framework 4.8 |     Test6 |          TEST6 | 163.67 ns | 1.626 ns | 1.441 ns | 0.0198 |     128 B |
| ParseFast_NameIgnoreCase | .NET Framework 4.8 | .NET Framework 4.8 |     Test6 |          TEST6 |  58.38 ns | 0.835 ns | 0.781 ns | 0.0037 |      24 B |

# Parse Value

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22567
Unknown processor
.NET SDK=7.0.100-preview.1.22110.4
  [Host]             : .NET 7.0.0 (7.0.22.7608), X64 RyuJIT
  .NET 6.0           : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT
  .NET Framework 4.8 : .NET Framework 4.8 (4.8.9022.1), X64 RyuJIT


```
|          Method |                Job |            Runtime | Value |      Mean |    Error |   StdDev |    Median | Ratio |  Gen 0 | Allocated |
|---------------- |------------------- |------------------- |------ |----------:|---------:|---------:|----------:|------:|-------:|----------:|
|     **Parse_Value** |           **.NET 6.0** |           **.NET 6.0** |     **1** |  **56.27 ns** | **0.190 ns** | **0.148 ns** |  **56.29 ns** |  **1.00** | **0.0014** |      **24 B** |
| ParseFast_Value |           .NET 6.0 |           .NET 6.0 |     1 |  12.69 ns | 0.086 ns | 0.080 ns |  12.70 ns |  0.23 | 0.0014 |      24 B |
|                 |                    |                    |       |           |          |          |           |       |        |           |
|     Parse_Value | .NET Framework 4.8 | .NET Framework 4.8 |     1 | 176.29 ns | 1.199 ns | 1.001 ns | 175.87 ns |  1.00 | 0.0110 |      72 B |
| ParseFast_Value | .NET Framework 4.8 | .NET Framework 4.8 |     1 |  55.64 ns | 1.152 ns | 1.653 ns |  54.71 ns |  0.32 | 0.0036 |      24 B |
|                 |                    |                    |       |           |          |          |           |       |        |           |
|     **Parse_Value** |           **.NET 6.0** |           **.NET 6.0** |    **64** |  **57.51 ns** | **0.383 ns** | **0.320 ns** |  **57.39 ns** |  **1.00** | **0.0014** |      **24 B** |
| ParseFast_Value |           .NET 6.0 |           .NET 6.0 |    64 |  13.56 ns | 0.136 ns | 0.120 ns |  13.57 ns |  0.24 | 0.0014 |      24 B |
|                 |                    |                    |       |           |          |          |           |       |        |           |
|     Parse_Value | .NET Framework 4.8 | .NET Framework 4.8 |    64 | 179.79 ns | 1.573 ns | 1.472 ns | 179.27 ns |  1.00 | 0.0110 |      72 B |
| ParseFast_Value | .NET Framework 4.8 | .NET Framework 4.8 |    64 |  59.91 ns | 1.207 ns | 1.292 ns |  60.01 ns |  0.33 | 0.0036 |      24 B |
