using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;
using TimeTrack_Pro.Model;

namespace TimeTrack_Pro.Code
{
    public class Point
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
    }

    public class SmartScheduling
    {
        private int year = 2000;
        private int month = 1;
        private int day = 1;
        private int min = 0;
        private int max = 0;
        private int hours = 24;
        private int point = 10;
        private OriginalSheetModel? model = null;
        private List<DateTime>? datas = null;
        private List<Point>? splitPoint = null;
        Dictionary<(Point,Point), List<DateTime>>? ranges = null;

        public SmartScheduling(OriginalSheetModel model) 
        {
            this.model = model;
            _load();
            _init_splitPoint();
            _init_ranges();
        }

        private void _load()
        {
            datas = new List<DateTime>();
            for (int i = 0; i < model?.Datas?.Count(); i++)
            {
                for (int j = 0; j < model?.Datas?[i]?.Datas?.Count(); j++)
                {
                    datas.AddRange(from item in model?.Datas?[i]?.Datas?[j]
                                   select item);
                }
            }
        }

        private void _init_splitPoint()
        {
            splitPoint = new List<Point>();            
            int n = 60 / point;
            for (int h = 0; h < hours; h++)
            {                
                for (int i = 0; i < n; i++)
                {
                    splitPoint.Add(new Point() { Hour = h, Minute = i * 10 });                    
                }
            }
            splitPoint.Add(new Point() { Hour = 24, Minute = 0 });
        }

        private void _init_ranges()
        {                        
            ranges = new Dictionary<(Point,Point), List<DateTime>>();           
            for (int i = 0; i < splitPoint.Count()-1; i++)
            {
                ranges.Add((splitPoint[i], splitPoint[i+1]),                     
                    datas.Where(d => {
                        int time1 = splitPoint[i].Hour * 60 + splitPoint[i].Minute;
                        int time2 = splitPoint[i+1].Hour * 60 + splitPoint[i+1].Minute;
                        int time3 = d.Hour * 60 + d.Minute;
                        if (time3 >= time1 && time3 < time2)
                            return true;
                        else
                            return false;
                    }).ToList());
            }            
        }

        private void AnalysisData()
        {

        }
    }
}
