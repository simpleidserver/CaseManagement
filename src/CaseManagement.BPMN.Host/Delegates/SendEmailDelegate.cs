using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Exceptions;
using System;
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
            var email = incoming.FirstOrDefault(i => i.Name == "email");
            if (email == null)
            {
                throw new BPMNProcessorException("Email parameter is missing");
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

                };
            }
            
            throw new NotImplementedException();
        }

        private class SendDelegateParameter
        {
            public string HttpBody { get; set; }
            public string Subject { get; set; }
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
