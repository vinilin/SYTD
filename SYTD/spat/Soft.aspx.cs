﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Net;

public partial class Soft : System.Web.UI.Page
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
            string kind1 = "", kind2 = "", param = "013";
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
            bindPath(Access, kind1, kind2, param);
            bindKind(Access, kind1, kind2, param);
            bindPH();
            //kindBind(Access);

            bindSoftList(kind1, kind2);

        }
        else
        {
            lbMessage.Text = "<script language=\"Javascript\">alert('没有站点数据，请先增加站点');</script>";
        }

        Access.Dispose();
    }

    private void bindPath(DataAccess.DataAccess Access,string kind1,string kind2,string param)
    {
        lbPath.Text = "软件下载";
        //string strSql = "select text from T_SystemKind where code='" + kind1 + "' and kind='" + param + "'";
        string strSql = "select NAME as text from PublishType where Category=3 ";
        DataTable tempDt = Access.execSql(strSql);
        if (tempDt != null && tempDt.Rows.Count > 0)
        {
            lbPath.Text += " — " + tempDt.Rows[0][0].ToString();
        }

    }
    private void bindPH()
    {
        //读出播放次数最高的音乐
        string strSql = "select TOP 10 BaseItem.id,BaseItem.Title,BaseItem.category from BaseItem ";
        strSql += " where BaseItem.Category=" + FileShareCommon.Category.Software;
        strSql += "  order by BaseItem.BrowseCount desc";

        DataAccess.DataAccess_PB Access = new DataAccess.DataAccess_PB();
        DataTable dt = Access.execSql(strSql);
        if (dt != null)
        {
            int rowCount = dt.Rows.Count;
            for (int i = 0; i < 8 - rowCount; i++)
            {
                DataRow dr = dt.NewRow();
                dr["id"] = 0;
                dr["title"] = "";
                dr["category"] = 0;
                dt.Rows.Add(dr);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TableRow tr = new TableRow();

                TableCell td2 = new TableCell();
                td2.Height = Unit.Pixel(25);
                if (dt.Rows[i]["title"].ToString() != "")
                {
                    if (dt.Rows[i]["title"].ToString().Length <= 10)
                    {
                        td2.Text = "<a href=\"softShow.aspx?Id=" + dt.Rows[i]["Id"].ToString() + "\" target=_blank>" + (i+1).ToString() + ". " + dt.Rows[i]["title"].ToString() + "</a>";
                    }
                    else
                    {
                        td2.Text = "<a href=\"softShow.aspx?Id=" + dt.Rows[i]["Id"].ToString() + "\" target=_blank>" + (i+1).ToString() + ". " + dt.Rows[i]["title"].ToString().Substring(0, 12) + "...</a>";
                    }
                }
                else
                {
                    td2.Text = "&nbsp;";
                }
                tr.Cells.Add(td2);

                tablePlayTimes.Rows.Add(tr);
                
            }
        }
        Access.Dispose();
    }
    private void bindKind(DataAccess.DataAccess Access, string kind1, string kind2,string param)
    {
        //string strSql = "select code,text from T_SystemKind where kind='" + param + "' order by listOrder";
        string strSql = "select ID as code, NAME as text from PublishType where Category=3 ";
        DataTable dt = Access.execSql(strSql);
        if (dt != null && dt.Rows.Count > 0)
        {
            TableRow tr;
            TableCell td;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tr = new TableRow();
                td = new TableCell();
                td.Text = "<img src=\"images/dot.gif\" border=\"0\">&nbsp;&nbsp;<a href=\"soft.aspx?kind1=" + dt.Rows[i]["code"].ToString() + "&kind2=&param=" + param + "\">" + dt.Rows[i]["text"].ToString() + "</a>";
                td.Height = Unit.Pixel(30);
                td.VerticalAlign = VerticalAlign.Middle;
                tr.Cells.Add(td);
                tb_kind.Rows.Add(tr);
            }
        }
    }
    private void bindPath(DataAccess.DataAccess Access, string kind1)
    {
        lbPath.Text = "软件下载";
        if (kind1 != "")
        {
            string strSql = "select name from publishType where id='" + kind1 + "' and category=3";
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                lbPath.Text += " — " + tempDt.Rows[0][0].ToString();
            }
        }
    }

    private void kindBind(DataAccess.DataAccess Access)
    {
        string strSql = "select * from Pub lishType where category=3";
        DataTable dt = Access.execSql(strSql);
        if (dt != null)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                //////////////////////////////////////////////////
                TableRow trKind = new TableRow();
                TableCell tdKind = new TableCell();
                tdKind.Text = "";
                tdKind.Height = Unit.Pixel(10);
                trKind.Cells.Add(tdKind);
                tb_kind.Rows.Add(trKind);

                TableRow trKind1 = new TableRow();
                TableCell tdKind1 = new TableCell();
                tdKind1.Width = Unit.Pixel(160);
                tdKind1.Height = Unit.Pixel(27);
                tdKind1.VerticalAlign = VerticalAlign.Middle;
                tdKind1.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;·<a href='Soft.aspx?kind1=" + dt.Rows[i]["id"].ToString() + "'>" + dt.Rows[i]["name"].ToString() + "</a>";
                trKind1.Cells.Add(tdKind1);
                tb_kind.Rows.Add(trKind1);
                //////////////////////////////////////////////////
            }
        }
    }

    private void bindSoftList(string kind1, string kind2)
    {
        //string strWhere = " BaseItem.publishType=3"; //publishType=3 表示软件
        string strWhere = " BaseItem.Category=3 ";
        if (kind1 != "")
        {
            strWhere += " and BaseItem.publishType = '" + kind1 + "' ";
            //strWhere = "BaseItem.publishType = '" + kind1 + "' and BaseItem.Category=2"; //publishType=2 表示音乐
        }
        ucGrid.tblName = "BaseItem";
        ucGrid.orderField = "Birth";
        ucGrid.pageSize = 8;
        ucGrid.pageIndex = 0;
        ucGrid.orderType = false;
        ucGrid.strWhere = strWhere;
        ucGrid.isShowHeader = false;
        ucGrid.databind();
    }
}
