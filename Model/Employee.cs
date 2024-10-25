using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
    //定义员工实体
    public class Employee
    {
        public int Number { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public DateTime CreatedTime { get; set; }
        public List<ShiftPreference> Preferences { get; set; }
    }
}
