﻿using MediatR;
using static CaseManagement.HumanTask.HumanTaskDef.Results.HumanTaskDefResult;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class AddHumanTaskDefDeadLineCommand : IRequest<string>
    {
        public string Id { get; set; }
        public HumanTaskDefinitionDeadLineResult DeadLine { get; set; }
    }
}
