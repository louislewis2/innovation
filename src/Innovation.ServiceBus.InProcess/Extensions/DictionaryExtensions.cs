namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        public static void TryAdd<T, K>(this Dictionary<T, K> dictionary, T key, K value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }
    }
}