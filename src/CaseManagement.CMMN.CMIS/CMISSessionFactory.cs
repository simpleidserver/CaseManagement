using Microsoft.Extensions.Options;
using PortCMIS.Client;
using PortCMIS.Client.Impl;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CMIS
{
    public class CMISSessionFactory : ICMISSessionFactory
    {
        private readonly CMISOptions _options;
        private ISession _session;

        public CMISSessionFactory(IOptions<CMISOptions> options)
        {
            _options = options.Value;
        }

        public ISession GetSession()
        {
            if (_session == null)
            {
                var factory = SessionFactory.NewInstance();
                _session = factory.CreateSession(new Dictionary<string, string>
                {
                    { SessionParameter.BindingType, _options.BindingType },
                    { SessionParameter.BrowserUrl, _options.BrowserUrl },
                    { SessionParameter.AtomPubUrl, _options.AtomPubUrl },
                    { SessionParameter.RepositoryId, _options.RepositoryId },
                    { SessionParameter.User, _options.User },
                    { SessionParameter.Password, _options.Password }
                });
            }

            return _session;
        }
    }
}
