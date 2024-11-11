using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrack_Pro.Model.Base;

namespace TimeTrack_Pro.Model
{
    public class ClassSection : TimeSlotBase
    {
        public string? Name { get; set; }
        public int Type { get; set; }
    }
}
