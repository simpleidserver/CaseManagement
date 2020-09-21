using System.Reflection;

namespace CaseManagement.Common
{
    public class CommonOptions
    {
        public CommonOptions()
        {
            NbRetry = 5;
            BlockThreadMS = 20;
            DeadLetterTimeMS = 1000;
            ApplicationAssembly = Assembly.GetCallingAssembly();
        }

        public int BlockThreadMS { get; set; }
        public int NbRetry { get; set; }
        public int DeadLetterTimeMS { get; set; }
        public Assembly ApplicationAssembly { get; set; }
    }
}
