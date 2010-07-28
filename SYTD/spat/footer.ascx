<%@ Control Language="C#" AutoEventWireup="true" CodeFile="footer.ascx.cs" Inherits="footer" %>
<table  cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="center"><!--分站列表-->
            <table  cellpadding="0" cellspacing="0" width="894" style="border-width:0px;border-collapse:collapse;" background="images/foot_b.gif">
                <tr>
                    <td align="center" valign="middle" height="31">
                           <asp:Label ID="lbSubList" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td height="100" bgcolor="F2F1F6">
               <table width="415" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                         <td height="20" align="center">版权所有©西南油气田通信公司</td>
                    </tr>
                    <tr>
                        <td height="20" align="center">客户及咨询热线：115，管理员E-mail：<a href="mailto:qing82131@yahoo.com.cn">qing82131@yahoo.com.cn</a></td>
                    </tr>
                    <tr>
                        <td height="23" align="center"><img src="images/jsq.gif" width="25" height="23">今日访问：<asp:Label id="lbCurrentCount" runat="server" /> 总访问：<asp:Label id="lbBrowseCount" runat="server" /></td>
                    </tr>
                    <tr>
                        <td height="20" align="center">版权所有，未经授权禁止转载、摘编、复制或建立镜象，否则将追究法律责任</td>
                    </tr>
            </table>
        </td>
    </tr>
</table>