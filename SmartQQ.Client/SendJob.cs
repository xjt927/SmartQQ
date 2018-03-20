using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Quartz;
using SmartQQ.Client.Views;
using SmartQQ.Core.Models;
using SmartQQ.Utils;
using XJT.Com.Core.Quartz;

namespace SmartQQ.Client
{
    class SendJob : IJob, IInterruptableJob
    {
        private bool interrupted;
        public void Execute(IJobExecutionContext context)
        {
            if (!interrupted)
            {
                string content = QuartzService.Instance.GetJobDataMap(context, "sendContent").ToString();
                List<Friend> friendInfoList = (List<Friend>)QuartzService.Instance.GetJobDataMap(context, "FriendInfoList");
                List<Group> groupInfoList = (List<Group>)QuartzService.Instance.GetJobDataMap(context, "GroupInfoList");
                List<Discussion> discussionInfoList = (List<Discussion>)QuartzService.Instance.GetJobDataMap(context, "DiscussionInfoList");

                friendInfoList.ForEach(x => x.Message(content));
                groupInfoList.ForEach(x => x.Message(content));
                discussionInfoList.ForEach(x => x.Message(content));
            }
            Tools.LogAction("发送完成！");
        }

        public void Interrupt()
        {
            Tools.LogAction("---  -- 中断代码执行 --");
            interrupted = true;
        }
    }
}
