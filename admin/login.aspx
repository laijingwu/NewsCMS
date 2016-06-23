<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="admin_login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <title>管理后台 | News CMS</title>
    <style type="text/css">/*<![CDATA[*/
        @import "resources/css/login.css";
    /*]]>*/</style>
</head>
<body>
    <div id="container">
        <form method="post" runat="server">
		<h1>管理后台 | News CMS <small style="font-size: 15px;"><asp:HyperLink NavigateUrl="../index.aspx" runat="server" Text="返回前台" ForeColor="Gray"></asp:HyperLink></small></h1>
		<div id="box">
			<p class="main">
				<label>用户名: </label>
                <asp:TextBox ID="UsernameInput" CssClass="text" runat="server" placeholder="默认为admin" required=""></asp:TextBox>
				<label>密码: </label>
                <asp:TextBox ID="PasswordInput" TextMode="Password" CssClass="text" runat="server" placeholder="默认为admin" required=""></asp:TextBox>
            </p>
            <p class="main" style="padding-top:10px;">
                <label>验证码: </label>
                <asp:TextBox ID="VerityCodeInput" CssClass="text" runat="server" MaxLength="4" required=""></asp:TextBox>
                &nbsp;
                <asp:Image runat="server" CssClass="verityImg" ImageUrl="~/ValidateCode.aspx" Height="25" />
			</p>
			<p class="space">
				<span><asp:CheckBox ID="RememberStatus" Text="记住我" runat="server" /></span>
                <asp:Button runat="server" CssClass="login" ID="Login" style="cursor: pointer;" OnClick="Login_Click" Text="登录" />
			</p>
		</div>
        </form>
	</div>
</body>
</html>
