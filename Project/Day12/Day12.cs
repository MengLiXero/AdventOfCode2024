using System.Numerics;
using System.Linq;

namespace AdventOfCode2024;

public class Day12 : IAocDay
{
    private static string[] _input;
    private static char[][] _grid;
    private static Dictionary<(char,Vector2), (int, int)> garden = new() { };
    private static int _price;
    private static bool _isPartTwo;
    
    private static void Initialize()
    {
        _input = File.ReadAllLines(Constants.baseDir + "Day12/data-aoc-day12.txt");
        _grid = _input
            .Select(line => line.ToCharArray())
            .ToArray();
        _grid = AddBorder();
    }
    
    private static char[][] AddBorder(char boarderElement = '.')
    {
        // Create the top and bottom border row
        char[] borderRow = Enumerable.Repeat(boarderElement, _grid[0].Length + 2).ToArray();

        // Add -1 to the sides of each row and build the bordered grid
        var borderedGrid = _grid
            .Select(row => new[] { boarderElement }.Concat(row).Concat(new[] { boarderElement }).ToArray())
            .Prepend(borderRow)
            .Append(borderRow)
            .ToArray();

        return borderedGrid;
    }

    public static void RunPart1()
    {
        Initialize();
        foreach (var c in _grid)
        {
            Console.WriteLine(string.Join(" ", c));
        }
        
        for (int row = 1; row < _grid.Length-1; row++)
        {
            for(int col = 1; col < _grid[row].Length-1; col++)
            {
                var plotType = _grid[row][col];
                // if(plotType == '.')
                //     continue;
                //Check if the current plot is exist in the garden(use LINQ), if so
                    //Check if current plot can lead to the plant lead. If so
                        //Check how many edges are adjacent to a plot of different plant.
                        //Add area(1) and edges to current record in the garden
                    //else
                        //Check how many edges are adjacent to a plot of different plant.
                        //Add current plot to the garden as a plant lead
                //else
                    //Check how many edges are adjacent to a plot of different plant.
                    //Add current plot to the garden as a plant lead

                    var matchingPlot = garden.Where(record => record.Key.Item1 == plotType);
                    var matched = false;
                    if (matchingPlot.Count() != 0)
                    {
                        foreach (var match in matchingPlot)
                        {
                            var originalAreaAndEdges = garden[(plotType, match.Key.Item2)];
                            HashSet<Vector2> visited = new();
                            visited.Add(new Vector2(row, col));
                            var found = false;
                            if (canTraverseToPlantLead(match.Key.Item2, new Vector2(row, col),plotType, visited, ref found))
                            {
                                var edges = checkEdges(plotType, new Vector2(row, col));
                                garden[(plotType, match.Key.Item2)] = (originalAreaAndEdges.Item1 + 1,
                                    originalAreaAndEdges.Item2 + edges);
                                matched = true;
                            }
                        }

                        if (!matched)
                        {
                            var edges = checkEdges(plotType, new Vector2(row,col));
                            garden.Add((plotType, new Vector2(row, col)), (1, edges));
                        }
                    }
                    else
                    {
                        var edges = checkEdges(plotType, new Vector2(row,col));
                        garden.Add((plotType, new Vector2(row, col)), (1, edges));
                    }
            }
        }

        foreach (var region in garden)
        {
            _price += region.Value.Item1 * region.Value.Item2;
        }
        
        Console.WriteLine($"Total price is {_price}");
        
    }

    private static bool canTraverseToPlantLead(Vector2 plantLead, Vector2 currentPlot, char plotType, HashSet<Vector2> visited, ref bool found)
    {
        if (plantLead == currentPlot || found)
        {
            found = true;
            return true; 
        }

        if (_grid[(int)currentPlot.X-1][(int)currentPlot.Y] == plotType && !visited.Contains(new Vector2(currentPlot.X - 1, currentPlot.Y)))
        {
            visited.Add(new Vector2(currentPlot.X - 1, currentPlot.Y));
            // Console.WriteLine("up");
            if( canTraverseToPlantLead(plantLead, new Vector2(currentPlot.X - 1, currentPlot.Y), plotType, visited, ref found))
            {
                return true;
            };
        }
        if (_grid[(int)currentPlot.X + 1][(int)currentPlot.Y] == plotType && !visited.Contains(new Vector2(currentPlot.X + 1, currentPlot.Y)))
        {
            visited.Add(new Vector2(currentPlot.X + 1, currentPlot.Y));
            // Console.WriteLine("down");
            if(canTraverseToPlantLead(plantLead, new Vector2(currentPlot.X + 1, currentPlot.Y), plotType, visited, ref found))
            {
                return true;
            };
        }
        if (_grid[(int)currentPlot.X][(int)currentPlot.Y - 1] == plotType && !visited.Contains(new Vector2(currentPlot.X, currentPlot.Y - 1)))
        {
            visited.Add(new Vector2(currentPlot.X, currentPlot.Y - 1));
            // Console.WriteLine("left");
            if(canTraverseToPlantLead(plantLead, new Vector2(currentPlot.X, currentPlot.Y - 1), plotType, visited, ref found))
            {
                return true;
            };
        }
        if (_grid[(int)currentPlot.X][(int)currentPlot.Y + 1] == plotType && !visited.Contains(new Vector2(currentPlot.X, currentPlot.Y + 1)))
        {
            visited.Add(new Vector2(currentPlot.X, currentPlot.Y + 1));
            // Console.WriteLine("right");
            if (canTraverseToPlantLead(plantLead, new Vector2(currentPlot.X, currentPlot.Y + 1), plotType, visited,
                    ref found))
            {
                return true;
            };
        } 
        return false; 
    }

    private static int checkEdges(char plotType, Vector2 currentPlot)
    {
        var topMatch = _grid[(int)currentPlot.X - 1][(int)currentPlot.Y] != plotType;
        var bottomMatch = _grid[(int)currentPlot.X + 1][(int)currentPlot.Y] != plotType;
        var leftMatch = _grid[(int)currentPlot.X][(int)currentPlot.Y - 1] != plotType;
        var rightMatch = _grid[(int)currentPlot.X][(int)currentPlot.Y + 1] != plotType;
        return (topMatch? 1 : 0) + (bottomMatch? 1 : 0) + (leftMatch? 1 : 0) + (rightMatch? 1 : 0);

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