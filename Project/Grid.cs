using System.Numerics;

namespace AdventOfCode2024;

public abstract class Grid
{
    protected static List<(int, int)> _baseVisited = new List<(int, int)>();

    protected static T[][] PopulateGrid<T>(string[] input, string gridType)
    {
        switch (gridType)
        {
            case "int":
                if (typeof(T) == typeof(int) &&
                    input.All(line => line.Split(' ').All(value => int.TryParse(value, out _))))
                {
                    return input
                        .Select(line => line.Split(' ')
                            .Select(int.Parse)
                            .Cast<T>()
                            .ToArray())
                        .ToArray();
                }

                break;

            case "char":
                if (typeof(T) == typeof(char))
                {
                    return input
                        .Select(line => line.ToCharArray().Cast<T>().ToArray())
                        .ToArray();
                }

                break;

            default:
                return new T[][] { };
        }

        return new T[][] { };
    }

    protected static void PrintGrid<T>(T[][]? grid)
    {
        if (grid != null)
        {
            for (int i = 0; i < grid.Length; i++)
            {
                Console.Write(i);
                Console.WriteLine(string.Join("", grid[i]));
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
                    return new Vector2(i, j);
                }
            }
        }

        return new Vector2(-1, -1);
    }

    protected static int BfsFindingShortestDistance(char[][] grid, (int, int) destination)
    {
        var visited = new HashSet<(int, int)>();
        var start = (row: 1, col: 1, steps: 0);
        visited.Add((start.row, start.col));
        var queue = new Queue<(int row, int col, int steps)>();
        queue.Enqueue(start);
        var directions = new List<(int row, int col)>()
        {
            (0, 1),
            (0, -1),
            (1, 0),
            (-1, 0)
        };
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.row == destination.Item1 && current.col == destination.Item2)
            {
                return current.steps;
            }

            foreach (var dir in directions)
            {
                if (grid[current.row + dir.row][current.col + dir.col] == '#')
                    continue;
                var next = (current.row + dir.row, current.col + dir.col, current.steps + 1);
                if (visited.Contains((next.Item1, next.Item2))) continue;
                queue.Enqueue(next);
                visited.Add((next.Item1, next.Item2));
            }
        }

        return -1;
    }

    protected static bool DfsRouteFinding(char[][] grid, (int, int) destination, (int, int) current)
    {
        if (current.Item1 == destination.Item1 && current.Item2 == destination.Item2)
        {
            return true;
        }

        var directions = new List<(int row, int col)>()
        {
            (0, 1),
            (0, -1),
            (1, 0),
            (-1, 0)
        };
        foreach (var next in from dir in directions
                 select (current.Item1 + dir.row, current.Item2 + dir.col)
                 into next
                 where grid[next.Item1][next.Item2] != '#'
                 where !_baseVisited.Contains((next.Item1, next.Item2))
                 select next)
        {
            _baseVisited.Add((next.Item1, next.Item2));
            if (DfsRouteFinding(grid, destination, next))
            {
                return true;
            }
        }

        return false;
    }
}