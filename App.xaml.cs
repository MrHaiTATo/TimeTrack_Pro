using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using log4net;
using log4net.Config;

namespace TimeTrack_Pro
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(App));

        public static ILog Log { get => _log; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);           
            XmlConfigurator.Configure(new FileInfo("log4net.config")); // 从 log4net.config 读取配置
            _log.Info("程序开始");
            // 示例日志记录
            //_log.Debug("Debug message");
            //_log.Info("Info message");
            //_log.Warn("Warning message");
            //_log.Error("Error message");
            //_log.Fatal("Fatal message");

            //全局错误处理
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;//Thead，处理在非UI线程上未处理的异常,当前域未处理异常
            DispatcherUnhandledException += App_DispatcherUnhandledException;//处理在UI线程上未处理的异常
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;//处理在Task上未处理的异常
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _log.Info("程序结束");
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            // 处理未被观察的异常
            // 可以记录日志或执行其他操作
            _log.Error("TaskScheduler_UnobservedTaskException出现错误：" + Environment.NewLine + e.Exception.ToString());
            MessageBox.Show("TaskScheduler_UnobservedTaskException出现错误：" + Environment.NewLine + e.Exception.ToString());

            // 标记异常已处理，防止应用程序崩溃
            e.SetObserved();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // 处理未经处理的异常
            // 请注意，这里的异常是无法恢复的，应用程序可能会退出
            _log.Error("CurrentDomain_UnhandledException出现错误：" + Environment.NewLine + e.ExceptionObject.ToString());
            MessageBox.Show("CurrentDomain_UnhandledException出现错误：" + Environment.NewLine + e.ExceptionObject.ToString());
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // 处理未经处理的异常
            // 请注意，这里的异常是无法恢复的，应用程序可能会退出
            // 显示错误信息
            _log.Error("App_DispatcherUnhandledException出现错误：" + Environment.NewLine + e.Exception.ToString());
            MessageBox.Show("App_DispatcherUnhandledException出现错误：" + Environment.NewLine + e.Exception.ToString());

            // 终止事件传播,防止应用程序崩溃
            e.Handled = true;
        }     
    }
}
