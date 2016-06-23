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

public partial class admin_users : System.Web.UI.Page
{
    // 当前登录用户数据
    public DataRow LoginedUser;
    // 待修改用户数据
    public DataRow CheckedUser;
    // 待修改用户组数据
    public DataRow CheckedUserGroup;
    // 默认头像存储文件夹
    public const string USERLOGO_FLODER = "../upload/logo/";
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

        // 未传入uid
        if (!isNumber(Request["uid"]))
        {
            Response.Write("非法操作");
            Response.End();
        }

        // 查询是否存在uid
        int uid = Convert.ToInt32(Request["uid"]);
        DataTable dtusers = new DataTable();
        if (mainSql.SqlSelect(string.Format("SELECT * FROM [users] WHERE [uid] = '{0}'", uid), ref dtusers) <= 0)
        {
            mainSql.SqlClose();
            Response.Write("非法操作");
            Response.Redirect("index.aspx", true);
        }
        CheckedUser = dtusers.Rows[0];

        // 用户组信息
        DataTable dtusergroup = new DataTable();
        mainSql.SqlSelect(string.Format("SELECT * FROM [groups] WHERE [gid] = {0}", Convert.ToInt32(CheckedUser["gid"])), ref dtusergroup);
        CheckedUserGroup = dtusergroup.Rows[0];

        if (!string.IsNullOrEmpty(CheckedUser["logo_url"].ToString()))
            UserLogoImg.ImageUrl = USERLOGO_FLODER + CheckedUser["logo_url"].ToString();

        if (!IsPostBack)
        {
            UsernameInput.Text = CheckedUser["username"].ToString();
            UserEmailInput.Text = CheckedUser["email"].ToString();
            UserNicknameInput.Text = CheckedUser["nickname"].ToString();
            // 用户组
            DataTable dtusergroups = new DataTable();
            if (mainSql.SqlSelect("SELECT * FROM [groups]", ref dtusergroups) > 0)
            {
                UserGroupList.DataTextField = "group_name";
                UserGroupList.DataValueField = "gid";
                UserGroupList.DataSource = dtusergroups;
                UserGroupList.DataBind();
                UserGroupList.SelectedValue = CheckedUser["gid"].ToString();
            }
        }

        mainSql.SqlClose();
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
        } catch {
            return false;
        }
    }

    protected void UserUpdateBtn_Click(object sender, EventArgs e)
    {
        Sql mainSql = new Sql();
        int uid = Convert.ToInt32(Request["uid"]);

        // 获取用户信息
        DataTable dtuser = new DataTable();
        if (mainSql.SqlSelect(string.Format("SELECT * FROM [users] WHERE [uid] = {0}", uid), ref dtuser) <= 0)
        {
            mainSql.SqlClose();
            Response.Write("<script>alert(\"用户不存在！\");</script>");
            return;
        }

        // 检测Username是否已存在
        DataTable dttmp = new DataTable();
        if (UsernameInput.Text != dtuser.Rows[0]["username"].ToString() && mainSql.SqlSelect(string.Format("SELECT * FROM [users] WHERE [username] = '{0}'", UsernameInput.Text), ref dttmp) > 0)
        {
            mainSql.SqlClose();
            Response.Write("<script>alert(\"目标用户名已被使用！\");</script>");
            return;
        }

        // 获取gid
        int gid = Convert.ToInt32(UserGroupList.SelectedValue);
        // 创建修改SQL语句
        string strSqlUpdate = string.Format("UPDATE [users] SET [gid] = '{0}',[nickname] = '{1}',[email] = '{2}'", gid, UserNicknameInput.Text, UserEmailInput.Text);

        // 是否需要修改密码
        if (!string.IsNullOrEmpty(UserNewPwdInput.Text) || !string.IsNullOrEmpty(UserNewPwd1Input.Text))
        {
            if (string.IsNullOrEmpty(UserNewPwdInput.Text) || string.IsNullOrEmpty(UserNewPwd1Input.Text))
            {
                Response.Write("<script>alert(\"如需修改密码请将新密码和确认密码填写完整！\");</script>");
                return;
            }
            if (UserNewPwdInput.Text != UserNewPwd1Input.Text)
            {
                Response.Write("<script>alert(\"新密码与确认密码不一致！\");</script>");
                return;
            }
            // 获取加密Key
            string AuthKey = ConfigurationManager.AppSettings["AuthKey"].ToString();
            // 生成salt
            string AuthSalt = Sql.GenerateRandomString(15);
            // 生成密文
            string EnCryptPassword = Sql.MD5Encrypt(AuthKey + UserNewPwdInput.Text + AuthSalt);
            strSqlUpdate += string.Format(",[salt] = '{0}',[password] = '{1}'", AuthSalt, EnCryptPassword);
        }

        // 是否需要更换头像
        if (!string.IsNullOrEmpty(UploadLogoImg.PostedFile.FileName))
        {
            File.Delete(Server.MapPath(USERLOGO_FLODER + dtuser.Rows[0]["logo_url"].ToString()));
            //string LogoName = UploadLogoImg.PostedFile.FileName;
            // 生成随机文件名
            string ImgFormat = UploadLogoImg.PostedFile.FileName.Substring(UploadLogoImg.PostedFile.FileName.LastIndexOf("."));
            string LogoName = Sql.GenerateRandomString(15) + ImgFormat;
            string LogoServerPath = Server.MapPath(USERLOGO_FLODER) + LogoName;
            UploadLogoImg.PostedFile.SaveAs(LogoServerPath);
            strSqlUpdate += string.Format(",[logo_url] = '{0}'", LogoName);
            //Response.Write(USERLOGO_FLODER + LogoName);
        }

        strSqlUpdate += string.Format(" WHERE [uid] = {0}", uid);

        // 操作数据库 修改
        if (mainSql.SqlQuery(strSqlUpdate) > 0)
        {
            mainSql.SqlClose();
            Response.Write("<script>alert(\"修改成功！\");self.location='users.aspx?uid=" + uid + "';</script>");
        }
        else
        {
            mainSql.SqlClose();
            Response.Write("<script>alert(\"修改失败！\");</script>");
        }
    }
}