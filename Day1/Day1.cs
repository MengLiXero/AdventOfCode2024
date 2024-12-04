namespace AdventOfCode2024;

public class Day1
{
    public static void Index()
    {
        string csvFilePath = "../../../data-aoc-day1.csv";
        string[] lines = File.ReadAllLines(csvFilePath);
        var firstColumn = new List<string>();

        var secondColumn = new List<string>();

        foreach (string line in File.ReadLines(csvFilePath))
        {
            string[] columns = line.Split(',');

            if (columns.Length >= 2)
            {
                firstColumn.Add(columns[0]);
                secondColumn.Add(columns[1]);
            }
        }

        firstColumn.Sort();
        secondColumn.Sort();

// var totalDistance = 0;
        var similarity = 0;
        for (int i = 0; i<secondColumn.Count;
             i++)
        {
            //totalDistance += Math.Abs(int.Parse(firstColumn[i]) - int.Parse(secondColumn[i]));
            for (int j = 0; j < firstColumn.Count; j++)
            {
                if (firstColumn[j] == secondColumn[i])
                {
                    similarity += int.Parse(secondColumn[i]);
                }
            }
        }

// Console.WriteLine(totalDistance);
        Console.WriteLine(similarity);
    }
    
}