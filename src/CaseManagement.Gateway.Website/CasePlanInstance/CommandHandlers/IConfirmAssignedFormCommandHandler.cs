﻿using CaseManagement.Gateway.Website.CasePlanInstance.Commands;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlanInstance.CommandHandlers
{
    public interface IConfirmAssignedFormCommandHandler
    {
        Task Handle(ConfirmAssignedFormCommand cmd);
    }
}
