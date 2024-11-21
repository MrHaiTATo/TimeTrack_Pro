using OfficeOpenXml;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TimeTrack_Pro.Model;

namespace TimeTrack_Pro.Code
{
    public class OriginalDataHandle
    {
        private List<OriginalData>? originalDatas;
        public List<OriginalData>? OriginalDatas { get { return originalDatas; } }
        
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
                originalDatas = new List<OriginalData>();
                for (int i = 0; ; i++)
                {
                    object ob = worksheet.Cells[$"A{2 + i * 4}"].Value;
                    if (ob == null) break;

                    message = worksheet.Cells[$"A{2 + i * 4}"].Value.ToString();
                    if (string.IsNullOrEmpty(message) || !message.Contains("登记号"))
                        break;

                    OriginalData data = new OriginalData();                    
                    
                    message = worksheet.Cells[$"C{2 + i * 4}"].Value.ToString();
                    if (string.IsNullOrEmpty(message) || !Regex.IsMatch(message, @"^[0-9]+$"))
                        continue;

                    data.Id = Convert.ToInt32(message);
                    message = worksheet.Cells[$"G{2 + i * 4}"].Value.ToString();
                    if (string.IsNullOrEmpty(message))
                        continue;

                    data.Name = message;
                    message = worksheet.Cells[$"L{2 + i * 4}"].Value.ToString();
                    if (string.IsNullOrEmpty(message))
                        continue;

                    data.Department = message;
                    message = worksheet.Cells[$"Q{2 + i * 4}"].Value.ToString();
                    if (string.IsNullOrEmpty(message))
                        continue;

                    data.RuleName = message;
                    data.Datas = new List<TimeSpan>[31];
                    for (int j = 0; j < 31; j++)
                    {
                        data.Datas[j] = new List<TimeSpan>();
                        message = worksheet.Cells[(i + 1)*4,j + 1].Value.ToString();
                        if (string.IsNullOrEmpty(message))
                            continue;

                        string[] times = message.Split(' ');
                        foreach (string time in times)
                        {
                            if (!Regex.IsMatch(time, @"^[0-9]{2}:[0-9]{2}$"))
                                continue;

                            data.Datas[j].Add(TimeSpan.Parse(time));
                        }
                    }
                    originalDatas.Add(data);
                }
            }
        }

        public void FilterByRule(AttendanceRule rule)
        {
            
        }
        
    }
}
