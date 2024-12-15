using System.Numerics;

namespace AdventOfCode2024;

public abstract class Grid
{
    private static string[] _input;
    protected static int[][]? _intGrid;
    protected static char[][] _charGrid;
    
    protected static void PopulateGrids(string path)
    {
        _input = File.ReadAllLines(Constants.baseDir + path);
        if (_input.All(line => line.Split(' ').All(value => int.TryParse(value, out _))))
        {
            _intGrid = _input
                .Select(line => line.Split(' ')
                    .Select(int.Parse)
                    .ToArray())
                .ToArray();
        }
        else
        {
            _intGrid = null;
        }
        
        _charGrid = _input
            .Select(line => line.ToCharArray())
            .ToArray();
    }
    
    protected static void PrintGrid<T>(T[][]? grid)
    {
        if (grid != null)
        {
            foreach (var line in grid)
            {
                Console.WriteLine(string.Join(" ", line));
            }
        }
    }
    
    protected static T[][] AddBoarder<T>(T[][] grid, T value)
    {
        T[] borderRow = Enumerable.Repeat(value, grid[0].Length + 2).ToArray();

        var borderedGrid = grid
            .Select(row => new[] { value }.Concat(row).Concat(new[] { value }).ToArray())
            .Prepend(borderRow)
            .Append(borderRow)
            .ToArray();

        return borderedGrid;
    }
    
    protected static Vector2 LocateAValue<T>(T[][] grid, T value)
    {
        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid[i].Length; j++)
            {
                if (grid[i][j].Equals(value))
                {
                    return new Vector2(i,j);
                }
            }
        }
        return new Vector2(-1,-1);
    }
}