using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day21;

public class Day21 : Grid, IAocDay
{
    private static char[][] _numericKeyPadGrid;
    private static List<char[]> _robotTwoOutput;
    private static List<char[]> _robotThreeOutput;
    private static List<char[]> _humanOutput;
    private static List<char[]> _robotOneOutputList;
    private static char[][] _directionalKeyPadGrid;
    private static int _sum;


    public static void RunPart1()
    {
        Initialize();
        foreach (var robotOneOutput in _robotOneOutputList)
        {
            _robotTwoOutput = GetNextRobotOutput(robotOneOutput, LocateAValue(_numericKeyPadGrid, 'A'), _numericKeyPadGrid);
            var shortestSequence = int.MaxValue;
            foreach (var robotTwoOutput in _robotTwoOutput)
            {
                _robotThreeOutput = GetNextRobotOutput(robotTwoOutput, LocateAValue(_directionalKeyPadGrid, 'A'),
                    _directionalKeyPadGrid);
                foreach (var robotThreeOutput in _robotThreeOutput)
                {
                    _humanOutput = GetNextRobotOutput(robotThreeOutput, LocateAValue(_directionalKeyPadGrid, 'A'),
                        _directionalKeyPadGrid);
                    foreach (var humanOutput in _humanOutput)
                    {
                        if (shortestSequence > humanOutput.Length)
                        {
                            shortestSequence = humanOutput.Length;
                        }
                    }
                }
            }
            string pattern = @"^\d+";
            var match = Regex.Matches(new string(robotOneOutput), pattern)[0];
            _sum+= int.Parse(match.Value)*shortestSequence;
            Console.WriteLine($"Length of the shortest sequence: {shortestSequence}");
        }
        Console.WriteLine($"Sum is {_sum}");
    }

    private static List<char[]> GetNextRobotOutput(char [] robotOneOutput, Vector2 startPosition, char[][] grid)
    {
        var routeLists = new List<List<List<(int, int)>>>();
        var robotTwoOutput = new List<char[]>();
        for (int i = 0; i < robotOneOutput.Length; i++)
        {
            var endPosition = LocateAValue(grid, robotOneOutput[i]);
            var result = BfsFindingAllShortestDistance(grid, ((int)startPosition.X, (int)startPosition.Y),
                ((int)endPosition.X, (int)endPosition.Y));
            var currentRoutes = new List<List<(int, int)>>();
            RetrieveRouteUsingRecursion(result.Item2, new List<(int, int)>(),
                ((int)endPosition.X, (int)endPosition.Y), currentRoutes);
            startPosition = endPosition;
            routeLists.Add(currentRoutes);
        }

        var combinedRoutes = routeLists[0];

        for (int i = 1; i < routeLists.Count; i++)
        {
            combinedRoutes = ListHelper.CombineTwoListsOfLists(combinedRoutes, routeLists[i]);
        }

        // foreach (var routes in combinedRoutes)
        // {
        //     PrintACoordinatesList(routes);
        // }

        foreach (var route in combinedRoutes)
        {
            var routeWithDirections = PopulateRouteDirections(route);
            var str = "";
            foreach (var (x, y, direction) in routeWithDirections)
            {
                str = str + ($"{(char)direction}") ;
                str = str.Replace('0', 'A');
            }
            // Console.Write(str);
            // Console.WriteLine();
            robotTwoOutput.Add(str.ToCharArray());
            
        }

        return robotTwoOutput;
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
                    routeWithDirections.Add((route[i].Item1, route[i].Item2, DirectionType.Left));
                    break;
                case (0, 1):
                    routeWithDirections.Add((route[i].Item1, route[i].Item2, DirectionType.Right));
                    break;
                case (-1, 0):
                    routeWithDirections.Add((route[i].Item1, route[i].Item2, DirectionType.Up));
                    break;
                case (1, 0):
                    routeWithDirections.Add((route[i].Item1, route[i].Item2, DirectionType.Down));
                    break;
                case (0, 0):
                    routeWithDirections.Add((route[i].Item1, route[i].Item2, DirectionType.None));
                    break;
                default:
                    throw new Exception("Invalid offset");
            }

            start = route[i];
        }

        routeWithDirections.Add((route[route.Count - 1].Item1, route[route.Count - 1].Item2, DirectionType.None));

        return routeWithDirections;
    }

    private static void Initialize()
    {
        var numericKeyPad = File.ReadAllLines(Constants.baseDir + "/Day21/data-aoc-day21-numeric-keypad.txt");

        _numericKeyPadGrid = PopulateGrid<char>(numericKeyPad, GridType.Char);
        _numericKeyPadGrid = AddBoarder(_numericKeyPadGrid, '#');
        PrintGrid(_numericKeyPadGrid);
        
        var directionalKeyPad = File.ReadAllLines(Constants.baseDir + "/Day21/data-aoc-day21-directional-keypad.txt");
        _directionalKeyPadGrid = PopulateGrid<char>(directionalKeyPad, GridType.Char);
        _directionalKeyPadGrid = AddBoarder(_directionalKeyPadGrid, '#');
        PrintGrid(_directionalKeyPadGrid);

        _robotOneOutputList = File.ReadAllLines(Constants.baseDir + "/Day21/data-aoc-day21-passwords.txt")
            .Select(line => line.ToCharArray()).ToList();
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