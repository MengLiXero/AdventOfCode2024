using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day3
{
    static string filePath;
    static string lines;
    public static void Index()
    {
        filePath = "../../../data-aoc-day3.txt";
        lines = File.ReadAllText(filePath);
        //Part1();
        Part2();
    }

    private static void Part2()
    {
        string[] linesArray = lines.Split("do()");
        var sum = 0;
        foreach (var line in linesArray)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var indexOfDont = line.IndexOf("don't()");
                var stringBeforeDont = "";
                stringBeforeDont = line.Substring(0, indexOfDont == -1 ? line.Length : indexOfDont);
                MatchCollection matches = Regex.Matches(stringBeforeDont, @"mul\((\d{1,3}),(\d{1,3})\)");
                foreach (Match match in matches)
                {
                    try
                    {
                        var firstNumber = match.Groups[1].Value;
                        var secondNumber = match.Groups[2].Value;
                        sum += int.Parse(firstNumber) * int.Parse(secondNumber);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        Console.WriteLine(sum);
    }

    private static void Part1()
    {
        string[] linesArray = lines.Split("mul(");
        var sum = 0;
        foreach (var line in linesArray)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var indexOfComma = line.IndexOf(',');
            if (indexOfComma == -1) continue;
            var stringBeforeComma = line.Substring(0, indexOfComma);
            var stringAfterComma = line.Substring(indexOfComma + 1);
            try
            {
                if (Regex.IsMatch(stringBeforeComma, @"^\d{1,3}$") &&
                    Regex.IsMatch(stringAfterComma, @"^(\d{1,3})\)"))
                {
                    var numberBeforeComma = int.Parse(stringBeforeComma);
                    var numberAfterComma = int.Parse(stringAfterComma.Substring(0, stringAfterComma.IndexOf(")")));
                    sum += numberBeforeComma * numberAfterComma;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        Console.WriteLine(sum);
    }
}