using PortCMIS.Client;

namespace CaseManagement.CMMN.CMIS
{
    public interface ICMISSessionFactory
    {
        ISession GetSession();
    }
}
