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

public partial class list : System.Web.UI.Page
{
    public int nTotalItem = 0;
    public int nTotalPage = 0;
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

        if (!string.IsNullOrEmpty(Request["cid"]))
        {
            DataTable dts = new DataTable();
            if (mainSql.SqlSelect(string.Format("SELECT * FROM [category] WHERE [cid] = {0}", Request["cid"]), ref dts) > 0)
            {
                NavPositon.Text = string.Format("<b>{0}新闻</b>", dts.Rows[0]["category_title"]);
                this.Page.Title = string.Format("{0}新闻 | 新闻中心", dts.Rows[0]["category_title"]);
                NavPositon.NavigateUrl = string.Format("list.aspx?cid={0}", Request["cid"]);
                controlPage(string.Format("SELECT * FROM [posts] WHERE [cid] = {0} ORDER BY [published] DESC", Request["cid"]));
                return;
            }
        }
        NavPositon.Text = "<b>最新新闻</b>";
        this.Page.Title = "最新新闻 | 新闻中心";
        NavPositon.NavigateUrl = "list.aspx";

        if (!IsPostBack)
        {
            controlPage();
        }

        mainSql.SqlClose();
    }

    public void controlPage(string strSelectAll = "SELECT * FROM [posts] ORDER BY [published] DESC")
    {
        Sql mainSql = new Sql();
        DataTable dts = new DataTable();
        if (mainSql.SqlSelect(strSelectAll, ref dts) > 0)
        {
            PagedDataSource page = new PagedDataSource();
            page.AllowPaging = true; //开启分页功能
            page.PageSize = 5;		//每页5条数据
            page.CurrentPageIndex = Convert.ToInt32(currentPage.Text) - 1;  //当前页的索引
            //绑定数据源
            page.DataSource = dts.DefaultView;
            NewestList.DataSource = page;


            nTotalItem = (nTotalItem == 0) ? dts.Rows.Count : nTotalItem;
            nTotalPage = (nTotalPage == 0) ? page.PageCount : nTotalPage;
            totalPage.Text = nTotalPage.ToString();

            lbtnpritPage.Enabled = true;
            lbtnnextPage.Enabled = true;
            lbtnfirstPage.Enabled = true;
            lbtnlastPage.Enabled = true;

            if (page.CurrentPageIndex < 1)
            {
                lbtnpritPage.Enabled = false;
                lbtnfirstPage.Enabled = false;
            }

            if (page.CurrentPageIndex == page.PageCount - 1)
            {
                lbtnnextPage.Enabled = false;
                lbtnlastPage.Enabled = false;
            }
            NewestList.DataBind();
        }
        else
        {
            lbtnpritPage.Enabled = false;
            lbtnnextPage.Enabled = false;
            lbtnfirstPage.Enabled = false;
            lbtnlastPage.Enabled = false;
        }

        ArrayList PageListArray = new ArrayList();
        for (int i = 1; i <= nTotalPage; i++)
            PageListArray.Add("第" + i + "页");
        PageListBtn.DataSource = PageListArray;
        PageListBtn.DataBind();
        PageListBtn.SelectedIndex = Convert.ToInt32(currentPage.Text) - 1;

        mainSql.SqlClose();
    }

    protected void lbtnfirstPage_Click(object sender, EventArgs e)
    {
        currentPage.Text = "1";
        controlPage();
    }

    protected void lbtnpritPage_Click(object sender, EventArgs e)
    {
        currentPage.Text = Convert.ToString(Convert.ToInt32(currentPage.Text) - 1);
        controlPage();
    }

    protected void lbtnnextPage_Click(object sender, EventArgs e)
    {
        currentPage.Text = Convert.ToString(Convert.ToInt32(currentPage.Text) + 1);
        controlPage();
    }

    protected void lbtnlastPage_Click(object sender, EventArgs e)
    {
        currentPage.Text = totalPage.Text;
        controlPage();
    }

    protected void PageListBtn_SelectedIndexChanged(object sender, EventArgs e)
    {
        currentPage.Text = Convert.ToString(PageListBtn.SelectedIndex + 1);
        controlPage();
    }
}