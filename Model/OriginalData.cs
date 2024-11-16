using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
    public class OriginalData : Employee
    {        
        public List<TimeSpan>[]? Datas { get; set; }
    }
}
