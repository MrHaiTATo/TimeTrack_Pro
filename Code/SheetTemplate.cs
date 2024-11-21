using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TimeTrack_Pro.Model;
using static TimeTrack_Pro.Demo.SmartDemo;
using System.Data;
using Org.BouncyCastle.Asn1.Pkcs;

namespace TimeTrack_Pro.Code
{
    public class SheetTemplate
    {
        public static string? FilePath { get; set; }

        public static readonly AttendanceRule DefaultRule = new AttendanceRule
        {
            RuleName = "白班",
            Inter_dayTime = new TimeSpan(0, 0, 0),
            SerialNumber = 0,
            AlarmsTimes = 6,
            AttendanceWay = 0,
            StatsUnit = 0,
            StatsWay = 0,
            ShiftMode = 0,
            AllowLate = 0,
            AllowEarly = 0,
            Classes = new ClassSection[][] { 
                /*星期日*/
                new ClassSection[3] {
                    new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                    new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(18, 0, 0), Type = 0 },
                    new ClassSection { Name = "班段3", StartTime = new TimeSpan(18, 30, 0), EndTime = new TimeSpan(20, 30, 0), Type = 1 }
                },
                /*星期一*/
                new ClassSection[3] {
                    new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                    new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(18, 0, 0), Type = 0 },
                    new ClassSection { Name = "班段3", StartTime = new TimeSpan(18, 30, 0), EndTime = new TimeSpan(20, 30, 0), Type = 1 }
                },
                /*星期二*/
                new ClassSection[3] {
                    new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                    new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(18, 0, 0), Type = 0 },
                    new ClassSection { Name = "班段3", StartTime = new TimeSpan(18, 30, 0), EndTime = new TimeSpan(20, 30, 0), Type = 1 }
                },
                /*星期三*/
                new ClassSection[3] {
                    new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                    new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(18, 0, 0), Type = 0 },
                    new ClassSection { Name = "班段3", StartTime = new TimeSpan(18, 30, 0), EndTime = new TimeSpan(20, 30, 0), Type = 1 }
                },
                /*星期四*/
                new ClassSection[3] {
                    new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                    new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(18, 0, 0), Type = 0 },
                    new ClassSection { Name = "班段3", StartTime = new TimeSpan(18, 30, 0), EndTime = new TimeSpan(20, 30, 0), Type = 1 }
                },
                /*星期五*/
                new ClassSection[3] {
                    new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                    new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(18, 0, 0), Type = 0 },
                    new ClassSection { Name = "班段3", StartTime = new TimeSpan(18, 30, 0), EndTime = new TimeSpan(20, 30, 0), Type = 1 }
                },
                /*星期六*/
                new ClassSection[3] {
                    new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                    new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(18, 0, 0), Type = 0 },
                    new ClassSection { Name = "班段3", StartTime = new TimeSpan(18, 30, 0), EndTime = new TimeSpan(20, 30, 0), Type = 1 }
                }
            }
        };

        /// <summary>
        /// 创建考勤统计表模板
        /// </summary>
        public static void CreateAttendanceStatisticsSheet(BakDatasHandle center)
        {
            // 在 Excel 包类上使用许可证上下文属性
            // 删除许可证异常
            // 必须设置，否则会报错
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//非商业

            //创建一个新的Excel包
            using (ExcelPackage package = new ExcelPackage())
            {
                DateTime select = new DateTime(2024, 8, 1);
                var datas = center.GetEmployeeAndAttendanceDataByDateTime(select);
                foreach (var employee in center.Employees)
                {
                    var Eattendances = datas.Where(e => e.UserIndex == employee.Index).ToList();
                    if (Eattendances.Count() == 0)
                        continue;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(employee.Name + "_" + employee.Id);
                    AttendanceRule rule;
                    if (Eattendances.FirstOrDefault().Class < Rules.RuleList.Count())
                        rule = Rules.RuleList.Find(r => r.SerialNumber == Eattendances.FirstOrDefault().Class);
                    else
                        rule = DefaultRule;
                    //应用样式
                    CreateAttendanceStatisticsSheet(worksheet);
                    //填充数据
                    FillAttendanceStatisticsSheet(worksheet, employee, Eattendances, rule);
                }
                //保存Excel文件
                FileInfo file = new FileInfo(@"F:\文档\考勤统计表.xlsx");
                package.SaveAs(file);
            }
        }

        public static void CreateAttendanceStatisticsSheet(List<StatisticsData> statistics)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//非商业
            using (ExcelPackage package = new ExcelPackage())
            {
                foreach (var data in statistics)
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(data.Name + "_" + data.Id);
                    //应用样式
                    CreateAttendanceStatisticsSheet(worksheet, data);
                }
                //保存Excel文件
                FileInfo file = new FileInfo(@"F:\文档\考勤统计表.xlsx");
                package.SaveAs(file);
            }
        }

        public static void CreateAttendanceStatisticsSheet(ExcelWorksheet worksheet)
        {
            string[] columns = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S" };
            (string, string)[] values;
            for (int i = 1; i < 19; i++)
            {
                worksheet.Columns[i].Width = 7;
            }
            worksheet.Rows[1].Height = 6.75;
            worksheet.Rows[2].Height = 21;
            worksheet.Rows[6].Height = 6.75;
            worksheet.Rows[7].Height = 19.5;
            worksheet.Rows[26].Height = 6.75;
            worksheet.Columns[19].Width = 1.5;
            worksheet.Cells["A1:S26"].Style.Numberformat.Format = "@";

            // 第一行
            SetMergeCellsStyle(worksheet, "A1:R1");
            worksheet.Cells["A1:R1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["A1:R1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            SetBorderCellStyle(worksheet.Cells["A1:R1"], ExcelBorderStyle.Thin, ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
            SetBorderColor(worksheet.Cells["A1:R1"], Color.Blue, Color.Empty, Color.Blue, Color.Empty);

            // 第二行
            SetGeneral1_1(worksheet.Cells["A2"], 10);
            worksheet.Cells["A2"].Value = "姓名";
            SetBorderCellStyle(worksheet.Cells["A2"], ExcelBorderStyle.None, ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
            SetBorderColor(worksheet.Cells["A2"], Color.Empty, Color.Empty, Color.Blue, Color.Empty);
            SetMergeCellsStyle(worksheet, "B2:D2");
            SetGeneral1_3(worksheet.Cells["B2:D2"], 10);

            SetGeneral1_1(worksheet.Cells["E2"], 10);
            worksheet.Cells["E2"].Value = "工号";
            SetMergeCellsStyle(worksheet, "F2:G2");
            SetGeneral1_3(worksheet.Cells["F2:G2"], 10);

            SetGeneral1_1(worksheet.Cells["H2"], 10);
            worksheet.Cells["H2"].Value = "部门";
            SetMergeCellsStyle(worksheet, "I2:K2");
            SetGeneral1_3(worksheet.Cells["I2:K2"], 10);
            worksheet.Cells["I2:K2"].Value = "公司";

            SetGeneral1_1(worksheet.Cells["L2"], 10);
            worksheet.Cells["L2"].Value = "班次";
            SetMergeCellsStyle(worksheet, "M2:O2");
            SetGeneral1_3(worksheet.Cells["M2:O2"], 10);

            SetGeneral1_1(worksheet.Cells["P2"], 10);
            worksheet.Cells["P2"].Value = "日期";
            SetMergeCellsStyle(worksheet, "Q2:R2");
            SetGeneral1_3(worksheet.Cells["Q2:R2"], 10);
            worksheet.Cells["Q2:R2"].Value = "2024-08";

            //第三、四行
            values = new (string, string)[] { ("A3:B3", "出勤(天)"), ("C3:D3", "工作时间(时分)"), ("E3:F3", "加班(时分)"), ("G3:H3", "迟到/早退"),
                                                  ("I3:J3", "请假(时分)"), ("K3:K4", "旷工(时分)"), ("L3:L4", "出差(时分)") };
            foreach (var (position, content) in values)
            {
                SetMergeCellsStyle(worksheet, position);
                SetGeneral1_1(worksheet.Cells[position], 10);
                SetBorderCellStyle(worksheet.Cells[position], ExcelBorderStyle.Thin);
                SetBorderColor(worksheet.Cells[position], Color.Blue);
                worksheet.Cells[position].Value = content;
            }

            SetMergeCellsStyle(worksheet, "M3:R3");
            SetGeneral1_1(worksheet.Cells["M3:R3"], 10);
            SetBorderCellStyle(worksheet.Cells["M3:R3"], ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
            SetBorderColor(worksheet.Cells["M3:R3"], Color.Blue, Color.Blue, Color.Blue, Color.Empty);
            worksheet.Cells["M3:R3"].Value = "工资";

            values = new (string, string)[] { ("A4","实际"), ("B4", "标准"), ("C4", "实际"), ("D4", "标准"), ("E4", "普通"), ("F4", "特殊"), ("G4", "次"),
                                                  ("H4", "分"), ("I4", "带薪假"), ("J4", "无薪假"), ("M4", "日薪"), ("N4", "加班"), ("O4", "扣款"), ("P4", "其他") };
            foreach (var (position, content) in values)
            {
                SetMergeCellsStyle(worksheet, position);
                SetGeneral1_1(worksheet.Cells[position], 10);
                SetBorderCellStyle(worksheet.Cells[position], ExcelBorderStyle.Thin);
                SetBorderColor(worksheet.Cells[position], Color.Blue);
                worksheet.Cells[position].Value = content;
            }

            SetMergeCellsStyle(worksheet, "Q4:R4");
            SetGeneral1_1(worksheet.Cells["Q4:R4"], 10);
            SetBorderCellStyle(worksheet.Cells["Q4:R4"], ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
            SetBorderColor(worksheet.Cells["Q4:R4"], Color.Blue, Color.Blue, Color.Blue, Color.Empty);
            worksheet.Cells["Q4:R4"].Value = "合计";

            //第五行
            foreach (var item in columns)
            {
                if (item == "Q" || item == "R" || item == "S")
                    continue;
                SetGeneral1_3(worksheet.Cells[$"{item}5"], 10);
                SetBorderCellStyle(worksheet.Cells[$"{item}5"], ExcelBorderStyle.Thin, ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin);
                SetBorderColor(worksheet.Cells[$"{item}5"], Color.Blue, Color.Empty, Color.Blue, Color.Blue);
            }
            SetMergeCellsStyle(worksheet, "Q5:R5");
            SetGeneral1_3(worksheet.Cells["Q5:R5"], 10);
            SetBorderCellStyle(worksheet.Cells["Q5:R5"], ExcelBorderStyle.Thin, ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
            SetBorderColor(worksheet.Cells["Q5:R5"], Color.Blue, Color.Empty, Color.Blue, Color.Empty);

            //第六行
            SetMergeCellsStyle(worksheet, "A6:R6");
            worksheet.Cells["A6:R6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["A6:R6"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            SetBorderCellStyle(worksheet.Cells["A6:R6"], ExcelBorderStyle.None, ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
            SetBorderColor(worksheet.Cells["A6:R6"], Color.Empty, Color.Empty, Color.Blue, Color.Empty);

            //第七行
            SetMergeCellsStyle(worksheet, "A7:R7");
            SetGeneral1_1(worksheet.Cells["A7:R7"], 14);
            worksheet.Cells["A7:R7"].Value = "考\x20\x20\x20\x20\x20勤\x20\x20\x20\x20\x20表";
            worksheet.Cells["A7:R7"].Style.Font.Bold = true;
            SetBorderCellStyle(worksheet.Cells["A7:R7"], ExcelBorderStyle.None, ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
            SetBorderColor(worksheet.Cells["A7:R7"], Color.Empty, Color.Empty, Color.Blue, Color.Empty);

            //第八到二十五行
            string[] days = GetDaysByWeek(8);
            for (int i = 0; i < 2; i++)
            {
                string seat = $"{(char)('A' + i * 9)}8:{(char)('A' + i * 9)}9";
                SetMergeCellsStyle(worksheet, seat);
                SetGeneral1_1(worksheet.Cells[seat], 10);
                SetBorderCellStyle(worksheet.Cells[seat], ExcelBorderStyle.Thin);
                SetBorderColor(worksheet.Cells[seat], Color.Blue);
                worksheet.Cells[seat].Value = "日\x20星期\x20期";

                values = new (string, string)[] {
                        ($"{(char)('B' + i * 9)}8:{(char)('C' + i * 9)}8", "班段1"),
                        ($"{(char)('D' + i * 9)}8:{(char)('E' + i * 9)}8", "班段2"),
                        ($"{(char)('F' + i * 9)}8:{(char)('G' + i * 9)}8", "班段3")
                    };
                foreach (var (position, content) in values)
                {
                    SetMergeCellsStyle(worksheet, position);
                    SetGeneral1_1(worksheet.Cells[position], 10);
                    SetBorderCellStyle(worksheet.Cells[position], ExcelBorderStyle.Thin);
                    SetBorderColor(worksheet.Cells[position], Color.Blue);
                    worksheet.Cells[position].Value = content;
                }

                values = new (string, string)[] {
                        ($"{(char)('B' + i * 9)}9", "上班"),
                        ($"{(char)('C' + i * 9)}9", "下班"),
                        ($"{(char)('D' + i * 9)}9", "上班"),
                        ($"{(char)('E' + i * 9)}9", "下班"),
                        ($"{(char)('F' + i * 9)}9", "签到"),
                        ($"{(char)('G' + i * 9)}9", "签退")
                    };
                foreach (var (position, content) in values)
                {
                    SetGeneral1_1(worksheet.Cells[position], 10);
                    SetBorderCellStyle(worksheet.Cells[position], ExcelBorderStyle.Thin);
                    SetBorderColor(worksheet.Cells[position], Color.Blue);
                    worksheet.Cells[position].Value = content;
                }

                seat = $"{(char)('H' + i * 9)}8:{(char)('I' + i * 9)}8";
                SetMergeCellsStyle(worksheet, seat);
                SetGeneral1_2(worksheet.Cells[seat], 10);
                SetBorderCellStyle(worksheet.Cells[seat], ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
                SetBorderColor(worksheet.Cells[seat], Color.Blue, Color.Blue, Color.Blue, Color.Empty);
                worksheet.Cells[seat].Value = "日\x20\x20统\x20\x20计";

                values = new (string, string)[] {
                        ($"{(char)('H' + i * 9)}9", "工作"),
                        ($"{(char)('I' + i * 9)}9", "加班")
                    };
                foreach (var (position, content) in values)
                {
                    SetGeneral1_2(worksheet.Cells[position], 10);
                    SetBorderCellStyle(worksheet.Cells[position], ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
                    SetBorderColor(worksheet.Cells[position], Color.Blue, Color.Blue, Color.Blue, Color.Empty);
                    worksheet.Cells[position].Value = content;
                }

                for (int j = 0; j < 16; j++)
                {
                    seat = $"{(char)('A' + i * 9)}{10 + j}";
                    SetGeneral1_1(worksheet.Cells[seat], 10);
                    SetBorderCellStyle(worksheet.Cells[seat], ExcelBorderStyle.Thin);
                    SetBorderColor(worksheet.Cells[seat], Color.Blue);
                    if (j + i * 16 < days.Count())
                    {
                        worksheet.Cells[seat].Value = days[j + i * 16];
                        if (days[j + i * 16].Contains("日") || days[j + i * 16].Contains("六"))
                            worksheet.Cells[seat].Style.Font.Color.SetColor(Color.Red);
                    }

                    for (int k = 0; k < 6; k++)
                    {
                        seat = $"{(char)('B' + i * 9 + k)}{10 + j}";
                        SetGeneral1_3(worksheet.Cells[seat], 10);
                        SetBorderCellStyle(worksheet.Cells[seat], ExcelBorderStyle.Thin);
                        SetBorderColor(worksheet.Cells[seat], Color.Blue);
                    }

                    for (int k = 0; k < 2; k++)
                    {
                        seat = $"{(char)('H' + i * 9 + k)}{10 + j}";
                        SetGeneral1_2(worksheet.Cells[seat], 10);
                        SetBorderCellStyle(worksheet.Cells[seat], ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
                        SetBorderColor(worksheet.Cells[seat], Color.Blue, Color.Blue, Color.Blue, Color.Empty);
                    }
                }
            }

            //最右边
            SetMergeCellsStyle(worksheet, "S1:S26");
            worksheet.Cells["S1:S26"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["S1:S26"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            SetBorderCellStyle(worksheet.Cells["S1:S26"], ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.None, ExcelBorderStyle.Thin);
            SetBorderColor(worksheet.Cells["S1:S26"], Color.Blue, Color.Blue, Color.Empty, Color.Blue);

            //最下边
            SetMergeCellsStyle(worksheet, "A26:R26");
            worksheet.Cells["A26:R26"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["A26:R26"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            SetBorderCellStyle(worksheet.Cells["A26:R26"], ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
            SetBorderColor(worksheet.Cells["A26:R26"], Color.Empty, Color.Blue, Color.Blue, Color.Empty);
        }

        public static void FillAttendanceStatisticsSheet(ExcelWorksheet worksheet, BakUseData employee, List<AttendanceData> attendances, AttendanceRule attendanceRule)
        {
            AttendanceRule rule = attendanceRule;
            List<AttendanceData?> dayData;
            //姓名
            worksheet.Cells["B2:D2"].Value = employee.Name;
            //工号
            worksheet.Cells["F2:G2"].Value = employee.Id;
            //部门
            worksheet.Cells["I2:K2"].Value = "公司";
            //班次
            worksheet.Cells["M2:O2"].Value = rule.RuleName;
            //日期
            worksheet.Cells["Q2:R2"].Value = "2024-08";
            var dData = attendances.GroupBy(a => a.ClockTime.Day);//通过日期进行分组
            //实际出勤
            worksheet.Cells["A5"].Value = dData.Count();
            //标准
            worksheet.Cells["B5"].Value = GetDays(8);
            int week = 0, hour = 0, min = 0, lateMin = 0, lateNum = 0, overH = 0, overM = 0;
            TimeSpan start, end, total, overTime;
            string seat;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    //选择当天的打卡数据                             
                    //清洗数据，如果一个时间段有多次打卡，选择最早的记录
                    dayData = attendances.Where(a => a.ClockTime.Day == j + 1 + i * 16)//找到当天的数据记录
                                         .GroupBy(a => a.ShiftClass)//通过班次分组
                                         .Select(g => g.OrderBy(a => a.ClockTime).FirstOrDefault())//对每个分组进行时间排列，选择最早的记录
                                         .OrderBy(a => a.ClockTime)//对已选择的记录再进行时间排列
                                         .ToList();
                    week = -1;
                    if (j + i * 16 < GetDays(8))
                        week = GetWeek(8, j + 1 + i * 16);
                    start = TimeSpan.Zero;
                    end = TimeSpan.Zero;
                    total = TimeSpan.Zero;
                    overTime = TimeSpan.Zero;
                    for (int k = 0; k < 6; k++)
                    {
                        seat = $"{(char)('B' + i * 9 + k)}{10 + j}";
                        var att = dayData.Find(a => a.ShiftClass == (ShiftClass)k);
                        if (att != null)
                        {
                            worksheet.Cells[seat].Value = att.ClockTime.ToString("HH:mm");
                            if (week >= 0)
                            {
                                //从规定的标准中，选择对应星期的班次
                                ClassSection s = rule.Classes[week][k / 2];
                                TimeSpan t;
                                if (k % 2 == 0)
                                {
                                    t = s.StartTime + new TimeSpan(0, rule.StatsUnit + rule.AllowLate, 0);
                                    //比较，选择正确的时间段。迟到
                                    if (att.ClockTime.TimeOfDay > t)
                                    {
                                        worksheet.Cells[seat].Style.Font.Color.SetColor(Color.Red);
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
                                        worksheet.Cells[seat].Style.Font.Color.SetColor(Color.Red);
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
                        }
                        else
                        {
                            start = TimeSpan.Zero;
                            end = TimeSpan.Zero;
                        }
                    }

                    for (int k = 0; k < 2; k++)
                    {
                        seat = $"{(char)('H' + i * 9 + k)}{10 + j}";
                        if (k == 0 && total != TimeSpan.Zero)
                        {
                            hour += total.Hours;
                            min += total.Minutes;

                            worksheet.Cells[seat].Value = total.ToString().Substring(0, 5);
                        }
                        if (k == 1 && overTime != TimeSpan.Zero)
                        {
                            overH += overTime.Hours;
                            overM += overTime.Minutes;

                            worksheet.Cells[seat].Value = overTime.ToString().Substring(0, 5);
                        }
                    }
                }
            }
            worksheet.Cells["C5"].Value = string.Format($"{hour + min / 60}:{min % 60}");
            hour = 0; min = 0;
            for (int k = 1; k <= GetDays(8); k++)
            {
                foreach (var s in rule.Classes[GetWeek(8, k)])
                {
                    if (s.Type == 0 && s.StartTime != TimeSpan.Zero && s.EndTime != TimeSpan.Zero && s.StartTime < s.EndTime)
                    {
                        var time = s.EndTime - s.StartTime;
                        hour += time.Hours;
                        min += time.Minutes;
                    }
                }
            }
            worksheet.Cells["D5"].Value = string.Format($"{hour + min / 60}:{min % 60}");
            worksheet.Cells["E5"].Value = string.Format($"{overH + overM / 60}:{overM % 60}");
            worksheet.Cells["F5"].Value = "0:00";
            worksheet.Cells["G5"].Value = lateNum;
            worksheet.Cells["H5"].Value = lateMin;
        }

        public static void CreateAttendanceStatisticsSheet(ExcelWorksheet worksheet, StatisticsData statistic)
        {            
            (string, string)[] values;
            for (int i = 1; i < 19; i++)
            {
                worksheet.Columns[i].Width = 7;
            }
            worksheet.Rows[1].Height = 6.75;
            worksheet.Rows[2].Height = 21;
            worksheet.Rows[6].Height = 6.75;
            worksheet.Rows[7].Height = 19.5;
            worksheet.Rows[26].Height = 6.75;
            worksheet.Columns[19].Width = 1.5;
            worksheet.Cells["A1:S26"].Style.Numberformat.Format = "@";

            // 第一行
            SetMergeCellsStyle(worksheet, "A1:R1");
            worksheet.Cells["A1:R1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["A1:R1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            SetBorderCellStyle(worksheet.Cells["A1:R1"], ExcelBorderStyle.Thin, ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
            SetBorderColor(worksheet.Cells["A1:R1"], Color.Blue, Color.Empty, Color.Blue, Color.Empty);

            // 第二行
            SetGeneral1_1(worksheet.Cells["A2"], 10);
            worksheet.Cells["A2"].Value = "姓名";
            SetBorderCellStyle(worksheet.Cells["A2"], ExcelBorderStyle.None, ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
            SetBorderColor(worksheet.Cells["A2"], Color.Empty, Color.Empty, Color.Blue, Color.Empty);
            SetMergeCellsStyle(worksheet, "B2:D2");
            SetGeneral1_3(worksheet.Cells["B2:D2"], 10);
            worksheet.Cells["B2:D2"].Value = statistic.Name;

            SetGeneral1_1(worksheet.Cells["E2"], 10);
            worksheet.Cells["E2"].Value = "工号";
            SetMergeCellsStyle(worksheet, "F2:G2");
            SetGeneral1_3(worksheet.Cells["F2:G2"], 10);
            worksheet.Cells["F2:G2"].Value = statistic.Id;

            SetGeneral1_1(worksheet.Cells["H2"], 10);
            worksheet.Cells["H2"].Value = "部门";
            SetMergeCellsStyle(worksheet, "I2:K2");
            SetGeneral1_3(worksheet.Cells["I2:K2"], 10);
            worksheet.Cells["I2:K2"].Value = statistic.Department;

            SetGeneral1_1(worksheet.Cells["L2"], 10);
            worksheet.Cells["L2"].Value = "班次";
            SetMergeCellsStyle(worksheet, "M2:O2");
            SetGeneral1_3(worksheet.Cells["M2:O2"], 10);
            worksheet.Cells["M2:O2"].Value = statistic.RuleName;

            SetGeneral1_1(worksheet.Cells["P2"], 10);
            worksheet.Cells["P2"].Value = "日期";
            SetMergeCellsStyle(worksheet, "Q2:R2");
            SetGeneral1_3(worksheet.Cells["Q2:R2"], 10);
            worksheet.Cells["Q2:R2"].Value = statistic.Date;

            //第三、四行
            values = new (string, string)[] { ("A3:B3", "出勤(天)"), ("C3:D3", "工作时间(时分)"), ("E3:F3", "加班(时分)"), ("G3:H3", "迟到/早退"),
                                                  ("I3:J3", "请假(时分)"), ("K3:K4", "旷工(时分)"), ("L3:L4", "出差(时分)") };
            foreach (var (position, content) in values)
            {
                SetMergeCellsStyle(worksheet, position);
                SetGeneral1_1(worksheet.Cells[position], 10);
                SetBorderCellStyle(worksheet.Cells[position], ExcelBorderStyle.Thin);
                SetBorderColor(worksheet.Cells[position], Color.Blue);
                worksheet.Cells[position].Value = content;
            }

            SetMergeCellsStyle(worksheet, "M3:R3");
            SetGeneral1_1(worksheet.Cells["M3:R3"], 10);
            SetBorderCellStyle(worksheet.Cells["M3:R3"], ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
            SetBorderColor(worksheet.Cells["M3:R3"], Color.Blue, Color.Blue, Color.Blue, Color.Empty);
            worksheet.Cells["M3:R3"].Value = "工资";

            values = new (string, string)[] { ("A4","实际"), ("B4", "标准"), ("C4", "实际"), ("D4", "标准"), ("E4", "普通"), ("F4", "特殊"), ("G4", "次"),
                                                  ("H4", "分"), ("I4", "带薪假"), ("J4", "无薪假"), ("M4", "日薪"), ("N4", "加班"), ("O4", "扣款"), ("P4", "其他") };
            foreach (var (position, content) in values)
            {
                SetMergeCellsStyle(worksheet, position);
                SetGeneral1_1(worksheet.Cells[position], 10);
                SetBorderCellStyle(worksheet.Cells[position], ExcelBorderStyle.Thin);
                SetBorderColor(worksheet.Cells[position], Color.Blue);
                worksheet.Cells[position].Value = content;
            }

            SetMergeCellsStyle(worksheet, "Q4:R4");
            SetGeneral1_1(worksheet.Cells["Q4:R4"], 10);
            SetBorderCellStyle(worksheet.Cells["Q4:R4"], ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
            SetBorderColor(worksheet.Cells["Q4:R4"], Color.Blue, Color.Blue, Color.Blue, Color.Empty);
            worksheet.Cells["Q4:R4"].Value = "合计";

            //第五行
            values = new (string, string)[] { 
                 
                ("A5", statistic.AtlAtd)/*出勤，实际*/, ("B5", statistic.StdAtd)/*出勤，标准*/, ("C5", statistic.AtlWorkTime)/*工作时间，实际*/, ("D5", statistic.StdWorkTime)/*工作时间，标准*/, 
                ("E5", statistic.Wko_Common)/*加班，普通*/, ("F5", statistic.Wko_Special)/*加班，特殊*/, ("G5", statistic.LateEarly_Count)/*迟到或早退，次*/, ("H5", statistic.LateEarly_Min)/*迟到或早退，分*/,
                ("I5","")/*请假，带薪假*/, ("J5","")/*请假，无薪假*/, ("K5","")/*旷工*/, ("L5","")/*出差*/,
                ("M5","")/*工资，日薪*/, ("N5","")/*工资，加班*/, ("O5","")/*工资，扣款*/, ("P5","")/*工资，其他*/, 
                ("Q5:R5","")/*工资，合计*/
            };
            foreach (var (position, content) in values)
            {
                SetMergeCellsStyle(worksheet, position);
                SetGeneral1_3(worksheet.Cells[position], 10);
                SetBorderCellStyle(worksheet.Cells[position], ExcelBorderStyle.Thin, ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin);
                SetBorderColor(worksheet.Cells[position], Color.Blue, Color.Empty, Color.Blue, Color.Blue);
                worksheet.Cells[position].Value = content;
            }                        

            //第六行
            SetMergeCellsStyle(worksheet, "A6:R6");
            worksheet.Cells["A6:R6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["A6:R6"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            SetBorderCellStyle(worksheet.Cells["A6:R6"], ExcelBorderStyle.None, ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
            SetBorderColor(worksheet.Cells["A6:R6"], Color.Empty, Color.Empty, Color.Blue, Color.Empty);

            //第七行
            SetMergeCellsStyle(worksheet, "A7:R7");
            SetGeneral1_1(worksheet.Cells["A7:R7"], 14);
            worksheet.Cells["A7:R7"].Value = "考\x20\x20\x20\x20\x20勤\x20\x20\x20\x20\x20表";
            worksheet.Cells["A7:R7"].Style.Font.Bold = true;
            SetBorderCellStyle(worksheet.Cells["A7:R7"], ExcelBorderStyle.None, ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
            SetBorderColor(worksheet.Cells["A7:R7"], Color.Empty, Color.Empty, Color.Blue, Color.Empty);

            //第八到二十五行
            string[] days = GetDaysByWeek(8);            
            for (int i = 0; i < 2; i++)
            {
                string seat = $"{(char)('A' + i * 9)}8:{(char)('A' + i * 9)}9";
                SetMergeCellsStyle(worksheet, seat);
                SetGeneral1_1(worksheet.Cells[seat], 10);
                SetBorderCellStyle(worksheet.Cells[seat], ExcelBorderStyle.Thin);
                SetBorderColor(worksheet.Cells[seat], Color.Blue);
                worksheet.Cells[seat].Value = "日\x20星期\x20期";

                values = new (string, string)[] {
                        ($"{(char)('B' + i * 9)}8:{(char)('C' + i * 9)}8", "班段1"),
                        ($"{(char)('D' + i * 9)}8:{(char)('E' + i * 9)}8", "班段2"),
                        ($"{(char)('F' + i * 9)}8:{(char)('G' + i * 9)}8", "班段3")
                    };
                foreach (var (position, content) in values)
                {
                    SetMergeCellsStyle(worksheet, position);
                    SetGeneral1_1(worksheet.Cells[position], 10);
                    SetBorderCellStyle(worksheet.Cells[position], ExcelBorderStyle.Thin);
                    SetBorderColor(worksheet.Cells[position], Color.Blue);
                    worksheet.Cells[position].Value = content;
                }

                values = new (string, string)[] {
                        ($"{(char)('B' + i * 9)}9", "上班"),
                        ($"{(char)('C' + i * 9)}9", "下班"),
                        ($"{(char)('D' + i * 9)}9", "上班"),
                        ($"{(char)('E' + i * 9)}9", "下班"),
                        ($"{(char)('F' + i * 9)}9", "签到"),
                        ($"{(char)('G' + i * 9)}9", "签退")
                    };
                foreach (var (position, content) in values)
                {
                    SetGeneral1_1(worksheet.Cells[position], 10);
                    SetBorderCellStyle(worksheet.Cells[position], ExcelBorderStyle.Thin);
                    SetBorderColor(worksheet.Cells[position], Color.Blue);
                    worksheet.Cells[position].Value = content;
                }

                seat = $"{(char)('H' + i * 9)}8:{(char)('I' + i * 9)}8";
                SetMergeCellsStyle(worksheet, seat);
                SetGeneral1_2(worksheet.Cells[seat], 10);
                SetBorderCellStyle(worksheet.Cells[seat], ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
                SetBorderColor(worksheet.Cells[seat], Color.Blue, Color.Blue, Color.Blue, Color.Empty);
                worksheet.Cells[seat].Value = "日\x20\x20统\x20\x20计";

                values = new (string, string)[] {
                        ($"{(char)('H' + i * 9)}9", "工作"),
                        ($"{(char)('I' + i * 9)}9", "加班")
                    };
                foreach (var (position, content) in values)
                {
                    SetGeneral1_2(worksheet.Cells[position], 10);
                    SetBorderCellStyle(worksheet.Cells[position], ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
                    SetBorderColor(worksheet.Cells[position], Color.Blue, Color.Blue, Color.Blue, Color.Empty);
                    worksheet.Cells[position].Value = content;
                }

                for (int j = 0; j < 16; j++)
                {
                                        
                    seat = $"{(char)('A' + i * 9)}{10 + j}";
                    SetGeneral1_1(worksheet.Cells[seat], 10);
                    SetBorderCellStyle(worksheet.Cells[seat], ExcelBorderStyle.Thin);
                    SetBorderColor(worksheet.Cells[seat], Color.Blue);
                    if (j + i * 16 < days.Count())
                    {                        
                        worksheet.Cells[seat].Value = days[j + i * 16];
                        if (days[j + i * 16].Contains("日") || days[j + i * 16].Contains("六"))
                            worksheet.Cells[seat].Style.Font.Color.SetColor(Color.Red);
                    }

                    
                    for (int k = 0; k < 6; k++)
                    {
                        seat = $"{(char)('B' + i * 9 + k)}{10 + j}";
                        SetGeneral1_3(worksheet.Cells[seat], 10);
                        SetBorderCellStyle(worksheet.Cells[seat], ExcelBorderStyle.Thin);
                        SetBorderColor(worksheet.Cells[seat], Color.Blue);                                                                      
                        worksheet.Cells[seat].Value = statistic.SignUpDatas[j + i * 16][k].Text;
                        if(statistic.SignUpDatas[j + i * 16][k].Color != Color.Empty)
                            worksheet.Cells[seat].Style.Font.Color.SetColor(statistic.SignUpDatas[j + i * 16][k].Color);
                    }

                    for (int k = 0; k < 2; k++)
                    {
                        seat = $"{(char)('H' + i * 9 + k)}{10 + j}";
                        SetGeneral1_2(worksheet.Cells[seat], 10);
                        SetBorderCellStyle(worksheet.Cells[seat], ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
                        SetBorderColor(worksheet.Cells[seat], Color.Blue, Color.Blue, Color.Blue, Color.Empty);
                        if (k == 0)
                        {                            
                            worksheet.Cells[seat].Value = statistic.SignUpDatas[j + i * 16][6].Text;
                        }
                        else
                        {
                            worksheet.Cells[seat].Value = statistic.SignUpDatas[j + i * 16][7].Text;
                        }                       
                    }
                }
            }            
            //最右边
            SetMergeCellsStyle(worksheet, "S1:S26");
            worksheet.Cells["S1:S26"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["S1:S26"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            SetBorderCellStyle(worksheet.Cells["S1:S26"], ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.None, ExcelBorderStyle.Thin);
            SetBorderColor(worksheet.Cells["S1:S26"], Color.Blue, Color.Blue, Color.Empty, Color.Blue);

            //最下边
            SetMergeCellsStyle(worksheet, "A26:R26");
            worksheet.Cells["A26:R26"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["A26:R26"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            SetBorderCellStyle(worksheet.Cells["A26:R26"], ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
            SetBorderColor(worksheet.Cells["A26:R26"], Color.Empty, Color.Blue, Color.Blue, Color.Empty);
        }

       

        /// <summary>
        /// 创建异常考勤统计表模板
        /// </summary>
        public static void CreatAttendanceExceptionSheet(BakDatasHandle center)
        {
            // 在 Excel 包类上使用许可证上下文属性
            // 删除许可证异常
            // 必须设置，否则会报错
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//非商业

            //创建一个新的Excel包
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("异常考勤");
                CreatAttendanceExceptionSheet(worksheet, center);
                //保存Excel文件
                FileInfo file = new FileInfo(@"F:\文档\考勤异常表.xlsx");
                package.SaveAs(file);
            }
        }

        public static void CreatAttendanceExceptionSheet(ExcelWorksheet worksheet, int rowCount)
        {
            (string, string)[] values;
            string seat;
            worksheet.Columns[13].Width = 30;

            // 第一行
            SetMergeCellsStyle(worksheet, "A1:M1");
            SetGeneral1_5(worksheet.Cells["A1:M1"], 18);
            SetBorderCellStyle(worksheet.Cells["A1:M1"], ExcelBorderStyle.Thin, ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin);
            SetBorderColor(worksheet.Cells["A1:M1"], Color.Green, Color.Empty, Color.Green, Color.Green);
            worksheet.Cells["A1:M1"].Style.Font.Bold = true;
            worksheet.Cells["A1:M1"].Value = "异\x20常\x20考\x20勤\x20统\x20计\x20表";

            //第二行
            SetGeneral1_5(worksheet.Cells["A2:M2"], 10);
            SetBorderCellStyle(worksheet.Cells["M2"], ExcelBorderStyle.None, ExcelBorderStyle.None, ExcelBorderStyle.None, ExcelBorderStyle.Thin);
            SetBorderColor(worksheet.Cells["M2"], Color.Empty, Color.Empty, Color.Empty, Color.Green);
            SetMergeCellsStyle(worksheet, "A2:B2");
            SetMergeCellsStyle(worksheet, "C2:D2");
            worksheet.Cells["A2:B2"].Value = "统计日期：";
            worksheet.Cells["C2:D2"].Style.Numberformat.Format = "@";
            worksheet.Cells["C2:D2"].Style.Font.Bold = true;
            
            //第三、四行
            SetGeneral1_6(worksheet.Cells["A3:K4"], 10);
            SetGeneral1_7(worksheet.Cells["L3:M4"], 10);
            values = new (string, string)[] { ("A3:A4", "工号"), ("B3:B4", "姓名"), ("C3:C4", "部门"), ("D3:D4", "班次"), ("E3:E4", "日期") };
            foreach (var (position, content) in values)
            {
                SetMergeCellsStyle(worksheet, position);
                SetBorderCellStyle(worksheet.Cells[position], ExcelBorderStyle.Thin);
                SetBorderColor(worksheet.Cells[position], Color.Green);
                worksheet.Cells[position].Value = content;
            }
            for (int k = 0; k < 3; k++)
            {
                seat = $"{(char)('F' + k * 2)}3";
                worksheet.Cells[seat].Value = "班段";
                SetBorderCellStyle(worksheet.Cells[seat], ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
                SetBorderColor(worksheet.Cells[seat], Color.Green, Color.Green, Color.Green, Color.Empty);

                seat = $"{(char)('G' + k * 2)}3";
                worksheet.Cells[seat].Value = k + 1;
                SetBorderCellStyle(worksheet.Cells[seat], ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.None, ExcelBorderStyle.Thin);
                SetBorderColor(worksheet.Cells[seat], Color.Green, Color.Green, Color.Empty, Color.Green);

                seat = $"{(char)('F' + k * 2)}4";
                worksheet.Cells[seat].Value = "上班";
                SetBorderCellStyle(worksheet.Cells[seat], ExcelBorderStyle.Thin);
                SetBorderColor(worksheet.Cells[seat], Color.Green);

                seat = $"{(char)('G' + k * 2)}4";
                worksheet.Cells[seat].Value = "下班";
                SetBorderCellStyle(worksheet.Cells[seat], ExcelBorderStyle.Thin);
                SetBorderColor(worksheet.Cells[seat], Color.Green);
            }
            SetMergeCellsStyle(worksheet, "L3:L4");
            SetBorderCellStyle(worksheet.Cells["L3:L4"]);
            SetBorderColor(worksheet.Cells["L3:L4"], Color.Green);
            worksheet.Cells["L3:L4"].Value = "迟到/早退(分)";
            SetBorderCellStyle(worksheet.Cells["M3"], ExcelBorderStyle.Thin, ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin);
            SetBorderColor(worksheet.Cells["M3"], Color.Green, Color.Empty, Color.Green, Color.Green);
            worksheet.Cells["M3"].Value = "备份";
            SetBorderCellStyle(worksheet.Cells["M4"], ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin);
            SetBorderColor(worksheet.Cells["M4"], Color.Empty, Color.Green, Color.Green, Color.Green);
            for (int i = 0; i < rowCount; i++)
            {
                seat = $"A{5 + i}:E{5 + i}";
                SetGeneral1_5(worksheet.Cells[seat], 10);
                SetBorderCellStyle(worksheet.Cells[seat]);
                SetBorderColor(worksheet.Cells[seat], Color.Green);

                seat = $"F{5 + i}:M{5 + i}";
                SetGeneral1_4(worksheet.Cells[seat], 10);
                SetBorderCellStyle(worksheet.Cells[seat]);
                SetBorderColor(worksheet.Cells[seat], Color.Green);

                worksheet.Cells[$"A{5 + i}:M{5 + i}"].Style.Numberformat.Format = "@";                
            }
        }

        public static void FillAttendanceExceptionSheet(ExcelWorksheet worksheet, int rowCount)
        {
            
        }

        public static void CreatAttendanceExceptionSheet(ExcelWorksheet worksheet, BakDatasHandle center)
        {
            (string, string)[] values;
            string seat;
            DateTime select = new DateTime(2024, 8, 1);
            var datas = center.GetEmployeeAndAttendanceDataByDateTime(select);

            worksheet.Columns[13].Width = 30;

            // 第一行
            SetMergeCellsStyle(worksheet, "A1:M1");
            SetGeneral1_5(worksheet.Cells["A1:M1"], 18);
            SetBorderCellStyle(worksheet.Cells["A1:M1"], ExcelBorderStyle.Thin, ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin);
            SetBorderColor(worksheet.Cells["A1:M1"], Color.Green, Color.Empty, Color.Green, Color.Green);
            worksheet.Cells["A1:M1"].Style.Font.Bold = true;
            worksheet.Cells["A1:M1"].Value = "异\x20常\x20考\x20勤\x20统\x20计\x20表";

            //第二行
            SetGeneral1_5(worksheet.Cells["A2:M2"], 10);
            SetBorderCellStyle(worksheet.Cells["M2"], ExcelBorderStyle.None, ExcelBorderStyle.None, ExcelBorderStyle.None, ExcelBorderStyle.Thin);
            SetBorderColor(worksheet.Cells["M2"], Color.Empty, Color.Empty, Color.Empty, Color.Green);
            SetMergeCellsStyle(worksheet, "A2:B2");
            SetMergeCellsStyle(worksheet, "C2:D2");
            worksheet.Cells["A2:B2"].Value = "统计日期：";
            worksheet.Cells["C2:D2"].Style.Numberformat.Format = "@";
            worksheet.Cells["C2:D2"].Style.Font.Bold = true;
            worksheet.Cells["C2:D2"].Value = select.ToString("yyyy-MM");

            //第三、四行
            SetGeneral1_6(worksheet.Cells["A3:K4"], 10);
            SetGeneral1_7(worksheet.Cells["L3:M4"], 10);

            values = new (string, string)[] { ("A3:A4", "工号"), ("B3:B4", "姓名"), ("C3:C4", "部门"), ("D3:D4", "班次"), ("E3:E4", "日期") };
            foreach (var (position, content) in values)
            {
                SetMergeCellsStyle(worksheet, position);
                SetBorderCellStyle(worksheet.Cells[position], ExcelBorderStyle.Thin);
                SetBorderColor(worksheet.Cells[position], Color.Green);
                worksheet.Cells[position].Value = content;
            }

            for (int k = 0; k < 3; k++)
            {
                seat = $"{(char)('F' + k * 2)}3";
                worksheet.Cells[seat].Value = "班段";
                SetBorderCellStyle(worksheet.Cells[seat], ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.None);
                SetBorderColor(worksheet.Cells[seat], Color.Green, Color.Green, Color.Green, Color.Empty);

                seat = $"{(char)('G' + k * 2)}3";
                worksheet.Cells[seat].Value = k + 1;
                SetBorderCellStyle(worksheet.Cells[seat], ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.None, ExcelBorderStyle.Thin);
                SetBorderColor(worksheet.Cells[seat], Color.Green, Color.Green, Color.Empty, Color.Green);

                seat = $"{(char)('F' + k * 2)}4";
                worksheet.Cells[seat].Value = "上班";
                SetBorderCellStyle(worksheet.Cells[seat], ExcelBorderStyle.Thin);
                SetBorderColor(worksheet.Cells[seat], Color.Green);

                seat = $"{(char)('G' + k * 2)}4";
                worksheet.Cells[seat].Value = "下班";
                SetBorderCellStyle(worksheet.Cells[seat], ExcelBorderStyle.Thin);
                SetBorderColor(worksheet.Cells[seat], Color.Green);
            }

            SetMergeCellsStyle(worksheet, "L3:L4");
            SetBorderCellStyle(worksheet.Cells["L3:L4"]);
            SetBorderColor(worksheet.Cells["L3:L4"], Color.Green);
            worksheet.Cells["L3:L4"].Value = "迟到/早退(分)";
            SetBorderCellStyle(worksheet.Cells["M3"], ExcelBorderStyle.Thin, ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin);
            SetBorderColor(worksheet.Cells["M3"], Color.Green, Color.Empty, Color.Green, Color.Green);
            worksheet.Cells["M3"].Value = "备份";
            SetBorderCellStyle(worksheet.Cells["M4"], ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin);
            SetBorderColor(worksheet.Cells["M4"], Color.Empty, Color.Green, Color.Green, Color.Green);

            var attendances = datas.Where(a => a.ClockState == ClockState.EXCEPTION)
                                   .GroupBy(a => a.UserIndex)
                                   .OrderBy(g => g.Key);
            int i = 0;
            AttendanceRule rule = null;
            BakUseData employee = null;
            foreach (var item in attendances)
            {
                var groups = item.GroupBy(a => a.ClockTime.Date);
                employee = center.Employees.Find(e => e.Index == item.Key);
                foreach (var val in groups)
                {
                    var sort = val.GroupBy(a => a.ShiftClass)
                                  .Select(g => g.OrderBy(a => a.ClockTime).FirstOrDefault())
                                  .OrderBy(a => a.ClockTime)
                                  .ToList();
                    rule = Rules.RuleList.Find(r => r.SerialNumber == val.FirstOrDefault().Class);

                    seat = $"A{5 + i}:E{5 + i}";
                    SetGeneral1_5(worksheet.Cells[seat], 10);
                    SetBorderCellStyle(worksheet.Cells[seat]);
                    SetBorderColor(worksheet.Cells[seat], Color.Green);

                    seat = $"F{5 + i}:M{5 + i}";
                    SetGeneral1_4(worksheet.Cells[seat], 10);
                    SetBorderCellStyle(worksheet.Cells[seat]);
                    SetBorderColor(worksheet.Cells[seat], Color.Green);

                    worksheet.Cells[$"A{5 + i}:M{5 + i}"].Style.Numberformat.Format = "@";

                    //工号
                    worksheet.Cells[$"A{5 + i}"].Value = employee.Id;
                    //姓名
                    worksheet.Cells[$"B{5 + i}"].Value = employee.Name;
                    //部门
                    worksheet.Cells[$"C{5 + i}"].Value = "公司";
                    //班次                
                    worksheet.Cells[$"D{5 + i}"].Value = rule.RuleName;
                    //日期
                    worksheet.Cells[$"E{5 + i}"].Value = val.Key.ToString("MM-dd");
                    TimeSpan norm = TimeSpan.Zero;
                    foreach (var s in sort)
                    {
                        switch (s.ShiftClass)
                        {
                            case ShiftClass.ONE_CLOCK_ON:
                                //班段1，上班
                                worksheet.Cells[$"F{5 + i}"].Value = s.ClockTime.ToString("HH:mm");
                                norm += s.ClockTime.TimeOfDay - rule.Classes[GetWeek(s.ClockTime.Month, s.ClockTime.Day)][0].StartTime;
                                break;
                            case ShiftClass.ONE_CLOCK_OFF:
                                //班段1，下班
                                worksheet.Cells[$"G{5 + i}"].Value = s.ClockTime.ToString("HH:mm");
                                norm += rule.Classes[GetWeek(s.ClockTime.Month, s.ClockTime.Day)][0].EndTime - s.ClockTime.TimeOfDay;
                                break;
                            case ShiftClass.TWO_CLOCK_ON:
                                //班段2，上班
                                worksheet.Cells[$"H{5 + i}"].Value = s.ClockTime.ToString("HH:mm");
                                norm += s.ClockTime.TimeOfDay - rule.Classes[GetWeek(s.ClockTime.Month, s.ClockTime.Day)][1].StartTime;
                                break;
                            case ShiftClass.TWO_CLOCK_OFF:
                                //班段2，下班
                                worksheet.Cells[$"I{5 + i}"].Value = s.ClockTime.ToString("HH:mm");
                                norm += rule.Classes[GetWeek(s.ClockTime.Month, s.ClockTime.Day)][1].EndTime - s.ClockTime.TimeOfDay;
                                break;
                            case ShiftClass.THREE_CLOCK_ON:
                                //班段3，上班
                                worksheet.Cells[$"J{5 + i}"].Value = s.ClockTime.ToString("HH:mm");
                                norm += s.ClockTime.TimeOfDay - rule.Classes[GetWeek(s.ClockTime.Month, s.ClockTime.Day)][2].StartTime;
                                break;
                            case ShiftClass.THREE_CLOCK_OFF:
                                //班段3，下班
                                worksheet.Cells[$"K{5 + i}"].Value = s.ClockTime.ToString("HH:mm");
                                norm += rule.Classes[GetWeek(s.ClockTime.Month, s.ClockTime.Day)][2].EndTime - s.ClockTime.TimeOfDay;
                                break;
                            case ShiftClass.VOIDANCE:
                                break;
                            default:
                                break;
                        }
                    }
                    worksheet.Cells[$"L{5 + i}"].Value = norm.ToString().Substring(0, 5);
                    i++;
                }
            }
        }

        /// <summary>
        /// 创建考勤汇总表模板
        /// </summary>
        public static void CreatAttendanceSummarySheet(BakDatasHandle center)
        {
            // 在 Excel 包类上使用许可证上下文属性
            // 删除许可证异常
            // 必须设置，否则会报错
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//非商业

            //创建一个新的Excel包
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("考勤汇总");
                CreatAttendanceSummarySheet(worksheet, center);
                //保存Excel文件
                FileInfo file = new FileInfo(@"F:\文档\考勤汇总表.xlsx");
                package.SaveAs(file);
            }
        }

        public static void CreatAttendanceSummarySheet(ExcelWorksheet worksheet, int rowCount)
        {
            (string, string)[] values;
            string seat;
            worksheet.Rows[1].Height = 26.25;
            worksheet.Rows[2].Height = 18;
            for (int k = 1; k <= 24; k++)
            {
                worksheet.Columns[5 + k].Width = 7;
            }
            for (int k = 0; k < 6; k++)
            {
                worksheet.Columns[5 + k].Width = 5.5;
            }
            worksheet.Columns[15].Width = 5;

            //第一行
            SetMergeCellsStyle(worksheet, "A1:X1");
            SetGeneral1_8(worksheet.Cells["A1:X1"], 18);
            SetBorderCellStyle(worksheet.Cells["A1:X1"], ExcelBorderStyle.Thin, ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin);
            SetBorderColor(worksheet.Cells["A1:X1"], Color.Green, Color.Empty, Color.Green, Color.Green);
            worksheet.Cells["A1:X1"].Style.Font.Bold = true;
            worksheet.Cells["A1:X1"].Value = "考\x20勤\x20汇\x20总\x20表";

            //第二行
            SetGeneral1_8(worksheet.Cells["A2:X2"], 10);
            SetMergeCellsStyle(worksheet, "A2:B2");
            worksheet.Cells["A2:B2"].Value = "统计日期";
            SetMergeCellsStyle(worksheet, "D2:G2");
            worksheet.Cells["D2:G2"].Style.Numberformat.Format = "yyyy-MM";
            SetMergeCellsStyle(worksheet, "K2:X2");
            worksheet.Cells["K2:X2"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["K2:X2"].Style.Border.Right.Color.SetColor(Color.FromArgb(0, 128, 0));

            //第三、四行
            SetGeneral1_1(worksheet.Cells["A3:X4"], 10);
            values = new (string, string)[] { ("A3:A4","工号"), ("B3:B4","姓名"), ("C3:C4","部门"), ("D3:D4","班次"), ("I3:I4","旷工(天)"), ("E3:F3","出勤(天)"),
                                              ("G3:H3","请假(天)"), ("K3:L3","工作(时分)"), ("M3:N3","加班(时分)"), ("J3:J4","出差(天)"), ("O3:P3","迟到/早退"),
                                              ("Q3:S3","加项工资"), ("T3:V3","减项工资"), ("W3:W4","实\x20际\x20工\x20资"), ("X3:X4","备\x20注"), ("E4","标准"), ("F4","实际"),
                                              ("G4","事假"), ("H4","病假"), ("K4","标准"), ("L4","实际"), ("M4","正常"), ("N4","特殊"), ("O4","次"), ("P4","分"),
                                              ("Q4","标准"), ("R4","加班"), ("S4","津贴"), ("T4","迟早"), ("U4","事假"), ("V4","扣款") };
            foreach (var (position, content) in values)
            {
                SetMergeCellsStyle(worksheet, position);
                SetBorderCellStyle(worksheet.Cells[position]);
                SetBorderColor(worksheet.Cells[position], Color.Green);
                worksheet.Cells[position].Value = content;
            }

            for (int i = 0; i < rowCount; i++)
            {
                seat = $"A{5 + i}:X{5 + i}";
                SetBorderCellStyle(worksheet.Cells[seat]);
                SetBorderColor(worksheet.Cells[seat], Color.Green);
                worksheet.Cells[seat].Style.Numberformat.Format = "@";//将数字作为文本处理
                if (i % 2 == 0)
                {
                    SetGeneral1_8(worksheet.Cells[seat], 10);
                }
                else
                {
                    seat = $"A{5 + i}:P{5 + i}";
                    SetGeneral1_1(worksheet.Cells[seat], 10);
                    seat = $"Q{5 + i}:X{5 + i}";
                    SetGeneral1_9(worksheet.Cells[seat], 10);
                }
            }
        }

        public static void FillAttendanceSummarySheet()
        {

        }

        public static void CreatAttendanceSummarySheet(ExcelWorksheet worksheet, BakDatasHandle center)
        {
            (string, string)[] values;
            string seat;
            DateTime select = new DateTime(2024, 8, 1);
            var datas = center.GetEmployeeAndAttendanceDataByDateTime(select);

            worksheet.Rows[1].Height = 26.25;
            worksheet.Rows[2].Height = 18;
            for (int k = 1; k <= 24; k++)
            {
                worksheet.Columns[5 + k].Width = 7;
            }
            for (int k = 0; k < 6; k++)
            {
                worksheet.Columns[5 + k].Width = 5.5;
            }
            worksheet.Columns[15].Width = 5;

            //第一行
            SetMergeCellsStyle(worksheet, "A1:X1");
            SetGeneral1_8(worksheet.Cells["A1:X1"], 18);
            SetBorderCellStyle(worksheet.Cells["A1:X1"], ExcelBorderStyle.Thin, ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin);
            SetBorderColor(worksheet.Cells["A1:X1"], Color.Green, Color.Empty, Color.Green, Color.Green);
            worksheet.Cells["A1:X1"].Style.Font.Bold = true;
            worksheet.Cells["A1:X1"].Value = "考\x20勤\x20汇\x20总\x20表";

            //第二行
            SetGeneral1_8(worksheet.Cells["A2:X2"], 10);
            SetMergeCellsStyle(worksheet, "A2:B2");
            worksheet.Cells["A2:B2"].Value = "统计日期";
            SetMergeCellsStyle(worksheet, "D2:G2");
            worksheet.Cells["D2:G2"].Style.Numberformat.Format = "yyyy-MM";
            worksheet.Cells["D2:G2"].Value = select.ToString("yyyy-MM");
            SetMergeCellsStyle(worksheet, "K2:X2");
            worksheet.Cells["K2:X2"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["K2:X2"].Style.Border.Right.Color.SetColor(Color.FromArgb(0, 128, 0));

            //第三、四行
            SetGeneral1_1(worksheet.Cells["A3:X4"], 10);
            values = new (string, string)[] { ("A3:A4","工号"), ("B3:B4","姓名"), ("C3:C4","部门"), ("D3:D4","班次"), ("I3:I4","旷工(天)"), ("E3:F3","出勤(天)"),
                                              ("G3:H3","请假(天)"), ("K3:L3","工作(时分)"), ("M3:N3","加班(时分)"), ("J3:J4","出差(天)"), ("O3:P3","迟到/早退"),
                                              ("Q3:S3","加项工资"), ("T3:V3","减项工资"), ("W3:W4","实\x20际\x20工\x20资"), ("X3:X4","备\x20注"), ("E4","标准"), ("F4","实际"),
                                              ("G4","事假"), ("H4","病假"), ("K4","标准"), ("L4","实际"), ("M4","正常"), ("N4","特殊"), ("O4","次"), ("P4","分"),
                                              ("Q4","标准"), ("R4","加班"), ("S4","津贴"), ("T4","迟早"), ("U4","事假"), ("V4","扣款") };
            foreach (var (position, content) in values)
            {
                SetMergeCellsStyle(worksheet, position);
                SetBorderCellStyle(worksheet.Cells[position]);
                SetBorderColor(worksheet.Cells[position], Color.Green);
                worksheet.Cells[position].Value = content;
            }

            var attendances = datas.GroupBy(a => a.UserIndex).OrderBy(g => g.Key);
            int i = 0, hour = 0, min = 0, lateMin = 0, lateNum = 0;
            TimeSpan start = TimeSpan.Zero, end = TimeSpan.Zero, total = TimeSpan.Zero;
            //循环绘制下一行    
            foreach (var item in attendances)
            {
                var attsByday = item.GroupBy(a => a.ClockTime.Day);
                var selects = attsByday.SelectMany(g =>
                {
                    return g.GroupBy(a => a.ShiftClass)
                            .Select(g => g.OrderBy(a => a.ClockTime).FirstOrDefault())
                            .OrderBy(a => a.ClockTime)
                            .ToList();
                });
                BakUseData employee = center.Employees.Find(a => a.Index == selects.FirstOrDefault().UserIndex);
                AttendanceRule rule = Rules.RuleList.Find(r => r.SerialNumber == selects.FirstOrDefault().Class);
                seat = $"A{5 + i}:X{5 + i}";
                SetBorderCellStyle(worksheet.Cells[seat]);
                SetBorderColor(worksheet.Cells[seat], Color.Green);
                worksheet.Cells[seat].Style.Numberformat.Format = "@";//将数字作为文本处理
                if (i % 2 == 0)
                {
                    SetGeneral1_8(worksheet.Cells[seat], 10);
                }
                else
                {
                    seat = $"A{5 + i}:P{5 + i}";
                    SetGeneral1_1(worksheet.Cells[seat], 10);
                    seat = $"Q{5 + i}:X{5 + i}";
                    SetGeneral1_9(worksheet.Cells[seat], 10);
                }
                //工号
                worksheet.Cells[$"A{5 + i}"].Value = employee.Id;
                //姓名
                worksheet.Cells[$"B{5 + i}"].Value = employee.Name;
                //部门
                worksheet.Cells[$"C{5 + i}"].Value = "公司";
                //班次
                worksheet.Cells[$"D{5 + i}"].Value = rule.RuleName;
                //出勤，标准
                worksheet.Cells[$"E{5 + i}"].Value = GetDays(8);
                //出勤，实际
                worksheet.Cells[$"F{5 + i}"].Value = attsByday.Count();
                //工作，标准
                hour = 0; min = 0;
                for (int k = 1; k <= GetDays(8); k++)
                {
                    foreach (var s in rule.Classes[GetWeek(8, k)])
                    {
                        if (s.StartTime != TimeSpan.Zero && s.EndTime != TimeSpan.Zero && s.StartTime < s.EndTime)
                        {
                            var time = s.EndTime - s.StartTime;
                            hour += time.Hours;
                            min += time.Minutes;
                        }
                    }
                }
                worksheet.Cells[$"K{5 + i}"].Value = string.Format("{0:000}:{1:00}", hour + min / 60, min % 60);
                //工作，实际
                hour = 0; min = 0;
                foreach (var attdance in selects.GroupBy(a => a.ClockTime.Date))
                {
                    int week = GetWeek(attdance.Key.Month, attdance.Key.Day);
                    for (int k = 0; k < 6; k++)
                    {
                        var att = attdance.ToList().Find(a => a.ShiftClass == (ShiftClass)k);
                        if (att != null)
                        {
                            if (week >= 0)
                            {
                                //从规定的标准中，选择对应星期的班次
                                ClassSection s = rule.Classes[week][k / 2];
                                if (k % 2 == 0)
                                {
                                    //比较，选择正确的时间段。迟到
                                    if (att.ClockTime.TimeOfDay > s.StartTime)
                                    {
                                        start = att.ClockTime.TimeOfDay;
                                        lateMin += (int)(start - s.StartTime).TotalMinutes;
                                        lateNum++;
                                    }
                                    else
                                    {
                                        start = s.StartTime;
                                    }
                                }
                                else
                                {
                                    //比较，选择正确的时间段。早退
                                    if (att.ClockTime.TimeOfDay < s.EndTime)
                                    {
                                        end = att.ClockTime.TimeOfDay;
                                        lateMin += (int)(s.EndTime - end).TotalMinutes;
                                        lateNum++;
                                    }
                                    else
                                    {
                                        end = s.EndTime;
                                    }
                                    //时间段不全或者后者小于前者，则不计算
                                    if (end != TimeSpan.Zero && start != TimeSpan.Zero && end > start)
                                        total += end - start;

                                    start = TimeSpan.Zero;
                                    end = TimeSpan.Zero;
                                }
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
                        total = TimeSpan.Zero;
                    }
                }
                worksheet.Cells[$"L{5 + i}"].Value = string.Format("{0:000}:{1:00}", hour + min / 60, min % 60);
                //加班，正常
                worksheet.Cells[$"M{5 + i}"].Value = "00:00";
                //加班，特殊
                worksheet.Cells[$"N{5 + i}"].Value = "00:00";
                //迟到，次
                worksheet.Cells[$"O{5 + i}"].Value = lateNum;
                //迟到，分
                worksheet.Cells[$"P{5 + i}"].Value = lateMin;

                hour = 0;
                min = 0;
                lateNum = 0;
                lateMin = 0;

                i++;
            }
        }

        /// <summary>
        /// 创建考勤原始表模板
        /// </summary>
        public static void CreatOriginalAttendanceSheet(BakDatasHandle center)
        {
            // 在 Excel 包类上使用许可证上下文属性
            // 删除许可证异常
            // 必须设置，否则会报错
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//非商业

            //创建一个新的Excel包
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("原始考勤表");
                CreatOriginalAttendanceSheet(worksheet, center);
                //保存Excel文件
                FileInfo file = new FileInfo(@"F:\文档\考勤原始表.xlsx");
                package.SaveAs(file);
            }
        }

        public static void CreatOriginalAttendanceSheet(ExcelWorksheet worksheet, BakDatasHandle center)
        {
            (string, string)[] values;
            string seat;
            DateTime time = DateTime.Now;
            int days = GetDays(8);
            DateTime select = new DateTime(2024, 8, 1);
            var datas = center.GetEmployeeAndAttendanceDataByDateTime(select);
            int rows = datas.GroupBy(a => a.UserIndex).Count();
            //表格总体设置
            worksheet.Cells[1, 1, rows * 4, days].Style.Numberformat.Format = "@";
            //worksheet.Cells[1, 1, rows * 4, days].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //worksheet.Cells[1, 1, rows * 4, days].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            for (int k = 0; k < days; k++)
            {
                worksheet.Columns[k + 1].Width = 6;
            }
            int i = 0;
            //循环构建多个表格
            foreach (var employee in center.Employees)
            {
                var Eattendances = datas.Where(e => e.UserIndex == employee.Index).ToList();
                if (Eattendances.Count() == 0)
                    continue;

                var rule = Rules.RuleList.Find(r => r.SerialNumber == Eattendances.FirstOrDefault().Class);
                worksheet.Rows[1 + i * 4].Height = 29.25;
                worksheet.Rows[4 + i * 4].Height = 76.5;
                worksheet.Cells[1 + i * 4, 1, 1 + i * 4, days].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[1 + i * 4, 1, 1 + i * 4, days].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[1 + i * 4, 1, 1 + i * 4, days].Style.Font.Bold = true;

                //第一行
                seat = $"A{1 + i * 4}:D{1 + i * 4}";
                SetMergeCellsStyle(worksheet, seat);
                worksheet.Cells[seat].Value = "原始考勤记录表：";

                for (int j = 0; j < 2; j++)
                {
                    seat = $"{(char)('J' + j * 8)}{1 + i * 4}:{(char)('K' + j * 8)}{1 + i * 4}";
                    SetMergeCellsStyle(worksheet, seat);
                    worksheet.Cells[seat].Value = time.Year;
                    worksheet.Cells[$"{(char)('L' + j * 8)}{1 + i * 4}"].Value = "年";
                    worksheet.Cells[$"{(char)('M' + j * 8)}{1 + i * 4}"].Value = time.Month.ToString("00");
                    worksheet.Cells[$"{(char)('N' + j * 8)}{1 + i * 4}"].Value = "月";
                    if (j == 0)
                        worksheet.Cells[$"{(char)('O' + j * 8)}{1 + i * 4}"].Value = 1;
                    else
                        worksheet.Cells[$"{(char)('O' + j * 8)}{1 + i * 4}"].Value = days;
                    worksheet.Cells[$"{(char)('P' + j * 8)}{1 + i * 4}"].Value = "月";
                }
                worksheet.Cells[$"Q{1 + i * 4}"].Value = "--";

                //第二行
                values = new (string, string)[] { ($"A{2 + i * 4}:B{2 + i * 4}","登记号："), ($"E{2 + i * 4}:F{2 + i * 4}", "姓名："),
                                                  ($"J{2 + i * 4}:K{2 + i * 4}","部门："), ($"O{2 + i * 4}:P{2 + i * 4}","班次："),
                                                  ($"T{2 + i * 4}:A{(char)('A' + days - 26 - 1)}{2 + i * 4}","注：浅青色区域为数据区") };
                foreach (var (position, content) in values)
                {
                    SetMergeCellsStyle(worksheet, position);
                    SetGeneral2_0(worksheet.Cells[position], 10);
                    SetBorderCellStyle(worksheet.Cells[position], ExcelBorderStyle.Thin);
                    SetBorderColor(worksheet.Cells[position], Color.Black);
                    worksheet.Cells[position].Value = content;
                }

                values = new (string, string)[] { ($"C{2 + i * 4}:D{2 + i * 4}", employee.Id.ToString("0000")), ($"G{2 + i * 4}:I{2 + i * 4}", employee.Name), ($"L{2 + i * 4}:N{2 + i * 4}","公司"),
                                                  ($"Q{2 + i * 4}:S{2 + i * 4}", rule.RuleName) };
                foreach (var (position, content) in values)
                {
                    SetMergeCellsStyle(worksheet, position);
                    SetGeneral1_5(worksheet.Cells[position], 10);
                    SetBorderCellStyle(worksheet.Cells[position], ExcelBorderStyle.Thin);
                    SetBorderColor(worksheet.Cells[position], Color.Black);
                    worksheet.Cells[position].Value = content;
                }
                //第三、四行
                for (int j = 0; j < days; j++)
                {
                    var dayData = Eattendances.Where(e => e.ClockTime.Day == j + 1);
                    //.GroupBy(e => e.ShiftClass)
                    //.Select(g => g.OrderBy(e => e.ClockTime).FirstOrDefault())
                    //.OrderBy(e => e.ClockTime)
                    //.ToList();
                    SetGeneral2_0(worksheet.Cells[3 + i * 4, j + 1], 12);
                    SetBorderCellStyle(worksheet.Cells[3 + i * 4, j + 1], ExcelBorderStyle.Thin);
                    SetBorderColor(worksheet.Cells[3 + i * 4, j + 1], Color.Black);
                    worksheet.Cells[3 + i * 4, j + 1].Value = j + 1;

                    SetGeneral1_5(worksheet.Cells[4 + i * 4, j + 1], 8);
                    worksheet.Cells[4 + i * 4, j + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    SetBorderCellStyle(worksheet.Cells[4 + i * 4, j + 1], ExcelBorderStyle.Thin);
                    SetBorderColor(worksheet.Cells[4 + i * 4, j + 1], Color.Black);
                    string val = "";
                    foreach (var item in dayData)
                    {
                        val += item.ClockTime.ToString("HH:mm") + "\x20";
                    }
                    worksheet.Cells[4 + i * 4, j + 1].Value = val;
                }
                i++;
            }
        }

        /// <summary>
        /// 创建考勤排班表模板
        /// </summary>
        /// <param name="worksheet"></param>
        public static void CreatAttendanceSheet()
        {
            // 在 Excel 包类上使用许可证上下文属性
            // 删除许可证异常
            // 必须设置，否则会报错
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//非商业

            //创建一个新的Excel包
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("排班表");
                CreatAttendanceSheet(worksheet);

                //保存Excel文件
                FileInfo file = new FileInfo(@"F:\文档\考勤排班表.xlsx");
                package.SaveAs(file);
            }
        }

        public static void CreatAttendanceSheet(ExcelWorksheet worksheet)
        {
            CreatAttendanceSheet(worksheet, 0, 1);
        }

        public static void CreatAttendanceSheet(ExcelWorksheet worksheet, int x, int y)
        {
            x = x < 0 ? 0 : x;
            y = y < 0 ? 0 : y;

            (string, string)[] values;

            //表格总体设置
            worksheet.Cells[1 + y, 1 + x, 14 + y, 10 + x].Style.Numberformat.Format = "@";

            for (int i = 1; i <= 10; i++)
            {
                worksheet.Columns[i + x].Width = 11;
            }
            values = new (string, string)[] { 
                 /*第一行*/
                ($"{(char)('A' + x)}{1 + y}","序号："), ($"{(char)('C' + x)}{1 + y}","名称："), ($"{(char)('H' + x)}{1 + y}","跨天时间："),                               
                 /*第五行*/
                ($"{(char)('A' + x)}{5 + y}:{(char)('J' + x)}{5 + y}","周考勤设置"),
                 /*第六行*/
                ($"{(char)('A' + x)}{6 + y}",""), ($"{(char)('B' + x)}{6 + y}:{(char)('D' + x)}{6 + y}","班段1"),
                ($"{(char)('E' + x)}{6 + y}:{(char)('G' + x)}{6 + y}","班段2"), ($"{(char)('H' + x)}{6 + y}:{(char)('J' + x)}{6 + y}","班段3"),
                 /*第七行*/
                ($"{(char)('A' + x)}{7 + y}",""), ($"{(char)('B' + x)}{7 + y}","上班"), ($"{(char)('C' + x)}{7 + y}","下班"), ($"{(char)('D' + x)}{7 + y}","类型"),
                ($"{(char)('E' + x)}{7 + y}","上班"), ($"{(char)('F' + x)}{7 + y}","下班"), ($"{(char)('G' + x)}{7 + y}","类型"), ($"{(char)('H' + x)}{7 + y}","上班"),
                ($"{(char)('I' + x)}{7 + y}","下班"), ($"{(char)('J' + x)}{7 + y}","类型"),
                 /*第八到十四行*/
                ($"{(char)('A' + x)}{8 + y}","周一"), ($"{(char)('A' + x)}{9 + y}","周二"), ($"{(char)('A' + x)}{10 + y}","周三"), ($"{(char)('A' + x)}{11 + y}","周四"),
                ($"{(char)('A' + x)}{12 + y}","周五"), ($"{(char)('A' + x)}{13 + y}","周六"), ($"{(char)('A' + x)}{14 + y}","周日")
            };
            foreach (var (position, content) in values)
            {
                SetMergeCellsStyle(worksheet, position);
                SetGeneral2_0(worksheet.Cells[position], 10);
                SetBorderCellStyle(worksheet.Cells[position]);
                SetBorderColor(worksheet.Cells[position], Color.Black);
                worksheet.Cells[position].Style.Font.Bold = true;
                worksheet.Cells[position].Value = content;
            }

            values = new (string, string)[] { 
                 /*第二行*/
                ($"{(char)('A' + x)}{2 + y}","闹铃次数"), ($"{(char)('B' + x)}{2 + y}:{(char)('C' + x)}{2 + y}","考勤方式"), ($"{(char)('D' + x)}{2 + y}","统计单位"),
                ($"{(char)('E' + x)}{2 + y}:{(char)('F' + x)}{2 + y}","统计方式"), ($"{(char)('G' + x)}{2 + y}:{(char)('H' + x)}{2 + y}","换班时间"),
                ($"{(char)('I' + x)}{2 + y}","允许迟到"), ($"{(char)('J' + x)}{2 + y}","允许早退")
            };
            foreach (var (position, content) in values)
            {
                SetMergeCellsStyle(worksheet, position);
                SetGeneral2_0(worksheet.Cells[position], 10);
                SetBorderCellStyle(worksheet.Cells[position], ExcelBorderStyle.Thin, ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin);
                SetBorderColor(worksheet.Cells[position], Color.Black, Color.Empty, Color.Black, Color.Black);
                worksheet.Cells[position].Style.Font.Bold = true;
                worksheet.Cells[position].Value = content;
            }

            values = new (string, string)[] { 
                 /*第三行*/
                ($"{(char)('A' + x)}{3 + y}","(Times)"), ($"{(char)('B' + x)}{3 + y}:{(char)('C' + x)}{3 + y}","(0:连续考勤\x20"+"1:非连续考勤)"),
                ($"{(char)('D' + x)}{3 + y}","(分钟\x20M)"), ($"{(char)('E' + x)}{3 + y}:{(char)('F' + x)}{3 + y}","(0:统计时间\x20"+"1:考勤时间)"),
                ($"{(char)('G' + x)}{3 + y}:{(char)('H' + x)}{3 + y}","(换班分割线\x20"+"0:1/2\x20"+"1:1/3)"), ($"{(char)('I' + x)}{3 + y}","(分钟\x20M)"),
                ($"{(char)('J' + x)}{3 + y}","(分钟\x20M)")
            };
            foreach (var (position, content) in values)
            {
                SetMergeCellsStyle(worksheet, position);
                SetGeneral2_2(worksheet.Cells[position], 8);
                SetBorderCellStyle(worksheet.Cells[position], ExcelBorderStyle.None, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin, ExcelBorderStyle.Thin);
                SetBorderColor(worksheet.Cells[position], Color.Empty, Color.Black, Color.Black, Color.Black);
                worksheet.Cells[position].Style.Font.Bold = true;
                worksheet.Cells[position].Value = content;
            }

            values = new (string, string)[] { 
                 /*第一行*/
                ($"{(char)('B' + x)}{1 + y}",""), ($"{(char)('D' + x)}{1 + y}:{(char)('G' + x)}{1 + y}",""), ($"{(char)('I' + x)}{1 + y}:{(char)('J' + x)}{1 + y}",""),
                 /*第四行*/
                ($"{(char)('A' + x)}{4 + y}",""), ($"{(char)('B' + x)}{4 + y}:{(char)('C' + x)}{4 + y}",""), ($"{(char)('D' + x)}{4 + y}",""),
                ($"{(char)('E' + x)}{4 + y}:{(char)('F' + x)}{4 + y}",""), ($"{(char)('G' + x)}{4 + y}:{(char)('H' + x)}{4 + y}",""),
                ($"{(char)('I' + x)}{4 + y}",""), ($"{(char)('J' + x)}{4 + y}","")
            };
            foreach (var (position, content) in values)
            {
                SetMergeCellsStyle(worksheet, position);
                SetGeneral2_1(worksheet.Cells[position], 10);
                SetBorderCellStyle(worksheet.Cells[position]);
                SetBorderColor(worksheet.Cells[position], Color.Black);
                worksheet.Cells[position].Value = content;
            }

            for (int i = 0; i < 7; i++)
            {
                values = new (string, string)[] {
                     /*班段1*/
                    ($"{(char)('B' + x)}{8 + y + i}",""), ($"{(char)('C' + x)}{8 + y + i}",""), ($"{(char)('D' + x)}{8 + y + i}",""),
                     /*班段2*/
                    ($"{(char)('E' + x)}{8 + y + i}",""), ($"{(char)('F' + x)}{8 + y + i}",""), ($"{(char)('G' + x)}{8 + y + i}",""),
                     /*班段3*/
                    ($"{(char)('H' + x)}{8 + y + i}",""), ($"{(char)('I' + x)}{8 + y + i}",""), ($"{(char)('J' + x)}{8 + y + i}","")
                };
                foreach (var (position, content) in values)
                {
                    SetMergeCellsStyle(worksheet, position);
                    SetGeneral2_1(worksheet.Cells[position], 10);
                    SetBorderCellStyle(worksheet.Cells[position]);
                    SetBorderColor(worksheet.Cells[position], Color.Black);
                    worksheet.Cells[position].Value = content;
                }
            }

        }

        /// <summary>
        /// 获取当前月的天数
        /// </summary>
        /// <returns></returns>
        private static int GetDays()
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
        private static int GetDays(int month)
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
        public static int GetWeek(int day)
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
        public static int GetWeek(int month, int day)
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
        private static string[] GetDaysByWeek()
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
        private static string[] GetDaysByWeek(int month)
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

        // 设置单元格的字体、颜色、对齐方式
        private static void SetGeneralStyle(ExcelRange range, string fontName, int fontSize, Color fontColor, Color backgroundColor,
            ExcelHorizontalAlignment horizontalAlignment = ExcelHorizontalAlignment.Center, ExcelVerticalAlignment verticalAlignment = ExcelVerticalAlignment.Center,
            bool bold = false, bool italic = false, bool underLine = false, bool strikeout = false)
        {
            range.Style.Font.SetFromFont(fontName, fontSize, bold, italic, underLine, strikeout);
            range.Style.Font.Color.SetColor(fontColor);
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(backgroundColor);
            range.Style.HorizontalAlignment = horizontalAlignment;
            range.Style.VerticalAlignment = verticalAlignment;
            range.Style.WrapText = true; //自动换行
        }

        // 合并单元格
        private static void SetMergeCellsStyle(ExcelWorksheet worksheet, string range)
        {
            worksheet.Cells[range].Merge = true;
        }

        //设置边框
        private static void SetBorderCellStyle(ExcelRange range, ExcelBorderStyle top = ExcelBorderStyle.Thin, ExcelBorderStyle bottom = ExcelBorderStyle.Thin, ExcelBorderStyle left = ExcelBorderStyle.Thin, ExcelBorderStyle right = ExcelBorderStyle.Thin)
        {
            range.Style.Border.Top.Style = top;
            range.Style.Border.Bottom.Style = bottom;
            range.Style.Border.Left.Style = left;
            range.Style.Border.Right.Style = right;
        }

        private static void SetBorderCellStyle(ExcelRange range, ExcelBorderStyle borderStyle)
        {
            range.Style.Border.Top.Style = borderStyle;
            range.Style.Border.Bottom.Style = borderStyle;
            range.Style.Border.Left.Style = borderStyle;
            range.Style.Border.Right.Style = borderStyle;
        }

        //设置边框颜色
        private static void SetBorderColor(ExcelRange range, Color topColor, Color bottomColor, Color leftColor, Color rightColor)
        {
            if (topColor != Color.Empty)
                range.Style.Border.Top.Color.SetColor(topColor);
            if (bottomColor != Color.Empty)
                range.Style.Border.Bottom.Color.SetColor(bottomColor);
            if (leftColor != Color.Empty)
                range.Style.Border.Left.Color.SetColor(leftColor);
            if (rightColor != Color.Empty)
                range.Style.Border.Right.Color.SetColor(rightColor);
        }

        private static void SetBorderColor(ExcelRange range, Color borderColor)
        {
            range.Style.Border.Top.Color.SetColor(borderColor);
            range.Style.Border.Bottom.Color.SetColor(borderColor);
            range.Style.Border.Left.Color.SetColor(borderColor);
            range.Style.Border.Right.Color.SetColor(borderColor);
        }

        //格式1：宋体
        private static void SetGeneral1(ExcelRange range, int fontSize, Color fontColor, Color backgroundColor)
        {
            SetGeneralStyle(range, "宋体", fontSize, fontColor, backgroundColor);
        }

        /// <summary>
        /// 格式1.1：背景浅蓝 字体蓝色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private static void SetGeneral1_1(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Blue, Color.FromArgb(153, 204, 255));
        }

        /// <summary>
        /// 格式1.2：背景浅青 字体蓝色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private static void SetGeneral1_2(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Blue, Color.FromArgb(204, 255, 204));
        }

        /// <summary>
        /// 格式1.3：背景白色 字体蓝色 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private static void SetGeneral1_3(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Blue, Color.White);
        }

        /// <summary>
        /// 格式1.4：背景淡黄 字体黑色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private static void SetGeneral1_4(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Black, Color.FromArgb(255, 255, 153));
        }

        /// <summary>
        /// 格式1.5：背景蓝绿 字体黑色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private static void SetGeneral1_5(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Black, Color.FromArgb(204, 255, 255));
        }

        /// <summary>
        /// 格式1.6：背景浅蓝 字体黑色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private static void SetGeneral1_6(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Black, Color.FromArgb(153, 204, 255));
        }

        /// <summary>
        /// 格式1.7：背景浅青 字体黑色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private static void SetGeneral1_7(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Black, Color.FromArgb(204, 255, 204));
        }

        /// <summary>
        /// 格式1.8：背景蓝绿 字体蓝色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private static void SetGeneral1_8(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Blue, Color.FromArgb(204, 255, 255));
        }

        /// <summary>
        /// 格式1.9：背景深绿 字体蓝色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private static void SetGeneral1_9(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Blue, Color.FromArgb(51, 204, 204));
        }

        /// <summary>
        /// 格式2.0：背景灰色 字体黑色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private static void SetGeneral2_0(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Black, Color.FromArgb(192, 192, 192));
        }

        /// <summary>
        /// 格式2.1：背景浅青 字体红色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private static void SetGeneral2_1(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Red, Color.FromArgb(204, 255, 204));
        }

        /// <summary>
        /// 格式2.2：背景灰色 字体蓝色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private static void SetGeneral2_2(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Blue, Color.FromArgb(192, 192, 192));
        }
    }
}
