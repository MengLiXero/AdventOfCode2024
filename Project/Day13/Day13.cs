using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day13: IAocDay
{
    private static string[] _input;
    private static List<ClawMachine> _clawMachines = new List<ClawMachine>();

    private static void Innitialize()
    {
        _input  = File.ReadAllLines(Constants.baseDir + "Day13/data-aoc-day13.txt");
        for (int i = 0; i < _input.Length-2; i=i+4)
        {
            ClawMachine clawMachine = new ClawMachine();
            MatchCollection ButtonAMatches = Regex.Matches(_input[i], @"\d+");
            MatchCollection ButtonBMatches = Regex.Matches(_input[i+1], @"\d+");
            MatchCollection PrizeMatches = Regex.Matches(_input[i+2], @"\d+");
            if(ButtonAMatches.Count != 2 || ButtonBMatches.Count != 2 || PrizeMatches.Count != 2)
                throw new Exception("Invalid input");
            clawMachine.ButtonA = new CustomedVector2(long.Parse(ButtonAMatches[0].Value), long.Parse(ButtonAMatches[1].Value));
            clawMachine.ButtonB = new CustomedVector2(long.Parse(ButtonBMatches[0].Value), long.Parse(ButtonBMatches[1].Value));
            clawMachine.Prize = new CustomedVector2(long.Parse(PrizeMatches[0].Value), long.Parse(PrizeMatches[1].Value));
            _clawMachines.Add(clawMachine);
        }

        // foreach (var clawMachine in _clawMachines)
        // {
        //     Console.WriteLine(clawMachine.ToString());
        // }
    }
    public static void RunPart1()
    {
        Innitialize();
        FindCombinations();

        Console.WriteLine($"The minimum tokens to win all the prizes is {calculateMinimumToken()}");
        Console.WriteLine($"Number of craw machines that can grab the prize: {_clawMachines.Count(x => x.Combinations != null)}");
    }

    private static void FindCombinations()
    {
        foreach (var clawMachine in _clawMachines)
        {
            for (int a = 0;
                 a <= long.Min((long)(clawMachine.Prize.X / clawMachine.ButtonA.X),
                     (long)(clawMachine.Prize.Y / clawMachine.ButtonA.Y));
                 a++)
            {
                int b = 0;
                if ((clawMachine.Prize.X - clawMachine.ButtonA.X * a) % clawMachine.ButtonB.X != 0
                    || (clawMachine.Prize.Y - clawMachine.ButtonA.Y * a) % clawMachine.ButtonB.Y != 0)
                    continue;
                while (clawMachine.ButtonA.X * a + clawMachine.ButtonB.X * b < clawMachine.Prize.X
                       && clawMachine.ButtonA.Y * a + clawMachine.ButtonB.Y * b < clawMachine.Prize.Y)
                {
                    b++;
                }

                if (clawMachine.ButtonA.X * a + clawMachine.ButtonB.X * b == clawMachine.Prize.X
                    && clawMachine.ButtonA.Y * a + clawMachine.ButtonB.Y * b == clawMachine.Prize.Y)
                {
                    if (clawMachine.Combinations == null)
                    {
                        clawMachine.Combinations = new List<CustomedVector2>();
                    }

                    clawMachine.Combinations.Add(new CustomedVector2(a, b));
                }
            }
        }
    }

    private static long calculateMinimumToken()
    {
        long _sum = 0;
        foreach (var clawMachine in _clawMachines)
        {
            long min = 0;
            if(clawMachine.Combinations!=null)
            {
                min =  (int)clawMachine.Combinations.Min(x => x.X*3+x.Y);
            }

            _sum += min;
        }

        return _sum;
    }

    public static void RunPart2()
    {
        Innitialize();
        foreach (var clawMachine in _clawMachines)
        {
            var prize = clawMachine.Prize; 
            prize.X = 10000000000000 + prize.X;
            prize.Y = 10000000000000 + prize.Y;
            clawMachine.Prize = prize; 
        }
        FindCombinations();
        Console.WriteLine($"The minimum tokens to win all the prizes is {calculateMinimumToken()}");
        Console.WriteLine($"Number of craw machines that can grab the prize: {_clawMachines.Count(x => x.Combinations != null)}");
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
