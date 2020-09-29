namespace System.Collections.Generic
{
    public static class GenericCollectionExtensions
    {
        public static bool IsEmpty(this KeyValuePair<string, string> kvp)
        {
            if (kvp.Equals(default(KeyValuePair<string, string>)) || string.IsNullOrWhiteSpace(kvp.Value))
            {
                return true;
            }

            return false;
        }
    }
}
