using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.ConditionalFormatting;
using HandyControl.Tools.Extension;
using System.IO;

namespace TimeTrack_Pro.Model
{
    public class EppDemo
    {
        public static void demo1()
        {
            // 在 Excel 包类上使用许可证上下文属性
            // 删除许可证异常
            // 必须设置，否则会报错
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//非商业

            //创建一个新的Excel包
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // 设置单元格的值
                worksheet.Cells["A1"].Value = "标题";
                worksheet.Cells["A2"].Value = 123456789;
                worksheet.Cells["A3"].Value = DateTime.Now;               

                //应用样式
                ApplyStyles(worksheet);

                //保存Excel文件
                FileInfo file = new FileInfo(@"F:\文档\styledExcel.xlsx");
                package.SaveAs(file);
            }
        }

        public static void demo2()
        {
            // 在 Excel 包类上使用许可证上下文属性
            // 删除许可证异常
            // 必须设置，否则会报错
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//非商业

            //创建一个新的Excel包
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                worksheet.Rows[1].Height = 8;
                worksheet.Rows[26].Height = 8;
                worksheet.Columns[19].Width = 1;
                //应用样式
                

                //保存Excel文件
                FileInfo file = new FileInfo(@"F:\文档\styledExcelDemo2.xlsx");
                package.SaveAs(file);
            }
        }

        private static void ApplyStyles(ExcelWorksheet worksheet)
        {
            // 样式1：设置单元格的字体、颜色、边框、对齐方式
            SetGeneralStyle(worksheet.Cells["A1"]);

            // 样式2：设置数字格式
            SetNumberFormat(worksheet.Cells["A2"]);

            // 样式3：设置日期格式
            SetDateFormat(worksheet.Cells["A3"]);

            // 样式4：合并单元格并设置样式
            SetMergeCellsStyle(worksheet, "A4:B4");

            // 样式5：设置条件格式
            SetConditionalFormatting(worksheet.Cells["A5"]);

            // 样式6：应用数据验证
            SetDataValidation(worksheet.Cells["A6"]);

            // 样式7：设置自动调整列宽
            worksheet.Column(1).AutoFit();

        }

        // 设置单元格的字体、颜色、边框、对齐方式
        private static void SetGeneralStyle(ExcelRange range)
        {
            range.Style.Font.SetFromFont("Calibri", 12, true);
            range.Style.Font.Color.SetColor(Color.FromArgb(255, 64, 64));
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 192));
            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Top.Color.SetColor(Color.FromArgb(128, 128, 128));
            range.Style.Border.Bottom.Color.SetColor(Color.FromArgb(128, 128, 128));
            range.Style.Border.Left.Color.SetColor(Color.FromArgb(128, 128, 128));
            range.Style.Border.Right.Color.SetColor(Color.FromArgb(128, 128, 128));
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }

        // 设置数字格式
        private static void SetNumberFormat(ExcelRange range)
        {
            range.Style.Numberformat.Format = "#,##0.00";
        }

        // 设置日期格式
        private static void SetDateFormat(ExcelRange range)
        {
            range.Style.Numberformat.Format = "yyyy-mm-dd HH:mm:ss";
        }

        // 合并单元格并设置样式
        private static void SetMergeCellsStyle(ExcelWorksheet worksheet, string range)
        {
            worksheet.Cells[range].Merge = true;
            worksheet.Cells[range].Style.Font.Bold = true;
            worksheet.Cells[range].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        //设置条件格式：如果A5单元格的值大于100，则背景颜色为红色
        private static void SetConditionalFormatting(ExcelRange range)
        {
            var rule = range.Worksheet.ConditionalFormatting.AddGreaterThan(new ExcelAddress("A5"));
            rule.Formula = "100";
            rule.Style.Fill.PatternType = ExcelFillStyle.Solid;
            rule.Style.Fill.BackgroundColor.SetColor(Color.Red);
        }

        // 应用数据验证：A6单元格只能输入10到100之间的整数
        private static void SetDataValidation(ExcelRange range)
        {
            var dv = range.DataValidation.AddIntegerDataValidation();
            dv.Formula.Value = 10;
            dv.Formula2.Value = 100;
            dv.Operator = OfficeOpenXml.DataValidation.ExcelDataValidationOperator.between;
        }
    }
}
