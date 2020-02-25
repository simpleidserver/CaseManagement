using CaseManagement.CMMN.AspNet.Extensions;
using CaseManagement.CMMN.CaseFile;
using CaseManagement.CMMN.CaseFile.Commands;
using CaseManagement.CMMN.CaseFile.Exceptions;
using CaseManagement.CMMN.Infrastructures;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CaseManagement.CMMN.AspNet.Apis
{
    [RoutePrefix(CMMNConstants.RouteNames.CaseFiles)]
    public class CaseFilesController : ApiController
    {
        private readonly ICaseFileService _caseFileService;

        public CaseFilesController(ICaseFileService caseFileService)
        {
            _caseFileService = caseFileService;
        }

        [HttpGet]
        [Route("count")]
        public async Task<IHttpActionResult> Count()
        {
            var result = await _caseFileService.Count();
            return Ok(result);
        }

        [HttpPost]
        [Route("me")]
        public async Task<IHttpActionResult> AddMe([FromBody] AddCaseFileCommand parameter)
        {
            try
            {
                parameter.Owner = this.GetNameIdentifier();
                var result = await _caseFileService.Add(parameter);
                return Content(HttpStatusCode.Created, result);
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpPost]
        [Route]
        public async Task<IHttpActionResult> Add([FromBody] AddCaseFileCommand parameter)
        {
            try
            {
                var result = await _caseFileService.Add(parameter);
                return Content(HttpStatusCode.Created, result);
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpPut]
        [Route("me/{id:string}")]
        public async Task<IHttpActionResult> UpdateMe(string id, [FromBody] UpdateCaseFileCommand parameter)
        {
            try
            {
                parameter.Id = id;
                parameter.Performer = this.GetNameIdentifier();
                await _caseFileService.UpdateMe(parameter);
                return Ok();
            }
            catch (UnknownCaseFileException)
            {
                return NotFound();
            }
            catch (UnauthorizedCaseFileException)
            {
                return Unauthorized();
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpPut]
        [Route("{id:string}")]
        public async Task<IHttpActionResult> Update(string id, [FromBody] UpdateCaseFileCommand parameter)
        {
            try
            {
                parameter.Id = id;
                await _caseFileService.Update(parameter);
                return Ok();
            }
            catch (UnknownCaseFileException)
            {
                return NotFound();
            }
            catch (UnauthorizedCaseFileException)
            {
                return Unauthorized();
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("me/{id:string}/publish")]
        public async Task<IHttpActionResult> PublishMe(string id)
        {
            try
            {
                var cmd = new PublishCaseFileCommand
                {
                    Id = id,
                    Performer = this.GetNameIdentifier()
                };
                var result = await _caseFileService.PublishMe(cmd);
                return Ok(result);
            }
            catch (UnknownCaseFileException)
            {
                return NotFound();
            }
            catch (UnauthorizedCaseFileException)
            {
                return Unauthorized();
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("{id:string}/publish")]
        public async Task<IHttpActionResult> Publish(string id)
        {
            try
            {
                var cmd = new PublishCaseFileCommand
                {
                    Id = id
                };
                var result = await _caseFileService.Publish(cmd);
                return Ok(result);
            }
            catch (UnknownCaseFileException)
            {
                return NotFound();
            }
            catch (UnauthorizedCaseFileException)
            {
                return Unauthorized();
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpGet]
        [Route("search")]
        public async Task<IHttpActionResult> Search()
        {
            var query = this.Request.GetQueryNameValuePairs();
            var result = await _caseFileService.Search(query);
            return Ok(result);
        }


        [HttpGet]
        [Route("me/{id:string}")]
        public async Task<IHttpActionResult> GetMe(string id)
        {
            try
            {
                var result = await _caseFileService.GetMe(id, this.GetNameIdentifier());
                return Ok(result);
            }
            catch (UnknownCaseFileException)
            {
                return NotFound();
            }
            catch (UnauthorizedCaseFileException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("{id:string}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            try
            {
                var result = await _caseFileService.Get(id);
                return Ok(result);
            }
            catch (UnknownCaseFileException)
            {
                return NotFound();
            }
            catch (UnauthorizedCaseFileException)
            {
                return Unauthorized();
            }
        }
    }
}