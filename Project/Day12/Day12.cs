using System.Numerics;
using System.Linq;

namespace AdventOfCode2024;

public class Day12 : IAocDay
{
    private static string[] _input;
    private static char[][] _grid;
    private static Dictionary<(char, Vector2), (int, int)> garden = new() { };
    private static Dictionary<(char, Vector2), HashSet<(Vector2,string)>> regionEdges = new() { };
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

        PopulateGarden();

        foreach (var region in garden)
        {
            _price += region.Value.Item1 * region.Value.Item2;
        }

        Console.WriteLine($"Total price is {_price}");
    }

    private static void PopulateGarden()
    {
        for (int row = 1; row < _grid.Length - 1; row++)
        {
            for (int col = 1; col < _grid[row].Length - 1; col++)
            {
                var plotType = _grid[row][col];
                if (plotType == 'O')
                {
                    Console.WriteLine("here");
                }
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
                        if (canTraverseToPlantLead(match.Key.Item2, new Vector2(row, col), plotType, visited,
                                ref found))
                        {
                            var edges = checkEdges((plotType, match.Key.Item2), new Vector2(row, col));
                            garden[(plotType, match.Key.Item2)] = (originalAreaAndEdges.Item1 + 1,
                                originalAreaAndEdges.Item2 + edges);
                            matched = true;
                        }
                    }

                    if (!matched)
                    {
                        var edges = checkEdges((plotType, new Vector2(row, col)), new Vector2(row, col));
                        garden.Add((plotType, new Vector2(row, col)), (1, edges));
                    }
                }
                else
                {
                    var edges = checkEdges((plotType, new Vector2(row, col)), new Vector2(row, col));
                    garden.Add((plotType, new Vector2(row, col)), (1, edges));
                }
            }
        }
    }

    private static bool canTraverseToPlantLead(Vector2 plantLead, Vector2 currentPlot, char plotType,
        HashSet<Vector2> visited, ref bool found)
    {
        if (plantLead == currentPlot || found)
        {
            found = true;
            return true;
        }

        if (_grid[(int)currentPlot.X - 1][(int)currentPlot.Y] == plotType &&
            !visited.Contains(new Vector2(currentPlot.X - 1, currentPlot.Y)))
        {
            visited.Add(new Vector2(currentPlot.X - 1, currentPlot.Y));
            // Console.WriteLine("up");
            if (canTraverseToPlantLead(plantLead, new Vector2(currentPlot.X - 1, currentPlot.Y), plotType, visited,
                    ref found))
            {
                return true;
            }

            ;
        }

        if (_grid[(int)currentPlot.X + 1][(int)currentPlot.Y] == plotType &&
            !visited.Contains(new Vector2(currentPlot.X + 1, currentPlot.Y)))
        {
            visited.Add(new Vector2(currentPlot.X + 1, currentPlot.Y));
            // Console.WriteLine("down");
            if (canTraverseToPlantLead(plantLead, new Vector2(currentPlot.X + 1, currentPlot.Y), plotType, visited,
                    ref found))
            {
                return true;
            }

            ;
        }

        if (_grid[(int)currentPlot.X][(int)currentPlot.Y - 1] == plotType &&
            !visited.Contains(new Vector2(currentPlot.X, currentPlot.Y - 1)))
        {
            visited.Add(new Vector2(currentPlot.X, currentPlot.Y - 1));
            // Console.WriteLine("left");
            if (canTraverseToPlantLead(plantLead, new Vector2(currentPlot.X, currentPlot.Y - 1), plotType, visited,
                    ref found))
            {
                return true;
            }

            ;
        }

        if (_grid[(int)currentPlot.X][(int)currentPlot.Y + 1] == plotType &&
            !visited.Contains(new Vector2(currentPlot.X, currentPlot.Y + 1)))
        {
            visited.Add(new Vector2(currentPlot.X, currentPlot.Y + 1));
            // Console.WriteLine("right");
            if (canTraverseToPlantLead(plantLead, new Vector2(currentPlot.X, currentPlot.Y + 1), plotType, visited,
                    ref found))
            {
                return true;
            }

            ;
        }

        return false;
    }

    private static int checkEdges((char, Vector2) plotLead, Vector2 currentPlot)
    {
        var isTopEdge = _grid[(int)currentPlot.X - 1][(int)currentPlot.Y] != plotLead.Item1;
        
        var isBottomEdge = _grid[(int)currentPlot.X + 1][(int)currentPlot.Y] != plotLead.Item1;
        
        var isLeftEdge = _grid[(int)currentPlot.X][(int)currentPlot.Y - 1] != plotLead.Item1;
        
        var isRightEdge = _grid[(int)currentPlot.X][(int)currentPlot.Y + 1] != plotLead.Item1;

        if (_isPartTwo)
        {
            var isNewTopEdge = isTopEdge && isEdgeCounted(plotLead, currentPlot, (-1, -1), (-1, 1), isTopEdge,"bottom");
            if (isTopEdge)
            {
                if (regionEdges.ContainsKey(plotLead))
                {
                    regionEdges[plotLead].Add((new Vector2(currentPlot.X - 1, currentPlot.Y),"bottom"));
                }
                else
                {
                    regionEdges.Add(plotLead, new HashSet<(Vector2,string)> { (new Vector2(currentPlot.X - 1, currentPlot.Y),"bottom") });
                }
            }
            var isNewBottomEdge = isBottomEdge && isEdgeCounted(plotLead, currentPlot, (1, -1), (1, 1), isBottomEdge,"top");
            if (isBottomEdge)
            {
                if (regionEdges.ContainsKey(plotLead))
                {
                    regionEdges[plotLead].Add((new Vector2(currentPlot.X + 1, currentPlot.Y),"top"));
                }
                else
                {
                    regionEdges.Add(plotLead, new HashSet<(Vector2,string)> { (new Vector2(currentPlot.X + 1, currentPlot.Y),"top") });
                }
            }
            var isNewLeftEdge = isLeftEdge && isEdgeCounted(plotLead, currentPlot, (-1, -1), (1, -1), isLeftEdge,"right");
            if (isLeftEdge)
            {
                if (regionEdges.ContainsKey(plotLead))
                {
                    regionEdges[plotLead].Add((new Vector2(currentPlot.X, currentPlot.Y - 1),"right"));
                }
                else
                {
                    regionEdges.Add(plotLead, new HashSet<(Vector2,string)> { (new Vector2(currentPlot.X, currentPlot.Y - 1),"right") });
                }
            }
            var isNewRightEdge = isRightEdge && isEdgeCounted(plotLead, currentPlot, (-1, 1), (1, 1), isRightEdge,"left");
            if (isRightEdge)
            {
                if (regionEdges.ContainsKey(plotLead))
                {
                    regionEdges[plotLead].Add((new Vector2(currentPlot.X, currentPlot.Y + 1),"left"));
                }
                else
                {
                    regionEdges.Add(plotLead, new HashSet<(Vector2,string)> { (new Vector2(currentPlot.X, currentPlot.Y + 1),"left") });
                }
            }
            return (isNewTopEdge ? 1 : 0) + (isNewBottomEdge ? 1 : 0) + (isNewLeftEdge ? 1 : 0) + (isNewRightEdge ? 1 : 0);
        }

        return (isTopEdge ? 1 : 0) + (isBottomEdge ? 1 : 0) + (isLeftEdge ? 1 : 0) + (isRightEdge ? 1 : 0);
    }

    private static bool isEdgeCounted((char, Vector2) plotLead, Vector2 currentPlot, (int, int) neighbour1,
        (int, int) neighbour2, bool isEdge, string side)
    {
        if (!isEdge)
        {
            return false;
        }

        if (regionEdges.ContainsKey(plotLead)
            && (regionEdges[plotLead]
                    .Contains((new Vector2(currentPlot.X + neighbour1.Item1, currentPlot.Y + neighbour1.Item2),side))
                || regionEdges[plotLead]
                    .Contains((new Vector2(currentPlot.X + neighbour2.Item1, currentPlot.Y + neighbour2.Item2),side))))
        {
            isEdge = false;
        }
        
        return isEdge;
    }

    public static void RunPart2()
    {
        _isPartTwo = true;
        RunPart1();
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