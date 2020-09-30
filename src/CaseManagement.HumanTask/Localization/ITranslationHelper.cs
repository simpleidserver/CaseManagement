using CaseManagement.HumanTask.Domains;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Localization
{
    public interface ITranslationHelper
    {
        Translation Translate(ICollection<Description> descriptions, Dictionary<string, string> parameters);
        Translation Translate(ICollection<Text> translations, Dictionary<string, string> parameters);
    }
}
