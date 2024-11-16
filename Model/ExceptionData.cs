using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
    public class ExceptionData : Employee
    {
        //日期
        public string? Date { get; set; }
        //班段1-上班
        public string? Class1_0 { get; set; }
        //班段1-下班
        public string? Class1_1 { get; set; }
        //班段2-上班
        public string? Class2_0 { get; set; }
        //班段2-下班
        public string? Class2_1 { get; set; }
        //班段3-上班
        public string? Class3_0 { get; set; }
        //班段3-下班
        public string? Class3_1 { get; set; }
        //迟到/早退
        public string? LateOrEarly { get; set; }
        //备注
        public string? Notes { get; set; }
       
    }
}
