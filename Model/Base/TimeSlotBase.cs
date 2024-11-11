using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model.Base
{
    public class TimeSlotBase
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan TimeDiffer 
        {
            get {
                return EndTime - StartTime;
            }
        }
    }
}
