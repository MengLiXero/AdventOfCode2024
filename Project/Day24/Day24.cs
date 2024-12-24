namespace AdventOfCode2024.Day24;

public class Day24 : IAocDay
{
    private static Dictionary<string, int> _wireValues = new();
    private static List<(string input1, BitwiseOperation operation, string input2, string output)> _instructions = new();

    private static void Initialize()

    {
        var input = File.ReadAllLines(Constants.baseDir + "Day24/data-aoc-day24.txt");
        var isSection2 = false;
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                isSection2 = true;
                continue;
            }

            if (!isSection2)
            {
                _wireValues.Add(line.Split(": ")[0].Trim(), int.Parse(line.Split(": ")[1].Trim()));
                Console.WriteLine($"Wire: {line.Split(": ")[0].Trim()} Value: {int.Parse(line.Split(": ")[1].Trim())}");
            }
            else
            {
                _instructions.Add((line.Split(" ")[0], line.Split(" ")[1] switch
                {
                    "AND" => BitwiseOperation.And,
                    "OR" => BitwiseOperation.Or,
                    "XOR" => BitwiseOperation.Xor,
                    _ => throw new NotImplementedException()
                }, line.Split(" ")[2], line.Split(" ")[4]));
            }
        }
    }

    public static void RunPart1()
    {
        Initialize();
        var output = "";
        CalculateGateOutputs();
        var sortedZList = GenerateSortedZList();
        foreach (var wire in sortedZList)
        {
            output += wire.Value.ToString();
        }

        var newOutput = new string(output.Reverse().ToArray());
        long result = Convert.ToInt64(newOutput, 2);
        Console.WriteLine($"Result: {result}");
    }

    private static Dictionary<string, int> GenerateSortedZList()
    {
        var zList = new Dictionary<string, int>();
        foreach (var wire in _wireValues)
        {
            if (wire.Key.StartsWith('z'))
            {
                zList.Add(wire.Key.Substring(1), wire.Value);
            }
        }

        var sortedZList = zList.OrderBy(x => x.Key)
            .ToDictionary(x => x.Key, x => x.Value);
        return sortedZList;
    }

    private static void CalculateGateOutputs()
    {
        for (var i = 0; i < _instructions.Count; i++)
        {
            var instruction = _instructions[i];
            if (!_wireValues.ContainsKey(instruction.input1) || !_wireValues.ContainsKey(instruction.input2))
            {
                _instructions.Remove(instruction);
                _instructions.Add(instruction);
                i--;
                continue;
            }

            switch (instruction.operation)
            {
                case BitwiseOperation.And:
                    _wireValues[instruction.output] = _wireValues[instruction.input1] & _wireValues[instruction.input2];
                    break;
                case BitwiseOperation.Or:
                    _wireValues[instruction.output] = _wireValues[instruction.input1] | _wireValues[instruction.input2];
                    break;
                case BitwiseOperation.Xor:
                    _wireValues[instruction.output] = _wireValues[instruction.input1] ^ _wireValues[instruction.input2];
                    break;
            }
        }
    }

    public static void RunPart2()
    {
        throw new NotImplementedException();
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