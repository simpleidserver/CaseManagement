using CaseManagement.HumanTask.Domains;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Localization
{
    public interface ITranslationHelper
    {
        Translation Translate(ICollection<PresentationElementDefinition> presentationElts);
        Translation Translate(ICollection<PresentationElementInstance> presentationElts);
    }
}
