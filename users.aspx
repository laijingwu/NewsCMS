<%@ Page Language="C#" AutoEventWireup="true" CodeFile="users.aspx.cs" Inherits="users" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <title>用户信息 | News CMS</title>
    <link rel="stylesheet" href="styles/index.css" />
    <link rel="stylesheet" href="styles/css.css" />
    <script src='scripts/jquery-1.11.2.min.js' type='text/javascript' charset="utf-8"></script>
    <script type="text/javascript" src="scripts/simpla.jquery.configuration.js"></script>
    <style>
        .user_form {
            width: 350px;
            margin: 50px auto;
            padding: 20px 23px;
            font-size: 14px;
            border: 2px solid #4eabb7;
        }
        .user_form td { padding: 5px 0; font-size: 14px; }
    </style>
</head>
<body>
    <div>
        <div id="banner">
            <div id="banMid">
                <h1><asp:Image runat="server" ID="SiteLogo" ImageUrl="images/team-logo.png" Width="60" Height="60" /> News CMS</h1>
                <span>A code monkey in PHP & C/C++</span>
            </div>
        </div>
        <div id="mainNav" style="background-image: url(images/mainNavBg.jpg)">
            <ul id="navMid">
                <li><a href="index.aspx">首页</a></li>
                <li><a href="list.aspx">新闻中心</a></li>
                <li><a style="color: red;" href="share.aspx">马上爆料</a></li>
                <li><a href="aboutexamining.aspx">关于考核</a></li>
                <%--<li><a href="login.aspx">登录</a></li>
                <li><a href="register.aspx">注册</a></li>--%>
                <li><a href="admin/login.aspx">后台登录</a></li>
            </ul>
        </div>
        <div id="container">
            <div id="main">
                <div class="user_form"">
                    <form runat="server" method="post">
                    <table border="0" style="width: 350px;">
                        <tr>
                            <td>头像</td>
                            <td><asp:Image runat="server" ID="UserLogoImg" Width="40" Height="40" ImageUrl="images/userpic.gif" style="vertical-align:middle; padding-bottom: 2px;"/></td>
                        </tr>
                        <tr>
                            <td>用户名</td>
                            <td><%=User["username"].ToString() %></td>
                        </tr>
                        <tr>
                            <td>用户级别</td>
                            <td><%=UserGroup["group_name"].ToString() %></td>
                        </tr>
                        <tr>
                            <td>昵称</td>
                            <td><asp:Label runat="server" ID="UserNickname"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>邮箱</td>
                            <td><asp:Label runat="server" ID="UserEmail"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>注册时间</td>
                            <td><%=string.Format("{0:yyyy-MM-dd HH:mm:ss}", User["registered"]) %></td>
                        </tr>
                    </table>
                    </form>
                </div>
            </div>
        </div>
        <div id="footer">
            <div id="footerMid">
                <div class="gadget">
                    <div style="text-align: center; color: white; padding-top: 30px;">Powered by Lai Jingwu.</div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
