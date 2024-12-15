using System.Numerics;

namespace AdventOfCode2024;

public class ClawMachine
{
    public CustomedVector2 ButtonA { get; set; }
    public CustomedVector2 ButtonB { get; set; }
    public CustomedVector2 Prize { get; set; }
    
    public List<CustomedVector2> Combinations { get; set; }
    
    public string ToString()
    {
        return $" Button A: {ButtonA}\n Button B: {ButtonB}\n Prize: {Prize}";
    }
}