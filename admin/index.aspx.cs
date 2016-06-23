using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;

public partial class admin_index : System.Web.UI.Page
{
    // 用于新闻列表分页
    public int nTotalItem = 0;
    public int nTotalPage = 0;
    // 用于用户列表分页
    public int nTotalUserItem = 0;
    public int nTotalUserPage = 0;
    // 用于分类列表分页
    public int nTotalCategoryItem = 0;
    public int nTotalCategoryPage = 0;
    // 当前登录用户数据
    public DataRow LoginedUser;
    // 默认头像存储文件夹
    public const string USERLOGO_FLODER = "../upload/logo/";
    // 默认图片存储文件夹
    public const string UPLOAD_FLODER = "../upload/";
    // 管理员用户组ID
    public const int GMGroupId = 2;

    protected void Page_Load(object sender, EventArgs e)
    {
        // 显示网站logo
        if (File.Exists(Server.MapPath("../images/team-logo.png")))
            SiteLogo.ImageUrl = "../images/team-logo.png";
        else if (File.Exists(Server.MapPath("../images/team-logo.jpg")))
            SiteLogo.ImageUrl = "../images/team-logo.jpg";
        else if (File.Exists(Server.MapPath("../images/team-logo.gif")))
            SiteLogo.ImageUrl = "../images/team-logo.gif";
        else if (File.Exists(Server.MapPath("../images/team-logo.bmp")))
            SiteLogo.ImageUrl = "../images/team-logo.bmp";
        else
            SiteLogo.Visible = false;

        Sql mainSql = new Sql();

        // 是否已登录
        if (Request.Cookies["NewsCMSCookie"] == null)
        {
            mainSql.SqlClose();
            Response.Redirect("login.aspx", true);
        }
        
        // 检查登录状态
        DataTable dtuser = new DataTable();
        if (mainSql.SqlSelect(string.Format("SELECT * FROM [users] WHERE [uid] = '{0}' AND [password] = '{1}' AND [gid] = {2}", Convert.ToInt32(Request.Cookies["NewsCMSCookie"].Values["uid"]), Request.Cookies["NewsCMSCookie"].Values["pwd"].ToString(), GMGroupId), ref dtuser) <= 0)
        {
            mainSql.SqlClose();
            Response.Redirect("login.aspx", true);
        }
        LoginedUser = dtuser.Rows[0];

        if (Request.Cookies["NewsCMSCookie"].Values["keep"] == "0")
        {
            //更新Cookie
            HttpCookie cookie = Request.Cookies["NewsCMSCookie"];
            cookie.Expires = DateTime.Now.AddMinutes(15);
            Response.SetCookie(cookie);
        }

        if (!IsPostBack)
        {
            // 新闻分类
            DataTable dtcategory = new DataTable();
            if (mainSql.SqlSelect("SELECT * FROM [category]", ref dtcategory) > 0)
            {
                CategoryPageList.DataTextField = "category_title";
                CategoryPageList.DataValueField = "cid";
                CategoryPageList.DataSource = dtcategory;
                CategoryPageList.DataBind();
                CategoryPageList.Items.Insert(0, "按分类查看");
                CategoryNewsList.DataTextField = "category_title";
                CategoryNewsList.DataValueField = "cid";
                CategoryNewsList.DataSource = dtcategory;
                CategoryNewsList.DataBind();
            }

            // 读取新闻
            controlPage();

            // 用户组
            DataTable dtusergroups = new DataTable();
            if (mainSql.SqlSelect("SELECT * FROM [groups]", ref dtusergroups) > 0)
            {
                UserGroupsList.DataTextField = "group_name";
                UserGroupsList.DataValueField = "gid";
                UserGroupsList.DataSource = dtusergroups;
                UserGroupsList.DataBind();
                UserGroupsList.Items.Insert(0, "按用户组查看");
            }

            // 读取用户
            controlUsersPage();

            // 读取分类
            controlCategoryPage();
        }

        mainSql.SqlClose();
    }

