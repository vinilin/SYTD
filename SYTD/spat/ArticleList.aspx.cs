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

public partial class ArticleList : System.Web.UI.Page
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
            string kind1="",kind2="",param="";
            if (Request.QueryString["kind1"]!=null && Request.QueryString["kind1"].ToString()!="")
            {
                kind1 = Request.QueryString["kind1"].ToString();
            }
            if (Request.QueryString["kind2"]!=null && Request.QueryString["kind2"].ToString()!="")
            {
                kind2 = Request.QueryString["kind2"].ToString();
            }
            if (Request.QueryString["param"]!=null && Request.QueryString["param"].ToString()!="")
            {
                param = Request.QueryString["param"].ToString();
            }
            if (param == "") { Response.Redirect("default.aspx"); }
            bindPath(Access, kind1, kind2, param, currentSubCode);
            bindKind(Access, kind1, kind2, param,currentSubCode);
            bindList(Access, kind1, kind2, param,currentSubCode);
        }
        else
        {
            lbMessage.Text = "<script language=\"Javascript\">alert('没有站点数据，请先增加站点');</script>";
        }

        Access.Dispose();
    }

    private void bindPath(DataAccess.DataAccess Access,string kind1,string kind2,string param,string currentSubCode)
    {
        string strSql = "";
        DataTable tempDt;
        switch(param)
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
                if (currentSubCode!="")
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
                if (kind1!="")
                {
                    strSql = "select text from T_SystemKind where code='" + kind1 + "' and kind='" + param + "' and subCode='" + currentSubCode + "'";
                    tempDt = Access.execSql(strSql);
                    if (tempDt != null && tempDt.Rows.Count > 0)
                    {
                        lbPath.Text += " — " + tempDt.Rows[0][0].ToString();
                        for (int i = 0; i < tempDt.Rows.Count;i++ )
                        {

                        }
                    }
                }
                if (kind2 != "")
                {
                    strSql = "select text from T_SystemKind where code='" + kind2 + "' and kind='" + param + "' and subCode='" + currentSubCode + "'";
                    tempDt = Access.execSql(strSql);
                    if (tempDt != null && tempDt.Rows.Count > 0)
                    {
                        lbPath.Text += " — " + tempDt.Rows[0][0].ToString();
                    }
                }
                break;

            case "009":
                lbPath.Text = "专题报道";
                if (kind1!="")
                {
                    strSql = "select text from T_SystemKind where code='" + kind1 + "' and kind='" + param + "'";
                    tempDt = Access.execSql(strSql);
                    if (tempDt!=null && tempDt.Rows.Count>0)
                    {
                        lbPath.Text += " — " + tempDt.Rows[0][0].ToString();
                    }
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Access"></param>
    /// <param name="kind1">大类</param>
    /// <param name="kind2">小类</param>
    /// <param name="param">栏目ID</param>
    private void bindKind(DataAccess.DataAccess Access, string kind1, string kind2, string param, string currentSubCode)
    {
        string strSql = "";
        DataTable dt;
        TableRow tr;
        TableCell td;
        switch(param)
        {
            case "001":

                break;

            case "002":

                break;

            case "003":
            case "004":
            case "006":
            case "009":
                strSql = "select code,text,linkOrKind,linkUrl from T_SystemKind where kind='"+param+"' order by listOrder";
                dt = Access.execSql(strSql);
                if (dt!=null && dt.Rows.Count>0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tr = new TableRow();
                        td = new TableCell();
                        if (dt.Rows[i]["linkOrKind"].ToString() == "0")
                        {
                            td.Text = "<img src=\"images/dot.gif\" border=\"0\">&nbsp;&nbsp;<a href=\"articleList.aspx?kind1=" + dt.Rows[i]["code"].ToString() + "&kind2=&param=" + param + "\">" + dt.Rows[i]["text"].ToString() + "</a>";
                        }
                        else
                        {
                            td.Text = "<img src=\"images/dot.gif\" border=\"0\">&nbsp;&nbsp;<a href=\"" + dt.Rows[i]["linkUrl"].ToString() + "\" target=\"_blank\">" + dt.Rows[i]["text"].ToString() + "</a>";
                        }
                        td.Height = Unit.Pixel(30);
                        td.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(td);
                        tb_kind.Rows.Add(tr);
                    }
                }
                break;

            case "005":
                tr = new TableRow();
                td = new TableCell();
                td.Text = "<img src=\"images/dot.gif\" border=\"0\">&nbsp;&nbsp;<a href=\"articleList.aspx?kind1=&kind2=&param=" + param + "&subCode=global\">全局</a>";
                td.Height = Unit.Pixel(30);
                td.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(td);
                tb_kind.Rows.Add(tr);

                Sub sub = new Sub();
                DataTable subList = sub.GetSubList(Access);

                if (subList != null && subList.Rows.Count > 0)
                {
                    for (int i = 0; i < subList.Rows.Count; i++)
                    {
                        tr = new TableRow();
                        td = new TableCell();
                        td.Text = "<img src=\"images/dot.gif\" border=\"0\">&nbsp;&nbsp;<a href=\"articleList.aspx?kind1=&kind2=&param=" + param + "&subCode=" + subList.Rows[i]["subCode"].ToString() + "\">" + subList.Rows[i]["subName"].ToString() + "</a>";
                        td.Height = Unit.Pixel(30);
                        td.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(td);
                        tb_kind.Rows.Add(tr);
                    }
                }
                break;

            case "007":
                strSql = "select code,text,linkOrKind,linkUrl from T_SystemKind where kind='" + param + "' and subCode='" + currentSubCode + "' order by listOrder";
                dt = Access.execSql(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tr = new TableRow();
                        td = new TableCell();
                        if (dt.Rows[i]["linkOrKind"].ToString() == "0")
                        {
                            td.Text = "<img src=\"images/dot.gif\" border=\"0\">&nbsp;&nbsp;<a href=\"articleList.aspx?kind1=" + dt.Rows[i]["code"].ToString() + "&kind2=&param=" + param + "\">" + dt.Rows[i]["text"].ToString() + "</a>";
                        }
                        else
                        {
                            td.Text = "<img src=\"images/dot.gif\" border=\"0\">&nbsp;&nbsp;<a href=\"" + dt.Rows[i]["linkUrl"].ToString() + "\" target=\"_blank\">" + dt.Rows[i]["text"].ToString() + "</a>";
                        }
                        td.Height = Unit.Pixel(30);
                        td.VerticalAlign = VerticalAlign.Middle;
                        tr.Cells.Add(td);
                        tb_kind.Rows.Add(tr);
                    }
                }
                break;

            case "008":
                /*
                strSql = "select * from T_SystemKind where kind='" + param + "' and subCode='" + currentSubCode + "' order by listOrder";
                dt = Access.execSql(strSql);
                
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] drs = dt.Select("pptr='1'");
                    if (drs != null && drs.Length > 0)
                    {
                        for (int i = 0; i < drs.Length; i++)
                        {
                            tr = new TableRow();
                            td = new TableCell();
                            td.Text = "<img src=\"images/dot.gif\" border=\"0\">&nbsp;&nbsp;<a href=\"articleList.aspx?kind1=" + drs[i]["code"].ToString() + "&kind2=&param=" + param + "\">" +drs[i]["text"].ToString() + "</a>";
                            td.Height = Unit.Pixel(30);
                            td.VerticalAlign = VerticalAlign.Middle;
                            tr.Cells.Add(td);
                            tb_kind.Rows.Add(tr);
                            bindKind2(dt, drs[i]["code"].ToString(), param);
                        }
                    }
                }
                 * */
                break;

            default:
                break;
        }
    }

    private void bindKind2(DataTable dt,string pptr,string param)
    {
        DataRow[] drs = dt.Select("pptr='" + pptr + "'");
        if (drs!=null && drs.Length>0)
        {
            for(int i=0;i<drs.Length;i++)
            {
                TableRow tr = new TableRow();
                TableCell td = new TableCell();
                td.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img src=\"images/dot.gif\" border=\"0\">&nbsp;&nbsp;<a href=\"articleList.aspx?kind1=" +pptr + "&kind2="+drs[i]["code"].ToString()+"&param=" + param + "\">" + drs[i]["text"].ToString() + "</a>";
                td.Height = Unit.Pixel(30);
                td.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(td);
                tb_kind.Rows.Add(tr);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Access"></param>
    /// <param name="kind1">大类</param>
    /// <param name="kind2">小类</param>
    /// <param name="param">栏目ID</param>
    private void bindList(DataAccess.DataAccess Access, string kind1, string kind2, string param, string currentSubCode)
    {
        string strWhere = "";
       switch (param)
       {
           case "005":
               strWhere = "T_Notify.state=1 and T_Notify.subCode='" + currentSubCode + "'";
               ucGrid.tblName = "T_Notify";
               ucGrid.searchKind = "show";
               ucGrid.orderField = "auditTime";
               ucGrid.attachLink = "articleDetail.aspx?kind1="+kind1+"&kind2="+kind2+"&param="+param+"&subCode="+currentSubCode+"";
               ucGrid.pageSize = 20;
               ucGrid.pageIndex = 0;
               ucGrid.orderType = true;
               ucGrid.strWhere = strWhere;
               ucGrid.isShowHeader = true;
               ucGrid.databind();
       	       break;

           case "001":
           case "002":
               strWhere = "T_ArticleList.state=1 and T_ArticleList.ArticleType='" + param + "'";
               ucGrid.tblName = "T_ArticleList";
               ucGrid.searchKind = "show";
               ucGrid.orderField = "auditTime";
               ucGrid.attachLink = "articleDetail.aspx?kind1=" + kind1 + "&kind2=" + kind2 + "&param=" + param + "";
               ucGrid.pageSize = 20;
               ucGrid.pageIndex = 0;
               ucGrid.orderType = true;
               ucGrid.strWhere = strWhere;
               ucGrid.isShowHeader = true;
               ucGrid.databind();
               break;

           case "003":
               strWhere = "T_ArticleList.state=1 and T_ArticleList.ArticleType='" + param + "' ";
               if (kind1 != "")
               {
                   strWhere += " and T_ArticleList.ArticleKind='" + kind1 + "'";
               }
               ucGrid.tblName = "T_ArticleList";
               ucGrid.searchKind = "qywh";
               ucGrid.orderField = "auditTime";
               ucGrid.attachLink = "articleDetail.aspx?kind1=" + kind1 + "&kind2=" + kind2 + "&param=" + param + "";
               ucGrid.pageSize = 20;
               ucGrid.pageIndex = 0;
               ucGrid.orderType = true;
               ucGrid.strWhere = strWhere;
               ucGrid.isShowHeader = true;
               ucGrid.databind();
               break;

           case "004":
           case "006":
               strWhere = "T_ArticleList.state=1 and T_ArticleList.ArticleType='" + param + "' ";
               if (kind1 != "")
               {
                  strWhere += " and T_ArticleList.ArticleKind='" + kind1 + "'";
               }
               ucGrid.tblName = "T_ArticleList";
               ucGrid.searchKind = "show";
               ucGrid.orderField = "auditTime";
               ucGrid.attachLink = "articleDetail.aspx?kind1=" + kind1 + "&kind2=" + kind2 + "&param=" + param + "";
               ucGrid.pageSize = 20;
               ucGrid.pageIndex = 0;
               ucGrid.orderType = true;
               ucGrid.strWhere = strWhere;
               ucGrid.isShowHeader = true;
               ucGrid.databind();
               break;

           case "007":
               strWhere = "T_ArticleList.state=1 and T_ArticleList.ArticleType='" + param + "' and subCode = '" + currentSubCode + "'";
               if (kind1 != "")
               {
                   strWhere += " and T_ArticleList.ArticleKind='" + kind1 + "'";
               }
               ucGrid.tblName = "T_ArticleList";
               ucGrid.searchKind = "show";
               ucGrid.orderField = "auditTime";
               ucGrid.attachLink = "articleDetail.aspx?kind1=" + kind1 + "&kind2=" + kind2 + "&param=" + param + "";
               ucGrid.pageSize = 20;
               ucGrid.pageIndex = 0;
               ucGrid.orderType = true;
               ucGrid.strWhere = strWhere;
               ucGrid.isShowHeader = true;
               ucGrid.databind();
               break;


           case "008":
                strWhere = "T_ArticleList.state=1 and T_ArticleList.ArticleType='" + param + "'";
               /*
               strWhere = "T_ArticleList.state=1 and T_ArticleList.ArticleType='" + param + "' and subCode = '" + currentSubCode + "'";
               if (kind1 != "")
               {
                   strWhere += " and T_ArticleList.ArticleKind='" + kind1 + "'";
               }
               if (kind2!="")
               {
                   strWhere += " and T_ArticleList.ArticleKind2='" + kind2 + "'";
               }
                * */
               ucGrid.tblName = "T_ArticleList";
               ucGrid.searchKind = "show";
               ucGrid.orderField = "auditTime";
               ucGrid.attachLink = "articleDetail.aspx?kind1=" + kind1 + "&kind2=" + kind2 + "&param=" + param + "";
               ucGrid.pageSize = 20;
               ucGrid.pageIndex = 0;
               ucGrid.orderType = true;
               ucGrid.strWhere = strWhere;
               ucGrid.isShowHeader = true;
               ucGrid.databind();
               break;

           case "009":
               strWhere = "T_ArticleList.state=1 and T_ArticleList.ArticleType='" + param + "'";
               if (kind1 != "")
               {
                   strWhere += " and T_ArticleList.ArticleKind='" + kind1 + "' ";
               }
               //strWhere += " and T_ArticleList.subCode='" + currentSubCode + "'";
               ucGrid.tblName = "T_ArticleList";
               ucGrid.searchKind = "show";
               ucGrid.orderField = "auditTime";
               ucGrid.attachLink = "articleDetail.aspx?kind1=" + kind1 + "&kind2=" + kind2 + "&param=" + param + "";
               ucGrid.pageSize = 20;
               ucGrid.pageIndex = 0;
               ucGrid.orderType = true;
               ucGrid.strWhere = strWhere;
               ucGrid.isShowHeader = true;
               ucGrid.databind();
               break;

           default:
               break;
       }
    }
}
