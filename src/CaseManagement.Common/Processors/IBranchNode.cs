using System.Collections;
using System.Collections.Generic;

namespace CaseManagement.Common.Processors
{
    public interface IBranchNode
    {
        string Id { get; }
        ICollection<string> Incoming { get; }
        bool IsLeaf();
    }
}
