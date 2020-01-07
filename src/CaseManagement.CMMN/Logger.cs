using System.Collections.Generic;
using System.IO;

namespace CaseManagement.CMMN
{
    public class Logger
    {
        private static object _obj = new object();

        public static void WriteLine(string msg)
        {
            lock(_obj)
            {
                var path = Path.Combine(@"c:\Projects\CaseManagement\", "output.txt");
                if (!File.Exists(path))
                {
                    File.Create(path).Close();
                }

                File.AppendAllLines(path, new List<string>
                {
                    msg
                });
            }
        }
    }
}
