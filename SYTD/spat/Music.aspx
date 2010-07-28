<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Music.aspx.cs" Inherits="Music" %>
<%@ Register TagPrefix="uc"  TagName="header" Src="~/header.ascx"%>
<%@ Register TagPrefix="uc"  TagName="footer" Src="~/footer.ascx"%>
<%@ Register TagPrefix="uc" TagName="musicGrid" Src="~/musicGrid.ascx" %>
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
                            <td align="center">
                                <table cellpadding="0" cellspacing="0" width="890" style="border-width:1px;border-style:solid;border-collapse:collapse;border-color:#dfdfdf">
                                    <tr>
                                        <td>
                                            <table  cellpadding="0" cellspacing="0" width="890" border="0">
                                                <tr>
                                                    <td width="220" align="center" valign="top" height="600">
                                                       <asp:Table ID="tb_kind" runat="server" Width="200" BorderWidth="0"></asp:Table>
                                                       <br />
                                                       <br />
                                                       <table cellpadding="0" cellspacing="0" style="border-width:1px;border-style:solid;border-collapse:collapse;border-color:#dfdfdf">
                                                         <tr>
                                                             <td width="200" height="30" valign="bottom" background="images/zxzx_left_t.gif" class="STYLE4">
                                                                 &nbsp;&nbsp;<img src="images/icon_1.gif" width="20" height="20" align=absbottom>
                                                                 音乐排行
                                                             </td>
                                                         </tr>
                                                         <tr>
                                                             <td>
                                                                <asp:Table ID="tablePlayTimes" runat="server" cellspacing="1" cellpadding="0" width="200" BorderWidth="0">
                                                                </asp:Table>
                                                             </td>
                                                             <td bgcolor="#95B5DC" height="1"></td>
                                                         </tr>
                                                         </table>
                                                    </td>
                                                    <td width="6" bgcolor="#dfdfdf"></td>
                                                    <td width="660" align="center" valign="top"  height="600">
                                                        <uc:musicGrid ID="ucGrid" runat="server" />
                                                    </td>
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
