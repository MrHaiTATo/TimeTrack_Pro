using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
    public class Sum_Stati_transit : Employee
    {
        /// <summary>
        /// 标准出勤
        /// </summary>
        public string? StdAtd { get; set; }
        /// <summary>
        /// 实际出勤
        /// </summary>
        public string? AtlAtd { get; set; }
        /// <summary>
        /// 标准工作时间
        /// </summary>
        public string? StdWorkTime { get; set; }
        /// <summary>
        /// 实际工作时间
        /// </summary>
        public string? AtlWorkTime { get; set; }
        /// <summary>
        /// 加班-普通
        /// </summary>
        public string? Wko_Common { get; set; }
        /// <summary>
        /// 加班-特殊
        /// </summary>
        public string? Wko_Special { get; set; }
        /// <summary>
        /// 迟到/早退-次
        /// </summary>
        public string? LateEarly_Count { get; set; }
        /// <summary>
        /// 迟到/早退-分
        /// </summary>
        public string? LateEarly_Min { get; set; }
        /// <summary>
        /// 旷工
        /// </summary>
        public string? Absentee { get; set; }
        /// <summary>
        /// 出差
        /// </summary>
        public string? Errand { get; set; }

        public Sum_Stati_transit GetSumStati()
        {
            return this;
        }
    }
}
