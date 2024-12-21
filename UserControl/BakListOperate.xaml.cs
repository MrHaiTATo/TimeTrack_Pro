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
using HandyControl.Data;
using HandyControl.Controls;
using TimeTrack_Pro.Model;

namespace TimeTrack_Pro.UserControl
{
    /// <summary>
    /// BakListOperate.xaml 的交互逻辑
    /// </summary>
    public partial class BakListOperate
    {
        enum SFile
        {
            UL,
            AL,
            SL
        }

        private BakDatasHandle center;
        private ExcelHelper sheet;

        public BakListOperate()
        {
            InitializeComponent();
            sheet = new ExcelHelper();
            center = new BakDatasHandle();
        }
        
        private void btnSelectUL_Click(object sender, RoutedEventArgs e)
        {
            SelectFile(SFile.UL);
        }

        private void btnSelectAL_Click(object sender, RoutedEventArgs e)
        {
            SelectFile(SFile.AL);
        }

        private void btnSelectSL_Click(object sender, RoutedEventArgs e)
        {
            SelectFile(SFile.SL);
        }

        private async void btnBuild_Click(object sender, RoutedEventArgs e)
        {           
            string msg = "";
            btnBuild.IsEnabled = false;
            if (string.IsNullOrEmpty(tbxAtdList.Text))
            {
                msg = "未选择考勤备份表！";
                Growl.Warning(msg, "InfoMessage");
                App.Log.Warn(msg);
                tbxAtdList.Focus();
                goto inputErrorHandle;
            }
            if(string.IsNullOrEmpty(tbxUserList.Text))
            {
                msg = "未选择用户备份表！";
                Growl.Warning(msg, "InfoMessage");
                App.Log.Warn(msg);
                tbxUserList.Focus();
                goto inputErrorHandle;
            }
            if(!cbxCustom.IsChecked.Value && string.IsNullOrEmpty(tbxShiftList.Text))
            {
                msg = "未选择排班表！";
                Growl.Warning(msg, "InfoMessage");
                App.Log.Warn(msg);
                tbxShiftList.Focus();   
                
            }
            if(cbxCustom.IsChecked.Value && ccbxRules.SelectedItems.Count == 0)
            {
                msg = "未选择考勤规则！";
                Growl.Warning(msg, "InfoMessage");
                App.Log.Warn(msg);
                ccbxRules.Focus();
                goto inputErrorHandle;
            }
            if(string.IsNullOrEmpty(dpDate.Text))
            {
                msg = "未选择报表月份！";
                Growl.Warning(msg, "InfoMessage");
                App.Log.Warn(msg);
                dpDate.Focus();
                goto inputErrorHandle;
            }    
            if(!cbxTJL.IsChecked.Value && !cbxHZL.IsChecked.Value &&
               !cbxYCL.IsChecked.Value && !cbxYSL.IsChecked.Value)
            {
                msg = "未选择报表！";
                Growl.Warning(msg, "InfoMessage");
                App.Log.Warn(msg);
                goto inputErrorHandle;
            }
            OpenFolderDialog openFolder = new OpenFolderDialog();           
            if(openFolder.ShowDialog().Value)
            {
                try
                {
                    string atdFile = tbxAtdList.Text;
                    string usersFile = tbxUserList.Text;
                    await Task.Run(() => center.LoadFile(atdFile, usersFile));
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
            if(file == SFile.SL)
            {
                openFile.Filter = "Excel File|*.xls;*.xlsx";
                openFile.FileOk += (s, ev) => tbxShiftList.Text = openFile.FileName;
            }
            else
            {
                openFile.Filter = "TXT|*.txt";
                if(file == SFile.UL)
                {
                    openFile.FileOk += (s, ev) => tbxUserList.Text = openFile.FileName;
                }
                else
                {
                    openFile.FileOk += (s, ev) => tbxAtdList.Text = openFile.FileName;
                }
            }
            openFile.ShowDialog();
        }

        private async Task BuildList(string savePath)
        {            
            int year = dpDate.DisplayDate.Year;
            int month = dpDate.DisplayDate.Month;
            if(cbxTJL.IsChecked.Value)
            {
                sheet.FilePath = savePath + "\\考勤统计表.xlsx";
                await Task.Run(() => sheet.CreateAtdStatiSheet(center.GetStatisticsSheetModel(year, month)));               
            }
            if(cbxHZL.IsChecked.Value)
            {
                sheet.FilePath = savePath + "\\考勤汇总表.xlsx";
                await Task.Run(() => sheet.CreatAtdSumSheet(center.GetSummarySheetModel(year, month)));
            }
            if(cbxYCL.IsChecked.Value)
            {
                sheet.FilePath = savePath + "\\考勤异常表.xlsx";
                await Task.Run(() => sheet.CreatAtdExpSheet(center.GetExceptionSheetModel(year, month)));
            }
            if(cbxYSL.IsChecked.Value)
            {
                sheet.FilePath = savePath + "\\考勤原始表.xlsx";
                await Task.Run(() => sheet.CreatAtdOrgSheet(center.GetOriginalSheetModel(year, month)));
            }            
        }
    }
}
