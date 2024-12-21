namespace AdventOfCode2024;

public class ListHelper
{
    public static List<List<T>> CombineTwoListsOfLists<T>(List<List<T>> list1, List<List<T>> list2)
    {
        var result = new List<List<T>>();
        foreach (var l1 in list1)
        {
            foreach (var l2 in list2)
            {
                var combinedList = new List<T>();
                combinedList.AddRange(l1);
                combinedList.AddRange(l2);
                result.Add(combinedList);
            }
        }

        return result;
    }
}