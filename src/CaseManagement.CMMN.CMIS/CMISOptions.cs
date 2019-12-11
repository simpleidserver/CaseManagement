namespace CaseManagement.CMMN.CMIS
{
    public class CMISOptions
    {
        public CMISOptions()
        {
            BindingType = PortCMIS.BindingType.AtomPub;
            BrowserUrl = "http://localhost:8080/chemistry-opencmis-server-inmemory-1.1.0/browser";
            AtomPubUrl = "http://localhost:8080/chemistry-opencmis-server-inmemory-1.1.0/atom11";
            RepositoryId = "A1";
            User = "test";
            Password = "test";
            WaitMs = 1000;
        }

        public string BindingType { get; set; }
        public string BrowserUrl { get; set; }
        public string AtomPubUrl { get; set; }
        public string RepositoryId { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int WaitMs { get; set; }
    }
}
