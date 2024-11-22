﻿using NPOI.SS.UserModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrack_Pro.Code;
using TimeTrack_Pro.Model;

namespace TimeTrack_Pro.Helper.EPPlus
{
    public class ExcelHelper : IDisposable
    {
        private bool disposed;
        private string fileName = null;
        private FileStream fs = null;
        ExcelPackage package = null;
        public ExcelHelper(string fileName)
        {
            this.fileName = fileName;
            disposed = false;
            // 在 Excel 包类上使用许可证上下文属性
            // 删除许可证异常
            // 必须设置，否则会报错
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private void Creat_init()
        {
            if (package == null)
            {
                package = new ExcelPackage();
            }
        }

        private void Save()
        {
            if (!string.IsNullOrEmpty(fileName) && fileName.IndexOf(".xlsx") > 0)
            {
                fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                package.SaveAs(fs);
                fs.Close();
                fs = null;
            }
        }

        public void CreateAtdStatiSheet(StatisticsSheetModel sheetModel)
        {
            ExcelWorksheet worksheet = null;
            Creat_init();
            foreach (var data in sheetModel.Datas)
            {
                worksheet = package.Workbook.Worksheets.Add(data.Name + "_" + data.Id);
                CreateAttendanceStatisticsSheet(worksheet, data);
            }
            Save();
        }

        private void CreateAttendanceStatisticsSheet(ExcelWorksheet worksheet, StatisticsData statistic)
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
            string[] days = statistic.DaysOfWeek;
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
                        if (statistic.SignUpDatas[j + i * 16][k].Color != Color.Empty)
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

        public void CreatAtdSumSheet(SummarySheetModel sheetModel)
        {
            ExcelWorksheet worksheet = null;
            Creat_init();            
            worksheet = package.Workbook.Worksheets.Add("考勤汇总");
            CreatAttendanceSummarySheet(worksheet, sheetModel);           
            Save();
        }

        private void CreatAttendanceSummarySheet(ExcelWorksheet worksheet, SummarySheetModel sheetModel)
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
            worksheet.Cells["D2:G2"].Value = sheetModel.Date;
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

            for (int i = 0; i < sheetModel.Datas.Count(); i++)                                        
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
                worksheet.Cells[5 + i, 1].Value = sheetModel.Datas[i].Id;
                worksheet.Cells[5 + i, 2].Value = sheetModel.Datas[i].Name;
                worksheet.Cells[5 + i, 3].Value = sheetModel.Datas[i].Department;
                worksheet.Cells[5 + i, 4].Value = sheetModel.Datas[i].RuleName;
                worksheet.Cells[5 + i, 5].Value = sheetModel.Datas[i].StdAtd;
                worksheet.Cells[5 + i, 6].Value = sheetModel.Datas[i].AtlAtd;
                worksheet.Cells[5 + i, 7].Value = sheetModel.Datas[i].MtrVct;
                worksheet.Cells[5 + i, 8].Value = sheetModel.Datas[i].SkeVct;
                worksheet.Cells[5 + i, 9].Value = sheetModel.Datas[i].Absentee;
                worksheet.Cells[5 + i, 10].Value = sheetModel.Datas[i].Errand;
                worksheet.Cells[5 + i, 11].Value = sheetModel.Datas[i].StdWorkTime;
                worksheet.Cells[5 + i, 12].Value = sheetModel.Datas[i].AtlWorkTime;
                worksheet.Cells[5 + i, 13].Value = sheetModel.Datas[i].Wko_Common;
                worksheet.Cells[5 + i, 14].Value = sheetModel.Datas[i].Wko_Special;
                worksheet.Cells[5 + i, 15].Value = sheetModel.Datas[i].LateEarly_Count;
                worksheet.Cells[5 + i, 16].Value = sheetModel.Datas[i].LateEarly_Min;
                worksheet.Cells[5 + i, 17].Value = sheetModel.Datas[i].AddWages_Std;
                worksheet.Cells[5 + i, 18].Value = sheetModel.Datas[i].AddWages_WorkOt;
                worksheet.Cells[5 + i, 19].Value = sheetModel.Datas[i].AddWages_Sbd;
                worksheet.Cells[5 + i, 20].Value = sheetModel.Datas[i].SubWages_LateEarly;
                worksheet.Cells[5 + i, 21].Value = sheetModel.Datas[i].SubWages_MtrVct;
                worksheet.Cells[5 + i, 22].Value = sheetModel.Datas[i].SubWages_CutPay;
                worksheet.Cells[5 + i, 23].Value = sheetModel.Datas[i].AtlPay;
                worksheet.Cells[5 + i, 24].Value = sheetModel.Datas[i].Notes;
            }
        }

