using System.Numerics;
using Microsoft.Diagnostics.Tracing.Parsers.ClrPrivate;

namespace AdventOfCode2024.Day15;

public class Day15 : Grid, IAocDay
{
    private static char[][] _charGrid;
    private static char[] _movement;
    private static Vector2 _robotPosition;
    private static HashSet<int> _visited;
    private static HashSet<int> _movedObjects = new HashSet<int>();

    private static void Initialize()
    {
        _charGrid = PopulateGrid<char>(File.ReadAllLines(Constants.baseDir + "Day15/data-aoc-day15.txt"),"char");
        PrintGrid(_charGrid);
        _movement = LoadAndPrintCharArray("Day15/data-aoc-day15-2.txt");
    }

    public static void RunPart1()
    {
        Initialize();
        // Find the robot position
        _robotPosition = LocateAValue(_charGrid, '@');
        Console.WriteLine($"Robot position: {_robotPosition.X}, {_robotPosition.Y}");
        MoveRobot(MoveOnGridPart1);
        var newRobotPosition = LocateAValue(_charGrid, '@');
        Console.WriteLine($"New robot position: {newRobotPosition.X}, {newRobotPosition.Y}");
        PrintGrid(_charGrid);
        
        Console.WriteLine($"The sum of all boxes' coordinates: {CalculateAllBoxesCoordinates(_charGrid)}");
    }
    
    public static void RunPart2()
    {
        Initialize();
        _charGrid = ExpandTheGrid();
        PrintGrid(_charGrid);
        // Find the robot position
        _robotPosition = LocateAValue(_charGrid, '@');
        Console.WriteLine($"Robot position: {_robotPosition.X}, {_robotPosition.Y}");
        MoveRobot(MoveOnGridPart2);
        var newRobotPosition = LocateAValue(_charGrid, '@');
        Console.WriteLine($"New robot position: {newRobotPosition.X}, {newRobotPosition.Y}");
        PrintGrid(_charGrid);
        Console.WriteLine($"The sum of all boxes' coordinates: {CalculateAllBoxesCoordinates(_charGrid)}");
    }

    private static char[][] ExpandTheGrid()
    {
        var input = File.ReadAllLines(Constants.baseDir + "Day15/data-aoc-day15.txt");
        for (int i = 0; i < input.Length; i++)
        {
            input[i] = input[i].Replace("#", "##")
                .Replace("O", "[]")
                .Replace(".", "..")
                .Replace("@", "@.");
        }

        return PopulateGrid<char>(input, "char");
    }

    private static void MoveRobot(Move MoveOnGrid)
    {
        // Iterate through the movement list and switch different movement types
        // If there is no space between current position and the wall, do nothing.
        foreach (var move in _movement)
        {
            _visited = new HashSet<int>();
            _movedObjects = new HashSet<int>();
            switch (move)
            {
                // Move the robot up
                case '^':
                    _robotPosition = MoveOnGrid(new Vector2(-1, 0), _robotPosition)
                        ? _robotPosition + new Vector2(-1, 0)
                        : _robotPosition;
                    // Console.WriteLine("Moving up");
                    // PrintGrid(_charGrid);
                    break;
                // Move the robot down
                case 'v':
                    _robotPosition = MoveOnGrid(new Vector2(1, 0), _robotPosition)
                        ? _robotPosition + new Vector2(1, 0)
                        : _robotPosition;
                    // Console.WriteLine("Moving down");
                    // PrintGrid(_charGrid);
                    break;
                // Move the robot left
                case '<':
                    _robotPosition = MoveOnGrid(new Vector2(0, -1), _robotPosition)
                        ? _robotPosition + new Vector2(0, -1)
                        : _robotPosition;
                    // Console.WriteLine("Moving left");
                    // PrintGrid(_charGrid);
                    break;
                // Move the robot right
                case '>':
                    _robotPosition = MoveOnGrid(new Vector2(0, 1), _robotPosition)
                        ? _robotPosition + new Vector2(0, 1)
                        : _robotPosition;
                    // Console.WriteLine("Moving right");
                    // PrintGrid(_charGrid);
                    break;
            }
        }
    }

