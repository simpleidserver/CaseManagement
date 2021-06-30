using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Host.Delegates
{
    public class SendEmailDelegate : IDelegateHandler
    {
        public Task<ICollection<MessageToken>> Execute(ICollection<MessageToken> incoming, DelegateConfigurationAggregate delegateConfiguration, CancellationToken cancellationToken)
        {
            var user = incoming.FirstOrDefault(i => i.Name == "userMessage");
            if (user == null)
            {
                throw new BPMNProcessorException("userMessage must be passed in the request");
            }

            var email = user.GetProperty("email");
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new BPMNProcessorException("email is not passed in the request");
            }

            var parameter = SendDelegateParameter.Build(delegateConfiguration);
            using (var smtpClient = new SmtpClient(parameter.SmtpHost)
            {
                Port = parameter.SmtpPort,
                Credentials = new NetworkCredential(parameter.SmtpUserName, parameter.SmtpPassword),
                EnableSsl = parameter.SmtpEnableSsl
            })
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(parameter.FromEmail),
                    Subject = parameter.Subject,
                    Body = parameter.HttpBody,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);
                smtpClient.Send(mailMessage);
            }

            return Task.FromResult(incoming);
        }

        private class SendDelegateParameter
        {
            public string HttpBody { get; set; }
            public string Subject { get; set; }
            public string FromEmail { get; set; }
            public string SmtpHost { get; set; }
            public int SmtpPort { get; set; }
            public string SmtpUserName { get; set; }
            public string SmtpPassword { get; set; }
            public bool SmtpEnableSsl { get; set; }

            public static SendDelegateParameter Build(DelegateConfigurationAggregate delegateConfiguration)
            {
                return new SendDelegateParameter
                {
                    HttpBody = delegateConfiguration.GetValue("httpBody"),
                    Subject = delegateConfiguration.GetValue("subject"),
                    FromEmail = delegateConfiguration.GetValue("fromEmail"),
                    SmtpHost = delegateConfiguration.GetValue("smtpHost"),
                    SmtpPort = delegateConfiguration.GetValueNumber("smtpPort"),
                    SmtpUserName = delegateConfiguration.GetValue("smtpUserName"),
                    SmtpPassword = delegateConfiguration.GetValue("smtpPassword"),
                    SmtpEnableSsl = delegateConfiguration.GetValueBoolean("smtpEnableSsl")
                };
            }
        }
    }
}
