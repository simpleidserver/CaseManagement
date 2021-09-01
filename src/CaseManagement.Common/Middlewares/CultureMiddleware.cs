using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Common.Middlewares
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public CultureMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<CultureMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            const string acceptLanguage = "Accept-Language";
            if (context.Request.Headers.ContainsKey(acceptLanguage))
            {
                var languages = context.Request.Headers[acceptLanguage];
                CultureInfo culture = null;
                foreach(var lang in languages)
                {
                    try
                    {
                        culture = CultureInfo.GetCultureInfo(lang);
                    }
                    catch (CultureNotFoundException) { }
                }

                if (culture != null && IsCultureValid(culture.Name))
                {
                    _logger.LogInformation($"Culture '{culture.ThreeLetterISOLanguageName}' is used");
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }
            }

            await _next.Invoke(context);
        }

        private static bool IsCultureValid(string cultureName)
        {
            for (int i = 0; i < cultureName.Length; i++)
            {
                char c = cultureName[i];
                if (char.IsLetterOrDigit(c) || c == '-' || c == '_')
                {
                    continue;
                }

                return false;
            }

            return true;
        }
    }
}
