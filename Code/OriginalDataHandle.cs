using OfficeOpenXml;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TimeTrack_Pro.Model;
using TimeTrack_Pro.Helper;
using System.Drawing;
using NPOI.SS.Formula.Functions;
using HandyControl.Controls;

namespace TimeTrack_Pro.Code
{
    public class OriginalDataHandle
    {
        private bool isshiftMode = true;

        public bool IsShiftMode
        {
            get { return isshiftMode; }
            set { isshiftMode = value; }
        }

        private OriginalSheetModel originalDatas;
        public OriginalSheetModel OriginalDatas { get { return originalDatas; } }
        
        public OriginalDataHandle(string path)
        {
            init(path);
        }

        public void init(string path)
        {          
            //创建一个新的Excel包
            using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                string? message;
                int year = 0, month = 0;
                string datestr = worksheet.Cells[2, 1].Value.ToString();
                string t = null;
                int l = 0;
                for (int i = 0; i < datestr.Length; i++)
                {
                    if (Regex.IsMatch(datestr.ElementAt(i).ToString(), @"^[0-9]+$"))                                            
                        t += datestr.ElementAt(i).ToString();                    
                }
                if(t.Length < 4)
                {
                    MessageBox.Show("获取原始表日期失败！");
                    App.Log.Info("获取原始表日期失败！");
                    return;
                }
                year = Convert.ToInt32(t.Substring(0,4));
                month = Convert.ToInt32(t.Substring(4,t.Length - 4));
                originalDatas = new OriginalSheetModel();
                
                originalDatas.Datas = new List<OriginalData>();
                originalDatas.Date = new DateTime(year, month, 31);
                for (int i = 0; ; i++)
                {                    
                    OriginalData data = new OriginalData();
                    if (worksheet.Cells[$"C{3 + i * 4}"].Value == null)
                        break;
                    message = worksheet.Cells[$"C{3 + i * 4}"].Value.ToString();
                    if (string.IsNullOrEmpty(message) || !Regex.IsMatch(message, @"^[0-9]+$"))
                        break;
                    data.Id = Convert.ToInt32(message);

                    if (worksheet.Cells[$"G{3 + i * 4}"].Value == null)
                        break;
                    message = worksheet.Cells[$"G{3 + i * 4}"].Value.ToString();
                    if (string.IsNullOrEmpty(message))
                        break;
                    data.Name = message;

                    if (worksheet.Cells[$"L{3 + i * 4}"].Value == null)
                        break;
                    message = worksheet.Cells[$"L{3 + i * 4}"].Value.ToString();
                    if (string.IsNullOrEmpty(message))
                        break;
                    data.Department = message;

                    if (worksheet.Cells[$"Q{3 + i * 4}"].Value == null)
                        break;
                    message = worksheet.Cells[$"Q{3 + i * 4}"].Value.ToString();
                    if (string.IsNullOrEmpty(message))
                        break;
                    data.RuleName = message;

                    data.Datas = new List<DateTime>[32];
                    DateTime date = new DateTime(year, month, 1);
                    for (int j = 0; j < data.Datas.Count(); j++)
                    {
                        data.Datas[j] = new List<DateTime>();
                        if (worksheet.Cells[5 + i * 4, j + 1].Value == null)
                        {
                            date = date.AddDays(1);
                            continue;
                        }
                        message = worksheet.Cells[5 + i * 4, j + 1].Value.ToString();
                        if (string.IsNullOrEmpty(message))
                        {
                            date = date.AddDays(1);
                            continue;
                        }
                        string[] times = message.Split(' ');
                        foreach (string time in times)
                        {
                            if (!Regex.IsMatch(time, @"^[0-9]{2}:[0-9]{2}$"))
                                continue;
                            DateTime dtime = DateTime.Parse(time);
                            
                            data.Datas[j].Add(new DateTime(date.Year,date.Month,date.Day,dtime.Hour,dtime.Minute,dtime.Second));
                        }
                        date = date.AddDays(1);
                    }
                    originalDatas.Datas.Add(data);
                }
            }
        }

