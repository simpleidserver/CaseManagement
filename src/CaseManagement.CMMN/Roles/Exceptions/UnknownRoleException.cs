using System;

namespace CaseManagement.CMMN.Roles.Exceptions
{
    public class UnknownRoleException : Exception
    {
        public UnknownRoleException(string role)
        {
            Role = role;
        }

        public string Role { get; set; }
    }
}
