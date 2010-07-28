using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

public partial class Redirect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataAccess.DataAccess Access = new DataAccess.DataAccess();
        
        //Sub sub = new Sub();
       // DataTable subList = sub.GetSubList(Access);
        string strSql = "select ss.serverip,sid.startip,sid.endip,sid.subcode ";
        strSql += " from T_SubSection ss join T_SubIpDivision sid on ";
        strSql+= " ss.subCode = sid.subCode ";
        DataTable subList = Access.execSql(strSql);
        string clientIp;
        if(Request.ServerVariables["HTTP_VIA"] == null)
        {
            clientIp= Request.UserHostAddress;
        }
        else
        {
            clientIp= Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        }

        foreach (DataRow xx in subList.Rows)
        {
            //string ipArea = xx["IpArea"].ToString();
            //string[] ipAreas = new string[2];
            //ipAreas = ipArea.Split('|');
            //if (ipArea.Length < 2)
                //continue;
            if (Common.IpCheck.IsIn(clientIp, xx["startip"].ToString(), xx["endip"].ToString()))
            {
                string pagefname = Request.QueryString["Page"];
                Response.Redirect("http://" + xx["serverIp"].ToString() + "/"+pagefname+"?subCode=" + xx["subCode"].ToString());
                return;
            }
        }

    }
}
