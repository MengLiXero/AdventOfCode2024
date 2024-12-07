using BenchmarkDotNet.Attributes;

namespace AdventOfCode2024;

[MemoryDiagnoser]
public class Day6 : IAocDay
{
    private static string[] _input;
    private static char[][] _grid;
    private static readonly HashSet<int> ObstaclesVisited = new();
    private static readonly HashSet<int> Visited = new();
    private static bool _isStuck;
    private static int _benchMarkNumber;

    private static void Initialize()
    {
        _input = File.ReadAllLines(Constants.baseDir + "data-aoc-day6.txt");
        _grid = _input.Select(line => line.ToCharArray()).ToArray();
    }

    public static void RunPart1()
    {
        // if(_input == null)
        // {
        Initialize();
        // }
        bool isContinue = true;
        int num = 0;
        while (isContinue && !_isStuck)
        {
            bool roundTerminated = false;
            for (int i = 0; i < _grid.Length && !roundTerminated; i++)
            {
                for (int j = 0; j < _grid[i].Length && !roundTerminated; j++)
                {
                    switch (_grid[i][j])
                    {
                        case '^':
                        {
                            for (int k = i - 1; k > -1; k--)
                            {
                                if (k == 0 && _grid[k][j] != '#')
                                {
                                    RecordVisit(k + 1, j);
                                    RecordVisit(k, j);
                                    isContinue = false;
                                    break;
                                }
                                else if (_grid[k][j] != '#')
                                {
                                    RecordVisit(k + 1, j);
                                }
                                else
                                {
                                    _isStuck = !(ObstaclesVisited.Add(HashCode.Combine(k + 1, j, '^')));
                                    _grid[k + 1][j] = '>';

                                    break;
                                }
                            }

                            roundTerminated = true;
                            break;
                        }

                        case '>':
                        {
                            for (int k = j + 1; k < _grid[i].Length; k++)
                            {
                                if (k == _grid[i].Length - 1 && _grid[i][k] != '#')
                                {
                                    RecordVisit(i, k - 1);
                                    RecordVisit(i, k);
                                    isContinue = false;
                                    break;
                                }
                                else if (_grid[i][k] != '#')
                                {
                                    RecordVisit(i, k - 1);
                                }
                                else
                                {
                                    _isStuck = !(ObstaclesVisited.Add(HashCode.Combine(i, k - 1, '>')));
                                    _grid[i][k - 1] = 'v';
                                    break;
                                }
                            }

                            roundTerminated = true;
                            break;
                        }

                        case 'v':
                        {
                            for (int k = i + 1; k < _grid.Length; k++)
                            {
                                if (k == _grid.Length - 1 && _grid[k][j] != '#')
                                {
                                    RecordVisit(k - 1, j);
                                    RecordVisit(k, j);
                                    isContinue = false;
                                    break;
                                }
                                else if (_grid[k][j] != '#')
                                {
                                    RecordVisit(k - 1, j);
                                }
                                else
                                {
                                    _isStuck = !(ObstaclesVisited.Add(HashCode.Combine(k - 1, j, 'v')));
                                    _grid[k - 1][j] = '<';
                                    break;
                                }
                            }

                            roundTerminated = true;
                            break;
                        }
                        case '<':
                        {
                            for (int k = j - 1; k > -1; k--)
                            {
                                if (k == 0 && _grid[i][k] != '#')
                                {
                                    RecordVisit(i, k + 1);
                                    RecordVisit(i, k);
                                    isContinue = false;
                                    break;
                                }
                                else if (_grid[i][k] != '#')
                                {
                                    RecordVisit(i, k + 1);
                                }
                                else
                                {
                                    _isStuck = !(ObstaclesVisited.Add(HashCode.Combine(i, k + 1, '<')));
                                    _grid[i][k + 1] = '^';
                                    break;
                                }
                            }

                            roundTerminated = true;
                            break;
                        }
                    }
                }
            }

            // Console.WriteLine("Round: " + num);
            num++;
            // foreach (var c in grid)
            // {
            //     Console.WriteLine(string.Join(" ", c));
            // }
        }

        Console.WriteLine("Number of positions the guard visited: " + Visited.Count);
    }

    private static void RecordVisit(int k, int j)
    {
        _grid[k][j] = 'X';
        Visited.Add(HashCode.Combine(k, j));
    }

    public static void RunPart2()
    {
        Initialize();
        RunPart1();
        _grid = _input.Select(line => line.ToCharArray()).ToArray();
        var sum = 0;
        for (int i = 0; i < _grid.Length; i++)
        {
            for (int j = 0; j < _grid[i].Length; j++)
            {
                if (Visited.Add(HashCode.Combine(i, j)))
                {
                    // Console.WriteLine("Skipping: " + i + " " + j);
                    continue;
                }

                if (_grid[i][j] != '#' &&
                    (_grid[i][j] != '^' && _grid[i][j] != '>' && _grid[i][j] != 'v' && _grid[i][j] != '<'))
                {
                    _grid[i][j] = '#';
                    // Console.WriteLine("Obstacle at: " + i + " " + j);
                    RunPart1();
                    if (_isStuck)
                    {
                        sum++;
                    }

                    _grid = _input.Select(line => line.ToCharArray()).ToArray();
                    _isStuck = false;
                    ObstaclesVisited.Clear();
                }
            }
        }

        Console.WriteLine("Number of positions can stuck the guard: " + sum);
    }

    [Benchmark]
    public void BenchmarkPart1() => Day6.RunPart1();

    [Benchmark]
    public void BenchmarkPart2() => Day6.RunPart2();
}