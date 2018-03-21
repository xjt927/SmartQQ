# SmartQQ
  使用C#，WPF开发的SmartQQ桌面操作程序。使用Quartz.Net实现定时发送，定时功能不太完善，可以再扩展。

1. 运行项目会请求SmartQQ获取登录二维码，用户用手机qq扫描下实现登录。  
**登录成功后会记录cookie，下次登录先使用cookie，如果cookie失效，再弹出登录二维码。**

2. 登录成功后获取好友、群分组、讨论组。

3. 使用：双击qq好友头像、群分组、讨论组，添加到发送列表。

4. 设置定时发送。

# 扫码登录
<div>
<img src="https://github.com/xjt927/SmartQQ/blob/master/%E8%AF%B4%E6%98%8E%E5%9B%BE%E7%89%87/TIM%E6%88%AA%E5%9B%BE20180320174033.jpg" height="300">
<img src="https://github.com/xjt927/SmartQQ/blob/master/%E8%AF%B4%E6%98%8E%E5%9B%BE%E7%89%87/%E5%BE%AE%E4%BF%A1%E5%9B%BE%E7%89%87_20180320174303.jpg" height="400">
</div>  

#  登录成功
<img src="https://github.com/xjt927/SmartQQ/blob/master/%E8%AF%B4%E6%98%8E%E5%9B%BE%E7%89%87/%E5%BE%AE%E4%BF%A1%E5%9B%BE%E7%89%87_20180320175816.png" height="400"> 

#  感谢
https://github.com/TJYSunset/DumbQQ  
https://github.com/ScienJus/smartqq  
https://github.com/MahApps/MahApps.Metro  
 **以及感谢所有开源者的无私分享。**

# 注意
- 该项目是兴趣使然开发的，其中参考了其他一些开源项目，在此谢他们。  
- 我把好友qq、群消息等接收信息功能注释掉了，如果有需要你可以自己加上。
- 开发完成后没有经过严格测试，只是简单实现发送功能，如有问题请联系我，会尽量修改。
