using System.Data;
using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdventOfCode2024;

public class Day5 : IAocDay
{
    private static string[] _input;
    private static List<int> _leftList = new();
    private static List<int> _rightList = new();
    private static int[][] _dataArray;
    private static int lastValue = -1;
    private static int sum;
    private static bool found = false;
    private static List<int[]> reOrderedList = new();
    private static bool modified;

    private static void Initialize()
    {
        _input = File.ReadAllLines(Constants.baseDir + "Day5/data-aoc-day5.txt");
        foreach (var line in _input)
        {
            if (line.IndexOf('|') != -1)
            {
                var leftValue = int.Parse(line.Substring(0, line.IndexOf('|')));
                _leftList.Add(leftValue);
                var rightValue = int.Parse(line.Substring(line.IndexOf('|') + 1));
                _rightList.Add(rightValue);
            }
        }

        _dataArray = _input.Where(line => line.IndexOf(',') != -1).Select(line =>
            line.Split(',').Select(int.Parse).ToArray()).ToArray();
    }

    public static void RunPart1()
    {
        Initialize();
        foreach (var line in _dataArray)
        {
            for (int i = 0; i < line.Length - 1; i++)
            {
                if (FindMatch(line, i, 0))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                sum += line[line.Length / 2];
            }
            found = false;
        }

        Console.WriteLine(sum);
    }

    private static bool FindMatch(int[] line, int i, int start)
    {
        var index = _leftList.IndexOf(line[i + 1], start);
        if (index == -1 || start + 1 >= _leftList.Count || found)
        {
            return false;
        }

        if (_rightList[index] == line[i])
        {
            return true;
        }

        return FindMatch(line, i, index + 1);
    }
    
    public static void RunPart2()
    {
        Console.WriteLine("Day 5, Part 2");
        Initialize();
        foreach (var line in _dataArray)
        {
            for (int i = 0; i < line.Length - 1; i++)
            {
                if (FindMatch(line, i, 0))
                {
                    Console.WriteLine($"line: {line.ToArray()}");
                    modified = true;
                    (line[i + 1], line[i]) = (line[i], line[i + 1]);
                    if (FindMatch(line, i, 0))
                    {
                        reOrderedList.Remove(line);
                        modified = false;
                        Array.Resize(ref _dataArray, _dataArray.Length + 1);
                        _dataArray[^1] = line;
                        break;
                    }
                }
            }
            if (modified)
            {
                sum += line[line.Length / 2];
            }
            modified = false;
        }

        foreach (var array in reOrderedList)
        {
            Console.WriteLine(string.Join(", ", array));
        }
        
        Console.WriteLine(sum);
    }

    [Benchmark]
    public void BenchmarkPart1()
    {
        throw new NotImplementedException();
    }

    [Benchmark]
    public void BenchmarkPart2()
    {
        throw new NotImplementedException();
    }
}