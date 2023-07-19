
- `ValueStringBuilder` is useful for string concatentation over small strings. I've omitted benchmarking appending single string for ValueStringBuilder, because when string interpolation is used, under-the-hood it will create [DefaultInterpolatedStringHandler](https://devblogs.microsoft.com/dotnet/string-interpolation-in-c-10-and-net-6/) which slows final result.
- All benchmarks set initial capacity over string builders, so we don't waste time and memory for resizing the underlying buffer
- For general purpose you should use `StringBuilder`

```

BenchmarkDotNet v0.13.6, Windows 11 (10.0.22621.1992/22H2/2022Update/SunValley2)
11th Gen Intel Core i5-11400F 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.100-preview.5.23303.2
  [Host]     : .NET 7.0.8 (7.0.823.31807), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.8 (7.0.823.31807), X64 RyuJIT AVX2


```
|                                     Method | Params |         Mean |        Error |      StdDev | Ratio | RatioSD |      Gen0 |    Gen1 |  Allocated | Alloc Ratio |
|------------------------------------------- |------- |-------------:|-------------:|------------:|------:|--------:|----------:|--------:|-----------:|------------:|
|                                     **Concat** |     **10** |     **480.4 ns** |      **8.81 ns** |    **16.34 ns** |  **4.28** |    **0.19** |    **0.2470** |       **-** |     **1552 B** |        **6.69** |
|             StingBuilderAppendSingleString |     10 |     165.3 ns |      3.36 ns |     5.43 ns |  1.49 |    0.06 |    0.0725 |       - |      456 B |        1.97 |
|                 StingBuilderSeveralAppends |     10 |     137.2 ns |      1.70 ns |     1.51 ns |  1.21 |    0.02 |    0.0725 |       - |      456 B |        1.97 |
|     ValueStringBuilderPooledSeveralAppends |     10 |     131.8 ns |      2.28 ns |     2.13 ns |  1.16 |    0.02 |    0.0370 |       - |      232 B |        1.00 |
| ValueStringBuilderStackallocSeveralAppends |     10 |     113.8 ns |      1.21 ns |     1.07 ns |  1.00 |    0.00 |    0.0370 |       - |      232 B |        1.00 |
|                                            |        |              |              |             |       |         |           |         |            |             |
|                                     **Concat** |    **100** |   **8,364.2 ns** |     **52.24 ns** |    **40.79 ns** |  **4.65** |    **0.07** |   **16.0980** |  **0.0763** |   **101008 B** |       **20.94** |
|             StingBuilderAppendSingleString |    100 |   1,543.4 ns |     15.86 ns |    13.24 ns |  0.86 |    0.01 |    0.6332 |  0.0038 |     3984 B |        0.83 |
|                 StingBuilderSeveralAppends |    100 |   1,332.6 ns |      9.59 ns |     8.50 ns |  0.74 |    0.01 |    0.6332 |  0.0038 |     3984 B |        0.83 |
|     ValueStringBuilderPooledSeveralAppends |    100 |   1,829.5 ns |     36.32 ns |    62.65 ns |  0.99 |    0.05 |    0.7687 |       - |     4824 B |        1.00 |
| ValueStringBuilderStackallocSeveralAppends |    100 |   1,797.4 ns |     23.98 ns |    22.43 ns |  1.00 |    0.00 |    0.7687 |       - |     4824 B |        1.00 |
|                                            |        |              |              |             |       |         |           |         |            |             |
|                                     **Concat** |   **1000** | **587,707.6 ns** | **10,620.42 ns** | **9,934.35 ns** | **30.27** |    **0.60** | **1651.3672** | **79.1016** | **10376304 B** |      **197.57** |
|             StingBuilderAppendSingleString |   1000 |  15,841.2 ns |    176.29 ns |   156.27 ns |  0.81 |    0.01 |    6.8054 |  0.2747 |    42880 B |        0.82 |
|                 StingBuilderSeveralAppends |   1000 |  15,041.7 ns |    216.94 ns |   192.31 ns |  0.77 |    0.02 |    6.8054 |  0.2747 |    42880 B |        0.82 |
|     ValueStringBuilderPooledSeveralAppends |   1000 |  19,984.2 ns |    163.55 ns |   152.99 ns |  1.03 |    0.02 |    8.3313 |       - |    52520 B |        1.00 |
| ValueStringBuilderStackallocSeveralAppends |   1000 |  19,445.9 ns |    291.44 ns |   258.35 ns |  1.00 |    0.00 |    8.3313 |       - |    52520 B |        1.00 |
