using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
