namespace CaseManagement.CMMN.CaseInstance.Repositories
{
    /*
    public class DirectoryCaseFileItemRepository : ICaseFileItemRepository
    {
        private const string METADATA_NAME = "directory";
        private readonly IQueueProvider _queueProvider;
        private CancellationToken _token;
        private string _tempDirectory;
        private WorkflowHandlerContext _context;
        private bool _stop;

        public DirectoryCaseFileItemRepository(IQueueProvider queueProvider)
        {
            _queueProvider = queueProvider;
        }

        public Task Task { get; private set; }

        public string CaseFileItemType => CASE_FILE_ITEM_TYPE;
        public const string CASE_FILE_ITEM_TYPE = "https://github.com/simpleidserver/casemanagement/directory";

        public CaseFileItem Get(CMMNCaseFileItem caseFileItem)
        {
            return new DirectoryCaseFileItem(caseFileItem.MetadataLst.First(m => m.Key == METADATA_NAME).Value);
        }

        public Task Start(WorkflowHandlerContext context, CancellationToken token)
        {
            _stop = false;
            _context = context;
            var pf = _context.ProcessFlowInstance;
            var caseFileItem = context.CurrentElement as CMMNCaseFileItem;
            if (caseFileItem.MetadataLst.Any())
            {
                _tempDirectory = caseFileItem.MetadataLst.First(m => m.Key == METADATA_NAME).Value;
            }
            else
            {
                _tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                Directory.CreateDirectory(_tempDirectory);
                var metadataLst = new Dictionary<string, string>
                {
                    { METADATA_NAME, _tempDirectory }
                };
                pf.CreateCaseFileItem(_context.GetCMMNCaseFileItem(), metadataLst);
            }

            Task = new Task(Handle, token, TaskCreationOptions.LongRunning);
            Task.Start();
            _token = token;
            return Task.CompletedTask;
        }

        private void Handle()
        {
            var fileSystemWatcher = new FileSystemWatcher
            {
                Path = _tempDirectory
            };
            fileSystemWatcher.Created += HandleFileCreated;
            fileSystemWatcher.EnableRaisingEvents = true;
            while (!_token.IsCancellationRequested)
            {
                Thread.Sleep(1000);
                if (_stop)
                {
                    break;
                }
            }

            var pf = _context.ProcessFlowInstance;
            if (_token.IsCancellationRequested)
            {
                pf.CancelElement(_context.CurrentElement);
            }
            else
            {
            }

            fileSystemWatcher.EnableRaisingEvents = false;
            fileSystemWatcher.Dispose();
        }

        private void HandleFileCreated(object sender, FileSystemEventArgs e)
        {
            var caseFileItem = _context.CurrentElement as CMMNCaseFileItem;
            _context.ProcessFlowInstance.AddChild(caseFileItem);
            if (_context.ProcessFlowInstance.NextElements(_context.CurrentElement.Id).All(s => s.Status == ProcessFlowInstanceElementStatus.Finished))
            {
                _stop = true;
            }
        }
    }
    */
}