using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
    public class OriginalSheetModel
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }       
        public List<OriginalData>? Datas { get; set; }
    }
}
