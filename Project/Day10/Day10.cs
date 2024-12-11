using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode2024;

[MemoryDiagnoser]
public class Day10 : IAocDay
{
    private static string[] _input;
    private static int[][] _grid;
    private static HashSet<int> summitVistedForAllTrailheads = new();
    private static int _sum;
    private static bool _isPartTwo;

    private static void Initialize()
    {
        _input = File.ReadAllLines(Constants.baseDir + "Day10/data-aoc-day10.txt");
        _grid = _input
            .Select(line => line.ToCharArray()
                .Select(c => int.Parse(c.ToString()))
                .ToArray())
            .ToArray();
        _grid = AddBorder();
    }
    
    public static void RunPart1()
    {
        Initialize();
        // foreach (var line in _grid)
        // {
        //     Console.WriteLine(string.Join(" ",line));
        // }

        for (int i = 0; i < _grid.Length; i++)
        {
            for (int j = 0; j < _grid[i].Length; j++)
            {
                if (_grid[i][j] == 0)
                {
                    // Console.WriteLine($"Current trailhead: {i}, {j}");
                    FindNext(0, new Vector2(i, j), new Vector2(i,j));
                    // Console.WriteLine($"Current sum: {summitVistedForAllTrailheads.Count}");
                }
            }
        }
        // if(_isPartTwo)
        //     Console.WriteLine(_sum);
        // else
        //     Console.WriteLine(summitVistedForAllTrailheads.Count);
    }

    private static bool FindNext(int currentNumber, Vector2 location, Vector2 startLocation)
    {
        if (currentNumber == 9)
        {
            if(_isPartTwo)
                _sum++;
            else
                summitVistedForAllTrailheads.Add(HashCode.Combine((int)location.X, (int)location.Y,(int)startLocation.X,(int)startLocation.Y));
            return true; 
        }

        if (_grid[(int)location.X - 1][(int)location.Y] == currentNumber + 1)
        {
            // Console.WriteLine("up");
             FindNext(currentNumber + 1, new Vector2(location.X - 1, location.Y), startLocation);
        }
        if (_grid[(int)location.X + 1][(int)location.Y] == currentNumber + 1)
        {
            // Console.WriteLine("down");
             FindNext(currentNumber + 1, new Vector2(location.X + 1, location.Y), startLocation);
        }
        if (_grid[(int)location.X][(int)location.Y - 1] == currentNumber + 1)
        {
            // Console.WriteLine("left");
             FindNext(currentNumber + 1, new Vector2(location.X, location.Y - 1), startLocation);
        }
        if (_grid[(int)location.X][(int)location.Y + 1] == currentNumber + 1)
        {
            // Console.WriteLine("right");
             FindNext(currentNumber + 1, new Vector2(location.X, location.Y + 1), startLocation);
        }

        return false; // Return true if any branch finds a valid path.
    }

    public static void RunPart2()
    {
        _isPartTwo = true;
        RunPart1();
    }

    [Benchmark]
    public void BenchmarkPart1()=> RunPart1();

    [Benchmark]
    public void BenchmarkPart2()=> RunPart2();
    
    
    private static int[][] AddBorder(int boarderNumber = -1)
    {
        // Create the top and bottom border row
        int[] borderRow = Enumerable.Repeat(boarderNumber, _grid[0].Length + 2).ToArray();

        // Add -1 to the sides of each row and build the bordered grid
        var borderedGrid = _grid
            .Select(row => new[] { boarderNumber }.Concat(row).Concat(new[] { boarderNumber }).ToArray())
            .Prepend(borderRow)
            .Append(borderRow)
            .ToArray();

        return borderedGrid;
    }
}