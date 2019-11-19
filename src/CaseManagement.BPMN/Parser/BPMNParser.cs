using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CaseManagement.BPMN.Parser
{
    public class BPMNParser : IBPMNParser
    {
        private const string WSDL_NAMESPACE = "http://schemas.xmlsoap.org/wsdl/";

        public BPMNParsed ParseWSDL(string bpmnTxt, string dataDefsTxt, string interfacesTxt)
        {
            var ns = new XmlSerializerNamespaces();
            var xmlSerializer = new XmlSerializer(typeof(tDefinitions));
            tDefinitions defs = null;
            using (var txtReader = new StringReader(bpmnTxt))
            {
                defs = (tDefinitions)xmlSerializer.Deserialize(txtReader);
            }

            /*
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
            var itemDefinitions = defs.Items.Where(i => i is tItemDefinition).Cast<tItemDefinition>();
            var messages = defs.Items.Where(i => i is tMessage).Cast<tMessage>();
            var interfaces = defs.Items.Where(i => i is tInterface).Cast<tInterface>();
            var wsdlMessages = ExtractMessagesFromWSDL(interfacesTxt, messageElements);
            foreach (var intf in interfaces)
            {
                foreach (var op in intf.operation)
                {
                    var bpmnOp = new BPMNOperation
                    {
                        Id = op.id,
                        Name = op.name
                    };
                    var inMessage = messages.FirstOrDefault(m => m.id == op.inMessageRef.Name);
                    if (inMessage != null)
                    {
                        var inMessageItemDefinition = itemDefinitions.First(i => i.id == inMessage.itemRef.Name);
                        bpmnOp.InMessage = wsdlMessages.First(w => w.Name == inMessageItemDefinition.structureRef.Name);
                    }

                    var outMessage = messages.FirstOrDefault(m => m.id == op.outMessageRef.Name);
                    if (outMessage != null)
                    {
                        var outMessageItemDefinition = itemDefinitions.First(i => i.id == outMessage.itemRef.Name);
                        bpmnOp.OutMessage = wsdlMessages.First(w => w.Name == outMessageItemDefinition.structureRef.Name);
                    }
                    
                    if (op.errorRef != null)
                    {
                        foreach (var fm in op.errorRef)
                        {
                            var errorMessage = messages.First(m => m.id == fm.Name);
                            var errorMessageItemDefinition = itemDefinitions.First(i => i.id == errorMessage.itemRef.Name);
                            bpmnOp.ErrorMessages.Add(wsdlMessages.First(w => w.Name == errorMessageItemDefinition.structureRef.Name));
                        }
                    }

                    operations.Add(bpmnOp);
                }
            }
            return new BPMNParsed(defs.Items.Where(t => t is tProcess).Cast<tProcess>().ToList(), operations);
            */

            return new BPMNParsed(defs.Items.Where(t => t is tProcess).Cast<tProcess>().ToList());
        }

        /*
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

        private ICollection<BPMNMessageElement> ExtractMessagesFromWSDL(string interfacesTxt, List<BPMNMessageElement> existingMessages)
        {
            var result = new List<BPMNMessageElement>();
            using (var txtReader = new StringReader(interfacesTxt))
            {
                var xdoc = XDocument.Load(txtReader);
                var messages = xdoc.Descendants(XName.Get("message", WSDL_NAMESPACE));
                foreach (var message in messages)
                {
                    var nameAttribute = message.Attribute(XName.Get("name"));
                    var nameAttributeValue = nameAttribute.Value;
                    var parts = message.Descendants(XName.Get("part", WSDL_NAMESPACE));
                    var recordParts = new List<BPMNMessageElement>();
                    foreach (var part in parts)
                    {
                        nameAttribute = part.Attribute(XName.Get("name"));
                        var elementAttribute = part.Attribute(XName.Get("element"));
                        if (elementAttribute != null)
                        {
                            recordParts.Add(existingMessages.First(m => m.Name == RemoveNamespace(elementAttribute.Value)));
                        }
                        else
                        {
                            var type = part.Attribute(XName.Get("type"));
                            recordParts.Add(new BPMNMessageElement
                            {
                                Name = nameAttribute.Value,
                                Type = type.Value
                            });
                        }
                    }

                    result.Add(new BPMNComplexMessageElement
                    {
                        Name = nameAttributeValue,
                        Items = recordParts
                    });
                }
            }

            return result;
        }

        */

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
