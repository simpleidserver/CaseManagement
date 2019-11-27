using CaseManagement.CMMN.Parser;
using System;
using System.Collections.Generic;
using System.IO;

namespace CaseManagement.CMMN
{
    public class CMMNDefinitionsBuilder
    {
        private readonly ICollection<tDefinitions> _definitions;

        public CMMNDefinitionsBuilder()
        {
            _definitions = new List<tDefinitions>();
        }

        public CMMNDefinitionsBuilder AddDefinition(tDefinitions def)
        {
            _definitions.Add(def);
            return this;
        }

        public CMMNDefinitionsBuilder ImportDefinition(string filePath)
        {
            var parser = new CMMNParser();
            var def = parser.ParseWSDL(File.ReadAllText(filePath));
            _definitions.Add(def);
            return this;
        }

        public ICollection<tDefinitions> Build()
        {
            return _definitions;
        }
    }
}
