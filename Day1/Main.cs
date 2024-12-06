using BenchmarkDotNet.Running;

namespace AdventOfCode2024;

public class EntryPoint
{
    public static void Main(string[] args)
    {
        //Day1.Index();
        //Day2.Index();
        //Day3.Index();
        // Day4.Index();
        //Day5.Index();
        // Day6.Index();
        BenchmarkRunner.Run<Day6>();
    }
    
}