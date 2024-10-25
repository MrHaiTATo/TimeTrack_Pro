using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
    public class AttendanceData
    {
        public int Number { get; set; }
        public DateTime ClockTime { get; set; }
        public int UserIndex { get; set; }
        public int Class { get; set; }
        public ShiftClass ShiftClass { get; set; }
        public ClockMethod ClockMethod { get; set; }
        public ClockState ClockState { get; set; }
    }
}
