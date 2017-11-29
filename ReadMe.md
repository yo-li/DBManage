###  数据库管理工具

---
*ps:该项目是Visual Studio 2015开发，数据库Microsoft SQL Server 2012(支持2008R)*

###### 如何使用？  

1.下载DBManage  

2.打开项目(Visual Studio 2015),修改配置文件Web.config的SqlConnectionString数据链接   

3.重新生成项目(生成项目时需要一些时间，需要重新还原程序集)    

4.将Sql.txt文件中的视图(<font color=#CD853F >[dbo].[db_talbe_list]</font>,<font color=#CD853F >[dbo].[db_talbe_columns]</font>)和储存过程(<font color=#CD853F> [dbo].[db_detail_add]</font>)
copy到目标数据库中执行       

效果图
![pic1](http://p044t0c5z.bkt.clouddn.com/2017-11-29_153218.png)
![pic1](http://p044t0c5z.bkt.clouddn.com/2017-11-29_153258.png)
![pic1](http://p044t0c5z.bkt.clouddn.com/2017-11-29_153338.png)
导出word文档效果图
![pic1](http://p044t0c5z.bkt.clouddn.com/2017-11-29_153439.png)
