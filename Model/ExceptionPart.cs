using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
    public class ExceptionPart
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string? Date { get; set; }
        /// <summary>
        /// 异常签到数据
        /// </summary>
        public string?[] ESignUpDatas { get; set; }
        /// <summary>
        /// 迟到/早退
        /// </summary>
        public string? LateOrEarly { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Notes { get; set; }

        public ExceptionPart()
        {
            Init();
        }

        private void Init()
        {
            ESignUpDatas = new string[6];
        }
    }
}
