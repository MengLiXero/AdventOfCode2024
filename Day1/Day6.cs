using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
namespace AdventOfCode2024;

public class Day6
{
    private static string[] input;
    private static char[][] grid;
    private static HashSet<int> obstaclesVisited = new();
    private static HashSet<int> visited = new();
    private static bool isStuck;

    [Benchmark]
    public void Index()
    {
        input = File.ReadAllLines("../../../../../../../data-aoc-day6.txt");
        grid = input.Select(line => line.ToCharArray()).ToArray();
        // part1();
        part2();
    }

    private static void part1()
    {
        bool isContinue = true;
        int sum = 0;
        int num = 0;
        while (isContinue && !isStuck)
        {
            bool roundTerminated = false;
            for (int i = 0; i < grid.Length && !roundTerminated; i++)
            {
                for (int j = 0; j < grid[i].Length && !roundTerminated; j++)
                {
                    switch (grid[i][j])
                    {
                        case '^':
                        {
                            for (int k = i - 1; k > -1; k--)
                            {
                                if (k == 0 && grid[k][j] != '#')
                                {
                                    grid[k + 1][j] = 'X';
                                    grid[k][j] = 'X';
                                    visited.Add(HashCode.Combine(k + 1, j));
                                    visited.Add(HashCode.Combine(k, j));
                                    sum = sum + 2;
                                    isContinue = false;
                                    break;
                                }
                                else if (grid[k][j] != '#')
                                {
                                    if (grid[k + 1][j] != 'X')
                                    {
                                        grid[k + 1][j] = 'X';
                                        visited.Add(HashCode.Combine(k + 1, j));
                                        sum++;
                                    }
                                }
                                else
                                {
                                    isStuck = !(obstaclesVisited.Add(HashCode.Combine(k + 1, j, '^')));
                                    grid[k + 1][j] = '>';

                                    break;
                                }
                            }

                            roundTerminated = true;
                            break;
                        }

                        case '>':
                        {
                            for (int k = j + 1; k < grid[i].Length; k++)
                            {
                                if (k == grid[i].Length - 1 && grid[i][k] != '#')
                                {
                                    grid[i][k - 1] = 'X';
                                    grid[i][k] = 'X';
                                    visited.Add(HashCode.Combine(i, k - 1));
                                    visited.Add(HashCode.Combine(i, k));
                                    sum = sum + 2;
                                    isContinue = false;
                                    break;
                                }
                                else if (grid[i][k] != '#')
                                {
                                    if (grid[i][k - 1] != 'X')
                                    {
                                        grid[i][k - 1] = 'X';
                                        visited.Add(HashCode.Combine(i, k - 1));
                                        sum++;
                                    }
                                }
                                else
                                {
                                    isStuck = !(obstaclesVisited.Add(HashCode.Combine(i, k - 1, '>')));
                                    grid[i][k - 1] = 'v';
                                    break;
                                }
                            }

                            roundTerminated = true;
                            break;
                        }

                        case 'v':
                        {
                            for (int k = i + 1; k < grid.Length; k++)
                            {
                                if (k == grid.Length - 1 && grid[k][j] != '#')
                                {
                                    grid[k - 1][j] = 'X';
                                    grid[k][j] = 'X';
                                    visited.Add(HashCode.Combine(k - 1, j));
                                    visited.Add(HashCode.Combine(k, j));
                                    isContinue = false;
                                    sum = sum + 2;
                                    break;
                                }
                                else if (grid[k][j] != '#')
                                {
                                    if (grid[k - 1][j] != 'X')
                                    {
                                        grid[k - 1][j] = 'X';
                                        visited.Add(HashCode.Combine(k - 1, j));
                                        sum++;
                                    }
                                }
                                else
                                {
                                    isStuck = !(obstaclesVisited.Add(HashCode.Combine(k - 1, j, 'v')));
                                    grid[k - 1][j] = '<';
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
                                if (k == 0 && grid[i][k] != '#')
                                {
                                    grid[i][k + 1] = 'X';
                                    grid[i][k] = 'X';
                                    visited.Add(HashCode.Combine(i, k + 1));
                                    visited.Add(HashCode.Combine(i, k));
                                    sum = sum + 2;
                                    isContinue = false;
                                    break;
                                }
                                else if (grid[i][k] != '#')
                                {
                                    if (grid[i][k + 1] != 'X')
                                    {
                                        grid[i][k + 1] = 'X';
                                        visited.Add(HashCode.Combine(i, k + 1));
                                        sum++;
                                    }
                                }
                                else
                                {
                                    isStuck = !(obstaclesVisited.Add(HashCode.Combine(i, k + 1, '<')));
                                    grid[i][k + 1] = '^';
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
            // if (num == 10000)
            // {
            //     Console.WriteLine("here");
            // }
            num++;
        }

        // Console.WriteLine("Number of positions the guard visited: " + sum);
    }

    private static void part2()
    {
        part1();
        grid = input.Select(line => line.ToCharArray()).ToArray();
        var sum = 0;
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                if (visited.Add(HashCode.Combine(i, j)))
                {
                    // Console.WriteLine("Skipping: " + i + " " + j);
                    continue;
                }
                if (grid[i][j] != '#' &&
                    (grid[i][j] != '^' && grid[i][j] != '>' && grid[i][j] != 'v' && grid[i][j] != '<'))
                {
                    grid[i][j] = '#';
                    // Console.WriteLine("Obstacle at: " + i + " " + j);
                    part1();
                    if (isStuck)
                    {
                        sum++;
                    }
                    grid = input.Select(line => line.ToCharArray()).ToArray();
                    isStuck = false;
                    obstaclesVisited.Clear();
                }
            }
        }
        
        Console.WriteLine("Number of positions can stuck the guard: " + sum);
    }
}