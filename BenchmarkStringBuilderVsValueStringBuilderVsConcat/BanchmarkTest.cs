using BenchmarkDotNet.Attributes;
using System.Text;

namespace BenchmarkStringBuilderVsValueStringBuilderVsConcat;

[MemoryDiagnoser]
public class BanchmarkTest
{
    [Params(10, 100, 1000)]
    public int Params { get; set; }
    public int[] Numbers { get; set; } = default!;
    public int MaxLength { get; set; }

    public const string Odd = nameof(Odd);
    public const string Even = nameof(Even);

    [GlobalSetup]
    public void Setup()
    {
        Numbers = Enumerable.Range(1, Params).ToArray();
        MaxLength = Params switch
        {
            <= 10 => Params * 9,
            <= 100 => Params * 10,
            <= 1000 => Params * 11,
            _ => 0,
        };
    }

    [Benchmark]
    public string Concat()
    {
        var str = string.Empty;
        foreach (var number in Numbers)
        {
            str += $"{number} is {(number % 2 == 0 ? Even : Odd)}";
        }

        return str;
    }

    [Benchmark]
    public string StingBuilderAppendSingleString()
    {
        var sb = new StringBuilder(MaxLength);
        foreach (var number in Numbers)
        {
            sb.Append($"{number} is {(number % 2 == 0 ? Even : Odd)}");
        }

        return sb.ToString();
    }

    [Benchmark]
    public string StingBuilderSeveralAppends()
    {
        var sb = new StringBuilder(MaxLength);
        foreach (var number in Numbers)
        {
            sb.Append(number);
            sb.Append(" is ");
            sb.Append(number % 2 == 0 ? Even : Odd);
        }

        return sb.ToString();
    }

    [Benchmark]
    public string ValueStringBuilderPooledSeveralAppends()
    {
        var vsb = new ValueStringBuilder(MaxLength);
        foreach (var number in Numbers)
        {
            vsb.Append(number.ToString());
            vsb.Append(" is ");
            vsb.Append(number % 2 == 0 ? Even : Odd);
        }

        return vsb.ToString();
    }

    [Benchmark(Baseline = true)]
    public string ValueStringBuilderStackallocSeveralAppends()
    {
        var vsb = new ValueStringBuilder(stackalloc char[MaxLength]);
        foreach (var number in Numbers)
        {
            vsb.Append(number.ToString());
            vsb.Append(" is ");
            vsb.Append(number % 2 == 0 ? Even : Odd);
        }

        return vsb.ToString();
    }
}
