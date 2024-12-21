using System.Numerics;

namespace AdventOfCode2024.Day21;

public class Day21 : Grid, IAocDay
{
    private static char[][] _numericKeyPadGrid;
    private static List<char[]> _passwordList;

    public static void RunPart1()
    {
        Initialize();

        foreach (var password in _passwordList)
        {
            var routeLists = new List<List<List<(int, int)>>>();
            var startPosition = LocateAValue(_numericKeyPadGrid, 'A');
            for (int i=0;i<password.Length;i++)
            {
                var endPosition = LocateAValue(_numericKeyPadGrid, password[i]);
                var result = BfsFindingAllShortestDistance(_numericKeyPadGrid, ((int)startPosition.X, (int)startPosition.Y),
                    ((int)endPosition.X, (int)endPosition.Y));
                var currentRoutes = new List<List<(int, int)>>();
                RetrieveRouteUsingRecursion(result.Item2, new List<(int, int)>(),
                    ((int)endPosition.X, (int)endPosition.Y), currentRoutes);
                startPosition = endPosition;
                routeLists.Add(currentRoutes);
            }
            
            var combinedRoutes = routeLists[0];
            
            for(int i=1;i<routeLists.Count;i++)
            {
                combinedRoutes = ListHelper.CombineTwoListsOfLists(combinedRoutes, routeLists[i]);
            }
            
            foreach (var routes in combinedRoutes)
            {
                PrintACoordinatesList(routes);
            }
            
            foreach (var route in combinedRoutes)
            {
                var routeWithDirections = PopulateRouteDirections(route);
                foreach (var (x, y, direction) in routeWithDirections)
                {
                    Console.Write($"{(char)direction} ");
                }
                Console.WriteLine();
            }
            
            
        }


        
    }

    private static List<(int, int, DirectionType)> PopulateRouteDirections(List<(int, int)> route)
    {
        var start = route[0];
        var routeWithDirections = new List<(int, int, DirectionType)>();
        for (var i = 1; i < route.Count; i++)
        {
            (int, int) offset = (route[i].Item1 - start.Item1, route[i].Item2 - start.Item2);
            switch (offset)
            {
                case (0, -1):
                    routeWithDirections.Add((route[i].Item1,route[i].Item2, DirectionType.Left));
                    break;
                case (0, 1):
                    routeWithDirections.Add((route[i].Item1,route[i].Item2, DirectionType.Right));
                    break;
                case (-1, 0):
                    routeWithDirections.Add((route[i].Item1,route[i].Item2, DirectionType.Up));
                    break;
                case (1, 0):
                    routeWithDirections.Add((route[i].Item1,route[i].Item2, DirectionType.Down));
                    break;
                case (0, 0):
                    routeWithDirections.Add((route[i].Item1,route[i].Item2, DirectionType.None));
                    break;
                default:
                    throw new Exception("Invalid offset");
            }

            start = route[i];
        }
        
        routeWithDirections.Add((route[route.Count-1].Item1,route[route.Count-1].Item2, DirectionType.None));

        return routeWithDirections;
    }

    private static void Initialize()
    {
        var numericKeyPad = File.ReadAllLines(Constants.baseDir + "/Day21/data-aoc-day21-numeric-keypad.txt");
        
        _numericKeyPadGrid = PopulateGrid<char>(numericKeyPad, GridType.Char);
        _numericKeyPadGrid = AddBoarder(_numericKeyPadGrid, '#');
        PrintGrid(_numericKeyPadGrid);
        
        _passwordList = File.ReadAllLines(Constants.baseDir + "/Day21/data-aoc-day21-passwords.txt").Select(line=>line.ToCharArray()).ToList();
        foreach (var line in _passwordList)
        {
            Console.WriteLine(new string(line));
        }
    }

    private static void PrintACoordinatesList(IEnumerable<(int, int)> list)
    {
        foreach (var (x, y) in list)
        {
            Console.Write($"({x},{y}) ");
        }

        Console.WriteLine();
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