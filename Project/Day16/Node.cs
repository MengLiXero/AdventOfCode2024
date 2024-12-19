namespace AdventOfCode2024.Day16;

public class Node(int row, int col, int distance, Node? parent, DirectionType direction)
{
    public int Row { get; set; } = row;
    public int Col { get; set; } = col;
    public int Distance { get; set; } = distance;

    public DirectionType Direction { get; set; } = direction;

    public Node Parent { get; set; } = parent;
}