using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule
{
    public interface IScheduleEngine
    {
        bool IsRunning { get; }
        bool AddJob(IJob job);
        bool RemoveJob(string jobId);
        IJob GetJob(string id);
        bool ExecuteJob(IJob job);
        bool ExecuteJob(string jobId);
    }
}
