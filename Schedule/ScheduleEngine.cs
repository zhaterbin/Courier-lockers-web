using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule
{
    public  class ScheduleEngine : BackgroundService, IScheduleEngine
    {
        private static ScheduleEngine _instance;
        private static readonly object InstanceLock = new();
        private static ConcurrentDictionary<string, IJob> _jobDic = new();
        private readonly TimeSpan _interval;
        private readonly ILogger _logger;

        private CancellationTokenSource _tokenSource;

        public ScheduleEngine(IEnumerable<IJob> jobs, ILogger<IScheduleEngine> logger) : this()
        {
            _logger = logger;
            foreach (IJob job in jobs)
            {
                _jobDic.TryAdd(job.JobId, job);
            }
        }

        private ScheduleEngine()
        {
            _interval = TimeSpan.FromMilliseconds(20);
            IsRunning = false;
        }

        public static ScheduleEngine Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                lock (InstanceLock)
                {
                    return _instance ??= new ScheduleEngine();
                }
            }
        }

        public bool IsRunning { get; private set; }

        public virtual bool AddJob(IJob job)
        {
            if (job == null)
            {
                return false;
            }

            return _jobDic.AddOrUpdate(job.JobId, _ => job, (_, _) => job) != null;
        }

        public IJob GetJob(string id)
        {
            _ = _jobDic.TryGetValue(id, out IJob job);
            return job;
        }

        public virtual bool ExecuteJob(IJob job)
        {
            try
            {
                job.Executing = true;
                job.Execute();
                job.LastExecuted = DateTime.Now;
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                job.Executing = false;
            }
        }

        public virtual bool ExecuteJob(string jobId)
        {
            if (_jobDic.TryRemove(jobId, out IJob job))
            {
                return job != null && ExecuteJob(job);
            }

            return false;
        }

        public virtual bool RemoveJob(string jobId)
        {
            return _jobDic.TryRemove(jobId, out IJob _);
        }

        public override void Dispose()
        {
            Stop();
            _jobDic.Clear();
            _jobDic = null;
            base.Dispose();
        }

        public Task Start(CancellationToken? token = null)
        {
            if (_tokenSource is { IsCancellationRequested: false })
            {
                return Task.CompletedTask;
            }

            _logger?.LogDebug($"{GetType().Name}:Starting");
            _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(token ?? new CancellationToken());
            Task task = Task.Factory.StartNew(CheckJob, _tokenSource.Token, TaskCreationOptions.LongRunning);
            IsRunning = true;
            _logger?.LogInformation($"{GetType().Name}:Started");
            return task;
        }

        public Task Stop()
        {
            _logger?.LogDebug($"{GetType().Name}:Stopping");
            _tokenSource?.Cancel();
            while (_jobDic.All(f => !f.Value.Executing))
            {
                Task.Delay(1000);
            }

            IsRunning = false;
            _logger?.LogInformation($"{GetType().Name}:Stopped");
            return Task.CompletedTask;
        }

        private void CheckJob(object obj)
        {
            CancellationToken token = (CancellationToken)obj;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (_jobDic == null)
                    {
                        return;
                    }

                    List<string> lockName = _jobDic.Values
                        .Where(f => f.Executing && !string.IsNullOrEmpty(f.LockName) && f.LockName != "#")
                        .Select(f => f.LockName).Distinct().ToList();


                    List<IJob> canDoList = _jobDic.Values.Where(f => !f.Executing && !lockName.Contains(f.LockName))
                        .OrderBy(f => f.LastExecuted).ToList();

                    foreach (IJob job in canDoList)
                    {
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }

                        if (!string.IsNullOrWhiteSpace(job.LockName) && job.LockName != "#" &&
                            lockName.Contains(job.LockName))
                        {
                            continue;
                        }

                        if (!DoJob(job))
                        {
                            continue;
                        }

                        try
                        {
                            if (!string.IsNullOrWhiteSpace(job.LockName) && job.LockName != "#")
                            {
                                lockName.Add(job.LockName);
                            }

                            job.Executing = true;
                            job.LastExecuted = DateTime.Now;
                            IJob job1 = job;
                            Task.Run(() => Execute(job1), token);
                        }
                        catch
                        {
                            job.Executing = false;
                        }
                    }

                    canDoList.Clear();
                }
                finally
                {
                    Thread.Sleep(_interval);
                }
            }
        }

        private static bool DoJob(IJob job)
        {
            if (job == null)
            {
                return false;
            }

            if (job.Executing)
            {
                return false;
            }

            if (job.MonthDay > 0)
            {
                return DoMonthJob(job);
            }

            if (job.WeekDay != -1)
            {
                return DoWeekJob(job);
            }

            if (job.AtTime != TimeSpan.Zero)
            {
                return DoDayJob(job);
            }

            if (job.Interval != TimeSpan.Zero)
            {
                return DoIntervalJob(job);
            }

            return false;
        }

        private static bool DoIntervalJob(IJob job)
        {
            DateTime currentDate = DateTime.Now;
            TimeSpan ts = currentDate.Subtract(job.LastExecuted);
            return ts >= job.Interval;
            //  if (ts >= job.Interval)
            // RequestExecuteJob(job);
        }

        private static bool DoDayJob(IJob job)
        {
            DateTime currentDate = DateTime.Now;
            if (job.LastExecuted.Year < currentDate.Year)
            {
                return currentDate.TimeOfDay >= job.AtTime;
            }
            //if (currentDate.TimeOfDay >= job.AtTime)
            //    RequestExecuteJob(job);

            if (job.LastExecuted.Year == currentDate.Year)
            {
                if (job.LastExecuted.Month < currentDate.Month)
                {
                    return currentDate.TimeOfDay >= job.AtTime;
                }

                //if (currentDate.TimeOfDay >= job.AtTime)
                //    RequestExecuteJob(job);
                if (job.LastExecuted.Month == currentDate.Month)
                {
                    return job.LastExecuted.Day < currentDate.Day && currentDate.TimeOfDay >= job.AtTime;
                }

                //if (job.LastExecuted.Day >= currentDate.Day) return ;
                //if (currentDate.TimeOfDay >= job.AtTime)
                //    RequestExecuteJob(job);
            }

            return false;
        }

        private static bool DoWeekJob(IJob job)
        {
            DateTime currentDate = DateTime.Now;
            return job.WeekDay == (int)currentDate.DayOfWeek
                   && job.LastExecuted.DayOfYear < currentDate.DayOfYear
                   && (job.AtTime == TimeSpan.Zero || currentDate.TimeOfDay >= job.AtTime);


            //var currentDate = DateTime.Now;
            //if (job.WeekDay != (int)currentDate.DayOfWeek ||
            //    job.LastExecuted.DayOfYear >= currentDate.DayOfYear) return;
            //if (job.AtTime != TimeSpan.Zero)
            //{
            //    if (currentDate.TimeOfDay >= job.AtTime)
            //        RequestExecuteJob(job);
            //}
            //else
            //{
            //    RequestExecuteJob(job);
            //}
        }

        private static bool DoMonthJob(IJob job)
        {
            DateTime currentDate = DateTime.Now;
            return job.MonthDay == currentDate.Day
                   && job.LastExecuted.Month < currentDate.Month
                   && (job.AtTime == TimeSpan.Zero || currentDate.TimeOfDay >= job.AtTime);
        }

        private static void Execute(IJob job)
        {
            try
            {
                job.Execute();
            }
            catch //(Exception e)
            {
                // mLogger?.Error($"执行定时器工作发生异常:{e.Message}", e);
            }
            finally
            {
                job.Executing = false;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {


            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger?.LogDebug($"{GetType().Name}:Executing");
                //GC.Collect();
                await Task.Delay(60000, stoppingToken);
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Start();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Stop();
        }
    }
}
