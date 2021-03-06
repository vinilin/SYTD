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

public partial class ArticleDetail : System.Web.UI.Page
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
            if (Request.QueryString["Id"] == null || Request.QueryString["Id"].ToString() == "")
            {
                Response.Redirect("default.aspx"); 
            }
            bindPath(Access, kind1, kind2, param, currentSubCode);
            BindContent(Access, Request.QueryString["Id"].ToString(), kind1, kind2, param, currentSubCode);
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
        lbPath.Text += " — 正文";
    }

    private void BindContent(DataAccess.DataAccess Access,string id,string kind1,string kine2,string param,string currentSubCode)
    {
        string strSql = "";
        DataTable dt;
        switch(param)
        {
            case "005":
                strSql = "select id,notifyTitle,notifyContent,auditTime as pubTime from  T_Notify where state=1 and id=" + id;
                dt = Access.execSql(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    lbTitle.Text = dt.Rows[0]["notifyTitle"].ToString();
                    lbPubTime.Text = ((DateTime)dt.Rows[0]["pubTime"]).ToString("yyyy-MM-dd");
                    lbContent.Text = dt.Rows[0]["notifyContent"].ToString();
                }
                break;

            case "001":
            case "002":
                
            case "003":
            case "004":
            case "006":
            case "007":
            case "008":
            case "009":
                strSql = "select articleTitle,articleContent,auditTime as pubTime,source,viewTimes from  T_ArticleList where state=1 and id=" + id;
                dt = Access.execSql(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    lbTitle.Text = dt.Rows[0]["articleTitle"].ToString();
                    if (param == "001" || param == "002")
                    {
                        if (dt.Rows[0]["source"].ToString()!="")
                        {
                            lbPubTime.Text = ((DateTime)dt.Rows[0]["pubTime"]).ToString("yyyy-MM-dd");
                            lbPubTime.Text += "    新闻来源：" + dt.Rows[0]["source"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        lbPubTime.Text += "访问次数：" + dt.Rows[0]["viewTimes"].ToString() + "次访问";

                        string strSql_temp = "update T_ArticleList set viewTimes = viewTimes+1 where id=" + id;
                        Access.execSqlNoQuery1(strSql_temp);
                    }
                    else
                    {
                        lbPubTime.Text = ((DateTime)dt.Rows[0]["pubTime"]).ToString("yyyy-MM-dd");
                    }
                    lbContent.Text = dt.Rows[0]["articleContent"].ToString();
                }
                break;

            default:
                break;
        }
    }
}
