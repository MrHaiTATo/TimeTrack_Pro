using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
    public class OriginalData
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string RuleName { get; set; }
        public List<TimeSpan>[] Datas { get; set; }
    }
}
