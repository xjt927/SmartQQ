using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using MahApps.Metro;

namespace SmartQQ.Client
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            Task.Factory.StartNew(StartLogin.DoWork);
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(Application_DispatcherUnhandledException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            //CultureInfo currentCultureInfo = CultureInfo.CurrentCulture;
           // Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("zh-cn");

            // get the current app style (theme and accent) from the application
            // you can then use the current theme and custom accent instead set a new theme
            Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Application.Current);

            // now set the Green accent and dark theme
            ThemeManager.ChangeAppStyle(Application.Current,
                                        ThemeManager.GetAccent("Orange"),
                                        ThemeManager.GetAppTheme("BaseLight")); // or appStyle.Item1

            base.OnStartup(e);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = e.ExceptionObject as Exception;
                string errorMsg = "非WPF窗体线程异常 : \n\n";
                MessageBox.Show(errorMsg + ex.Message + Environment.NewLine + ex.StackTrace);
            }
            catch
            {
                MessageBox.Show("不可恢复的WPF窗体线程异常，应用程序将退出！");
            }
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = e.Exception;
                string errorMsg = "WPF窗体线程异常 : \n\n";
                MessageBox.Show(errorMsg + ex.Message + Environment.NewLine + ex.StackTrace);
            }
            catch
            {
                MessageBox.Show("不可恢复的WPF窗体线程异常，应用程序将退出！");
            }
        }
    }
}
