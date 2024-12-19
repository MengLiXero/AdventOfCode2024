namespace AdventOfCode2024.Day16;

public class Day16 : Grid, IAocDay
{
    private static string[] _input;
    private static char[][] _grid;
    private static void Initialize()
    {
        _input = File.ReadAllLines(Constants.baseDir + $"/Day16/data-aoc-day16.txt");
        _grid = PopulateGrid<char>(_input, GridType.Char);
        PrintGrid(_grid);
    }

    public static void RunPart1()
    {
        Initialize();
        
        var startPosition = LocateAValue(_grid, 'S');
        var endPosition = LocateAValue(_grid, 'E');
        var start = ((int)startPosition.X, (int)startPosition.Y);
        var end = ((int)endPosition.X, (int)endPosition.Y);
        var minScore = DijkstraShortestDistanceInWeightedGraph(_grid,start, end);
        Console.WriteLine($"Minimum score: {minScore}");
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