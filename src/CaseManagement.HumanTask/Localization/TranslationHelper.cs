using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Resources;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        public Translation Translate(ICollection<Description> descriptions, Dictionary<string, string> parameters)
        {
            var result = descriptions.Cast<Text>().ToList();
            var translation = Translate(result, parameters);
            var description = descriptions.First(_ => _.Language == translation.Language);
            translation.ContentType = description.ContentType;
            return translation;
        }

        public Translation Translate(ICollection<Text> translations, Dictionary<string, string> parameters)
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            var code = culture.Name;
            var translation = translations.FirstOrDefault(_ => _.Language == code);
            if (translation == null)
            {
                _logger.LogError($"Missing translation for the language '{code}'");
                throw new BadOperationExceptions(string.Format(Global.MissingTranslation, code));
            }

            var value = translation.Value;
            var result = Parse(value, parameters);
            return new Translation
            {
                Value = result,
                Language = code
            };
        }

        private static string Parse(string str, Dictionary<string, string> parameters)
        {
            var regex = new Regex(@"\$([a-zA-Z]|_)*\$");
            var result = regex.Replace(str, (m) =>
            {
                if (string.IsNullOrWhiteSpace(m.Value))
                {
                    return string.Empty;
                }

                var key = m.Value.TrimStart('$').TrimEnd('$');
                if (!parameters.ContainsKey(key))
                {
                    return string.Empty;
                }

                return parameters[key];
            });

            return result;
        }
    }
}