    public void controlPage(string strSelectAll = "SELECT * FROM [posts] ORDER BY [published] DESC")
    {
        Sql mainSql = new Sql();
        DataTable dts = new DataTable();
        if (mainSql.SqlSelect(strSelectAll, ref dts) > 0)
        {
            PagedDataSource page = new PagedDataSource();
            page.AllowPaging = true; //开启分页功能
            page.PageSize = 5;		//每页10条数据
            page.CurrentPageIndex = Convert.ToInt32(currentPage.Text) - 1;  //当前页的索引
            //绑定数据源
            page.DataSource = dts.DefaultView;
            NewsList.DataSource = page;


            nTotalItem = (nTotalItem == 0) ? dts.Rows.Count : nTotalItem;
            nTotalPage = (nTotalPage == 0) ? page.PageCount : nTotalPage;
            totalPage.Text = nTotalPage.ToString();

            lbtnpritPage.Enabled = true;
            lbtnnextPage.Enabled = true;
            lbtnfirstPage.Enabled = true;
            lbtnlastPage.Enabled = true;

            if (page.CurrentPageIndex < 1)
            {
                lbtnpritPage.Enabled = false;
                lbtnfirstPage.Enabled = false;
            }

            if (page.CurrentPageIndex == page.PageCount - 1)
            {
                lbtnnextPage.Enabled = false;
                lbtnlastPage.Enabled = false;
            }
            NewsList.DataBind();
        }
        else
        {
            NewsList.DataSource = new ArrayList();
            NewsList.DataBind();
            currentPage.Text = "0";
            totalPage.Text = "0";
            lbtnpritPage.Enabled = false;
            lbtnnextPage.Enabled = false;
            lbtnfirstPage.Enabled = false;
            lbtnlastPage.Enabled = false;
        }

        mainSql.SqlClose();
    }

    public void controlUsersPage(string strSelectAll = "SELECT * FROM [users] ORDER BY [uid]")
    {
        Sql mainSql = new Sql();
        DataTable dts = new DataTable();
        if (mainSql.SqlSelect(strSelectAll, ref dts) > 0)
        {
            PagedDataSource page = new PagedDataSource();
            page.AllowPaging = true; //开启分页功能
            page.PageSize = 5;		//每页5条数据
            page.CurrentPageIndex = Convert.ToInt32(currentUserPage.Text) - 1;  //当前页的索引
            //绑定数据源
            page.DataSource = dts.DefaultView;
            UsersList.DataSource = page;

            nTotalUserItem = (nTotalUserItem == 0) ? dts.Rows.Count : nTotalUserItem;
            nTotalUserPage = (nTotalUserPage == 0) ? page.PageCount : nTotalUserPage;
            totalUserPage.Text = nTotalUserPage.ToString();

            lbtnpritUserPage.Enabled = true;
            lbtnnextUserPage.Enabled = true;
            lbtnfirstUserPage.Enabled = true;
            lbtnlastUserPage.Enabled = true;

            if (page.CurrentPageIndex < 1)
            {
                lbtnpritUserPage.Enabled = false;
                lbtnfirstUserPage.Enabled = false;
            }

            if (page.CurrentPageIndex == page.PageCount - 1)
            {
                lbtnnextUserPage.Enabled = false;
                lbtnlastUserPage.Enabled = false;
            }
            UsersList.DataBind();
        }
        else
        {
            UsersList.DataSource = new ArrayList();
            UsersList.DataBind();
            currentUserPage.Text = "0";
            totalUserPage.Text = "0";
            lbtnpritUserPage.Enabled = false;
            lbtnnextUserPage.Enabled = false;
            lbtnfirstUserPage.Enabled = false;
            lbtnlastUserPage.Enabled = false;
        }

        mainSql.SqlClose();
    }

