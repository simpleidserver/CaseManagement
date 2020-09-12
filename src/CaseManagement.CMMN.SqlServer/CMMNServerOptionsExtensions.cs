namespace CaseManagement.CMMN
{
    public static class CMMNServerOptionsExtensions
    {
        private const string CONNECTION_STRING_NAME = "connection_string";

        public static void SetConnectionString(this CMMNServerOptions serverOptions, string value)
        {
            if(!serverOptions.Metadata.ContainsKey(CONNECTION_STRING_NAME))
            {
                serverOptions.Metadata.Add(CONNECTION_STRING_NAME, value);
                return;
            }

            serverOptions.Metadata[CONNECTION_STRING_NAME] = value;
        }

        public static string GetConnectionString(this CMMNServerOptions serverOptions)
        {
            if (!serverOptions.Metadata.ContainsKey(CONNECTION_STRING_NAME))
            {
                return null;
            }

            return serverOptions.Metadata[CONNECTION_STRING_NAME];
        }
    }
}
