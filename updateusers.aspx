<%@ Page Language="C#" AutoEventWireup="true" CodeFile="updateusers.aspx.cs" Inherits="updateusers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <title>修改用户信息 | News CMS</title>
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
                            <td><asp:Image runat="server" ID="UserLogoImg" Width="40" Height="40" ImageUrl="images/userpic.gif" style="vertical-align:middle; padding-bottom: 2px;"/></td>
                            <td><asp:FileUpload runat="server" ID="UploadLogoImg" AllowMultiple="false" style="display: none;" onchange="checkformat(this);"/><a onclick="upload()" style="cursor: pointer; color: #4eabb7;">更换头像</a></td>
                        </tr>
                        <tr>
                            <td>用户名</td>
                            <td><%=User["username"].ToString() %></td>
                        </tr>
                        <tr>
                            <td>旧密码</td>
                            <td><asp:TextBox CssClass="text" runat="server" ID="UserOldPwdInput" TextMode="Password" placeholder="必填" required=""></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>新密码</td>
                            <td><asp:TextBox runat="server" ID="UserNewPwdInput" TextMode="Password" placeholder="不修改请留空"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>确认密码</td>
                            <td><asp:TextBox runat="server" ID="UserNewPwd1Input" TextMode="Password" placeholder="不修改请留空"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>用户级别</td>
                            <td><%=UserGroup["group_name"] %></td>
                        </tr>
                        <tr>
                            <td>昵称</td>
                            <td><asp:TextBox runat="server" ID="UserNicknameInput" MaxLength="15" required=""></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>邮箱</td>
                            <td><asp:TextBox runat="server" ID="UserEmailInput" TextMode="Email" required=""></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>注册时间</td>
                            <td><%=string.Format("{0:yyyy-MM-dd HH:mm:ss}", User["registered"]) %></td>
                        </tr>
                        <tr><td colspan="2"><div style="text-align: center; position:relative; bottom: -5px;"><asp:Button runat="server" CssClass="send" ID="UploadUser" style="background: url(images/submit.gif) no-repeat center; width: 88px; height: 29px; border:none; cursor: pointer;" OnClick="UploadUser_Click" /></div></td></tr>
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
    <script>
        function upload() {
            $('#UploadLogoImg').click();
        }
        function checkformat(target) {
            var fileSize = 0;
            fileSize = target.files[0].size;
            var size = fileSize / 1024;
            if (size > 5000) {
                alert("头像不能大于5M");
                target.value = "";
                return
            }

            var name = target.value;
            var fileName = name.substring(name.lastIndexOf(".") + 1).toLowerCase();
            if (fileName != "jpg" && fileName != "jpeg" && fileName != "png" && fileName != "bmp" && fileName != "gif") {
                alert("请选择支持的图片格式文件上传(jpg,png,gif,bmp等)");
                target.value = "";
                return
            }
        }
    </script>
</body>
</html>
