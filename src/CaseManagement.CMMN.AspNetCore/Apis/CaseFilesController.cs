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
using System;
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

        [HttpPost("me")]
        [Authorize("me_add_casefile")]
        public Task<IActionResult> AddMe([FromBody] AddCaseFileCommand parameter)
        {
            parameter.Owner = this.GetNameIdentifier();
            return InternalAdd(parameter);
        }

        [HttpPost]
        [Authorize("add_casefile")]
        public Task<IActionResult> Add([FromBody] AddCaseFileCommand parameter)
        {
            return InternalAdd(parameter);
        }

        [HttpPut("me/{id}")]
        [Authorize("me_update_casefile")]
        public Task<IActionResult> UpdateMe(string id, [FromBody] UpdateCaseFileCommand parameter)
        {
            parameter.Id = id;
            parameter.BypassUserValidation = false;
            parameter.Performer = this.GetNameIdentifier();
            return InternalUpdate(parameter);
        }

        [HttpPut("{id}")]
        [Authorize("update_casefile")]
        public Task<IActionResult> Update(string id, [FromBody] UpdateCaseFileCommand parameter)
        {
            parameter.Id = id;
            parameter.BypassUserValidation = true;
            return InternalUpdate(parameter);
        }

        [HttpGet("me/{id}/publish")]
        [Authorize("me_publish_casefile")]
        public Task<IActionResult> PublishMe(string id)
        {
            var cmd = new PublishCaseFileCommand
            {
                Id = id,
                BypassUserValidation = false,
                Performer = this.GetNameIdentifier()
            };
            return InternalPublish(cmd);
        }

        [HttpGet("{id}/publish")]
        [Authorize("publish_casefile")]
        public Task<IActionResult> Publish(string id)
        {
            var cmd = new PublishCaseFileCommand
            {
                Id = id,
                BypassUserValidation = true
            };
            return InternalPublish(cmd);
        }

        [HttpGet("search")]
        [Authorize("get_casefile")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _queryRepository.Find(ExtractFindParameter(query));
            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("me/{id}")]
        [Authorize("me_get_casefile")]
        public async Task<IActionResult> GetMe(string id)
        {
            var result = await _queryRepository.FindById(id);
            if (result == null)
            {
                return new NotFoundResult();
            }

            if (result.Owner != this.GetNameIdentifier())
            {
                return new UnauthorizedResult();
            }

            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("{id}")]
        [Authorize("get_casefile")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _queryRepository.FindById(id);
            if (result == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(ToDto(result));
        }

        private async Task<IActionResult> InternalAdd(AddCaseFileCommand parameter)
        {
            try
            {
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

        private async Task<IActionResult> InternalUpdate(UpdateCaseFileCommand parameter)
        {
            try
            {
                await _updateCaseFileCommandHandler.Handle(parameter);
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

        private async Task<IActionResult> InternalPublish(PublishCaseFileCommand publishCaseFileCommand)
        {
            try
            {
                var newCaseFileId = await _publishCaseFileCommandHandler.Handle(publishCaseFileCommand);
                return new OkObjectResult(new JObject
                {
                    { "id", newCaseFileId }
                });
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
                { "update_datetime", resp.UpdateDateTime },
                { "version", resp.Version },
                { "file_id", resp.FileId },
                { "owner", resp.Owner },
                { "status", Enum.GetName(typeof(CaseFileStatus), resp.Status).ToLowerInvariant() }
            };
        }

        private static FindCaseFilesParameter ExtractFindParameter(IEnumerable<KeyValuePair<string, string>> query)
        {
            string owner;
            string text;
            bool takeLatest = false;
            string caseFileId;
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

            if (query.TryGet("take_latest", out takeLatest))
            {
                parameter.TakeLatest = takeLatest;
            }

            if (query.TryGet("case_file_id", out caseFileId))
            {
                parameter.CaseFileId = caseFileId;
            }

            return parameter;
        }
    }
}