        private List<Employee> GetTypeDatas(int Type)
        {            
            List<Employee> employees = new List<Employee>();
            if (OriginalDatas == null)
                return employees;
            Employee one = null;
            
            TimeSpan span = new TimeSpan(1, 0, 0);
            TimeSpan dayMin, dayMax;
            bool overDay = false;           
            int days = DateTimeHelper.GetDays(originalDatas.Date.Year, originalDatas.Date.Month);
            foreach (var org in OriginalDatas.Datas)
            {
                AttendanceRule rule = null;
                if (Rules.RuleList.Count() > 0 && (Rules.RuleList.Find(r => r.RuleName == org.RuleName) != null))
                {
                    rule = Rules.RuleList.Find(r => r.RuleName == org.RuleName);
                }
                else
                {
                    rule = Rules.DefaultRule;
                }
                if(rule.Inter_dayTime != TimeSpan.Zero)
                {
                    overDay = true;
                    dayMin = rule.Inter_dayTime;
                    dayMax = rule.Inter_dayTime.Add(new TimeSpan(1,0,0,0));
                }
                else
                {
                    dayMin = new TimeSpan(0, 0, 0);
                    dayMax = new TimeSpan(23, 59, 59);
                }
                if (Type == 0)
                    one = new StatisticsData(org);
                else if (Type == 1)
                    one = new SummaryData(org);
                else 
                    one = new ExceptionData(org);
                if (Type == 0 || Type == 1)
                {

                    //实际出勤
                    ((Sum_Stati_transit)one).AtlAtd = org.Datas.Where(a => a.Count() > 0).Count().ToString();
                    //标准
                    ((Sum_Stati_transit)one).StdAtd = days.ToString();
                }
                if (Type == 0)
                    ((StatisticsData)one).DaysOfWeek = DateTimeHelper.GetDaysByWeek(originalDatas.Date.Year, originalDatas.Date.Month);
                int hour = 0, min = 0, lateMin = 0, lateNum = 0, overH = 0, overM = 0;
                int Dlate = 0;
                ExceptionPart part = null;
                TimeSpan start, end, total, overTime;
                for (int d = 0; d < days; d++)
                {
                    DateTime todayTime = new DateTime(originalDatas.Date.Year, originalDatas.Date.Month, d + 1);
                    int week = DateTimeHelper.GetWeek(originalDatas.Date.Year, originalDatas.Date.Month, d + 1);
                    ClassSection[] sections = rule.Classes[week];
                    TimeSpan[] times = new TimeSpan[6] { TimeSpan.Zero, TimeSpan.Zero,
                                                         TimeSpan.Zero, TimeSpan.Zero,
                                                         TimeSpan.Zero, TimeSpan.Zero,};   
                    
                    List<DateTime> relDatas = new List<DateTime>();
                    relDatas.AddRange(org.Datas[d].ToArray());
                    if (overDay)
                    {
                        var dd = org.Datas[d + 1].Where(a => a.TimeOfDay >= TimeSpan.Zero && a.TimeOfDay <= rule.Inter_dayTime);
                        relDatas.AddRange(dd.ToArray());
                    }
                    var ts = relDatas.Select(t => t - todayTime).Order().ToList();
                    foreach (var t in ts)
                    {
                        selectTimeSpane(ref times, sections, t, dayMin, dayMax, rule.ShiftMode);
                    }                    
                    start = TimeSpan.Zero;
                    end = TimeSpan.Zero;
                    total = TimeSpan.Zero;
                    overTime = TimeSpan.Zero;
                    Dlate = 0;
                    for (int i = 0; i < times.Length; i++)
                    {
                        if (times[i] != TimeSpan.Zero)
                        {
                            ClassSection seon = sections[i / 2];
                            if (Type == 0)
                                ((StatisticsData)one).SignUpDatas[d][i].Text = string.Format("{0:00}:{1:00}", times[i].Hours, times[i].Minutes);
                            TimeSpan t;
                            if (i % 2 == 0)
                            {                               
                                t = seon.StartTime + new TimeSpan(0, rule.AllowLate + rule.StatsUnit, 0);
                                //比较，选择正确的时间段。迟到
                                if (times[i] > t)
                                {
                                    if (Type == 0)
                                        ((StatisticsData)one).SignUpDatas[d][i].Color = Color.Red;
                                    else if (Type == 2)
                                    {
                                        if (part == null)
                                            part = new ExceptionPart();
                                        part.ESignUpDatas[i] = string.Format("{0:00}:{1:00}", times[i].Hours, times[i].Minutes);
                                    }
                                    start = times[i] - new TimeSpan(0, rule.StatsUnit + rule.AllowLate, 0);
                                    Dlate += (int)(times[i] - t).TotalMinutes;
                                    lateNum++;
                                }
                                else
                                {
                                    start = seon.StartTime;
                                }
                            }
                            else
                            {
                                t = seon.EndTime - new TimeSpan(0, rule.AllowLate + rule.StatsUnit, 0);
                                if (times[i] < t)
                                {
                                    if (Type == 0)
                                        ((StatisticsData)one).SignUpDatas[d][i].Color = Color.Red;
                                    else if (Type == 2)
                                    {
                                        if (part == null)
                                            part = new ExceptionPart();
                                        part.ESignUpDatas[i] = string.Format("{0:00}:{1:00}", times[i].Hours, times[i].Minutes);
                                    }
                                    end = times[i] + new TimeSpan(0, rule.StatsUnit + rule.AllowEarly, 0);
                                    Dlate += (int)(t - times[i]).TotalMinutes;
                                    lateNum++;
                                }
                                else
                                {
                                    end = seon.EndTime;
                                }
                                //时间段不全或者后者小于前者，则不计算
                                if (end != TimeSpan.Zero && start != TimeSpan.Zero && end > start)
                                {
                                    if (seon.Type == 0)//正常
                                        total += end - start;
                                    else if (seon.Type == 1)//加班
                                        overTime += end - start;
                                }
                                start = TimeSpan.Zero;
                                end = TimeSpan.Zero;
                            }
                        }
                        else
                        {
                            start = TimeSpan.Zero;
                            end = TimeSpan.Zero;
                        }
                    }
                    if (Type == 2 && part != null)
                    {
                        part.Date = string.Format("{0:00}-{1:00}", originalDatas.Date.Month, d + 1);
                        part.LateOrEarly = string.Format("{0:00}:{1:00}", Dlate / 60, Dlate % 60);
                        ((ExceptionData)one).Parts.Add(part);
                        part = null;
                    }
                    lateMin += Dlate;
                    if (total != TimeSpan.Zero)
                    {
                        hour += total.Hours;
                        min += total.Minutes;
                        if (Type == 0)
                            ((StatisticsData)one).SignUpDatas[d][6].Text = total.ToString().Substring(0, 5);
                    }
                    if (overTime != TimeSpan.Zero)
                    {
                        overH += overTime.Hours;
                        overM += overTime.Minutes;
                        if (Type == 0)
                            ((StatisticsData)one).SignUpDatas[d][7].Text = total.ToString().Substring(0, 5);
                    }
                }
                //日期
                if (Type == 0)
                    ((StatisticsData)one).Date = string.Format($"{originalDatas.Date.Year}-{originalDatas.Date.Month.ToString("00")}");
                if (Type == 0 || Type == 1)
                {
                    ((Sum_Stati_transit)one).AtlWorkTime = string.Format("{0:00}:{1:00}", hour + min / 60, min % 60);
                    ((Sum_Stati_transit)one).StdWorkTime = rule.GetStdTimeStr(originalDatas.Date.Year, originalDatas.Date.Month);
                    ((Sum_Stati_transit)one).Wko_Common = string.Format("{0:00}:{1:00}", overH + overM / 60, overM % 60);
                    ((Sum_Stati_transit)one).Wko_Special = string.Format("{0:00}:{1:00}", 0, 0);
                    ((Sum_Stati_transit)one).LateEarly_Count = lateNum.ToString();
                    ((Sum_Stati_transit)one).LateEarly_Min = lateMin.ToString();
                }
                employees.Add(one);
            }
            return employees.OrderBy(s => s.Id).ToList();
        }
        /// <summary>
        /// 对输入的签到时间段进行选择
        /// </summary>
        /// <param name="times">输出的时间段集合</param>
        /// <param name="sections"></param>
        /// <param name="daySpan"></param>
        /// <param name="dayMin"></param>
        /// <param name="dayMax"></param>
        /// <param name="mode"></param>
        private void selectTimeSpane(ref TimeSpan[] times, ClassSection[] sections, TimeSpan daySpan, TimeSpan dayMin, TimeSpan dayMax, int mode)
        {
            //换班等分线
            double ratio = mode == 0 ? (double)1 / (double)2 : (double)1 / (double)3;
            double r = 0;
            if (sections[0].StartTime == TimeSpan.Zero || sections[0].EndTime == TimeSpan.Zero)
                return;
            if (daySpan >= dayMin && daySpan <= sections[0].StartTime)
            {
                if (times[0] == TimeSpan.Zero)
                    times[0] = daySpan;
                return;
            }

            if (daySpan > sections[0].StartTime && daySpan < sections[0].EndTime)
            {
                if (times[0] == TimeSpan.Zero)
                {
                    times[0] = daySpan;
                }
                else
                {
                    if (times[1] == TimeSpan.Zero)
                        times[1] = daySpan;
                    else
                    {                        
                        if (sections[0].EndTime.Subtract(times[1]).Ticks > sections[0].EndTime.Subtract(daySpan).Ticks)
                            times[1] = daySpan;                        
                    }
                }
                return;
            }

            if (sections[1].StartTime == TimeSpan.Zero && sections[1].EndTime == TimeSpan.Zero)
            {
                if (daySpan >= sections[0].EndTime && daySpan <= dayMax)
                {
                    if (times[1] == TimeSpan.Zero)
                        times[1] = daySpan;  
                    else
                    {
                        if (times[1] < sections[0].EndTime)
                            times[1] = daySpan;
                        else
                        {
                            if (times[1].Subtract(sections[0].EndTime).Ticks > daySpan.Subtract(sections[0].EndTime).Ticks)
                                times[1] = daySpan;
                        }
                    }
                }
                return;
            }

            if (daySpan >= sections[0].EndTime && daySpan <= sections[1].StartTime)
            {                
                if(isshiftMode)
                {
                    r = (double)daySpan.Subtract(sections[0].EndTime).Ticks / (double)sections[1].StartTime.Subtract(sections[0].EndTime).Ticks;
                    if (0 <= r && r <= 1)
                    {
                        if(r < ratio)
                        {
                            if (times[1] == TimeSpan.Zero)
                                times[1] = daySpan;
                            else
                            {
                                if (times[1] < sections[0].EndTime)
                                    times[1] = daySpan;
                                else
                                {
                                    if (times[1].Subtract(sections[0].EndTime).Ticks > daySpan.Subtract(sections[0].EndTime).Ticks)
                                        times[1] = daySpan;
                                }
                            }
                        }
                        else
                        {
                            if (times[2] == TimeSpan.Zero)
                                times[2] = daySpan;
                        }
                    }                                      
                }
                else
                {
                    if (times[1] == TimeSpan.Zero)
                    {
                        if (times[0] == TimeSpan.Zero)
                        {
                            if (times[2] == TimeSpan.Zero)                                                           
                                times[2] = daySpan;
                        }
                        else
                        {                            
                            times[1] = daySpan;
                        }
                    }
                    else
                    {                                              
                        if (times[2] == TimeSpan.Zero)
                        {                            
                            times[2] = daySpan;
                        }                                               
                    }
                }                
                return;
            }

            if (daySpan > sections[1].StartTime && daySpan < sections[1].EndTime)
            {
                if (times[2] == TimeSpan.Zero)
                {
                    times[2] = daySpan;
                }
                else
                {
                    if (times[3] == TimeSpan.Zero)
                        times[3] = daySpan;
                    else
                    {
                        if (sections[1].EndTime.Subtract(times[3]) > sections[1].EndTime.Subtract(daySpan))
                            times[3] = daySpan;
                    }
                }
                return;
            }

            if (sections[2].StartTime == TimeSpan.Zero && sections[2].EndTime == TimeSpan.Zero)
            {
                if (daySpan >= sections[1].EndTime && daySpan <= dayMax)
                {
                    if (times[3] == TimeSpan.Zero)
                        times[3] = daySpan;      
                    else
                    {
                        if (times[3] < sections[1].EndTime)
                            times[3] = daySpan;
                        else
                        {
                            if (times[3].Subtract(sections[1].EndTime).Ticks > daySpan.Subtract(sections[1].EndTime).Ticks)
                                times[3] = daySpan;
                        }
                    }
                }
                return;
            }

            if (daySpan >= sections[1].EndTime && daySpan <= sections[2].StartTime)
            {
                if (isshiftMode)
                {
                    r = (double)daySpan.Subtract(sections[1].EndTime).Ticks / (double)sections[2].StartTime.Subtract(sections[1].EndTime).Ticks;
                    if(0 <= r && r <= 1)
                    {
                        if(r < ratio)
                        {
                            if (times[3] == TimeSpan.Zero)
                                times[3] = daySpan;
                            else
                            {
                                if (times[3] < sections[1].EndTime)
                                    times[3] = daySpan;
                                else
                                {
                                    if (times[3].Subtract(sections[1].EndTime).Ticks > daySpan.Subtract(sections[1].EndTime).Ticks)
                                        times[3] = daySpan;
                                }
                            }
                        }
                        else
                        {
                            if(times[4] == TimeSpan.Zero)
                                times[4] = daySpan;
                        }
                    }
                }
                else
                {
                    if (times[3] == TimeSpan.Zero)
                    {
                        if (times[2] == TimeSpan.Zero)
                        {
                            if (times[4] == TimeSpan.Zero)                                                            
                                times[4] = daySpan;                            
                        }
                        else
                        {                           
                            times[3] = daySpan;
                        }
                    }
                    else
                    {
                        if (times[4] == TimeSpan.Zero)                                                   
                            times[4] = daySpan;                       
                    }
                }                                                  
                return;
            }

            if (daySpan > sections[2].StartTime && daySpan < sections[2].EndTime)
            {
                if (times[4] == TimeSpan.Zero)
                {
                    times[4] = daySpan;
                }
                else
                {
                    if (times[5] == TimeSpan.Zero)
                        times[5] = daySpan;
                    else
                    {
                        if (sections[2].EndTime.Subtract(times[5]) > sections[2].EndTime.Subtract(daySpan))
                            times[5] = daySpan;
                    }
                }
                return;
            }

            if (daySpan >= sections[2].EndTime && daySpan <= dayMax)
            {
                if (times[5] == TimeSpan.Zero)
                    times[5] = daySpan;  
                else
                {
                    if (times[5] < sections[2].EndTime)
                        times[5] = daySpan;
                    else
                    {
                        if (times[5].Subtract(sections[2].EndTime).Ticks > daySpan.Subtract(sections[2].EndTime).Ticks)
                            times[5] = daySpan;
                    }
                }
                return;
            }
        }

