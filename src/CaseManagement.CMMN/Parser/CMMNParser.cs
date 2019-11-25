using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace CaseManagement.CMMN.Parser
{
    public class CMMNParser : ICMMNParser
    {
        public tDefinitions ParseWSDL(string cmmnTxt)
        {
            var xmlSerializer = new XmlSerializer(typeof(tDefinitions));
            tDefinitions defs = null;
            using (var txtReader = new StringReader(cmmnTxt))
            {
                defs = (tDefinitions)xmlSerializer.Deserialize(txtReader);
            }

            return defs;
        }

        public string Serialize(tDefinitions def)
        {
            var xmlSerializer = new XmlSerializer(typeof(tDefinitions));
            var strBuilder = new StringBuilder();
            using (var txtWriter = new StringWriter(strBuilder))
            {
                xmlSerializer.Serialize(txtWriter, def);
            }

            return strBuilder.ToString();
        }
    }
}
