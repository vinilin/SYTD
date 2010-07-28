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

public partial class header : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

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
}
