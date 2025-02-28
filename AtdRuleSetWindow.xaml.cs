using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using TimeTrack_Pro.Model;

namespace TimeTrack_Pro
{
    /// <summary>
    /// AtdRuleSetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AtdRuleSetWindow : Window
    {
        public ObservableCollection<LunBanItem>? LunBanItems { get; set; } = null;
        public ObservableCollection<AttendanceRule>? AttendanceRules { get; set; } = null;

        public AtdRuleSetWindow()
        {
            __init();
            DataContext = this;
            InitializeComponent();            
        }

        private void __init()
        {
            LunBanItems = new ObservableCollection<LunBanItem>();
            AttendanceRules = new ObservableCollection<AttendanceRule>();
        }

        private void gdpb_edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void gdpb_delete_Click(object sender, RoutedEventArgs e)
        {
            if (Dgguding.SelectedItem == null)
                return;
            AttendanceRules?.Remove((AttendanceRule)Dgguding.SelectedItem);
        }

        private void lbAdd_Click(object sender, RoutedEventArgs e)
        {
            LunBanItems?.Add(new LunBanItem());
        }

        private void lb_delete_Click(object sender, RoutedEventArgs e)
        {
            if (Dglunban.SelectedItem == null)
                return;
            LunBanItems?.Remove((LunBanItem)Dglunban.SelectedItem);
        }

        private void arAdd_Click(object sender, RoutedEventArgs e)
        {
            AttendanceRule rule = new AttendanceRule();
            rule.Init();
            AttendanceRules?.Add(rule);
        }

        private void arImport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void arExport_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
