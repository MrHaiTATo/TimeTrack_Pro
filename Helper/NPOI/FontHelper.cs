using NPOI.SS.UserModel;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Helper.NPOI
{
    public class FontHelper
    {
        public static IFont STBlue(IWorkbook worksheet, int size)
        {
            IFont font = worksheet.CreateFont();
            font.FontName = "宋体";
            font.FontHeightInPoints = size;
            font.Color = IndexedColors.Blue.Index;
            return font;
        }

        public static IFont STBlueBlod(IWorkbook worksheet, int size)
        {
            IFont font = worksheet.CreateFont();
            font.FontName = "宋体";
            font.IsBold = true;
            font.FontHeightInPoints = size;
            font.Color = IndexedColors.Blue.Index;
            return font;
        }

        public static IFont STRed(IWorkbook worksheet, int size)
        {
            IFont font = worksheet.CreateFont();
            font.FontName = "宋体";
            font.FontHeightInPoints = size;
            font.Color = IndexedColors.Red.Index;
            return font;
        }
    }
}
