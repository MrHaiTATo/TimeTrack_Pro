using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrack_Pro.Model.Base;

namespace TimeTrack_Pro.Model
{
    public class BakUseData : Employee
    {
        public int Number { get; set; }
        public int Index { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
