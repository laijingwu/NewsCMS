<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <title>News CMS</title>
    <link rel="stylesheet" href="styles/index.css" />
    <script src="scripts/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        function search() {
            var search = $("#searchInput").val();
            if (search == "")
            {
                alert("搜索内容不能为空！");
                return false;
            }
        }
        $(document).ready(function () {
            var currentIndex = 0;
            var DEMO; //函数对象
            var currentID = 0; //取得鼠标下方的对象ID
            var pictureID = 0; //索引ID
            $("#ifocus_piclist li").eq(0).show(); //默认
            autoScroll();
            $("#ifocus_btn li").hover(function () {
                StopScrolll();
                $("#ifocus_btn li").removeClass("current")//所有的li去掉当前的样式加上正常的样式
                $(this).addClass("current"); //而本身则加上当前的样式去掉正常的样式
                currentID = $(this).attr("id"); //取当前元素的ID
                pictureID = currentID.substring(1); //取第一个字符后
                $("#ifocus_piclist li").eq(pictureID).fadeIn("slow"); //本身显示
                $("#ifocus_piclist li").not($("#ifocus_piclist li")[pictureID]).hide(); //除了自身别的全部隐藏
                $("#ifocus_tx li").hide();
                $("#ifocus_tx li").eq(pictureID).show();
            }, function () {
                //当鼠标离开对象的时候获得当前的对象的ID以便能在启动自动时与其同步
                currentID = $(this).attr("id"); //取当前元素的ID
                pictureID = currentID.substring(1); //取第一个字符后
                currentIndex = pictureID;
                //autoScroll();
            });
            //自动滚动
            function autoScroll() {
                $("#ifocus_btn li:last").removeClass("current");
                $("#ifocus_tx li:last").hide();
                $("#ifocus_btn li").eq(currentIndex).addClass("current");
                $("#ifocus_btn li").eq(currentIndex - 1).removeClass("current");
                $("#ifocus_tx li").eq(currentIndex).show();
                $("#ifocus_tx li").eq(currentIndex - 1).hide();
                $("#ifocus_piclist li").eq(currentIndex).fadeIn("slow");
                $("#ifocus_piclist li").eq(currentIndex - 1).hide();
                currentIndex++; currentIndex = currentIndex >= 12 ? 0 : currentIndex;
                DEMO = setTimeout(autoScroll, 5000);
            }
            function StopScrolll()//当鼠标移动到对象上面的时候停止自动滚动
            {
                clearTimeout(DEMO);
            }
        });
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
                <li><a class="active" href="index.aspx">首页</a></li>
                <li><a href="list.aspx">新闻中心</a></li>
                <li><a style="color: red;" href="share.aspx">马上爆料</a></li>
                <li><a href="aboutexamining.aspx">关于考核</a></li>
                <%--<li><a href="login.aspx">登录</a></li>
                <li><a href="register.aspx">注册</a></li>--%>
                <li><a href="admin/login.aspx">后台登录</a></li>
            </ul>
        </div>
        <div id="container">
            <!-- FLASH切换图-->
            <div id="ifocus">
                <div id="ifocus_pic">
                    <div id="ifocus_piclist" style="left: 0; top: 0;">
                        <ul>
                            <asp:Repeater runat="server" ID="BannerImgRepeater">
                                <ItemTemplate>
                                    <li><img src="<%=BANNER_URL %><%#GetDataItem() %>" /></li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                    <div id="ifocus_opdiv"></div>
                    <div id="ifocus_tx">
                        <ul>
                            <li class="current"><%=BannerDesList[0].ToString() %></li>
                            <asp:Repeater runat="server" ID="BannerDesRepeater">
                                <ItemTemplate>
                                    <li class="normal"><%#GetDataItem() %></li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
                <div id="ifocus_btn">
                    <ul>
                        <li class="current" id="p0">01</li>
                        <li id="p1">02</li>
                        <li id="p2">03</li>
                        <li id="p3">04</li>
                        <li id="p4">05</li>
                        <li id="p5">06</li>
                        <li id="p6">07</li>
                        <li id="p7">08</li>
                        <li id="p8">09</li>
                        <li id="p9">10</li>
                        <li id="p10">11</li>
                        <li id="p11">12</li>
                    </ul>
                </div>
            </div>
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
                        <p class="blockTitle2">新闻 <i>News</i></p>
                        <div class="clear"></div>
                        <div class="ifm_title">
                        	<span><a href="list.aspx"><i>更多>></i></a></span>
                            <img class="icon" src="images/icon1.jpg" />最新新闻
                        </div>
                        <div class="ifm_ul">
                            <ul>
                                <asp:Repeater ID="NewestList" runat="server">
                                    <ItemTemplate>
                                        <li><a href="article.aspx?pid=<%#Eval("pid") %>"><%#Eval("post_title") %></a><span><%#Eval("published","{0:yyyy-MM-dd HH:mm:ss}") %></span></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>
                        <div class="clear"></div>
                        <asp:Repeater ID="NewsCategoryRepeater" runat="server" OnItemDataBound="NewsCategoryRepeater_ItemDataBound">
                              <ItemTemplate>
                                 <div class="ifm_title">
                        	            <span><a href="list.aspx?cid=<%#Eval("cid") %>"><i>更多>></i></a></span>
                                        <img class="icon" src="images/icon-2.png" /><%#Eval("category_title") %>
                                    </div>
                                    <div class="ifm_ul">
                                        <ul>       
                                          <asp:Repeater ID="NewsRepeater" runat="server" OnItemDataBound="NewsRepeater_ItemDataBound">
                                                <ItemTemplate>
                                                     <li><a href="article.aspx?pid=<%#Eval("pid") %>"><%#Eval("post_title") %></a><span><%#Eval("published","{0:yyyy-MM-dd HH:mm:ss}") %></span></li>       
                                                </ItemTemplate>
                                          </asp:Repeater>
                                        </ul>
                                    </div>
                                    <div class="clear"></div>
                              </ItemTemplate>
                        </asp:Repeater>
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
