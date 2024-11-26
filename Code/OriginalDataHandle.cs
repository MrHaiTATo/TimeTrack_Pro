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
            // 在 Excel 包类上使用许可证上下文属性
            // 删除许可证异常
            // 必须设置，否则会报错
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//非商业

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

                            data.Datas[j].Add(DateTime.Parse(time));
                        }
                    }
                    originalDatas.Datas.Add(data);
                }
            }
        }

        public List<Employee> GetTypeDatas(int year, int month, int Type)
        {
            List<Employee> employees = new List<Employee>();
            Employee one = null;
            AttendanceRule rule = null;
            foreach (var org in OriginalDatas.Datas)
            {
                if (Rules.RuleList.Count() > 0 && (Rules.RuleList.Find(r => r.RuleName == org.RuleName) != null))
                {
                    rule = Rules.RuleList.Find(r => r.RuleName == org.RuleName);
                }
                else
                    rule = Rules.DefaultRule;
                if (Type == 0)
                    one = new StatisticsData(org);
                else if (Type == 1)
                    one = new SummaryData(org);
                else 
                    one = new ExceptionData(org);
                for (int d = 0; d < org.Datas.Count(); d++)
                {
                    int week = DateTimeHelper.GetWeek(year, month, d + 1);
                    ClassSection section;
                    TimeSpan time1_s, time1_e, time2_s, time2_e, time3_s, time3_e;
                    foreach (var t in org.Datas[d])
                    {
                        if (t.TimeOfDay <= (rule.Classes[week][0].StartTime + new TimeSpan(0,rule.StatsUnit + rule.AllowLate,0)))
                        {

                        }
                    }
                }           
                employees.Add(one);
            }
            return employees;
        }
        
    }
}
