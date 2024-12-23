﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrack_Pro.Helper;

namespace TimeTrack_Pro.Model
{
    public class AttendanceRule
    {				
		/// <summary>
		/// 规则名称
		/// </summary>
		public string? RuleName { get; set; }
				
		/// <summary>
		/// 跨天时间
		/// </summary>
		public TimeSpan Inter_dayTime { get; set; }
        		
		/// <summary>
		/// 序号
		/// </summary>
		public int SerialNumber { get; set; }
        
		/// <summary>
		/// 闹铃次数
		/// </summary>
		public int AlarmsTimes { get; set; }

		/// <summary>
		/// 考勤方式
		/// </summary>
		public int AttendanceWay { get; set; }
       
		/// <summary>
		/// 允许误差
		/// </summary>
		public int StatsUnit { get; set; }

		/// <summary>
		/// 统计方式
		/// </summary>
		public int StatsWay { get; set; }
        
		/// <summary>
		/// 换班模式
		/// </summary>
		public int ShiftMode { get; set; }
        
		/// <summary>
		/// 允许迟到
		/// </summary>
		public int AllowLate { get; set; }

		/// <summary>
		/// 允许早退
		/// </summary>
		public int AllowEarly { get; set; }

		/// <summary>
		/// 上、下班标准
		/// </summary>
        public ClassSection[][]? Classes { get; set; }

		public string GetStdTimeStr(int year, int month)
		{							
			int stdH = 0, stdM = 0;
			if (Classes != null && Classes.Count() == 7)
			{
				for (int k = 1; k <= DateTimeHelper.GetDays(year, month); k++)
				{
					ClassSection[] cs = Classes[DateTimeHelper.GetWeek(year, month, k)];
					if (cs == null)
						continue;
                    foreach (var s in cs)
					{
						if (s.Type == 0 && s.StartTime != TimeSpan.Zero && s.EndTime != TimeSpan.Zero && s.StartTime < s.EndTime)
						{
							var time = s.TimeDiffer;
							stdH += time.Hours;
							stdM += time.Minutes;
						}
					}
				}
			}
			return string.Format("{0:00}:{1:00}", stdH + stdM / 60, stdM % 60);
        }

		public AttendanceRule Copy()
		{
			AttendanceRule rule = new AttendanceRule()
			{
				RuleName = this.RuleName,
				Inter_dayTime = this.Inter_dayTime,
				SerialNumber = this.SerialNumber,
				AlarmsTimes = this.AlarmsTimes,
				AttendanceWay = this.AttendanceWay,
				StatsUnit = this.StatsUnit,
				StatsWay = this.StatsWay,
				ShiftMode = this.ShiftMode,
				AllowLate = this.AllowLate,
				AllowEarly = this.AllowEarly,
				Classes = new ClassSection[7][]
			};
			if (Classes != null)
			{
				for (int i = 0; i < 7; i++)
				{
					rule.Classes[i] = new ClassSection[3];
					for (int j = 0; j < 3; j++)
					{
						rule.Classes[i][j] = new ClassSection();
						rule.Classes[i][j].Name = Classes[i][j].Name;
                        rule.Classes[i][j].StartTime = Classes[i][j].StartTime;
                        rule.Classes[i][j].EndTime = Classes[i][j].EndTime;
                    }
				}
			}
			return rule;
		}
    }
}
