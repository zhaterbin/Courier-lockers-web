using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule
{
    public abstract class BaseJob :IJob
    {
        protected BaseJob()
        {
            LastExecuted = DateTime.Now;
            Interval = TimeSpan.Zero;
            AtTime = TimeSpan.Zero;
            MonthDay = 0;
            WeekDay = -1;
            Executing = false;
        }

        #region IJob 成员

        public string JobId { get; set; }

        public string JobName { get; set; }

        public DateTime LastExecuted { get; set; }

        public int MonthDay { get; set; }

        public int WeekDay { get; set; }

        public TimeSpan AtTime { get; set; }

        public TimeSpan Interval { get; set; }

        public bool Executing { get; set; }

        // public IBafLogger JobLogger { get; set; }
        public virtual string LockName { get; set; } = "#";

        public abstract void Execute();

        public bool Equals(IJob x, IJob y)
        {
            return x?.JobId == y?.JobId;
        }

        public int GetHashCode(IJob obj)
        {
            return JobId.GetHashCode();
        }

        public bool Equals(IJob other)
        {
            return other?.JobId == JobId;
        }
        #endregion
    }
}
