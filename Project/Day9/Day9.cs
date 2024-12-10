using System.Numerics;

namespace AdventOfCode2024.Day9;

public class Day9 : IAocDay
{
    private static char[] _input;

    private static void Initialize()
    {
        _input = File.ReadAllText(Constants.baseDir + "Day9/data-aoc-day9.txt").ToArray();
    }

    public static void RunPart1()
    {
        Initialize();
        // diskmap to block view
        var blockView = DiskMapToBlockView();
        // compact the space
        var blockViewArray = blockView.ToArray();
        CompactTheSpace(blockViewArray);
        // calculate the checksum
        CalculateChecksum(blockViewArray);
    }

    private static void CalculateChecksum(FileBlock[] blockViewArray)
    {
        BigInteger checksum = 0;
        for (int i = 0; i < blockViewArray.Length; i++)
        {
            if (blockViewArray[i].ID !=-1)
            {
                checksum += i * blockViewArray[i].ID;
            }
        }

        Console.WriteLine(checksum);
    }

    private static void CompactTheSpace(FileBlock[] blockViewArray)
    {
        var leftStart = 0;
        var rightStart = blockViewArray.Length - 1;
        for (int i = rightStart; i > leftStart; i--)
        {
            if (blockViewArray[i].ID !=-1)
            {
                for (int j = leftStart; j < rightStart; j++)
                {
                    if (blockViewArray[j].ID ==-1)
                    {
                        (blockViewArray[i], blockViewArray[j]) = (blockViewArray[j], blockViewArray[i]);
                        leftStart = j;
                        rightStart = i;
                        break;
                    }
                }
            }
        }
    }
    
    private static void CompactTheSpace2(FileBlock[] blockViewArray)
    {
        var leftStart = 0;
        var rightStart = blockViewArray.Length - 1;
        for (int i = rightStart; i > leftStart; i--)
        {
            if (blockViewArray[i].ID !=-1)
            {
                for (int j = leftStart; j < rightStart; j++)
                {
                    if (blockViewArray[j].ID == -1)
                    {
                        (blockViewArray[i], blockViewArray[j]) = (blockViewArray[j], blockViewArray[i]);
                        leftStart = j;
                        rightStart = i;

                        // Console.WriteLine($"-------rightstart: {rightStart}---leftstart: {leftStart}---------");
                        // Console.WriteLine();
                        // foreach (var c in blockViewArray)
                        // {
                        //     Console.Write(c);
                        // }
                        //
                        // Console.WriteLine();
                        break;
                    }
                }
            }
        }
        
        // Console.WriteLine("---------------BlockViewArray----------------");
        // foreach (var c in blockViewArray)
        // {
        //     Console.Write(c);
        // }
    }

    private static List<FileBlock> DiskMapToBlockView()
    {
        List<FileBlock> blockView = new();
        int id = 0;
        for (int i = 0; i < _input.Length; i = i + 2)
        {
            FileBlock block = new(id);
            block.size = (int)char.GetNumericValue(_input[i]);
            blockView.AddRange(Enumerable.Repeat(block, block.size));
            id++;
            if (i < _input.Length - 1)
            {
                blockView.AddRange(Enumerable.Repeat(new FileBlock(-1), (int)char.GetNumericValue(_input[i+1])));
            }
        }

        // Console.WriteLine("---------------BlockView----------------");
        // foreach (var c in blockView)
        // {
        //     Console.Write(c);
        // }

        return blockView;
    }

    public static void RunPart2()
    {
        Initialize();
        // diskmap to block view
        var blockView = DiskMapToBlockView();
        // compact the space
        var blockViewArray = blockView.ToArray();
        
        // calculate the checksum
        CalculateChecksum(blockViewArray);    }

    public void BenchmarkPart1()
    {
        throw new NotImplementedException();
    }

    public void BenchmarkPart2()
    {
        throw new NotImplementedException();
    }
}