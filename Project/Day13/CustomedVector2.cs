namespace AdventOfCode2024;

public struct CustomedVector2
{
    public long X { get; set; }
    public long Y { get; set; }

    public CustomedVector2(long x, long y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}