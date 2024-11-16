using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace TimeTrack_Pro.UserControl
{
    /// <summary>
    /// AttendanceRuleSet.xaml 的交互逻辑
    /// </summary>
    public partial class AttendanceRuleSet
    {        
        public AttendanceRuleSet()
        {
            InitializeComponent();          
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox? text = sender as TextBox;
            if (string.IsNullOrEmpty(text.Text))
                return;
            if (!Regex.IsMatch(text.Text, @"^[0-9]+$"))
                return;
        }
    }
}
