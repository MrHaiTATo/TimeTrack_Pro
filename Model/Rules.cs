using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace TimeTrack_Pro.Model
{
    public class Rules
    {
		private static List<AttendanceRule> list = new List<AttendanceRule>
        { 
			new AttendanceRule
			{
                RuleName = "月",
                Inter_dayTime = new TimeSpan(0, 0, 0),
                SerialNumber = 0,
                AlarmsTimes = 6,
                AttendanceWay = 0,
                StatsUnit = 0,
                StatsWay = 1,
                ShiftMode = 0,
                AllowLate = 0,
                AllowEarly = 0,
                Shifts = new Shift[][] { 
                    /*星期日*/
                    new Shift[3] {
                        new Shift { Id = 18, Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new Shift { Id = 19, Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new Shift { Id = 20, Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = -1 }
                    },
                    /*星期一*/
                    new Shift[3] {
                        new Shift { Id = 0, Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new Shift { Id = 1, Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new Shift { Id = 2, Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = -1 }
                    },
                    /*星期二*/
                    new Shift[3] {
                        new Shift { Id = 3, Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new Shift { Id = 4, Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new Shift { Id = 5, Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = -1 }
                    },
                    /*星期三*/
                    new Shift[3] {
                        new Shift { Id = 6, Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new Shift { Id = 7, Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new Shift { Id = 8, Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = -1 }
                    },
                    /*星期四*/
                    new Shift[3] {
                        new Shift { Id = 9, Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new Shift { Id = 10, Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new Shift { Id = 11, Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = -1 }
                    },
                    /*星期五*/
                    new Shift[3] {
                        new Shift { Id = 12, Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new Shift { Id = 13, Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new Shift { Id = 14, Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = -1 }
                    },
                    /*星期六*/
                    new Shift[3] {
                        new Shift { Id = 15, Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new Shift { Id = 16, Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new Shift { Id = 17, Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = -1 }
                    }
                }
            },
            new AttendanceRule
            {
                RuleName = "885",
                Inter_dayTime = new TimeSpan(0, 0, 0),
                SerialNumber = 1,
                AlarmsTimes = 7,
                AttendanceWay = 1,
                StatsUnit = 1,
                StatsWay = 0,
                ShiftMode = 0,
                AllowLate = 0,
                AllowEarly = 0,
                Shifts = new Shift[7][] { 
                    /*星期日*/
                    new Shift[3] {
                        new Shift { Id = 18, Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new Shift { Id = 19, Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new Shift { Id = 20, Name = "班段3", StartTime = new TimeSpan(19, 0, 0), EndTime = new TimeSpan(23, 0, 0), Type = 1 }
                    },
                    /*星期一*/
                    new Shift[3] {
                        new Shift { Id = 0, Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new Shift { Id = 1, Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new Shift { Id = 2, Name = "班段3", StartTime = new TimeSpan(19, 0, 0), EndTime = new TimeSpan(23, 0, 0), Type = 1 }
                    },
                    /*星期二*/
                    new Shift[3] {
                        new Shift { Id = 3, Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new Shift { Id = 4, Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new Shift { Id = 5, Name = "班段3", StartTime = new TimeSpan(19, 0, 0), EndTime = new TimeSpan(23, 0, 0), Type = 1 }
                    },
                    /*星期三*/
                    new Shift[3] {
                        new Shift { Id = 6, Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new Shift { Id = 7, Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new Shift { Id = 8, Name = "班段3", StartTime = new TimeSpan(19, 0, 0), EndTime = new TimeSpan(23, 0, 0), Type = 1 }
                    },
                    /*星期四*/
                    new Shift[3] {
                        new Shift { Id = 9, Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new Shift { Id = 10, Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new Shift { Id = 11, Name = "班段3", StartTime = new TimeSpan(19, 0, 0), EndTime = new TimeSpan(23, 0, 0), Type = 1 }
                    },
                    /*星期五*/
                    new Shift[3] {
                        new Shift { Id = 12, Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new Shift { Id = 13, Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new Shift { Id = 14, Name = "班段3", StartTime = new TimeSpan(19, 0, 0), EndTime = new TimeSpan(23, 0, 0), Type = 1 }
                    },
                    /*星期六*/
                    new Shift[3] {
                        new Shift { Id = 15, Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new Shift { Id = 16, Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new Shift { Id = 17, Name = "班段3", StartTime = new TimeSpan(19, 0, 0), EndTime = new TimeSpan(23, 0, 0), Type = 1 }
                    }
                }
            }
        };

		public static List<AttendanceRule> RuleList
		{
			get { return list; }
			set { list = value; }
		}

        public static void GetRuleList(string path)
        {
            try
            {
                _getRuleListFromXlsx(path);
            }
            catch 
            {
                _getRuleListFromXml(path);
            }
        }

        private static void _getRuleListFromXlsx(string path)
        {
            // 在 Excel 包类上使用许可证上下文属性
            // 删除许可证异常
            // 必须设置，否则会报错
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//非商业

            //创建一个新的Excel包
            using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
            {
                if (package.Workbook.Worksheets.Count() > 0)
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    string numStr = @"^[0-9]+$";
                    string timeStr = @"^[0-9]{2}:[0-9]{2}$";
                    int num = 0;
                    object obj;
                    AttendanceRule rule;
                    list.Clear();
                    while (true)
                    {
                        obj = worksheet.Cells[$"B{2 + num * 15}"].Value;
                        if (obj == null || !Regex.IsMatch(obj.ToString(), numStr))
                            break;
                        obj = worksheet.Cells[$"I{2 + num * 15}"].Value;
                        if (obj == null || !Regex.IsMatch(obj.ToString(), timeStr))
                            break;
                        obj = worksheet.Cells[$"D{2 + num * 15}"].Value;
                        if (obj == null)
                            break;
                        obj = worksheet.Cells[$"A{5 + num * 15}"].Value;
                        if (obj == null || !Regex.IsMatch(obj.ToString(), numStr))
                            break;
                        obj = worksheet.Cells[$"B{5 + num * 15}"].Value;
                        if (obj == null || !Regex.IsMatch(obj.ToString(), @"^[0-1]$"))
                            break;
                        obj = worksheet.Cells[$"D{5 + num * 15}"].Value;
                        if (obj == null || !Regex.IsMatch(obj.ToString(), numStr))
                            break;
                        obj = worksheet.Cells[$"E{5 + num * 15}"].Value;
                        if (obj == null || !Regex.IsMatch(obj.ToString(), @"^[0-1]$"))
                            break;
                        obj = worksheet.Cells[$"G{5 + num * 15}"].Value;
                        if (obj == null || !Regex.IsMatch(obj.ToString(), @"^[0-1]$"))
                            break;
                        obj = worksheet.Cells[$"I{5 + num * 15}"].Value;
                        if (obj == null || !Regex.IsMatch(obj.ToString(), numStr))
                            break;
                        obj = worksheet.Cells[$"J{5 + num * 15}"].Value;
                        if (obj == null || !Regex.IsMatch(obj.ToString(), numStr))
                            break;
                        rule = new AttendanceRule();
                        rule.SerialNumber = Convert.ToInt32(worksheet.Cells[$"B{2 + num * 15}"].Value);
                        rule.RuleName = worksheet.Cells[$"D{2 + num * 15}"].Value.ToString();
                        rule.Inter_dayTime = TimeSpan.Parse(worksheet.Cells[$"I{2 + num * 15}"].Value.ToString());
                        rule.AlarmsTimes = Convert.ToInt32(worksheet.Cells[$"A{5 + num * 15}"].Value);
                        rule.AttendanceWay = Convert.ToInt32(worksheet.Cells[$"B{5 + num * 15}"].Value);
                        rule.StatsUnit = Convert.ToInt32(worksheet.Cells[$"D{5 + num * 15}"].Value);
                        rule.StatsWay = Convert.ToInt32(worksheet.Cells[$"E{5 + num * 15}"].Value);
                        rule.ShiftMode = Convert.ToInt32(worksheet.Cells[$"G{5 + num * 15}"].Value);
                        rule.AllowLate = Convert.ToInt32(worksheet.Cells[$"I{5 + num * 15}"].Value);
                        rule.AllowEarly = Convert.ToInt32(worksheet.Cells[$"J{5 + num * 15}"].Value);
                        rule.Shifts = new Shift[7][];
                        Shift[] shifts;
                        for (int i = 0; i < 7; i++)
                        {
                            shifts = new Shift[3];
                            if (i < 6)
                                rule.Shifts[i + 1] = shifts;
                            else
                                rule.Shifts[0] = shifts;
                            for (int j = 0; j < 3; j++)
                            {
                                shifts[j] = new Shift();

                                shifts[j].Id = j + i * 3;
                                shifts[j].Name = string.Format($"班段{j + 1}");
                                obj = worksheet.Cells[$"{(char)('B' + j * 3)}{9 + num * 15 + i}"].Value;
                                if (obj != null && Regex.IsMatch(obj.ToString(), timeStr))
                                {
                                    shifts[j].StartTime = TimeSpan.Parse(obj.ToString());
                                }
                                else
                                {
                                    shifts[j].StartTime = TimeSpan.Zero;
                                }
                                obj = worksheet.Cells[$"{(char)('C' + j * 3)}{9 + num * 15 + i}"].Value;
                                if (obj != null && Regex.IsMatch(obj.ToString(), timeStr))
                                {
                                    shifts[j].EndTime = TimeSpan.Parse(obj.ToString());
                                }
                                else
                                {
                                    shifts[j].EndTime = TimeSpan.Zero;
                                }
                                obj = worksheet.Cells[$"{(char)('D' + j * 3)}{9 + num * 15 + i}"].Value;
                                if (obj != null && Regex.IsMatch(obj.ToString(), @"^[0-1]$"))
                                {
                                    shifts[j].Type = Convert.ToInt32(obj);
                                }
                                else
                                {
                                    shifts[j].Type = -1;
                                }
                            }
                        }
                        list.Add(rule);
                        num++;
                    }
                }
            }
        }

        private static void _getRuleListFromXml(string path)
        {                       
            XmlDocument document = new XmlDocument();
            document.Load(path);
            XmlElement table = document["Workbook"]["Worksheet"]["Table"];
            var rows = table.ChildNodes.Cast<XmlNode>().ToList().Where(n => n.Name == "Row");
            string numStr = @"^[0-9]+$";
            string timeStr = @"^[0-9]{2}:[0-9]{2}$";
            string str;
            list.Clear();
            for (int i = 0; i < rows.Count() / 15; i++)
            {
                AttendanceRule rule = new AttendanceRule();
                str = rows.ElementAt(1 + i * 15).ChildNodes[1].InnerText;
                if (string.IsNullOrEmpty(str) || !Regex.IsMatch(str, numStr))
                    continue;
                rule.SerialNumber = Convert.ToInt32(str);

                str = rows.ElementAt(1 + i * 15).ChildNodes[3].InnerText;
                if (string.IsNullOrEmpty(str))
                    continue;
                rule.RuleName = str;

                str = rows.ElementAt(1 + i * 15).ChildNodes[5].InnerText;
                if (string.IsNullOrEmpty(str) || !Regex.IsMatch(str, timeStr))
                    continue;
                rule.Inter_dayTime = TimeSpan.Parse(str);

                str = rows.ElementAt(4 + i * 15).ChildNodes[0].InnerText;
                if(string.IsNullOrEmpty(str) || !Regex.IsMatch(str, numStr))
                    continue;
                rule.AlarmsTimes = Convert.ToInt32(str);

                str = rows.ElementAt(4 + i * 15).ChildNodes[1].InnerText;
                if (string.IsNullOrEmpty(str) || !Regex.IsMatch(str, @"^[0-1]$"))
                    continue;
                rule.AttendanceWay = Convert.ToInt32(str);

                str = rows.ElementAt(4 + i * 15).ChildNodes[2].InnerText;
                if (string.IsNullOrEmpty(str) || !Regex.IsMatch(str, numStr))
                    continue;
                rule.StatsUnit = Convert.ToInt32(str);

                str = rows.ElementAt(4 + i * 15).ChildNodes[3].InnerText;
                if (string.IsNullOrEmpty(str) || !Regex.IsMatch(str, @"^[0-1]$"))
                    continue;
                rule.StatsWay = Convert.ToInt32(str);

                str = rows.ElementAt(4 + i * 15).ChildNodes[4].InnerText;
                if (string.IsNullOrEmpty(str) || !Regex.IsMatch(str, @"^[0-1]$"))
                    continue;
                rule.ShiftMode = Convert.ToInt32(str);

                str = rows.ElementAt(4 + i * 15).ChildNodes[5].InnerText;
                if (string.IsNullOrEmpty(str) || !Regex.IsMatch(str, numStr))
                    continue;
                rule.AllowLate = Convert.ToInt32(str);

                str = rows.ElementAt(4 + i * 15).ChildNodes[6].InnerText;
                if (string.IsNullOrEmpty(str) || !Regex.IsMatch(str, numStr))
                    continue;
                rule.AllowEarly = Convert.ToInt32(str);

                rule.Shifts = new Shift[7][];
                Shift[] shifts;
                for (int j = 0; j < 7; j++)
                {
                    shifts = new Shift[3];
                    if (j < 6)
                        rule.Shifts[j + 1] = shifts;
                    else
                        rule.Shifts[0] = shifts;
                    for (int k = 0; k < 3; k++)
                    {
                        shifts[k] = new Shift();

                        shifts[k].Id = k + j * 3;
                        shifts[k].Name = string.Format($"班段{k + 1}");
                        str = rows.ElementAt(8 + i * 15 + j).ChildNodes[1 + k * 3].InnerText;
                        if(!string.IsNullOrEmpty(str) && Regex.IsMatch(str, timeStr))
                        {
                            shifts[k].StartTime = TimeSpan.Parse(str);
                        }
                        else
                        {
                            shifts[k].StartTime = TimeSpan.Zero;
                        }

                        str = rows.ElementAt(8 + i * 15 + j).ChildNodes[2 + k * 3].InnerText;
                        if (!string.IsNullOrEmpty(str) && Regex.IsMatch(str, timeStr))
                        {
                            shifts[k].EndTime = TimeSpan.Parse(str);
                        }
                        else
                        {
                            shifts[k].EndTime = TimeSpan.Zero;
                        }

                        str = rows.ElementAt(8 + i * 15 + j).ChildNodes[3 + k * 3].InnerText;
                        if (!string.IsNullOrEmpty(str) && Regex.IsMatch(str, @"^[0-1]$"))
                        {
                            shifts[k].Type = Convert.ToInt32(str);
                        }
                        else
                        {
                            shifts[k].Type = -1;
                        }
                    }
                }
                list.Add(rule);
            }                        
        }

    }
}
