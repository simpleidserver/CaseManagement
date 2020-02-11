using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures
{
    public class PerformanceMonitoringJob : IJob
    {
        private readonly IPerformanceCommandRepository _performanceCommandRepository;
        private readonly CMMNServerOptions _options;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _currentTask;

        public string QueueName => "performance";

        public PerformanceMonitoringJob(IPerformanceCommandRepository performanceCommandRepository, IOptions<CMMNServerOptions> options)
        {
            _performanceCommandRepository = performanceCommandRepository;
            _options = options.Value;
        }

        public Task Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _currentTask = new Task(Handle, TaskCreationOptions.LongRunning);
            _currentTask.Start();
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            _cancellationTokenSource.Cancel();
            _currentTask.Wait();
            return Task.CompletedTask;
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

                var performanceStatistic = new PerformanceAggregate
                {
                    CaptureDateTime = DateTime.UtcNow,
                    MachineName = Environment.MachineName,
                    NbWorkingThreads = GetWorkingThreads(),
                    MemoryConsumedMB = GetConsumedMemoryMB()
                };
                _performanceCommandRepository.Add(performanceStatistic);
                await _performanceCommandRepository.SaveChanges();
                await _performanceCommandRepository.KeepLastRecords(_options.MaxNbPerformanceRecords, Environment.MachineName);
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