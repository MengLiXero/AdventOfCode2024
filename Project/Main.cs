using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace AdventOfCode2024;

public class EntryPoint
{
    public static void Main(string[] args)
    {
        var options = new (string Description, Action Method)[]
        {
            // ("Run Day1 Part 1", Day1.RunPart1),
            // ("Run Day1 Part 2", Day1.RunPart2),
            // ("Run Day2 Part 1", Day2.RunPart1),
            // ("Run Day2 Part 2", Day2.RunPart2),
            // ("Run Day3 Part 1", Day3.RunPart1),
            // ("Run Day3 Part 2", Day3.RunPart2),
            // ("Run Day4 Part 1", Day4.RunPart1),
            // ("Run Day4 Part 2", Day4.RunPart2),
            // ("Run Day5 Part 1", Day5.RunPart1),
            // ("Run Day5 Part 2", Day5.RunPart2),
            ("Run Day6 Part 1", Day6.RunPart1),
            ("Run Day6 Part 2", Day6.RunPart2),
            ("Run Benchmark", () => ChooseBenchmarkToRun())
        };

        Console.WriteLine("Select an option to run:");
        for (int i = 0; i < options.Length; i++)
        {
            Console.WriteLine($"{i + 1}: {options[i].Description}");
        }

        Console.WriteLine("Enter the number of your choice:");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= options.Length)
        {
            Console.WriteLine($"Running: {options[choice - 1].Description}");
            options[choice - 1].Method.Invoke();
        }
        else
        {
            Console.WriteLine("Invalid choice. Exiting...");
        }
    }

    private static void ChooseBenchmarkToRun()
    {
        Console.WriteLine("Enter the benchmark to run (e.g., '6-1' for Day 6, Part 1 or '6-2' for Day 6, Part 2):");

        var input = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(input) || !input.Contains('-') || input.Split('-').Length != 2)
        {
            Console.WriteLine("Invalid input format. Use 'X-Y' format (e.g., '6-1').");
            return;
        }
        
        var parts = input.Split('-');
        if (int.TryParse(parts[0], out int day) &&
            int.TryParse(parts[1], out int part) &&
            day > 0 && day <= 30 && part > 0 && part <= 2)
        {
            Type benchmarkClassType = Type.GetType($"AdventOfCode2024.Day{day}");
            if (benchmarkClassType == null)
            {
                Console.WriteLine($"Benchmark class for Day {day} not found.");
                return;
            }

            string benchmarkName = $"BenchMarkPart{part}";

            Console.WriteLine($"Running benchmark for Day {day}, Part {part} ({benchmarkName})");

            var customConfig = new CustomConfig(benchmarkName);
            BenchmarkRunner.Run(benchmarkClassType, customConfig);
        }
        else
        {
            Console.WriteLine("Invalid input. Day must be between 1 and 20, and Part must be 1 or 2.");
        }
    }
}