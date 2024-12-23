using Microsoft.Diagnostics.Tracing.Parsers.ClrPrivate;

namespace AdventOfCode2024.Day18;

public class Day18 : Grid, IAocDay
{
    // a hashset?
    private static List<(int, int)> _input;
    private const int _row = 71;
    private const int _col = 71;
    private static char[][] _grid = new char[_row][];
    private static int _numberOfFallingBytes = 1024;

    private static void initialize()
    {
        _input = File.ReadAllLines(Constants.baseDir + "/Day18/data-aoc-day18.txt")
            .Select(line => line.Split(','))
            .Select(parts => (int.Parse(parts[0]), int.Parse(parts[1])))
            .ToList();
        _grid = new char[_row][];
        var truncatedInput = _input.Take(_numberOfFallingBytes).ToList();

        for (int i = 0; i < _row; i++)
        {
            _grid[i] = new char[_col];
            for (int j = 0; j < _col; j++)
            {
                if (truncatedInput.Contains((j, i)))
                    _grid[i][j] = '#';
                else
                    _grid[i][j] = '.';
            }
        }

        _grid = AddBoarder<char>(_grid, '#');
        // PrintGrid<char>(_grid);
    }

    public static void RunPart1()
    {
        initialize();
        var truncatedInput = _input.Take(_numberOfFallingBytes).ToList();
        if (truncatedInput.Contains((0, 0)) || truncatedInput.Contains((70, 70)))
        {
            throw new Exception("Start/End point is corrupted");
        }

        var minSteps = BfsFindingShortestDistance(_grid, (1,1),(_grid.Length - 2, _grid[0].Length - 2));
        Console.WriteLine($"Minimum steps: {minSteps}");
    }

    public static void RunPart2()
    {
        initialize();
        for (int i = _numberOfFallingBytes - 1; i < _input.Count; i++)
        {
            initialize();
            _baseVisited = new List<(int, int)>();
            var found = DfsRouteFinding(_grid, (_grid.Length - 2, _grid[0].Length - 2), (1, 1));
            if (!found)
            {
                Console.WriteLine($"Couldn't find a route after: {_input[i]}");
                break;
            }

            _numberOfFallingBytes++;
        }
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