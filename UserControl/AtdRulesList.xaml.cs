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

        public AtdRulesList()
        {
            InitializeComponent();
        }

        private void btnOrgList_Click(object sender, RoutedEventArgs e)
        {
            SelectFile(SFile.OL);
        }        

        private void btnSelectSL_Click(object sender, RoutedEventArgs e)
        {
            SelectFile(SFile.SL);
        }

        private void btnBuild_Click(object sender, RoutedEventArgs e)
        {
            string msg = "";
            if (string.IsNullOrEmpty(tbxOrgList.Text))
            {
                msg = "未选择原始表！";
                Growl.Warning(msg, "InfoMessage");
                App.Log.Warn(msg);
                tbxOrgList.Focus();
                return;
            }
            if (!cbxCustom.IsChecked.Value && string.IsNullOrEmpty(tbxShiftList.Text))
            {
                msg = "未选择排班表！";
                Growl.Warning(msg, "InfoMessage");
                App.Log.Warn(msg);
                tbxShiftList.Focus();
                return;
            }
            if (cbxCustom.IsChecked.Value && ccbxRules.SelectedItems.Count == 0)
            {
                msg = "未选择考勤规则！";
                Growl.Warning(msg, "InfoMessage");
                App.Log.Warn(msg);
                ccbxRules.Focus();
                return;
            }
            OpenFolderDialog openFolder = new OpenFolderDialog();
            openFolder.FolderOk += (s, ev) => {
                if(cbxCustom.IsChecked.Value)
                {

                }
                else
                {

                }
            };
            openFolder.ShowDialog();
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

        private void BuildList()
        {
            int year = dpDate.DisplayDate.Year;
            int month = dpDate.DisplayDate.Month;
            if (cbxTJL.IsChecked.HasValue)
            {

            }
            if (cbxHZL.IsChecked.HasValue)
            {

            }
            if (cbxYCL.IsChecked.HasValue)
            {

            }
        }
    }
}
