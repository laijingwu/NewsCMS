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

public partial class article : System.Web.UI.Page
{
    public DataRow Post;
    public DataRow Author;
    public string AuthorDisplayname;
    public DataRow PostCategory;
    public DataRow AuthorGroup;
    // 默认头像存储文件夹
    public const string USERLOGO_FLODER = "./upload/logo/";
    protected void Page_Load(object sender, EventArgs e)
    {
        // 显示网站logo
        if (File.Exists(Server.MapPath("images/team-logo.png")))
            SiteLogo.ImageUrl = "images/team-logo.png";
        else if (File.Exists(Server.MapPath("images/team-logo.jpg")))
            SiteLogo.ImageUrl = "images/team-logo.jpg";
        else if (File.Exists(Server.MapPath("images/team-logo.gif")))
            SiteLogo.ImageUrl = "images/team-logo.gif";
        else if (File.Exists(Server.MapPath("images/team-logo.bmp")))
            SiteLogo.ImageUrl = "images/team-logo.bmp";
        else
            SiteLogo.Visible = false;

        Sql mainSql = new Sql();

        // 新闻主体
        DataTable dtpost = new DataTable();
        if (string.IsNullOrEmpty(Request["pid"]) || mainSql.SqlSelect(string.Format("SELECT * FROM [posts] WHERE [pid] = {0}", Request["pid"]), ref dtpost) <= 0)
        {
            Response.Redirect("list.aspx", true);
        }
        Post = dtpost.Rows[0];
        // 新闻所属分类
        DataTable dtmcategory = new DataTable();
        mainSql.SqlSelect(string.Format("SELECT * FROM [category] WHERE [cid] = {0}", Convert.ToInt32(Post["cid"])), ref dtmcategory);
        PostCategory = dtmcategory.Rows[0];
        // 作者信息
        if (string.IsNullOrEmpty(Post["uid"].ToString()))
        {
            string strEmail = Post["email"].ToString();
            string[] EmailArray = strEmail.Split('@');
            //AuthorDisplayname = string.Format("{0}[{1}]", Post["nickname"].ToString(), EmailArray[0]);
            AuthorDisplayname = Post["nickname"].ToString();
        }
        else
        {
            DataTable dtauthor = new DataTable();
            mainSql.SqlSelect(string.Format("SELECT * FROM [users] WHERE [uid] = {0}", Convert.ToInt32(Post["uid"])), ref dtauthor);
            Author = dtauthor.Rows[0];

            // 作者用户组信息
            DataTable dtauthorgroup = new DataTable();
            mainSql.SqlSelect(string.Format("SELECT * FROM [groups] WHERE [gid] = {0}", Convert.ToInt32(Author["gid"])), ref dtauthorgroup);
            AuthorGroup = dtauthorgroup.Rows[0];
        }

        Page.Title = Post["post_title"].ToString() + " | 新闻中心";
        PostTitle.Text = NavTitle.Text = Post["post_title"].ToString();

        // 标签信息
        string PostTagAll = Post["tag"].ToString();
        ArrayList TagArrayList = new ArrayList();
        string[] sTagArray = PostTagAll.Split('|');
        foreach (string i in sTagArray)
            TagArrayList.Add(i);
        PostTagList.DataSource = TagArrayList;
        PostTagList.DataBind();

        // 评论列表
        DataTable dtcomments = new DataTable();
        mainSql.SqlSelect(string.Format("SELECT * FROM [comments] WHERE [pid] = {0} AND [last_commented_id] IS NULL", Convert.ToInt32(Post["pid"])), ref dtcomments);
        //CommentsList.DataSource = dtcomments;
        //CommentsList.DataBind();
        ParentCommentList.DataSource = dtcomments;
        ParentCommentList.DataBind();

        // 新闻分类
        DataTable dtcategory = new DataTable();
        if (mainSql.SqlSelect("SELECT * FROM [category]", ref dtcategory) > 0)
        {
            NewsCategoryList.DataSource = dtcategory;
            NewsCategoryList.DataBind();
        }

        // 更新阅读量
        if (!IsPostBack)
            mainSql.SqlQuery(string.Format("UPDATE [posts] SET [post_read] = {0} WHERE [pid] = {1}", Convert.ToInt32(Post["post_read"]) + 1, Convert.ToInt32(Post["pid"])));

        mainSql.SqlClose();
    }

