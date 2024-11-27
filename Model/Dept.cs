using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrack_Pro.Model.Base;

namespace TimeTrack_Pro.Model
{
    public class Dept
    {
        public string? Name { get; set; }
        public List<EmployeeBase>? Employees { get; set; }

        public Dept(string name = null, List<EmployeeBase> employees = null)
        {
            this.Name = name;
            this.Employees = employees;
        }
        
    }
}
