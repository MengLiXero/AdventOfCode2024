using System.Numerics;

namespace AdventOfCode2024.Day9;

public class FileBlock(BigInteger id)
{
    public BigInteger ID { get; set; } = id;
    public int size { get; set; }
}