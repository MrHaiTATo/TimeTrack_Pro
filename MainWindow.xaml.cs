﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Quartz;
using TimeTrack_Pro.Code;
using TimeTrack_Pro.Demo;

namespace TimeTrack_Pro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BakDatasHandle center;
        private OriginalDataHandle originalDataHandle;

        public WindowState State { get => WindowState.Minimized; }
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            web.Navigate(new Uri("http://192.168.1.3"));
        }

        private async void btn_demo_Click(object sender, RoutedEventArgs e)
        {
            await SmartDemo.Demo.demo1();
        }

        private void btn_Excel_Click(object sender, RoutedEventArgs e)
        {
            EppDemo.demo1();
        }

        private void btn_attendanceSheetBeta_Click(object sender, RoutedEventArgs e)
        {
            SheetTemplate.CreateAttendanceStatisticsSheet(center.GetStatisticsDatas(2024, 8));
            //SheetTemplate.CreateAttendanceStatisticsSheet(center);
        }

        private void btn_exceptionBeta_Click(object sender, RoutedEventArgs e)
        {
            SheetTemplate.CreatAttendanceExceptionSheet(center);
        }

        private void btn_SummarySheet_Click(object sender, RoutedEventArgs e)
        {
            SheetTemplate.CreatAttendanceSummarySheet(center);
        }

        private void btn_OriginalSheet_Click(object sender, RoutedEventArgs e)
        {
            SheetTemplate.CreatOriginalAttendanceSheet(center);
        }

        private void btn_AttendanceSheetBeta_Click_1(object sender, RoutedEventArgs e)
        {
            SheetTemplate.CreatAttendanceSheet();
        }

        private void btn_DataReadBeta_Click(object sender, RoutedEventArgs e)
        {
            string attendancePath = @"F:\文档\BakRcdData.TXT";
            string employeePath = @"F:\文档\BakUseData.TXT";
            center = new BakDatasHandle(attendancePath, employeePath);
        }

        private void btn_ShiftReadBeta_Click(object sender, RoutedEventArgs e)
        {
            string path = @"F:\文档\考勤排班表.xls";
            Rules.GetRuleList(path);
            var rules = Rules.RuleList;
        }

        private void btn_OriginalReadBeta_Click(object sender, RoutedEventArgs e)
        {
            originalDataHandle = new OriginalDataHandle(@"F:\文档\考勤原始表.xlsx");                        
        }

        private void WindowMaximizeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void CopyCommand(object sender, ExecutedRoutedEventArgs e)
        {            
                   
        }
    }
}