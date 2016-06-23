<%@ Page Language="C#" AutoEventWireup="true" CodeFile="users.aspx.cs" Inherits="admin_users" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <title>修改用户信息 | News CMS</title>
    <link rel="stylesheet" href="resources/css/reset.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="resources/css/style.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="resources/css/invalid.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="resources/css/tab.css" type="text/css" />
    <script type="text/javascript" src="resources/scripts/jquery-1.3.2.min.js"></script>
    <script type="text/javascript" src="resources/scripts/simpla.jquery.configuration.js"></script>
    <script type="text/javascript" src="resources/scripts/facebox.js"></script>
    <script type="text/javascript" src="resources/scripts/jquery.wysiwyg.js"></script>
    <script type="text/javascript" src="resources/scripts/jquery.datePicker.js"></script>
    <script type="text/javascript" src="resources/scripts/jquery.date.js"></script>
    <script type="text/javascript" src="resources/scripts/js.js"></script>
</head>
<body>
    <div id="body-wrapper">
        <form method="post" runat="server">
        <div id="sidebar">
        <div id="sidebar-wrapper">
            <h1 id="sidebar-title"><a href="index.aspx">修改用户信息 | News CMS</a></h1>
            <div style="text-align: center; margin-top: 40px;">
                <a href="../index.aspx" target="_blank"><asp:Image runat="server" ID="SiteLogo" ImageUrl="resources/images/logo.png" Width="60" Height="60" /></a>
            </div>
            <a href="index.aspx">
                <img id="logo" src="resources/images/logo.png" alt="News CMS" /></a>
            <div id="profile-links">
                Hello, <a target="_blank" href="users.aspx?uid=<%=LoginedUser["uid"].ToString() %>" title="admin"><%=string.IsNullOrEmpty(LoginedUser["nickname"].ToString()) ? LoginedUser["username"].ToString() : LoginedUser["nickname"].ToString() %></a><br />
                <br />
                <a href="../index.aspx" title="View the Site" target="_blank">查看网站</a> | <asp:LinkButton runat="server" ID="LogoutBtn" OnClick="LogoutBtn_Click">退出</asp:LinkButton>
            </div>
            <div style="padding: 0 5px;">
             <div class="content-box">
                <div class="content-box-header">
                    <h3>三轮考核完成</h3>
                </div>
                <div class="content-box-content">
                    <div class="tab-content default-tab">
                        <p>师兄师姐，新年快乐！</p>
                    </div>
                </div>
            </div>
            <div class="content-box closed-box">
                <div class="content-box-header">
                    <h3>其他个人站点</h3>
                </div>
                <div class="content-box-content">
                    <div class="tab-content default-tab tab-link">
                        <p><a href="https://www.laijingwu.com">个人主站</a></p>
                        <p><a href="http://weibo.com/208488849/">新浪微博</a></p>
                        <p><a href="https://git.oschina.net/laijingwu">Git@OSC</a></p>
                        <p><a href="http://i.youku.com/laijingwu">优酷自频道</a></p>
                    </div>
                </div>
            </div>
            </div>
        </div>
    </div>
	
    <div id="main-content">  
        <h2>Welcome</h2>
            <p id="page-intro">What would you like to do?</p>
        <ul class="shortcut-buttons-set">
                <li><a class="shortcut-button" href="index.aspx"><span>
                    <img src="resources/images/icons/pencil_48.png" alt="icon" /><br />
                    返回后台 </span></a></li>
        </ul>
        <div class="clear"></div>
        <div class="content-box">
                <div class="content-box-header">
                    <h3>用户信息修改</h3>
                    <div class="clear"></div>
                </div>
                <div class="content-box-content">
                    <table border="0" style="text-align: center; margin: auto;">
                        <tr>
                            <td><asp:Image runat="server" ID="UserLogoImg" Width="40" Height="40" ImageUrl="../images/userpic.gif" style="vertical-align:middle; padding-bottom: 2px;"/></td>
                            <td><asp:FileUpload runat="server" ID="UploadLogoImg" AllowMultiple="false" style="display: none;" onchange="checkformat(this);"/><a onclick="upload()" style="cursor: pointer; color: #4eabb7;">更换头像</a></td>
                        </tr>
                        <tr>
                            <td>用户名</td>
                            <td><asp:TextBox CssClass="text" runat="server" ID="UsernameInput" placeholder="必填" required=""></asp:TextBox></td>
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
                            <td>
                                <asp:DropDownList ID="UserGroupList" runat="server">
                                   <asp:ListItem>请选择</asp:ListItem>
                                </asp:DropDownList>
                            </td>
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
                            <td><%=string.Format("{0:yyyy-MM-dd HH:mm:ss}", CheckedUser["registered"]) %></td>
                        </tr>
                        <tr><td colspan="2"><div style="text-align: center; position:relative; bottom: -5px;"><asp:Button runat="server" CssClass="button" ID="UserUpdateBtn" OnClick="UserUpdateBtn_Click" Text="确认修改" /></div></td></tr>
                    </table>
                    <div class="clear"></div>
                </div>
            </div>
            
        <div class="clear"></div>
        <div id="footer">
                <small>&#169; Copyright 2016 网园资讯工作室 | Powered by <a href="https://www.laijingwu.com">Lai Jingwu</a>. | <a href="#">Top</a> </small>
        </div>
    </div>
        </form>
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
