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
using TimeTrack_Pro.Model;

namespace TimeTrack_Pro.Code
{
    public class BakDatasHandle
    {
        private List<AttendanceData> attendanceDatas;
        public List<AttendanceData> AttendanceDatas { get { return attendanceDatas; } }

        private List<BakUseData> employees;
        public List<BakUseData> Employees { get { return employees; } }


        public BakDatasHandle(string attendanceFile, string employeeFile)
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
                        Debug.WriteLine(e.Message);
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

        public List<AttendanceData> GetEmployeeAndAttendanceDataByDateTime(DateTime selectTime)
        {
            return attendanceDatas.Where(a => a.ClockTime.Year == selectTime.Year && a.ClockTime.Month == selectTime.Month).ToList();
        }

        public List<StatisticsData> GetStatisticsDatas(int year, int month)
        {
            DateTime selectTime = new DateTime(year, month, 1);
            List<BakUseData> AvabUseDatas = Employees.Where(u => u.CreatedTime <= selectTime)
                                                     .ToList();
            List<StatisticsData> statistics = new List<StatisticsData>();               
            foreach (var employee in AvabUseDatas)
            {
                StatisticsData sheet = new StatisticsData();                
                List<AttendanceData> AvabDatas = GetEmployeeAndAttendanceDataByDateTime(selectTime)//获取对应时间的数据
                                                                        .Where(a => a.UserIndex == employee.Index)
                                                                        .Where(a => a.ClockTime >= employee.CreatedTime)
                                                                        .ToList();
                if (AvabDatas.Count() == 0)
                    continue;
                AttendanceRule rule;
                int week = 0, hour = 0, min = 0, lateMin = 0, lateNum = 0, overH = 0, overM = 0;
                int days = GetDays(month), stdH = 0, stdM = 0;
                TimeSpan start, end, total, overTime;
                if (AvabDatas.FirstOrDefault().Class >= 0 && AvabDatas.FirstOrDefault().Class < Rules.RuleList.Count())
                    rule = Rules.RuleList.Find(r => r.SerialNumber == AvabDatas.FirstOrDefault().Class);
                else
                    rule = Rules.DefaultRule;
                //姓名
                sheet.Name = employee.Name;
                //工号
                sheet.Id = employee.Id;
                //部门
                sheet.Department = "";
                //班次
                sheet.RuleName = rule.RuleName;
                //日期
                sheet.Date = string.Format($"{year}-{month.ToString("00")}");
                var dData = AvabDatas.GroupBy(a => a.ClockTime.Day);//通过日期进行分组
                //实际出勤
                sheet.AtlAtd = dData.Count().ToString();
                //标准
                sheet.StdAtd = days.ToString();                
                for (int i = 0; i <= days; i++)
                {                    
                    //选择当天的打卡数据                             
                    //清洗数据，如果一个时间段有多次打卡，选择最早的记录
                    var dayData = AvabDatas.Where(a => a.ClockTime.Day == i + 1)//找到当天的数据记录
                                            .GroupBy(a => a.ShiftClass)//通过班次分组
                                            .Select(g => g.OrderBy(a => a.ClockTime).FirstOrDefault())//对每个分组进行时间排列，选择最早的记录
                                            .OrderBy(a => a.ClockTime)//对已选择的记录再进行时间排列
                                            .ToList();                                    
                    if (dayData.Count() == 0)
                        continue;
                    week = GetWeek(month, i + 1);
                    start = TimeSpan.Zero;
                    end = TimeSpan.Zero;
                    total = TimeSpan.Zero;
                    overTime = TimeSpan.Zero;
                    for (int k = 0; k < 6; k++)
                    {
                        var att = dayData.Find(a => a.ShiftClass == (ShiftClass)k);
                        if (att != null)
                        {                            
                            sheet.SignUpDatas[i][k].Text = att.ClockTime.ToString("HH:mm");
                            //从规定的标准中，选择对应星期的班次
                            ClassSection s = rule.Classes[week][k / 2];
                            TimeSpan t;
                            if (k % 2 == 0)
                            {
                                t = s.StartTime + new TimeSpan(0, rule.StatsUnit + rule.AllowLate, 0);
                                //比较，选择正确的时间段。迟到
                                if (att.ClockTime.TimeOfDay > t)
                                {
                                    sheet.SignUpDatas[i][k].Color = Color.Red;
                                    start = att.ClockTime.TimeOfDay - new TimeSpan(0, rule.StatsUnit + rule.AllowLate, 0);
                                    lateMin += (int)(att.ClockTime.TimeOfDay - t).TotalMinutes;
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
                                if (att.ClockTime.TimeOfDay < t)
                                {
                                    sheet.SignUpDatas[i][k].Color = Color.Red;
                                    end = att.ClockTime.TimeOfDay + new TimeSpan(0, rule.StatsUnit + rule.AllowEarly, 0);
                                    lateMin += (int)(t - att.ClockTime.TimeOfDay).TotalMinutes;
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
                    if (total != TimeSpan.Zero)
                    {                        
                        hour += total.Hours;
                        min += total.Minutes;                        
                        sheet.SignUpDatas[i][6].Text =total.ToString().Substring(0, 5);                         
                    }
                    if (overTime != TimeSpan.Zero)
                    {
                        overH += overTime.Hours;
                        overM += overTime.Minutes;                        
                        sheet.SignUpDatas[i][7].Text = total.ToString().Substring(0, 5);                         
                    }
                }
                sheet.AtlWorkTime = string.Format($"{hour + min / 60}:{min % 60}");
                for (int k = 1; k <= GetDays(month); k++)
                {
                    foreach (var s in rule.Classes[GetWeek(month, k)])
                    {
                        if (s.Type == 0 && s.StartTime != TimeSpan.Zero && s.EndTime != TimeSpan.Zero && s.StartTime < s.EndTime)
                        {
                            var time = s.EndTime - s.StartTime;
                            stdH += time.Hours;
                            stdM += time.Minutes;
                        }
                    }
                }
                sheet.StdWorkTime = string.Format($"{stdH + stdM / 60}:{stdM % 60}");
                sheet.Wko_Common = string.Format($"{overH + overM / 60}:{overM % 60}");
                sheet.Wko_Special = "00:00";
                sheet.LateEarly_Count = lateNum.ToString();
                sheet.LateEarly_Min = lateMin.ToString();
                statistics.Add(sheet);
            }           
            return statistics.OrderBy(s => s.Id).ToList();
        }

        public List<SummaryData> GetSummaryDatas(int year, int month)
        {
            List<SummaryData> summaries = new List<SummaryData>();

            return summaries;
        }

        public List<ExceptionData> GetExceptionDatas(int year, int month)
        {
            List<ExceptionData> exceptions = new List<ExceptionData>();

            return exceptions;
        }

        public List<OriginalData> GetOriginalDatas(int year, int month)
        {
            List<OriginalData> originals = new List<OriginalData>();

            return originals;
        }

        /// <summary>
        /// 获取当前月的天数
        /// </summary>
        /// <returns></returns>
        private int GetDays()
        {
            // 获取当前月份第一天
            DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            // 获取下个月的第一天
            DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);
            // 获取本月最后一天
            DateTime lastDayOfMonth = firstDayOfNextMonth.AddDays(-1);
            // 获取本月的天数
            int daysInMonth = lastDayOfMonth.Day;
            return daysInMonth;
        }

        /// <summary>
        /// 获取今年某月的天数
        /// </summary>
        /// <returns></returns>
        private int GetDays(int month)
        {
            month = ((month > 12) || (month < 1)) ? 1 : month;
            // 获取当前月份第一天
            DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, month, 1);
            // 获取下个月的第一天
            DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);
            // 获取本月最后一天
            DateTime lastDayOfMonth = firstDayOfNextMonth.AddDays(-1);
            // 获取本月的天数
            int daysInMonth = lastDayOfMonth.Day;
            return daysInMonth;
        }

        /// <summary>
        /// 获取当月某天的星期
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public int GetWeek(int day)
        {
            day = (day < 0 || day > 31) ? 1 : day;
            DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, day);
            return (int)firstDayOfMonth.DayOfWeek;
        }

        /// <summary>
        ///  获取某月某天的星期
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public int GetWeek(int month, int day)
        {
            month = (month < 0 || month > 12) ? DateTime.Today.Month : month;
            day = (day < 0 || day > 31) ? 1 : day;
            DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, month, day);
            return (int)firstDayOfMonth.DayOfWeek;
        }