    protected void PostTagList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

    }

    //protected void CommentsList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    //{
    //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
    //    {
    //        if (string.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "uid").ToString()))
    //        {
    //            string strEmail = DataBinder.Eval(e.Item.DataItem, "email").ToString();
    //            string[] EmailArray = strEmail.Split('@');
    //            ((Label)e.Item.FindControl("CommenterDisplayName")).Text = string.Format("{0}[{1}]", DataBinder.Eval(e.Item.DataItem, "nickname").ToString(), EmailArray[0]);
    //            ((HyperLink)e.Item.FindControl("CommenterLogoLink")).Enabled = false;
    //            ((HyperLink)e.Item.FindControl("CommenterLink")).Enabled = false;
    //            return;
    //        }
    //        Sql mainSql = new Sql();
    //        DataTable dt = new DataTable();
    //        string strSelect = string.Format("SELECT * FROM [users] WHERE [uid] = {0}", DataBinder.Eval(e.Item.DataItem, "uid"));
    //        mainSql.SqlSelect(strSelect, ref dt);
    //        mainSql.SqlClose();
    //        ((Label)e.Item.FindControl("CommenterDisplayName")).Text = string.IsNullOrEmpty(dt.Rows[0]["nickname"].ToString()) ? dt.Rows[0]["username"].ToString() : dt.Rows[0]["nickname"].ToString();
    //        ((HyperLink)e.Item.FindControl("CommenterLogoLink")).NavigateUrl += string.Format("?uid={0}", DataBinder.Eval(e.Item.DataItem, "uid"));
    //        ((HyperLink)e.Item.FindControl("CommenterLink")).NavigateUrl += string.Format("?uid={0}", DataBinder.Eval(e.Item.DataItem, "uid"));
    //    }
    //}

    protected void CommentSend_Click(object sender, EventArgs e)
    {
        Sql addSql = new Sql();
        DateTime NowTime = DateTime.Now;    // 现在时间
        string strCommentNickname = CommentNickname.Text;   // 评论人昵称
        string strCommentEmail = CommentEmail.Text; // 评论人邮箱
        string strCommentContent = CommentContent.Text; // 评论内容
        string strSqlInsert;
        
        // 是否为被评论的子回复
        if (!string.IsNullOrEmpty(LastCommentedId.Value))
            // 插入数据 评论存入数据库
            strSqlInsert = string.Format("INSERT INTO [comments]([pid],[nickname],[email],[comment_content],[commented],[last_commented_id]) VALUES({0},'{1}','{2}','{3}','{4}',{5});SELECT @@IDENTITY", Post["pid"], strCommentNickname, strCommentEmail, strCommentContent, NowTime, Convert.ToInt32(LastCommentedId.Value));
        else
            strSqlInsert = string.Format("INSERT INTO [comments]([pid],[nickname],[email],[comment_content],[commented]) VALUES({0},'{1}','{2}','{3}','{4}');SELECT @@IDENTITY", Post["pid"], strCommentNickname, strCommentEmail, strCommentContent, NowTime);
        int cid = addSql.SqlInsert(strSqlInsert);
        if (cid > 0)
        {
            // 更新评论数
            addSql.SqlQuery(string.Format("UPDATE [posts] SET [comments_count] = {0} WHERE [pid] = {1}", Convert.ToInt32(Post["comments_count"]) + 1, Post["pid"]));
            addSql.SqlClose();
            Response.Write("<script>alert(\"评论成功！\");self.location='article.aspx?pid=" + Post["pid"].ToString() + "';</script>");
        }
        else
        {
            addSql.SqlClose();
            Response.Write("<script>alert(\"评论失败！请与网站管理员联系。\");self.location='article.aspx?pid=" + Post["pid"].ToString() + "';</script>");
        }
    }

    protected void ParentCommentList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Sql mainSql = new Sql();
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "uid").ToString()))
            {
                string strEmail = DataBinder.Eval(e.Item.DataItem, "email").ToString();
                string[] EmailArray = strEmail.Split('@');
                ((Label)e.Item.FindControl("CommenterDisplayName")).Text = string.Format("{0}[{1}]", DataBinder.Eval(e.Item.DataItem, "nickname").ToString(), EmailArray[0]);
                ((HyperLink)e.Item.FindControl("CommenterLogoLink")).Enabled = false;
                ((HyperLink)e.Item.FindControl("CommenterLink")).Enabled = false;
            }
            else
            {
                mainSql.SqlSelect(string.Format("SELECT * FROM [users] WHERE [uid] = {0}", DataBinder.Eval(e.Item.DataItem, "uid")), ref dt);
                ((Label)e.Item.FindControl("CommenterDisplayName")).Text = string.IsNullOrEmpty(dt.Rows[0]["nickname"].ToString()) ? dt.Rows[0]["username"].ToString() : dt.Rows[0]["nickname"].ToString();
                ((HyperLink)e.Item.FindControl("CommenterLogoLink")).NavigateUrl += string.Format("?uid={0}", DataBinder.Eval(e.Item.DataItem, "uid"));
                ((HyperLink)e.Item.FindControl("CommenterLink")).NavigateUrl += string.Format("?uid={0}", DataBinder.Eval(e.Item.DataItem, "uid"));
                ((Image)e.Item.FindControl("CommenterLogo")).ImageUrl = string.Format("{0}{1}", USERLOGO_FLODER, dt.Rows[0]["logo_url"]);
            }
            
            Repeater rpson = (Repeater)e.Item.FindControl("SonCommentList");
            dt.Clear();
            string sql = string.Format("SELECT * FROM [comments] WHERE [last_commented_id] = {0}", DataBinder.Eval(e.Item.DataItem, "comment_id"));
            if (mainSql.SqlSelect(sql, ref dt) > 0)
            {
                rpson.DataSource = dt.DefaultView;
                rpson.DataBind();
            }
            mainSql.SqlClose();
        }
    }

    protected void SonCommentList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Sql mainSql = new Sql();
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "uid").ToString()))
            {
                string strEmail = DataBinder.Eval(e.Item.DataItem, "email").ToString();
                string[] EmailArray = strEmail.Split('@');
                ((Label)e.Item.FindControl("SonCommenterDisplayName")).Text = string.Format("{0}[{1}]", DataBinder.Eval(e.Item.DataItem, "nickname").ToString(), EmailArray[0]);
                ((HyperLink)e.Item.FindControl("SonCommenterLogoLink")).Enabled = false;
                ((HyperLink)e.Item.FindControl("SonCommenterLink")).Enabled = false;
            }
            else
            {
                mainSql.SqlSelect(string.Format("SELECT * FROM [users] WHERE [uid] = {0}", DataBinder.Eval(e.Item.DataItem, "uid")), ref dt);
                ((Label)e.Item.FindControl("SonCommenterDisplayName")).Text = string.IsNullOrEmpty(dt.Rows[0]["nickname"].ToString()) ? dt.Rows[0]["username"].ToString() : dt.Rows[0]["nickname"].ToString();
                ((HyperLink)e.Item.FindControl("SonCommenterLogoLink")).NavigateUrl += string.Format("?uid={0}", DataBinder.Eval(e.Item.DataItem, "uid"));
                ((HyperLink)e.Item.FindControl("SonCommenterLink")).NavigateUrl += string.Format("?uid={0}", DataBinder.Eval(e.Item.DataItem, "uid"));
                ((Image)e.Item.FindControl("SonCommenterLogo")).ImageUrl = string.Format("{0}{1}", USERLOGO_FLODER, dt.Rows[0]["logo_url"]);
            }
            mainSql.SqlClose();
        }
    }
}