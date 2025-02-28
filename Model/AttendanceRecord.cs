using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
    public class AttendanceRecord
    {
        public int EmployeeID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan CheckIn1 { get; set; }
        public TimeSpan CheckOut1 { get; set; }
        public TimeSpan CheckIn2 { get; set; }
        public TimeSpan CheckOut2 { get; set; }
        public TimeSpan CheckIn3 { get; set; }
        public TimeSpan CheckOut3 { get; set; }
    }

    class DailyAttendance
    {
        public DateTime Date { get; set; }
        public List<TimeSpan> CheckIn1s { get; set; }
        public List<TimeSpan> CheckOut1s { get; set; }
        public List<TimeSpan> CheckIn2s { get; set; }
        public List<TimeSpan> CheckOut2s { get; set; }
        public List<TimeSpan> CheckIn3s { get; set; }
        public List<TimeSpan> CheckOut3s { get; set; }
    }

    public class TimeStats
    {
        public TimeSpan Average { get; set; }
        public TimeSpan StandardDeviation { get; set; }
    }
}
