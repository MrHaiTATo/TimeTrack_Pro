using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model.Base
{
    public class TimeSlotBase
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public TimeSpan StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public TimeSpan EndTime { get; set; }
        /// <summary>
        /// 时间差
        /// </summary>
        public TimeSpan TimeDiffer 
        {
            get {
                return EndTime - StartTime;
            }
        }
    }
}
