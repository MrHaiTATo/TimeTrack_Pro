using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using TimeTrack_Pro.Model;

namespace TimeTrack_Pro.Code
{
    public class Rules
    {
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
                Classes = new ClassSection[][] { 
                    /*星期日*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 }
                    },
                    /*星期一*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 }
                    },
                    /*星期二*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 }
                    },
                    /*星期三*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 }
                    },
                    /*星期四*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 }
                    },
                    /*星期五*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 }
                    },
                    /*星期六*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(0, 0, 0), Type = 0 }
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
                StatsUnit = 0,
                StatsWay = 0,
                ShiftMode = 0,
                AllowLate = 0,
                AllowEarly = 0,
                Classes = new ClassSection[7][] { 
                    /*星期日*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(19, 0, 0), EndTime = new TimeSpan(23, 0, 0), Type = 1 }
                    },
                    /*星期一*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(19, 0, 0), EndTime = new TimeSpan(23, 0, 0), Type = 1 }
                    },
                    /*星期二*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(19, 0, 0), EndTime = new TimeSpan(23, 0, 0), Type = 1 }
                    },
                    /*星期三*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(19, 0, 0), EndTime = new TimeSpan(23, 0, 0), Type = 1 }
                    },
                    /*星期四*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(19, 0, 0), EndTime = new TimeSpan(23, 0, 0), Type = 1 }
                    },
                    /*星期五*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(19, 0, 0), EndTime = new TimeSpan(23, 0, 0), Type = 1 }
                    },
                    /*星期六*/
                    new ClassSection[3] {
                        new ClassSection { Name = "班段1", StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(12, 0, 0), Type = 0 },
                        new ClassSection { Name = "班段2", StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 15, 0), Type = 0 },
                        new ClassSection { Name = "班段3", StartTime = new TimeSpan(19, 0, 0), EndTime = new TimeSpan(23, 0, 0), Type = 1 }
                    }
                }
            }
        };

        public static List<AttendanceRule> RuleList
        {
            get { return list; }
            //set { list = value; }
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
                        rule.Classes = new ClassSection[7][];
                        ClassSection[] classes;
                        for (int i = 0; i < 7; i++)
                        {
                            classes = new ClassSection[3];
                            if (i < 6)
                                rule.Classes[i + 1] = classes;
                            else
                                rule.Classes[0] = classes;
                            for (int j = 0; j < 3; j++)
                            {
                                classes[j] = new ClassSection();

                                classes[j].Name = string.Format($"班段{j + 1}");
                                obj = worksheet.Cells[$"{(char)('B' + j * 3)}{9 + num * 15 + i}"].Value;
                                if (obj != null && Regex.IsMatch(obj.ToString(), timeStr))
                                {
                                    classes[j].StartTime = TimeSpan.Parse(obj.ToString());
                                }
                                else
                                {
                                    classes[j].StartTime = TimeSpan.Zero;
                                }
                                obj = worksheet.Cells[$"{(char)('C' + j * 3)}{9 + num * 15 + i}"].Value;
                                if (obj != null && Regex.IsMatch(obj.ToString(), timeStr))
                                {
                                    classes[j].EndTime = TimeSpan.Parse(obj.ToString());
                                }
                                else
                                {
                                    classes[j].EndTime = TimeSpan.Zero;
                                }
                                obj = worksheet.Cells[$"{(char)('D' + j * 3)}{9 + num * 15 + i}"].Value;
                                if (obj != null && Regex.IsMatch(obj.ToString(), @"^[0-1]$"))
                                {
                                    classes[j].Type = Convert.ToInt32(obj);
                                }
                                else
                                {
                                    classes[j].Type = 0;
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
            XmlElement? table = document["Workbook"]?["Worksheet"]?["Table"];
            var rows = table?.ChildNodes.Cast<XmlNode>().ToList().Where(n => n.Name == "Row");
            string numStr = @"^[0-9]+$";
            string timeStr = @"^[0-9]{2}:[0-9]{2}$";
            string str;
            list.Clear();
            for (int i = 0; i < rows?.Count() / 15; i++)
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
                if (string.IsNullOrEmpty(str) || !Regex.IsMatch(str, numStr))
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

                rule.Classes = new ClassSection[7][];
                ClassSection[] classes;
                for (int j = 0; j < 7; j++)
                {
                    classes = new ClassSection[3];
                    if (j < 6)
                        rule.Classes[j + 1] = classes;
                    else
                        rule.Classes[0] = classes;
                    for (int k = 0; k < 3; k++)
                    {
                        classes[k] = new ClassSection();

                        classes[k].Name = string.Format($"班段{k + 1}");
                        str = rows.ElementAt(8 + i * 15 + j).ChildNodes[1 + k * 3].InnerText;
                        if (!string.IsNullOrEmpty(str) && Regex.IsMatch(str, timeStr))
                        {
                            classes[k].StartTime = TimeSpan.Parse(str);
                        }
                        else
                        {
                            classes[k].StartTime = TimeSpan.Zero;
                        }

                        str = rows.ElementAt(8 + i * 15 + j).ChildNodes[2 + k * 3].InnerText;
                        if (!string.IsNullOrEmpty(str) && Regex.IsMatch(str, timeStr))
                        {
                            classes[k].EndTime = TimeSpan.Parse(str);
                        }
                        else
                        {
                            classes[k].EndTime = TimeSpan.Zero;
                        }

                        str = rows.ElementAt(8 + i * 15 + j).ChildNodes[3 + k * 3].InnerText;
                        if (!string.IsNullOrEmpty(str) && Regex.IsMatch(str, @"^[0-1]$"))
                        {
                            classes[k].Type = Convert.ToInt32(str);
                        }
                        else
                        {
                            classes[k].Type = 0;
                        }
                    }
                }
                list.Add(rule);
            }
        }

        public static AtdRuleModel GetRuleModel()
        {
            AtdRuleModel atdRuleModel = new AtdRuleModel();
            atdRuleModel.Datas = RuleList;
            return atdRuleModel;
        }
    }
}
