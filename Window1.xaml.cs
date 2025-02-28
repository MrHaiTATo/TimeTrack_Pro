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
using System.Windows.Shapes;

namespace TimeTrack_Pro
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            LoadEmployees();
            LoadShifts();
        }

        private void LoadEmployees()
        {
            // 模拟加载员工数据
            var employees = new List<eEmployee>
            {
                new eEmployee { Name = "张三" },
                new eEmployee { Name = "李四" },
                new eEmployee { Name = "王五" }
            };
            EmployeeList.ItemsSource = employees;
        }

        private void LoadShifts()
        {
            // 模拟加载排班数据
            var shifts = new List<eShift>
            {
                new eShift { Date = "2023-10-01", ShiftName = "早班", Employee = "张三" },
                new eShift { Date = "2023-10-02", ShiftName = "中班", Employee = "李四" },
                new eShift { Date = "2023-10-03", ShiftName = "晚班", Employee = "王五" }
            };
            ShiftGrid.ItemsSource = shifts;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // 保存排班表逻辑
            MessageBox.Show("排班表已保存！");
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            // 加载排班表逻辑
            MessageBox.Show("排班表已加载！");
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            // 导出排班表逻辑
            MessageBox.Show("排班表已导出！");
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            // 添加员工逻辑
            MessageBox.Show("员工已添加！");
        }

        private void ApplyRule_Click(object sender, RoutedEventArgs e)
        {
            // 应用排班规则逻辑
            MessageBox.Show("排班规则已应用！");
        }
    }

    public class eEmployee
    {
        public string Name { get; set; }
    }

    public class eShift
    {
        public string Date { get; set; }
        public string ShiftName { get; set; }
        public string Employee { get; set; }
    }
}

