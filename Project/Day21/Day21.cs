using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day21 : Grid, IAocDay
{
    private static char[][] _numericKeyPadGrid;
    private static List<char[]> _robotTwoOutputs;
    private static List<char[]> _robotThreeOutput;
    private static List<char[]> _humanOutput;
    private static List<char[]> _robotOneOutputList;
    private static char[][] _directionalKeyPadGrid;
    private static int _sum;
    private static Dictionary<((char,char),int),long> _memo = new Dictionary<((char, char), int), long>();
    private static long _sum2;
    private const int TotalDirectionalKeyPadRobots = 25;


    public static void RunPart1()
    {
        Initialize();
        foreach (var robotOneOutput in _robotOneOutputList)
        {
            _robotTwoOutputs = GetNextRobotOutput(robotOneOutput, LocateAValue(_numericKeyPadGrid, 'A'), _numericKeyPadGrid);
            var shortestSequence = int.MaxValue;
            foreach (var robotTwoOutput in _robotTwoOutputs)
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
            combinedRoutes = ICollectionHelper.CombineTwoListsOfLists(combinedRoutes, routeLists[i]);
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

        _numericKeyPadGrid = PopulateGrid<char>(numericKeyPad);
        _numericKeyPadGrid = AddBoarder(_numericKeyPadGrid, '#');
        PrintGrid(_numericKeyPadGrid);
        
        var directionalKeyPad = File.ReadAllLines(Constants.baseDir + "/Day21/data-aoc-day21-directional-keypad.txt");
        _directionalKeyPadGrid = PopulateGrid<char>(directionalKeyPad);
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
        Initialize();
        foreach (var robotOneOutput in _robotOneOutputList)
        {
            _robotTwoOutputs = GetNextRobotOutput(robotOneOutput, LocateAValue(_numericKeyPadGrid, 'A'), _numericKeyPadGrid);
        long min = long.MaxValue;
        long sum = 0;
            foreach (var robotTwoOutput in _robotTwoOutputs)
            {
                // var robotTwoOutput = new char[] { '<', 'A', '^', 'A', '>', '^', '^', 'A', 'v', 'v', 'v', 'A' };
                sum = 0;
                for(int i=0;i<robotTwoOutput.Length;i++)
                {
                    if (i == 0)
                    {
                        sum += CountMovesAfterNRobots(('A', robotTwoOutput[i]), TotalDirectionalKeyPadRobots);
                    }
                    else
                    {
                        sum += CountMovesAfterNRobots((robotTwoOutput[i-1], robotTwoOutput[i]), TotalDirectionalKeyPadRobots);
                    }
                    // Console.WriteLine($"Current i: {i}");
                    // Console.WriteLine($"Current sum: {sum}");
                }
                
                if(min>sum)
                {
                    min = sum;
                }
            }
            Console.WriteLine($"Minimum number of moves after 3 robots: {min}");
            string pattern = @"^\d+";
            var match = Regex.Matches(new string(robotOneOutput), pattern)[0];
            _sum2+= int.Parse(match.Value)*min;
            Console.WriteLine($"Sum2 is {_sum2}");
        }
        // foreach (var kvp in _memo)
        // {
        //     var key = kvp.Key;
        //     var value = kvp.Value;
        //
        //     Console.WriteLine($"Key: (({key.Item1.Item1}, {key.Item1.Item2}), {key.Item2}), Value: {value}");
        // }
    }
    private static long CountMovesAfterNRobots((char,char) currentStep,int n)
    {
        //Base case
        if (n == 0)
        {
            return 1;
        }

        // If the value is already calculated, return it
        var key = (currentStep, n);
        if (_memo.TryGetValue(key, out long numOfSteps))
        {
            return numOfSteps;
        }
        
        var currentOutputUnitMapped = new char[]{currentStep.Item1, currentStep.Item2};
        var nextRobotOutputs = GetNextRobotOutput(currentOutputUnitMapped, LocateAValue(_directionalKeyPadGrid, currentStep.Item1), _directionalKeyPadGrid);
        long sum = 0;
        long minimum = long.MaxValue;
        foreach (var output in nextRobotOutputs)
        { 
            sum = 0;
            for (int i = 0; i < output.Length-1; i++)
            { 
                sum += CountMovesAfterNRobots((output[i], output[i+1]), n-1);
            }
            if (minimum > sum)
            {
                minimum = sum;
            }
        }
        _memo[key] = minimum;
        
        return minimum;
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