        public void CreatAtdExpSheet(ExceptionSheetModel sheetModel)
        {
            ExcelWorksheet worksheet = null;
            Creat_init();
            worksheet = package.Workbook.Worksheets.Add("异常考勤");
            CreatAttendanceExceptionSheet(worksheet, sheetModel);
            Save();
        }

        private void CreatAttendanceExceptionSheet(ExcelWorksheet worksheet, ExceptionSheetModel sheetModel)
        {
            (string, string)[] values;
            string seat;
            worksheet.Columns[13].Width = 30;
            //第一行
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
            worksheet.Cells["C2:D2"].Value = sheetModel.Date;

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

            for (int i = 0; i < sheetModel.Datas.Count(); i++)
            {
                seat = $"A{5 + i}:E{5 + i}";
                SetGeneral1_5(worksheet.Cells[seat], 10);
                SetBorderCellStyle(worksheet.Cells[seat]);
                SetBorderColor(worksheet.Cells[seat], Color.Green);

                seat = $"F{5 + i}:M{5 + i}";
                SetGeneral1_4(worksheet.Cells[seat], 10);
                SetBorderCellStyle(worksheet.Cells[seat]);
                SetBorderColor(worksheet.Cells[seat], Color.Green);

                worksheet.Cells[5 + i, 1].Value = sheetModel.Datas[i].Id;
                worksheet.Cells[5 + i, 2].Value = sheetModel.Datas[i].Name;
                worksheet.Cells[5 + i, 3].Value = sheetModel.Datas[i].Department;
                worksheet.Cells[5 + i, 4].Value = sheetModel.Datas[i].RuleName;
                worksheet.Cells[5 + i, 5].Value = sheetModel.Datas[i].Date;
                worksheet.Cells[5 + i, 6].Value = sheetModel.Datas[i].ESignUpDatas[0];
                worksheet.Cells[5 + i, 7].Value = sheetModel.Datas[i].ESignUpDatas[1];
                worksheet.Cells[5 + i, 8].Value = sheetModel.Datas[i].ESignUpDatas[2];
                worksheet.Cells[5 + i, 9].Value = sheetModel.Datas[i].ESignUpDatas[3];
                worksheet.Cells[5 + i, 10].Value = sheetModel.Datas[i].ESignUpDatas[4];
                worksheet.Cells[5 + i, 11].Value = sheetModel.Datas[i].ESignUpDatas[5];
                worksheet.Cells[5 + i, 12].Value = sheetModel.Datas[i].LateOrEarly;
                worksheet.Cells[5 + i, 13].Value = sheetModel.Datas[i].Notes;
            }
        }

