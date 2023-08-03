<p align="center">
    <img src="images/logo.png" width=150/>
</p>
<h2 align="center">Hamibot遥控器</h2>
<div align="center">
Hamibot遥控器是基于Hamibot官方开放的API接口开发的一款自动化脚本运行控制APP
</div>

<p align="center">
    <a href="https://github.com/laosanyuan/HamibotRemoteControl/stargazers">
        <img alt="GitHub stars" src="https://img.shields.io/github/stars/laosanyuan/HamibotRemoteControl">
    </a>
    <a href="https://github.com/laosanyuan/HamibotRemoteControl/network">
        <img alt="GitHub forks" src="https://img.shields.io/github/forks/laosanyuan/HamibotRemoteControl">
    </a>
    <a>
        <img alt="Actions" src="https://img.shields.io/github/downloads/laosanyuan/HamibotRemoteControl/total">
    </a>
</p>

## 简介

[Hamibot](https://hamibot.com/) 是一款适用于安卓系统的自动化工具，能操控任意 APP，实现自动化操作，提高工作效率。 

由于Hamibot官方当前只提供了网页控制台操作和定时执行两种方式来管理脚本的运行，只能通过网页端来查看状态和执行操作，使用起来不是很方便。但是提供了官方API供用户使用：https://docs.hamibot.com/rest/overview/。

Hamibot遥控器就是基于这个前提开发的供用户使用的可以通过app查看机器人状态、执行脚本运行操作的开源第三方工具。

## 使用效果

<div style="width: 750px; overflow: hidden;">
    <img src="images/app_home.jpg" style="float: left; width: 250px;" />
    <img src="images/app_scripts.jpg" style="float: left; width: 250px;" />
    <img src="images/app_settings.jpg" style="float: left; width: 250px;" />
</div>

## 使用方法

1. 注册Hamibot：https://hamibot.com/referrals/m7ax
2. 在“设置-开发者-令牌”中获取个人访问令牌。
3. 安装Hamibot遥控器。
4. 打开设置填入个人令牌。

个人令牌获取位置：

![example](images/hamibot_example.png)

## 功能

### 现有功能

当前支持的功能都是根据官方API开发，主要提供了以下几个功能：

1. 获取机器人、脚本信息。
2. 运行脚本。
3. 停止脚本运行。
4. 设置API请求方式。

### TODO

1. 支持修改机器人
2. 支持隐藏不需要的脚本、机器人。
3. 支持统计API调用次数统计。
4. 优化UI。
5. 增加版本更新提醒。

## 注意

由于API为官方提供，使用本软件时需要遵守官方的使用规范。

API配额当前为每月300次，超出使用额度后，如需继续使用需要到官网自行购买。为节省额度，建议在app设置中将自动刷新关闭，只在必要的时候手动进行机器人列表、脚本列表的更新。脚本类型可以只获取自己使用的类型，也会减少API请求次数。

当然，如果额度足够，开启自动刷新会获得更好的使用体验。

## 其他

如有任何好的建议和使用问题，可以联系作者进行交流。

![qr](images/qr_code.jpg)



##  免责声明

**本项目开发初衷仅为学习及技术交流，切勿将其用于任何非法用途，否则一切后果自负，与本项目及作者无关。**