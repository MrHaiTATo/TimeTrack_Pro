using HandyControl.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeTrack_Pro.Code;
using TimeTrack_Pro.Helper.EPPlus;

namespace TimeTrack_Pro.UserControl
{
    /// <summary>
    /// AtdRulesList.xaml 的交互逻辑
    /// </summary>
    public partial class AtdRulesList
    {
        enum SFile
        {
            OL,            
            SL
        }
        private OriginalDataHandle OriginalData;
        private ExcelHelper sheet;
        public AtdRulesList()
        {
            InitializeComponent();
            sheet = new ExcelHelper();
        }

        private void btnOrgList_Click(object sender, RoutedEventArgs e)
        {
            SelectFile(SFile.OL);
        }        

        private void btnSelectSL_Click(object sender, RoutedEventArgs e)
        {
            SelectFile(SFile.SL);
        }

        private async void btnBuild_Click(object sender, RoutedEventArgs e)
        {
            string msg = "";
            btnBuild.IsEnabled = false;
            if (string.IsNullOrEmpty(tbxOrgList.Text))
            {
                msg = "未选择原始表！";
                Growl.Warning(msg, "InfoMessage");
                App.Log.Warn(msg);
                tbxOrgList.Focus();
                goto inputErrorHandle;
            }
            if (!cbxCustom.IsChecked.Value && string.IsNullOrEmpty(tbxShiftList.Text))
            {
                msg = "未选择排班表！";
                Growl.Warning(msg, "InfoMessage");
                App.Log.Warn(msg);
                tbxShiftList.Focus();
                goto inputErrorHandle;
            }
            if (cbxCustom.IsChecked.Value && ccbxRules.SelectedItems.Count == 0)
            {
                msg = "未选择考勤规则！";
                Growl.Warning(msg, "InfoMessage");
                App.Log.Warn(msg);
                ccbxRules.Focus();
                goto inputErrorHandle;
            }
            OpenFolderDialog openFolder = new OpenFolderDialog();            
            if(openFolder.ShowDialog().Value)
            {
                try
                {
                    OriginalData = new OriginalDataHandle(tbxOrgList.Text);
                    if (cbxCustom.IsChecked.Value)
                    {
                        Rules.RuleList.Clear();
                    }
                    else
                    {
                        Rules.GetRuleList(tbxShiftList.Text);
                    }
                    await BuildList(openFolder.FolderName);
                    Growl.Info("生成完成！", "InfoMessage");
                }
                catch (Exception ex)
                {
                    Growl.Error(ex.Message, "ErrorMessage");
                    App.Log.Error(ex.Message + $" 异常发生位置：{ex.StackTrace}");
                }
            }

inputErrorHandle:
            btnBuild.IsChecked = false;
            btnBuild.IsEnabled = true;
        }

        private void SelectFile(SFile file)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Excel File|*.xls;*.xlsx";
            if(file == SFile.OL)
            {
                openFile.FileOk += (s, ev) => tbxOrgList.Text = openFile.FileName;
            }
            else
            {
                openFile.FileOk += (s, ev) => tbxShiftList.Text = openFile.FileName;
            }
            openFile.ShowDialog();
        }

        private async Task BuildList(string savePath)
        {
            int year = dpDate.DisplayDate.Year;
            int month = dpDate.DisplayDate.Month;
            if (cbxTJL.IsChecked.HasValue)
            {
                sheet.FilePath = savePath + "\\考勤统计表.xlsx";
                await Task.Run(() => sheet.CreateAtdStatiSheet(OriginalData.GetStatisticsSheetModel()));
            }
            if (cbxHZL.IsChecked.HasValue)
            {
                sheet.FilePath = savePath + "\\考勤汇总表.xlsx";
                await Task.Run(() => sheet.CreatAtdSumSheet(OriginalData.GetSummarySheetModel()));
            }
            if (cbxYCL.IsChecked.HasValue)
            {
                sheet.FilePath = savePath + "\\考勤异常表.xlsx";
                await Task.Run(() => sheet.CreatAtdExpSheet(OriginalData.GetExceptionSheetModel()));
            }
        }
    }
}
