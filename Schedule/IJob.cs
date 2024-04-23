using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule
{
    public  interface IJob : IEquatable<IJob>, IEqualityComparer<IJob>
    {
        string LockName { get; set; }
        string JobId { get; set; }
        string JobName { get; set; }
        DateTime LastExecuted { get; set; }
        int MonthDay { get; set; }
        int WeekDay { get; set; }
        TimeSpan AtTime { get; set; }
        TimeSpan Interval { get; set; }
        bool Executing { get; set; }
        void Execute();
    }
}
