using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Sql 的摘要说明
/// </summary>
public class Sql
{
    SqlConnection conn = new SqlConnection();
	public Sql()
	{
        conn.ConnectionString = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;
        if (conn.State == ConnectionState.Closed)
            conn.Open();
	}

    /// <summary>
    /// SQL查询数据
    /// </summary>
    /// <param name="cmdText"></param>
    /// <param name="dt"></param>
    /// <returns></returns>
    public int SqlSelect(string cmdText, ref DataTable dt)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = cmdText;
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        adapter.Fill(dt);
        int n = dt.Rows.Count;
        return n;
    }

    /// <summary>
    /// SQL执行语句
    /// </summary>
    /// <param name="cmdText"></param>
    /// <returns></returns>
    public int SqlQuery(string cmdText)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = cmdText;
        return cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// SQL插入数据
    /// </summary>
    /// <param name="cmdText"></param>
    /// <returns></returns>
    public int SqlInsert(string cmdText)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = cmdText;
        int id = Convert.ToInt32(cmd.ExecuteScalar().ToString());
        return id;
    }

    /// <summary>
    /// MD5加密
    /// </summary>
    /// <param name="strText"></param>
    /// <returns></returns>
    public static string MD5Encrypt(string strText)
    {
        string newStr = "";
        MD5 md = new MD5CryptoServiceProvider();
        byte[] result = md.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strText));
        for (int i = 0; i < result.Length; i++)
            newStr += string.Format("{0:x}", result[i]);
        return newStr;
    }

    /// <summary>
    /// 生成随机字符串
    /// </summary>
    /// <param name="Length"></param>
    /// <returns></returns>
    public static string GenerateRandomString(int Length)
    {
        char[] constant = {
            '0','1','2','3','4','5','6','7','8','9',
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
        };
        StringBuilder newRandom = new StringBuilder(62);
        Random rd = new Random();
        for (int i = 0; i < Length; i++)
            newRandom.Append(constant[rd.Next(62)]);
        return newRandom.ToString();
    }

    /// <summary>
    /// 关闭数据库
    /// </summary>
    public void SqlClose()
    {
        if (conn.State == ConnectionState.Open)
            conn.Close();
    }

    ~Sql()
    {
        //if (conn.State == ConnectionState.Open)
            //conn.Close();
    }
}