using System.Data;
using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdventOfCode2024;

public class Day5 : IAocDay
{
    private static string[] _input;
    private static List<int> _leftList = new();
    private static List<int> _rightList = new();
    private static int[] _leftArray;
    private static int[] _rightArray;
    private static int[][] _dataArray;
    private static List<int> _sortedList = new ();
    private static Dictionary<int,int> occurences = new();
    private static int lastValue = -1;

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

        _dataArray = _input.Where(line=>line.IndexOf(',') != -1).Select(line =>
            line.Split(',').Select(int.Parse).ToArray()).ToArray();
        _leftArray = _leftList.ToArray();
        _rightArray = _rightList.ToArray();
    }

    public static void RunPart1()
    {
        Initialize();
        int result = 0;
        foreach (var value in _leftArray)
        {
            occurences[value] = _rightArray.Count(x => x == value);
        }

        //Check if it's a loop
        var firstValue = _leftArray.Except(_rightArray).ToList().Single();
        var lastValue = _rightArray.Except(_leftArray).ToList().Single();
        _sortedList.Add(firstValue);
        
        FindNextSmallest(firstValue,0);
        _sortedList.Add(lastValue);
        // _sortedList.ForEach(Console.WriteLine);
        Dictionary<int,int> _sortedDict = _sortedList.Select((value, index) => new {value, index})
            .ToDictionary(x => x.value, x => x.index);
        foreach (var line in _dataArray)
        {
            int[] order = line.Select(x=>_sortedDict[x]).ToArray();
            if (order.Zip(order.Skip(1), (a, b) => a <= b).All(x => x))
            {
                if (line.Length % 2 == 0)
                {
                    throw new DataException("the length of the list to be compared is not an odd number");
                }
                result += line[line.Length / 2];
            }
            }
        Console.WriteLine($"The result is : {result}");
    }

    private static void FindNextSmallest(int value, int startIndex)
    {
        if(startIndex >= _leftList.Count)
            return;
        if(_sortedList.Count == _leftList.Count)
            return;
        if (_leftList.IndexOf(value, startIndex) == -1)
            return;
        var candidate = _rightList[_leftList.IndexOf(value, startIndex)];
        if (occurences.ContainsKey(candidate)&&(occurences[candidate] <= _sortedList.Count))
        {
            _sortedList.Add(candidate);
            FindNextSmallest(candidate,0);
        }
        else
        {
            FindNextSmallest(value, _leftList.IndexOf(value, startIndex) + 1);
        }
    }
    

    public static void RunPart2()
    {
        throw new NotImplementedException();
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