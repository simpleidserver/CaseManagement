﻿using CaseManagement.CMMN.CaseFile.CommandHandlers;
using CaseManagement.CMMN.CaseFile.Commands;
using CaseManagement.CMMN.CaseFile.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile
{
    public class CaseFileService : ICaseFileService
    {
        private readonly ICaseFileQueryRepository _queryRepository;
        private readonly IAddCaseFileCommandHandler _addCaseFileCommandHandler;
        private readonly IUpdateCaseFileCommandHandler _updateCaseFileCommandHandler;
        private readonly IPublishCaseFileCommandHandler _publishCaseFileCommandHandler;

        public CaseFileService(ICaseFileQueryRepository queryRepository, IAddCaseFileCommandHandler addCaseFileCommandHandler, IUpdateCaseFileCommandHandler updateCaseFileCommandHandler, IPublishCaseFileCommandHandler publishCaseFileCommandHandler)
        {
            _queryRepository = queryRepository;
            _addCaseFileCommandHandler = addCaseFileCommandHandler;
            _updateCaseFileCommandHandler = updateCaseFileCommandHandler;
            _publishCaseFileCommandHandler = publishCaseFileCommandHandler;
        }

        public async Task<JObject> Count()
        {
            var result = await _queryRepository.Count();
            return new JObject
            {
                { "count", result }
            };
        }

        public async Task<JObject> Add(AddCaseFileCommand parameter)
        {
            var result = await _addCaseFileCommandHandler.Handle(parameter);
            return new JObject
            {
                { "id", result }
            };
        }

        public Task<bool> UpdateMe(UpdateCaseFileCommand parameter)
        {
            parameter.BypassUserValidation = false;
            return _updateCaseFileCommandHandler.Handle(parameter);
        }

        public Task<bool> Update(UpdateCaseFileCommand parameter)
        {
            parameter.BypassUserValidation = true;
            return _updateCaseFileCommandHandler.Handle(parameter);
        }

        public async Task<JObject> PublishMe(PublishCaseFileCommand parameter)
        {
            parameter.BypassUserValidation = false;
            var result = await _publishCaseFileCommandHandler.Handle(parameter);
            return new JObject
            {
                { "id", result }
            };
        }

        public async Task<JObject> Publish(PublishCaseFileCommand parameter)
        {
            parameter.BypassUserValidation = true;
            var result = await _publishCaseFileCommandHandler.Handle(parameter);
            return new JObject
            {
                { "id", result }
            };
        }

        public async Task<JObject> GetMe(string id, string nameIdentifier)
        {
            var result = await _queryRepository.FindById(id);
            if (result == null)
            {
                throw new UnknownCaseFileException(id);
            }

            if (result.Owner != nameIdentifier)
            {
                throw new UnauthorizedCaseFileException(nameIdentifier, id);
            }

            return ToDto(result);
        }

        public async Task<JObject> Get(string id)
        {
            var result = await _queryRepository.FindById(id);
            if (result == null)
            {
                throw new UnknownCaseFileException(id);
            }

            return ToDto(result);
        }

        public async Task<JObject> Search(IEnumerable<KeyValuePair<string, string>> query)
        {
            var result = await _queryRepository.Find(ExtractFindParameter(query));
            return ToDto(result);
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