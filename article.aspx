<%@ Page Language="C#" AutoEventWireup="true" CodeFile="article.aspx.cs" Inherits="article" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <title>新闻中心 | News CMS</title>
    <link rel="stylesheet" href="styles/index.css" />
    <link rel="stylesheet" href="styles/css.css" />
    <!-- jQuery -->
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
                        <p class="blockTitle2">当前位置 >> <a href="list.aspx" target="_self" class="">新闻中心</a> >> <a href="list.aspx?cid=<%=Post["cid"].ToString() %>"><%=PostCategory["category_title"].ToString() %>新闻</a> >> <asp:Label runat="server" ID="NavTitle" CssClass="no-float"><b>最新新闻</b></asp:Label></p>
                        <div class="clear"></div>
                        <div class="art_content">
                            <h3><asp:Label runat="server" ID="PostTitle" CssClass="no-float"></asp:Label></h3>
                            <h4>分享者:<% if (string.IsNullOrEmpty(AuthorDisplayname)) { %><a href="users.aspx?uid=<%=Post["uid"].ToString() %>"><%=string.IsNullOrEmpty(Author["nickname"].ToString()) ? Author["username"].ToString() : Author["nickname"].ToString() %></a><% } else { %><%=AuthorDisplayname %><% } %>&nbsp;&nbsp;新闻类别:<a href="list.aspx?cid=<%=Post["cid"].ToString() %>"><%=PostCategory["category_title"].ToString() %>新闻</a>&nbsp;&nbsp;发布时间:<%=string.Format("{0:yyyy-MM-dd HH:mm:ss}", Post["published"]) %>&nbsp;&nbsp;阅读量:<%=Post["post_read"].ToString() %></h4>
                            <div id="article">
                                <%=HttpUtility.UrlDecode(Post["post_content"].ToString()) %>
                            </div>
                            <asp:Repeater ID="PostTagList" runat="server" OnItemDataBound="PostTagList_ItemDataBound">
                                 <HeaderTemplate><p><b>标签</b>: </HeaderTemplate>
                                 <ItemTemplate><u><a href="#"><%#Container.DataItem %></a></u></ItemTemplate>
                                 <SeparatorTemplate>&nbsp;&nbsp;</SeparatorTemplate>
                                 <FooterTemplate></p></FooterTemplate>
                            </asp:Repeater>
                        </div>
                        <form method="post" id="leavereply" runat="server">
                        <div class="art_comment">
                            <h2>讨论(<%=Post["comments_count"].ToString() %>)</h2>
                            <div class="clr"></div>
                            <%--<asp:Repeater ID="CommentsList" runat="server" OnItemDataBound="CommentsList_ItemDataBound">
                                <ItemTemplate>
                                    <div class="comment">
                                        <asp:HyperLink NavigateUrl="users.aspx" runat="server" ID="CommenterLogoLink"><asp:Image runat="server" CssClass="userpic" ID="CommenterLogo" Height="35" Width="35" ImageUrl="images/userpic.gif" /></asp:HyperLink>
                                        <p><asp:HyperLink NavigateUrl="users.aspx" runat="server" ID="CommenterLink"><asp:Label runat="server" ID="CommenterDisplayName" CssClass="no-float"></asp:Label></asp:HyperLink> 回复:<span style="margin-right:10px;"><small><%#Eval("commented", "{0:yyyy-MM-dd HH:mm:ss}") %></small></span></p>
                                        <p><%#Eval("comment_content") %></p>
                                        <p><small><a style="cursor: pointer; text-decoration: underline; color: darkblue;" onclick="commentit(<%#Eval("comment_id") %>,<%#Container.ItemIndex %>)">评论Ta</a></small></p>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>--%>
                            <asp:Repeater ID="ParentCommentList" runat="server" OnItemDataBound="ParentCommentList_ItemDataBound">
                                <ItemTemplate>
                                    <div class="comment">
                                        <asp:HyperLink NavigateUrl="users.aspx" runat="server" ID="CommenterLogoLink"><asp:Image runat="server" CssClass="userpic" ID="CommenterLogo" Height="35" Width="35" ImageUrl="images/userpic.gif" /></asp:HyperLink>
                                        <p><asp:HyperLink NavigateUrl="users.aspx" runat="server" ID="CommenterLink"><asp:Label runat="server" ID="CommenterDisplayName" CssClass="no-float"></asp:Label></asp:HyperLink> 回复:<span style="margin-right:10px;"><small><%#Eval("commented", "{0:yyyy-MM-dd HH:mm:ss}") %></small></span></p>
                                        <p><%#Eval("comment_content") %></p>
                                        <p><small><a style="cursor: pointer; text-decoration: underline; color: darkblue;" onclick="commentit(<%#Eval("comment_id") %>,<%#Container.ItemIndex %>)">评论Ta</a></small></p>
                                    </div>
                                    <asp:Repeater ID="SonCommentList" runat="server" OnItemDataBound="SonCommentList_ItemDataBound">
                                        <HeaderTemplate><div style="margin-bottom: 20px;"></HeaderTemplate>
                                        <ItemTemplate>
                                             <div class="comment" style="margin-left: 30px;">
                                                <asp:HyperLink NavigateUrl="users.aspx" runat="server" ID="SonCommenterLogoLink"><asp:Image runat="server" CssClass="userpic" ID="SonCommenterLogo" Height="35" Width="35" ImageUrl="images/userpic.gif" /></asp:HyperLink>
                                                <p><asp:HyperLink NavigateUrl="users.aspx" runat="server" ID="SonCommenterLink"><asp:Label runat="server" ID="SonCommenterDisplayName" CssClass="no-float"></asp:Label></asp:HyperLink> 回复:<span style="margin-right:10px;"><small><%#Eval("commented", "{0:yyyy-MM-dd HH:mm:ss}") %></small></span></p>
                                                <p><%#Eval("comment_content") %></p>
                                            </div>
                                         </ItemTemplate>
                                        <FooterTemplate></div></FooterTemplate>
                                    </asp:Repeater>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div class="art_comment">
                            <h2 id="CommentTitle">评论</h2>
                            <div class="clr"></div>
                                <ol>
                                    <li>
                                        <label id="LabelCommentNickname" for="CommentNickname">昵称 (必填)</label>
                                        <asp:TextBox ID="CommentNickname" CssClass="text" runat="server" required=""></asp:TextBox>
                                    </li>
                                    <li>
                                        <label id="LabelCommentEmail" for="CommentEmail">邮箱 (必填)</label>
                                        <asp:TextBox ID="CommentEmail" CssClass="text" TextMode="Email" runat="server" required=""></asp:TextBox>
                                    </li>
                                    <li>
                                        <label id="LabelCommentContent" for="CommentContent">内容</label>
                                        <asp:TextBox TextMode="MultiLine" ID="CommentContent" Rows="6" Columns="50" runat="server" required=""></asp:TextBox>
                                    </li>
                                    <li>
                                        <asp:HiddenField runat="server" Value="" ID="LastCommentedId" />
                                        <asp:Button runat="server" CssClass="send" ID="CommentSend" style="background: url(images/submit.gif) no-repeat center; width: 88px; height: 29px; border:none; cursor: pointer;" OnClick="CommentSend_Click" />
                                        <div class="clr"></div>
                                    </li>
                                </ol>
                        </div>
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

    <script type="text/javascript">
        // 评论子回复
        //function commentit(comment_id, index) {
        //    $("#LastCommentedId").val(comment_id);
        //    var display_name = $("#CommentsList_CommenterDisplayName_" + index).text();
        //    $("#CommentTitle").html("评论 " + display_name + "&nbsp;&nbsp;<small><a style=\"font-size: 14px; font-weight: normal; cursor: pointer; text-decoration: underline; color: darkblue;\" onclick=\"cancel()\">取消</a></small>");
        //    $("html,body").animate({ scrollTop: $("#CommentTitle").offset().top }, 1000);
        //}

        function commentit(comment_id, index) {
            $("#LastCommentedId").val(comment_id);
            var display_name = $("#ParentCommentList_CommenterDisplayName_" + index).text();
            $("#CommentTitle").html("评论 " + display_name + "&nbsp;&nbsp;<small><a style=\"font-size: 14px; font-weight: normal; cursor: pointer; text-decoration: underline; color: darkblue;\" onclick=\"cancel()\">取消</a></small>");
            $("html,body").animate({ scrollTop: $("#CommentTitle").offset().top }, 1000);
        }

        function cancel() {
            $("#LastCommentedId").val("");
            $("#CommentTitle").html("评论");
        }

        function search() {
            var search = $("#searchInput").val();
            if (search == "") {
                alert("搜索内容不能为空！");
                return false;
            }
        }
    </script>
</body>
</html>
