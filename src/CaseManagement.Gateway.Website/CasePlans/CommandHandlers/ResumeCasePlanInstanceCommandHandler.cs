using CaseManagement.Gateway.Website.CasePlanInstance.Services;
using CaseManagement.Gateway.Website.CasePlans.Commands;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.CommandHandlers
{
    public class ResumeCasePlanInstanceCommandHandler : IResumeCasePlanInstanceCommandHandler
    {
        private readonly ICasePlanInstanceService _casePlanInstanceService;

        public ResumeCasePlanInstanceCommandHandler(ICasePlanInstanceService casePlanInstanceService)
        {
            _casePlanInstanceService = casePlanInstanceService;
        }

        public Task Handle(ResumeCasePlanInstanceCommand cmd)
        {
            return _casePlanInstanceService.ResumeMe(cmd.CasePlanInstanceId, cmd.IdentityToken);
        }
    }
}
