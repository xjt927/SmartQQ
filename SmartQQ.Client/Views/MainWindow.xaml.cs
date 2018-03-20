using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Quartz;
using SmartQQ.Client.Models;
using SmartQQ.Core.Models;
using SmartQQ.Utils;
using XJT.Com.Core.Quartz;

namespace SmartQQ.Client.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            Tools.LogAction = LogInfo;
            InitializeComponent();
        }

        public readonly List<Friend> FriendInfoList = new List<Friend>();
        public readonly List<Group> GroupInfoList = new List<Group>();
        public readonly List<Discussion> DiscussionInfoList = new List<Discussion>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Tools.LogAction("开始加载QQ好友");
            Task.Factory.StartNew(() =>
            {
                Tools.smartQqClient.Categories.ForEach(s =>
                {
                    this.FriendListControl.Dispatcher.Invoke((Action)(() =>
                    {
                        Expander t = new Expander
                        {
                            Header = s,
                            HeaderTemplate = this.FindResource("FriendListTemplate") as DataTemplate
                        };
                        ListView v = new ListView
                        {
                            ItemsSource = s.Members,
                            Width = 280,
                            BorderThickness = new Thickness(0),
                            ItemTemplate = this.FindResource("FriendList") as DataTemplate,
                            SelectionMode = SelectionMode.Single
                        };
                        t.Content = v;
                        FriendListControl.Children.Add(t);
                    }));
                });
            });

            Task.Factory.StartNew(() =>
            {
                Tools.smartQqClient.Groups.ForEach(s =>
                {
                    lock (GroupsListControl)
                    {
                        this.GroupsListControl.Dispatcher.Invoke((Action)(() =>
                       {
                           Expander t = new Expander
                           {
                               Header = s,
                               HeaderTemplate = this.FindResource("GroupListTemplate") as DataTemplate
                           };
                           ListView v = new ListView
                           {
                               Width = 280,
                               BorderThickness = new Thickness(0),
                               ItemTemplate = this.FindResource("GroupList") as DataTemplate,
                               SelectionMode = SelectionMode.Single
                           };
                           //v.ItemsSource = s.Members;
                           t.Content = v;
                           GroupsListControl.Children.Add(t);
                       }));
                    }
                });
            });

            Task.Factory.StartNew(() =>
            {
                Tools.smartQqClient.Discussions.ForEach(s =>
                {
                    lock (DiscussionsListControl)
                    {
                        this.DiscussionsListControl.Dispatcher.Invoke((Action)(() =>
                       {
                           Expander t = new Expander
                           {
                               Header = s,
                               HeaderTemplate = this.FindResource("DiscussionListTemplate") as DataTemplate
                           };
                           ListView v = new ListView
                           {
                               ItemsSource = s.Members,
                               Width = 280,
                               BorderThickness = new Thickness(0),
                               ItemTemplate = this.FindResource("DiscussionList") as DataTemplate,
                               SelectionMode = SelectionMode.Single
                           };
                           t.Content = v;
                           DiscussionsListControl.Children.Add(t);
                       }));
                    }
                });
            });
        }

        /// <summary>
        /// 日志输出
        /// </summary>
        private void LogInfo(string strLog)
        {
            try
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    //this.Visibility = Visibility.Visible;
                    string log = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " " + strLog + "\r\n";
                    LogBox.Text = log + LogBox.Text;
                }));

                Tools.Log2File(strLog);
                System.GC.Collect();
            }
            catch (Exception ex)
            {
                Tools.Log2File(ex.ToString());
                System.GC.Collect();
            }
        }

        #region 分组双击事件

        private void Friend_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            object objFriend = (((System.Windows.Input.MouseDevice)e.Device).Target);
            if (objFriend != null)
            {
                var border = objFriend as Border;
                object objFriend2 = border != null ? border.DataContext : ((TextBlock)objFriend).DataContext;

                Friend friendInfo = objFriend2 as Friend;
                if (FriendInfoList.Contains(friendInfo))
                {
                    return;
                }
                FriendInfoList.Add(friendInfo);
                string nickName = String.IsNullOrEmpty(friendInfo.Alias) ? friendInfo.Nickname : friendInfo.Alias;
                ItemContent.Text += nickName + " | ";
                Tools.LogAction(nickName);
            }
        }

        private void Group_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            object objGroup = (((System.Windows.Input.MouseDevice)e.Device).Target);
            if (objGroup != null)
            {
                object objGroup2 = ((System.Windows.Controls.ContentControl)objGroup).Content;
                Group groupInfo = objGroup2 as Group;
                if (GroupInfoList.Contains(groupInfo))
                {
                    return;
                }
                GroupInfoList.Add(groupInfo);
                ItemContent.Text += groupInfo.Name + " | ";
                Tools.LogAction(groupInfo.Name);
            }
        }

        private void Discussion_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            object objDiscussion = (((System.Windows.Input.MouseDevice)e.Device).Target);
            if (objDiscussion != null)
            {
                object objDiscussion2 = ((System.Windows.Controls.ContentControl)objDiscussion).Content;
                Discussion discussionInfo = objDiscussion2 as Discussion;
                if (DiscussionInfoList.Contains(discussionInfo))
                {
                    return;
                }
                DiscussionInfoList.Add(discussionInfo);
                ItemContent.Text += discussionInfo.Name + " | ";
                Tools.LogAction(discussionInfo.Name);
            }
        }

        #endregion

        #region 鼠标上下滚动事件

        private void Friend_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer viewer = FriendScrollViewer;  //scrollview 为Scrollview的名字，在Xaml文件中定义。
            if (viewer == null)
                return;
            double num = Math.Abs((int)(e.Delta / 2));
            double offset = 0.0;
            if (e.Delta > 0)
            {
                offset = Math.Max((double)0.0, (double)(viewer.VerticalOffset - num));
            }
            else
            {
                offset = Math.Min(viewer.ScrollableHeight, viewer.VerticalOffset + num);
            }
            if (offset != viewer.VerticalOffset)
            {
                viewer.ScrollToVerticalOffset(offset);
                e.Handled = true;
            }
        }

        private void Group_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer viewer = GroupScrollViewer;  //scrollview 为Scrollview的名字，在Xaml文件中定义。
            if (viewer == null)
                return;
            double num = Math.Abs((int)(e.Delta / 2));
            double offset = 0.0;
            if (e.Delta > 0)
            {
                offset = Math.Max((double)0.0, (double)(viewer.VerticalOffset - num));
            }
            else
            {
                offset = Math.Min(viewer.ScrollableHeight, viewer.VerticalOffset + num);
            }
            if (offset != viewer.VerticalOffset)
            {
                viewer.ScrollToVerticalOffset(offset);
                e.Handled = true;
            }
        }

        private void Discussion_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer viewer = DiscussionScrollViewer;  //scrollview 为Scrollview的名字，在Xaml文件中定义。
            if (viewer == null)
                return;
            double num = Math.Abs((int)(e.Delta / 2));
            double offset = 0.0;
            if (e.Delta > 0)
            {
                offset = Math.Max((double)0.0, (double)(viewer.VerticalOffset - num));
            }
            else
            {
                offset = Math.Min(viewer.ScrollableHeight, viewer.VerticalOffset + num);
            }
            if (offset != viewer.VerticalOffset)
            {
                viewer.ScrollToVerticalOffset(offset);
                e.Handled = true;
            }
        }

        #endregion

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            FriendInfoList.Clear();
            GroupInfoList.Clear();
            DiscussionInfoList.Clear();
            ItemContent.Clear();
        }

        /// <summary>
        /// 立即发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            String content = SendContent.Text.Trim();
            if (String.IsNullOrWhiteSpace(content))
            {
                Tools.LogAction("没有发送内容！");
                return;
            }
            if (string.IsNullOrWhiteSpace(ItemContent.Text))
            {
                Tools.LogAction("没有发送对象！");
                return;
            }
            FriendInfoList.ForEach(x => x.Message(content));
            GroupInfoList.ForEach(x => x.Message(content));
            DiscussionInfoList.ForEach(x => x.Message(content));
            Tools.LogAction("发送完成！");
        }

        /// <summary>
        /// 定时发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cron_Click(object sender, RoutedEventArgs e)
        {
            //if (string.IsNullOrWhiteSpace(ItemContent.Text))
            //{
            //    Tools.LogAction("没有发送对象！");
            //    return;
            //}
            double? interval = Interval.Value;
            double? repeatCount = RepeatCount.Value;
            String content = SendContent.Text.Trim();
            if (String.IsNullOrWhiteSpace(content))
            {
                Tools.LogAction("发送内容不能为空！");
                return;
            }
            if (interval == null)
            {
                Tools.LogAction("发送间隔不能为空！");
                return;
            }
            if (repeatCount == null)
            {
                Tools.LogAction("发送次数不能为空！");
                return;
            }

            var ts = new ToggleSwitch();
            ToggleSwitch @switch = sender as ToggleSwitch;
            if (@switch != null)
            {
                ts = @switch;
            }
            if (ts.IsChecked != null && (bool)ts.IsChecked)
            {
                //  string cronContent = QuartzContent.Text;
                IJobDetail job = QuartzService.Instance.CreateJob<SendJob>("sendJob", "sendJob");
                DateTimeOffset? starTimeOffset = null;
                if (DateTimePicker.SelectedTime != null)
                {
                    starTimeOffset = DateTimePicker.DisplayDate.ToDateTime();
                }
                QuartzService.Instance.PutJobDataMap(job, "sendContent", content);
                QuartzService.Instance.PutJobDataMap(job, "FriendInfoList", FriendInfoList);
                QuartzService.Instance.PutJobDataMap(job, "GroupInfoList", GroupInfoList);
                QuartzService.Instance.PutJobDataMap(job, "DiscussionInfoList", DiscussionInfoList);

                if (starTimeOffset != null)
                {
                    ITrigger trigger = QuartzService.Instance.CreateTriggerByStartAt((DateTimeOffset)starTimeOffset, "sendTrigger", "sendGroup",
                        new TimeSpan(0, 0, 0, 0, Convert.ToInt32(interval)), Convert.ToInt32(repeatCount) - 1);

                    QuartzService.Instance.ScheduleAddJob(job, trigger);
                }
                QuartzService.Instance.SchedulerStart();
                Tools.LogAction("已启动定时发送");
            }
            else
            {
                //取消定时发送
                QuartzService.Instance.SchedulerUnscheduleAllJobs();
                Tools.LogAction("已关闭定时发送");
            }
        }
    }
}
