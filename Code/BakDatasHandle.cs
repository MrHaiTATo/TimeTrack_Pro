using NPOI.Util;
using Org.BouncyCastle.Asn1.Pkcs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TimeTrack_Pro.Helper;
using TimeTrack_Pro.Model;

namespace TimeTrack_Pro.Code
{
    public class BakDatasHandle
    {
        private List<AttendanceData> attendanceDatas;
        public List<AttendanceData> AttendanceDatas { get { return attendanceDatas; } }

        private List<BakUseData> employees;
        public List<BakUseData> Employees { get { return employees; } }

        public BakDatasHandle() { }

        public BakDatasHandle(string attendanceFile, string employeeFile)
        {
            _init(attendanceFile, employeeFile);
        }

        public void LoadFile(string attendanceFile, string employeeFile)
        {
            _init(attendanceFile, employeeFile);
        }

        private void _init(string attendanceFile, string employeeFile)
        {
            string row;
            string[] cells;
            using (StreamReader reader = new StreamReader(attendanceFile))
            {
                attendanceDatas = new List<AttendanceData>();
                while (!reader.EndOfStream)
                {
                    row = reader.ReadLine();
                    if (string.IsNullOrEmpty(row) || row.Contains("NO") || row.Contains("YYYY/MM/DD"))
                        continue;
                    AttendanceData attendance = new AttendanceData();
                    cells = row.Split('|');
                    try
                    {
                        if (Regex.IsMatch(cells[0].Trim(), @"^[0-9]+$"))
                            attendance.Number = Convert.ToInt32(cells[0].Trim());
                        if (Regex.IsMatch(cells[1].Trim(), @"^[0-9]{4}(\-[0-9]{2}){2}$") && Regex.IsMatch(cells[2].Trim(), @"^[0-9]{2}:[0-9]{2}$"))
                            attendance.ClockTime = DateTime.ParseExact(cells[1].Trim() + " " + cells[2].Trim(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                        if (Regex.IsMatch(cells[3].Trim(), @"^[0-9]+$"))
                            attendance.UserIndex = Convert.ToInt32(cells[3].Trim());
                        if (Regex.IsMatch(cells[4].Trim(), @"^([0-9]{1,2})+\-[0-6]+$"))
                        {
                            attendance.Class = Convert.ToInt32(cells[4].Trim().Substring(0, 1));
                            attendance.ShiftClass = (ShiftClass)Convert.ToInt32(cells[4].Trim().Substring(2, 1));
                        }
                        else
                        {
                            attendance.Class = -1;
                            if (Regex.IsMatch(cells[4].Trim(), @"^\-[0-6]+$"))
                            {
                                attendance.ShiftClass = (ShiftClass)Convert.ToInt32(cells[4].Trim().Substring(1, 1));
                            }
                        }
                        if (Regex.IsMatch(cells[5].Trim(), @"^[1-5]{1}\s\-\s[0-9]{1}\s\-\s[0-1]{1}\s\-\s[0-9]{1}$"))
                        {
                            attendance.ClockMethod = (ClockMethod)Convert.ToInt32(cells[5].Trim().Substring(0, 1));
                            attendance.ClockState = (ClockState)Convert.ToInt32(cells[5].Trim().Substring(8, 1));
                        }
                        attendanceDatas.Add(attendance);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
                reader.Close();
            }
            using (StreamReader reader = new StreamReader(employeeFile))
            {
                employees = new List<BakUseData>();
                while (!reader.EndOfStream)
                {
                    row = reader.ReadLine();
                    if (string.IsNullOrEmpty(row) || row.Contains("NO") || row.Contains("UserName"))
                        continue;
                    cells = row.Split('|');
                    try
                    {
                        BakUseData employee = new BakUseData();
                        if (Regex.IsMatch(cells[0].Trim(), @"^[0-9]+$"))
                            employee.Number = Convert.ToInt32(cells[0].Trim());
                        employee.Name = cells[1].Trim();
                        if (Regex.IsMatch(cells[2].Trim(), @"^[0-9]+$"))
                        {
                            employee.Index = Convert.ToInt32(cells[2].Trim());
                        }
                        else
                        {
                            employee.Index = -1;
                        }
                        if (Regex.IsMatch(cells[3].Trim(), @"^[0-9]+$"))
                        {
                            employee.Id = Convert.ToInt32(cells[3].Trim());
                        }
                        else
                        {
                            employee.Id = -1;
                        }
                        if (Regex.IsMatch(cells[4].Trim(), @"^[0-9]{4}(\-[0-9]{2}){2}$"))
                            employee.CreatedTime = DateTime.ParseExact(cells[4].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        employees.Add(employee);
                    }
                    catch (Exception e)
                    {
                        App.Log.Error(e.Message + $" 异常发生位置：{e.StackTrace}");
                    }
                }
                employees.RemoveAll(e =>
                {
                    if (string.IsNullOrEmpty(e.Name) || e.Id == -1 || e.Index == -1)
                        return true;
                    else
                        return false;
                });
                employees = employees.GroupBy(e => e.Id).Select(g => g.OrderByDescending(e => e.CreatedTime).First()).ToList();
                reader.Close();
            }
        }

        private List<AttendanceData> GetEmployeeAndAttendanceDataByDateTime(DateTime select)
        {
            return attendanceDatas.Where(a => a.ClockTime.Year == select.Year &&
                                              a.ClockTime.Month == select.Month).ToList();
        }

        private List<AttendanceData> GetEmployeeAndAttendanceDataByDateTime(DateTime start, DateTime end)
        {
            return attendanceDatas.Where(a => (DateTime.Compare(a.ClockTime, start) >= 0) && 
                                              (DateTime.Compare(a.ClockTime, end) <= 0)).ToList();
        }

        private List<Employee> GetTypeDatas(int year, int month, int Type)
        {
            DateTime selectTime = new DateTime(year, month, 1);
            List<BakUseData> AvabUseDatas = Employees.Where(u => u.CreatedTime <= selectTime)
                                                     .ToList();
            List<Employee> statistics = new List<Employee>();
            int days = DateTimeHelper.GetDays(year, month);
            foreach (var employee in AvabUseDatas)
            {
                Employee sheet;
                if (Type == 0)
                    sheet = new StatisticsData();
                else if (Type == 1)
                    sheet = new SummaryData();
                else if (Type == 2)
                    sheet = new ExceptionData();
                else
                    sheet = new OriginalData();
                List<AttendanceData> AvabDatas = GetEmployeeAndAttendanceDataByDateTime(selectTime)//获取对应时间的数据
                                                                        .Where(a => a.UserIndex == employee.Index)
                                                                        .Where(a => a.ClockTime >= employee.CreatedTime)
                                                                        .ToList();
                if (AvabDatas.Count() == 0)
                    continue;
                AttendanceRule rule;
                int week = 0, hour = 0, min = 0, lateMin = 0, lateNum = 0, overH = 0, overM = 0;
                int Dlate = 0;               
                ExceptionPart part = null;
                TimeSpan start, end, total, overTime;
                if (AvabDatas.FirstOrDefault().Class >= 0 && AvabDatas.FirstOrDefault().Class < Rules.RuleList.Count())
                    rule = Rules.RuleList.Find(r => r.SerialNumber == AvabDatas.FirstOrDefault().Class);
                else
                    rule = Rules.DefaultRule;
                //是否跨天
                if(rule.Inter_dayTime != TimeSpan.Zero)
                {
                    DateTime et = selectTime.AddMonths(1)
                                            .AddHours(rule.Inter_dayTime.Hours)
                                            .AddMinutes(rule.Inter_dayTime.Minutes);
                    //更新数据
                    AvabDatas = GetEmployeeAndAttendanceDataByDateTime(selectTime, et)//获取对应时间的数据
                                                                        .Where(a => a.UserIndex == employee.Index)
                                                                        .Where(a => a.ClockTime >= employee.CreatedTime)
                                                                        .ToList(); 
                }
                //姓名
                sheet.Name = employee.Name;
                //工号
                sheet.Id = employee.Id;
                //部门
                sheet.Department = "公司";
                //班次
                sheet.RuleName = rule.RuleName;
                if (Type == 3)
                {
                    ((OriginalData)sheet).Datas = new List<DateTime>[days];
                    for (int i = 0; i < days; i++)
                    {
                        ((OriginalData)sheet).Datas[i] = new List<DateTime>();
                        foreach (var item in AvabDatas.Where(a => a.ClockTime.Day == i + 1))
                            ((OriginalData)sheet).Datas[i].Add(item.ClockTime);
                    }
                    statistics.Add(sheet);
                    continue;
                }
                if (Type == 0 || Type == 1)
                {
                    var dData = AvabDatas.GroupBy(a => a.ClockTime.Day);//通过日期进行分组
                    //实际出勤
                    ((Sum_Stati_transit)sheet).AtlAtd = dData.Count().ToString();
                    //标准
                    ((Sum_Stati_transit)sheet).StdAtd = days.ToString();
                }               
                if (Type == 0)
                    ((StatisticsData)sheet).DaysOfWeek = DateTimeHelper.GetDaysByWeek(year, month);
                for (int i = 0; i < days; i++)
                {
                    DateTime today = new DateTime(year, month,  i + 1);
                    //选择当天的打卡数据                             
                    //清洗数据，如果一个时间段有多次打卡，选择最早的记录
                    List<AttendanceData?>? dayData = null;
                    if (rule.Inter_dayTime == TimeSpan.Zero)//是否跨天
                    {
                        dayData = AvabDatas.Where(a => a.ClockTime.Day == i + 1)//找到当天的数据记录
                                           .GroupBy(a => a.ShiftClass)//通过班次分组
                                           .Select(g => g.OrderBy(a => a.ClockTime).FirstOrDefault())//对每个分组进行时间排列，选择最早的记录
                                           .OrderBy(a => a.ClockTime)//对已选择的记录再进行时间排列
                                           .ToList();
                    }
                    else
                    {
                        DateTime s = new DateTime(year, month, i + 1, rule.Inter_dayTime.Hours, rule.Inter_dayTime.Minutes, 0);                                          
                        DateTime e = s.AddDays(1);
                        dayData = AvabDatas.Where(a => (DateTime.Compare(a.ClockTime, s) >= 0) && (DateTime.Compare(a.ClockTime, e) <= 0))//找到当天的数据记录
                                           .GroupBy(a => a.ShiftClass)//通过班次分组
                                           .Select(g => g.OrderBy(a => a.ClockTime).FirstOrDefault())//对每个分组进行时间排列，选择最早的记录
                                           .OrderBy(a => a.ClockTime)//对已选择的记录再进行时间排列
                                           .ToList();
                    }

                    if (dayData.Count() == 0)
                        continue;
                    week = DateTimeHelper.GetWeek(year, month, i + 1);
                    start = TimeSpan.Zero;
                    end = TimeSpan.Zero;
                    total = TimeSpan.Zero;
                    overTime = TimeSpan.Zero;
                    Dlate = 0;
                    for (int k = 0; k < 6; k++)
                    {
                        var att = dayData.Find(a => a.ShiftClass == (ShiftClass)k);
                        if (att != null)
                        {
                            if (Type == 0)
                                ((StatisticsData)sheet).SignUpDatas[i][k].Text = att.ClockTime.ToString("HH:mm");                                                           
                            //从规定的标准中，选择对应星期的班次
                            ClassSection s = rule.Classes[week][k / 2];
                            TimeSpan t, span;
                            span = att.ClockTime - today;//获取当天从零点的时间差值
                            if (k % 2 == 0)
                            {
                                t = s.StartTime + new TimeSpan(0, rule.StatsUnit + rule.AllowLate, 0);                                
                                //比较，选择正确的时间段。迟到
                                if (span > t)
                                {                                   
                                    if (Type == 0)
                                        ((StatisticsData)sheet).SignUpDatas[i][k].Color = Color.Red;
                                    else if (Type == 2)
                                    {
                                        if(part == null)
                                            part = new ExceptionPart();                                        
                                        part.ESignUpDatas[k] = att.ClockTime.ToString("HH:mm");                                       
                                    }
                                    start = span - new TimeSpan(0, rule.StatsUnit + rule.AllowLate, 0);
                                    Dlate += (int)(span - t).TotalMinutes;
                                    lateNum++;
                                }
                                else
                                {
                                    start = s.StartTime;
                                }
                            }
                            else
                            {
                                t = s.EndTime - new TimeSpan(0, rule.StatsUnit + rule.AllowEarly, 0);
                                //比较，选择正确的时间段。早退
                                if (span < t)
                                {                                    
                                    if (Type == 0)
                                        ((StatisticsData)sheet).SignUpDatas[i][k].Color = Color.Red;
                                    else if (Type == 2)
                                    {
                                        if (part == null)
                                            part = new ExceptionPart();                                                                                    
                                        part.ESignUpDatas[k] = att.ClockTime.ToString("HH:mm");
                                    }
                                    end = span + new TimeSpan(0, rule.StatsUnit + rule.AllowEarly, 0);
                                    Dlate += (int)(t - span).TotalMinutes;
                                    lateNum++;
                                }
                                else
                                {
                                    end = s.EndTime;
                                }
                                //时间段不全或者后者小于前者，则不计算
                                if (end != TimeSpan.Zero && start != TimeSpan.Zero && end > start)
                                {
                                    if (s.Type == 0)//正常
                                        total += end - start;
                                    else if (s.Type == 1)//加班
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
                        part.Date = string.Format("{0:00}-{1:00}", month, i + 1);                       
                        part.LateOrEarly = string.Format("{0:00}:{1:00}", Dlate / 60, Dlate % 60);                      
                        ((ExceptionData)sheet).Parts.Add(part);
                        part = null;
                    }
                    lateMin += Dlate;
                    if (total != TimeSpan.Zero)
                    {                        
                        hour += total.Hours;
                        min += total.Minutes;   
                        if(Type == 0)
                            ((StatisticsData)sheet).SignUpDatas[i][6].Text =total.ToString().Substring(0, 5);                         
                    }
                    if (overTime != TimeSpan.Zero)
                    {
                        overH += overTime.Hours;
                        overM += overTime.Minutes;   
                        if(Type == 0)
                            ((StatisticsData)sheet).SignUpDatas[i][7].Text = total.ToString().Substring(0, 5);                         
                    }
                }
                //日期
                if (Type == 0)
                    ((StatisticsData)sheet).Date = string.Format($"{year}-{month.ToString("00")}");
                if (Type == 0 || Type == 1)
                {
                    ((Sum_Stati_transit)sheet).AtlWorkTime = string.Format("{0:00}:{1:00}", hour + min / 60, min % 60);
                    ((Sum_Stati_transit)sheet).StdWorkTime = rule.GetStdTimeStr(year, month);
                    ((Sum_Stati_transit)sheet).Wko_Common = string.Format("{0:00}:{1:00}", overH + overM / 60, overM % 60);
                    ((Sum_Stati_transit)sheet).Wko_Special = string.Format("{0:00}:{1:00}", 0, 0);
                    ((Sum_Stati_transit)sheet).LateEarly_Count = lateNum.ToString();
                    ((Sum_Stati_transit)sheet).LateEarly_Min = lateMin.ToString();
                }               
                statistics.Add(sheet);
            }           
            return statistics.OrderBy(s => s.Id).ToList();
        }

        private List<StatisticsData> GetStatisticsDatas(int year, int month)
        {
            return GetTypeDatas(year, month, 0).Select(s => (StatisticsData)s).ToList();
        }

        public StatisticsSheetModel GetStatisticsSheetModel(int year, int month)
        {
            StatisticsSheetModel sheetModel = new StatisticsSheetModel();
            sheetModel.Datas = GetStatisticsDatas(year, month);
            return sheetModel;
        }

        private List<SummaryData> GetSummaryDatas(int year, int month)
        {           
            return GetTypeDatas(year, month, 1).Select(s => (SummaryData)s).ToList();
        }

        public SummarySheetModel GetSummarySheetModel(int year, int month)
        {
            SummarySheetModel summarySheetModel = new SummarySheetModel();
            summarySheetModel.Date = string.Format($"{year}-{month.ToString("00")}");
            summarySheetModel.Datas = GetTypeDatas(year, month, 1).Select(s => (SummaryData)s).ToList();
            return summarySheetModel;
        }

        private List<ExceptionData> GetExceptionDatas(int year, int month)
        {
            return GetTypeDatas(year, month, 2).Select(s => (ExceptionData)s).ToList();
        }

        public ExceptionSheetModel GetExceptionSheetModel(int year, int month)
        {
            ExceptionSheetModel sheetModel = new ExceptionSheetModel();
            sheetModel.Date = string.Format($"{year}-{month.ToString("00")}");
            sheetModel.Datas = GetExceptionDatas(year, month);
            return sheetModel;
        }

        private List<OriginalData> GetOriginalDatas(int year, int month)
        {
            return GetTypeDatas(year, month, 3).Select(s => (OriginalData)s).ToList();
        }

        public OriginalSheetModel GetOriginalSheetModel(int year, int month)
        {
            OriginalSheetModel sheetModel = new OriginalSheetModel();
            sheetModel.Date = new DateTime(year, month, DateTimeHelper.GetDays(year, month));
            sheetModel.Datas = GetOriginalDatas(year, month);
            return sheetModel;
        }

    }
}
