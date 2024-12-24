using System.Numerics;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdventOfCode2024.Day20;

public class Day20 : Grid, IAocDay
{
    private static char[][] _grid;
    private static HashSet<(int, int)> _fragileWalls = new ();
    private static int _currentShortestDistance;
    private static Dictionary<int,int> _cheatedShortestDistanceList = new();
    private static HashSet<((int,int),(int,int))> _cheatsSet = new ();
    private static int _savedSteps = 74;
    private static Dictionary<int,int> _savedPicoSeconds = new ();
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
        Initialize();
        var start = LocateAValue(_grid, 'S');
        var end = LocateAValue(_grid, 'E');
        var count = 0;
        var savedPicoseconds = 76;
        _currentShortestDistance = BfsFindingShortestDistance(_grid, ((int)start.X, (int)start.Y), ((int)end.X, (int)end.Y));
        while (savedPicoseconds >= 68)
        {
            CheatedBfsFindingShortestDistance(_grid, ((int)start.X, (int)start.Y), ((int)end.X, (int)end.Y), savedPicoseconds);
            savedPicoseconds--;
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
    
    private static int CheatedBfsFindingShortestDistance(char[][] grid, (int,int) start,(int, int) end, int savedPicoSeconds)
    {
        int routeCount = 0;
        var visited = new List<(int row, int col, int steps)>();
        
        var startNode = (row: start.Item1, col: start.Item2, steps: 0, cheats:new List<(int,int)>(){}, startCheat:(-1,-1), endCheat:(-1,-1), new HashSet<(int, int)>());
        visited.Add((startNode.row, startNode.col, 0));
        var queue = new Queue<(int row, int col, int steps, List<(int,int)> cheats, (int,int) startCheat, (int,int) endCheat, HashSet<(int,int)> previouslyVisited)>();
        queue.Enqueue(startNode);
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
            var cheats = current.cheats.ToList();
            var alreadyVisited = current.previouslyVisited.ToHashSet();
            var startcheat = current.startCheat;
            var endcheat = current.endCheat;

            if (current.steps == _currentShortestDistance - savedPicoSeconds)
            {
                if (current.row == end.Item1 && current.col == end.Item2)
                {
                    if(!_cheatsSet.Contains((current.startCheat,current.endCheat)))
                    {
                        routeCount++;
                        _cheatsSet.Add((current.startCheat,current.endCheat));
                        
                        // Console.WriteLine($"SavedSteps: {_savedSteps}");
                        // Console.WriteLine($"Start and End:");
                        // foreach (var cheat in _cheatsSet)
                        // {
                        //     Console.Write($"{cheat} , ");
                        // }
                        // Console.WriteLine();
                        //
                        // Console.WriteLine("Cheats:");
                        // foreach (var c in current.cheats)
                        // {
                        //     Console.Write($"{c}, ");
                        // }
                        // Console.WriteLine();
                    }
                }
                continue;
            }

            if (current.steps < _currentShortestDistance - savedPicoSeconds)
            {
                foreach (var dir in directions)
                {
                    cheats = current.cheats.ToList();
                    
                    if (current.row + dir.row < 0
                        || current.row + dir.row > grid.Length - 1
                        || current.col + dir.col < 0
                        || current.col + dir.col > grid[0].Length - 1)
                    {
                        continue;
                    }
                    else if (grid[current.row + dir.row][current.col + dir.col] == '#')
                    {
                        if (cheats.Count == 0)
                        {
                            startcheat = (current.row, current.col);
                        }
                        cheats.Add((current.row + dir.row, current.col + dir.col));
                        
                        if (cheats.Count == 20)
                        {
                            continue;
                        }
                    }
                    
                    if (current.cheats.Contains((current.row, current.col)))
                    {
                        endcheat = (current.row + dir.row, current.col + dir.col);
                    }
                    
                    var next = (current.row + dir.row, current.col + dir.col, current.steps + 1, cheats, startcheat, endcheat, alreadyVisited: alreadyVisited);
                    if(alreadyVisited.Contains((next.Item1,next.Item2)))
                    {
                        continue;
                    }
                    alreadyVisited.Add((current.row + dir.row, current.col+ dir.col));
                    queue.Enqueue(next);
                }
            }
        }
        Console.WriteLine($"Saved picoseconds: {savedPicoSeconds}");
        Console.WriteLine($"routecount: {routeCount}");
        _savedPicoSeconds.Add(savedPicoSeconds, routeCount);
        return -1;
    }

}