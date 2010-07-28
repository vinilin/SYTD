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

public partial class kqfw : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataAccess.DataAccess Access = new DataAccess.DataAccess();
        Sub sub = new Sub();
        DataTable subList = sub.GetSubList(Access);
        if (subList != null && subList.Rows.Count > 0)
        {
            string currentSubCode = "";
            string requestSubCode = "";
            if (Request.QueryString["subCode"] != null && Request.QueryString["subCode"].ToString() != "")
            {
                requestSubCode = Request.QueryString["subCode"].ToString();
                currentSubCode = requestSubCode;
            }
            else
            {
                currentSubCode = sub.GetCurrentSub(Access, requestSubCode, subList.Rows[0]["subCode"].ToString());
            }

            ucHeader.Bind(subList);
            ucFooter.Bind(subList);
            string kind1 = "", kind2 = "", param = "";
            if (Request.QueryString["kind1"] != null && Request.QueryString["kind1"].ToString() != "")
            {
                kind1 = Request.QueryString["kind1"].ToString();
            }
            if (Request.QueryString["kind2"] != null && Request.QueryString["kind2"].ToString() != "")
            {
                kind2 = Request.QueryString["kind2"].ToString();
            }
            if (Request.QueryString["param"] != null && Request.QueryString["param"].ToString() != "")
            {
                param = Request.QueryString["param"].ToString();
            }
            if (param == "") { Response.Redirect("default.aspx"); }
            bindPath(Access, kind1, kind2, param, currentSubCode);
            bindKind(Access, param, currentSubCode);
            //bindList(Access, kind1, kind2, param, currentSubCode);
        }
        else
        {
            lbMessage.Text = "<script language=\"Javascript\">alert('没有站点数据，请先增加站点');</script>";
        }

        Access.Dispose();
    }

    private void bindPath(DataAccess.DataAccess Access, string kind1, string kind2, string param, string currentSubCode)
    {
        string strSql = "";
        DataTable tempDt;
        switch (param)
        {
            case "001":
                lbPath.Text = "石油要闻";
                break;

            case "002":
                lbPath.Text = "图片新闻";
                break;

            case "003":
                lbPath.Text = "企业文化";
                strSql = "select text from T_SystemKind where code='" + kind1 + "' and kind='" + param + "'";
                tempDt = Access.execSql(strSql);
                if (tempDt != null && tempDt.Rows.Count > 0)
                {
                    lbPath.Text += " — " + tempDt.Rows[0][0].ToString();
                }
                break;

            case "004":
                lbPath.Text = "技术资讯";
                strSql = "select text from T_SystemKind where code='" + kind1 + "' and kind='" + param + "'";
                tempDt = Access.execSql(strSql);
                if (tempDt != null && tempDt.Rows.Count > 0)
                {
                    lbPath.Text += " — " + tempDt.Rows[0][0].ToString();
                }
                break;

            case "005":
                lbPath.Text = "公告";
                if (currentSubCode != "")
                {
                    if (kind1.ToLower() == "global")
                    {
                        lbPath.Text += " — 全局";
                    }
                    else
                    {
                        strSql = "select subName from T_SubSection where subCode='" + currentSubCode + "'";
                        tempDt = Access.execSql(strSql);
                        if (tempDt != null && tempDt.Rows.Count > 0)
                        {
                            lbPath.Text += " — " + tempDt.Rows[0][0].ToString();
                        }
                    }
                }
                break;

            case "006":
                lbPath.Text = "为您服务";
                strSql = "select text from T_SystemKind where code='" + kind1 + "' and kind='" + param + "'";
                tempDt = Access.execSql(strSql);
                if (tempDt != null && tempDt.Rows.Count > 0)
                {
                    lbPath.Text += " — " + tempDt.Rows[0][0].ToString();
                }
                break;

            case "007":
                lbPath.Text = "专业服务";
                strSql = "select text from T_SystemKind where code='" + kind1 + "' and kind='" + param + "' and subCode='" + currentSubCode + "'";
                tempDt = Access.execSql(strSql);
                if (tempDt != null && tempDt.Rows.Count > 0)
                {
                    lbPath.Text += " — " + tempDt.Rows[0][0].ToString();
                }
                break;

            case "008":
                lbPath.Text = "矿区服务";
                break;

            case "009":
                lbPath.Text = "专题报道";
                if (kind1 != "")
                {
                    strSql = "select text from T_SystemKind where code='" + kind1 + "' and kind='" + param + "'";
                    tempDt = Access.execSql(strSql);
                    if (tempDt != null && tempDt.Rows.Count > 0)
                    {
                        lbPath.Text += " — " + tempDt.Rows[0][0].ToString();
                    }
                }
                break;

            default:
                break;
        }
    }

    private void bindKind(DataAccess.DataAccess Access,string param,string currentSubCode)
    {
        TableRow tr;
        TableCell td;
        string strSql = "select * from T_SystemKind where subCode='" + currentSubCode + "' and kind='" + param + "'";
        DataTable dt = Access.execSql(strSql);
        if (dt!=null && dt.Rows.Count>0)
        {
            DataRow[] drs = dt.Select("pptr='1'");
            if (drs != null && drs.Length > 0)
            {
                for (int i = 0; i < drs.Length; i++)
                {
                    tr = new TableRow();
                    td = new TableCell();
                    td.BackColor = System.Drawing.Color.AliceBlue;
                    td.Text = "<a href=\"articleList.aspx?kind1="+drs[i]["code"].ToString()+"&kind2=&param="+param+"\">"+drs[i]["text"].ToString()+"</a>";
                    td.Height = Unit.Pixel(30);
                    td.VerticalAlign = VerticalAlign.Middle;
                    td.ColumnSpan = 6;
                    tr.Cells.Add(td);
                    tb_kqfw.Rows.Add(tr);
                    bindKind(dt, drs[i]["code"].ToString(),param);
                }
            }
        }
    }

    private void bindKind(DataTable dt,string pptr,string param)
    {
        TableRow tr;
        TableCell td;
        DataRow[] drs = dt.Select("pptr='" + pptr + "'");
        if (drs != null && drs.Length > 0)
        {
            for (int i = 0; i < drs.Length; i++)
            {
                tr = new TableRow();
                for (int j = 0; j < 6; j++)
                {
                    if (i >= drs.Length)
                    {
                        td = new TableCell();
                        td.Text = "&nbsp;";
                        td.Height = Unit.Pixel(30);
                        td.VerticalAlign = VerticalAlign.Middle;
                        td.Width = Unit.Pixel(130);
                        tr.Cells.Add(td);
                    }
                    else
                    {
                        td = new TableCell();
                        td.Text = "<a href=\"articleList.aspx?kind1=" + pptr +"&kind2=" + drs[i]["code"].ToString() + "&param=" + param + "\">" + drs[i]["text"].ToString() + "</a>";
                        td.Height = Unit.Pixel(30);
                        td.VerticalAlign = VerticalAlign.Middle;
                        td.Width = Unit.Pixel(130);
                        tr.Cells.Add(td);
                    }
                    i++;
                }
                tb_kqfw.Rows.Add(tr);  
            }
        }
    }
}


