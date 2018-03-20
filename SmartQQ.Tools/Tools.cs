/******************************************************************************** 
** Copyright(c) 2016  All Rights Reserved. 
** auth： 薛江涛 
** mail： xjt927@126.com 
** date： 2016/1/15 23:35:21 
** desc： 尚未编写描述 
** Ver :  V1.0.0 
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using SmartQQ.Core.Client;

namespace SmartQQ.Utils
{
    public class Tools
    {
        public static SmartQQClient smartQqClient { get; set; }

        /// <summary>
        /// 日志委托
        /// </summary>
        public static Action<string> LogAction;

        /// <summary>
        /// 运行日期
        /// </summary>
        public static List<string> RunDate = new List<string>();

        /// <summary>
        /// 会议主题
        /// </summary>
        public static string MeetingTitle { get; set; }

        /// <summary>
        /// 房间名称
        /// </summary>
        public static List<string> RoomList = new List<string>();

        /// <summary>
        /// 用户名，验证码，cookei
        /// </summary>
        public static UserInfo LoginInfo = new UserInfo();

        /// <summary>
        /// 查找控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null) return null;
            T foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // 如果子控件不是需查找的控件类型 
                T childType = child as T;
                if (childType == null)
                {
                    // 在下一级控件中递归查找 
                    foundChild = FindChild<T>(child, childName);
                    // 找到控件就可以中断递归操作   
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // 如果控件名称符合参数条件 
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // 查找到了控件 
                    foundChild = (T)child;
                    break;
                }
            }
            return foundChild;
        }

        /// <summary> 
        /// 日志输出到本地
        /// </summary>
        /// <param name="log"></param>
        public static void Log2File(string log)
        {
            try
            {

                string path = Directory.GetCurrentDirectory() + "\\logfile.txt";
                using (var sWriter = new StreamWriter(path, true, Encoding.UTF8))
                {
                    sWriter.WriteLine(DateTime.Now + " " + log);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 设置配置文件参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetAppSettingValue(string key, string value)
        {
            XmlDocument doc = new XmlDocument();
            //获得配置文件的全路径
            string strFileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            System.IO.File.SetAttributes(strFileName, System.IO.FileAttributes.Normal);
            doc.Load(strFileName);
            //找出名称为“add”的所有元素
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                //获得将当前元素的key属性
                if (nodes[i].ParentNode.LocalName == "appSettings")
                {
                    XmlAttribute att = nodes[i].Attributes["key"];
                    //根据元素的第一个属性来判断当前的元素是不是目标元素
                    if (att.Value == key)
                    {
                        //对目标元素中的第二个属性赋值
                        att = nodes[i].Attributes["value"];
                        att.Value = value;
                        break;
                    }
                }
                else if (nodes[i].ParentNode.LocalName == "connectionStrings")
                {
                    XmlAttribute att = nodes[i].Attributes["name"];
                    //根据元素的第一个属性来判断当前的元素是不是目标元素
                    if (att.Value == key)
                    {
                        //对目标元素中的第二个属性赋值
                        att = nodes[i].Attributes["connectionString"];
                        att.Value = value;
                        break;
                    }
                }
            }
            //保存上面的修改
            doc.Save(strFileName);
        }
    }

    public class UserInfo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// cookei
        /// </summary>
        public string Cookie { get; set; }

    }

}
