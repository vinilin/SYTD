<%@ Control Language="C#" AutoEventWireup="true" CodeFile="header.ascx.cs" Inherits="header" %>
<table cellpadding="0" cellspacing="0" width="898" style="border-width:0px;border-collapse:collapse;">
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0" width="898" background="images/topbg.gif" style="border-width:0px;border-collapse:collapse;">
                <tr>
                    <td width="205"><img src="images/logo.gif" /></td>
                    <td width="690" align="right">
                        <asp:Label ID="lbSubList" runat="server"></asp:Label>&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td align="center">
            <img alt="" src="images/banner.jpg" width="894" />
        </td>    
    </tr>
    <tr>
        <td height="35" valign="middle" background="images/dht.gif">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td align="center" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td align="center" valign="middle"><a href="default.aspx"><font color="#FFFFFF"><b>首 页</b></font></a></td>
                    <td align="center" valign="middle"><font color="#FFFFFF"><b>&nbsp; | &nbsp;</b></td>
                    <td align="center" valign="middle"><a href="ArticleList.aspx?kind1=&kind2=&param=004"><font color="#FFFFFF"><b>技术资讯</b></font></a> </td>
                    <td align="center" valign="middle"><font color="#FFFFFF"><b>&nbsp; | &nbsp;</b></td>
                    <td align="center" valign="middle"><a href="Redirect.aspx?Page=Movie.aspx"><font color="#FFFFFF"><b>石油影院</b></font></a></td>
                    <td align="center" valign="middle"><font color="#FFFFFF"><b>&nbsp; | &nbsp;</b></td>
                    <td align="center" valign="middle"><a href="Redirect.aspx?Page=Music.aspx"><font color="#FFFFFF"><b>音乐欣赏</b></font></a></td>
                    <td align="center" valign="middle"><font color="#FFFFFF"><b>&nbsp; | &nbsp;</b></td>
                    <td align="center" valign="middle"><a href="Redirect.aspx?Page=Cartoon.aspx"><font color="#FFFFFF"><b>动漫欣赏</b></font></a></td>
                    <td align="center" valign="middle"><font color="#FFFFFF"><b>&nbsp; | &nbsp;</b></td>
                    <td align="center" valign="middle"><a href="Redirect.aspx?Page=Soft.aspx"><font color="#FFFFFF"><b>软件下载</b></font></a></td>
                    
                    <td align="center" valign="middle">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td align="right" valign="middle">
                         <font color="#FFFFFF">
                        <script language=JavaScript>
                            today=new Date();
                            function initArray()
                            {this.length=initArray.arguments.length
                            for(var i=0;i<this.length;i++)
                            this[i+1]=initArray.arguments[i]}
                            var d=new initArray(
                            "<font color=red>星期日</font>",
                            "星期一",
                            "星期二",
                            "星期三",
                            "星期四",
                            "星期五",
                            "<font color=red>星期六</font>");
                            document.write(
                            "<font style='font-size:9pt;font-family:宋体'> ",
                            today.getYear(),"年",
                            today.getMonth()+1,"月",
                            today.getDate(),"日 ",
                            d[today.getDay()+1],
                            "</font>");
                            </script>
                        </font>
                        </td>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                </tr>
            </table>
        </td>    
    </tr>
</table>