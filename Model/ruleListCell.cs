using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
    public class ruleListCell
    {
        public MText Start { set; get; } = new MText();
        public MText End { get; set; } = new MText();
        public string Mode { get; set; } = "正常";
    }
}
