<%@ Page Language="C#" AutoEventWireup="true" CodeFile="search.aspx.cs" Inherits="search" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <title>搜索结果 | 新闻中心</title>
    <link rel="stylesheet" href="styles/index.css" />
    <link rel="stylesheet" href="styles/css.css" />
    <script src='scripts/jquery-1.11.2.min.js' type='text/javascript' charset="utf-8"></script>
    <script type="text/javascript">
        function search() {
            var search = $("#searchInput").val();
            if (search == "")
            {
                alert("搜索内容不能为空！");
                return false;
            }
        }
    </script>
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
                <li><a class="active" href="list.aspx">新闻中心</a></li>
                <li><a style="color: red;" href="share.aspx">马上爆料</a></li>
                <li><a href="aboutexamining.aspx">关于考核</a></li>
                <%--<li><a href="login.aspx">登录</a></li>
                <li><a href="register.aspx">注册</a></li>--%>
                <li><a href="admin/login.aspx">后台登录</a></li>
            </ul>
        </div>
        <div id="container">
            <div id="main">
                <div id="mainLeft">
                    <div class="class_left">
                        <span class="blockTitle">搜索 <i>Search</i></span>
                        <div class="clear"></div>
                        <form method="post" action="search.aspx" onsubmit="return search();">
                            <input name="searchInput" id="searchInput" type="text" placeholder="请输入关键字" />
                            <input id="submitBtn" type="submit" class="pointer" style="cursor: pointer;" value="搜索" />
                        </form>
                        
                    </div>
                    <div class="class_left">
                        <span class="blockTitle">新闻分类 <i>Category</i></span>
                        <div class="clear"></div>
                        <ul>
                            <li><a href="list.aspx" target="_blank">全部新闻</a></li>
                            <asp:Repeater ID="NewsCategoryList" runat="server">
                                <ItemTemplate>
                                    <li><a href="list.aspx?cid=<%#Eval("cid") %>" target="_blank"><%#Eval("category_title") %></a></li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                    <div class="class_left">
                        <span class="blockTitle">友情链接 <i>Link</i></span><div class="clear"></div>
                        <ul>
                            <li><a href="http://wangyuan.info">网园主页</a></li>
                            <li><a href="http://www.sgu.gd.cn">网园博客</a></li>
                            <li><a href="http://www.7bus.net">7bus</a></li>
                            <li><a href="http://wy.zeffee.com">网园作业平台</a></li>
                            <li><a href="https://www.laijingwu.com/">我的主站</a></li>
                        </ul>
                    </div>
                </div>
                <div id="mainRight">
                    <div id="show">
                        <form runat="server" method="post">
                        <p class="blockTitle2">当前位置 >> <asp:Label ID="NavPositon" runat="server" style="float: none;"><b>搜索结果</b></asp:Label></p>
                        <div class="clear"></div>
                        <div class="list_ul">
                            <asp:Label runat="server" ID="NoResult" style="float: none; padding:0 30px; top: 20px; position: relative;" Visible="false">搜索结果为空！</asp:Label>
                                <asp:Repeater ID="NewestList" runat="server">
                                    <HeaderTemplate><ul></HeaderTemplate>
                                     <ItemTemplate>
                                         <li><a href="article.aspx?pid=<%#Eval("pid") %>"><%#Eval("post_title") %></a><span><%#Eval("published","{0:yyyy-MM-dd HH:mm:ss}") %></span></li>
                                     </ItemTemplate>
                                    <FooterTemplate></ul></FooterTemplate>
                                </asp:Repeater>
                        </div>
                        <div class="clear"></div>
                           <div style="text-align: right;">共搜索到<asp:Label runat="server" ID="totalItem" Text="0" style="float: none;"></asp:Label>条。</div>
                         </form>
                    </div>
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
