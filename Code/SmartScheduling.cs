using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrack_Pro.Model;

namespace TimeTrack_Pro.Code
{
    public class SmartScheduling
    {
        private int min = 0;
        private int max = 0;
        private int hours = 24;
        private int point = 10;
        private List<DateTime> datas = new List<DateTime>();
        private List<DateTime[]> splitPoint = new List<DateTime[]>();
        public SmartScheduling(OriginalSheetModel model) 
        {
            _load(model);
            _init_splitPoint();
        }

        private void _load(OriginalSheetModel model)
        {
            for (int i = 0; i < model.Datas.Count(); i++)
            {
                for (int j = 0; j < model.Datas[i].Datas.Count(); j++)
                {
                    foreach (var item in model.Datas[i].Datas[j])
                    {
                        datas.Add(item);
                    }
                }
            }
        }

        private void _init_splitPoint()
        {
            DateTime time = new DateTime(0,0,0);
            for (int h = 0; h < hours; h++)
            {

            }
        }
    }
}
