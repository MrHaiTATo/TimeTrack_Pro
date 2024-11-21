using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
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
        public ExcelHelper(string fileName)
        {
            this.fileName = fileName;
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

        public void CreateAtdStatiSheets(List<StatisticsData> statistics)
        {
            ISheet sheet = null;
            Creat_init();
            if (workbook == null)
                return;
            foreach (var data in statistics)
            {
                sheet = workbook.CreateSheet(data.Name + "_" + data.Id);
                CreatAtdStatiSheet(sheet, data);

            }
            Save();
        }

        private void CreatAtdStatiSheet(ISheet sheet, StatisticsData data)
        {
            try
            {

            }
            catch (Exception e)
            {
                throw;
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