        private List<StatisticsData> GetStatisticsDatas()
        {
            return GetTypeDatas(0).Select(s => (StatisticsData)s).ToList();
        }

        public StatisticsSheetModel GetStatisticsSheetModel()
        {
            StatisticsSheetModel sheetModel = new StatisticsSheetModel();
            sheetModel.Datas = GetStatisticsDatas();
            return sheetModel;
        }

        private List<SummaryData> GetSummaryDatas()
        {
            return GetTypeDatas(1).Select(s => (SummaryData)s).ToList();
        }

        public SummarySheetModel GetSummarySheetModel()
        {
            SummarySheetModel summarySheetModel = new SummarySheetModel();
            summarySheetModel.Date = string.Format($"{originalDatas.Date.Year}-{originalDatas.Date.Month.ToString("00")}");
            summarySheetModel.Datas = GetTypeDatas(1).Select(s => (SummaryData)s).ToList();
            return summarySheetModel;
        }

        private List<ExceptionData> GetExceptionDatas()
        {
            return GetTypeDatas(2).Select(s => (ExceptionData)s).ToList();
        }

        public ExceptionSheetModel GetExceptionSheetModel()
        {
            ExceptionSheetModel sheetModel = new ExceptionSheetModel();
            sheetModel.Date = string.Format($"{originalDatas.Date.Year}-{originalDatas.Date.Month.ToString("00")}");
            sheetModel.Datas = GetExceptionDatas();
            return sheetModel;
        }
    }
}
