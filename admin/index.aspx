<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="admin_index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <title>管理后台 | News CMS</title>
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
            <h1 id="sidebar-title"><a href="index.aspx">管理后台 | News CMS</a></h1>
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
	
    <asp:HiddenField runat="server" ID="TabIndex" Value="1" />
    <div id="main-content">  
        <h2>Welcome</h2>
            <p id="page-intro">What would you like to do?</p>
        <ul class="shortcut-buttons-set">
                <li><div id="addArticle"><a class="shortcut-button" href="#"><span>
                    <img src="resources/images/icons/pencil_48.png" alt="icon" /><br />
                    添加新闻 </span></a></div></li>
                <li><div id="addMain"><a class="shortcut-button" href="#"><span>
                    <img src="resources/images/icons/paper_content_pencil_48.png" alt="icon" /><br />
                    添加分类 </span></a></div></li>
                <li>
                    <a class="shortcut-button" style="cursor: pointer;" id="sitelogolink" onclick="uploadlogo();"><span>
                    <img src="resources/images/icons/image_add_48.png" alt="icon" /><br />
                    上传站标 </span></a><asp:FileUpload runat="server" ID="WebSiteLogoInput" style="display: none;" onchange="checkformat(this);" /><asp:LinkButton runat="server" ID="WebSiteLogoUplBtn" OnClick="WebSiteLogoUplBtn_Click" style="display: none;" Text="Upload"></asp:LinkButton></li>
        </ul>
        <div class="clear"></div>
		<div class="video_show" id="video_show">
	        <div class="black">
	        </div>
	        <div class="white">
	        	<hr class="hr1" /><br />
	        	<label class="lab">添加新分类</label>
                <asp:TextBox runat="server" CssClass="text-input small-input" ID="addCategoryTitle" placeholder="分类名不能为空" Width="200"></asp:TextBox>
                <asp:LinkButton runat="server" CssClass="button" Text="确认提交" ID="addCategoryBtn" OnClick="addCategoryBtn_Click"></asp:LinkButton><br /><br />
        		<hr class="hr2" />
                <a style="cursor: pointer;" id="exit" class="img_close"><img src="./resources/images/closelabel.gif" /></a>
	        </div>
	    </div>
        <div class="content-box">
                <div class="content-box-header">
                    <h3>控制板</h3>
                    <ul class="content-box-tabs">
                        <li><a id="navTab1" href="#tab1" class="default-tab">新闻管理</a></li>
                        <li><a id="navTab2" href="#tab2">发布新闻</a></li>
                        <li><a id="navTab3" href ="#tab3">用户管理</a></li>
                        <li><a id="navTab4" href ="#tab4">分类管理</a></li>
                    </ul>
                    <div class="clear"></div>
                </div>
                <div class="content-box-content">
                    <!-- 文章管理部分 -->
                    <div class="tab-content default-tab" id="tab1">
                        <table>
                            <thead>
                                <tr>
                                    <th><input class="check-all" type="checkbox" /></th>
                                    <th>新闻标题</th>
                                    <th>所属分类</th>
                                    <th>发布人</th>
                                    <th>添加时间</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tfoot>
                                <tr>
                                    <td colspan="6">
                                        <div class="bulk-actions align-left">
                                            <asp:DropDownList ID="CategoryPageList" runat="server" OnSelectedIndexChanged="CategoryPageList_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem>全部</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="pagination">
                                            <asp:LinkButton ID="lbtnfirstPage" runat="server" OnClick="lbtnfirstPage_Click">&laquo; 首页</asp:LinkButton>
                                            <asp:LinkButton ID="lbtnpritPage" runat="server" OnClick="lbtnpritPage_Click">&laquo; 上一页</asp:LinkButton>
                                            <asp:Label runat="server" ID="currentPage" Text="1"></asp:Label> / <asp:Label runat="server" ID="totalPage" Text="1"></asp:Label>
                                            <asp:LinkButton ID="lbtnnextPage" runat="server" OnClick="lbtnnextPage_Click">下一页 &raquo;</asp:LinkButton>
                                            <asp:LinkButton ID="lbtnlastPage" runat="server" OnClick="lbtnlastPage_Click">尾页 &raquo;</asp:LinkButton>
                                        </div>
                                        <div class="clear"></div>
                                    </td>
                                </tr>
                            </tfoot>
                            <tbody>
                                    <asp:Repeater ID="NewsList" runat="server" OnItemDataBound="NewsList_ItemDataBound" OnItemCommand="NewsList_ItemCommand">
                                         <ItemTemplate>
                                            <tr>
                                                <td><input type="checkbox" /></td>
                                                <td><a href="../article.aspx?pid=<%#Eval("pid") %>" target="_blank"><%#Eval("post_title") %></a></td>
                                                <td><a href="../list.aspx?cid=<%#Eval("cid") %>" target="_blank"><asp:Label runat="server" ID="CategoryTitle"></asp:Label></a></td>
                                                <td><asp:HyperLink runat="server" ID="AuthorLink" Target="_blank" NavigateUrl="users.aspx"><asp:Label runat="server" ID="Author"></asp:Label></asp:HyperLink></td>
                                                <td><%#Eval("published","{0:yyyy-MM-dd HH:mm:ss}") %></td>
                                                <td>
                                                    <a href="article.aspx?pid=<%#Eval("pid") %>" title="Edit"><img src="resources/images/icons/pencil.png" alt="Edit" /></a>
                                                    <asp:LinkButton ID="lbtnDeletePost" runat="server" CommandName="delete" CommandArgument='<%#Eval("pid") %>'><img src="resources/images/icons/cross.png" alt="Delete" /></asp:LinkButton>
                                                </td>
                                            </tr>
                                         </ItemTemplate>
                                    </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                  <!-- 发布文章部分 -->
                    <div class="tab-content" id="tab2">
                            <fieldset>
                                <p>
                                    <label for="AddNewTitle">新闻标题</label>
                                    <asp:TextBox runat="server" CssClass="text-input medium-input datepicker" ID="AddNewTitle" oninput="OnInput (event)" onpropertychange="OnPropChanged (event)" required=""></asp:TextBox>
                                    <span id="AddTitleNoticeSpan" class="input-notification error png_bg">新闻标题不能为空</span>
                                </p>
                                <p>
                                    <label>新闻分类</label>
                                    <asp:DropDownList ID="CategoryNewsList" runat="server">
                                    </asp:DropDownList>
                                </p>
                                <p>
                                    <label id="LabelPostContent">内容</label>
                                    <iframe src="../wysiwyg.html" id="editor_iframe" width="80%" height="300" style="overflow: hidden; border: none;"></iframe>
                                </p>
                                <p>
                                    <label for="AddNewTag">标签</label>
                                    <asp:TextBox ID="NewsTagInput" CssClass="text-input medium-input datepicker" runat="server" placeholder="标签以空格分隔" ></asp:TextBox>
                                </p>
                                <p>
                                    <asp:HiddenField ID="EditorContent" runat="server" />
                                    <input id="PostAddBtnImg" class="button" type="submit" value="确认发布" />
                                    <asp:Button runat="server" ID="PostAddBtn" OnClick="PostAddBtn_Click" style="display: none;" />
                                </p>
                            </fieldset>
                            <div class="clear"></div>
                    </div>
                    <!-- 用户管理部分 -->
                     <div class="tab-content" id="tab3">
                        <table>
                            <thead>
                                <tr>
                                    <th>用户头像</th>
                                    <th>用户名</th>
                                    <th>用户级别</th>
                                    <th>昵称</th>
                                    <th>邮箱</th>
                                    <th>注册时间</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tfoot>
                                <tr>
                                    <td colspan="7">
                                        <div class="bulk-actions align-left">
                                            <asp:DropDownList ID="UserGroupsList" runat="server" OnSelectedIndexChanged="UserGroupsList_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem>全部</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="pagination">
                                            <asp:LinkButton ID="lbtnfirstUserPage" runat="server" OnClick="lbtnfirstUserPage_Click">&laquo; 首页</asp:LinkButton>
                                            <asp:LinkButton ID="lbtnpritUserPage" runat="server" OnClick="lbtnpritUserPage_Click">&laquo; 上一页</asp:LinkButton>
                                            <asp:Label runat="server" ID="currentUserPage" Text="1"></asp:Label> / <asp:Label runat="server" ID="totalUserPage" Text="1"></asp:Label>
                                            <asp:LinkButton ID="lbtnnextUserPage" runat="server" OnClick="lbtnnextUserPage_Click">下一页 &raquo;</asp:LinkButton>
                                            <asp:LinkButton ID="lbtnlastUserPage" runat="server" OnClick="lbtnlastUserPage_Click">尾页 &raquo;</asp:LinkButton>
                                        </div>
                                        <div class="clear"></div>
                                    </td>
                                </tr>
                            </tfoot>
                            <tbody>
                                    <asp:Repeater ID="UsersList" runat="server" OnItemDataBound="UsersList_ItemDataBound" OnItemCommand="UsersList_ItemCommand">
                                         <ItemTemplate>
                                            <tr>
                                                <td><asp:Image runat="server" ID="UserLogoImg" CssClass="user_img" /></td>
                                                <td><a target="_blank" href="users.aspx?uid=<%#Eval("uid") %>"><%#Eval("username") %></a></td>
                                                <td><asp:Label runat="server" ID="UserGroupName"></asp:Label></td>
                                                <td><%#Eval("nickname") %></td>
                                                <td><a href="mailto: <%#Eval("email") %>" target="_blank"><%#Eval("email") %></a></td>
                                                <td><%#Eval("registered","{0:yyyy-MM-dd HH:mm:ss}") %></td>
                                                <td>
                                                    <a href="users.aspx?uid=<%#Eval("uid") %>" title="Edit"><img src="resources/images/icons/pencil.png" alt="Edit" /></a>
                                                    <asp:LinkButton ID="lbtnDeleteUser" runat="server" CommandName="delete" CommandArgument='<%#Eval("uid") %>'><img src="resources/images/icons/cross.png" alt="Delete" /></asp:LinkButton>
                                                </td>
                                            </tr>
                                         </ItemTemplate>
                                    </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                    <!-- 分类管理部分 -->
                    <div class="tab-content" id="tab4">
                        <table>
                            <thead>
                                <tr>
                                    <th>分类名称</th>
                                    <th>新闻数量</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tfoot>
                                <tr>
                                    <td colspan="3">
                                        <div class="bulk-actions align-left">
                                            <div class="add" id="add"><a class="button" href="#">添加新类</a></div>
                                        </div>
                                        <div class="pagination">
                                            <asp:LinkButton ID="lbtnfirstCategoryPage" runat="server" OnClick="lbtnfirstCategoryPage_Click">&laquo; 首页</asp:LinkButton>
                                            <asp:LinkButton ID="lbtnpritCategoryPage" runat="server" OnClick="lbtnpritCategoryPage_Click">&laquo; 上一页</asp:LinkButton>
                                            <asp:Label runat="server" ID="currentCategoryPage" Text="1"></asp:Label> / <asp:Label runat="server" ID="totalCategoryPage" Text="1"></asp:Label>
                                            <asp:LinkButton ID="lbtnnextCategoryPage" runat="server" OnClick="lbtnnextCategoryPage_Click">下一页 &raquo;</asp:LinkButton>
                                            <asp:LinkButton ID="lbtnlastCategoryPage" runat="server" OnClick="lbtnlastCategoryPage_Click">尾页 &raquo;</asp:LinkButton>
                                        </div>
                                        <div class="clear"></div>
                                    </td>
                                </tr>
                            </tfoot>
                            <tbody>
                                <asp:Repeater ID="CategoryList" runat="server" OnItemDataBound="CategoryList_ItemDataBound" OnItemCommand="CategoryList_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td><a target="_blank" href="../list.aspx?cid=<%#Eval("cid") %>"><%#Eval("category_title") %></a></td>
                                            <td><asp:Label runat="server" ID="CountNewsInCategory"></asp:Label></td>
                                            <td>
                                                <a href="category.aspx?cid=<%#Eval("cid") %>" title="Edit"><img src="resources/images/icons/pencil.png" alt="Edit" /></a>
                                                <asp:LinkButton ID="lbtnDeleteCategory" runat="server" CommandName="delete" CommandArgument='<%#Eval("cid") %>'><img src="resources/images/icons/cross.png" alt="Delete" /></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
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
        $(function () {
            function changeTab(n) {
                $('.content-box ul.content-box-tabs li a').parent().siblings().find("a").removeClass('current');
                $('#navTab' + n).addClass('current');
                var currentTab = $('#navTab' + n).attr('href');
                $(currentTab).siblings().hide();
                $(currentTab).show();
            }

            changeTab($('#TabIndex').val());

            $("#PostAddBtnImg").bind("click", function (event) {
                var editorText = $("#editor_iframe").contents().find("#editor").html();
                if (editorText == "") {
                    alert("新闻内容为空！");
                    return false;
                }
                $("#EditorContent").val(encodeURIComponent($("#editor_iframe").contents().find("#editor").html()));
                $("#PostAddBtn").click();
                return false;
            });
            $('#addArticle').click(
			    function () {
			        changeTab(2);
			        return false;
			    });
            $("#LabelPostContent").bind("click", function (event) {
                $("#editor_iframe").contents().find("#editor").focus();
            });
        });

        // Firefox, Google Chrome, Opera, Safari, Internet Explorer from version 9
        function OnInput(event) {
            t();
        }
        // Internet Explorer
        function OnPropChanged(event) {
            if (event.propertyName.toLowerCase() == "value") {
                t();
            }
        }

        function t() {
            if ($("#AddNewTitle").val() != "")
                $("#AddTitleNoticeSpan").css("display", "none");
            else
                $("#AddTitleNoticeSpan").css("display", "initial");
        }

        function uploadlogo() {
            $('#WebSiteLogoInput').click();
        }
        function checkformat(target) {
            var fileSize = 0;
            fileSize = target.files[0].size;
            var size = fileSize / 1024;
            if (size > 5000) {
                alert("站标不能大于5M");
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
            document.getElementById("WebSiteLogoUplBtn").click();
        }
    </script>
</body>
</html>
