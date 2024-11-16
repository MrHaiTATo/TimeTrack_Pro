using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
    public class SummaryData : Employee
    {
        //标准出勤
        public string? StdAtd { get; set; }
        //实际出勤
        public string? AtlAtd { get; set; }
        //事假
        public string? MtrVct { get; set; }
        //病假
        public string? SkeVct { get; set; }
        //旷工
        public string? Absentee { get; set; }
        //出差
        public string? Errand { get; set; }
        //标准工作时间
        public string? SadWorkTime { get; set; }
        //实际工作时间
        public string? AtlWorkTime { get; set; }
        //加项工资-标准
        public string? AddWages_Std { get; set; }
        //加项工资-加班
        public string? AddWages_WorkOt { get; set; }
        //加项工资-津贴
        public string? AddWages_Sbd { get; set; }
        //减项工资-迟早
        public string? SubWages_LateEarly { get; set; }
        //减项工资-事假
        public string? SubWages_MtrVct { get; set; }
        //减项工资-扣款
        public string? SubWages_CutPay { get; set; }
        //实际工资
        public string? AtlPay { get; set; }
        //备注
        public string? Notes { get; set; }
    }
}
