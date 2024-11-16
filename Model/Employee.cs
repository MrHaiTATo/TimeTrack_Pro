using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrack_Pro.Model.Base;

namespace TimeTrack_Pro.Model
{

    //定义员工实体
    public class Employee : EmployeeBase
    {
        public string? Department { get; set; }
        public string? RuleName { get; set; }                
    }

}
