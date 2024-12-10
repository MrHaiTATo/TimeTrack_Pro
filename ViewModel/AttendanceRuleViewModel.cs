using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TimeTrack_Pro.Code;
using TimeTrack_Pro.Model;

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

        private ObservableCollection<ruleListCell[]> ruleListCells;
        
        private TimeSpan time;
        public string TSpan 
        {
            get {
                if (time.Days > 0)
                    return string.Format("{0},{1:00}:{2:00}", time.Days, time.Hours, time.Minutes);
                else
                    return string.Format("{0:00}:{1:00}", time.Hours, time.Minutes);
            }
            set {
                TimeSpan t;
                if(TimeSpan.TryParse(value, out t))
                    time = t;
                else
                {
                    //App.Log.Debug("无法将输入的字符转化为对应的TimeSpan。");
                }
            } 
        }

        private AttendanceRule atdRule = null;
        
        public string RuleName
        {
            get => atdRule.RuleName;
            set => atdRule.RuleName = value;
        }

        public string CrossingTime
        {
            get
            {                
                return string.Format("{0:00}:{1:00}", time.Hours, time.Minutes);
            }
            set
            {
                int hour = 0, min = 0;
                //if()
            }
        }

        public AttendanceRuleViewModel()
        {
            ShiftDataList = GetShiftDataList();
            AttendanceWayDataList = GetAttendanceWayDataList();
            StatsWayDataList = GetStatsWayDataList();
            ShiftModeDataList = GetShiftModeDataList();
            atdRule = GetAtdRule();
            ruleListCells = GetRuleListCells();
        }

        private string getTimeSpan(TimeSpan time)
        {
            if (time.Days > 0)
                return string.Format("{0},{1:00}:{2:00}", time.Days, time.Hours, time.Minutes);
            else
                return string.Format("{0:00}:{1:00}", time.Hours, time.Minutes);
        }

        private void setTimeSpan(ref TimeSpan time,ref MText t)
        {
            int day = 0, hour = 0, min = 0;
            if(atdRule.Inter_dayTime.Ticks >= 6e8)
            {
                if (Regex.IsMatch(t.Text, @"^[0-9]{1},[0-9]{1,2}:[0-9]{1,2}$"))
                {
                    day = Convert.ToInt32(t.Text.Substring(0, 1));
                    if(day == 1)
                    {
                        string[] strings = t.Text.Split(',')[1].Split(':');
                        hour = Convert.ToInt32(strings[0]);
                        min = Convert.ToInt32(strings[1]);
                        time = new TimeSpan(day, hour, min, 0);                        
                    }                                        
                }
                t.Color = Color.Red;
                return;
            }
            if (Regex.IsMatch(t.Text, @"^[0-9]{1},[0-9]{1,2}:[0-9]{1,2}$"))
            {
                time = new TimeSpan();
            }
            else 
            {

            }
        }

        private ObservableCollection<ruleListCell[]> GetRuleListCells()
        {
            ObservableCollection<ruleListCell[]> rules = new ObservableCollection<ruleListCell[]>();
            ruleListCell[] cells = null;
            for (int i = 0; i < 7; i++)
            {
                cells = new ruleListCell[3];
                for (int j = 0; j < 3; j++)
                {
                    cells[j] = new ruleListCell();
                }
                rules.Add(cells);
            }
            return rules;
        }

        private AttendanceRule GetAtdRule()
        {
            return new AttendanceRule
            {
                RuleName = "",
                Inter_dayTime = new TimeSpan(0, 0, 0),
                SerialNumber = 0,
                AlarmsTimes = 0,
                AttendanceWay = 0,
                StatsUnit = 0,
                StatsWay = 0,
                ShiftMode = 0,
                AllowLate = 0,
                AllowEarly = 0,
                Classes = new ClassSection[][] { 
                    /*星期日*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 }
                    },
                    /*星期一*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 }
                    },
                    /*星期二*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 }
                    },
                    /*星期三*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 }
                    },
                    /*星期四*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 }
                    },
                    /*星期五*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 }
                    },
                    /*星期六*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 }
                    }
                }
            };
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
