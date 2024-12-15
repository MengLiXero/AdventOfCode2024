using System.Numerics;

namespace AdventOfCode2024.Day15;

public class Day15 : Grid, IAocDay
{
    private static char[] _movement;
    private static Vector2 _robotPosition;

    private static void Initialize()
    {
        PopulateGrids("Day15/data-aoc-day15.txt");
        PrintGrid(_charGrid);
        _movement = LoadAndPrintCharArray("Day15/data-aoc-day15-2.txt");
    }

    public static void RunPart1()
    {
        Initialize();
        // Find the robot position
        _robotPosition = LocateAValue(_charGrid, '@');
        Console.WriteLine($"Robot position: {_robotPosition.X}, {_robotPosition.Y}");
        // Iterate through the movement list and switch different movement types
        // If there is no space between current position and the wall, do nothing.
        foreach (var move in _movement)
        {
            switch (move)
            {
                // Move the robot up
                case '^':
                    _robotPosition = MoveOnGrid(new Vector2(-1,0), _robotPosition) 
                        ? _robotPosition + new Vector2(-1,0) 
                        : _robotPosition;
                    // Console.WriteLine("Moving up");
                    // PrintGrid(_charGrid);
                    break;
                // Move the robot down
                case 'v':
                    _robotPosition = MoveOnGrid(new Vector2(1,0), _robotPosition) 
                        ? _robotPosition + new Vector2(1,0) 
                        : _robotPosition;
                    // Console.WriteLine("Moving down");
                    // PrintGrid(_charGrid);
                    break;
                // Move the robot left
                case '<':
                    _robotPosition = MoveOnGrid(new Vector2(0,-1), _robotPosition) 
                        ? _robotPosition + new Vector2(0,-1) 
                        : _robotPosition;
                    // Console.WriteLine("Moving left");
                    // PrintGrid(_charGrid);
                    break;
                // Move the robot right
                case '>':
                    _robotPosition = MoveOnGrid(new Vector2(0,1), _robotPosition) 
                        ? _robotPosition + new Vector2(0,1) 
                        : _robotPosition;
                    // Console.WriteLine("Moving right");
                    // PrintGrid(_charGrid);
                    break;
            }
        }
        
        var newRobotPosition = LocateAValue(_charGrid, '@');
        Console.WriteLine($"New robot position: {newRobotPosition.X}, {newRobotPosition.Y}");
        PrintGrid(_charGrid);
        
        Console.WriteLine($"The sum of all boxes' coordinates: {CalculateAllBoxesCoordinates(_charGrid)}");
    }

    private static long CalculateAllBoxesCoordinates(char[][] grid)
    {
        long sum = 0;
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                if (grid[i][j] == 'O')
                {
                    sum += 100 * i + j;
                }
            }
        }

        return sum;
    }

    private static bool MoveOnGrid(Vector2 direction, Vector2 position)
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
                var success = MoveOnGrid(direction, new Vector2(nextObjectRow, nextObjectCol));
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