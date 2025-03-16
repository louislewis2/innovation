namespace System.Collections.Generic
{
    public static class IDictionaryExtensions
    {
        public static int Length(this IDictionary<string, string[]> keyValuePairs)
        {
            if (keyValuePairs == null)
            {
                return -1;
            }

            return keyValuePairs.Count;
        }
    }
}