        /// <summary>
        /// 设置单元格的字体、颜色、对齐方式
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontName"></param>
        /// <param name="fontSize"></param>
        /// <param name="fontColor"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="horizontalAlignment"></param>
        /// <param name="verticalAlignment"></param>
        /// <param name="bold"></param>
        /// <param name="italic"></param>
        /// <param name="underLine"></param>
        /// <param name="strikeout"></param>
        private void SetGeneralStyle(ExcelRange range, string fontName, int fontSize, Color fontColor, Color backgroundColor,
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

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="range"></param>
        private void SetMergeCellsStyle(ExcelWorksheet worksheet, string range)
        {
            worksheet.Cells[range].Merge = true;
        }

        /// <summary>
        /// 设置边框
        /// </summary>
        /// <param name="range"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        private void SetBorderCellStyle(ExcelRange range, ExcelBorderStyle top = ExcelBorderStyle.Thin, ExcelBorderStyle bottom = ExcelBorderStyle.Thin, ExcelBorderStyle left = ExcelBorderStyle.Thin, ExcelBorderStyle right = ExcelBorderStyle.Thin)
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

        /// <summary>
        /// 设置边框颜色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="topColor"></param>
        /// <param name="bottomColor"></param>
        /// <param name="leftColor"></param>
        /// <param name="rightColor"></param>
        private void SetBorderColor(ExcelRange range, Color topColor, Color bottomColor, Color leftColor, Color rightColor)
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

        private void SetBorderColor(ExcelRange range, Color borderColor)
        {
            range.Style.Border.Top.Color.SetColor(borderColor);
            range.Style.Border.Bottom.Color.SetColor(borderColor);
            range.Style.Border.Left.Color.SetColor(borderColor);
            range.Style.Border.Right.Color.SetColor(borderColor);
        }

        /// <summary>
        /// 格式1：宋体
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        /// <param name="fontColor"></param>
        /// <param name="backgroundColor"></param>
        private void SetGeneral1(ExcelRange range, int fontSize, Color fontColor, Color backgroundColor)
        {
            SetGeneralStyle(range, "宋体", fontSize, fontColor, backgroundColor);
        }

        /// <summary>
        /// 格式1.1：背景浅蓝 字体蓝色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private void SetGeneral1_1(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Blue, Color.FromArgb(153, 204, 255));
        }

        /// <summary>
        /// 格式1.2：背景浅青 字体蓝色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private void SetGeneral1_2(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Blue, Color.FromArgb(204, 255, 204));
        }

        /// <summary>
        /// 格式1.3：背景白色 字体蓝色 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private void SetGeneral1_3(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Blue, Color.White);
        }

        /// <summary>
        /// 格式1.4：背景淡黄 字体黑色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private void SetGeneral1_4(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Black, Color.FromArgb(255, 255, 153));
        }

        /// <summary>
        /// 格式1.5：背景蓝绿 字体黑色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private void SetGeneral1_5(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Black, Color.FromArgb(204, 255, 255));
        }

        /// <summary>
        /// 格式1.6：背景浅蓝 字体黑色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private void SetGeneral1_6(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Black, Color.FromArgb(153, 204, 255));
        }

        /// <summary>
        /// 格式1.7：背景浅青 字体黑色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private void SetGeneral1_7(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Black, Color.FromArgb(204, 255, 204));
        }

        /// <summary>
        /// 格式1.8：背景蓝绿 字体蓝色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private void SetGeneral1_8(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Blue, Color.FromArgb(204, 255, 255));
        }

        /// <summary>
        /// 格式1.9：背景深绿 字体蓝色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private void SetGeneral1_9(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Blue, Color.FromArgb(51, 204, 204));
        }

        /// <summary>
        /// 格式2.0：背景灰色 字体黑色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private void SetGeneral2_0(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Black, Color.FromArgb(192, 192, 192));
        }

        /// <summary>
        /// 格式2.1：背景浅青 字体红色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private void SetGeneral2_1(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Red, Color.FromArgb(204, 255, 204));
        }

        /// <summary>
        /// 格式2.2：背景灰色 字体蓝色
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fontSize"></param>
        private void SetGeneral2_2(ExcelRange range, int fontSize)
        {
            SetGeneral1(range, fontSize, Color.Blue, Color.FromArgb(192, 192, 192));
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
                    if(package != null)
                    {
                        package.Dispose();
                        package = null;
                    }                   
                    fileName = null;
                }
            }
        }
    }
}