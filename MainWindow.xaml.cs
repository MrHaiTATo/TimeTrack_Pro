using System.Text;
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
using TimeTrack_Pro.Model;

namespace TimeTrack_Pro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btn_demo_Click(object sender, RoutedEventArgs e)
        {
            await Model.Demo.demo1();
        }

        private void btn_Excel_Click(object sender, RoutedEventArgs e)
        {
            EppDemo.demo1();
        }

        private void btn_attendanceSheetBeta_Click(object sender, RoutedEventArgs e)
        {
            SheetTemplate.CreateAttendanceStatisticsSheet();
        }

        private void btn_exceptionBeta_Click(object sender, RoutedEventArgs e)
        {
            SheetTemplate.CreatAttendanceExceptionSheet(10);
        }

        private void btn_SummarySheet_Click(object sender, RoutedEventArgs e)
        {
            SheetTemplate.CreatAttendanceSummarySheet(10);
        }

        private void btn_OriginalSheet_Click(object sender, RoutedEventArgs e)
        {
            SheetTemplate.CreatOriginalAttendanceSheet(10);
        }

        private void btn_AttendanceSheetBeta_Click_1(object sender, RoutedEventArgs e)
        {
            SheetTemplate.CreatAttendanceSheet();
        }
    }
}