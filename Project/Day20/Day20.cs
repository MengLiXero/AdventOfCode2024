using System.Numerics;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdventOfCode2024.Day20;

public class Day20 : Grid, IAocDay
{
    private static char[][] _grid;
    private static HashSet<(int, int)> _fragileWalls = new ();
    private const int SavedPicoseconds = 100;


    public static void RunPart1()
    {
        Initialize();
        FindAllFragileWalls();
        var start = LocateAValue(_grid, 'S');
        var end = LocateAValue(_grid, 'E');
        var count = 0;
        var currentShortestDistance = BfsFindingShortestDistance(_grid, ((int)start.X, (int)start.Y), ((int)end.X, (int)end.Y));

        FindShortestDistanceWithCheat(start, end, currentShortestDistance, count);
        Console.WriteLine($"Number of cheats would save more than {SavedPicoseconds} picoseconds: {count}");
    }

    private static void FindShortestDistanceWithCheat(Vector2 start, Vector2 end, int currentShortestDistance, int count)
    {
        foreach (var fragileWall in _fragileWalls)
        {
            var temp = _grid[fragileWall.Item1][fragileWall.Item2];
            _grid[fragileWall.Item1][fragileWall.Item2] = '.';
            if (fragileWall.Item1 == 7 && fragileWall.Item2 == 6)
            {
                Console.WriteLine("here");
            }

            var cheatShortestDistance =
                BfsFindingShortestDistance(_grid, ((int)start.X, (int)start.Y), ((int)end.X, (int)end.Y));
            _grid[fragileWall.Item1][fragileWall.Item2] = temp;
            if (cheatShortestDistance <= currentShortestDistance - SavedPicoseconds)
            {
                Console.WriteLine($"Saved picoseconds: {currentShortestDistance - cheatShortestDistance}");
                Console.WriteLine($"Fragile wall at {fragileWall.Item1}, {fragileWall.Item2} is removed");
                count++;
            }
        }
    }

    private static void FindAllFragileWalls()
    {
        for (var i = 1; i < _grid.Length-1; i++)
        {
            for (var j = 1; j < _grid[i].Length-1; j++)
            {
                if (_grid[i][j] != '#') continue;
                if (!IsFragileWall(i, j)) continue;
                _fragileWalls.Add((i, j));
                Console.WriteLine($"Fragile wall found at {i}, {j}");
            }
        }
    }

    private static bool IsFragileWall(int row, int col)
    {
        if ((_grid[row - 1][col] != '#' && _grid[row + 1][col] != '#') ||
            (_grid[row][col - 1] != '#' && _grid[row][col + 1] != '#'))
        {
            return true;
        }

        return false;
    }

    private static void Initialize()
    {
        var input = File.ReadAllLines(Constants.baseDir + "/Day20/data-aoc-day20.txt");
        _grid = PopulateGrid<char>(input);
        PrintGrid(_grid);
    }

    public static void RunPart2()
    {
        throw new NotImplementedException();
    }

    public void BenchmarkPart1()
    {
        throw new NotImplementedException();
    }

    public void BenchmarkPart2()
    {
        throw new NotImplementedException();
    }
}