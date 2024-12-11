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

    public static void RunPart2()
    {
        Initialize();
        // diskmap to block view
        var blockView = DiskMapToBlockView();
        // compact the space
        var blockViewArray = blockView.ToArray();
        CompactTheSpace2(blockViewArray);
        // calculate the checksum
        CalculateChecksum(blockViewArray);
    }

    private static void CalculateChecksum(int[] blockViewArray)
    {
        BigInteger checksum = 0;
        for (int i = 0; i < blockViewArray.Length; i++)
        {
            if (blockViewArray[i] != -1)
            {
                checksum += i * blockViewArray[i];
            }
        }

        Console.WriteLine(checksum);
    }

    private static void CompactTheSpace(int[] blockViewArray)
    {
        var leftStart = 0;
        var rightStart = blockViewArray.Length - 1;
        for (int i = rightStart; i > leftStart; i--)
        {
            if (blockViewArray[i] != -1)
            {
                for (int j = leftStart; j < rightStart; j++)
                {
                    if (blockViewArray[j] == -1)
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

    private static void CompactTheSpace2(int[] blockViewArray)
    {
        for (int rightIndex = blockViewArray.Length-1; rightIndex > 0; rightIndex--)
        {
            var fileSize = 0;
            var currentIndex = rightIndex;
            if (blockViewArray[rightIndex] != -1)
            {
                while (rightIndex>0&&blockViewArray[rightIndex] != -1 && blockViewArray[rightIndex] == blockViewArray[currentIndex])
                {
                    fileSize++;
                    rightIndex--;
                }

                for (int i = 0; i < rightIndex; i++)
                {
                    if (blockViewArray[i] == -1)
                    {
                        var freeSpace = 0;
                        while (i<blockViewArray.Length && blockViewArray[i] == -1)
                        {
                            freeSpace++;
                            i++;
                        }

                        if (freeSpace >= fileSize)
                        {
                            for (int j = 0; j < fileSize; j++)
                            {
                                (blockViewArray[rightIndex + 1 + j], blockViewArray[i - freeSpace + j]) = (
                                    blockViewArray[i - freeSpace + j], blockViewArray[rightIndex + 1 + j]);
                            }

                            // foreach (var num in blockViewArray)
                            // {
                            //     Console.Write(num);
                            // }
                            rightIndex++;

                            // Console.WriteLine();
                            break;
                        }
                        
                        
                    }
                }
                rightIndex++;
            }
        }
    }

    private static List<int> DiskMapToBlockView()
        {
            List<int> blockView = new();
            int id = 0;
            for (int i = 0; i < _input.Length; i = i + 2)
            {
                blockView.AddRange(Enumerable.Repeat(id, (int)char.GetNumericValue(_input[i])));
                id++;
                if (i < _input.Length - 1)
                {
                    blockView.AddRange(Enumerable.Repeat(-1, (int)char.GetNumericValue(_input[i + 1])));
                }
            }

            return blockView;
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