using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrack_Pro.Model;

namespace TimeTrack_Pro.Helper.NPOI
{
    public class ExcelHelper : IDisposable
    {
        private bool disposed;
        private IWorkbook workbook = null;
        private string fileName = null;
        private FileStream fs = null;

        public string FilePath
        {
            set { fileName = value; }
            get { return fileName; }
        }

        public ExcelHelper()
        {            
            disposed = false;
        }

        private void Creat_init()
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                Work_init();
            }
        }

        private void Work_init()
        {
            if (workbook != null)
            {
                workbook.Dispose();
                workbook = null;
            }
            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook();
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook();
        }

        private void Save()
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                workbook.Write(fs);
                fs.Close();
                fs = null;
            }
        }

        public void CreateAtdStatiSheets(StatisticsSheetModel statistics)
        {
            ISheet sheet = null;
            Creat_init();
            if (workbook == null)
                return;
            //预设字体
            IFont stb_9 = FontHelper.STBlue(workbook, 9);
            IFont stb_10 = FontHelper.STBlue(workbook, 10);
            IFont stb_14_b = FontHelper.STBlueBlod(workbook, 14);
            IFont str_9 = FontHelper.STRed(workbook, 9);
            //预设单元格样式
            ICellStyle[] cellStyles = new ICellStyle[15];
            cellStyles[0] = CellStyleHelper.Style0(workbook, stb_10, false);
            cellStyles[1] = CellStyleHelper.Style0(workbook, stb_9, true, true, true, false);
            cellStyles[2] = CellStyleHelper.Style0(workbook, str_9, true, true, true, false);
            cellStyles[3] = CellStyleHelper.Style1(workbook, stb_9, false);
            cellStyles[4] = CellStyleHelper.Style1(workbook, stb_9, true, true, true, false);
            cellStyles[5] = CellStyleHelper.Style1(workbook, stb_10, true, true, true, false);
            cellStyles[6] = CellStyleHelper.Style1(workbook, stb_10, false);
            cellStyles[7] = CellStyleHelper.Style1(workbook, stb_14_b, false, false, true, false);
            cellStyles[8] = CellStyleHelper.Style1(workbook, str_9, true, true, true, false);
            cellStyles[9] = CellStyleHelper.Style1(workbook, stb_9, true, true, false, true);
            cellStyles[10] = CellStyleHelper.Style2(workbook, stb_10, true, true, true, false);
            cellStyles[11] = CellStyleHelper.Style2(workbook, stb_9, true, true, true, false);
            cellStyles[12] = CellStyleHelper.Style1(workbook, stb_9, false, true, true, false);
            cellStyles[13] = CellStyleHelper.Style1(workbook, stb_9, false, false, true, false);
            cellStyles[14] = CellStyleHelper.Style0(workbook, stb_9, false, true, true, false);
            List<short> styles = new List<short>();
            for (int i = 0; i < cellStyles.Length; i++)
            {
                styles.Add(cellStyles[i].Index);
            }            
            foreach (var data in statistics.Datas)
            {
                sheet = workbook.CreateSheet(data.Name + "_" + data.Id);
                CreatAtdStatiSheet(sheet, data, styles.ToArray());
            }
            Save();
        }

        private void CreatAtdStatiSheet(ISheet sheet, StatisticsData data, short[] styles)
        {            
            //设置列宽
            for (int c = 0; c < 18; c++)
            {
                sheet.SetColumnWidth(c, 6 * 256);
            }
            sheet.SetColumnWidth(18, 256);
            //创建行并设置行高
            for (int r = 0; r < 26; r++)
            {
                IRow row = sheet.CreateRow(r);
                if (r == 0 || r == 5 || r == 25)
                    row.Height = 136;
                if (r == 1)
                    row.Height = 419;
                if ((r >= 2 && r <= 4) || (r >= 7 && r <= 24))
                    row.Height = 283;
                if (r == 6)
                    row.Height = 391;
                for (int c = 0; c < 19; c++)
                {
                    ICell cell = row.CreateCell(c);                
                }
            }
            //合并单元格                      
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 17));
            sheet.AddMergedRegion(new CellRangeAddress(5, 5, 0, 17));
            sheet.AddMergedRegion(new CellRangeAddress(6, 6, 0, 17));
            sheet.AddMergedRegion(new CellRangeAddress(25, 25, 0, 17));
            sheet.AddMergedRegion(new CellRangeAddress(0, 25, 18, 18));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 3));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 5, 6));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 8, 10));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 12, 14));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 16, 17));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 1));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 2, 3));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 4, 5));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 6, 7));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 8, 9));
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 10, 10));
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 11, 11));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 12, 17));
            sheet.AddMergedRegion(new CellRangeAddress(3, 3, 16, 17));
            sheet.AddMergedRegion(new CellRangeAddress(7, 8, 0, 0));
            sheet.AddMergedRegion(new CellRangeAddress(7, 7, 1, 2));
            sheet.AddMergedRegion(new CellRangeAddress(7, 7, 3, 4));
            sheet.AddMergedRegion(new CellRangeAddress(7, 7, 5, 6));
            sheet.AddMergedRegion(new CellRangeAddress(7, 7, 7, 8));
            sheet.AddMergedRegion(new CellRangeAddress(7, 8, 9, 9));
            sheet.AddMergedRegion(new CellRangeAddress(7, 7, 10, 11));
            sheet.AddMergedRegion(new CellRangeAddress(7, 7, 12, 13));
            sheet.AddMergedRegion(new CellRangeAddress(7, 7, 14, 15));
            sheet.AddMergedRegion(new CellRangeAddress(7, 7, 16, 17));
            //设置样式
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[12]), 0, 1, 0, 18);                        
            sheet.GetRow(1).Cells[0].CellStyle = workbook.GetCellStyleAt(styles[13]);
            sheet.GetRow(1).Cells[0].SetCellValue("姓名");
            sheet.GetRow(1).Cells[1].CellStyle = workbook.GetCellStyleAt(styles[0]);
            sheet.GetRow(1).Cells[1].SetCellValue(data.Name);
            sheet.GetRow(1).Cells[4].CellStyle = workbook.GetCellStyleAt(styles[3]);
            sheet.GetRow(1).Cells[4].SetCellValue("工号");
            sheet.GetRow(1).Cells[5].CellStyle = workbook.GetCellStyleAt(styles[0]);
            sheet.GetRow(1).Cells[5].SetCellValue(data.Id);
            sheet.GetRow(1).Cells[7].CellStyle = workbook.GetCellStyleAt(styles[3]);
            sheet.GetRow(1).Cells[7].SetCellValue("部门");
            sheet.GetRow(1).Cells[8].CellStyle = workbook.GetCellStyleAt(styles[0]);
            sheet.GetRow(1).Cells[8].SetCellValue(data.Department);
            sheet.GetRow(1).Cells[11].CellStyle = workbook.GetCellStyleAt(styles[3]);
            sheet.GetRow(1).Cells[11].SetCellValue("班次");
            sheet.GetRow(1).Cells[12].CellStyle = workbook.GetCellStyleAt(styles[0]);
            sheet.GetRow(1).Cells[12].SetCellValue(data.RuleName);
            sheet.GetRow(1).Cells[15].CellStyle = workbook.GetCellStyleAt(styles[3]);
            sheet.GetRow(1).Cells[15].SetCellValue("日期");
            sheet.GetRow(1).Cells[16].CellStyle = workbook.GetCellStyleAt(styles[0]);
            sheet.GetRow(1).Cells[16].SetCellValue(data.Date);
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[4]), 2, 2, 0, 18);            
            sheet.GetRow(2).Cells[0].SetCellValue("出勤(天)");            
            sheet.GetRow(2).Cells[2].SetCellValue("工作时间(时分)");            
            sheet.GetRow(2).Cells[4].SetCellValue("加班(时分)");            
            sheet.GetRow(2).Cells[6].SetCellValue("迟到/早退");            
            sheet.GetRow(2).Cells[8].SetCellValue("请假(时分)");            
            sheet.GetRow(2).Cells[10].SetCellValue("旷工(时分)");           
            sheet.GetRow(2).Cells[11].SetCellValue("出差(时分)");            
            sheet.GetRow(2).Cells[12].SetCellValue("工资");
            sheet.GetRow(3).Cells[0].SetCellValue("实际");            
            sheet.GetRow(3).Cells[1].SetCellValue("标准");
            sheet.GetRow(3).Cells[2].SetCellValue("实际");
            sheet.GetRow(3).Cells[3].SetCellValue("标准");
            sheet.GetRow(3).Cells[4].SetCellValue("普通");
            sheet.GetRow(3).Cells[5].SetCellValue("特殊");
            sheet.GetRow(3).Cells[6].SetCellValue("次");
            sheet.GetRow(3).Cells[7].SetCellValue("分");
            sheet.GetRow(3).Cells[8].SetCellValue("带薪假");
            sheet.GetRow(3).Cells[9].SetCellValue("无薪假");
            sheet.GetRow(3).Cells[12].SetCellValue("日薪");
            sheet.GetRow(3).Cells[13].SetCellValue("加班");
            sheet.GetRow(3).Cells[14].SetCellValue("扣款");
            sheet.GetRow(3).Cells[15].SetCellValue("其他");
            sheet.GetRow(3).Cells[16].SetCellValue("合计");
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[14]), 4, 1, 0, 18);
            sheet.GetRow(4).Cells[0].SetCellValue(data.AtlAtd);
            sheet.GetRow(4).Cells[1].SetCellValue(data.StdAtd);
            sheet.GetRow(4).Cells[2].SetCellValue(data.AtlWorkTime);
            sheet.GetRow(4).Cells[3].SetCellValue(data.StdWorkTime);            
            sheet.GetRow(4).Cells[4].SetCellValue(data.Wko_Common);
            sheet.GetRow(4).Cells[5].SetCellValue(data.Wko_Special);
            sheet.GetRow(4).Cells[6].SetCellValue(data.LateEarly_Count);
            sheet.GetRow(4).Cells[7].SetCellValue(data.LateEarly_Min);
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[13]), 5, 1, 0, 18);
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[7]), 6, 1, 0, 18);
            sheet.GetRow(6).Cells[0].SetCellValue("考\x20\x20勤\x20\x20表");           
            for (int i = 0; i < 2; i++)
            {
                SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[5]), 7, 2, 0 + i * 9, 7);
                SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[4]), 9, 16, 0 + i * 9, 1);
                SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[1]), 9, 16, 1 + i * 9, 6);
                SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[10]), 7, 2, 7 + i * 9, 2);
                SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[11]), 9, 16, 7 + i * 9, 2);
                sheet.GetRow(7).Cells[0 + i * 9].SetCellValue("日\x20星期\x20期");
                sheet.GetRow(7).Cells[1 + i * 9].SetCellValue("班段1");
                sheet.GetRow(7).Cells[3 + i * 9].SetCellValue("班段2");
                sheet.GetRow(7).Cells[5 + i * 9].SetCellValue("班段3");
                sheet.GetRow(7).Cells[7 + i * 9].SetCellValue("日\x20统\x20计");
                sheet.GetRow(8).Cells[1 + i * 9].SetCellValue("上班");
                sheet.GetRow(8).Cells[2 + i * 9].SetCellValue("下班");
                sheet.GetRow(8).Cells[3 + i * 9].SetCellValue("上班");
                sheet.GetRow(8).Cells[4 + i * 9].SetCellValue("下班");
                sheet.GetRow(8).Cells[5 + i * 9].SetCellValue("签到");
                sheet.GetRow(8).Cells[6 + i * 9].SetCellValue("签退");
                sheet.GetRow(8).Cells[7 + i * 9].SetCellValue("工作");
                sheet.GetRow(8).Cells[8 + i * 9].SetCellValue("加班");
                for (int d = 0; d < 16; d++)
                {
                    if (d + i * 16 < data.DaysOfWeek.Count())
                    {
                        sheet.GetRow(9 + d).Cells[0 + i * 9].SetCellValue(data.DaysOfWeek[d + i * 16]);
                        if (data.DaysOfWeek[d + i * 16].Contains("日") || data.DaysOfWeek[d + i * 16].Contains("六"))
                            sheet.GetRow(9 + d).Cells[0 + i * 9].CellStyle = workbook.GetCellStyleAt(styles[8]);
                    }                    
                    for (int k = 0; k < 8; k++)
                    {
                        sheet.GetRow(9 + d).Cells[1 + k + i * 9].SetCellValue(data.SignUpDatas[d + i * 16][k].Text);
                        if(data.SignUpDatas[d + i * 16][k].Color == System.Drawing.Color.Red)
                            sheet.GetRow(9 + d).Cells[1 + k + i * 9].CellStyle = workbook.GetCellStyleAt(styles[2]);
                    }
                }
            }
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[4]), 25, 1, 0, 18);
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[9]), 0, 26, 18, 1);
        }

        public void CreatAtdSumSheet(SummarySheetModel sheetModel)
        {

        }

        private void CreatAttendanceSummarySheet(ISheet sheet, SummarySheetModel sheetModel)
        {

        }

        private void SetMergedStyle(ISheet sheet, ICellStyle style, int firstRow, int rowCount,int firstColumn, int columnCount)
        {
            for (int r = 0; r < rowCount; r++)
            {
                IRow row = sheet.GetRow(firstRow + r);
                for (int c = 0; c < columnCount; c++)
                {
                    row.Cells[firstColumn + c].CellStyle = style;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                }
            }
        }
    }
}
