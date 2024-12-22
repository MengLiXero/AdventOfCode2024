namespace AdventOfCode2024.Day22;

public class Day22 : IAocDay
{
    private static long _sum;
    private const int times = 2000;
    private static Dictionary<long,List<long>> _secretNumbers = new Dictionary<long, List<long>>();

    public static void RunPart1()
    {
        var secretNumberArray = File.ReadAllLines(Constants.baseDir+"Day22/data-aoc-day22.txt").Select(long.Parse).ToArray();
        foreach (var secretNumber in secretNumberArray)
        {
            Console.WriteLine(secretNumber);
            _secretNumbers.Add(secretNumber, new List<long>(){secretNumber});
            var secreNumber2000 = CalculateSecretNumberAfterNTimes(secretNumber, times-1, secretNumber);
            _sum+=secreNumber2000;
        }
        Console.WriteLine($"Sum of secret numbers after {times} times: {_sum}");
    }

    private static long CalculateSecretNumberAfterNTimes(long secretNumber, int n, long originalSecretNumber)
    {
        if (n==0)
        {
            return secretNumber;
        }
            var result1 = step1(secretNumber);
            var result2 = step2(result1);
            var result3 = step3(result2);
            // Console.WriteLine($"Secret number after {times-n+1} times: {result3}");
            if (_secretNumbers.ContainsKey(originalSecretNumber))
            {
                _secretNumbers[originalSecretNumber].Add(result3);
            }
            else
            {
                _secretNumbers.Add(originalSecretNumber, new List<long>{result3});
            }
            return CalculateSecretNumberAfterNTimes(result3, n - 1, originalSecretNumber);
    }

    public static void RunPart2()
    {
        RunPart1();
        Dictionary<long, List<int>> InformationPriceInBananaCurrency = _secretNumbers.ToDictionary(
            kvp => kvp.Key, 
            kvp => kvp.Value.Select(num => (int)(num % 10)).ToList()
        );
        Dictionary<long,List<int>> AllPriceChange = new Dictionary<long, List<int>>();
        foreach (var secretNumber in InformationPriceInBananaCurrency)
        {
            var priceChange = new List<int>();
            for (int i = 1; i < secretNumber.Value.Count; i++)
            {
                priceChange.Add(secretNumber.Value[i] - secretNumber.Value[i-1]);
            }
            AllPriceChange.Add(secretNumber.Key, priceChange);
        }
        
        Dictionary<(long,(int,int,int,int)),int> FourConsecutivePriceChange = new Dictionary<(long, (int, int, int, int)), int>();
        foreach (var pricechange in AllPriceChange)
        {
            for(int i=4;i<pricechange.Value.Count+1;i++)
            {
                var fourConsecutivePriceChange = (pricechange.Value[i-4], pricechange.Value[i-3], pricechange.Value[i-2], pricechange.Value[i-1]);
                if (!FourConsecutivePriceChange.ContainsKey((pricechange.Key, fourConsecutivePriceChange)))
                {
                    FourConsecutivePriceChange.Add((pricechange.Key, fourConsecutivePriceChange), InformationPriceInBananaCurrency[pricechange.Key][i]);
                }
            }
        }

        var temp = new Dictionary<(int, int, int, int), int>() { };
        foreach (var pair in FourConsecutivePriceChange)
        {
            if(temp.ContainsKey(pair.Key.Item2))
            {
                temp[pair.Key.Item2]+=pair.Value;
            }
            else
            {
                temp.Add(pair.Key.Item2, pair.Value);
            }
        }
        
        var sortedTemp = temp.OrderByDescending(kvp => kvp.Value).FirstOrDefault();

        Console.WriteLine($"Tuple with highest value: {sortedTemp.Key}, Value: {sortedTemp.Value}");



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