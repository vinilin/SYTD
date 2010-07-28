<%@ Page Language="C#" AutoEventWireup="true" CodeFile="musicShow.aspx.cs" Inherits="musicShow" %>
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
                                                    <td width="10" bgcolor="#dfdfdf"></td>
                                                    <td width="660" align="center" valign="top" >
                                                        <table border="0" width="600">
                                                            <tr>
                                                                <td rowspan=3 width="180" align=center valign=middle><asp:Label ID="lbImg" runat="server"></asp:Label></td>
                                                                <td valign=top>
                                                                <table valign=top width="100%">
                                                                   <tr><td height=5></td></tr>
                                                                    <tr><td height=30 valign=top>专辑：<asp:Label ID="lbTitle" runat="server"></asp:Label></td></tr>
                                                                    <tr>
                                                                       <td height=30 valign=top>歌手：<asp:Label ID="lbSonger" runat="server"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                       <td valign=top>介绍：<asp:Label ID="lbInfo" runat="server"></asp:Label></td>
                                                                    </tr>
                                                                    <tr><td height=5></td></tr>
                                                                </table>
                                                                </td>
                                                           </tr>
                                                            
                                                        </table> 
                                                        <br /><br />
                                                        <asp:Label ID="lbfsPath" runat="server" Text="" Visible="false"></asp:Label>
                                                        <br />
                                                  <!--      
                                                        <asp:Table ID="tbList" runat="server">
                                                               <asp:TableRow>
                                                                    <asp:TableCell HorizontalAlign="Center" VerticalAlign="Middle">序号</asp:TableCell>
                                                                    <asp:TableCell HorizontalAlign="Center" VerticalAlign="Middle">歌曲名</asp:TableCell>
                                                                    <asp:TableCell HorizontalAlign="Center" VerticalAlign="Middle">试听</asp:TableCell>
                                                                    <asp:TableCell HorizontalAlign="Center" VerticalAlign="Middle">播放次数</asp:TableCell>
                                                               </asp:TableRow>
                                                        </asp:Table>
                                                     -->   
                     <table align=center width="520" border="0" cellpadding="0" cellspacing="0">
                      <tr><td align=center colspan=3 >
                      <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" >
                                                            <Columns>
                              <asp:BoundField DataField="ShowIndex" HeaderText="序号">
                                <HeaderStyle BackColor="#EDF6FF" Font-Bold="False" Font-Italic="False" 
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
                                    ForeColor="#003300" Width="48px" Height="30px" HorizontalAlign="Center" 
                                    VerticalAlign="Middle" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                                    Font-Strikeout="False" Font-Underline="False" ForeColor="#003300" 
                                    Height="25px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="选择">
                                <HeaderStyle BackColor="#EDF6FF" Font-Bold="False" Font-Italic="False" 
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
                                    ForeColor="#003300" Width="50px" Height="30px" HorizontalAlign="Center" 
                                    VerticalAlign="Middle" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                                    Font-Strikeout="False" Font-Underline="False" ForeColor="#003300" 
                                    Height="25px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemTemplate>
                                   <asp:CheckBox ID="chboxId" runat="server" />
                                </ItemTemplate>    
                            </asp:TemplateField>
                            <asp:BoundField DataField="FileName" HeaderText="歌曲名称" >
                                <HeaderStyle BackColor="#EDF6FF" Font-Bold="False" Font-Italic="False" 
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
                                    ForeColor="#003300" Width="326px" Height="30px" HorizontalAlign="Center" 
                                    VerticalAlign="Middle" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                                    Font-Strikeout="False" Font-Underline="False" ForeColor="#003300" 
                                    Height="25px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="试听" Visible=false>
                                <HeaderStyle BackColor="#EDF6FF" Font-Bold="False" Font-Italic="False" 
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
                                    ForeColor="#003300" Width="85px" Height="30px" HorizontalAlign="Center" 
                                    VerticalAlign="Middle" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                                    Font-Strikeout="False" Font-Underline="False" ForeColor="#003300" 
                                    Height="25px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemTemplate>
                                  &nbsp;&nbsp;<img src="images/s16.gif" width="20" height="20" alt="" border="0" />&nbsp;&nbsp;</a>
                                </ItemTemplate>    
                            </asp:TemplateField>
                             </Columns>
                                
                           </asp:GridView>
                      </td></tr>
                      <tr>
                        <td width="48" height="30" align="center"><a onclick="cancelAll();" style="cursor:hand">取消</a></td>
                        <td width="50" align="center"><a onclick="selectAll();" style="cursor:hand">全选</a></td>
                        <td width="400" align="right"><asp:Button ID="btnPlay" runat="server" Text="播放选中歌曲" 
                                onclick="btnPlay_Click" /></td>
                        </tr>
                    </table>
                        
                                                        <br />
                                                        <br />
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
<script language="javascript">
    
    function select(Id)
    {
        var   e=document.getElementsByTagName( "input"); 
        for(var   i=0;i <e.length;i++) 
        { 
            if   (e[i].type== "checkbox") 
            { 
                e[i].checked=true; 
            } 
        } 
    }

    function   selectAll() 
    { 
        var   e=document.getElementsByTagName( "input"); 
        for(var   i=0;i <e.length;i++) 
        { 
            if   (e[i].type== "checkbox") 
            { 
                e[i].checked=true; 
            } 
        } 
    } 
    
    function   cancelAll() 
    { 
        var   e=document.getElementsByTagName( "input"); 
        for(var   i=0;i <e.length;i++) 
        { 
            if   (e[i].type== "checkbox") 
            { 
                e[i].checked=false; 
            } 
        }
    } 
    </script>