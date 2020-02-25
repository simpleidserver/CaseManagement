using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.CaseFile;
using CaseManagement.CMMN.CaseFile.Commands;
using CaseManagement.CMMN.CaseFile.Exceptions;
using CaseManagement.CMMN.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Apis
{
    [Route(CMMNConstants.RouteNames.CaseFiles)]
    public class CaseFilesController : Controller
    {
        private readonly ICaseFileService _caseFileService;

        public CaseFilesController(ICaseFileService caseFileService)
        {
            _caseFileService = caseFileService;
        }

        [HttpGet("count")]
        [Authorize("get_statistic")]
        public async Task<IActionResult> Count()
        {
            var result = await _caseFileService.Count();
            return new OkObjectResult(result);
        }

        [HttpPost("me")]
        [Authorize("me_add_casefile")]
        public async Task<IActionResult> AddMe([FromBody] AddCaseFileCommand parameter)
        {
            try
            {
                parameter.Owner = this.GetNameIdentifier();
                var result = await _caseFileService.Add(parameter);
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    Content = result.ToString()
                };
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPost]
        [Authorize("add_casefile")]
        public async Task<IActionResult> Add([FromBody] AddCaseFileCommand parameter)
        {
            try
            {
                var result = await _caseFileService.Add(parameter);
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    Content = result.ToString()
                };
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPut("me/{id}")]
        [Authorize("me_update_casefile")]
        public async Task<IActionResult> UpdateMe(string id, [FromBody] UpdateCaseFileCommand parameter)
        {
            try
            {
                parameter.Id = id;
                parameter.Performer = this.GetNameIdentifier();
                await _caseFileService.UpdateMe(parameter);
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

        [HttpPut("{id}")]
        [Authorize("update_casefile")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateCaseFileCommand parameter)
        {
            try
            {
                parameter.Id = id;
                await _caseFileService.Update(parameter);
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

        [HttpGet("me/{id}/publish")]
        [Authorize("me_publish_casefile")]
        public async Task<IActionResult> PublishMe(string id)
        {
            try
            {
                var cmd = new PublishCaseFileCommand
                {
                    Id = id,
                    Performer = this.GetNameIdentifier()
                };
                var result = await _caseFileService.PublishMe(cmd);
                return new OkObjectResult(result);
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

        [HttpGet("{id}/publish")]
        [Authorize("publish_casefile")]
        public async Task<IActionResult> Publish(string id)
        {
            try
            {
                var cmd = new PublishCaseFileCommand
                {
                    Id = id
                };
                var result = await _caseFileService.Publish(cmd);
                return new OkObjectResult(result);
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
        [Authorize("get_casefile")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _caseFileService.Search(query);
            return new OkObjectResult(result);
        }

        [HttpGet("me/{id}")]
        [Authorize("me_get_casefile")]
        public async Task<IActionResult> GetMe(string id)
        {
            try
            {
                var result = await _caseFileService.GetMe(id, this.GetNameIdentifier());
                return new OkObjectResult(result);
            }
            catch(UnknownCaseFileException)
            {
                return new NotFoundResult();
            }
            catch(UnauthorizedCaseFileException)
            {
                return new UnauthorizedResult();
            }
        }

        [HttpGet("{id}")]
        [Authorize("get_casefile")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await _caseFileService.Get(id);
                return new OkObjectResult(result);
            }
            catch (UnknownCaseFileException)
            {
                return new NotFoundResult();
            }
            catch (UnauthorizedCaseFileException)
            {
                return new UnauthorizedResult();
            }
        }
    }
}