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

public partial class users : System.Web.UI.Page
{
    // 用户信息
    public DataRow User;
    // 用户组信息
    public DataRow UserGroup;
    // 默认头像存储文件夹
    public const string USERLOGO_FLODER = "./upload/logo/";
    public const int GMGroupId = 2; // 管理员用户组ID

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

        // 用户数据
        DataTable dtuser = new DataTable();
        if (string.IsNullOrEmpty(Request["uid"]) || mainSql.SqlSelect(string.Format("SELECT * FROM [users] WHERE [uid] = {0}", Request["uid"]), ref dtuser) <= 0)
        {
            Response.Redirect("index.aspx", true);
        }
        User = dtuser.Rows[0];

        // 用户用户组信息
        DataTable dtusergroup = new DataTable();
        mainSql.SqlSelect(string.Format("SELECT * FROM [groups] WHERE [gid] = {0}", Convert.ToInt32(User["gid"])), ref dtusergroup);
        UserGroup = dtusergroup.Rows[0];

        mainSql.SqlClose();

        if (!string.IsNullOrEmpty(User["logo_url"].ToString()))
            UserLogoImg.ImageUrl = USERLOGO_FLODER + User["logo_url"].ToString();

        UserEmail.Text = User["email"].ToString();
        UserNickname.Text = User["nickname"].ToString();
    }
}