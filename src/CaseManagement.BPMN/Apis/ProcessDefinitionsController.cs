using Microsoft.AspNetCore.Mvc;

namespace CaseManagement.BPMN.Apis
{
    [Route(BPMNConstants.RouteNames.ProcessDefinitions)]
    public class ProcessDefinitionsController : Controller
    {
        public ProcessDefinitionsController()
        {
            
        }
        
        public IActionResult Get()
        {
            return null;
        }
    }
}
