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
        /// <summary>
        /// 部门
        /// </summary>
        public string? Department { get; set; }
        /// <summary>
        /// 班次（考勤规则名）
        /// </summary>
        public string? RuleName { get; set; }

        public Employee() {  }        
    }

}
