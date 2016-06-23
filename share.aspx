<%@ Page Language="C#" AutoEventWireup="true" CodeFile="share.aspx.cs" Inherits="share" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <title>新闻爆料 | News CMS</title>
    <link rel="stylesheet" href="styles/index.css" />
    <link rel="stylesheet" href="styles/css.css" />
    <script src='scripts/jquery-1.11.2.min.js' type='text/javascript' charset="utf-8"></script>
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
                <li><a class="active" style="color: red;" href="share.aspx">马上爆料</a></li>
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
                        <p class="blockTitle2">当前位置 >> <a href="share.aspx"><b>新闻爆料/分享</b></a></p>
                        <div class="clear"></div>
                        <div class="art_content">
                            <div class="article">
                                <h2>新闻大爆料</h2>
                                <div class="clear"></div>
                                <p>你可以向我们分享生活中发生的热点问题、新鲜好玩的新闻等。</p>
                                <p>当然，这一切我们都不会为此支付稿酬 : )</p>
                            </div>
                            <div class="article">
                                <h2>立刻分享！</h2>
                                <div class="clear"></div>
                                <form method="post" id="sendpost" runat="server">
                                    <ol>
                                        <li>
                                            <label id="LabelPostTitle" for="PostTitleInput">标题</label>
                                            <asp:TextBox ID="PostTitleInput" CssClass="text" runat="server" placeholder="必填" required=""></asp:TextBox>
                                        </li>
                                        <li>
                                            <label id="LabelPostAuthorNickname" for="PostAuthorNickInput">发布人</label>
                                            <asp:TextBox ID="PostAuthorNickInput" CssClass="text" runat="server" placeholder="必填" required=""></asp:TextBox>
                                        </li>
                                        <li>
                                            <label id="LabelPostAuthorEmail" for="PostAuthorEmailInput">发布人邮箱</label>
                                            <asp:TextBox ID="PostAuthorEmailInput" CssClass="text" TextMode="Email" placeholder="必填" runat="server" required=""></asp:TextBox>
                                        </li>
                                        <li>
                                            <label id="LabelPostContent">文章内容</label>
                                            <iframe src="wysiwyg.html" id="editor_iframe" width="580" height="300" style="overflow: hidden; border: none;"></iframe>
                                        </li>
                                        <li>
                                            <label id="LabelPostCategory">新闻分类</label>
                                            <asp:DropDownList ID="CategoryList" runat="server">
                                            </asp:DropDownList>
                                        </li>
                                        <li>
                                            <label id="LabelPostTag">标签</label>
                                            <asp:TextBox ID="PostTagInput" CssClass="text" runat="server" placeholder="标签以空格分隔"></asp:TextBox>
                                        </li>
                                        <li>
                                            <asp:HiddenField ID="EditorContent" runat="server" />
                                            <input type="image" name="imageField" id="imageField" src="images/submit.gif" class="send" />
                                            <asp:Button runat="server" CssClass="send" ID="PostSend" style="background: url(images/submit.gif) no-repeat center; width: 88px; height: 29px; border:none; cursor: pointer; display:none;" OnClick="PostSend_Click" />
                                            <div class="clr"></div>
                                        </li>
                                    </ol>
                                </form>
                            </div>
                        </div>
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
    <script>
        function search() {
            var search = $("#searchInput").val();
            if (search == "") {
                alert("搜索内容不能为空！");
                return false;
            }
        }
        $(function () {
            $("#imageField").bind("click", function (event) {
                var editorText = $("#editor_iframe").contents().find("#editor").html();
                if (editorText == "") {
                    alert("新闻内容为空！");
                    return false;
                }
                $("#EditorContent").val(encodeURIComponent($("#editor_iframe").contents().find("#editor").html()));
                $("#PostSend").click();
                return false;
            });
            $("#LabelPostContent").bind("click", function (event) {
                $("#editor_iframe").contents().find("#editor").focus();
            });
        })
    </script>
</body>
</html>
