using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
    public class ExceptionSheetModel
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string? Date { get; set; }
        public List<ExceptionData> Datas { get; set; }
    }
}
