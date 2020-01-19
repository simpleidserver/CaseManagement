using CaseManagement.CMMN.CaseInstance.Repositories;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors.CaseFileItem
{
    public class DirectoryCaseFileItemListener : ICaseFileItemListener
    {
        private readonly ICaseFileItemRepository _caseFileItemRepository;

        public DirectoryCaseFileItemListener(ICaseFileItemRepository caseFileItemRepository)
        {
            _caseFileItemRepository = caseFileItemRepository;
        }

        public const string CASE_FILE_ITEM_TYPE = "https://github.com/simpleidserver/casemanagement/directory";
        public string CaseFileItemType => CASE_FILE_ITEM_TYPE;

        public Task Start(ProcessorParameter parameter, CancellationToken token)
        {
            return BuildTask(parameter, token);
        }

        private Task BuildTask(ProcessorParameter processorParameter, CancellationToken token)
        {
            var task = new Task(async () => await HandleTask(processorParameter, token), token, TaskCreationOptions.LongRunning);
            task.Start();
            return task;
        }

        private async Task HandleTask(ProcessorParameter processorParameter, CancellationToken token)
        {
            bool isNewDirectory = false;
            var result = await _caseFileItemRepository.FindByCaseElementInstance(processorParameter.CaseElementInstance.Id);
            string tmpDirectory;
            if (result == null)
            {
                isNewDirectory = true;
                tmpDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                Directory.CreateDirectory(tmpDirectory);
            }
            else
            {
                tmpDirectory = result.Value;
            }

            var fileSystemWatcher = new FileSystemWatcher
            {
                Path = tmpDirectory
            };
            try
            {
                var cancellationTokenSource = new CancellationTokenSource();
                fileSystemWatcher.Created += (s, e) => HandleFileCreated(processorParameter);
                fileSystemWatcher.EnableRaisingEvents = true;
                if (isNewDirectory)
                {
                    await _caseFileItemRepository.AddCaseFileItem(processorParameter.CaseInstance.Id, processorParameter.CaseElementInstance.Id, processorParameter.CaseElementInstance.CaseElementDefinitionId, tmpDirectory);
                }

                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    Thread.Sleep(CMMNConstants.WAIT_INTERVAL_MS);
                    try
                    {
                        token.ThrowIfCancellationRequested();
                    }
                    catch
                    {
                        cancellationTokenSource.Cancel();
                    }
                }
            }
            finally
            {
                fileSystemWatcher.EnableRaisingEvents = false;
                fileSystemWatcher.Dispose();
            }
        }

        private void HandleFileCreated(ProcessorParameter parameter)
        {
            parameter.CaseInstance.MakeTransitionAddChild(parameter.CaseElementInstance.Id);
        }
    }
}
