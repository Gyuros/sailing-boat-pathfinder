namespace SailingBoatPathfinder.Logic.Extensions;

public static class DictionaryExtensions
{
    public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue fallbackValue)
    {
        if (dict.TryGetValue(key, out TValue? val))
        {
            return val;
        }
        
        dict.Add(key, fallbackValue);

        return fallbackValue;
    }
}