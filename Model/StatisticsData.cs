using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
    public class StatisticsData : Sum_Stati_transit
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string? Date { get; set; }               
        /// <summary>
        /// 日期和星期
        /// </summary>
        public string?[] DaysOfWeek { set; get; }
        /// <summary>
        /// 打卡记录
        /// </summary>
        public List<SheetCell[]> SignUpDatas { get; set; }

        public StatisticsData()
        {
            SignUpDatas = new List<SheetCell[]>(32);
            for (int i = 0; i < SignUpDatas.Capacity; i++)
            {

                SignUpDatas.Add(new SheetCell[8]);
            }
        }
    }
}
