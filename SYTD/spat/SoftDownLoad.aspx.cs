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

public partial class SoftDownLoad : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["url"] != null && Request.QueryString["Id"] != null)
        {
            DataAccess.DataAccess_PB Access = new DataAccess.DataAccess_PB();
            Access.execSqlNoQuery1("update BaseItem set BrowseCount=BrowseCount+1 where Id=" + Request.QueryString["Id"].ToString());
            Access.Dispose();
            Response.Redirect(Request.QueryString["url"].ToString());
        }
    }
}
