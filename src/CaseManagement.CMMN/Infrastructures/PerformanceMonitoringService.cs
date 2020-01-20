using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Persistence;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures
{
    public class PerformanceMonitoringService : IMessageConsumer
    {
        private readonly ILogger _logger;
        private readonly IStatisticCommandRepository _statisticCommandRepository;
        private readonly CMMNServerOptions _options;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _currentTask;

        public string QueueName => "performance";

        public PerformanceMonitoringService(ILogger<PerformanceMonitoringService> logger, IStatisticCommandRepository statisticCommandRepository, IOptions<CMMNServerOptions> options)
        {
            _logger = logger;
            _statisticCommandRepository = statisticCommandRepository;
            _options = options.Value;
        }

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _currentTask = new Task(Handle, TaskCreationOptions.LongRunning);
            _currentTask.Start();
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _currentTask.Wait();
        }

        public virtual async void Handle()
        {
            DateTime currentDateTime = DateTime.UtcNow;
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                Thread.Sleep(_options.BlockThreadMS);
                var diff = (DateTime.UtcNow - currentDateTime).TotalSeconds;
                if (diff < _options.PerformanceIntervalInSeconds)
                {
                    continue;
                }

                var performanceStatistic = new PerformanceStatisticAggregate
                {
                    CaptureDateTime = DateTime.UtcNow,
                    MachineName = Environment.MachineName,
                    NbWorkingThreads = GetWorkingThreads(),
                    MemoryConsumedMB = GetConsumedMemoryMB()
                };
                _statisticCommandRepository.Add(performanceStatistic);
                await _statisticCommandRepository.SaveChanges();
                await _statisticCommandRepository.KeepLastRecords(_options.MaxNbPerformanceRecords, Environment.MachineName);
                currentDateTime = DateTime.UtcNow;
            }
        }

        private static int GetWorkingThreads()
        {
            int maxThreads;
            int completionPortThreads;
            ThreadPool.GetMaxThreads(out maxThreads, out completionPortThreads);
            int availableThreads;
            ThreadPool.GetAvailableThreads(out availableThreads, out completionPortThreads);
            return maxThreads - availableThreads;
        }

        private static double GetConsumedMemoryMB()
        {
            return System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 * Math.Pow(10, -6);
        }
    }
}