using System.Numerics;
using AdventOfCode2024.Day16;

namespace AdventOfCode2024;

public abstract class Grid
{
    protected static List<(int, int)> _baseVisited = new List<(int, int)>();

    protected static Dictionary<(int, int), DirectionType> directionMap = new Dictionary<(int, int), DirectionType>()
    {
        { (-1, 0), DirectionType.Up }, { (1, 0), DirectionType.Down }, { (0, -1), DirectionType.Left },
        { (0, 1), DirectionType.Right }
    };

    protected static T[][] PopulateGrid<T>(string[] input, GridType gridType)
    {
        switch (gridType)
        {
            case GridType.Int:
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

            case GridType.Char:
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
                // Console.Write(i);
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

    protected static int DijkstraShortestDistanceInWeightedGraph(char[][] grid, (int, int) start, (int, int) end)
    {
        var directions = new List<(int row, int col)>()
        {
            (0, 1),
            (0, -1),
            (1, 0),
            (-1, 0)
        };
        var visited = new HashSet<(int, int)>();
        var priorityQueue = new PriorityQueue<Node,int>();
        var predecessors = new Dictionary<(int,int), (int,int)>();
        var allNodes = new Dictionary<(int, int), Node>();
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
                if (grid[i][j] == '.' || grid[i][j] == 'S' || grid[i][j] == 'E')
                {
                    var currentNode = new Node(i, j, int.MaxValue, null, DirectionType.None);
                    allNodes.Add((i, j), currentNode);
                }
        }

        allNodes[start].Distance = 0;
        allNodes[start].Direction = DirectionType.Right;
        var current = allNodes[start];
        var next = (-1, -1);
        priorityQueue.Enqueue(current, current.Distance);
        while (true)
        {
            if (priorityQueue.Count == 0)
            {
                throw new Exception("No path found");
            }
            current = priorityQueue.Dequeue();
            if ((current.Row,current.Col) == end)
            {
                break;
            }
            visited.Add((current.Row, current.Col));
            foreach (var dir in directions)
            {
                next = (current.Row + dir.row, current.Col + dir.col);
                
                if (allNodes.ContainsKey(next) && !visited.Contains(next))
                {
                    if (current.Direction == DirectionType.None || current.Direction == directionMap[dir])
                    {
                        if (allNodes[next].Distance > current.Distance + 1)
                        {
                            allNodes[next].Distance = current.Distance + 1;
                            if(predecessors.ContainsKey((next.Item1,next.Item2)))
                                predecessors[(next.Item1,next.Item2)] = (current.Row,current.Col);
                            else
                                predecessors.Add((next.Item1,next.Item2), (current.Row,current.Col));
                            allNodes[next].Direction = directionMap[dir];
                            priorityQueue.Enqueue(allNodes[next], allNodes[next].Distance);
                        }
                    }
                    else
                    {
                        if (allNodes[next].Distance > current.Distance + 1001)
                        {
                            allNodes[next].Distance = current.Distance + 1001;
                            if (predecessors.ContainsKey((next.Item1, next.Item2)))
                            {
                                predecessors[(next.Item1, next.Item2)] = (current.Row, current.Col);
                            }else
                                predecessors.Add((next.Item1,next.Item2), (current.Row,current.Col));
                            allNodes[next].Direction = directionMap[dir];
                            priorityQueue.Enqueue(allNodes[next], allNodes[next].Distance);
                        }
                    }
                }
            }
        }
        grid = CombineRouteAndGrid(grid, allNodes,RetrieveRoute(predecessors, end));
        PrintGrid(grid);
        return allNodes[end].Distance;
    }

    private static char[][] CombineRouteAndGrid(char[][] grid, Dictionary<(int, int), Node> allNodes, IEnumerable<(int, int)> route)
    {
        if (grid != null)
        {
            for (int i = 0; i < grid.Length; i++)
            {
                for(int j = 0; j < grid[i].Length; j++)
                {
                    if (route.Contains((i, j)) && grid[i][j]!='S' && grid[i][j]!='E')
                    {
                        switch (allNodes[(i, j)].Direction)
                        {
                            case DirectionType.Up:
                                grid[i][j] = '^';
                                break;
                            case DirectionType.Down:
                                grid[i][j] = 'v';
                                break;
                            case DirectionType.Left:
                                grid[i][j] = '<';
                                break;
                            case DirectionType.Right:
                                grid[i][j] = '>';
                                break;
                            default:
                                grid[i][j] = '.';
                                break;
                        }
                    }
                }
                
            }
        }

        return grid;
    }

    private static IEnumerable<(int, int)> RetrieveRoute(Dictionary<(int, int), (int, int)> predecessors, (int, int) end)
    {
        var route = new List<(int, int)>();
        var current = end;
        var isContinue = true;
        while (isContinue)
        {
            route.Add(current);
            if (predecessors.ContainsKey(current))
                current = predecessors[current];
            else
                isContinue = false;
        }

        route.Reverse();
        return route;
    }
}