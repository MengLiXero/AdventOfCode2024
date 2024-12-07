using BenchmarkDotNet.Attributes;

namespace AdventOfCode2024;

[MemoryDiagnoser]
public class Day7 : IAocDay
{
    private static string[] _input;
    private static long[] _results;
    private static long[][] _operands;

    private static void Initialize()
    {
        _input = File.ReadAllLines(Constants.baseDir + "Day7/data-aoc-day7.txt");
        _results = _input.Select(line => long.Parse(line.Substring(0, line.IndexOf(':')))).ToArray();
        _operands = _input.Select(line => line.Substring(line.IndexOf(':') + 2).Split(' ').Select(long.Parse).ToArray())
            .ToArray();
        if (_results.Length != _operands.Length)
        {
            throw new Exception("Results and Operands are not equal");
        }
    }

    public static void RunPart1()
    {
        Initialize();
        long total = 0;
        for (int i = 0; i < _results.Length; i++)
        {
            var sum = _operands[i][0];
            var resultsArray = new HashSet<long>();
            recursiveCalculation(_operands[i], 1, sum, _results[i], resultsArray);
            // Console.WriteLine("Results list: ");
            foreach (var result in resultsArray)
            {
                // Console.WriteLine(result);
                if (_results[i] == result)
                {
                    total += _results[i];
                }
            }
        }

        // Console.WriteLine($"Total value is : {total}");
    }

    private static void recursiveCalculation(long[] operands, int index, long sum, long answer,
        HashSet<long> resultsSet)
    {
        if (index == operands.Length)
        {
            // Console.WriteLine("End of current calculation");
            resultsSet.Add(sum);
            return;
        }

        if (resultsSet.Contains(answer))
        {
            return;
        }

        if (sum > answer)
        {
            return;
        }

        recursiveCalculation(operands, index + 1, sum * operands[index], answer, resultsSet);
        recursiveCalculation(operands, index + 1, sum + operands[index], answer, resultsSet);
        // Part 2
        recursiveCalculation(operands, index + 1, long.Parse(sum.ToString() + operands[index].ToString()), answer,
            resultsSet);
    }

    public static void RunPart2()
    {
        RunPart1();
    }

    [Benchmark]
    public void BenchmarkPart1() => Day7.RunPart1();

    [Benchmark]
    public void BenchmarkPart2() => Day7.RunPart2();
}