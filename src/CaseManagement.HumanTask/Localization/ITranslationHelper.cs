using CaseManagement.HumanTask.Domains;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Localization
{
    public interface ITranslationHelper
    {
        Translation Translate(ICollection<Description> descriptions);
        Translation Translate(ICollection<Text> translations);
    }
}
