using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TimeTrack_Pro.Model;
using System.Windows.Shapes;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Quartz;
using NPOI.SS.Formula.Functions;
using HandyControl.Controls;

namespace TimeTrack_Pro.Code
{
    public class Point
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
    }

    class DayRecord
    {
        public DateTime Date { get; set; }
        public List<TimeSpan> Records { get; set; }
    }

    class RangeAndRatio
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public List<TimeSpan> Datas { get; set; }
        public double Ratio { get; set; }
        public TimeSpan Average {
            get {
                if (Datas.Count > 0)
                {
                    double averageTicks = Datas.Average(t => t.Ticks);
                    return TimeSpan.FromTicks((long)averageTicks);
                }
                else
                    return TimeSpan.Zero;
            } }
    }

    public class SmartScheduling
    {
        //限定第一到第三次的打卡范围
        public TimeSpan FirstStart { get; set; } = new TimeSpan(0,6,0,0);
        public TimeSpan FirstEnd { get; set; } = new TimeSpan(0,12,30,0);
        public TimeSpan SecondStart { get; set; } = new TimeSpan(0,12,30,0);
        public TimeSpan SecondEnd { get; set; } = new TimeSpan(0,18,30,0);
        public TimeSpan ThirdStart { get; set; } = new TimeSpan(0,18,30,0);
        public TimeSpan ThirdEnd { get; set; } = new TimeSpan(0,21,0,0);

        private string filePath = "F:\\文档\\atendance_data_csv.txt";
        private TimeSpan crossTime = TimeSpan.Zero;                
        private int hours = 24;
        private int point = 10;//每个相邻区间的时间间隔（单位：分钟）        
        private double minPro = 0.01;//每各区间的最少占比要求        
        private double m = 0.8;
        private OriginalSheetModel? model = null;
        private List<DateTime>? datas = null;        
        private List<AttendanceRecord>? records = null;       
        private List<RangeAndRatio> ars = null;

        public SmartScheduling(OriginalSheetModel model)  
        {
            this.model = model;
            _load();            
            _init_ranges();
            _init_records();
            Output();
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

        private void _init_ranges()
        {
            int totalCount = 0;
            //将24个小时分割成间隔为point的时间段
            List<Point> splitPoint = new List<Point>();
            int n = 60 / point;
            for (int h = 0; h < hours; h++)
            {
                for (int i = 0; i < n; i++)
                {
                    splitPoint.Add(new Point() { Hour = h, Minute = i * point });
                }
            }
            splitPoint.Add(new Point() { Hour = 24, Minute = 0 });
            //将签到数据筛选归类到各自的时间段
            ars = new List<RangeAndRatio>();
            for (int i = 0; i < splitPoint.Count()-1; i++)
            {
                RangeAndRatio ratio = new RangeAndRatio();
                ratio.Start = new TimeSpan(splitPoint[i].Hour, splitPoint[i].Minute, 0);
                ratio.End = new TimeSpan(splitPoint[i + 1].Hour, splitPoint[i + 1].Minute, 0);
                ratio.Datas = datas.Where(d =>
                {
                    int time1 = splitPoint[i].Hour * 60 + splitPoint[i].Minute;
                    int time2 = splitPoint[i + 1].Hour * 60 + splitPoint[i + 1].Minute;
                    int time3 = d.Hour * 60 + d.Minute;
                    if (time3 >= time1 && time3 < time2)
                        return true;
                    else
                        return false;
                }).Select(d => d.TimeOfDay).ToList();
                totalCount += ratio.Datas.Count;                              
                ars.Add(ratio);
            }
            //求每一个时间段的数据占比
            for (int i = 0; i < ars.Count; i++)
            {
                ars[i].Ratio = (double)ars[i].Datas.Count / totalCount;
            }
            //删除不符合最少占比的时间段
            var newars = ars.Where(a => a.Ratio > minPro).ToList();
            //将相邻且相近的数据合并
            List<RangeAndRatio> rars = new List<RangeAndRatio>();
            int j = 0;
            RangeAndRatio one = newars[j];
            while (true)
            {
                if (j + 1 >= newars.Count)
                {
                    rars.Add(one);
                    break;
                }
                else
                    j++;
                if (newars[j].Average - one.Average < new TimeSpan(0, point * 2, 0))
                {
                    one.End = one.End.Add(new TimeSpan(0,point,0));
                    one.Datas.AddRange(newars[j].Datas);
                    one.Ratio = (double)one.Datas.Count / totalCount;                    
                }
                else
                {
                    rars.Add(one);                    
                    one = newars[j];
                }                
            }
            double minSign = (double)1 / (rars.Count * 2);
            rars.RemoveAll(r => r.Ratio < minSign);
            List<TimeSpan> workTable = new List<TimeSpan>();
            for (int i = 0; i < rars.Count; i++)
            {
                if (crossTime == new TimeSpan(0, 0, 0, 0))
                {                    
                    workTable.Add(rars[i].Average);
                }
                else
                {                   
                    if (rars[i].End < crossTime)
                        workTable.Add(rars[i].Average.Add(new TimeSpan(1, 0, 0, 0)));
                    else
                        workTable.Add(rars[i].Average);                    
                }
            }
            workTable.OrderBy(r => r.Ticks);

        }           

        private void _init_records()
        {            
            records = new List<AttendanceRecord>();
            foreach (var item in model.Datas)
            {
                List<DayRecord> newDatas = ArrangeModelData(item.Datas);
                foreach (var data in newDatas)
                {
                    bool firstFlag = false;
                    bool secondFlag = false;
                    bool thirdFlag = false;
                    AttendanceRecord attendanceRecord = new AttendanceRecord();
                    attendanceRecord.EmployeeID = item.Id;
                    attendanceRecord.Date = data.Date;
                    for (int i = 0; i < data.Records.Count(); i++)
                    {                       
                        if (FirstStart <= data.Records[i] && data.Records[i] < FirstEnd)
                        {
                            if (!firstFlag)
                            {
                                attendanceRecord.CheckIn1 = data.Records[i];
                                firstFlag = true;
                            }
                            else
                            {
                                if (data.Records[i] > attendanceRecord.CheckIn1.Add(new TimeSpan(0, 0, 30, 0)))
                                    attendanceRecord.CheckOut1 = data.Records[i];
                            }
                        }
                        if(SecondStart <= data.Records[i] && data.Records[i] < SecondEnd)
                        {
                            if (!secondFlag)
                            {
                                attendanceRecord.CheckIn2 = data.Records[i];
                                secondFlag = true;
                            }
                            else
                            {
                                if (data.Records[i] > attendanceRecord.CheckIn2.Add(new TimeSpan(0, 0, 30, 0)))
                                    attendanceRecord.CheckOut2 = data.Records[i];
                            }
                        }
                        if(ThirdStart <= data.Records[i] && data.Records[i] < ThirdEnd)
                        {
                            if (!thirdFlag)
                            {
                                attendanceRecord.CheckIn3 = data.Records[i];
                                thirdFlag = true;
                            }
                            else
                            {
                                if (data.Records[i] > attendanceRecord.CheckIn3.Add(new TimeSpan(0, 0, 30, 0)))
                                    attendanceRecord.CheckOut3 = data.Records[i];
                            }
                        }
                    }
                    records.Add(attendanceRecord);
                }
            }
        }
        /// <summary>
        /// 整理数据，将跨天的数据放上一天数组中
        /// </summary>
        /// <param name="OriginalData"></param>
        /// <returns></returns>
        private List<DayRecord> ArrangeModelData(List<DateTime>[] OriginalData)
        {
            List<DayRecord> dayRecords = new List<DayRecord>();
            int n = 0;
            for (int j = 0; j < OriginalData.Count(); j++)
            {
                if (OriginalData[j].Count == 0)
                    continue;
                DayRecord dayRecord = new DayRecord();
                dayRecord.Date = OriginalData[j][0].Date;
                dayRecord.Records = new List<TimeSpan>();
                for (int i = 0; i < OriginalData[j].Count(); i++)
                {
                    if (OriginalData[j][i].TimeOfDay < crossTime)
                    {
                        TimeSpan time = dayRecord.Date - dayRecords[n - 1].Date;
                        if (n > 0 && time == new TimeSpan(1, 0, 0, 0))
                        {
                            if (dayRecords[n - 1].Records.Count > 0)
                            {
                                if (OriginalData[j][i].TimeOfDay - dayRecords[n - 1].Records.Last() > new TimeSpan(0, 0, 30, 0))
                                    dayRecords[n - 1].Records.Add(OriginalData[j][i].TimeOfDay.Add(new TimeSpan(1, 0, 0, 0)));
                            }
                            else
                            {
                                dayRecords[n - 1].Records.Add(OriginalData[j][i].TimeOfDay.Add(new TimeSpan(1, 0, 0, 0)));
                            }                               
                        }                           
                    }
                    else
                    {
                        if(dayRecord.Records.Count > 0)
                        {
                            if(OriginalData[j][i].TimeOfDay - dayRecord.Records.Last() > new TimeSpan(0,0,30,0))
                                dayRecord.Records.Add(OriginalData[j][i].TimeOfDay);
                        }
                        else
                        {
                            dayRecord.Records.Add(OriginalData[j][i].TimeOfDay);
                        }                        
                    }
                }
                dayRecords.Add(dayRecord);
                n++;
            }                                        
            return dayRecords;
        }

        public void CreateFile_CSV()
        {           
            using (FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {                
                for (int i = 0; i < model?.Datas?.Count(); i++)
                {
                    for (int j = 0; j < model?.Datas?[i].Datas?.Count(); j++)
                    {                       
                        foreach (var item in model?.Datas?[i].Datas?[j])
                        {
                            string data = $"{model?.Datas?[i].Name},";
                            data += item.ToString("HH,mm");
                            data += "\r\n";
                            file.Write(Encoding.UTF8.GetBytes(data));                            
                        }
                    }
                }
            }
        }

        public void Output()
        {
            // 分组按日期计算每天的平均打卡时间
            var dailyRecords = records.GroupBy(r => r.Date)
                                      .Select(g => new DailyAttendance
                                      {
                                          Date = g.Key,
                                          CheckIn1s = g.Select(r => r.CheckIn1).ToList(),
                                          CheckOut1s = g.Select(r => r.CheckOut1).ToList(),
                                          CheckIn2s = g.Select(r => r.CheckIn2).ToList(),
                                          CheckOut2s = g.Select(r => r.CheckOut2).ToList(),
                                          CheckIn3s = g.Select(r => r.CheckIn3).ToList(),
                                          CheckOut3s = g.Select(r => r.CheckOut3).ToList()
                                      })
                                      .ToList();

            // 计算每天的平均打卡时间
            var firstCheckInStats = CalculateTimeStats(dailyRecords.SelectMany(d => d.CheckIn1s));
            var firstCheckOutStats = CalculateTimeStats(dailyRecords.SelectMany(d => d.CheckOut1s));
            var secondCheckInStats = CalculateTimeStats(dailyRecords.SelectMany(d => d.CheckIn2s));
            var secondCheckOutStats = CalculateTimeStats(dailyRecords.SelectMany(d => d.CheckOut2s));
            var thirdCheckInStats = CalculateTimeStats(dailyRecords.SelectMany(d => d.CheckIn3s));
            var thirdCheckOutStats = CalculateTimeStats(dailyRecords.SelectMany(d => d.CheckOut3s));

            // 推测规定上下班时间
            TimeSpan firstCheckInTime = CalculateThreshold(firstCheckInStats.Average, firstCheckInStats.StandardDeviation, m, true);
            TimeSpan firstCheckOutTime = CalculateThreshold(firstCheckOutStats.Average, firstCheckOutStats.StandardDeviation, m, false);
            TimeSpan secondCheckInTime = CalculateThreshold(secondCheckInStats.Average, secondCheckInStats.StandardDeviation, m, true);
            TimeSpan secondCheckOutTime = CalculateThreshold(secondCheckOutStats.Average, secondCheckOutStats.StandardDeviation, m, false);
            TimeSpan thirdCheckInTime = CalculateThreshold(thirdCheckInStats.Average, thirdCheckInStats.StandardDeviation, m, true);
            TimeSpan thirdCheckOutTime = CalculateThreshold(thirdCheckOutStats.Average, thirdCheckOutStats.StandardDeviation, m, false);

            // 输出结果
            Console.WriteLine($"推测的第一次上班时间: {firstCheckInTime}");
            Console.WriteLine($"推测的第一次下班时间: {firstCheckOutTime}");
            Console.WriteLine($"推测的第二次上班时间: {secondCheckInTime}");
            Console.WriteLine($"推测的第二次下班时间: {secondCheckOutTime}");
            Console.WriteLine($"推测的第三次上班时间: {thirdCheckInTime}");
            Console.WriteLine($"推测的第三次下班时间: {thirdCheckOutTime}");
        }

        private TimeSpan HandleCrossTime(TimeSpan checkIn, TimeSpan checkOut, TimeSpan crossTime)
        {
            if (checkOut < checkIn && checkOut <= crossTime)
            {
                checkOut = checkOut.Add(new TimeSpan(1, 0, 0, 0)); // 跨天情况，将checkOut加上一天
            }
            return checkOut;
        }

        private TimeStats CalculateTimeStats(IEnumerable<TimeSpan> times)
        {
            var ntimes = times.ToList();
            ntimes.RemoveAll(r => r == new TimeSpan(0,0,0,0));
            if (ntimes.Count == 0)
                return new TimeStats
                {
                    Average = new TimeSpan(0,0,0,0),
                    StandardDeviation = new TimeSpan(0,0,0,0)
                };
            double averageTicks = ntimes.Average(t => t.Ticks);
            double standardDeviationTicks = Math.Sqrt(ntimes.Select(t => (t.Ticks - averageTicks) * (t.Ticks - averageTicks)).Average());

            return new TimeStats
            {
                Average = TimeSpan.FromTicks((long)averageTicks),
                StandardDeviation = TimeSpan.FromTicks((long)standardDeviationTicks)
            };
        }

        private TimeSpan CalculateThreshold(TimeSpan average, TimeSpan standardDeviation, double n, bool isCheckIn)
        {
            if (average == new TimeSpan(0, 0, 0, 0) && standardDeviation == new TimeSpan(0, 0, 0, 0))
                return new TimeSpan(0,0,0,0);
            double multiplier = isCheckIn ? -1 : 1;
            double zScore = CalculateZScore(n);
            TimeSpan threshold = average.Add(new TimeSpan((long)(multiplier * zScore * standardDeviation.Ticks)));

            // 确保时间在一天内
            if (threshold.TotalHours < 0)
            {
                threshold = threshold.Add(new TimeSpan(1, 0, 0, 0));
            }
            else if (threshold.TotalHours >= 24)
            {
                threshold = threshold.Subtract(new TimeSpan(1, 0, 0, 0));
            }

            return threshold;
        }

        private double CalculateZScore(double n)
        {
            // 使用正态分布表查找Z值
            // 这里我们使用简单的近似方法，实际应用中可以使用更精确的库
            return Math.Sqrt(2) * SpecialFunctions.ErfInverse(2 * n - 1);
        }
    } 
    
    static class SpecialFunctions
    {
        // 求误差函数的逆
        public static double ErfInverse(double x)
        {
            if (x > 1 || x < -1)
            {
                throw new ArgumentOutOfRangeException(nameof(x), "x must be between -1 and 1");
            }

            double w;
            if (x >= 0)
            {
                w = 1 - x;
            }
            else
            {
                w = 1 + x;
            }

            double t = Math.Sqrt(-2 * Math.Log(w / 2.0));
            double y = t - ((0.0498673470 * t + 0.0212137947) * t - 0.0197084182) / ((0.0993484626 * t + 0.588581573) * t + 1.0);
            if (x < 0)
            {
                y = -y;
            }

            return y;
        }
    }
}
