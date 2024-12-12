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
using TimeTrack_Pro.Model;

namespace TimeTrack_Pro.UserControl
{
    /// <summary>
    /// RuleManage.xaml 的交互逻辑
    /// </summary>
    public partial class RuleManage
    {
        public RuleManage()
        {
            InitializeComponent();
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AttendanceRule rule = e.Parameter as AttendanceRule;

        }
    }
}
