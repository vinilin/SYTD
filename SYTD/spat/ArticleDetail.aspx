<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ArticleDetail.aspx.cs" Inherits="ArticleDetail" %>
<%@ Register TagPrefix="uc"  TagName="header" Src="~/header.ascx"%>
<%@ Register TagPrefix="uc"  TagName="footer" Src="~/footer.ascx"%>
<html>
<head id="Head1" runat="server">
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
                            <td height="40" valign="middle" align="center">
                                <table cellpadding="0" cellspacing="0" width="890" style="border-width:1px;border-style:solid;border-collapse:collapse;border-color:#dfdfdf">
                                    <tr>
                                        <td  height="30" valign="middle">
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbPath" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0" width="896" style="border-width:1px;border-style:solid;border-collapse:collapse;border-color:#dfdfdf">
                                    <tr>
                                        <td align="center">
                                            <table cellpadding="0" cellspacing="0" width="90%">
                                                <tr>
                                                   <td height="20"></td>
                                                </tr>
                                                <tr>
                                                   <td align="center"><asp:Label ID="lbTitle" runat="server" Font-Bold="True" 
                                                           Font-Size="Large"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                   <td height="10"></td>
                                                </tr>
                                                <tr>
                                                   <td align="center">发布日期：<asp:Label ID="lbPubTime" runat="server"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                   <td height="20"><hr size="1" width="190%"/></td>
                                                </tr>
                                                <tr>
                                                   <td>
                                                        <asp:Label ID="lbContent" runat="server"></asp:Label>
                                                        <asp:Label ID="lbSource" runat="server"></asp:Label>
                                                   </td>
                                                </tr>
                                                <tr>
                                                   <td height="30"></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                     </table>
               </td>
            </tr>
            <tr>
                <td>
                    <uc:footer ID="ucFooter" runat="server" />
                </td>
            </tr>
         </table>               
    </div>
    <asp:Label ID="lbMessage" runat="server"></asp:Label>
    </form>
</body>
</html>
