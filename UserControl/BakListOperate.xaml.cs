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
    /// BakListOperate.xaml 的交互逻辑
    /// </summary>
    public partial class BakListOperate
    {
        public BakListOperate()
        {
            InitializeComponent();
        }
        
        private void btnSelectUL_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "TXT|*.txt";
            if(openFile.ShowDialog().HasValue)
            {
                tbxUserList.Text = openFile.FileName;
            }
        }

        private void btnSelectAL_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "TXT|*.txt";
            if (openFile.ShowDialog().HasValue)
            {
                tbxAtdList.Text = openFile.FileName;
            }
        }
    }
}
