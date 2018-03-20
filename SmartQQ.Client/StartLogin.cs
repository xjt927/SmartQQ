using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using SmartQQ.Client.Views;
using SmartQQ.Core.Client;
using SmartQQ.Utils;

namespace SmartQQ.Client
{
    class StartLogin
    {
        //System.Environment.CurrentDirectory+"\\dump.json";
        private static readonly string CookiePath = "dump.json";
        private static readonly SmartQQClient Client = new SmartQQClient { CacheTimeout = TimeSpan.FromDays(1) };

        public static void DoWork()
        { 
            //// 好友消息回调
            //Client.FriendMessageReceived +=
            //    (sender, message) =>
            //    {
            //        Tools.LogAction($"{message.Sender.Alias ?? message.Sender.Nickname}:{message.Content}");
            //    };
            //// 群消息回调
            //Client.GroupMessageReceived += (sender, message) =>
            //{
            //    Tools.LogAction(
            //        $"[{message.Group.Name}]{message.Sender.Alias ?? message.Sender.Nickname}:{message.Content}");
            //    if (message.Content.IsMatch(@"^\s*Knock knock\s*$"))
            //        message.Reply("Who's there?");
            //    else if (message.StrictlyMentionedMe)
            //        message.Reply("什么事？");
            //};
            //// 讨论组消息回调
            //Client.DiscussionMessageReceived +=
            //    (sender, message) =>
            //    {
            //        Tools.LogAction($"[{message.Discussion.Name}]{message.Sender.Nickname}:{message.Content}");
            //    };
            //// 消息回显
            //Client.MessageEcho += (sender, e) => { Tools.LogAction($"{e.Target.Name}>{e.Content}"); };
            if (File.Exists(CookiePath))
            {
                // 尝试使用cookie登录
                if (Client.Start(File.ReadAllText(CookiePath)) != SmartQQClient.LoginResult.Succeeded)
                    QrLogin();
            }
            else
            {
                QrLogin();
            }
            Tools.smartQqClient = Client;

            Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
             
            // 导出cookie
            try
            {
                File.WriteAllText(CookiePath, Client.DumpCookies());
            }
            catch
            {
                // Ignored
            }
            // 防止程序终止
            //while (Client.Status == SmartQQClient.ClientStatus.Active)
            //{
            //}
        }

        private static void QrLogin()
        {
            while (true)
                switch (Client.Start(path => Process.Start(path)))
                {
                    case SmartQQClient.LoginResult.Succeeded:
                        return;
                    case SmartQQClient.LoginResult.QrCodeExpired:
                        continue;
                    default:
                        Tools.LogAction("登录失败，需要重试吗？(y/n)");
                        var response = Console.ReadLine();
                        if (response.IsMatch(@"^\s*y(es)?\s*$", RegexOptions.IgnoreCase))
                            continue;
                        Environment.Exit(1);
                        return;
                }
        }
        private static void ThreadStartingPoint()
        { 
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Tools.LogAction($"欢迎，{Client.Nickname}!");
            System.Windows.Threading.Dispatcher.Run();
        }
    }
}
