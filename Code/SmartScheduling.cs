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
        private int year = 2000;
        private int month = 1;
        private int day = 1;
        private int min = 0;
        private int max = 0;
        private int hours = 24;
        private int point = 10;
        private List<DateTime> datas = new List<DateTime>();
        private List<DateTime>[] splitPoint = null;
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
            splitPoint = new List<DateTime>[24];
            DateTime time = new DateTime(year,month,day,0,0,0);
            int n = 60 / point;
            for (int h = 0; h < hours; h++)
            {
                splitPoint[h] = new List<DateTime>();
                for (int i = 0; i < n; i++)
                {
                    splitPoint[h].Add(time);
                    time = time.AddMinutes(point);
                }
            }
        }
    }
}
