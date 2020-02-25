using CaseManagement.CMMN.FormInstance.Commands;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.FormInstance.CommandHandlers
{
    public interface IConfirmFormInstanceCommandHandler
    {
        Task Handle(ConfirmFormInstanceCommand cmd);
    }
}
