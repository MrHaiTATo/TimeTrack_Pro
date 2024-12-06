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
using TimeTrack_Pro.Code;
using TimeTrack_Pro.Demo;
using TimeTrack_Pro.Helper.EPPlus;
using TimeTrack_Pro.Model;
using TimeTrack_Pro.ViewModel;

namespace TimeTrack_Pro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BakDatasHandle center;
        private OriginalDataHandle originalDataHandle;
        private ExcelHelper sheet;
        private MainWindowViewModel VM { get; set; }

        public WindowState State { get => WindowState.Minimized; }
        public MainWindow()
        {            
            InitializeComponent();
            One_Load();
            this.DataContext = this;
            VM = (MainWindowViewModel)Resources["MainModel"];
            web.Navigate(new Uri("http://192.168.1.3"));
            sheet = new ExcelHelper();
        }

        /// <summary>
        /// 只需要在程序中设置一次的参数或运行一次的函数
        /// </summary>
        private void One_Load()
        {
            ExcelHelper.EPPlus_Init();
        }

        private async void btn_demo_Click(object sender, RoutedEventArgs e)
        {
            await SmartDemo.Demo.demo1();
        }

        private void btn_Excel_Click(object sender, RoutedEventArgs e)
        {
            EppDemo.demo1();
        }

        private async void btn_attendanceSheetBeta_Click(object sender, RoutedEventArgs e)
        {
            string fileName = @"F:\文档\考勤统计表.xlsx";
            sheet.FilePath = fileName;          
            double Msec = 0;
            System.Timers.Timer timer = new System.Timers.Timer(10); // 创建一个每秒触发一次的定时器
            timer.Elapsed += (s, e) => Msec += 10;// 注册事件处理方法                             
            timer.AutoReset = true; // 默认为 true，表示一次性触发后自动重置，继续计时
            timer.Enabled = true; // 启动定时器
            Task task = sheet.CreateAtdStatiSheet(center.GetStatisticsSheetModel(2024, 8));            
            await task;
            timer.Enabled = false;
            timer.Dispose();            
            MessageBox.Show(this,string.Format("用时：{0:0.000} 秒",Msec / 1000));
        }


        private void btn_exceptionBeta_Click(object sender, RoutedEventArgs e)
        {
            string fileName = @"F:\文档\考勤异常表.xlsx";
            sheet.FilePath = fileName;
            sheet.CreatAtdExpSheet(center.GetExceptionSheetModel(2024, 8));
            
        }

        private void btn_SummarySheet_Click(object sender, RoutedEventArgs e)
        {
            string fileName = @"F:\文档\考勤汇总表.xlsx";
            sheet.FilePath = fileName;
            sheet.CreatAtdSumSheet(center.GetSummarySheetModel(2024, 8));            
        }

        private void btn_OriginalSheet_Click(object sender, RoutedEventArgs e)
        {
            string fileName = @"F:\文档\考勤原始表.xlsx";
            sheet.FilePath = fileName;
            sheet.CreatAtdOrgSheet(center.GetOriginalSheetModel(2024, 8));            
        }

        private void btn_AttendanceSheetBeta_Click_1(object sender, RoutedEventArgs e)
        {
            string fileName = @"E:\mahaitao\GitHub\TestData\考勤排班表.xlsx";
            sheet.FilePath = fileName;
            sheet.CreatAtdSchedulingSheet(Rules.GetRuleModel());            
        }

        private void btn_DataReadBeta_Click(object sender, RoutedEventArgs e)
        {
            DateTime date1 = new DateTime(2024,8,1,2,0,0);
            DateTime date2 = new DateTime(2024, 8, 2, 2, 0, 0);
            TimeSpan span = date2 - date1;
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

        private async void btn_OriginalReadBeta_Click(object sender, RoutedEventArgs e)
        {
            string resourceFile = @"F:\文档\考勤原始表.xlsx";
            string savePath = @"F:\文档\原始表\考勤统计表.xlsx";
            originalDataHandle = new OriginalDataHandle(resourceFile);
            var statistics = originalDataHandle.GetStatisticsSheetModel();
            sheet.FilePath = savePath;
            await sheet.CreateAtdStatiSheet(statistics);
            
            var summarys = originalDataHandle.GetSummarySheetModel();
            sheet.FilePath = @"F:\文档\原始表\考勤汇总表.xlsx";
            sheet.CreatAtdSumSheet(summarys);
            var exceptions = originalDataHandle.GetExceptionSheetModel();
            sheet.FilePath = @"F:\文档\原始表\考勤异常表.xlsx";
            sheet.CreatAtdExpSheet(exceptions);
        }

        private void WindowMaximizeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void CopyCommand(object sender, ExecutedRoutedEventArgs e)
        {            
                   
        }

        private void btn_Npio_Click(object sender, RoutedEventArgs e)
        {
            NpioDemo npioDemo = new NpioDemo();
            npioDemo.GenerateExcelWithComplexStyles();
        }

        private void btn_message_Click(object sender, RoutedEventArgs e)
        {
            HandyControl.Controls.Growl.Info("Info Message", "InfoMessage");
        }

        private void StackPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContextMenu menu = (sender as StackPanel).ContextMenu;
            MenuItem item = menu.Items[0] as MenuItem;
            item.Header = Resources["Clear"];
        }

        private void FuntionOptionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VM.SelectedIndex < 0)
            {
                return;
            };
            mainContent.Children.Clear();
            string name = ((sender as ListBox).SelectedItem as LBDataModel).Name;
            int selectIndex = VM.SelectedIndex;
        }
    }
}