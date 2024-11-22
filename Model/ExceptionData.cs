using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
    public class ExceptionData : Employee
    {        
        public List<ExceptionPart> Parts { get; set; }

        public ExceptionData()
        {
            Init();
        }

        public ExceptionData(Employee employee)
        {
            Init();
            this.Id = employee.Id;
            this.Name = employee.Name;
            this.Department = employee.Department;
            this.RuleName = employee.RuleName;
        }

        private void Init()
        {
            Parts = new List<ExceptionPart>();
        }
    }
}
