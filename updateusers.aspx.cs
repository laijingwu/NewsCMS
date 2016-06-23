using System;
using System.Collections.Generic;
using System.IO;
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

public partial class updateusers : System.Web.UI.Page
{
    public DataRow User;
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

        if (!IsPostBack)
        {
            UserEmailInput.Text = User["email"].ToString();
            UserNicknameInput.Text = User["nickname"].ToString();
        }
    }

    protected void UploadUser_Click(object sender, EventArgs e)
    {
        Sql mainSql = new Sql();
        int uid = Convert.ToInt32(Request["uid"]);
        string strSqlUpdate = string.Format("UPDATE [users] SET [nickname] = '{0}',[email] = '{1}'", UserNicknameInput.Text, UserEmailInput.Text);

        // 旧密码为空
        if (string.IsNullOrEmpty(UserOldPwdInput.Text))
        {
            Response.Write("<script>alert(\"请输入旧密码！\");</script>");
            return;
        }

        // 获取加密Key
        string AuthKey = ConfigurationManager.AppSettings["AuthKey"].ToString();
        // 判断旧密码是否正确
        DataTable dtuser = new DataTable();
        mainSql.SqlSelect(string.Format("SELECT * FROM [users] WHERE [uid] = {0}", uid), ref dtuser);
        string salt = dtuser.Rows[0]["salt"].ToString();
        if (Sql.MD5Encrypt(AuthKey + UserOldPwdInput.Text + salt) != dtuser.Rows[0]["password"].ToString())
        {
            Response.Write("<script>alert(\"旧密码输入错误！\");</script>");
            UserOldPwdInput.Text = UserNewPwdInput.Text = UserNewPwd1Input.Text = null;
            return;
        }

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
            if (UserOldPwdInput.Text == UserNewPwdInput.Text)
            {
                Response.Write("<script>alert(\"新密码不能与旧密码一致！\");</script>");
                return;
            }
            // 生成salt
            string AuthSalt = Sql.GenerateRandomString(15);
            // 生成密文
            string EnCryptPassword = Sql.MD5Encrypt(AuthKey + UserNewPwdInput.Text + AuthSalt);
            strSqlUpdate += string.Format(",[salt] = '{0}',[password] = '{1}'", AuthSalt, EnCryptPassword);
        }

        // 是否需要更换头像
        if (!string.IsNullOrEmpty(UploadLogoImg.PostedFile.FileName))
        {
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