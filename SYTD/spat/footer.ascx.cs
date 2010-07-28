using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class footer : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        wirteLog();
    }

    public void Bind(DataTable subList)
    {
        lbSubList.Text = "";
        if (subList != null && subList.Rows.Count > 0)
        {
            for (int i = 0; i < subList.Rows.Count; i++)
            {
                lbSubList.Text += "<a href='default.aspx?subCode=" + subList.Rows[i]["subCode"].ToString() + "'>" + subList.Rows[i]["subName"].ToString() + "</a>";
                if (i < subList.Rows.Count - 1)
                {
                    lbSubList.Text += "&nbsp;&nbsp;|&nbsp;&nbsp;";
                }
            }
        }
    }

    private void wirteLog()
    {
        string urlReferrer = "";
        if (Page.Request.UrlReferrer != null)
        {
            urlReferrer = Page.Request.UrlReferrer.ToString();
        }
        string urlCrrent = "http://" + HttpContext.Current.Request.Url.Host;
        DataAccess.DataAccess Access = new DataAccess.DataAccess();
        DataTable tempDt;
        if (!urlReferrer.StartsWith(urlCrrent))
        {
            string strSql = "select * from T_BrowseLog where RQ='" + System.DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "'";
            tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                strSql = "update T_BrowseLog set browseCount=browseCount+1 where rq='" + System.DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "'";
            }
            else
            {
                strSql = "insert into T_BrowseLog(rq,browseCount) values('" + System.DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "',1)";
            }
            Access.execSqlNoQuery1(strSql);
        }
        string browseCount = "";
        string currentCount = "";
        tempDt = Access.execSql("select sum(browseCount) from T_BrowseLog");
        if (tempDt != null && tempDt.Rows.Count > 0)
        {
            browseCount = tempDt.Rows[0][0].ToString();
        }
        tempDt = Access.execSql("select browseCount from T_BrowseLog where rq='" + System.DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "'");
        if (tempDt != null && tempDt.Rows.Count > 0)
        {
            currentCount = tempDt.Rows[0][0].ToString();
        }
        Access.Dispose();
        lbCurrentCount.Text = currentCount;
        lbBrowseCount.Text = browseCount;

    }
}
