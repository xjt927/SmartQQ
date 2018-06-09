using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SmartQQ.Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void ClickGo(object sender, RoutedEventArgs e)
        {

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory+"/Resource/ptqrshow.png";
            sqImage.Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
        }
    }
}