        /// <summary>
        /// 获取当月天数并按星期进行排列的集合
        /// </summary>
        /// <returns></returns>
        private string[] GetDaysByWeek()
        {
            // 获取当前月份第一天
            DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            // 获取这一天时星期几
            DayOfWeek dayOfWeek = firstDayOfMonth.DayOfWeek;
            // 获取下个月的第一天
            DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);
            // 获取本月最后一天
            DateTime lastDayOfMonth = firstDayOfNextMonth.AddDays(-1);
            // 获取本月的天数
            int daysInMonth = lastDayOfMonth.Day;
            string[] days = new string[daysInMonth];
            string[] weeks = { "日", "一", "二", "三", "四", "五", "六" };
            for (int i = 0; i < daysInMonth; i++)
            {
                days[i] = string.Format("{0:00}", i + 1) + " " + weeks[((int)dayOfWeek + i) % 7];
            }
            return days;
        }

        /// <summary>
        /// 获取某月天数并按星期进行排列的集合
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        private string[] GetDaysByWeek(int month)
        {
            month = (month > 12 || month < 1) ? DateTime.Today.Month : month;
            // 获取当前月份第一天
            DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, month, 1);
            // 获取这一天时星期几
            DayOfWeek dayOfWeek = firstDayOfMonth.DayOfWeek;
            // 获取下个月的第一天
            DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);
            // 获取本月最后一天
            DateTime lastDayOfMonth = firstDayOfNextMonth.AddDays(-1);
            // 获取本月的天数
            int daysInMonth = lastDayOfMonth.Day;
            string[] days = new string[daysInMonth];
            string[] weeks = { "日", "一", "二", "三", "四", "五", "六" };
            for (int i = 0; i < daysInMonth; i++)
            {
                days[i] = string.Format("{0:00}", i + 1) + " " + weeks[((int)dayOfWeek + i) % 7];
            }
            return days;
        }
    }
}