    public void controlCategoryPage(string strSelectAll = "SELECT * FROM [category] ORDER BY [cid]")
    {
        Sql mainSql = new Sql();
        DataTable dts = new DataTable();
        if (mainSql.SqlSelect(strSelectAll, ref dts) > 0)
        {
            PagedDataSource page = new PagedDataSource();
            page.AllowPaging = true; //开启分页功能
            page.PageSize = 5;		//每页10条数据
            page.CurrentPageIndex = Convert.ToInt32(currentCategoryPage.Text) - 1;  //当前页的索引
            //绑定数据源
            page.DataSource = dts.DefaultView;
            CategoryList.DataSource = page;

            nTotalCategoryItem = (nTotalCategoryItem == 0) ? dts.Rows.Count : nTotalCategoryItem;
            nTotalCategoryPage = (nTotalCategoryPage == 0) ? page.PageCount : nTotalCategoryPage;
            totalCategoryPage.Text = nTotalCategoryPage.ToString();

            lbtnpritCategoryPage.Enabled = true;
            lbtnnextCategoryPage.Enabled = true;
            lbtnfirstCategoryPage.Enabled = true;
            lbtnlastCategoryPage.Enabled = true;

            if (page.CurrentPageIndex < 1)
            {
                lbtnpritCategoryPage.Enabled = false;
                lbtnfirstCategoryPage.Enabled = false;
            }

            if (page.CurrentPageIndex == page.PageCount - 1)
            {
                lbtnnextCategoryPage.Enabled = false;
                lbtnlastCategoryPage.Enabled = false;
            }
            CategoryList.DataBind();
        }
        else
        {
            CategoryList.DataSource = new ArrayList();
            CategoryList.DataBind();
            currentCategoryPage.Text = "0";
            totalCategoryPage.Text = "0";
            lbtnpritCategoryPage.Enabled = false;
            lbtnnextCategoryPage.Enabled = false;
            lbtnfirstCategoryPage.Enabled = false;
            lbtnlastCategoryPage.Enabled = false;
        }

        mainSql.SqlClose();
    }

    protected void lbtnfirstPage_Click(object sender, EventArgs e)
    {
        TabIndex.Value = 1.ToString();
        currentPage.Text = "1";
        controlPage();
    }

    protected void lbtnpritPage_Click(object sender, EventArgs e)
    {
        TabIndex.Value = 1.ToString();
        currentPage.Text = Convert.ToString(Convert.ToInt32(currentPage.Text) - 1);
        controlPage();
    }

    protected void lbtnnextPage_Click(object sender, EventArgs e)
    {
        TabIndex.Value = 1.ToString();
        currentPage.Text = Convert.ToString(Convert.ToInt32(currentPage.Text) + 1);
        controlPage();
    }

    protected void lbtnlastPage_Click(object sender, EventArgs e)
    {
        TabIndex.Value = 1.ToString();
        currentPage.Text = totalPage.Text;
        controlPage();
    }

    protected void NewsList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Sql mainSql = new Sql();
            DataTable dt = new DataTable();
            mainSql.SqlSelect(string.Format("SELECT * FROM [category] WHERE [cid] = {0}", DataBinder.Eval(e.Item.DataItem, "cid")), ref dt);
            ((Label)e.Item.FindControl("CategoryTitle")).Text = dt.Rows[0]["category_title"].ToString();

