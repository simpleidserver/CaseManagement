namespace CaseManagement.HumanTask
{
    public class HumanTaskServerOptions
    {
        public HumanTaskServerOptions()
        {
            OAuthTokenEndpoint = "http://localhost:60001/token";
            ClientId = "humanTaskClient";
            ClientSecret = "humanTaskClientSecret";
            Scope = "complete_humantask";
        }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
        public string OAuthTokenEndpoint { get; set; }
    }
}
