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

public partial class admin_article : System.Web.UI.Page
{
    // 当前登录用户数据
    public DataRow LoginedUser;
    // 待修改新闻数据
    public DataRow CheckedPost;
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

        // 未传入pid
        if (!isNumber(Request["pid"]))
        {
            Response.Write("非法操作");
            Response.End();
        }

        // 查询是否存在pid
        int pid = Convert.ToInt32(Request["pid"]);
        DataTable dt = new DataTable();
        if (mainSql.SqlSelect(string.Format("SELECT * FROM [posts] WHERE [pid] = '{0}'", pid), ref dt) <= 0)
        {
            mainSql.SqlClose();
            Response.Write("非法操作");
            Response.Redirect("index.aspx", true);
        }
        CheckedPost = dt.Rows[0];

        if (!IsPostBack)
        {
            PostTitleInput.Text = CheckedPost["post_title"].ToString();
            EditorContentBefor.Value = HttpUtility.UrlDecode(CheckedPost["post_content"].ToString());
            PostTagInput.Text = CheckedPost["tag"].ToString().Replace('|', ' ');
            // 新闻分类
            DataTable dtcategory = new DataTable();
            if (mainSql.SqlSelect("SELECT * FROM [category]", ref dtcategory) > 0)
            {
                CategoryList.DataTextField = "category_title";
                CategoryList.DataValueField = "cid";
                CategoryList.DataSource = dtcategory;
                CategoryList.DataBind();
                CategoryList.SelectedValue = CheckedPost["cid"].ToString();
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
        }
        catch
        {
            return false;
        }
    }

    protected void PostUpdateBtn_Click(object sender, EventArgs e)
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

        Sql mainSql = new Sql();
        // 获取表单信息
        string Title = PostTitleInput.Text.Trim();
        int cid = Convert.ToInt32(CategoryList.SelectedValue);
        string PostTag = Regex.Replace(PostTagInput.Text.Trim(), @"\s+", " ").Replace(" ", "|");

        // 操作数据库 修改
        if (mainSql.SqlQuery(string.Format("UPDATE [posts] SET [cid] = '{0}',[post_title] = '{1}',[post_content] = '{2}',[tag] = '{3}' WHERE [pid] = {4}", cid, Title, DecodeEditor, PostTag, CheckedPost["pid"])) > 0)
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
}