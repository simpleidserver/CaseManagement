using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Form.Exceptions;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Form
{
    public class FormService : IFormService
    {
        private readonly IFormQueryRepository _formQueryRepository;

        public FormService(IFormQueryRepository formQueryRepository)
        {
            _formQueryRepository = formQueryRepository;
        }
        
        public async Task<JObject> Get(string id)
        {
            var result = await _formQueryRepository.FindFormById(id);
            if (result == null)
            {
                throw new UnknownFormException(id);
            }

            return ToDto(result);
        }

        public Task<FormAggregate> GetForm(string id)
        {
            return _formQueryRepository.FindFormById(id);
        }

        public async Task<JObject> Search(IEnumerable<KeyValuePair<string, string>> query)
        {
            var result = await _formQueryRepository.Find(ExtractFindParameter(query));
            return ToDto(result);
        }

        public async Task<JObject> Get(string formId, int version)
        {
            var aggregateId = FormAggregate.BuildIdentifier(formId, version);
            var result = await _formQueryRepository.FindFormById(aggregateId);
            if (result == null)
            {
                throw new UnknownFormException(aggregateId);
            }

            return ToDto(result);
        }

        private static JObject ToDto(FindResponse<FormAggregate> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => ToDto(r))) }
            };
        }

        private static JObject ToDto(FormAggregate form)
        {
            var result = new JObject
            {
                { "id", form.Id },
                { "version", form.Version },
                { "status", Enum.GetName(typeof(FormStatus), form.Status).ToLowerInvariant() },
                { "elements",  new JArray(form.Elements.Select(e => ToDto(e))) },
                { "create_datetime", form.CreateDateTime },
                { "update_datetime", form.UpdateDateTime }
            };
            foreach (var title in form.Titles)
            {
                result.Add($"title#{title.Language}", title.Value);
            }

            return result;
        }

        private static JObject ToDto(FormElement formElt)
        {
            var result = new JObject
            {
                { "id", formElt.Id },
                { "is_required", formElt.IsRequired },
                { "type", Enum.GetName(typeof(FormElementTypes), formElt.Type).ToLowerInvariant() }
            };
            foreach (var name in formElt.Names)
            {
                result.Add($"title#{name.Language}", name.Value);
            }

            foreach (var description in formElt.Descriptions)
            {
                result.Add($"description#{description.Language}", description.Value);
            }

            return result;
        }

        private static FindFormParameter ExtractFindParameter(IEnumerable<KeyValuePair<string, string>> query)
        {
            var parameter = new FindFormParameter();
            parameter.ExtractFindParameter(query);
            IEnumerable<string> ids;
            if (query.TryGet("id", out ids))
            {
                parameter.Ids = ids;
            }

            return parameter;
        }
    }
}
