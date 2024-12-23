namespace AdventOfCode2024;

public class ICollectionHelper
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
    
    public static void AddToDictionary<Tkey,Tvalue>(Tkey key, Tvalue value, Dictionary<Tkey,HashSet<Tvalue>> dictionary)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key].Add(value);
        }
        else
        {
            dictionary[key] = new HashSet<Tvalue> {value};
        }
    }
    
    public static void PrintoutADictionaryWithHashSet<Tkey,Tvalue>(Dictionary<Tkey,HashSet<Tvalue>> dictionary)
    {
        foreach (var connection in dictionary)
        {
            foreach (var c in connection.Value)
            {
                Console.WriteLine($"Current connection: {connection.Key} - {c}");
            }

            Console.WriteLine($"Total connections of current computer {connection.Key}: {connection.Value.Count}");
        }
    }
}