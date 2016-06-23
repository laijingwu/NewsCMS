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
using System.IO;

public partial class search : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request["searchInput"]))
        {
            Response.Write("<script>alert(\"搜索关键词为空！\");self.location='index.aspx';</script>");
            Response.End();
        }

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

        string search = Request["searchInput"];

        NavPositon.Text = "搜索“<b>" + search + "</b>”";

        DataTable dt = new DataTable();
        if (mainSql.SqlSelect(string.Format("SELECT * FROM [posts] WHERE [post_title] LIKE '%{0}%'OR [post_content] LIKE '%{0}%' OR [tag] LIKE '%{0}%' ORDER BY [published] DESC", search), ref dt) > 0)
        {
            NewestList.DataSource = dt;
            NewestList.DataBind();
            totalItem.Text = dt.Rows.Count.ToString();
        }
        else
        {
            NoResult.Visible = true;
        }

        mainSql.SqlClose();
    }
}