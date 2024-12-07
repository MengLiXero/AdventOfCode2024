using BenchmarkDotNet.Attributes;

namespace AdventOfCode2024;

public class Day4 : IAocDay
{
    private static string[] input;

    public static void Initialize()
    {
        Console.WriteLine("Day 4");
        input = File.ReadAllLines("../../../Day4/1data-aoc-day4.txt");
    }

    public static void RunPart1()
    {
        Initialize();
        int sum = 0;
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] == 'X')
                {
                    sum += connectedNumOfWords('M', 'A', 'S', i, j);
                }
            }
        }

        Console.WriteLine(sum);
    }

    public static void RunPart2()
    {
        Initialize();
        int sum = 0;
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] == 'A' && isXCombo('M', 'S', i, j))
                {
                    sum++;
                }
            }
        }

        Console.WriteLine(sum);
    }


    private static int connectedNumOfWords(char c1, char c2, char c3, int x, int y)
    {
        int totalConnected = 0;
        if (x - 3 > -1 && y - 3 > -1 &&
            input[x - 1][y - 1] == c1 && input[x - 2][y - 2] == c2 && input[x - 3][y - 3] == c3)
        {
            totalConnected++;
        }

        if (x - 3 > -1 &&
            input[x - 1][y] == c1 && input[x - 2][y] == c2 && input[x - 3][y] == c3)
        {
            totalConnected++;
        }

        if (x - 3 > -1 && y + 3 < input[x].Length &&
            input[x - 1][y + 1] == c1 && input[x - 2][y + 2] == c2 && input[x - 3][y + 3] == c3)
        {
            totalConnected++;
        }

        if (y - 3 > -1 &&
            input[x][y - 1] == c1 && input[x][y - 2] == c2 && input[x][y - 3] == c3)
        {
            totalConnected++;
        }

        if (y + 3 < input[x].Length &&
            input[x][y + 1] == c1 && input[x][y + 2] == c2 && input[x][y + 3] == c3)
        {
            totalConnected++;
        }

        if (x + 3 < input.Length && y - 3 > -1 &&
            input[x + 1][y - 1] == c1 && input[x + 2][y - 2] == c2 && input[x + 3][y - 3] == c3)
        {
            totalConnected++;
        }

        if (x + 3 < input.Length &&
            input[x + 1][y] == c1 && input[x + 2][y] == c2 && input[x + 3][y] == c3)
        {
            totalConnected++;
        }

        if (x + 3 < input.Length && y + 3 < input[x].Length &&
            input[x + 1][y + 1] == c1 && input[x + 2][y + 2] == c2 && input[x + 3][y + 3] == c3)
        {
            totalConnected++;
        }

        return totalConnected;
    }

    private static bool isXCombo(char c1, char c2, int x, int y)
    {
        if (((x - 1 > -1 && y - 1 > -1 && input[x - 1][y - 1] == c1) &&
             (x + 1 < input.Length && y + 1 < input[x].Length && input[x + 1][y + 1] == c2))
            && ( ((x - 1 > -1 && y + 1 < input[x].Length && input[x - 1][y + 1] == c1) &&
                (x + 1 < input.Length && y - 1 > -1 && input[x + 1][y - 1] == c2)) ||
            ((x - 1 > -1 && y + 1 < input[x].Length && input[x - 1][y + 1] == c2) &&
             (x + 1 < input.Length && y - 1 > -1 && input[x + 1][y - 1] == c1))))
        {
            return true;
        }

        if (((x - 1 > -1 && y - 1 > -1 && input[x - 1][y - 1] == c2) &&
             (x + 1 < input.Length && y + 1 < input[x].Length && input[x + 1][y + 1] == c1))
            && (((x - 1 > -1 && y + 1 < input[x].Length && input[x - 1][y + 1] == c1) &&
                (x + 1 < input.Length && y - 1 > -1 && input[x + 1][y - 1] == c2)) ||
            ((x - 1 > -1 && y + 1 < input[x].Length && input[x - 1][y + 1] == c2) &&
             (x + 1 < input.Length && y - 1 > -1 && input[x + 1][y - 1] == c1))))
        {
            return true;
        }

        return false;
    }
    
    [Benchmark]
    public void BenchmarkPart1()=> Day4.RunPart1();
    
    [Benchmark]
    public void BenchmarkPart2()=> Day4.RunPart2();
}