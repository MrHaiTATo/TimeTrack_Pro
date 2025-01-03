using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrack_Pro.Model;
using static NPOI.HSSF.Util.HSSFColor;

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
            IFont stb_14_b = FontHelper.STBlue(workbook, 14, true);
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
                    row.CreateCell(c);                
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
            ISheet sheet = null;
            Creat_init();
            if (workbook == null)
                return;
            //自定义颜色                       
            byte[] lightBlue = new byte[] { 204, 255, 255 };                                
            //预设字体
            IFont st_18_b = FontHelper.STBlue(workbook, 18, true);
            IFont st_10 = FontHelper.STBlue(workbook, 10);
            //预设单元格样式
            ICellStyle[] cellStyles = new ICellStyle[7];
            cellStyles[0] = CellStyleHelper.CustomStyle(workbook, st_18_b, lightBlue, IndexedColors.Green.RGB, false, true, true, true);
            cellStyles[1] = CellStyleHelper.CustomStyle(workbook, st_10, lightBlue, IndexedColors.Green.RGB, false, false, true, false);
            cellStyles[2] = CellStyleHelper.CustomStyle(workbook, st_10, lightBlue, IndexedColors.Green.RGB, false, false, false, false);
            cellStyles[3] = CellStyleHelper.CustomStyle(workbook, st_10, lightBlue, IndexedColors.Green.RGB, false, false, false, true);
            cellStyles[4] = CellStyleHelper.CustomStyle(workbook, st_10, lightBlue, IndexedColors.Green.RGB, true, true, true, true);
            cellStyles[5] = CellStyleHelper.Style1(workbook, st_10, IndexedColors.Green.Index, true, true, true, true);
            cellStyles[6] = CellStyleHelper.Style4(workbook, st_10, true);
            List<short> styles = new List<short>();
            for (int i = 0; i < cellStyles.Count(); i++)
            {
                styles.Add(cellStyles[i].Index);
            }
            sheet = workbook.CreateSheet("考勤汇总");
            CreatAttendanceSummarySheet(sheet, sheetModel, styles.ToArray());
            Save();
        }

        private void CreatAttendanceSummarySheet(ISheet sheet, SummarySheetModel sheetModel, short[] styles)
        {
            for (int r = 0; r < 4; r++)
            {
                IRow row = sheet.CreateRow(r);
                for (int c = 0; c < 24; c++)
                {
                    row.CreateCell(c);
                }
            }
            //设置列宽
            for (int c = 4; c < 10; c++)
            {
                sheet.SetColumnWidth(c, 5.5 * 256);                
            }
            sheet.SetColumnWidth(14, 5.5 * 256);
            //设置行高            
            sheet.GetRow(0).Height = 527;
            sheet.GetRow(1).Height = 357;
            //合并单元格
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 23));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 1));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 3, 6));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 10, 23));
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 0, 0));
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 1, 1));
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 2, 2));
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 3, 3));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 4, 5));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 6, 7));
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 8, 8));
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 9, 9));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 10, 11));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 12, 13));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 14, 15));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 16, 18));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 19, 21));
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 22, 22));
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 23, 23));
            //设置样式和数据填充
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[0]), 0, 1, 0, 24);
            sheet.GetRow(0).Cells[0].SetCellValue("考\x20勤\x20汇\x20总\x20表");
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[1]), 1, 1, 0, 2);
            sheet.GetRow(1).Cells[0].SetCellValue("统计日期:");
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[2]), 1, 1, 2, 8);
            sheet.GetRow(1).Cells[3].SetCellValue(sheetModel.Date);
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[3]), 1, 1, 10, 14);
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[5]), 2, 2, 0, 24);
            sheet.GetRow(2).Cells[0].SetCellValue("工号");
            sheet.GetRow(2).Cells[1].SetCellValue("姓名");
            sheet.GetRow(2).Cells[2].SetCellValue("部门");
            sheet.GetRow(2).Cells[3].SetCellValue("班次");
            sheet.GetRow(2).Cells[4].SetCellValue("出勤(天)");
            sheet.GetRow(3).Cells[4].SetCellValue("标准");
            sheet.GetRow(3).Cells[5].SetCellValue("实际");
            sheet.GetRow(2).Cells[6].SetCellValue("请假(天)");
            sheet.GetRow(3).Cells[6].SetCellValue("事假");
            sheet.GetRow(3).Cells[7].SetCellValue("病假");
            sheet.GetRow(2).Cells[8].SetCellValue("旷工(天)");
            sheet.GetRow(2).Cells[9].SetCellValue("出差(天)");
            sheet.GetRow(2).Cells[10].SetCellValue("工作(天)");
            sheet.GetRow(3).Cells[10].SetCellValue("标准");
            sheet.GetRow(3).Cells[11].SetCellValue("实际");
            sheet.GetRow(2).Cells[12].SetCellValue("加班(时分)");
            sheet.GetRow(3).Cells[12].SetCellValue("正常");
            sheet.GetRow(3).Cells[13].SetCellValue("特殊");
            sheet.GetRow(2).Cells[14].SetCellValue("迟到/早退");
            sheet.GetRow(3).Cells[14].SetCellValue("次");
            sheet.GetRow(3).Cells[15].SetCellValue("分");
            sheet.GetRow(2).Cells[16].SetCellValue("加\x20项\x20工\x20资");
            sheet.GetRow(3).Cells[16].SetCellValue("标准");
            sheet.GetRow(3).Cells[17].SetCellValue("加班");
            sheet.GetRow(3).Cells[18].SetCellValue("津贴");
            sheet.GetRow(2).Cells[19].SetCellValue("减\x20项\x20工\x20资");
            sheet.GetRow(3).Cells[19].SetCellValue("迟早");
            sheet.GetRow(3).Cells[20].SetCellValue("事假");
            sheet.GetRow(3).Cells[21].SetCellValue("扣款");
            sheet.GetRow(2).Cells[22].SetCellValue("实\x20际\x20工\x20资");
            sheet.GetRow(2).Cells[23].SetCellValue("备\x20注");
            int dataRow = 4;
            for (int r = 0; r < sheetModel.Datas.Count(); r++)
            {
                IRow row = sheet.CreateRow(dataRow + r);
                for (int c = 0; c < 24; c++)
                {
                    row.CreateCell(c);
                }
                if (r % 2 == 0)
                {
                    SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[4]), dataRow + r, 1, 0, 24);
                }
                else
                {
                    SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[5]), dataRow + r, 1, 0, 16);
                    SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[6]), dataRow + r, 1, 16, 8);
                }
                row.Cells[0].SetCellValue(sheetModel.Datas[r].Id);
                row.Cells[1].SetCellValue(sheetModel.Datas[r].Name);
                row.Cells[2].SetCellValue(sheetModel.Datas[r].Department);
                row.Cells[3].SetCellValue(sheetModel.Datas[r].RuleName);
                row.Cells[4].SetCellValue(sheetModel.Datas[r].StdAtd);
                row.Cells[5].SetCellValue(sheetModel.Datas[r].AtlAtd);
                row.Cells[6].SetCellValue(sheetModel.Datas[r].MtrVct);
                row.Cells[7].SetCellValue(sheetModel.Datas[r].SkeVct);
                row.Cells[8].SetCellValue(sheetModel.Datas[r].Absentee);
                row.Cells[9].SetCellValue(sheetModel.Datas[r].Errand);
                row.Cells[10].SetCellValue(sheetModel.Datas[r].StdWorkTime);
                row.Cells[11].SetCellValue(sheetModel.Datas[r].AtlWorkTime);
                row.Cells[12].SetCellValue(sheetModel.Datas[r].Wko_Common);
                row.Cells[13].SetCellValue(sheetModel.Datas[r].Wko_Special);
                row.Cells[14].SetCellValue(sheetModel.Datas[r].LateEarly_Count);
                row.Cells[15].SetCellValue(sheetModel.Datas[r].LateEarly_Min);
                row.Cells[16].SetCellValue(sheetModel.Datas[r].AddWages_Std);
                row.Cells[17].SetCellValue(sheetModel.Datas[r].AddWages_WorkOt);
                row.Cells[18].SetCellValue(sheetModel.Datas[r].AddWages_Sbd);
                row.Cells[19].SetCellValue(sheetModel.Datas[r].SubWages_LateEarly);
                row.Cells[20].SetCellValue(sheetModel.Datas[r].SubWages_MtrVct);
                row.Cells[21].SetCellValue(sheetModel.Datas[r].SubWages_CutPay);
                row.Cells[22].SetCellValue(sheetModel.Datas[r].AtlPay);
                row.Cells[23].SetCellValue(sheetModel.Datas[r].Notes);
            }
        }

        public void CreatAtdExpSheet(ExceptionSheetModel sheetModel)
        {
            ISheet sheet = null;
            Creat_init();
            if (workbook == null)
                return;
            //自定义颜色                       
            byte[] lightBlue = new byte[] { 204, 255, 255 };
            byte[] lightGreen = new byte[] { 204, 255, 204 };
            byte[] lightYellow = new byte[] { 255, 255, 153 };
            //预设字体
            IFont st_18_b = FontHelper.STBlack(workbook, 18, true);
            IFont st_10_b = FontHelper.STBlack(workbook, 10, true);
            IFont st_10 = FontHelper.STBlack(workbook, 10);
            //预设单元格样式
            ICellStyle[] cellStyles = new ICellStyle[8];
            cellStyles[0] = CellStyleHelper.CustomStyle(workbook, st_18_b, lightBlue, IndexedColors.Green.RGB, false, true, true, true);
            cellStyles[1] = CellStyleHelper.CustomStyle(workbook, st_10, lightBlue, IndexedColors.Green.RGB, false, false, true, false);
            cellStyles[2] = CellStyleHelper.CustomStyle(workbook, st_10_b, lightBlue, IndexedColors.Green.RGB, false, false, false, false);
            cellStyles[3] = CellStyleHelper.CustomStyle(workbook, st_10, lightBlue, IndexedColors.Green.RGB, false, false, false, true);
            cellStyles[4] = CellStyleHelper.CustomStyle(workbook, st_10, lightBlue, IndexedColors.Green.RGB, true, true, true, true);
            cellStyles[5] = CellStyleHelper.Style1(workbook, st_10, IndexedColors.Green.Index, true, true, true, true);
            cellStyles[6] = CellStyleHelper.CustomStyle(workbook, st_10, lightGreen, IndexedColors.Green.RGB, true, true, true, true);
            cellStyles[7] = CellStyleHelper.CustomStyle(workbook, st_10, lightYellow, IndexedColors.Green.RGB, true, true, true, true);
            List<short> styles = new List<short>();
            for (int i = 0; i < cellStyles.Count(); i++)
            {
                styles.Add(cellStyles[i].Index);
            }
            sheet = workbook.CreateSheet("异常考勤");
            CreatAttendanceExceptionSheet(sheet, sheetModel, styles.ToArray());
            Save();
        }

        private void CreatAttendanceExceptionSheet(ISheet sheet, ExceptionSheetModel sheetModel, short[] styles)
        {
            for (int r = 0; r < 4; r++)
            {
                IRow row = sheet.CreateRow(r);
                for (int c = 0; c < 13; c++)
                {
                    row.CreateCell(c);
                }
            }
            //设置列宽
            sheet.SetColumnWidth(12, 30 * 256);
            //设置行高            
            sheet.GetRow(0).Height = 527;
            //合并单元格
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 12));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 1));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 2, 3));
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 0, 0));
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 1, 1));
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 2, 2));
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 3, 3));
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 4, 4));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 5, 6));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 7, 8));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 9, 10));
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 11, 11));
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 12, 12));
            //设置样式和数据填充
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[0]), 0, 1, 0, 13);
            sheet.GetRow(0).Cells[0].SetCellValue("异\x20常\x20考\x20勤\x20统\x20计\x20表");
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[1]), 1, 1, 0, 2);
            sheet.GetRow(1).Cells[0].SetCellValue("统计日期:");
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[2]), 1, 1, 2, 10);
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[3]), 1, 1, 12, 1);
            sheet.GetRow(1).Cells[2].SetCellValue(sheetModel.Date);
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[5]), 2, 2, 0, 11);
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[6]), 2, 2, 11, 2);
            sheet.GetRow(2).Cells[0].SetCellValue("工号");
            sheet.GetRow(2).Cells[1].SetCellValue("姓名");
            sheet.GetRow(2).Cells[2].SetCellValue("部门");
            sheet.GetRow(2).Cells[3].SetCellValue("班次");
            sheet.GetRow(2).Cells[4].SetCellValue("日期");
            sheet.GetRow(2).Cells[5].SetCellValue("班段1");
            sheet.GetRow(3).Cells[5].SetCellValue("上班");
            sheet.GetRow(3).Cells[6].SetCellValue("下班");
            sheet.GetRow(2).Cells[7].SetCellValue("班段2");
            sheet.GetRow(3).Cells[7].SetCellValue("上班");
            sheet.GetRow(3).Cells[8].SetCellValue("下班");
            sheet.GetRow(2).Cells[9].SetCellValue("班段3");
            sheet.GetRow(3).Cells[9].SetCellValue("上班");
            sheet.GetRow(3).Cells[10].SetCellValue("下班");
            sheet.GetRow(2).Cells[11].SetCellValue("迟到/早退(分)");
            sheet.GetRow(2).Cells[12].SetCellValue("备注");            
            int dataRow = 4;
            for (int r = 0; r < sheetModel.Datas.Count(); r++)
            {                
                for (int d = 0; d < sheetModel.Datas[r].Parts.Count(); d++)
                {
                    IRow row = sheet.CreateRow(dataRow);
                    for (int c = 0; c < 13; c++)
                    {
                        row.CreateCell(c);
                    }
                    SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[4]), dataRow, 1, 0, 5);
                    SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[7]), dataRow, 1, 5, 8);
                    row.Cells[0].SetCellValue(sheetModel.Datas[r].Id);
                    row.Cells[1].SetCellValue(sheetModel.Datas[r].Name);
                    row.Cells[2].SetCellValue(sheetModel.Datas[r].Department);
                    row.Cells[3].SetCellValue(sheetModel.Datas[r].RuleName);
                    row.Cells[4].SetCellValue(sheetModel.Datas[r].Parts[d].Date);
                    row.Cells[5].SetCellValue(sheetModel.Datas[r].Parts[d].ESignUpDatas[0]);
                    row.Cells[6].SetCellValue(sheetModel.Datas[r].Parts[d].ESignUpDatas[1]);
                    row.Cells[7].SetCellValue(sheetModel.Datas[r].Parts[d].ESignUpDatas[2]);
                    row.Cells[8].SetCellValue(sheetModel.Datas[r].Parts[d].ESignUpDatas[3]);
                    row.Cells[9].SetCellValue(sheetModel.Datas[r].Parts[d].ESignUpDatas[4]);
                    row.Cells[10].SetCellValue(sheetModel.Datas[r].Parts[d].ESignUpDatas[5]);
                    row.Cells[11].SetCellValue(sheetModel.Datas[r].Parts[d].LateOrEarly);
                    row.Cells[12].SetCellValue(sheetModel.Datas[r].Parts[d].Notes);
                    dataRow++;
                }
            }
        }

        public void CreatAtdOrgSheet(OriginalSheetModel sheetModel)
        {
            ISheet sheet = null;
            Creat_init();
            if (workbook == null)
                return;
            //自定义颜色                       
            byte[] lightBlue = new byte[] { 204, 255, 255 };
            byte[] Grey = new byte[] { 192, 192, 192 };            
            //预设字体
            IFont st_20_b = FontHelper.STBlack(workbook, 20, true);
            IFont st_11_b = FontHelper.STBlack(workbook, 11, true);
            IFont st_12 = FontHelper.STBlack(workbook, 12);
            IFont st_10 = FontHelper.STBlack(workbook, 10);
            IFont st_8 = FontHelper.STBlack(workbook, 8);
            //预设单元格样式
            ICellStyle[] cellStyles = new ICellStyle[6];
            cellStyles[0] = CellStyleHelper.Style0(workbook, st_20_b, IndexedColors.Black.Index, false, true, true, true);
            cellStyles[1] = CellStyleHelper.Style0(workbook, st_11_b, IndexedColors.Black.Index, false, false, true, true);
            cellStyles[1].Alignment = HorizontalAlignment.Left;
            cellStyles[2] = CellStyleHelper.CustomStyle(workbook, st_10, lightBlue, IndexedColors.Black.RGB, true, true, true, true);
            cellStyles[3] = CellStyleHelper.CustomStyle(workbook, st_8, lightBlue, IndexedColors.Black.RGB, true, true, true, true);
            cellStyles[3].VerticalAlignment = VerticalAlignment.Top;
            cellStyles[4] = CellStyleHelper.CustomStyle(workbook, st_10, Grey, IndexedColors.Black.RGB, true, true, true, true);
            cellStyles[5] = CellStyleHelper.CustomStyle(workbook, st_12, Grey, IndexedColors.Black.RGB, true, true, true, true);

            List<short> styles = new List<short>();
            for (int i = 0; i < cellStyles.Count(); i++)
            {
                styles.Add(cellStyles[i].Index);
            }
            sheet = workbook.CreateSheet("考勤原始表");
            CreatOriginalAttendanceSheet(sheet, sheetModel, styles.ToArray());
            Save();
        }

        private void CreatOriginalAttendanceSheet(ISheet sheet, OriginalSheetModel sheetModel, short[] styles)
        {
            int days = DateTimeHelper.GetDays(sheetModel.Date.Year, sheetModel.Date.Month);
            for (int r = 0; r < 2; r++)
            {
                IRow row = sheet.CreateRow(r);
                for (int c = 0; c < days; c++)
                {
                    row.CreateCell(c);
                }
            }
            //设置列宽
            for (int i = 0; i < days; i++)
            {
                sheet.SetColumnWidth(i, 5.3 * 256);
            }           
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, days - 1));
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, days - 1));
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[0]), 0, 1, 0, days);
            sheet.GetRow(0).Cells[0].SetCellValue("考\x20勤\x20原\x20始\x20表");
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[1]), 1, 1, 0, days);
            sheet.GetRow(1).Cells[0].SetCellValue("统计日期：" + sheetModel.Date.ToString("yyyy/MM"));
            int dataRow = 2;
            for (int d = 0; d < sheetModel.Datas.Count(); d++)
            {
                for (int r = 0; r < 4; r++)
                {
                    IRow row = sheet.CreateRow(dataRow + r);
                    for (int c = 0; c < days; c++)
                    {
                        row.CreateCell(c);
                    }
                }
                sheet.AddMergedRegion(new CellRangeAddress(dataRow, dataRow, 0, 1));
                sheet.AddMergedRegion(new CellRangeAddress(dataRow, dataRow, 2, 3));
                sheet.AddMergedRegion(new CellRangeAddress(dataRow, dataRow, 4, 5));
                sheet.AddMergedRegion(new CellRangeAddress(dataRow, dataRow, 6, 8));
                sheet.AddMergedRegion(new CellRangeAddress(dataRow, dataRow, 9, 10));
                sheet.AddMergedRegion(new CellRangeAddress(dataRow, dataRow, 11, 13));
                sheet.AddMergedRegion(new CellRangeAddress(dataRow, dataRow, 14, 15));
                sheet.AddMergedRegion(new CellRangeAddress(dataRow, dataRow, 16, 18));
                sheet.AddMergedRegion(new CellRangeAddress(dataRow, dataRow, 19, days - 1));
                SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[4]), dataRow, 1, 0, 2);
                sheet.GetRow(dataRow).Cells[0].SetCellValue("登记号：");
                SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[2]), dataRow, 1, 2, 2);
                sheet.GetRow(dataRow).Cells[2].SetCellValue(sheetModel.Datas[d].Id);
                SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[4]), dataRow, 1, 4, 2);
                sheet.GetRow(dataRow).Cells[4].SetCellValue("姓名：");
                SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[2]), dataRow, 1, 6, 3);
                sheet.GetRow(dataRow).Cells[6].SetCellValue(sheetModel.Datas[d].Name);
                SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[4]), dataRow, 1, 9, 2);
                sheet.GetRow(dataRow).Cells[9].SetCellValue("部门：");
                SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[2]), dataRow, 1, 11, 3);
                sheet.GetRow(dataRow).Cells[11].SetCellValue(sheetModel.Datas[d].Department);
                SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[4]), dataRow, 1, 14, 2);
                sheet.GetRow(dataRow).Cells[14].SetCellValue("班次：");
                SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[2]), dataRow, 1, 16, 3);
                sheet.GetRow(dataRow).Cells[16].SetCellValue(sheetModel.Datas[d].RuleName);
                SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[4]), dataRow, 1, 19, days - 19);
                sheet.GetRow(dataRow).Cells[19].SetCellValue("注：浅青色区域为数据区");
                SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[5]), dataRow + 1, 1, 0, days);
                SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[3]), dataRow + 2, 1, 0, days);
                for (int i = 0; i < days; i++)
                {
                    string orgdata = "";
                    foreach (var item in sheetModel.Datas[d].Datas[i])
                    {
                        orgdata += string.Format("{0:00}:{1:00} ",item.Hour, item.Minute);
                    }
                    sheet.GetRow(dataRow + 1).Cells[i].SetCellValue(i + 1);
                    sheet.GetRow(dataRow + 2).Cells[i].SetCellValue(orgdata);
                }
                dataRow += 4;
            }
        }

        public void CreatAtdSchedulingSheet(AtdRuleModel atdRule)
        {
            ISheet sheet = null;
            Creat_init();
            if (workbook == null)
                return;
            //自定义颜色                       
            byte[] lightGreen = new byte[] { 204, 255, 204 };
            byte[] lightYellow = new byte[] { 255, 255, 153 };
            byte[] Grey = new byte[] { 192, 192, 192 };
            //预设字体
            IFont stBlue_10 = FontHelper.STBlue(workbook, 10, true);
            IFont stRed_10 = FontHelper.STRed(workbook, 10);
            IFont stBlack_10 = FontHelper.STBlack(workbook, 10, true);
            IFont stBlue_8 = FontHelper.STBlue(workbook, 8, true);
            //预设单元格样式
            ICellStyle[] cellStyles = new ICellStyle[5];
            cellStyles[0] = CellStyleHelper.CustomStyle(workbook, stRed_10, lightGreen, IndexedColors.Black.RGB, true, true, true, true);
            cellStyles[1] = CellStyleHelper.CustomStyle(workbook, stBlack_10, Grey, IndexedColors.Black.RGB, true, true, true, true);
            cellStyles[2] = CellStyleHelper.CustomStyle(workbook, stBlack_10, Grey, IndexedColors.Black.RGB, false, true, true, true);
            cellStyles[3] = CellStyleHelper.CustomStyle(workbook, stBlue_8, Grey, IndexedColors.Black.RGB, true, false, true, true);
            cellStyles[4] = CellStyleHelper.CustomStyle(workbook, stBlue_10, lightYellow, IndexedColors.Black.RGB, true, true, true, true);          

            List<short> styles = new List<short>();
            for (int i = 0; i < cellStyles.Count(); i++)
            {
                styles.Add(cellStyles[i].Index);
            }
            sheet = workbook.CreateSheet("排班表");
            IRow row = sheet.CreateRow(0);
            for (int c = 0; c < 10; c++)
            {
                sheet.SetColumnWidth(c, 11.12 * 256);
                row.CreateCell(c).CellStyle = cellStyles[4];
            }
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 9));
            row.Cells[0].SetCellValue("绿色区域为编辑区域，下一个班次需要本班次后面留一个空 白行后拷贝复制下表填写(不包括本行)");
            for (int i = 0; i < atdRule.Datas.Count(); i++)
            {
                CreatAttendanceSchedulingSheet(sheet, atdRule.Datas[i], styles.ToArray(), 0, 1 + i * 15);
            }            
            Save();
        }

        private void CreatAttendanceSchedulingSheet(ISheet sheet, AttendanceRule rule, short[] styles, int x, int y)
        {
            for (int r = 0; r < 15; r++)
            {
                IRow row = sheet.CreateRow(r + y);
                for (int c = 0; c < 10; c++)
                {
                    row.CreateCell(c);
                }
            }
            //合并单元格
            sheet.AddMergedRegion(new CellRangeAddress(y, y, x + 3, x + 6));
            sheet.AddMergedRegion(new CellRangeAddress(y, y, x + 8, x + 9));
            sheet.AddMergedRegion(new CellRangeAddress(y + 1, y + 1, x + 1, x + 2));
            sheet.AddMergedRegion(new CellRangeAddress(y + 1, y + 1, x + 4, x + 5));
            sheet.AddMergedRegion(new CellRangeAddress(y + 1, y + 1, x + 6, x + 7));
            sheet.AddMergedRegion(new CellRangeAddress(y + 2, y + 2, x + 1, x + 2));
            sheet.AddMergedRegion(new CellRangeAddress(y + 2, y + 2, x + 4, x + 5));
            sheet.AddMergedRegion(new CellRangeAddress(y + 2, y + 2, x + 6, x + 7));
            sheet.AddMergedRegion(new CellRangeAddress(y + 3, y + 3, x + 1, x + 2));
            sheet.AddMergedRegion(new CellRangeAddress(y + 3, y + 3, x + 4, x + 5));
            sheet.AddMergedRegion(new CellRangeAddress(y + 3, y + 3, x + 6, x + 7));
            sheet.AddMergedRegion(new CellRangeAddress(y + 4, y + 4, x, x + 9));
            sheet.AddMergedRegion(new CellRangeAddress(y + 5, y + 5, x + 1, x + 3));
            sheet.AddMergedRegion(new CellRangeAddress(y + 5, y + 5, x + 4, x + 6));
            sheet.AddMergedRegion(new CellRangeAddress(y + 5, y + 5, x + 7, x + 9));
            //设置样式和数据填充
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[1]), y, 1, x, 1);
            sheet.GetRow(y).Cells[x].SetCellValue("序号：");
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[0]), y, 1, x + 1, 1);
            sheet.GetRow(y).Cells[x + 1].SetCellValue(rule.SerialNumber);
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[1]), y, 1, x + 2, 1);
            sheet.GetRow(y).Cells[x + 2].SetCellValue("名称：");
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[0]), y, 1, x + 3, 4);
            sheet.GetRow(y).Cells[x + 3].SetCellValue(rule.RuleName);
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[1]), y, 1, x + 7, 1);
            sheet.GetRow(y).Cells[x + 7].SetCellValue("跨天时间：");
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[0]), y, 1, x + 8, 2);
            sheet.GetRow(y).Cells[x + 8].SetCellValue(string.Format("{0:00}:{1:00}",rule.Inter_dayTime.Hours, rule.Inter_dayTime.Minutes));
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[2]), y + 1, 1, x, 10);
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[3]), y + 2, 1, x, 10);
            sheet.GetRow(y + 1).Cells[x].SetCellValue("闹铃次数");
            sheet.GetRow(y + 1).Cells[x + 1].SetCellValue("考勤方式");
            sheet.GetRow(y + 1).Cells[x + 3].SetCellValue("统计单位");
            sheet.GetRow(y + 1).Cells[x + 4].SetCellValue("统计方式");
            sheet.GetRow(y + 1).Cells[x + 6].SetCellValue("换班时间");
            sheet.GetRow(y + 1).Cells[x + 8].SetCellValue("允许迟到");
            sheet.GetRow(y + 1).Cells[x + 9].SetCellValue("允许早退");
            sheet.GetRow(y + 2).Cells[x].SetCellValue("(Times)");
            sheet.GetRow(y + 2).Cells[x + 1].SetCellValue("(0:连续考勤\x20"+"1:非连续考勤)");
            sheet.GetRow(y + 2).Cells[x + 3].SetCellValue("(分钟\x20M)");
            sheet.GetRow(y + 2).Cells[x + 4].SetCellValue("(0:统计时间\x20"+"1:考勤时间)");
            sheet.GetRow(y + 2).Cells[x + 6].SetCellValue("(换班分割线\x20"+"0:1/2\x20"+"1:1/3)");
            sheet.GetRow(y + 2).Cells[x + 8].SetCellValue("(分钟\x20M)");
            sheet.GetRow(y + 2).Cells[x + 9].SetCellValue("(分钟\x20M)");
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[0]), y + 3, 1, x, 10);
            sheet.GetRow(y + 3).Cells[x].SetCellValue(rule.AlarmsTimes);
            sheet.GetRow(y + 3).Cells[x + 1].SetCellValue(rule.AttendanceWay);
            sheet.GetRow(y + 3).Cells[x + 3].SetCellValue(rule.StatsUnit);
            sheet.GetRow(y + 3).Cells[x + 4].SetCellValue(rule.StatsWay);
            sheet.GetRow(y + 3).Cells[x + 6].SetCellValue(rule.ShiftMode);
            sheet.GetRow(y + 3).Cells[x + 8].SetCellValue(rule.AllowLate);
            sheet.GetRow(y + 3).Cells[x + 9].SetCellValue(rule.AllowEarly);
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[1]), y + 4, 3, x, 10);
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[1]), y + 7, 7, x, 1);
            SetMergedStyle(sheet, workbook.GetCellStyleAt(styles[0]), y + 7, 7, x + 1, 9);
            sheet.GetRow(y + 4).Cells[x].SetCellValue("周\x20考\x20勤\x20设\x20置");
            sheet.GetRow(y + 5).Cells[x + 1].SetCellValue("班段1");
            sheet.GetRow(y + 5).Cells[x + 4].SetCellValue("班段2");
            sheet.GetRow(y + 5).Cells[x + 7].SetCellValue("班段3");
            sheet.GetRow(y + 6).Cells[x + 1].SetCellValue("上班");
            sheet.GetRow(y + 6).Cells[x + 2].SetCellValue("下班");
            sheet.GetRow(y + 6).Cells[x + 3].SetCellValue("类型");
            sheet.GetRow(y + 6).Cells[x + 4].SetCellValue("上班");
            sheet.GetRow(y + 6).Cells[x + 5].SetCellValue("下班");
            sheet.GetRow(y + 6).Cells[x + 6].SetCellValue("类型");
            sheet.GetRow(y + 6).Cells[x + 7].SetCellValue("上班");
            sheet.GetRow(y + 6).Cells[x + 8].SetCellValue("下班");
            sheet.GetRow(y + 6).Cells[x + 9].SetCellValue("类型");
            sheet.GetRow(y + 7).Cells[x].SetCellValue("周一");
            sheet.GetRow(y + 8).Cells[x].SetCellValue("周二");
            sheet.GetRow(y + 9).Cells[x].SetCellValue("周三");
            sheet.GetRow(y + 10).Cells[x].SetCellValue("周四");
            sheet.GetRow(y + 11).Cells[x].SetCellValue("周五");
            sheet.GetRow(y + 12).Cells[x].SetCellValue("周六");
            sheet.GetRow(y + 13).Cells[x].SetCellValue("周日");
            int week = 1;
            for (int w = 0; w < 7; w++)
            {
                week = week % 7;
                sheet.GetRow(y + w + 7).Cells[x + 1].SetCellValue(string.Format("{0:00}:{1:00}", rule.Classes[week][0].StartTime.Hours, rule.Classes[week][0].StartTime.Minutes));
                sheet.GetRow(y + w + 7).Cells[x + 2].SetCellValue(string.Format("{0:00}:{1:00}", rule.Classes[week][0].EndTime.Hours, rule.Classes[week][0].EndTime.Minutes));
                sheet.GetRow(y + w + 7).Cells[x + 3].SetCellValue(rule.Classes[week][0].Type);
                sheet.GetRow(y + w + 7).Cells[x + 4].SetCellValue(string.Format("{0:00}:{1:00}", rule.Classes[week][1].StartTime.Hours, rule.Classes[week][0].StartTime.Minutes));
                sheet.GetRow(y + w + 7).Cells[x + 5].SetCellValue(string.Format("{0:00}:{1:00}", rule.Classes[week][1].EndTime.Hours, rule.Classes[week][0].EndTime.Minutes));
                sheet.GetRow(y + w + 7).Cells[x + 6].SetCellValue(rule.Classes[week][1].Type);
                sheet.GetRow(y + w + 7).Cells[x + 7].SetCellValue(string.Format("{0:00}:{1:00}", rule.Classes[week][2].StartTime.Hours, rule.Classes[week][0].StartTime.Minutes));
                sheet.GetRow(y + w + 7).Cells[x + 8].SetCellValue(string.Format("{0:00}:{1:00}", rule.Classes[week][2].EndTime.Hours, rule.Classes[week][0].EndTime.Minutes));
                sheet.GetRow(y + w + 7).Cells[x + 9].SetCellValue(rule.Classes[week][2].Type);
                week++;
            }
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
                    if (workbook != null)
                    {
                        workbook.Dispose();
                        workbook = null;
                        disposed = true;
                    }
                }
            }
        }
    }
}
