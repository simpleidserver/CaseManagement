using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.CaseFile.CommandHandlers;
using CaseManagement.CMMN.CaseFile.Commands;
using CaseManagement.CMMN.CaseFile.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Apis
{
    [Route(CMMNConstants.RouteNames.CaseFiles)]
    public class CaseFilesController : Controller
    {
        private readonly ICaseFileQueryRepository _queryRepository;
        private readonly IAddCaseFileCommandHandler _addCaseFileCommandHandler;
        private readonly IUpdateCaseFileCommandHandler _updateCaseFileCommandHandler;
        private readonly IPublishCaseFileCommandHandler _publishCaseFileCommandHandler;

        public CaseFilesController(ICaseFileQueryRepository queryRepository, IAddCaseFileCommandHandler addCaseFileCommandHandler, IUpdateCaseFileCommandHandler updateCaseFileCommandHandler, IPublishCaseFileCommandHandler publishCaseFileCommandHandler)
        {
            _queryRepository = queryRepository;
            _addCaseFileCommandHandler = addCaseFileCommandHandler;
            _updateCaseFileCommandHandler = updateCaseFileCommandHandler;
            _publishCaseFileCommandHandler = publishCaseFileCommandHandler;
        }

        [HttpGet("count")]
        [Authorize("get_statistic")]
        public async Task<IActionResult> Count()
        {
            var result = await _queryRepository.Count();
            return new OkObjectResult(new
            {
                count = result
            });
        }

        [HttpPost]
        [Authorize("add_casefile")]
        public async Task<IActionResult> Create([FromBody] AddCaseFileCommand parameter)
        {
            try
            {
                parameter.NameIdentifier = this.GetNameIdentifier();
                var result = await _addCaseFileCommandHandler.Handle(parameter);
                var jObj = new JObject
                {
                    { "id", result }
                };
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    Content = jObj.ToString()
                };
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPut("{id}")]
        [Authorize("update_casefile")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateCaseFileCommand parameter)
        {
            try
            {
                parameter.Id = id;
                parameter.Performer = this.GetNameIdentifier();
                await _updateCaseFileCommandHandler.Handle(parameter);
                return new OkResult();
            }
            catch(UnknownCaseFileException)
            {
                return new NotFoundResult();
            }
            catch(UnauthorizedCaseFileException)
            {
                return new UnauthorizedResult();
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpGet("{id}/publish")]
        [Authorize("publish_casefile")]
        public async Task<IActionResult> Publish(string id)
        {
            try
            {
                var cmd  = new PublishCaseFileCommand
                {
                    Id = id,
                    Performer = this.GetNameIdentifier()
                };
                await _publishCaseFileCommandHandler.Handle(cmd);
                return new OkResult();
            }
            catch (UnknownCaseFileException)
            {
                return new NotFoundResult();
            }
            catch (UnauthorizedCaseFileException)
            {
                return new UnauthorizedResult();
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _queryRepository.Find(ExtractFindParameter(query));
            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _queryRepository.FindById(id);
            if (result == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(ToDto(result));
        }

        private static JObject ToDto(FindResponse<CaseFileAggregate> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => ToDto(r))) }
            };
        }

        private static JObject ToDto(CaseFileAggregate resp)
        {
            return new JObject
            {
                { "id", resp.Id },
                { "name", resp.Name },
                { "description", resp.Description },
                { "payload", resp.Payload },
                { "create_datetime", resp.CreateDateTime },
                { "update_datetime", resp.UpdateDateTime }
            };
        }

        private static FindCaseFilesParameter ExtractFindParameter(IEnumerable<KeyValuePair<string, string>> query)
        {
            string owner;
            string text;
            var parameter = new FindCaseFilesParameter();
            parameter.ExtractFindParameter(query);
            if (query.TryGet("owner", out owner))
            {
                parameter.Owner = owner;
            }

            if (query.TryGet("text", out text))
            {
                parameter.Text = text;
            }

            return parameter;
        }
    }
}