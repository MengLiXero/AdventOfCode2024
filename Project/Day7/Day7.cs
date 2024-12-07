namespace AdventOfCode2024.Day7;

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

        Console.WriteLine($"Total value is : {total}");
    }

    private static void recursiveCalculation(long[] operands, int index, long sum, long answer, HashSet<long> resultsArray)
    {
        if (index == operands.Length)
        {
            // Console.WriteLine("End of current calculation");
            resultsArray.Add(sum);
            return;
        }

        if (resultsArray.Contains(answer))
        {
            return;
        }
        
        if(sum>answer)
        {
            return;
        }
        
        recursiveCalculation(operands, index + 1, sum * operands[index], answer, resultsArray);
        recursiveCalculation(operands, index + 1, sum + operands[index], answer, resultsArray);
        recursiveCalculation(operands, index + 1, long.Parse(sum.ToString() + operands[index].ToString()), answer, resultsArray);
    }

    public static void RunPart2()
    {
        throw new NotImplementedException();
    }
}