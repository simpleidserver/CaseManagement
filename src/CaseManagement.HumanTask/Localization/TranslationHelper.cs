using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Resources;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CaseManagement.HumanTask.Localization
{
    public class TranslationHelper : ITranslationHelper
    {
        private readonly ILogger<TranslationHelper> _logger;

        public TranslationHelper(ILogger<TranslationHelper> logger)
        {
            _logger = logger;
        }

        public Translation Translate(ICollection<Description> descriptions)
        {
            var result = descriptions.Cast<Text>().ToList();
            var translation = Translate(result);
            var description = descriptions.First(_ => _.Language == translation.Language);
            translation.ContentType = description.ContentType;
            return translation;
        }

        public Translation Translate(ICollection<Text> translations)
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            var code = culture.Name;
            var translation = translations.FirstOrDefault(_ => _.Language == code);
            if (translation == null)
            {
                _logger.LogError($"Missing translation for the language '{code}'");
                throw new BadOperationExceptions(string.Format(Global.MissingTranslation, code));
            }

            return new Translation
            {
                Value = translation.Value,
                Language = code
            };
        }
    }
}
