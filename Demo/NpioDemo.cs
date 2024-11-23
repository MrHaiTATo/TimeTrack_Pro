using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Demo
{
    public class NpioDemo
    {
        public void GenerateExcelWithComplexStyles()
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Complex Styles Demo");

            // 设置字体
            IFont boldFont = workbook.CreateFont();
            boldFont.FontName = "Arial";
            boldFont.IsBold = true;
            boldFont.FontHeightInPoints = 16;

            // 设置单元格样式
            ICellStyle headerStyle = workbook.CreateCellStyle();
            headerStyle.SetFont(boldFont);
            headerStyle.Alignment = HorizontalAlignment.Center;
            headerStyle.VerticalAlignment = VerticalAlignment.Center;
            headerStyle.FillForegroundColor = IndexedColors.LightYellow.Index;
            headerStyle.FillPattern = FillPattern.SolidForeground;
            headerStyle.BorderBottom = BorderStyle.Thin;
            headerStyle.BorderLeft = BorderStyle.Thin;
            headerStyle.BorderRight = BorderStyle.Thin;
            headerStyle.BorderTop = BorderStyle.Thin;

            // 设置单元格数字格式
            ICellStyle decimalStyle = workbook.CreateCellStyle();
            decimalStyle.DataFormat = workbook.CreateDataFormat().GetFormat("#,##0.00");

            // 创建列标题
            IRow headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("ID");
            headerRow.GetCell(0).CellStyle = headerStyle;
            headerRow.CreateCell(1).SetCellValue("Product Name");
            headerRow.GetCell(1).CellStyle = headerStyle;
            headerRow.CreateCell(2).SetCellValue("Price");
            headerRow.GetCell(2).CellStyle = headerStyle;

            // 添加数据行
            for (int i = 1; i <= 5; i++)
            {
                IRow dataRow = sheet.CreateRow(i);
                dataRow.CreateCell(0).SetCellValue(i);
                dataRow.CreateCell(1).SetCellValue("Product " + i);
                dataRow.CreateCell(2).CellStyle = decimalStyle;
                dataRow.GetCell(2).SetCellValue(i * 100.23);
            }

            // 合并单元格
            CellRangeAddress merged = new CellRangeAddress(
                0, 0,
                0, 1);
            sheet.AddMergedRegion(merged);

            // 写入文件
            using (FileStream file = new FileStream(@"F:\文档\output.xlsx", FileMode.Create))
            {
                workbook.Write(file);
            }

            workbook.Close();
        }
    }
}
