using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
    public class ExceptionData : Employee
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string? Date { get; set; }
        /// <summary>
        /// 班段1-上班
        /// </summary>
        public string? Class1_0 { get; set; }
        /// <summary>
        /// 班段1-下班
        /// </summary>
        public string? Class1_1 { get; set; }
        /// <summary>
        /// 班段2-上班
        /// </summary>
        public string? Class2_0 { get; set; }
        /// <summary>
        /// 班段2-下班
        /// </summary>
        public string? Class2_1 { get; set; }
        /// <summary>
        /// 班段3-上班
        /// </summary>
        public string? Class3_0 { get; set; }
        /// <summary>
        /// 班段3-下班
        /// </summary>
        public string? Class3_1 { get; set; }
        /// <summary>
        /// 迟到/早退
        /// </summary>
        public string? LateOrEarly { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Notes { get; set; }
       
    }
}
