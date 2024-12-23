using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day23 : IAocDay
{
    private static string[]? _input;
    private static Dictionary<string,HashSet<string>> _connections = new();
    private static readonly HashSet<HashSet<string>> ThreeInterConnectedComputers = new();
    private static readonly HashSet<HashSet<string>> ThreeInterConnectedComputersWithT = new();
    private static HashSet<string> _largestInterConnectedGroup = new();
    private static void Initialize()
    {
        _input = File.ReadAllLines(Constants.baseDir + "Day23/data-aoc-day23.txt");
        foreach (var line in _input)
        {
            var pattern = @"^(.*?)-(.*?)$";
            var match = Regex.Match(line, pattern);
            if (match.Success)
            {
                var left = match.Groups[1].Value;
                var right = match.Groups[2].Value;
                ICollectionHelper.AddToDictionary(left, right,
                    _connections);
                ICollectionHelper.AddToDictionary(right, left,
                    _connections);
            }
        }

        ICollectionHelper.PrintoutADictionaryWithHashSet(_connections);
    }
    public static void RunPart1()
    {
        Initialize();
        FindThreeInterConnectedComputers();
        FindThreeInterConnectedComputersWithT();
    }

    private static void FindThreeInterConnectedComputersWithT()
    {
        foreach (var group in ThreeInterConnectedComputers)
        {
            foreach (var c in group)
            {
                if (c.StartsWith('t'))
                {
                    ThreeInterConnectedComputersWithT.Add(group);
                }
            }
        }
        Console.WriteLine($"The sum of three inter connected computers with T: {ThreeInterConnectedComputersWithT.Count}");
    }

    private static void FindThreeInterConnectedComputers()
    {
        foreach (var kvp in _connections)
        {
            for (int i = 0; i < kvp.Value.Count; i++)
            {
                for (int j = i + 1; j < kvp.Value.Count; j++)
                {
                    if (_connections[kvp.Value.ElementAt(i)].Contains(kvp.Value.ElementAt(j)))
                    {
                        var newSet = new HashSet<string>() { kvp.Key, kvp.Value.ElementAt(i), kvp.Value.ElementAt(j) };
                        if (!ThreeInterConnectedComputers.Any(existingSet => existingSet.SetEquals(newSet)))
                        {
                            ThreeInterConnectedComputers.Add(newSet);
                        }
                    }
                }
            }
        }

        Console.WriteLine($"Total three inter connected computers: {ThreeInterConnectedComputers.Count}");
    }

    public static void RunPart2()
    {
        Initialize();
        FindLargestInterConnectedGroup();
        List<string> largestInterConnectedGroupList = _largestInterConnectedGroup.OrderBy(name => name).ToList();
        string password = string.Join(",", largestInterConnectedGroupList);
        Console.WriteLine($"The password is: {password}");
    }

    private static void FindLargestInterConnectedGroup()
    {
        FindThreeInterConnectedComputers();
        var maxConnectedGroupCount = -1;
        foreach (var group in ThreeInterConnectedComputers)
        {
            foreach (var kvp in _connections)
            {
                if(group.Contains(kvp.Key))
                {
                    continue;
                }
                
                var found = true;
                foreach (var computer in group)
                {
                    if (!_connections[kvp.Key].Contains(computer))
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    group.Add(kvp.Key);
                }
            }
            if(maxConnectedGroupCount < group.Count)
            {
                maxConnectedGroupCount = group.Count;
                _largestInterConnectedGroup.Clear();
                _largestInterConnectedGroup = group;
            }
        }
        
        Console.WriteLine($"Max connected group count: {maxConnectedGroupCount}");
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