using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Threading.Tasks;
using NPOI.XSSF.UserModel;
using SixLabors.ImageSharp;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using static NPOI.HSSF.Util.HSSFColor;

namespace TimeTrack_Pro.Helper.NPOI
{
    public class CellStyleHelper
    {
        /// <summary>
        /// 设置背景色
        /// </summary>
        /// <param name="style"></param>
        /// <param name="color"></param>
        public static void SetBGC(ref ICellStyle style, short color)
        {
            style.FillForegroundColor = color;
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

        public static ICellStyle Style(IWorkbook workbook, IFont font, short bgColor, short bdColor, bool bottom, bool top, bool left, bool right)
        {            
            ICellStyle cellStyle = workbook.CreateCellStyle();
            if (font != null)
                cellStyle.SetFont(font);            
            SetBGC(ref cellStyle, bgColor);
            SetAlignmentCenter(ref cellStyle);
            SetBorderColor(ref cellStyle, bdColor, bottom, top, left, right);
            cellStyle.WrapText = true;
            cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat("@");
            return cellStyle;
        }

        /// <summary>
        /// 自定义边框颜色和背景颜色
        /// </summary>      
        /// <returns></returns>
        public static ICellStyle CustomStyle(IWorkbook workbook, IFont font, byte[] bgColor, byte[] bdColor, bool bottom, bool top, bool left, bool right)
        {
            ICellStyle cellStyle = workbook.CreateCellStyle();
            IColor custom_bgColor;
            IColor custom_bdColor;
            if (workbook is XSSFWorkbook)
            {           
                XSSFCellStyle xSSFCellStyle = cellStyle as XSSFCellStyle;           
                custom_bgColor = new XSSFColor(bgColor);
                custom_bdColor = new XSSFColor(bdColor);
                xSSFCellStyle.SetFillForegroundColor((XSSFColor)custom_bgColor);
                xSSFCellStyle.FillPattern = FillPattern.SolidForeground;
                if (bottom)
                {
                    xSSFCellStyle.BorderBottom = BorderStyle.Thin;
                    xSSFCellStyle.SetBottomBorderColor((XSSFColor)custom_bdColor);
                }
                else
                {
                    xSSFCellStyle.BorderBottom = BorderStyle.None;
                }
                if (left)
                {
                    xSSFCellStyle.BorderLeft = BorderStyle.Thin;
                    xSSFCellStyle.SetLeftBorderColor((XSSFColor)custom_bdColor);
                }
                else
                {
                    xSSFCellStyle.BorderLeft = BorderStyle.None;
                }
                if (right)
                {
                    xSSFCellStyle.BorderRight = BorderStyle.Thin;
                    xSSFCellStyle.SetRightBorderColor((XSSFColor)custom_bdColor);
                }
                else
                {
                    xSSFCellStyle.BorderRight = BorderStyle.None;
                }
                if (top)
                {
                    xSSFCellStyle.BorderTop = BorderStyle.Thin;
                    xSSFCellStyle.SetTopBorderColor((XSSFColor)custom_bdColor);
                }
                else
                {
                    xSSFCellStyle.BorderTop = BorderStyle.None;
                }
            }
            else
            {
                HSSFWorkbook hssfWorkbook = workbook as HSSFWorkbook;
                custom_bgColor = hssfWorkbook.GetCustomPalette().FindColor(bgColor[0], bgColor[1], bgColor[2]);
                if (custom_bgColor == null)
                {
                    custom_bgColor = hssfWorkbook.GetCustomPalette().AddColor(bgColor[0], bgColor[1], bgColor[2]);
                }
                SetBGC(ref cellStyle, custom_bgColor.Indexed);
                custom_bdColor = hssfWorkbook.GetCustomPalette().FindColor(bdColor[0], bdColor[1], bdColor[2]);
                if (custom_bdColor == null)
                {
                    custom_bdColor = hssfWorkbook.GetCustomPalette().AddColor(bgColor[0], bgColor[1], bgColor[2]);
                }
                SetBorderColor(ref cellStyle, custom_bdColor.Indexed, bottom, top, left, right);
            }
            if (font != null)
                cellStyle.SetFont(font);            
            SetAlignmentCenter(ref cellStyle);
            cellStyle.WrapText = true;
            cellStyle.DataFormat = workbook.CreateDataFormat().GetFormat("@");
            return cellStyle;
        }                

        /// <summary>
        /// 默认背景色，字体居中
        /// </summary>        
        /// <returns></returns>
        public static ICellStyle Style0(IWorkbook workbook, IFont font, short bdColor, bool bottom, bool top, bool left, bool right)
        {
            return Style(workbook, font, IndexedColors.Automatic.Index, bdColor, bottom, top, left, right);
        }

        /// <summary>
        /// 默认背景色，边框为蓝色，字体居中
        /// </summary>
        /// <returns></returns>
        public static ICellStyle Style0(IWorkbook workbook, IFont font, bool bottom, bool top, bool left, bool right)
        {            
            return Style0(workbook, font, IndexedColors.Blue.Index, bottom, top, left, right);
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
        /// 背景浅蓝，字体居中
        /// </summary>
        /// <returns></returns>
        public static ICellStyle Style1(IWorkbook workbook, IFont font, short bdColor, bool bottom, bool top, bool left, bool right)
        {
            return Style(workbook, font, IndexedColors.PaleBlue.Index, bdColor, bottom, top, left, right);
        }

        /// <summary>
        /// 背景浅蓝，边框蓝色，字体居中
        /// </summary>
        /// <returns></returns>
        public static ICellStyle Style1(IWorkbook workbook, IFont font, bool bottom, bool top, bool left, bool right)
        {
            return Style1(workbook, font, IndexedColors.Blue.Index, bottom, top, left, right);
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
        /// 背景浅绿，字体居中
        /// </summary>
        /// <returns></returns>
        public static ICellStyle Style2(IWorkbook workbook, IFont font, short bdColor, bool bottom, bool top, bool left, bool right)
        {
            return Style(workbook, font, IndexedColors.LightGreen.Index, bdColor, bottom, top, left, right);
        }

        /// <summary>
        /// 背景浅绿，边框蓝色，字体居中
        /// </summary>
        /// <returns></returns>
        public static ICellStyle Style2(IWorkbook workbook, IFont font, bool bottom, bool top, bool left, bool right)
        {
            return Style2(workbook, font, IndexedColors.Blue.Index, bottom, top, left, right);
        }

        /// <summary>
        /// 背景浅绿，边框蓝色，字体居中
        /// </summary>
        /// <returns></returns>
        public static ICellStyle Style2(IWorkbook workbook, IFont font, bool border)
        {
            return Style2(workbook, font, border, border, border, border);
        }

        /// <summary>
        /// 背景蓝紫，字体居中
        /// </summary>       
        /// <returns></returns>
        public static ICellStyle Style3(IWorkbook workbook, IFont font, short bdColor, bool bottom, bool top, bool left, bool right)
        {            
            return Style(workbook, font, IndexedColors.LightCornflowerBlue.Index, bdColor, bottom, top, left, right);
        }

        /// <summary>
        /// 背景蓝紫，边框绿色，字体居中
        /// </summary>       
        /// <returns></returns>
        public static ICellStyle Style3(IWorkbook workbook, IFont font, bool bottom, bool top, bool left, bool right)
        {
            return Style3(workbook, font, IndexedColors.Green.Index, bottom, top, left, right);
        }

        /// <summary>
        /// 背景蓝紫，边框绿色，字体居中
        /// </summary>        
        /// <returns></returns>
        public static ICellStyle Style3(IWorkbook workbook, IFont font, bool border)
        {
            return Style3(workbook, font, border, border, border, border);
        }

        /// <summary>
        /// 背景天蓝，字体居中
        /// </summary>       
        /// <returns></returns>
        public static ICellStyle Style4(IWorkbook workbook, IFont font, short bdColor, bool bottom, bool top, bool left, bool right)
        {
            return Style(workbook, font, IndexedColors.SkyBlue.Index, bdColor, bottom, top, left, right);
        }

        /// <summary>
        /// 背景天蓝，边框绿色，字体居中
        /// </summary>       
        /// <returns></returns>
        public static ICellStyle Style4(IWorkbook workbook, IFont font, bool bottom, bool top, bool left, bool right)
        {
            return Style4(workbook, font, IndexedColors.Green.Index, bottom, top, left, right);
        }

        /// <summary>
        /// 背景天蓝，边框绿色，字体居中
        /// </summary>        
        /// <returns></returns>
        public static ICellStyle Style4(IWorkbook workbook, IFont font, bool border)
        {
            return Style4(workbook, font, border, border, border, border);
        }
    }
}
