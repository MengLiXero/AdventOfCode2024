namespace AdventOfCode2024.Day22;

public class Day22 : IAocDay
{
    private static long _sum;
    private const int times = 2000;

    public static void RunPart1()
    {
        var secretNumberArray = File.ReadAllLines(Constants.baseDir+"Day22/data-aoc-day22.txt").Select(long.Parse).ToArray();
        foreach (var secretNumber in secretNumberArray)
        {
            Console.WriteLine(secretNumber);
            var secreNumber2000 = CalculateSecretNumberAfterNTimes(secretNumber, times);
            _sum+=secreNumber2000;
        }
        Console.WriteLine($"Sum of secret numbers after {times} times: {_sum}");
    }

    private static long CalculateSecretNumberAfterNTimes(long secretNumber, int n)
    {
        if (n==0)
        {
            return secretNumber;
        }
            var result1 = step1(secretNumber);
            var result2 = step2(result1);
            var result3 = step3(result2);
            // Console.WriteLine($"Secret number after {times-n+1} times: {result3}");
            return CalculateSecretNumberAfterNTimes(result3, n - 1);
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
    
    private static long step1(long secretNumber)
    {
        long result = 0;
        long temp = secretNumber * 64;
        result = secretNumber ^ temp;
        result %= 16777216;
        return result;
    }
    
    private static long step2(long secretNumber)
    {
        long result = 0;
        long temp = secretNumber / 32;
        result = secretNumber ^ temp;
        result %= 16777216;
        return result;
    }
    
    private static long step3(long secretNumber)
    {
        long result = 0;
        long temp = secretNumber * 2048 ;
        result = secretNumber ^ temp;
        result %= 16777216;
        return result;
    }
}