    private static long CalculateAllBoxesCoordinates(char[][] grid)
    {
        long sum = 0;
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                if (grid[i][j] == 'O' || grid[i][j]=='[')
                {
                    sum += 100 * i + j;
                }
            }
        }

        return sum;
    }

    private delegate bool Move(Vector2 direction, Vector2 position);
    private static bool MoveOnGridPart1(Vector2 direction, Vector2 position)
    {
        var currentObjectRow = (int)position.X;
        var currentObjectCol = (int)position.Y;
        var currentObject = _charGrid[currentObjectRow][currentObjectCol];
        var nextObjectRow = (int)(position.X + direction.X);
        var nextObjectCol = (int)(position.Y + direction.Y);
        
        switch (_charGrid[nextObjectRow][nextObjectCol])
        {
            // If the position is an Empty space, move the robot/obstacle, make the current position empty.
            case '.':
                _charGrid[currentObjectRow][currentObjectCol] = '.';
                _charGrid[nextObjectRow][nextObjectCol] = currentObject;
                return true;
            // If the position is an Obstacle, move the robot/obstacle, make the current position empty,move(this Obstacle),
            case 'O':
                var success = MoveOnGridPart1(direction, new Vector2(nextObjectRow, nextObjectCol));
                if (success)
                {
                    _charGrid[currentObjectRow][currentObjectCol] = '.';
                    _charGrid[nextObjectRow][nextObjectCol] = currentObject;
                    return true;
                }
                else
                {
                    Console.WriteLine("Couldn't move the object");
                    return false;
                }
            // If the position is a Wall, return false.
            case '#':
                return false;
            default:
                return false;
        }
    }
    
    private static bool TryMoveOnGridPart2(Vector2 direction, Vector2 position)
    {
        
        var currentObjectRow = (int)position.X;
        var currentObjectCol = (int)position.Y;
        var nextObjectRow = (int)(position.X + direction.X);
        var nextObjectCol = (int)(position.Y + direction.Y);
        _visited.Add(HashCode.Combine(currentObjectRow, currentObjectCol));
        if(_visited.Contains(HashCode.Combine(nextObjectRow, nextObjectCol)))
            return true;
        switch (_charGrid[nextObjectRow][nextObjectCol])
        {
            case '.':
                return true;
            case '[':
                return TryMoveOnGridPart2(direction, new Vector2(nextObjectRow, nextObjectCol))
                       && TryMoveOnGridPart2(direction, new Vector2(nextObjectRow,nextObjectCol+1));
            case ']':
                return TryMoveOnGridPart2(direction, new Vector2(nextObjectRow, nextObjectCol))
                       && TryMoveOnGridPart2(direction, new Vector2(nextObjectRow,nextObjectCol-1));
            case '#':
                return false;
        }
        return false;
    }
    
    private static bool MoveOnGridPart2(Vector2 direction, Vector2 position)
    {
        var currentObjectRow = (int)position.X;
        var currentObjectCol = (int)position.Y;
        var currentObject = _charGrid[currentObjectRow][currentObjectCol];
        if(currentObjectRow==3 && currentObjectCol==5 && currentObject=='@')
            Console.WriteLine("Here");
        var objectLeftOfCurrent = _charGrid[currentObjectRow][currentObjectCol - 1];
        var objectRightOfCurrent = _charGrid[currentObjectRow][currentObjectCol + 1];
        var nextObjectRow = (int)(position.X + direction.X);
        var nextObjectCol = (int)(position.Y + direction.Y);
        if(_movedObjects.Contains(HashCode.Combine(currentObjectRow, currentObjectCol)))
            return true;
        if (TryMoveOnGridPart2(direction, position))
        {
            switch (_charGrid[nextObjectRow][nextObjectCol])
            {
                case '.':
                    _charGrid[currentObjectRow][currentObjectCol] = '.';
                    _charGrid[nextObjectRow][nextObjectCol] = currentObject;
                    _movedObjects.Add(HashCode.Combine(currentObjectRow, currentObjectCol));
                    break;
                case '[':
                    MoveOnGridPart2(direction, new Vector2(nextObjectRow, nextObjectCol));
                    Console.WriteLine($"Current object: {currentObject}");
                    // PrintGrid(_charGrid);
                    MoveOnGridPart2(direction, new Vector2(nextObjectRow, nextObjectCol + 1));
                    Console.WriteLine($"Current object: {currentObject}");
                    // PrintGrid(_charGrid);
                    if(currentObject!='@')
                        MoveOnGridPart2(direction, new Vector2(currentObjectRow, currentObjectCol + 1));
                    _charGrid[currentObjectRow][currentObjectCol] = '.';
                    // PrintGrid(_charGrid);
                    if (currentObject != '@')
                    {
                        _charGrid[currentObjectRow][currentObjectCol + 1] = '.';
                    }
                    // PrintGrid(_charGrid);
                    _charGrid[nextObjectRow][nextObjectCol] = currentObject;
                    // PrintGrid(_charGrid);
                    _movedObjects.Add(HashCode.Combine(currentObjectRow, currentObjectCol));
                    break;
                case ']':
                    MoveOnGridPart2(direction, new Vector2(nextObjectRow, nextObjectCol));
                    MoveOnGridPart2(direction, new Vector2(nextObjectRow, nextObjectCol - 1));
                    if(currentObject!='@')
                        MoveOnGridPart2(direction, new Vector2(currentObjectRow, currentObjectCol - 1));
                    _charGrid[currentObjectRow][currentObjectCol] = '.';
                    if (currentObject != '@')
                    {
                        _charGrid[currentObjectRow][currentObjectCol - 1] = '.';
                    }
                    _charGrid[nextObjectRow][nextObjectCol] = currentObject;
                    _movedObjects.Add(HashCode.Combine(currentObjectRow, currentObjectCol));
                    break;
            }
            return true;
        }

        return false;
    }

    

    public void BenchmarkPart1()
    {
        throw new NotImplementedException();
    }

    public void BenchmarkPart2()
    {
        throw new NotImplementedException();
    }

    private static char[] LoadAndPrintCharArray(string path)
    {
        var charArray = File.ReadAllText(Constants.baseDir + path).Where(c => c != '\n' && c != '\r').ToArray();
        foreach (var c in charArray)
        {
            Console.Write(string.Join(" ", c));
        }

        Console.WriteLine();
        return charArray;
    }
}