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
        /// <summary>
        /// 时间段名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 时间段类型：0 正常，1 加班
        /// </summary>
        public int Type { get; set; }
    }
}
