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

public partial class admin_category : System.Web.UI.Page
{
    // 当前登录用户数据
    public DataRow LoginedUser;
    // 待修改分类数据
    public DataRow CheckedCategory;
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

        // 未传入cid
        if (!isNumber(Request["cid"]))
        {
            Response.Write("非法操作");
            Response.End();
        }

        // 查询是否存在cid
        int cid = Convert.ToInt32(Request["cid"]);
        DataTable dtusers = new DataTable();
        if (mainSql.SqlSelect(string.Format("SELECT * FROM [category] WHERE [cid] = '{0}'", cid), ref dtusers) <= 0)
        {
            mainSql.SqlClose();
            Response.Write("非法操作");
            Response.Redirect("index.aspx", true);
        }
        CheckedCategory = dtusers.Rows[0];
        mainSql.SqlClose();

        if (!IsPostBack)
            CategoryNameInput.Text = CheckedCategory["category_title"].ToString();
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

    private bool isNumber(string i)
    {
        try
        {
            Convert.ToInt32(i);
            return true;
        }
        catch
        {
            return false;
        }
    }

    protected void CategoryUpdateBtn_Click(object sender, EventArgs e)
    {
        Sql mainSql = new Sql();
        // 获取分类信息
        DataTable dt = new DataTable();
        if (mainSql.SqlSelect(string.Format("SELECT * FROM [category] WHERE [cid] = {0}", Convert.ToInt32(CheckedCategory["cid"])), ref dt) <= 0)
        {
            mainSql.SqlClose();
            Response.Write("<script>alert(\"分类不存在！\");</script>");
            return;
        }

        // 操作数据库 修改
        if (mainSql.SqlQuery(string.Format("UPDATE [category] SET [category_title] = '{0}' WHERE [cid] = {1}", CategoryNameInput.Text, CheckedCategory["cid"])) > 0)
        {
            mainSql.SqlClose();
            Response.Write("<script>alert(\"修改成功！\");self.location='index.aspx';</script>");
        }
        else
        {
            mainSql.SqlClose();
            Response.Write("<script>alert(\"修改失败！\");self.location='index.aspx';</script>");
        }
    }
}