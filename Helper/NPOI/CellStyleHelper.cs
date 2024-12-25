using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using System.Threading.Tasks;
using NPOI.XSSF.UserModel;
using SixLabors.ImageSharp;
using NPOI.HSSF.UserModel;

namespace TimeTrack_Pro.Helper.NPOI
{
    public class CellStyleHelper
    {
        /// <summary>
        /// 设置背景色
        /// </summary>
        /// <param name="style"></param>
        /// <param name="color"></param>
        public static void SetBGC(ref ICellStyle style, IndexedColors color)
        {
            style.FillForegroundColor = color.Index;
            style.FillPattern = FillPattern.SolidForeground;
        }

        /// <summary>
        /// 设置字体居中
        /// </summary>
        /// <param name="style"></param>
        public static void SetAlignmentCenter(ref ICellStyle style)
        {
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
        }

        /// <summary>
        /// 设置边框颜色
        /// </summary>
        /// <param name="cellStyle"></param>
        /// <param name="color"></param>
        /// <param name="bottom"></param>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static void SetBorderColor(ref ICellStyle cellStyle, short color, bool bottom, bool top, bool left, bool right)
        {
            //XSSFColor xSSFColor = new XSSFColor(color);
            //XSSFCellStyle xSSFCellStyle = (XSSFCellStyle)cellStyle;
            if (bottom)
            {
                cellStyle.BorderBottom = BorderStyle.Thin;
                cellStyle.BottomBorderColor = color;
            }
            else
            {
                cellStyle.BorderBottom = BorderStyle.None;
            }
            if (left)
            {
                cellStyle.BorderLeft = BorderStyle.Thin;
                cellStyle.LeftBorderColor = color;
            }
            else
            {
                cellStyle.BorderLeft = BorderStyle.None;
            }
            if (right)
            {
                cellStyle.BorderRight = BorderStyle.Thin;
                cellStyle.RightBorderColor = color;
            }
            else
            {
                cellStyle.BorderRight = BorderStyle.None;
            }
            if (top)
            {
                cellStyle.BorderTop = BorderStyle.Thin;
                cellStyle.TopBorderColor = color;
            }
            else
            {
                cellStyle.BorderTop = BorderStyle.None;
            }
        }

        /// <summary>
        /// 默认背景色，边框为蓝色，字体居中
        /// </summary>
        /// <returns></returns>
        public static ICellStyle Style0(IWorkbook workbook, IFont font, bool bottom, bool top, bool left, bool right)
        {
            ICellStyle cellStyle = workbook.CreateCellStyle();
            SetBorderColor(ref cellStyle, IndexedColors.Blue.Index, bottom, top, left, right);
            if(font != null)
                cellStyle.SetFont(font);
            SetAlignmentCenter(ref cellStyle);
            cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat("@");
            return cellStyle;
        }

        /// <summary>
        /// 默认背景色，边框为蓝色，字体居中
        /// </summary>
        /// <returns></returns>
        public static ICellStyle Style0(IWorkbook workbook, IFont font, bool border)
        {
            return Style0(workbook, font, border, border, border, border);
        }

        /// <summary>
        /// 背景浅蓝，边框蓝色，字体居中
        /// </summary>
        /// <returns></returns>
        public static ICellStyle Style1(IWorkbook workbook, IFont font, bool bottom, bool top, bool left, bool right)
        {
            ICellStyle cellStyle = workbook.CreateCellStyle();
            if (font != null)
                cellStyle.SetFont(font);
            SetBGC(ref cellStyle, IndexedColors.PaleBlue);            
            SetAlignmentCenter(ref cellStyle);
            SetBorderColor(ref cellStyle, IndexedColors.Blue.Index, bottom, top, left, right);
            cellStyle.WrapText = true;
            cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat("@");
            return cellStyle;
        }

        /// <summary>
        /// 背景浅蓝，边框蓝色，字体居中
        /// </summary>
        /// <returns></returns>
        public static ICellStyle Style1(IWorkbook workbook, IFont font, bool border)
        {
            return Style1(workbook, font, border, border, border, border);
        }

        /// <summary>
        /// 背景浅绿，边框蓝色，字体居中
        /// </summary>
        /// <returns></returns>
        public static ICellStyle Style2(IWorkbook workbook, IFont font, bool bottom, bool top, bool left, bool right)
        {
            ICellStyle cellStyle = workbook.CreateCellStyle();
            if (font != null)
                cellStyle.SetFont(font);
            SetBGC(ref cellStyle, IndexedColors.LightGreen);
            SetAlignmentCenter(ref cellStyle);
            SetBorderColor(ref cellStyle, IndexedColors.Blue.Index, bottom, top, left, right);
            cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat("@");
            return cellStyle;
        }

        /// <summary>
        /// 背景浅绿，边框蓝色，字体居中
        /// </summary>
        /// <returns></returns>
        public static ICellStyle Style2(IWorkbook workbook, IFont font, bool border)
        {
            return Style2(workbook, font, border, border, border, border);
        }

        
    }
}