            if (string.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "uid").ToString()))
            {
                string strEmail = DataBinder.Eval(e.Item.DataItem, "email").ToString();
                string[] EmailArray = strEmail.Split('@');
                //((Label)e.Item.FindControl("Author")).Text = string.Format("{0}[{1}]", DataBinder.Eval(e.Item.DataItem, "nickname").ToString(), EmailArray[0]);
                ((Label)e.Item.FindControl("Author")).Text = string.Format("{0}[{1}]", DataBinder.Eval(e.Item.DataItem, "nickname").ToString(), DataBinder.Eval(e.Item.DataItem, "email").ToString());
                ((HyperLink)e.Item.FindControl("AuthorLink")).Enabled = false;
                //((Label)e.Item.FindControl("Author")).Text = DataBinder.Eval(e.Item.DataItem, "nickname").ToString();
                //((HyperLink)e.Item.FindControl("AuthorLink")).NavigateUrl = DataBinder.Eval(e.Item.DataItem, "email").ToString();
                return;
            }

            dt.Clear();
            mainSql.SqlSelect(string.Format("SELECT * FROM [users] WHERE [uid] = {0}", DataBinder.Eval(e.Item.DataItem, "uid")), ref dt);
            mainSql.SqlClose();
            ((Label)e.Item.FindControl("Author")).Text = string.IsNullOrEmpty(dt.Rows[0]["nickname"].ToString()) ? dt.Rows[0]["username"].ToString() : dt.Rows[0]["nickname"].ToString();
            ((HyperLink)e.Item.FindControl("AuthorLink")).NavigateUrl += string.Format("?uid={0}", DataBinder.Eval(e.Item.DataItem, "uid"));
        }
    }

    protected void PostAddBtn_Click(object sender, EventArgs e)
    {
        // URL解码
        string DecodeEditor = HttpUtility.UrlDecode(EditorContent.Value);  // 取得编辑器中HTML代码
        string[] srcArray = GetHtmlImageUrlList(DecodeEditor);   // 取得img src内地址
        List<string> SrcList = new List<string>(srcArray);

        foreach (string s in SrcList)
        {
            if (s.Substring(0, 22).Contains("data:image/"))
            {
                int nIndex = s.IndexOf(";base64,");
                string ImgFormat = s.Remove(nIndex).Substring(11);  // 取得图片格式
                string base64Img = s.Substring(nIndex + 8); // 取得base64图片数据
                string baseImgName = Sql.GenerateRandomString(15);  // 随机文件名
                baseImgName = Base64StringToImage(baseImgName, ImgFormat, base64Img);   // 获得完整文件名
                DecodeEditor = DecodeEditor.Replace(s, UPLOAD_FLODER + baseImgName);    // 替换base64数据为图片文件路径
            }
        }

        // 获取表单信息
        string Title = AddNewTitle.Text.Trim();
        int uid = Convert.ToInt32(LoginedUser["uid"]);
        int cid = Convert.ToInt32(CategoryNewsList.SelectedValue);
        string PostTag = Regex.Replace(NewsTagInput.Text.Trim(), @"\s+", " ").Replace(" ", "|");
        DateTime NowTime = DateTime.Now;

        // 写入数据库
        Sql mainSql = new Sql();
        int pid = mainSql.SqlInsert(string.Format("INSERT INTO [posts]([uid],[cid],[post_title],[post_content],[published],[tag]) VALUES({0},{1},'{2}','{3}','{4}','{5}');SELECT @@IDENTITY", uid, cid, Title, HttpUtility.UrlEncode(DecodeEditor), NowTime, PostTag));

        if (pid > 0)
            Response.Write("<script>alert(\"发布成功！\");self.location='index.aspx';</script>");
        else
            Response.Write("<script>alert(\"发布失败！\");self.location='index.aspx';</script>");
    }

    /// <summary>
    /// 正则取出SRC中内容
    /// </summary>
    /// <param name="sHtmlText">待正则的HTML代码</param>
    /// <returns>SRC字符串数组</returns>
    public static string[] GetHtmlImageUrlList(string sHtmlText)
    {
        // 定义正则表达式用来匹配 img 标签   
        Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

        // 搜索匹配的字符串   
        MatchCollection matches = regImg.Matches(sHtmlText);
        int i = 0;
        string[] sUrlList = new string[matches.Count];

        // 取得匹配项列表   
        foreach (Match match in matches)
            sUrlList[i++] = match.Groups["imgUrl"].Value;
        return sUrlList;
    }

    /// <summary>
    /// Base64字符串转换成图片文件
    /// </summary>
    /// <param name="SavaImgName">保存图片名</param>
    /// <param name="ImgFormat">图片格式</param>
    /// <param name="Base64String">图片base64数据</param>
    /// <returns></returns>
    public string Base64StringToImage(string SavaImgName, string ImgFormat, string Base64String)
    {
        try
        {
            MemoryStream ms = new MemoryStream(Convert.FromBase64String(Base64String));
            Bitmap bmp = new Bitmap(ms);
            switch (ImgFormat)
            {
                case "jpeg":
                    SavaImgName += ".jpg";
                    bmp.Save(HttpContext.Current.Server.MapPath(UPLOAD_FLODER) + SavaImgName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case "bmp":
                    SavaImgName += ".bmp";
                    bmp.Save(HttpContext.Current.Server.MapPath(UPLOAD_FLODER) + SavaImgName, System.Drawing.Imaging.ImageFormat.Bmp);
                    break;
                case "gif":
                    SavaImgName += ".gif";
                    bmp.Save(HttpContext.Current.Server.MapPath(UPLOAD_FLODER) + SavaImgName, System.Drawing.Imaging.ImageFormat.Gif);
                    break;
                case "png":
                    SavaImgName += ".png";
                    bmp.Save(HttpContext.Current.Server.MapPath(UPLOAD_FLODER) + SavaImgName, System.Drawing.Imaging.ImageFormat.Png);
                    break;
                default:
                    ms.Close();
                    return null;
            }
            ms.Close();
            return SavaImgName;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    protected void CategoryPageList_SelectedIndexChanged(object sender, EventArgs e)
    {
        // 刷新新闻列表
        TabIndex.Value = 1.ToString();
        currentPage.Text = "1";
        nTotalPage = nTotalItem = 0;
        if (CategoryPageList.SelectedIndex != 0)    // 不为第一项
            controlPage(string.Format("SELECT * FROM [posts] WHERE [cid] = {0} ORDER BY [published] DESC", CategoryPageList.SelectedValue));
        else
            controlPage();
    }

    protected void UsersList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            if (!string.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "logo_url").ToString()))
                ((System.Web.UI.WebControls.Image)e.Item.FindControl("UserLogoImg")).ImageUrl = USERLOGO_FLODER + DataBinder.Eval(e.Item.DataItem, "logo_url").ToString();
            else
                ((System.Web.UI.WebControls.Image)e.Item.FindControl("UserLogoImg")).ImageUrl = "resources/images/userpic.gif";

            Sql mainSql = new Sql();
            DataTable dt = new DataTable();
            mainSql.SqlSelect(string.Format("SELECT * FROM [groups] WHERE [gid] = {0}", DataBinder.Eval(e.Item.DataItem, "gid")), ref dt);
            ((Label)e.Item.FindControl("UserGroupName")).Text = dt.Rows[0]["group_name"].ToString();
            mainSql.SqlClose();
        }
    }

    protected void lbtnfirstUserPage_Click(object sender, EventArgs e)
    {
        TabIndex.Value = 3.ToString();
        currentUserPage.Text = "1";
        controlUsersPage();
    }

    protected void lbtnpritUserPage_Click(object sender, EventArgs e)
    {
        TabIndex.Value = 3.ToString();
        currentUserPage.Text = Convert.ToString(Convert.ToInt32(currentUserPage.Text) - 1);
        controlUsersPage();
    }

    protected void lbtnnextUserPage_Click(object sender, EventArgs e)
    {
        TabIndex.Value = 3.ToString();
        currentUserPage.Text = Convert.ToString(Convert.ToInt32(currentUserPage.Text) + 1);
        controlUsersPage();
    }

    protected void lbtnlastUserPage_Click(object sender, EventArgs e)
    {
        TabIndex.Value = 3.ToString();
        currentUserPage.Text = totalUserPage.Text;
        controlUsersPage();
    }

    protected void UserGroupsList_SelectedIndexChanged(object sender, EventArgs e)
    {
        // 刷新用户列表
        TabIndex.Value = 3.ToString();
        currentUserPage.Text = "1";
        nTotalUserPage = nTotalUserItem = 0;
        if (UserGroupsList.SelectedIndex != 0)    // 不为第一项
            controlUsersPage(string.Format("SELECT * FROM [users] WHERE [gid] = {0} ORDER BY [uid]", UserGroupsList.SelectedValue));
        else
            controlUsersPage();
    }

    protected void CategoryList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Sql mainSql = new Sql();
            DataTable dt = new DataTable();
            int n = mainSql.SqlSelect(string.Format("SELECT * FROM [posts] WHERE [cid] = {0}", DataBinder.Eval(e.Item.DataItem, "cid")), ref dt);
            ((Label)e.Item.FindControl("CountNewsInCategory")).Text = n.ToString();
            mainSql.SqlClose();
        }
    }

    protected void lbtnfirstCategoryPage_Click(object sender, EventArgs e)
    {
        TabIndex.Value = 4.ToString();
        currentCategoryPage.Text = "1";
        controlCategoryPage();
    }

    protected void lbtnpritCategoryPage_Click(object sender, EventArgs e)
    {
        TabIndex.Value = 4.ToString();
        currentCategoryPage.Text = Convert.ToString(Convert.ToInt32(currentCategoryPage.Text) - 1);
        controlCategoryPage();
    }

    protected void lbtnnextCategoryPage_Click(object sender, EventArgs e)
    {
        TabIndex.Value = 4.ToString();
        currentCategoryPage.Text = Convert.ToString(Convert.ToInt32(currentCategoryPage.Text) + 1);
        controlCategoryPage();
    }

    protected void lbtnlastCategoryPage_Click(object sender, EventArgs e)
    {
        TabIndex.Value = 4.ToString();
        currentCategoryPage.Text = totalCategoryPage.Text;
        controlCategoryPage();
    }

    protected void LogoutBtn_Click(object sender, EventArgs e)
    {
        if (Request.Cookies["NewsCMSCookie"] != null)
        {
            HttpCookie cookie = Request.Cookies["NewsCMSCookie"];
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.SetCookie(cookie);
        }
        Response.Write("<script>alert(\"登出成功！\");self.location='login.aspx';</script>");
    }

    protected void UsersList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int uid = 0;
        switch (e.CommandName)
        {
            case "delete":
                uid = Convert.ToInt32(e.CommandArgument);
                deleteUser(uid);
                break;
        }
    }

    private void deleteUser(int uid)
    {
        Sql mainSql = new Sql();
        //操作数据库 删除账户
        if (mainSql.SqlQuery(string.Format("DELETE FROM [users] WHERE [uid] = {0}", uid)) > 0)
        {
            // 删除文章
            mainSql.SqlQuery(string.Format("DELETE FROM [posts] WHERE [uid] = {0}", uid));
            DataTable dt = new DataTable();
            if (mainSql.SqlSelect(string.Format("SELECT * FROM [comments] WHERE [uid] = '{0}'", uid), ref dt) > 0)
            {
                foreach (DataRow i in dt.Rows)
                {
                    // 删除子评论
                    mainSql.SqlQuery(string.Format("DELETE FROM [comments] WHERE [last_commented_id] = {0}", i["comment_id"]));
                }
            }
            // 删除评论
            mainSql.SqlQuery(string.Format("DELETE FROM [comments] WHERE [uid] = {0}", uid));
            mainSql.SqlClose();
            Response.Write("<script>alert(\"删除成功！\");self.location='index.aspx';</script>");
        }
        else
        {
            mainSql.SqlClose();
            Response.Write("<script>alert(\"删除失败！\");self.location='index.aspx';</script>");
        }
    }

    protected void NewsList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int pid = 0;
        switch (e.CommandName)
        {
            case "delete":
                pid = Convert.ToInt32(e.CommandArgument);
                deletePost(pid);
                break;
        }
    }

    protected void CategoryList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int cid = 0;
        switch (e.CommandName)
        {
            case "delete":
                cid = Convert.ToInt32(e.CommandArgument);
                deleteCategory(cid);
                break;
        }
    }

    private void deletePost(int pid)
    {
        Sql mainSql = new Sql();
        //操作数据库 删除文章
        if (mainSql.SqlQuery(string.Format("DELETE FROM [posts] WHERE [pid] = {0}", pid)) > 0)
        {
            // 删除评论
            mainSql.SqlQuery(string.Format("DELETE FROM [comments] WHERE [pid] = {0}", pid));
            mainSql.SqlClose();
            Response.Write("<script>alert(\"删除成功！\");self.location='index.aspx';</script>");
        }
        else
        {
            mainSql.SqlClose();
            Response.Write("<script>alert(\"删除失败！\");self.location='index.aspx';</script>");
        }
    }

    private void deleteCategory(int cid)
    {
        Sql mainSql = new Sql();
        //操作数据库 删除分类
        if (mainSql.SqlQuery(string.Format("DELETE FROM [category] WHERE [cid] = {0}", cid)) > 0)
        {
            DataTable dt = new DataTable();
            if (mainSql.SqlSelect(string.Format("SELECT * FROM [posts] WHERE [cid] = '{0}'", cid), ref dt) > 0)
            {
                foreach (DataRow i in dt.Rows)
                {
                    // 删除评论
                    mainSql.SqlQuery(string.Format("DELETE FROM [comments] WHERE [comment_id] = {0}", i["pid"]));
                }
            }

            // 删除文章
            mainSql.SqlQuery(string.Format("DELETE FROM [posts] WHERE [cid] = {0}", cid));
            mainSql.SqlClose();
            Response.Write("<script>alert(\"删除成功！\");self.location='index.aspx';</script>");
        }
        else
        {
            mainSql.SqlClose();
            Response.Write("<script>alert(\"删除失败！\");self.location='index.aspx';</script>");
        }
    }

    protected void addCategoryBtn_Click(object sender, EventArgs e)
    {
        // 判断是否为空
        if (string.IsNullOrEmpty(addCategoryTitle.Text))
        {
            Response.Write("<script>alert(\"分类名不能为空！\");</script>");
            return;
        }

        Sql mainSql = new Sql();
        DataTable dtcategory = new DataTable();
        if (mainSql.SqlSelect(string.Format("SELECT * FROM [category] WHERE [category_title] = '{0}'", addCategoryTitle.Text), ref dtcategory) > 0)
        {
            mainSql.SqlClose();
            Response.Write("<script>alert(\"分类名已存在！\");</script>");
            return;
        }

        int cid = mainSql.SqlInsert(string.Format("INSERT INTO [category]([category_title]) VALUES('{0}');SELECT @@IDENTITY", addCategoryTitle.Text));

        if (cid > 0)
            Response.Write("<script>alert(\"分类创建成功！\");self.location='index.aspx';</script>");
        else
            Response.Write("<script>alert(\"分类创建失败！\");self.location='index.aspx';</script>");
    }

    protected void WebSiteLogoUplBtn_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(WebSiteLogoInput.PostedFile.FileName))
        {
            if (File.Exists(Server.MapPath("../images/team-logo.png")))
                File.Delete(Server.MapPath("../images/team-logo.png"));
            if (File.Exists(Server.MapPath("../images/team-logo.jpg")))
                File.Delete(Server.MapPath("../images/team-logo.jpg"));
            if (File.Exists(Server.MapPath("../images/team-logo.gif")))
                File.Delete(Server.MapPath("../images/team-logo.gif"));
            if (File.Exists(Server.MapPath("../images/team-logo.bmp")))
                File.Delete(Server.MapPath("../images/team-logo.bmp"));

            string ImgFormat = WebSiteLogoInput.PostedFile.FileName.Substring(WebSiteLogoInput.PostedFile.FileName.LastIndexOf("."));
            string LogoServerPath = Server.MapPath("../images/team-logo") + ImgFormat;
            WebSiteLogoInput.PostedFile.SaveAs(LogoServerPath);
            Response.Write("<script>alert(\"上传成功！\");self.location='index.aspx';</script>");
        }
    }
}