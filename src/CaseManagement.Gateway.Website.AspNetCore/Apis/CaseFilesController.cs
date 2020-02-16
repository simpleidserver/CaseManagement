using CaseManagement.Gateway.Website.AspNetCore.Extensions;
using CaseManagement.Gateway.Website.CaseFile.CommandHandlers;
using CaseManagement.Gateway.Website.CaseFile.Commands;
using CaseManagement.Gateway.Website.CaseFile.DTOs;
using CaseManagement.Gateway.Website.CaseFile.Queries;
using CaseManagement.Gateway.Website.CaseFile.QueryHandlers;
using CaseManagement.Gateway.Website.CasePlans.QueryHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.AspNetCore.Apis
{
    [Route(ServerConstants.RouteNames.CaseFiles)]
    public class CaseFilesController : Controller
    {
        private readonly IAddCaseFileCommandHandler _addCaseFileCommandHandler;
        private readonly ISearchMyLatestCaseFileQueryHandler _searchMyLatestPublishedCaseFileQueryHandler;
        private readonly ISearchCaseFileHistoryQueryHandler _searchCaseFileHistoryQueryHandler;
        private readonly IGetCaseFileQueryHandler _getCaseFileQueryHandler;
        private readonly IUpdateCaseFileCommandHandler _updateCaseFileCommandHandler;
        private readonly IPublishCaseFileCommandHandler _publishCaseFileCommandHandler;
        
        public CaseFilesController(IAddCaseFileCommandHandler addCaseFileCommandHandler, ISearchMyLatestCaseFileQueryHandler searchMyLatestPublishedCaseFileQueryHandler, ISearchCaseFileHistoryQueryHandler searchCaseFileHistoryQueryHandler, IGetCaseFileQueryHandler getCaseFileQueryHandler, IUpdateCaseFileCommandHandler updateCaseFileCommandHandler, IPublishCaseFileCommandHandler  publishCaseFileCommandHandler, ISearchMyLatestCasePlanQueryHandler searchMyLatestCasePlanQueryHandler)
        {
            _addCaseFileCommandHandler = addCaseFileCommandHandler;
            _searchMyLatestPublishedCaseFileQueryHandler = searchMyLatestPublishedCaseFileQueryHandler;
            _searchCaseFileHistoryQueryHandler = searchCaseFileHistoryQueryHandler;
            _getCaseFileQueryHandler = getCaseFileQueryHandler;
            _updateCaseFileCommandHandler = updateCaseFileCommandHandler;
            _publishCaseFileCommandHandler = publishCaseFileCommandHandler;
        }

        [HttpPost]
        [Authorize("add_casefile")]
        public async Task<IActionResult> Add([FromBody] AddCaseFileCommand command)
        {
            command.Owner = this.GetNameIdentifier();
            var resut = await _addCaseFileCommandHandler.Handle(command);
            return new ContentResult
            {
                StatusCode = (int)HttpStatusCode.Created,
                Content = new JObject
                {
                    { "id", resut }
                }.ToString(),
                ContentType = "application/json"
            };
        }

        [HttpGet("me/search")]
        [Authorize("get_casefile")]
        public async Task<IActionResult> SearchMyLatest()
        {
            var query = Request.Query.ToEnumerable();
            var nameIdentifier = this.GetNameIdentifier();
            var result = await _searchMyLatestPublishedCaseFileQueryHandler.Handle(new SearchMyLatestPublishedCaseFileQuery(query, nameIdentifier));
            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("{id}/history/search")]
        [Authorize("get_casefile")]
        public async Task<IActionResult> SearchHistory(string id)
        {
            var query = Request.Query.ToEnumerable();
            var nameIdentifier = this.GetNameIdentifier();
            var result = await _searchCaseFileHistoryQueryHandler.Handle(new SearchCaseFileHistoryQuery
            {
                CaseFileId = id,
                NameIdentifier = nameIdentifier,
                Queries = query
            });
            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("{id}")]
        [Authorize("get_casefile")]
        public async Task<IActionResult> GetCaseFile(string id)
        {
            var result = await _getCaseFileQueryHandler.Handle(new GetCaseFileQuery { CaseFileId = id });
            if (result.Owner != this.GetNameIdentifier())
            {
                return new UnauthorizedResult();
            }

            return new OkObjectResult(ToDto(result));
        }

        [HttpPut("{id}")]
        [Authorize("update_casefile")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateCaseFileCommand command)
        {
            command.CaseFileId = id;
            command.Performer = this.GetNameIdentifier();
            await _updateCaseFileCommandHandler.Handle(command);
            return new OkResult();
        }
        
        [HttpGet("{id}/publish")]
        [Authorize("publish_casefile")]
        public async Task<IActionResult> Publish(string id)
        {
            var result = await _publishCaseFileCommandHandler.Handle(new PublishCaseFileCommand { CaseFileId = id, Performer = this.GetNameIdentifier() });
            return new OkObjectResult(new JObject
            {
                { "id", result }
            });
        }

        private static JObject ToDto(FindResponse<CaseFileResponse> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => ToDto(r))) }
            };
        }

        private static JObject ToDto(CaseFileResponse resp)
        {
            return new JObject
            {
                { "id", resp.Id },
                { "name", resp.Name },
                { "description", resp.Description },
                { "payload", resp.Payload },
                { "create_datetime", resp.CreateDateTime },
                { "update_datetime", resp.UpdateDateTime },
                { "version", resp.Version },
                { "file_id", resp.FileId },
                { "owner", resp.Owner },
                { "status", resp.Status }
            };
        }
    }
}
