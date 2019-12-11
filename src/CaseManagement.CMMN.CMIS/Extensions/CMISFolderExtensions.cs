using PortCMIS.Client;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CMIS.Extensions
{
    public static class CMISFolderExtensions
    {
        public static IFolder CreateCMMNFolder(this IFolder folder, string folderName)
        {
            var folderProperties = new Dictionary<string, object>
            {
                { "cmis:objectTypeId", "cmis:folder" },
                { "cmis:name",  folderName}
            };
            return folder.CreateFolder(folderProperties);
        }
    }
}
