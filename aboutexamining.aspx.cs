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
using System.IO;

public partial class aboutexamining : System.Web.UI.Page
{
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

        // 新闻分类
        DataTable dtcategory = new DataTable();
        if (mainSql.SqlSelect("SELECT * FROM [category]", ref dtcategory) > 0)
        {
            NewsCategoryList.DataSource = dtcategory;
            NewsCategoryList.DataBind();
        }

        // 获取加密Key
        //string AuthKey = ConfigurationManager.AppSettings["AuthKey"].ToString();

        //生成salt
        //Response.Write(Sql.GenerateRandomString(15));
    }
}