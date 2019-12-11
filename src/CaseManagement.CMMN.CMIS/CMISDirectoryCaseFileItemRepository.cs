using CaseManagement.CMMN.CaseInstance.Repositories;
using CaseManagement.CMMN.CMIS.Extensions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using Microsoft.Extensions.Options;
using PortCMIS.Client;
using PortCMIS.Enums;
using PortCMIS.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CMIS
{
    public class CMISDirectoryCaseFileItemRepository : ICaseFileItemRepository
    {
        private readonly ICMISSessionFactory _cmisSessionFactory;
        private readonly CMISOptions _cmisOptions;
        private bool _stop;
        private Task _task;
        private WorkflowHandlerContext _context;
        private CancellationToken _token;

        public const string CASE_FILE_ITEM_TYPE = "http://www.omg.org/spec/CMMN/DefinitionType/CMISFolder";

        public CMISDirectoryCaseFileItemRepository(ICMISSessionFactory cmisSessionFactory, IOptions<CMISOptions> options)
        {
            _cmisSessionFactory = cmisSessionFactory;
            _cmisOptions = options.Value;
        }

        public string CaseFileItemType => CASE_FILE_ITEM_TYPE;
        public Task Task => _task;

        public Task Start(WorkflowHandlerContext context, CancellationToken token)
        {
            _stop = false;
            _context = context;
            _token = token;
            var caseFileItem = context.CurrentElement as CMMNCaseFileItem;
            if (!caseFileItem.MetadataLst.Any())
            {
                var session = _cmisSessionFactory.GetSession();
                var rootFolder = session.GetRootFolder();
                var newFolder = rootFolder.CreateCMMNFolder($"cmmns-process-{context.ProcessFlowInstance.Id}");
                var pf = context.ProcessFlowInstance;
                var metadataLst = new Dictionary<string, string>
                {
                    { "directory", newFolder.Id }
                };
                pf.CreateCaseFileItem(_context.GetCMMNCaseFileItem(), metadataLst);
            }

            _task = new Task(Handle, token, TaskCreationOptions.LongRunning);
            _task.Start();
            return Task.CompletedTask;
        }

        public CaseFileItem Get(CMMNCaseFileItem caseFileItem)
        {
            throw new NotImplementedException();
        }

        private void Handle()
        {
            var previousFilesInfo = new List<FileInfo>();
            var session = _cmisSessionFactory.GetSession();
            var caseFileItem = _context.CurrentElement as CMMNCaseFileItem;
            var directoryId = caseFileItem.MetadataLst.First(m => m.Key == "directory").Value;
            var previousFolder = session.GetObject(directoryId) as IFolder;
            bool eventOccured = false;
            while (!_token.IsCancellationRequested)
            {
                eventOccured = false;
                if (_stop)
                {
                    break;
                }

                Task.Delay(_cmisOptions.WaitMs).Wait();
                var oc = new OperationContext
                {
                    IncludeRelationships = IncludeRelationships.Both
                };
                IFolder newFolder = null;
                try
                {
                    newFolder = session.GetObject(directoryId, oc) as IFolder;
                }
                catch (CmisObjectNotFoundException) { }

                var newFilesInfo = new List<FileInfo>();
                if (newFolder == null)
                {
                    eventOccured = true;
                    HandleRemoved();
                    _stop = true;
                }
                else
                {
                    if (IsDifferent(previousFolder, newFolder))
                    {
                        eventOccured = true;
                        HandleUpdate();
                    }

                    var newFolderNbRelationships = newFolder.Relationships == null ? 0 : newFolder.Relationships.Count();
                    var previousFolderNbRelationships = previousFolder.Relationships == null ? 0 : previousFolder.Relationships.Count();
                    if (newFolderNbRelationships < previousFolderNbRelationships)
                    {
                        eventOccured = true;
                        HandleRemoveReference();
                    }

                    if (newFolderNbRelationships > previousFolderNbRelationships)
                    {
                        eventOccured = true;
                        HandleAddReference();
                    }

                    GetFilesInfo(newFolder, string.Empty, newFilesInfo);
                    var createdItems = newFilesInfo.Where(nf => previousFilesInfo.All(pf => pf.Id != nf.Id));
                    var removedItems = previousFilesInfo.Where(pf => newFilesInfo.All(nf => nf.Id != pf.Id));
                    foreach (var createdItem in createdItems)
                    {
                        if (createdItem.FilePath == newFolder.Path)
                        {
                            continue;
                        }

                        eventOccured = true;
                        HandleChildAdded();
                    }

                    foreach (var removedItem in removedItems)
                    {
                        eventOccured = true;
                        HandleChildRemoved();
                    }
                }

                previousFilesInfo = newFilesInfo;
                previousFolder = newFolder;
                if (eventOccured)
                {
                    _context.ExecuteNext(_token).Wait();
                    if (_context.ProcessFlowInstance.NextElements(_context.CurrentElement.Id).All(s => s.Status == ProcessFlowInstanceElementStatus.Finished))
                    {
                        _stop = true;
                    }
                }
            }

            if (_token.IsCancellationRequested)
            {
                var pf = _context.ProcessFlowInstance;
                pf.CancelElement(_context.CurrentElement);
            }
        }

        private void HandleChildAdded()
        {
            Debug.WriteLine("Child added");
            var pf = _context.ProcessFlowInstance;
            pf.AddChild(_context.GetCMMNCaseFileItem());
        }

        private void HandleChildRemoved()
        {
            Debug.WriteLine("Child removed");
            var pf = _context.ProcessFlowInstance;
            pf.RemoveChild(_context.GetCMMNCaseFileItem());
        }

        private void HandleRemoved()
        {
            Debug.WriteLine("Directory removed");
            var pf = _context.ProcessFlowInstance;
            pf.Delete(_context.GetCMMNCaseFileItem());
        }

        private void HandleAddReference()
        {
            Debug.WriteLine("Reference added");
            var pf = _context.ProcessFlowInstance;
            pf.AddReference(_context.GetCMMNCaseFileItem());
        }

        private void HandleRemoveReference()
        {
            Debug.WriteLine("Reference removed");
            var pf = _context.ProcessFlowInstance;
            pf.RemoveReference(_context.GetCMMNCaseFileItem());
        }

        private void HandleUpdate()
        {
            Debug.WriteLine("Updated");
            var pf = _context.ProcessFlowInstance;
            pf.Update(_context.GetCMMNCaseFileItem());
        }

        private bool IsDifferent(IFolder previousFolder, IFolder newFolder)
        {
            foreach(var newProperty in newFolder.Properties)
            {
                var previousProperty = previousFolder.Properties.First(p => p.Id == newProperty.Id);
                if (previousProperty.ValueAsString != newProperty.ValueAsString)
                {
                    return true;
                }
            }

            return false;
        }

        private void GetFilesInfo(ICmisObject cmisObj, string basePath, List<FileInfo> filesInfo)
        {
            var cmisFolder = cmisObj as IFolder;
            if (cmisFolder != null)
            {
                foreach(var child in cmisFolder.GetChildren())
                {
                    GetFilesInfo(child, cmisFolder.Path, filesInfo);
                }
            }

            var filePath = $"{basePath}/{cmisObj.Name}";
            var updateDateTime = cmisObj.LastModificationDate;
            filesInfo.Add(new FileInfo(cmisObj.Id, filePath, updateDateTime));
        }

        private class FileInfo
        {
            public FileInfo(string id, string filePath, DateTime? updateDateTime = null)
            {
                Id = id;
                FilePath = filePath;
                UpdateDateTime = updateDateTime;
            }

            public string Id { get; set; }
            public string FilePath { get; set; }
            public DateTime? UpdateDateTime { get; set; }
        }
    }
}
