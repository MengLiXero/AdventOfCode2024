using BenchmarkDotNet.Attributes;

namespace AdventOfCode2024;

public interface IAocDay
{
    public static abstract void RunPart1();
    public static abstract void RunPart2();
}