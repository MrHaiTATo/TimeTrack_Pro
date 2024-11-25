using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
    public class SummaryData : Sum_Stati_transit
    {       
        /// <summary>
        /// 事假
        /// </summary>
        public string? MtrVct { get; set; }
        /// <summary>
        /// 病假
        /// </summary>
        public string? SkeVct { get; set; }       
        /// <summary>
        /// 加项工资-标准
        /// </summary>
        public string? AddWages_Std { get; set; }
        /// <summary>
        /// 加项工资-加班
        /// </summary>
        public string? AddWages_WorkOt { get; set; }
        /// <summary>
        /// 加项工资-津贴
        /// </summary>
        public string? AddWages_Sbd { get; set; }
        /// <summary>
        /// 减项工资-迟早
        /// </summary>
        public string? SubWages_LateEarly { get; set; }
        /// <summary>
        /// 减项工资-事假
        /// </summary>
        public string? SubWages_MtrVct { get; set; }
        /// <summary>
        /// 减项工资-扣款
        /// </summary>
        public string? SubWages_CutPay { get; set; }
        /// <summary>
        /// 实际工资
        /// </summary>
        public string? AtlPay { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Notes { get; set; }

        public SummaryData() { }
        public SummaryData(Employee employee) 
        {
            this.Id = employee.Id;
            this.Name = employee.Name;
            this.Department = employee.Department;
            this.RuleName = employee.RuleName;
        }
    }
}
