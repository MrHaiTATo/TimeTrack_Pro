using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.ViewModel
{
    class AttendanceRuleViewModel : ViewModelBase
    {
        private ObservableCollection<string> attendanceWayDataList;
        public ObservableCollection<string> AttendanceWayDataList
        {
            get => attendanceWayDataList;
            set => Set(ref attendanceWayDataList, value);
        }

        private ObservableCollection<string> statsWayDataList;
        public ObservableCollection<string> StatsWayDataList
        {
            get => statsWayDataList;
            set => Set(ref statsWayDataList, value);
        }

        private ObservableCollection<string> shiftModeDataList;
        public ObservableCollection<string> ShiftModeDataList
        {
            get => shiftModeDataList;
            set => Set(ref shiftModeDataList, value);
        }

        private ObservableCollection<string> shiftDataList;
        public ObservableCollection<string> ShiftDataList
        {
            get => shiftDataList;
            set => Set(ref shiftDataList, value);
        }

        private TimeSpan time;
        public string TSpan 
        { 
            get => time.ToString();
            set {
                TimeSpan t;
                if(TimeSpan.TryParse(value, out t))
                    time = t;
            } 
        }

        public AttendanceRuleViewModel()
        {
            ShiftDataList = GetShiftDataList();
            AttendanceWayDataList = GetAttendanceWayDataList();
            StatsWayDataList = GetStatsWayDataList();
            ShiftModeDataList = GetShiftModeDataList();
        }

        private ObservableCollection<string> GetShiftDataList()
        {
            return new ObservableCollection<string>
            {
                "正常",
                "加班",                
            };
        }

        private ObservableCollection<string> GetAttendanceWayDataList()
        {
            return new ObservableCollection<string>
            {
                "连续考勤",
                "非连续考勤",
            };
        }

        private ObservableCollection<string> GetStatsWayDataList()
        {
            return new ObservableCollection<string>
            {
                "统计时间",
                "考勤时间",
            };
        }

        private ObservableCollection<string> GetShiftModeDataList()
        {
            return new ObservableCollection<string>
            {
                "1/2",
                "1/3",
            };
        }
    }
}
