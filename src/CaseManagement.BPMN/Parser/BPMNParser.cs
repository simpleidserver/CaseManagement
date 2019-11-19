using CaseManagement.BPMN.Domains;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CaseManagement.BPMN.Parser
{
    public class BPMNParser : IBPMNParser
    {
        private const string WSDL_NAMESPACE = "http://schemas.xmlsoap.org/wsdl/";

        public BPMNParsed ParseWSDL(string bpmnTxt, string dataDefsTxt, string interfacesTxt)
        {
            // TAKE TYPE DEFINITIONS FROM WSDL AND DATADEFINITIONS WSDL.
            var ns = new XmlSerializerNamespaces();
            var xmlSerializer = new XmlSerializer(typeof(tDefinitions));
            tDefinitions defs = null;
            using (var txtReader = new StringReader(bpmnTxt))
            {
                defs = (tDefinitions)xmlSerializer.Deserialize(txtReader);
            }

            var messageElements = new List<BPMNMessageElement>();
            using (var txtReader = new StringReader(dataDefsTxt))
            {
                var dataDefs = XmlSchema.Read(txtReader, null);
                foreach(var item in dataDefs.Items)
                {
                    messageElements.Add(ParseMessageElement(item));
                }
            }

            var operations = new List<BPMNOperation>();
            var itemDefinitions = defs.Items.Where(i => i is tItemDefinition);
            var messageDefinitions = defs.Items.Where(i => i is tMessage);
            using (var txtReader = new StringReader(interfacesTxt))
            {
                var xdoc = XDocument.Load(txtReader);
                var messages = xdoc.Descendants(XName.Get("message", WSDL_NAMESPACE));
                foreach(var message in messages)
                {
                    var nameAttribute = message.Attribute(XName.Get("name"));
                    var parts = message.Descendants(XName.Get("part", WSDL_NAMESPACE));
                    var record = new BPMNOperation
                    {
                        Name = nameAttribute.Value
                    };
                    foreach (var part in parts)
                    {
                        nameAttribute = part.Attribute(XName.Get("name"));
                        var elementAttribute = part.Attribute(XName.Get("element"));
                        if (elementAttribute != null)
                        {
                            record.Parts.Add(messageElements.First(m => m.Name == RemoveNamespace(elementAttribute.Value)));
                        }
                        else
                        {
                            var type = part.Attribute(XName.Get("type"));
                            record.Parts.Add(new BPMNMessageElement
                            {
                                Name = nameAttribute.Value,
                                Type = type.Value
                            });
                        }
                    }

                    operations.Add(record);
                }
            }

            return new BPMNParsed(defs, messageElements, operations);
        }

        private static BPMNMessageElement ParseMessageElement(XmlSchemaObject xmlSchemaObj)
        {
            if (xmlSchemaObj is XmlSchemaComplexType)
            {
                var complexAttr = ((XmlSchemaComplexType)xmlSchemaObj);
                var sequence = (XmlSchemaSequence)complexAttr.Particle;
                var complexMessageElement = new BPMNComplexMessageElement
                {
                    Name = complexAttr.Name
                };
                foreach(var item in sequence.Items)
                {
                    complexMessageElement.Items.Add(ParseMessageElement(item));
                }

                return complexMessageElement;
            }

            if (xmlSchemaObj is XmlSchemaElement)
            {
                var attr = ((XmlSchemaElement)xmlSchemaObj);
                if (attr.SchemaType is XmlSchemaComplexType)
                {
                    var complexAttr = (XmlSchemaComplexType)attr.SchemaType;
                    var sequence = (XmlSchemaSequence)complexAttr.Particle;
                    var complexMessageElement = new BPMNComplexMessageElement
                    {
                        Name = attr.Name
                    };
                    foreach (var item in sequence.Items)
                    {
                        complexMessageElement.Items.Add(ParseMessageElement(item));
                    }

                    return complexMessageElement;
                }

                return new BPMNMessageElement
                {
                    Name = attr.Name,
                    Type = attr.SchemaTypeName.Name
                };
            }

            return null;
        }

        private static string RemoveNamespace(string str)
        {
            var splitted = str.Split(':');
            if (splitted.Count() != 2)
            {
                return str;
            }

            return splitted.Last();
        }
    }
}
