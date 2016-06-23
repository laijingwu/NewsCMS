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
using System.Drawing.Imaging;

public partial class share : System.Web.UI.Page
{
    // 默认图片存储文件夹
    public const string UPLOAD_FLODER = "./upload/";

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

        if (!IsPostBack)
        {
            // 新闻分类
            DataTable dtcategory = new DataTable();
            int n = mainSql.SqlSelect("SELECT * FROM [category]", ref dtcategory);
            if (n > 0)
            {
                NewsCategoryList.DataSource = dtcategory;
                NewsCategoryList.DataBind();
                CategoryList.DataTextField = "category_title";
                CategoryList.DataValueField = "cid";
                CategoryList.DataSource = dtcategory;
                CategoryList.DataBind();
            }
        }
        

        mainSql.SqlClose();
    }

    protected void PostSend_Click(object sender, EventArgs e)
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
        string Title = PostTitleInput.Text.Trim();
        string Nickname = PostAuthorNickInput.Text.Trim();
        string Email = PostAuthorEmailInput.Text.Trim();
        int cid = Convert.ToInt32(CategoryList.SelectedValue);
        string PostTag = Regex.Replace(PostTagInput.Text.Trim(), @"\s+", " ").Replace(" ", "|");
        DateTime NowTime = DateTime.Now;

        // 写入数据库
        Sql mainSql = new Sql();
        string strSqlInsert = string.Format("INSERT INTO [posts]([cid],[email],[nickname],[post_title],[post_content],[published],[tag]) VALUES({0},'{1}','{2}','{3}','{4}','{5}','{6}');SELECT @@IDENTITY", cid, Email, Nickname, Title, HttpUtility.UrlEncode(DecodeEditor), NowTime, PostTag);
        int pid = mainSql.SqlInsert(strSqlInsert);

        if (pid > 0)
        {
            // 发布成功
            Response.Redirect(string.Format("article.aspx?pid={0}", pid), true);
        }
        else
        {
            // 发布失败
            Response.Write("<script>alert(\"分享失败！\");self.location='share.aspx';</script>");
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
                    bmp.Save(HttpContext.Current.Server.MapPath(UPLOAD_FLODER) + SavaImgName, ImageFormat.Jpeg);
                    break;
                case "bmp":
                    SavaImgName += ".bmp";
                    bmp.Save(HttpContext.Current.Server.MapPath(UPLOAD_FLODER) + SavaImgName, ImageFormat.Bmp);
                    break;
                case "gif":
                    SavaImgName += ".gif";
                    bmp.Save(HttpContext.Current.Server.MapPath(UPLOAD_FLODER) + SavaImgName, ImageFormat.Gif);
                    break;
                case "png":
                    SavaImgName += ".png";
                    bmp.Save(HttpContext.Current.Server.MapPath(UPLOAD_FLODER) + SavaImgName, ImageFormat.Png);
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