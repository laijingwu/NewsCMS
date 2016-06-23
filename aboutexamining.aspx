<%@ Page Language="C#" AutoEventWireup="true" CodeFile="aboutexamining.aspx.cs" Inherits="aboutexamining" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <title>关于考核 | News CMS</title>
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
                <li><a href="list.aspx">新闻中心</a></li>
                <li><a style="color: red;" href="share.aspx">马上爆料</a></li>
                <li><a class="active" href="aboutexamining.aspx">关于考核</a></li>
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
                        <p class="blockTitle2">当前位置 >> <a href="aboutexamining.aspx"><b>关于考核</b></a></p>
                        <div class="clear"></div>
                        <div class="article">
                            <h2>我的编程之路</h2>
                            <div class="clear"></div>
                            <p><strong>最初的目标：C/C++ 桌面端游戏方向</strong></p>
                            <p style="text-indent:2em;">小学还懵懂无知的时候，像大多数人一样是沉浸在游戏中的，但因为游戏外挂的盛行，运营商的无能，饱含友情与欢乐的世界支离破碎。从此立下目标，走编程的道路。</p>
                            <p><strong>渐行渐远的目标</strong></p>
                            <p style="text-indent:2em;">小学学习Flash，初中学习PHP、视频特效<small>（韶大全媒体视觉部真的很水）</small>，高中学习C++。一路走来，都觉得目标好遥远，看不到边际。</p>
                            <p><strong>未来，任重而道远</strong></p>
                            <p style="text-indent:2em;">都快奔2的人了，每次想起都会觉得好失落，但是又激励自己比别人更努力，似乎又回到了“只要学不死，就往死里学”的高三。</p>
                        </div>
                        <div class="article">
                            <h2>那些考核的日子</h2>
                            <div class="clear"></div>
                            <p><strong>轻松中略带些危机</strong></p>
                            <p style="text-indent:2em;">因为过年等等种种原因，二月十三号才开始制作。面对师兄师姐们的要求，尽管那些功能以前的考核中都基本实现过，但要整合到一个完整的系统中还是有那么点麻烦和问题。幸运地是，这些问题都解决了。</p>
                            <p><strong>繁忙中学会抽时间</strong></p>
                            <p style="text-indent:2em;">假期有自己的学习计划，除了做网园的考核还准备初学JAVA和深入学习C++，加之家里有组织探亲，朋友结婚有拍摄和后期制作任务，总觉得时间不够用，但最终还是做完了。 : )</p>
                        </div>
                        <div class="article">
                            <h2>对于网园的建议</h2>
                            <div class="clear"></div>
                            <p><strong>欢乐 + 技术&nbsp;|&nbsp;不忘初心，永远走下去</strong></p>
                        </div>
                        <div class="article">
                            <h2>关于这个作品</h2>
                            <div class="clear"></div>
                            <p><strong>制作周期</strong></p>
                            <p style="text-indent:2em;">开发时间：2016/02/13 - 2016/02/22</p>
                            <p><strong>吐槽~！</strong></p>
                            <p style="text-indent:2em;">这个前端模板未免有些丑，有些javascript代码有bug。不过用来考核凑着用着了，我的前端也不行。=.=</p>
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
</body>
</html>
