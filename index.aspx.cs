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
using System.IO;
using System.Collections;

public partial class index : System.Web.UI.Page
{
    // 网站Banner图片地址
    public const string BANNER_URL = "./images/banner/";
    public ArrayList BannerDesList;

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

        // 主页Banner
        string[] BannerURLArray = { "1.jpg", "2.jpg", "3.jpg", "4.jpg", "5.jpg", "6.jpg", "7.jpg", "8.jpg", "9.jpg", "10.jpg", "11.jpg", "12.png", "header1.jpg", "header2.jpg", "header3.jpg" };
        string[] BannerDesArray = { "未知的前方", "上帝的视角", "海平面上的阳光", "迷失在灯红酒绿中", "依海而居", "城市运输核心——地铁", "I'm waiting for you.", "熟悉的街道", "Welcome to Hong Kong!", "我想和你去走走", "BE A HERO.", "Technology stays true here.", "不一样的高海拔天池", "享受暖阳", "勇攀高峰" };
        int len = BannerURLArray.Length;
        Random rnd = new Random();
        for (int j = 0; j < len; j++)
        {
            int pos = rnd.Next(len);
            string temp = BannerURLArray[pos];
            string temp1 = BannerDesArray[pos];
            BannerURLArray[pos] = BannerURLArray[j];
            BannerURLArray[j] = temp;
            BannerDesArray[pos] = BannerDesArray[j];
            BannerDesArray[j] = temp1;
        }
        ArrayList BannerURLList = new ArrayList();
        ArrayList BannerDesListTemp = new ArrayList();
        for (int i = 0; i < 12; i++)
        {
            BannerURLList.Add(BannerURLArray[i]);
            BannerDesListTemp.Add(BannerDesArray[i]);
        }
        BannerDesList = new ArrayList(BannerDesListTemp);
        BannerDesListTemp.RemoveAt(0);
        BannerImgRepeater.DataSource = BannerURLList;
        BannerImgRepeater.DataBind();
        BannerDesRepeater.DataSource = BannerDesListTemp;
        BannerDesRepeater.DataBind();

        Sql mainSql = new Sql();
        
        // 新闻分类
        DataTable dtcategory = new DataTable();
        if (mainSql.SqlSelect("SELECT * FROM [category]", ref dtcategory) > 0)
        {
            NewsCategoryList.DataSource = dtcategory;
            NewsCategoryList.DataBind();
            NewsCategoryRepeater.DataSource = dtcategory.DefaultView;
            NewsCategoryRepeater.DataBind();
        }

        // 最新新闻
        DataTable dtnewest = new DataTable();
        if (mainSql.SqlSelect("SELECT TOP 5 * FROM [posts] ORDER BY [published] DESC", ref dtnewest) > 0)
        {
            NewestList.DataSource = dtnewest;
            NewestList.DataBind();
        }

        mainSql.SqlClose();
    }

    protected void NewsCategoryRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Sql mainSql = new Sql();
            DataTable dt = new DataTable();
            string strSelect = string.Format("SELECT TOP 5 * FROM [posts] WHERE [cid] = {0} ORDER BY [published] DESC", DataBinder.Eval(e.Item.DataItem, "cid"));
            if (mainSql.SqlSelect(strSelect, ref dt) > 0)
            {
                Repeater rpson = (Repeater)e.Item.FindControl("NewsRepeater");
                rpson.DataSource = dt.DefaultView;
                rpson.DataBind();
            }
            mainSql.SqlClose();
        }
    }

    protected void NewsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

    }
}