using System.Numerics;
using BenchmarkDotNet.Attributes;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;

namespace AdventOfCode2024;

[MemoryDiagnoser]
public class Day8 : IAocDay
{
    private static string[] _input;
    private static char[][] _grid;
    private static HashSet<Vector2> _result = new();
    private static List<Vector2> signalList = new();

    private static void Initialize()
    {
        _input = File.ReadAllLines(Constants.baseDir + "Day8/data-aoc-day8.txt");
        _grid = _input.Select(line => line.ToCharArray()).ToArray();
    }

    public static void RunPart1()
    {
        // Console.WriteLine("Day 8, Part 1");
        Initialize();
        var numOfSignals = _grid.SelectMany(subArray => subArray).Count(c => c != '.');
        Vector2 coordinate = new Vector2(0, 0);

        while (signalList.Count < numOfSignals)
        {
            FindAntinodes(coordinate, false);
        }

        // Console.WriteLine($"Number of Antinodes: {_result.Count}");
        // foreach (var vector in signalList)
        // {
        //     Console.WriteLine($"X: {vector.X}, Y: {vector.Y}");
        // }
        // foreach (var vector in _result)
        // {
        //     if(_grid[(int)vector.X][(int)vector.Y] == '.')
        //         _grid[(int)vector.X][(int)vector.Y] = '#';
        // }
        // foreach (var subArray in _grid)
        // {
        //     Console.WriteLine(subArray);
        // }
    }

    private static void FindAntinodes(Vector2 coordinate, bool isPart2)
    {
        char? signal = null;
        for (int i = 0; i < _grid.Length; i++)
        {
            for (int j = 0; j < _grid[i].Length; j++)
            {
                if (_grid[i][j] != '.' && signal == null && !signalList.Contains(new Vector2(i, j)))
                {
                    signal = _grid[i][j];
                    signalList.Add(new Vector2(i, j));
                    coordinate = new Vector2(i, j);
                }
                else if (_grid[i][j] == signal && !signalList.Contains(new Vector2(i, j)))
                {
                    Vector2 offset = new Vector2(i, j) - coordinate;
                    FindAllAntinodes1(coordinate - offset, offset, isPart2);
                    FindAllAntinodes2(new Vector2(i, j) + offset, offset, isPart2);
                    if (isPart2)
                    {
                        _result.Add(coordinate);
                        _result.Add(new Vector2(i, j));
                    }
                }
            }
        }
    }

    private static void FindAllAntinodes1(Vector2 coordinate, Vector2 offset, bool isPart2)
    {
        if (BoundaryCheck(coordinate))
        {
            _result.Add(coordinate);
            if (isPart2)
                FindAllAntinodes1(coordinate - offset, offset, isPart2);
        }
    }

    private static void FindAllAntinodes2(Vector2 coordinate, Vector2 offset, bool isPart2)
    {
        if (BoundaryCheck(coordinate))
        {
            _result.Add(coordinate);
            if(isPart2)
                FindAllAntinodes2(coordinate + offset, offset, isPart2);
        }
    }


    private static bool BoundaryCheck(Vector2 newCoordinate1)
    {
        return newCoordinate1.X > -1 && newCoordinate1.Y > -1 && newCoordinate1.X < _grid.Length &&
               newCoordinate1.Y < _grid[0].Length;
    }

    public static void RunPart2()
    {
        // Console.WriteLine("Day 8, Part 2");
        Initialize();
        var numOfSignals = _grid.SelectMany(subArray => subArray).Count(c => c != '.');
        Vector2 coordinate = new Vector2(0, 0);

        while (signalList.Count < numOfSignals)
        {
            FindAntinodes(coordinate,true);
        }

        // Console.WriteLine($"Number of Antinodes: {_result.Count}");
    }
    
    [Benchmark]
    public void BenchmarkPart1() => RunPart1();

    [Benchmark]
    public void BenchmarkPart2() => RunPart2();
}