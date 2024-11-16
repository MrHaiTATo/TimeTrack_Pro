using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
    public class StatisticsData : Employee
    {
        //日期
        public string? Date { get; set; }
        //标准出勤
        public string? StdAtd { get; set; }
        //实际出勤
        public string? AtlAtd { get; set; }
        //标准工作时间
        public string? SadWorkTime { get; set; }
        //实际工作时间
        public string? AtlWorkTime { get; set; }
        //加班-普通
        public string? Wko_Common { get; set; }
        //加班-特殊
        public string? Wko_Special { get; set; }
        //迟到/早退-次
        public string? LateEarly_Count { get; set; }
        //迟到/早退-分
        public string? LateEarly_Min { get; set; }
        //打卡记录
        public List<string?[]> SignUpDatas { get; set; }

        public StatisticsData()
        {
            SignUpDatas = new List<string?[]>(31);
            for (int i = 0; i < SignUpDatas.Capacity; i++)
            {
                SignUpDatas.Add(new string[8]);
            }
        }
    }
}
