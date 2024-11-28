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

namespace TimeTrack_Pro.Code
{
    public class OriginalDataHandle
    {
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
                originalDatas = new OriginalSheetModel();
                originalDatas.Datas = new List<OriginalData>();
                for (int i = 0; ; i++)
                {                    
                    OriginalData data = new OriginalData();                    
                    
                    message = worksheet.Cells[$"C{2 + i * 4}"].Value.ToString();
                    if (string.IsNullOrEmpty(message) || !Regex.IsMatch(message, @"^[0-9]+$"))
                        break;

                    data.Id = Convert.ToInt32(message);
                    message = worksheet.Cells[$"G{2 + i * 4}"].Value.ToString();
                    if (string.IsNullOrEmpty(message))
                        break;

                    data.Name = message;
                    message = worksheet.Cells[$"L{2 + i * 4}"].Value.ToString();
                    if (string.IsNullOrEmpty(message))
                        break;

                    data.Department = message;
                    message = worksheet.Cells[$"Q{2 + i * 4}"].Value.ToString();
                    if (string.IsNullOrEmpty(message))
                        break;

                    data.RuleName = message;
                    data.Datas = new List<DateTime>[31];
                    for (int j = 0; j < 31; j++)
                    {
                        data.Datas[j] = new List<DateTime>();
                        message = worksheet.Cells[(i + 1)*4,j + 1].Value.ToString();
                        if (string.IsNullOrEmpty(message))
                            continue;

                        string[] times = message.Split(' ');
                        foreach (string time in times)
                        {
                            if (!Regex.IsMatch(time, @"^[0-9]{2}:[0-9]{2}$"))
                                continue;
                            DateTime date = DateTime.Parse(time);
                            data.Datas[j].Add(date);
                        }
                    }
                    originalDatas.Datas.Add(data);
                }
            }
        }

        public List<Employee> GetTypeDatas(int Type)
        {
            List<Employee> employees = new List<Employee>();
            Employee one = null;
            AttendanceRule rule = null;
            TimeSpan span = new TimeSpan(1, 0, 0);
            TimeSpan dayMin, dayMax;
            bool overDay = false;
            int hour = 0, min = 0, lateMin = 0, lateNum = 0, overH = 0, overM = 0;
            int Dlate = 0;            
            TimeSpan start, end, total, overTime;
            int days = DateTimeHelper.GetDays(originalDatas.Date.Year, originalDatas.Date.Month);
            foreach (var org in OriginalDatas.Datas)
            {
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
                for (int d = 0; d < days; d++)
                {
                    DateTime todayTime = new DateTime(originalDatas.Date.Year, originalDatas.Date.Month, d + 1);
                    int week = DateTimeHelper.GetWeek(originalDatas.Date.Year, originalDatas.Date.Month, d + 1);
                    ClassSection[] sections = rule.Classes[week];
                    TimeSpan[] times = new TimeSpan[6];                    
                    List<DateTime> relDatas = new List<DateTime>();
                    relDatas.AddRange(org.Datas[d].ToArray());
                    if (overDay)
                    {
                        var dd = org.Datas[d + 1].Where(a => a.TimeOfDay >= TimeSpan.Zero && a.TimeOfDay <= rule.Inter_dayTime);
                        relDatas.AddRange(dd.ToArray());
                    }
                    foreach (var t in relDatas)
                    {
                        TimeSpan daySpan = t - todayTime;
                        if (daySpan >= dayMin && daySpan <= sections[0].StartTime)                           
                        {
                            if (times[0] == TimeSpan.Zero)
                            {
                                times[0] = daySpan;                               
                            }                            
                        }
                        else if(daySpan > sections[0].StartTime && daySpan <= sections[0].EndTime)                                
                        {
                            if(times[0] == TimeSpan.Zero)
                            {
                                times[0] = daySpan;
                                lateNum++;                                
                            }
                            else
                            {                                
                                if (times[1] == TimeSpan.Zero)
                                {
                                    times[1] = daySpan;
                                    lateNum++;                                    
                                }
                            }                            
                        }
                        else if(daySpan > sections[0].EndTime && daySpan <= sections[1].StartTime)                              
                        {
                            if (times[1] == TimeSpan.Zero)
                            {
                                if(times[0] == TimeSpan.Zero)
                                {
                                    times[2] = daySpan;                                    
                                }
                                else
                                {
                                    times[1] = daySpan;                                    
                                }                                
                            }
                            else
                            {
                                if(times[2] == TimeSpan.Zero)
                                {
                                    times[2] = daySpan;                                    
                                }                                
                            }
                        }
                        else if(daySpan > sections[1].StartTime && daySpan <= sections[1].EndTime)
                        {
                            if (times[2] == TimeSpan.Zero)
                            {
                                times[2] = daySpan;
                                lateNum++;                                
                            }
                            else
                            {
                                if (times[3] == TimeSpan.Zero)
                                {
                                    times[3] = daySpan;
                                    lateNum++;                                    
                                }
                            }
                        }
                        else if(daySpan > sections[1].EndTime && daySpan <= sections[2].StartTime)
                        {
                            if (times[3] == TimeSpan.Zero)
                            {
                                if (times[2] == TimeSpan.Zero)
                                {
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
                                {
                                    times[4] = daySpan;                                    
                                }
                            }
                        }
                        else if(daySpan > sections[2].StartTime && daySpan <= sections[2].EndTime)
                        {
                            if (times[4] == TimeSpan.Zero)
                            {
                                times[4] = daySpan;
                                lateNum++;                                                                   
                            }
                            else
                            {
                                if (times[5] == TimeSpan.Zero)
                                {
                                    times[5] = daySpan;
                                    lateNum++;                                    
                                }
                            }
                        }
                        else if(daySpan > sections[2].EndTime && daySpan <= dayMax)
                        {
                            if (times[5] == TimeSpan.Zero)
                            {
                                times[5] = daySpan;                                
                            }
                        }
                    }

                }           
                employees.Add(one);
            }
            return employees;
        }
        
    }
}
