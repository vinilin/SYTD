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
using System.Text.RegularExpressions;
public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataAccess.DataAccess Access = new DataAccess.DataAccess();
        Sub sub = new Sub();
        DataTable subList = sub.GetSubList(Access);
        if (subList != null && subList.Rows.Count > 0)
        {
            string requestSubCode="";
            if (Request.QueryString["subCode"] != null && Request.QueryString["subCode"].ToString() != "")
            {
                requestSubCode = Request.QueryString["subCode"].ToString();
            }
            string currentSubCode = sub.GetCurrentSub(Access, requestSubCode, subList.Rows[0]["subCode"].ToString());
            
            ucHeader.Bind(subList);
            ucFooter.Bind(subList);
            qywh(Access);
            tpxw(Access);
            gonggao(Access, currentSubCode);
            ztbd(Access);
            wnfw(Access);
            zyfw(Access, currentSubCode);
            syyw(Access);
            kqfw(Access, currentSubCode);
            link(Access);
            spxw(Access);
            PlayNews(Access);
        }
        else
        {
            lbMessage.Text = "<script language=\"Javascript\">alert('没有站点数据，请先增加站点');</script>";
        }
        Access.Dispose();
    }
    private void PlayNews(DataAccess.DataAccess access)
    {
        string sql = "select top 1 * from T_VidioNewsList where state = 1 order by id desc"; 
        DataTable dt = access.execSql(sql);
        string url = "";
        if(dt.Rows.Count > 0)
            url = new Common.SysConfig().GetValueByKey("ManagementUrl") + "UploadFile/015/" + dt.Rows[0]["sequence"].ToString() + "/" + dt.Rows[0]["defaultVidio"].ToString();
        string html = "";
        html+="<object id=\"Player\" height=\"185\" width=\"200\" classid=\"CLSID:6BF52A52-394A-11d3-B153-00C04F79FAA6\" align=\"center\" border=\"0\">";
        html+="<param name=\"AutoStart\" value=\"0\">";
        html+="<param name=\"Balance\" value=\"0\">";
        html+="<param name=\"enabled\" value=\"-1\">";
        html+="<param name=\"EnableContextMenu\" value=\"-1\">";
        html+="<param name=\"url\" value=\""+url+"\">";
        //html+="<param name=\"url\" value=\"..\\sytdmanagement\\uploadFile\\015\\20100704001956119\\x.wmv\">";
        html+="<param name=\"PlayCount\" value=\"1\">";
        html+="<param name=\"rate\" value=\"1\">";
        html+="<param name=\"currentPosition\" value=\"0\">";
        html+="<param name=\"currentMarker\" value=\"0\">";
        html+="<param name=\"defaultFrame\" value=\"\">";
        html+="<param name=\"invokeURLs\" value=\"0\">";
        html+="<param name=\"baseURL\" value=\"\">";
        html+="<param name=\"stretchToFit\" value=\"0\">";
        html+="<param name=\"volume\" value=\"100\">";
        html+="<param name=\"mute\" value=\"0\">";
        html+="<param name=\"uiMode\" value=\"mini\">";
        html+="<param name=\"windowlessVideo\" value=\"-1\">";
        html+="<param name=\"fullScreen\" value=\"0\">";
        html+="<param name=\"enableErrorDialogs\" value=\"-1\">";
        html+="<param name=\"SAMIStyle\" value>";
        html+="<param name=\"SAMILang\" value>";
        html+="<param name=\"SAMIFilename\" value>";
        html+="<param name=\"captioningID\" value>";
        html+="</object>";
        this.lb_Player.Text = html;
         

    }
    private void qywh(DataAccess.DataAccess Access)
    {
        string param = "003";
        string strSql = "select top 4 code,text,sequence,defaultPic from T_SystemKind where kind='" + param + "' order by listOrder";
        DataTable dt = Access.execSql(strSql);
        if (dt == null)
        {
            dt = new DataTable();
            DataColumn dc1 = new DataColumn("code");
            DataColumn dc2 = new DataColumn("text");
            DataColumn dc3 = new DataColumn("sequence");
            DataColumn dc4 = new DataColumn("defaultPic");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);            
            dt.Columns.Add(dc4);
        }
        int nCount = dt.Rows.Count;
        for (int i = 0; i < 4 - nCount; i++)
        {
            DataRow dr = dt.NewRow();
            dr["code"] = "";
            dr["text"] = "";
            dr["sequence"] = "";
            dr["defaultPic"] = "";
            dt.Rows.Add(dr);
        }
        /*
            TableRow tr_space = new TableRow();
            TableCell td_space = new TableCell();
            td_space.Height = Unit.Pixel(20);
            td_space.ColumnSpan = 2;
            tr_space.Cells.Add(td_space);
            tb_qywh.Rows.Add(tr_space);
         * */
        for (int i = 0; i < dt.Rows.Count; i++)
        {

            TableRow tr = new TableRow();
            TableCell td;

            td=new TableCell();
            string str = "";
            if (dt.Rows[i]["defaultPic"].ToString() != "")
            {
                str += "<a href=\"articleList.aspx?kind1=" + dt.Rows[i]["code"].ToString() + "&kind2=&param=" + param + "\"><img src=\"" + new Common.SysConfig().GetValueByKey("ManagementUrl") + "uploadFile/" + param + "/" + dt.Rows[i]["sequence"].ToString() + "/" + dt.Rows[i]["defaultPic"].ToString() + "\" border=\"0\" width=\"60\" height=\"34\"></a>";
            }
            str += "<br/>";
            str += "<a href=\"articleList.aspx?kind1=" + dt.Rows[i]["code"].ToString() + "&kind2=&param=" + param +"\">" + dt.Rows[i]["text"].ToString() + "</a>";
            td.Text = str;
            td.Width = Unit.Pixel(100);
            td.Height = Unit.Pixel(30);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.VerticalAlign = VerticalAlign.Bottom;
            tr.Cells.Add(td);

            i++;

            if (i < dt.Rows.Count)
            {
                td = new TableCell();
                str = "";
                if (dt.Rows[i]["defaultPic"].ToString() != "")
                {
                    str += "<a href=\"articleList.aspx?kind1=" + dt.Rows[i]["code"].ToString() + "&kind2=&param=" + param + "\"><img src=\"" + new Common.SysConfig().GetValueByKey("ManagementUrl") + "uploadFile/" + param + "/" + dt.Rows[i]["sequence"].ToString() + "/" + dt.Rows[i]["defaultPic"].ToString() + "\" border=\"0\" width=\"60\" height=\"34\"></a>";
                }
                str += "<br/>";
                str += "<a href=\"articleList.aspx?kind1=" + dt.Rows[i]["code"].ToString() + "&kind2=&param=" + param + "\">" + dt.Rows[i]["text"].ToString() + "</a>";
                td.Text = str;
                td.Width = Unit.Pixel(100);
                td.Height = Unit.Pixel(30);
                td.HorizontalAlign = HorizontalAlign.Center;
                td.VerticalAlign = VerticalAlign.Bottom;
                tr.Cells.Add(td);
            }
            else
            {
                /*
                td = new TableCell();
                td.Text = "&nbsp;";
                td.Width = Unit.Pixel(100);
                td.Height = Unit.Pixel(20);
                td.HorizontalAlign = HorizontalAlign.Center;
                td.VerticalAlign = VerticalAlign.Bottom;
                tr.Cells.Add(td);
                 **/
            }
            tb_qywh.Rows.Add(tr);
        }
    }

    private void tpxw(DataAccess.DataAccess Access)
    {
        string param = "002";
        string strSql = "select top 8 id,articleTitle,sequence,defaultPic,auditTime as pubTime from T_ArticleList where state=1 and articleType='" + param + "' order by auditTime desc";
        DataTable dt = Access.execSql(strSql);
        if (dt == null)
        {
            dt = new DataTable();
            DataColumn dc1 = new DataColumn("id");
            DataColumn dc2 = new DataColumn("articleTitle");
            DataColumn dc3 = new DataColumn("sequence");
            DataColumn dc4 = new DataColumn("defaultPic");
            DataColumn dc5 = new DataColumn("auditTime");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);
            dt.Columns.Add(dc5);
        }
        int nCount = dt.Rows.Count;
        for (int i = 0; i < 8 - nCount; i++)
        {
            DataRow dr = dt.NewRow();
            dr["id"] = 0;
            dr["articleTitle"] = "";
            dr["sequence"] = "";
            dr["defaultPic"] = "";
            dt.Rows.Add(dr);
        }
        string str = "";

        str += "<script type=\"text/Javascript\">" + "\r\n";
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["articleTitle"].ToString() != "" && dt.Rows[i]["defaultPic"].ToString()!="")
            {
                string tempStr = dt.Rows[i]["articleTitle"].ToString();
                if (tempStr.Length > 13)
                {
                    tempStr = tempStr.Substring(0, 11) + "...";
                    //tempStr = GetSubString(tempStr,22);
                }
                str += "imgUrl" + (i + 1).ToString() + "=\"" + new Common.SysConfig().GetValueByKey("ManagementUrl") + "UploadFile/" + param + "/" + dt.Rows[i]["sequence"].ToString() + "/" + dt.Rows[i]["defaultPic"].ToString() + "\";" + "\r\n";
                str += "imgtext" + (i + 1).ToString() + "=\"" + tempStr + "\";" + "\r\n";
                str += "imgLink" + (i + 1).ToString() + "=escape(\"articleDetail.aspx?kind1=&kind2=&param=" + param + "&Id=" + dt.Rows[i]["Id"].ToString() + "\");" + "\r\n";
            }
            else
            {
                str += "imgUrl" + (i + 1).ToString() + "=\"\";" + "\r\n";
                str += "imgtext" + (i + 1).ToString() + "=\"\";" + "\r\n";
                str += "imgLink" + (i + 1).ToString() + "=\"\";" + "\r\n";
            }
        }
        str += "var focus_width=205;" + "\r\n";
        str += "var focus_height=170;" + "\r\n";
        str += "var text_height=24;" + "\r\n";
        str += "var swf_height = focus_height+text_height;" + "\r\n";

        str += " var pics=imgUrl1+\"|\"+imgUrl2+\"|\"+imgUrl3+\"|\"+imgUrl4+\"|\"+imgUrl5;" + "\r\n";
        str += "var links=imgLink1+\"|\"+imgLink2+\"|\"+imgLink3+\"|\"+imgLink4+\"|\"+imgLink5;" + "\r\n";
        str += "var texts=imgtext1+\"|\"+imgtext2+\"|\"+imgtext3+\"|\"+imgtext4+\"|\"+imgtext5;" + "\r\n";

        str += "document.write('<object classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" codebase=\"http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0\" width=\"'+ focus_width +'\" height=\"'+ swf_height +'\">');" + "\r\n";
        str += "document.write('<param name=\"allowScriptAccess\" value=\"sameDomain\"><param name=\"movie\" value=\"images/flashpic21.swf\"><param name=\"quality\" value=\"high\"><param name=\"bgcolor\" value=\"#F0F0F0\">');" + "\r\n";
        str += "document.write('<param name=\"menu\" value=\"false\"><param name=wmode value=\"opaque\">');" + "\r\n";
        str += "document.write('<param name=\"FlashVars\" value=\"pics='+pics+'&links='+links+'&texts='+texts+'&borderwidth='+focus_width+'&borderheight='+focus_height+'&textheight='+text_height+'\">');" + "\r\n";
        str += "document.write('<embed src=\"images/flashpic21.swf\" wmode=\"opaque\" FlashVars=\"pics='+pics+'&links='+links+'&texts='+texts+'&borderwidth='+focus_width+'&borderheight='+focus_height+'&textheight='+text_height+'\" menu=\"false\" bgcolor=\"#F0F0F0\" quality=\"high\" width=\"'+ focus_width +'\" height=\"'+ focus_height +'\" allowScriptAccess=\"sameDomain\" type=\"application/x-shockwave-flash\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" />');  document.write('</object>');" + "\r\n";
        str += "</script>" + "\r\n";
        lb_tpxw.Text = str;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            TableCell td1 = new TableCell();

            string tempStr=dt.Rows[i]["articleTitle"].ToString();
            if (tempStr.Length>=15)
            {
                tempStr = tempStr.Substring(0, 13) + "...";
                //tempStr = GetSubString(tempStr,30);
            }
            if (dt.Rows[i]["articleTitle"].ToString()!="" && dt.Rows[i]["pubTime"].ToString()!="")
            {
                td.Text = "<img src=\"images/dot.gif\" border=0>&nbsp;<a href=\"articleDetail.aspx?kind1=&kind2=&param=" + param + "&Id=" + dt.Rows[i]["Id"].ToString() + "\" target=\"_blank\"" + " title=\"" + dt.Rows[i]["articleTitle"].ToString()+"\">"  + tempStr + "</a>";
                td1.Text = "["+((DateTime)dt.Rows[i]["pubTime"]).ToString("MM-dd")+"]";
            }
            else
            {
                td.Text = "&nbsp;";
            }

            td.Height = Unit.Pixel(25);
            td1.Height = Unit.Pixel(25);
            td.Width = Unit.Pixel(180);
            td1.Width = Unit.Pixel(40);
            tr.Cells.Add(td);
            tr.Cells.Add(td1);
            
            tb_tpxw.Rows.Add(tr);

            TableRow tr_s = new TableRow();
            TableCell td_s = new TableCell();
            td_s.Height = Unit.Pixel(1);
            td_s.Attributes.Add("background", "images/line.gif");
            td_s.ColumnSpan = 2;
            tr_s.Cells.Add(td_s);
            tb_tpxw.Rows.Add(tr_s);
        }
    }

    private void gonggao(DataAccess.DataAccess Access,string currentSubCode)
    {
        string param = "005";
        string currentTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string strSql = "select top 2 id,notifyTitle from T_Notify where state=1 and pubStartTime<='" + currentTime + "' and pubEndTime>='" + currentTime + "' and (subCode='"+currentSubCode+"' or subCode='global')";
        DataTable dt = Access.execSql(strSql);
        string tempStr = "";
        if (dt != null && dt.Rows.Count > 2 )
        {
           tempStr="<div id=\"marquees\">";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tempStr += "<img src=\"images/dot.gif\" border=\"0\">&nbsp;<a href=\"articleDetail.aspx?kind1=&kind2=&param=" + param + "&Id=" + dt.Rows[i]["Id"].ToString() + "\" target=\"_blank\">" + dt.Rows[i]["notifyTitle"].ToString() + "</a><br><br>";
            }
        tempStr += "</div> " + "\r\n";
        tempStr += "<script language=\"JavaScript\">" + "\r\n";

        tempStr += "marqueesHeight=220;" + "\r\n";
        tempStr += "stopscroll=false;" + "\r\n";

        tempStr += "with(marquees){" + "\r\n";
        tempStr += "   style.width=0;" + "\r\n";
        tempStr += "   style.height=marqueesHeight;" + "\r\n";
        tempStr += "   style.overflowX=\"visible\";" + "\r\n";
        tempStr += "   style.overflowY=\"hidden\";" + "\r\n";
        tempStr += "   noWrap=true;" + "\r\n";
        tempStr += "   onmouseover=new Function(\"stopscroll=true\");" + "\r\n";
        tempStr += "   onmouseout=new Function(\"stopscroll=false\");" + "\r\n";
        tempStr += "}" + "\r\n";
        tempStr += "document.write('<div id=\"templayer\" style=\"position:absolute;z-index:1;visibility:hidden\"></div>');" + "\r\n";

        tempStr += "preTop=0; currentTop=0; " + "\r\n";

        tempStr += "function init(){" + "\r\n";
        tempStr += "   templayer.innerHTML=\"\";" + "\r\n";
        tempStr += "   while(templayer.offsetHeight<marqueesHeight){" + "\r\n";
        tempStr += "     templayer.innerHTML+=marquees.innerHTML;" + "\r\n";
        tempStr += "   }" + "\r\n";
        tempStr += "   marquees.innerHTML=templayer.innerHTML+templayer.innerHTML;" + "\r\n";
        tempStr += "   setInterval(\"scrollUp()\",45);" + "\r\n";
        tempStr += "}" + "\r\n";
        tempStr += "document.body.onload=init;" + "\r\n";

        tempStr += "function scrollUp(){" + "\r\n";
        tempStr += "   if(stopscroll==true) return;" + "\r\n";
        tempStr += "   preTop=marquees.scrollTop;" + "\r\n";
        tempStr += "   marquees.scrollTop+=1;" + "\r\n";
        tempStr += "   if(preTop==marquees.scrollTop){" + "\r\n";
        tempStr += "     marquees.scrollTop=templayer.offsetHeight-marqueesHeight;" + "\r\n";
        tempStr += "     marquees.scrollTop+=1;" + "\r\n";
        tempStr += "   }" + "\r\n";
        tempStr += "}" + "\r\n";
        tempStr += "</script>";

        }
        else if(dt!=null && dt.Rows.Count>0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tempStr += "<br><img src=\"images/dot.gif\" border=\"0\">&nbsp;<a href=\"articleDetail.aspx?kind1=&kind2=&param=" + param + "&Id=" + dt.Rows[i]["Id"].ToString() + "\" target=\"_blank\">" + dt.Rows[i]["notifyTitle"].ToString() + "</a><br>";
            }
        }
        else
        {
            tempStr = "<br><br><br><div align=center width=100%>暂无公告</div>";
        }
        lb_gongga.Text = tempStr;
    }

    private void ztbd(DataAccess.DataAccess Access)
    {
        string param = "009";
        string strSql = "select top 1 code,text,defaultPic,sequence,linkOrKind,linkUrl from T_SystemKind where kind='" + param + "' order by listOrder";
        DataTable dt = Access.execSql(strSql);
        if (dt!=null && dt.Rows.Count>0)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            if (dt.Rows[0]["linkOrKind"].ToString() == "0")
            {
                td.Text = "<a href=\"articleList.aspx?kind1=" + dt.Rows[0]["code"].ToString() + "&kind2=&param=" + param + "\"><img src=\"" + new Common.SysConfig().GetValueByKey("ManagementUrl") + "UploadFile/" + param + "/" + dt.Rows[0]["sequence"].ToString() + "/" + dt.Rows[0]["defaultPic"].ToString() + "\" border=0 width=\"890\" height=\"80\"></a>";
            }
            else
            {
                td.Text = "<a href=\""+ dt.Rows[0]["linkUrl"].ToString() +"\" target=\"_blank\"><img src=\"" + new Common.SysConfig().GetValueByKey("ManagementUrl") + "UploadFile/" + param + "/" + dt.Rows[0]["sequence"].ToString() + "/" + dt.Rows[0]["defaultPic"].ToString() + "\" border=0 width=\"890\" height=\"80\"></a>"; 
            }
            tr.Cells.Add(td);
            tb_ztbd.Rows.Add(tr);
        }
    }

    private void wnfw(DataAccess.DataAccess Access)
    {
        string param = "006";
        string strSql = "select top 6 code,text,linkOrKind,linkUrl from T_SystemKind where kind='" + param + "' order by listOrder";
        DataTable dt = Access.execSql(strSql);
        if (dt == null)
        {
            dt = new DataTable();
            DataColumn dc1 = new DataColumn("code");
            DataColumn dc2 = new DataColumn("text");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
        }
        int nCount = dt.Rows.Count;
        for (int i = 0; i < 6 - nCount; i++)
        {
            DataRow dr = dt.NewRow();
            dr["code"] = "";
            dr["text"] = "";
            dt.Rows.Add(dr);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            TableRow tr_space = new TableRow();
            TableCell td_space = new TableCell();
            //td_space.Height = Unit.Pixel(2);
            //td_space.ColumnSpan = 2;
            //tr_space.Cells.Add(td_space);
            //tb_wnfw.Rows.Add(tr_space);

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            string str = "";
            if (dt.Rows[i]["linkOrKind"].ToString() == "0")
            {
                str += "&nbsp;&nbsp;<img src=\"images/dot.gif\" border=\"0\">&nbsp;<a href=\"articleList.aspx?kind1=" + dt.Rows[i]["code"].ToString() + "&kind2=&param=" + param + "\">" + dt.Rows[i]["text"].ToString() + "</a>";
            }
            else
            {
                str += "&nbsp;&nbsp;<img src=\"images/dot.gif\" border=\"0\">&nbsp;<a href=\"" + dt.Rows[i]["linkUrl"].ToString() + "\" target=\"_blank\">" + dt.Rows[i]["text"].ToString() + "</a>";
            }
            td.Text = str;
            td.Height = Unit.Pixel(25);
            td.Width = Unit.Pixel(100);
            //td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            i++;

            if (i < dt.Rows.Count)
            {
                td = new TableCell();
                str = "";
                if (dt.Rows[i]["linkOrKind"].ToString() == "0")
                {
                    str += "&nbsp;&nbsp;<img src=\"images/dot.gif\" border=\"0\">&nbsp;<a href=\"articleList.aspx?kind1=" + dt.Rows[i]["code"].ToString() + "&kind2=&param=" + param + "\">" + dt.Rows[i]["text"].ToString() + "</a>";
                }
                else
                {
                    str += "&nbsp;&nbsp;<img src=\"images/dot.gif\" border=\"0\">&nbsp;<a href=\"" + dt.Rows[i]["linkUrl"].ToString() + "\" target=\"_blank\">" + dt.Rows[i]["text"].ToString() + "</a>";
                }
                td.Text = str;
                td.Height = Unit.Pixel(25);
                td.Width = Unit.Pixel(100);
                //td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);
            }
            else
            {
                td = new TableCell();
                td.Text = "&nbsp;";
                td.Width = Unit.Pixel(100);
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);
            }
            tb_wnfw.Rows.Add(tr);

            tr_space = new TableRow();
            td_space = new TableCell();
            td_space.Attributes.Add("background","images/line.gif");
            td_space.Height = Unit.Pixel(1);
            td_space.ColumnSpan = 2;
            tr_space.Cells.Add(td_space);
            tb_wnfw.Rows.Add(tr_space);
        }
    }

    private void zyfw(DataAccess.DataAccess Access,string currentSubCode)
    {
        string param = "007";
        string strSql = "select top 6 code,text,defaultPic,sequence,linkOrKind,linkUrl from T_SystemKind where kind='" + param + "' and subCode='" + currentSubCode + "' order by listOrder";
        DataTable dt = Access.execSql(strSql);
        if (dt == null)
        {
            dt = new DataTable();
            DataColumn dc1 = new DataColumn("code");
            DataColumn dc2 = new DataColumn("text");
            DataColumn dc3 = new DataColumn("sequence");
            DataColumn dc4 = new DataColumn("defaultPic");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);
        }
        int nCount = dt.Rows.Count;
        for (int i = 0; i < 6 - nCount; i++)
        {
            DataRow dr = dt.NewRow();
            dr["code"] = "";
            dr["text"] = "";
            dr["sequence"] = "";
            dr["defaultPic"] = "";
            dt.Rows.Add(dr);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            TableRow tr_space = new TableRow();
            TableCell td_space = new TableCell();
            td_space.Height = Unit.Pixel(10);
            td_space.ColumnSpan = 2;
            tr_space.Cells.Add(td_space);
            tb_qywh.Rows.Add(tr_space);

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            string str = "";
            if (dt.Rows[i]["defaultPic"].ToString() != "")
            {
                string link = "";
                if (dt.Rows[i]["linkOrKind"].ToString() == "0")
                {
                    link += "<a href=\"articleList.aspx?kind1=" + dt.Rows[i]["code"].ToString() + "&kind2=&param=" + param + "\">";
                }
                else
                {
                    link += "<a href=\"" + dt.Rows[i]["linkUrl"].ToString() + "\" target = \"_blank\">";
                }
                str += "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td width=\"45\">";
                str += link + "<img src=\"" + new Common.SysConfig().GetValueByKey("ManagementUrl") + "uploadFile/" + param + "/" + dt.Rows[i]["sequence"].ToString() + "/" + dt.Rows[i]["defaultPic"].ToString() + "\" border=\"0\" width=\"45\" height=\"45\"></a>";
                str += "</td><td width=\"55\" align=\"center\">";
                str += link + "" + dt.Rows[i]["text"].ToString() + "</a>";
                str += "</td></tr></table>";
            }
            else
            {
                str += "&nbsp;";
            }
            td.Text = str;
            td.Width = Unit.Pixel(100);
            td.Height = Unit.Pixel(45);
            td.HorizontalAlign = HorizontalAlign.Center;
            td.BorderWidth = 1;
            tr.Cells.Add(td);

            i++;

            if (i < dt.Rows.Count)
            {
                td = new TableCell();
                str = "";
                if (dt.Rows[i]["defaultPic"].ToString() != "")
                {
                    string link = "";
                    if (dt.Rows[i]["linkOrKind"].ToString() == "0")
                    {
                        link += "<a href=\"articleList.aspx?kind1=" + dt.Rows[i]["code"].ToString() + "&kind2=&param=" + param + "\">";
                    }
                    else
                    {
                        link += "<a href=\"" + dt.Rows[i]["linkUrl"].ToString() + "\" target = \"_blank\">";
                    }
                    str += "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td width=\"45\">";
                    str += link + "<img src=\"" + new Common.SysConfig().GetValueByKey("ManagementUrl") + "uploadFile/" + param + "/" + dt.Rows[i]["sequence"].ToString() + "/" + dt.Rows[i]["defaultPic"].ToString() + "\" border=\"0\" width=\"45\" height=\"45\"></a>";
                    str += "</td><td width=\"55\" align=\"center\">";
                    str += link + "" + dt.Rows[i]["text"].ToString() + "</a>";
                    str += "</td></tr></table>";
                }
                else
                {
                    str += "&nbsp;";
                }
                td.Text = str;
                td.Width = Unit.Pixel(100);
                td.Height = Unit.Pixel(45);
                td.HorizontalAlign = HorizontalAlign.Center;
                td.BorderWidth = 1;
                tr.Cells.Add(td);
            }
            else
            {
                td = new TableCell();
                td.Text = "&nbsp;";
                td.Width = Unit.Pixel(100);
                td.Height = Unit.Pixel(45);
                td.HorizontalAlign = HorizontalAlign.Center;
                td.BorderWidth = 1;
                tr.Cells.Add(td);
            }
            tb_zyfw.Rows.Add(tr);
        }
    }
    public string GetSubString(string str, int length)
    {
    string temp = str;
    int j = 0;
    int k = 0;
    for (int i = 0; i < temp.Length; i++)
    {
    if (Regex.IsMatch(temp.Substring(i, 1), @"[\u4e00-\u9fa5]+"))
    {
    j += 2;
    }
    else
    {
    j += 1;
    }
    if (j <= length)
    {
    k += 1;
    }
    if (j >= length)
    {
    return temp.Substring(0, k) + "...";
    }
    }
    return temp;
    }
    private void spxw(DataAccess.DataAccess Access)
    {
        string param = "015";
        string strSql = "select top 2 id,articleTitle,auditTime as pubTime from T_VidioNewsList where state=1 and articleType='" + param + "' order by auditTime desc";
        DataTable dt = Access.execSql(strSql);
        if (dt == null)
        {
            dt = new DataTable();
            DataColumn dc1 = new DataColumn("id");
            DataColumn dc2 = new DataColumn("articleTitle");
            DataColumn dc3 = new DataColumn("pubTime");

            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
        }
        int nCount = dt.Rows.Count;
        for (int i = 0; i < 2 - nCount; i++)
        {
            DataRow dr = dt.NewRow();
            dr["id"] = 0;
            dr["articleTitle"] = "";
            dt.Rows.Add(dr);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            TableCell td1 = new TableCell();
            if (dt.Rows[i]["articleTitle"].ToString() != "")
            {
                string tempStr=dt.Rows[i]["articleTitle"].ToString();
                //if (tempStr.Length>14)
                //{
                    tempStr = GetSubString(tempStr,11);
                //}
                    td.Text = "<img src=\"images/dot.gif\">&nbsp;<a href=\"VidioDetail.aspx?kind1=&kind2=&param=" + param + "&Id=" + dt.Rows[i]["Id"].ToString() + "\" target=\"_blank\"" + " title=\"" + dt.Rows[i]["articleTitle"].ToString()+"\">" + tempStr + "</a>";
                td1.Text = "【" + ((DateTime)dt.Rows[i]["pubTime"]).ToString("yyyy-MM-dd") + "】";
                td1.HorizontalAlign = HorizontalAlign.Left;
                td1.Width = Unit.Pixel(150);
            }
            else
            {
                td.Text = "<img src=\"images/dot.gif\">&nbsp;";
            }
            td.Height = Unit.Pixel(20);
            td.Width = Unit.Pixel(200);

            tr.Cells.Add(td);
            tr.Cells.Add(td1);

            TableRow tr_s = new TableRow();
            TableCell td_s = new TableCell();
            td_s.Attributes.Add("background", "images/line.gif");
            td_s.ColumnSpan = 2;
            tr_s.Cells.Add(td_s);
            tb_spxw.Rows.Add(tr);
            tb_spxw.Rows.Add(tr_s);
        }
    }

    private void syyw(DataAccess.DataAccess Access)
    {
        string param = "001";
        string strSql = "select top 9 id,articleTitle,auditTime as pubTime from T_ArticleList where state=1 and articleType='" + param + "' order by auditTime desc";
        DataTable dt = Access.execSql(strSql);
        if (dt == null)
        {
            dt = new DataTable();
            DataColumn dc1 = new DataColumn("id");
            DataColumn dc2 = new DataColumn("articleTitle");
            DataColumn dc3 = new DataColumn("pubTime");

            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
        }
        int nCount = dt.Rows.Count;
        for (int i = 0; i < 9 - nCount; i++)
        {
            DataRow dr = dt.NewRow();
            dr["id"] = 0;
            dr["articleTitle"] = "";
            dt.Rows.Add(dr);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            TableCell td1 = new TableCell();
            if (dt.Rows[i]["articleTitle"].ToString() != "")
            {
                string tempStr=dt.Rows[i]["articleTitle"].ToString();
                if (tempStr.Length>20)
                {
                    tempStr = tempStr.Substring(0, 20) + "...";
                 //   tempStr = GetSubString(tempStr,40);
                }
                td.Text = "<img src=\"images/dot.gif\">&nbsp;<a href=\"articleDetail.aspx?kind1=&kind2=&param=" + param + "&Id=" + dt.Rows[i]["Id"].ToString() + "\" target=\"_blank\"" + " title=\"" + dt.Rows[i]["articleTitle"].ToString()+"\">"  + tempStr + "</a>";
                td1.Text = "【" + ((DateTime)dt.Rows[i]["pubTime"]).ToString("yyyy-MM-dd") + "】";
                td1.HorizontalAlign = HorizontalAlign.Right;
                td1.Width = Unit.Pixel(80);
            }
            else
            {
                td.Text = "<img src=\"images/dot.gif\">&nbsp;";
            }
            td.Height = Unit.Pixel(27);
            td.Width = Unit.Pixel(260);

            tr.Cells.Add(td);
            tr.Cells.Add(td1);

            TableRow tr_s = new TableRow();
            TableCell td_s = new TableCell();
            td_s.Attributes.Add("background", "images/line.gif");
            td_s.ColumnSpan = 2;
            tr_s.Cells.Add(td_s);
            tb_syyw.Rows.Add(tr);
            tb_syyw.Rows.Add(tr_s);
        }
    }

  private void kqfw(DataAccess.DataAccess Access,string currentSubCode)
    {
        string param = "008";
        string strSql = "select top 9 id,articleTitle,auditTime as pubTime from T_ArticleList where state=1 and articleType='" + param + "' order by auditTime desc";
        DataTable dt = Access.execSql(strSql);
        if (dt == null)
        {
            dt = new DataTable();
            DataColumn dc1 = new DataColumn("id");
            DataColumn dc2 = new DataColumn("articleTitle");
            DataColumn dc3 = new DataColumn("pubTime");

            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
        }
        int nCount = dt.Rows.Count;
        for (int i = 0; i < 9 - nCount; i++)
        {
            DataRow dr = dt.NewRow();
            dr["id"] = 0;
            dr["articleTitle"] = "";
            dt.Rows.Add(dr);
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            TableCell td1 = new TableCell();
            if (dt.Rows[i]["articleTitle"].ToString() != "")
            {
                string tempStr = dt.Rows[i]["articleTitle"].ToString();
                if (tempStr.Length > 18)
                {
                    tempStr = tempStr.Substring(0, 18) + "...";
                    //   tempStr = GetSubString(tempStr,40);
                }
                td.Text = "&nbsp;&nbsp;<img src=\"images/dot.gif\">&nbsp;<a href=\"articleDetail.aspx?kind1=&kind2=&param=" + param + "&Id=" + dt.Rows[i]["Id"].ToString() + "\" target=\"_blank\" title=\"" + dt.Rows[i]["articleTitle"].ToString() + "\" >" + tempStr + "</a>";
                td1.Text = "【" + ((DateTime)dt.Rows[i]["pubTime"]).ToString("MM-dd") + "】";
                td1.HorizontalAlign = HorizontalAlign.Right;
                td1.Width = Unit.Pixel(80);
            }
            else
            {
                td.Text = "<img src=\"images/dot.gif\">&nbsp;";
            }
            td.Height = Unit.Pixel(27);
            td.Width = Unit.Pixel(260);

            tr.Cells.Add(td);
            tr.Cells.Add(td1);

            tb_kqfw.Rows.Add(tr);

            TableRow tr_space = new TableRow();
            TableCell td_space = new TableCell();
            td_space.Attributes.Add("background", "images/line.gif");
            td_space.Height = Unit.Pixel(1);
            td_space.ColumnSpan = 2;
            tr_space.Cells.Add(td_space);
            tb_kqfw.Rows.Add(tr_space);
        }
        //string strSql = "select top 10 code,text from T_SystemKind where kind='" + param + "' and subCode='"+currentSubCode+"' and pptr='1' order by listOrder";
        //DataTable dt = Access.execSql(strSql);
        //if (dt == null)
        //{
        //    dt = new DataTable();
        //    DataColumn dc1 = new DataColumn("code");
        //    DataColumn dc2 = new DataColumn("text");
        //    dt.Columns.Add(dc1);
        //    dt.Columns.Add(dc2);
        //}
        //int nCount = dt.Rows.Count;
        //for (int i = 0; i < 10 - nCount; i++)
        //{
        //    DataRow dr = dt.NewRow();
        //    dr["code"] = "";
        //    dr["text"] = "";
        //    dt.Rows.Add(dr);
        //}
        //for (int i = 0; i < dt.Rows.Count; i++)
        //{
        //    TableRow tr_space = new TableRow();
        //    TableCell td_space = new TableCell();
        //    //td_space.Height = Unit.Pixel(2);
        //    //td_space.ColumnSpan = 2;
        //    //tr_space.Cells.Add(td_space);
        //    //tb_wnfw.Rows.Add(tr_space);

        //    TableRow tr = new TableRow();
        //    TableCell td;

        //    td = new TableCell();
        //    string str = "";
        //    str += "&nbsp;&nbsp;<img src=\"images/dot.gif\" border=\"0\">&nbsp;<a href=\"articleList.aspx?kind1=" + dt.Rows[i]["code"].ToString() + "&kind2=&param=" + param + "\">" + dt.Rows[i]["text"].ToString() + "</a>";
        //    td.Text = str;
        //    td.Height = Unit.Pixel(25);
        //    td.Width = Unit.Pixel(310);
        //    //td.HorizontalAlign = HorizontalAlign.Center;
        //    tr.Cells.Add(td);
//*
//            i++;

//            if (i < dt.Rows.Count)
//            {
//                td = new TableCell();
//                str = "";
//                str += "&nbsp;&nbsp;<img src=\"images/dot.gif\" border=\"0\">&nbsp;<a href=\"articleList.aspx?kind1=" + dt.Rows[i]["code"].ToString() + "&kind2=&param=" + param + "\">" + dt.Rows[i]["text"].ToString() + "</a>";
//                td.Text = str;
//                td.Height = Unit.Pixel(25);
//                td.Width = Unit.Pixel(150);
//                //td.HorizontalAlign = HorizontalAlign.Center;
//                tr.Cells.Add(td);
//            }
//            else
//            {
//                td = new TableCell();
//                td.Text = "&nbsp;";
//                td.Width = Unit.Pixel(150);
//                td.HorizontalAlign = HorizontalAlign.Center;
//                tr.Cells.Add(td);
//            }
//*/
//            tb_kqfw.Rows.Add(tr);

//            tr_space = new TableRow();
//            td_space = new TableCell();
//            td_space.Attributes.Add("background", "images/line.gif");
//            td_space.Height = Unit.Pixel(1);
//            td_space.ColumnSpan = 2;
//            tr_space.Cells.Add(td_space);
//            tb_kqfw.Rows.Add(tr_space);
//        }
    }

    private void link(DataAccess.DataAccess Access)
    {
        string strSql = "select linkName,defaultPic,linkUrl,sequence from T_DefaultLink where state=1 order by listOrder";
        DataTable dt = Access.execSql(strSql);
        if (dt!=null && dt.Rows.Count>0)
        {
            TableRow tr = new TableRow();
            for(int i=0;i<dt.Rows.Count;i++)
            {
                TableCell td = new TableCell();
                td.Text = "&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"" + dt.Rows[i]["linkUrl"].ToString() + "\" target=\"_blank\"><img src=\"" + new Common.SysConfig().GetValueByKey("ManagementUrl") + "/uploadFile/defaultLink/" + dt.Rows[i]["sequence"].ToString() + "/" + dt.Rows[i]["defaultPic"].ToString() + "\" height=\"68\" border=\"0\"></a>&nbsp;&nbsp;&nbsp;&nbsp;";
                tr.Cells.Add(td);
            }
            tb_link.Rows.Add(tr);
        }
    }
}
