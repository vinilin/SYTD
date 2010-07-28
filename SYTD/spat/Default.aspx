<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register TagPrefix="uc"  TagName="header" Src="~/header.ascx"%>
<%@ Register TagPrefix="uc"  TagName="footer" Src="~/footer.ascx"%>
<html>
<head runat="server">
    <title>石油天地</title>
    <link href="Style.css" type="text/css" rel="Stylesheet" />
    <style>
       body
       {
           background-image: url(images/cbg.gif);
       }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellpadding="0" cellspacing="0" border="0" width="900" align="center">
            <tr>
               <td bgcolor="#ffffff" align="center">
                     <table cellpadding="0" cellspacing="0" border="0" width="896">
                        <tr>
                            <td><!--header-->
                                <uc:header ID="ucHeader" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td height="4"></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table cellpadding="0" cellspacing="0" border="0" width="896">
                                    <tr>
                                        <td valign="top"><!--企业文化-->
                                            <table cellpadding="0" cellspacing="0" width="218" height="260" style="border-width:1px;border-style:solid;border-collapse:collapse;border-color:#dfdfdf">
                                            <tr>
                                                   <td valign="top" align="center" height = "100">
                                                        <table cellpadding="0" cellspacing="0" width="216" border="0" background="images/gg_t.gif">
                                                            <tr>
                                                                <td align="right" valign="middle" height="32"><a href="articleList.aspx?kind1=&kind2=&param=005">更多...</a>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                            </tr>
                                                        </table>
                                                        <table cellpadding="0" cellspacing="0" width="200" border="0">
                                                            <tr>
                                                                <td><asp:Label ID="lb_gongga" runat="server"></asp:Label></td>
                                                            </tr>
                                                        </table>
                                                   </td>
                                            </tr>
                                                <tr>
                                                   <td valign="top" align="center">
                                                        <table cellpadding="0" cellspacing="0" width="216" border="0" background="images/qywh_t.gif">
                                                            <tr>
                                                                <td align="right" valign="middle" height="32"><a href="articleList.aspx?kind1=&kind2=&param=003">更多...</a>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                            </tr>
                                                        </table>
                                                        <asp:Table ID="tb_qywh" runat="server" BorderStyle="none" Height = "100%" CellPadding="0" CellSpacing="0"></asp:Table>
                                                        
                                                   </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="4"></td>
                                        <td valign="top"><!--图片新闻-->
                                            <table cellpadding="0" cellspacing="0" width="450" height="260" style="border-width:1px;border-style:solid;border-collapse:collapse;border-color:#dfdfdf">
                                                <tr>
                                                   <td valign="top" align="center">
                                                        <table cellpadding="0" cellspacing="0" width="448" border="0" background="images/tpxw_t.gif">
                                                            <tr>
                                                                <td align="right" valign="middle" height="32"><a href="articleList.aspx?kind1=&kind2=&param=002">更多...</a>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                            </tr>
                                                        </table>
                                                        <table cellpadding="0" cellspacing="0" width="448" border="0">
                                                            <tr>
                                                                <td colspan="5" height="4"></td>
                                                            </tr>
                                                            <tr>
                                                                <td width="2"></td>
                                                                <td width="200">
                                                                    <asp:Label ID="lb_tpxw" runat="server"></asp:Label>
                                                                </td>
                                                                <td width="4"></td>
                                                                <td width="220">                                                                
                                                                    <asp:Table ID="tb_tpxw" runat="server" BorderStyle="none" CellPadding="0" CellSpacing="0"></asp:Table>
                                                                </td>
                                                                <td width="2"></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="5" height="4"></td>
                                                            </tr>
                                                        </table>
                                                   </td>
                                                </tr>
                                            </table>    
                                        </td>
                                        <td width="4"></td>
                                        <td valign="top"><!--公告-->
                                            <table cellpadding="0" cellspacing="0" width="218" height="260" style="border-width:1px;border-style:solid;border-collapse:collapse;border-color:#dfdfdf">
                                                <tr>
                                                   <td valign="top" align="center">
                                                        <table cellpadding="0" cellspacing="0" width="216" border="0" background="images/spxw.jpg">
                                                            <tr>
                                                                <td align="right" valign="middle" height="32"><a href="VidioList.aspx?kind1=&kind2=&param=015">更多...</a>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                            </tr>
                                                        </table>
                                                       <asp:Label ID="lb_Player" runat="server" Text="Label"></asp:Label>
                                                                <asp:Table ID="tb_spxw" runat="server" BorderStyle="none" CellPadding="0" CellSpacing="0" Width="200"></asp:Table>
                                                   </td>
                                                </tr>
                                            </table>    
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="4"></td>
                        </tr>
                        <tr>
                            <td align="center"><!--专题报道-->
                                <asp:Table ID="tb_ztbd" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0"></asp:Table>
                            </td>
                        </tr>
                        <tr>
                            <td height="4"></td>
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0" border="0" width="896" >
                                    <tr>
                                        <td valign="top"><!--为你服务-->
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                   <td>
                                                        <table cellpadding="0" cellspacing="0" width="218" height="118" style="border-width:1px;border-style:solid;border-collapse:collapse;border-color:#dfdfdf">
                                                            <tr>
                                                               <td valign="top" align="center">
                                                                    <table cellpadding="0" cellspacing="0" width="216" border="0" background="images/wnfw_t.gif">
                                                                        <tr>
                                                                            <td align="right" valign="middle" height="32"><a href="articleList.aspx?kind1=&kind2=&param=006">更多...</a>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                                        </tr>
                                                                    </table>
                                                                    <asp:Table ID="tb_wnfw" runat="server" BorderStyle="none" CellPadding="0" CellSpacing="0"></asp:Table>
                                                               </td>
                                                            </tr>
                                                        </table>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                    <td height="2"></td>
                                                 </tr>
                                                 <tr>
                                                    <td>           
                                                        <table cellpadding="0" cellspacing="0" width="218" height="180" style="border-width:1px;border-style:solid;border-collapse:collapse;border-color:#dfdfdf">
                                                            <tr>
                                                               <td valign="top" align="center">
                                                                    <table cellpadding="0" cellspacing="0" width="216" border="0" background="images/zyfw_t.gif">
                                                                        <tr>
                                                                            <td align="right" valign="middle" height="32"><a href="articleList.aspx?kind1=&kind2=&param=007">更多...</a>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                                        </tr>
                                                                    </table>
                                                                    <asp:Table ID="tb_zyfw" runat="server" BorderStyle="none" CellPadding="0" CellSpacing="0"></asp:Table>
                                                               </td>
                                                            </tr>
                                                        </table>  
                                                   </td>
                                                </tr>
                                            </table>                 
                                        </td>
                                        <td width="4"></td>
                                        <td valign="top"><!--石油要闻-->
                                            <table cellpadding="0" cellspacing="0" width="350" height="300" style="border-width:1px;border-style:solid;border-collapse:collapse;border-color:#dfdfdf">
                                                <tr>
                                                   <td valign="top" align="center">
                                                        <table cellpadding="0" cellspacing="0" width="348" border="0" background="images/syyw_t.gif">
                                                            <tr>
                                                                <td align="right" valign="middle" height="32"><a href="articleList.aspx?kind1=&kind2=&param=001">更多...</a>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                            </tr>
                                                        </table>
                                                        <asp:Table ID="tb_syyw" runat="server" BorderStyle="none" CellPadding="0" CellSpacing="0" Width="340"></asp:Table>
                                                   </td>
                                                </tr>
                                            </table>   
                                        </td>
                                        <td width="4"></td>
                                        <td valign="top"><!--矿区服务-->
                                            <table cellpadding="0" cellspacing="0" width="318" height="300" style="border-width:1px;border-style:solid;border-collapse:collapse;border-color:#dfdfdf">
                                                <tr>
                                                   <td valign="top" align="left">
                                                        <table cellpadding="0" cellspacing="0" width="316" border="0" background="images/kqfw_t.gif">
                                                            <tr>
                                                                <td align="right" valign="middle" height="32"><a href="articleList.aspx?kind1=&kind2=&param=008">更多...</a>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                            </tr>
                                                        </table>
                                                        <asp:Table ID="tb_kqfw" runat="server" BorderStyle="none" CellPadding="0" CellSpacing="0"></asp:Table>
                                                   </td>
                                                </tr>
                                            </table>   
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="4"></td>
                        </tr>
                        <tr>
                            <td><hr /></td>
                        </tr>
                        <tr>
                            <td align="center"><!--链接-->
                                <asp:Table ID="tb_link" runat="server" BorderStyle="none" CellPadding="0" CellSpacing="0"></asp:Table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc:footer ID="ucFooter" runat="server" />
                            </td>
                        </tr>
                    </table>
               </td>
            </tr>
    </div>
    <asp:Label ID="lbMessage" runat="server"></asp:Label>
    </form>
</body>
</html>
