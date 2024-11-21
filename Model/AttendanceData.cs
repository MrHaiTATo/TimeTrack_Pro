using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
    public class AttendanceData
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 打卡时间
        /// </summary>
        public DateTime ClockTime { get; set; }
        /// <summary>
        /// 员工标识（与BakUseData.Index对应）
        /// </summary>
        public int UserIndex { get; set; }
        /// <summary>
        /// 班次（考勤规则，与AttendanceRule.SerialNumber对应）
        /// </summary>
        public int Class { get; set; }
        /// <summary>
        /// 班段
        /// </summary>
        public ShiftClass ShiftClass { get; set; }
        /// <summary>
        /// 签到方式
        /// </summary>
        public ClockMethod ClockMethod { get; set; }
        /// <summary>
        /// 签到状态
        /// </summary>
        public ClockState ClockState { get; set; }
    }
}
