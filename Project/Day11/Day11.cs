using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode2024;

public class Day11 : IAocDay
{
    private static long[] _input;
    private static List<long> newList;
    private static List<long[]> _input2;
    private static Dictionary<long,long> _spawn35 = new Dictionary<long, long>();

    private static void Initialize()
    {
        _input = File.ReadAllText(Constants.baseDir + "Day11/data-aoc-day11.txt").Split(' ').ToArray()
            .Select(a => long.Parse(a.ToString())).ToArray();
    }

    public static void RunPart1()
    {
        Initialize();

        var blinkTimes = 0;
        while (blinkTimes < 25)
        {
            Console.WriteLine($"Blinking...{blinkTimes}");
            newList = new List<long>();
            for (int i = 0; i < _input.Length; i++)
            {
                if (_input[i] == 0)
                {
                    newList.Add(1);
                    continue;
                }

                var inputString = _input[i].ToString();
                if (inputString.Length % 2 == 0)
                {
                    newList.Add(int.Parse(inputString.Substring(0, inputString.Length / 2)));
                    newList.Add(int.Parse(inputString.Substring(inputString.Length / 2)));
                    continue;
                }

                newList.Add(_input[i] * 2024);
            }

            _input = newList.ToArray();
            blinkTimes++;
            // foreach (var element in newList)
            // {
            //     Console.WriteLine(string.Join(" ",element));
            //     
            // }
        }

        Console.WriteLine($"Number of stones: {newList.Count}");
    }

    public static void RunPart2()
    {
        Initialize();

        var blinking40 = Blinking(40, _input);

        BigInteger sum = 0;
        foreach (var element in blinking40)
        {
            if (_spawn35.ContainsKey(element))
            {
                sum+=_spawn35[element];
                continue;
            }
            long count = Blinking(35, new long[]{element}).Length;
            _spawn35[element] = count;
            sum += count;
        }
        Console.WriteLine($"Number of stones: {sum}");
    }

    private static long[] Blinking(int limit, long[] input)
    {
        var start = 0;
        while (start < limit)
        {
            // if (start % 10 == 0)
            // {
            //     // Console.WriteLine($"Blinking...{start}");
            // }

            newList = new List<long>();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == 0)
                {
                    newList.Add(1);
                    continue;
                }

                var inputString = input[i].ToString();
                if (inputString.Length % 2 == 0)
                {
                    newList.Add(int.Parse(inputString.Substring(0, inputString.Length / 2)));
                    newList.Add(int.Parse(inputString.Substring(inputString.Length / 2)));
                    continue;
                }
                newList.Add(input[i] * 2024);
                
                
            }

            input = newList.ToArray();
            start++;
            // foreach (var element in newList)
            // {
            //     Console.WriteLine(string.Join(" ",element));
            //     
            // }

            
        }
        
        

        return input;
    }

    [Benchmark]
    public void BenchmarkPart1()=> 
        RunPart1();

    [Benchmark]
    public void BenchmarkPart2()=>
        RunPart2();
}