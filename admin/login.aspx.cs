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
using System.Collections;
using System.Text;

public partial class admin_login : System.Web.UI.Page
{
    public const int GMGroupId = 2; // 管理员用户组ID
    protected void Page_Load(object sender, EventArgs e)
    {
        // 是否已登录
        if (Request.Cookies["NewsCMSCookie"] != null)
        {
            Sql loginSql = new Sql();
            int uid = Convert.ToInt32(Request.Cookies["NewsCMSCookie"].Values["uid"]);
            string EnCryptPassword = Convert.ToString(Request.Cookies["NewsCMSCookie"].Values["pwd"]);
            DataTable dt = new DataTable();
            if (loginSql.SqlSelect(string.Format("SELECT * FROM [users] WHERE [uid] = {0} AND [password] = '{1}'", uid, EnCryptPassword), ref dt) > 0)
            {
                loginSql.SqlClose();
                Response.Redirect("index.aspx", true);
            }
            loginSql.SqlClose();
        }

        // 获取加密Key
        //string AuthKey = ConfigurationManager.AppSettings["AuthKey"].ToString();

        // 生成salt
        //Response.Write(Sql.GenerateRandomString(15));
    }

    protected void Login_Click(object sender, EventArgs e)
    {
        string Account = UsernameInput.Text;
        string Password = PasswordInput.Text;
        string vCode = Session["CheckCode"].ToString();
        bool Remember = RememberStatus.Checked;
        if (VerityCodeInput.Text.Trim().ToUpper() != vCode.ToUpper())    // 检测验证码
        {
            Response.Write("<script>alert('验证码错误！');</script>");
            PasswordInput.Text = null;
            VerityCodeInput.Text = null;
            PasswordInput.Focus();
            return;
        }

        Sql loginSql = new Sql();
        // 获取加密Key
        string AuthKey = ConfigurationManager.AppSettings["AuthKey"].ToString();
        // 获取加密Salt
        DataTable dtuser = new DataTable();
        if (loginSql.SqlSelect(string.Format("SELECT * FROM [users] WHERE [username] = '{0}'", Account), ref dtuser) <= 0) // 账号不存在
        {
            Response.Write("<script>alert('用户名或密码错误！');</script>");
            loginSql.SqlClose();
            UsernameInput.Text = null;
            PasswordInput.Text = null;
            VerityCodeInput.Text = null;
            UsernameInput.Focus();
            return;
        }
        // 验证管理员身份
        if (Convert.ToInt32(dtuser.Rows[0]["gid"]) != GMGroupId)
        {
            Response.Write("<script>alert('您不是管理员！');</script>");
            loginSql.SqlClose();
            Response.Redirect("../index.aspx", true);
        }
        // 获取salt
        string AuthSalt = dtuser.Rows[0]["salt"].ToString();
        // 生成密文
        string EnCryptPassword = Sql.MD5Encrypt(AuthKey + Password + AuthSalt);

        // 验证密码
        if (EnCryptPassword == dtuser.Rows[0]["password"].ToString())
        {
            // 验证成功
            if (Remember == true)   // 记住我 有效时间变为15天
            {
                TimeSpan ts = new TimeSpan(15, 0, 0, 0);
                setCookie(dtuser.Rows[0], EnCryptPassword, ts, 1);
            }
            else
            {
                TimeSpan ts = new TimeSpan(0, 15, 0);   // Cookie有效时间 默认15分钟
                setCookie(dtuser.Rows[0], EnCryptPassword, ts, 0);
            }
            loginSql.SqlClose();
            Response.Redirect("index.aspx", true);
        }
        else
        {
            // 密码错误
            Response.Write("<script>alert('用户名或密码错误！');</script>");
            loginSql.SqlClose();
            UsernameInput.Text = null;
            PasswordInput.Text = null;
            VerityCodeInput.Text = null;
            UsernameInput.Focus();
            return;
        }
    }
    /// <summary>
    /// Cookie写入
    /// </summary>
    /// <param name="UserRow">用户数据</param>
    /// <param name="EnCryptPassword">用户密码密文</param>
    /// <param name="ts">Cookie有效时间</param>
    private void setCookie(DataRow UserRow, string EnCryptPassword, TimeSpan ts, int LongTime)
    {
        // 登录成功 写入Cookie
        HttpCookie cookie = new HttpCookie("NewsCMSCookie"); // 定义Cookie对象
        DateTime date = DateTime.Now;   // 定义时间对象
        cookie.Expires = date.Add(ts);  // 添加作用时间
        cookie.Values.Add("uid", UserRow["uid"].ToString());
        cookie.Values.Add("pwd", EnCryptPassword);
        cookie.Values.Add("keep", LongTime.ToString());
        Response.AppendCookie(cookie);  // 写入Cookie
    }
}