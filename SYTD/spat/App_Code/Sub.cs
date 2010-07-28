using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Net;

/// <summary>
///Sub 的摘要说明
/// </summary>
public class Sub
{
    public Sub()
    {
       
    }

    public string GetCurrentSub(DataAccess.DataAccess Access,string requestSub,string centerSubCode)
    {
        string currentSub = "";
        if (requestSub!="")
        {
            currentSub = requestSub;
        }
        else
        {
            if (HttpContext.Current.Session["subCode"] != null && HttpContext.Current.Session["subCode"].ToString() != "")
            {
                currentSub = HttpContext.Current.Session["subCode"].ToString();
            }
            else
            {
                //string test = HttpContext.Current.Request.UserHostAddress;
                string clientIp = GetClientIp();
                uint Ip = IpConvertInt(clientIp);
                string strSql = "select subCode from T_SubIpDivision ";
                strSql += " where startNumber<=" + Ip + " and endNumber>=" + Ip;
                DataTable dt = Access.execSql(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    currentSub = dt.Rows[0]["subCode"].ToString();
                }
                else
                {
                    currentSub = centerSubCode;
                }
            }
        }
        HttpContext.Current.Session["subCode"] = currentSub;
        HttpContext.Current.Session.Timeout = 180;
        return currentSub;
    }

    private uint IpConvertInt(string Ip)
    {
        IPAddress ip_addr = IPAddress.Parse(Ip);
        uint ip_num = 0;
        try
        {
            ip_num = (uint)IPAddress.NetworkToHostOrder((int)(ip_addr.Address));
        }
        catch (System.Exception e)
        {
        	
        } 
        return ip_num;
    }


    public DataTable GetSubList(DataAccess.DataAccess Access)
    {
        string strSql = "select subCode,subName,serverIp from T_SubSection order by isCenter desc,subCode Asc";
        return Access.execSql(strSql);
    }

    private string GetClientIp()
    {
        string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (null == result || result == String.Empty)
        {
            result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }

        if (null == result || result == String.Empty)
        {
            result = HttpContext.Current.Request.UserHostAddress;
        }
        return result;

    }
}
