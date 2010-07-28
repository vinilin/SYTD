<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SoftShow.aspx.cs" Inherits="SoftShow" %>

<%@ Register TagPrefix="uc"  TagName="header" Src="~/header.ascx"%>
<%@ Register TagPrefix="uc"  TagName="footer" Src="~/footer.ascx"%>
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
                               <table border="0" width="900" cellpadding="0" cellspacing="0">
                                   <tr>
                                       <td width="10">&nbsp;</td>
                                       <td width="360" valign="top" align="center"><asp:Label ID="lbImg" runat="server"></asp:Label></td>
                                       <td width="10">&nbsp;</td>
                                       <td width="480" valign="top">
                                           <table  cellpadding="2" cellspacing="1" bgcolor="#95B5DC" width="100%">
                                               <tr>
                                                   <td height="46"  background="images/s11.gif" bgcolor="#FFFFFF">&nbsp;&nbsp;<asp:Label ID="lbName" runat=server CssClass="STYLE5"></asp:Label></td>
                                               </tr>
                                               <tr>
                                                   <td height="25" align="left" bgcolor="#FFFFFF"><b>软件版本：</b><asp:Label ID="lbVersion" runat=server></asp:Label></td>
                                               </tr>
                                               <tr>
                                                   <td height="25" align="left" bgcolor="#FFFFFF"><b>发布公司：</b><asp:Label ID="lbManufacturer" runat=server></asp:Label></td>
                                               </tr>
                                               <tr>
                                                   <td height="25" align="left" bgcolor="#FFFFFF"><b>界面语言：</b><asp:Label ID="lbLanguage" runat=server></asp:Label></td>
                                               </tr>
                                               <tr>
                                                   <td height="25" align="left" bgcolor="#FFFFFF"><b>下载次数：</b><asp:Label ID="lbBrowseCount" runat=server></asp:Label></td>
                                               </tr>
                                               <tr>
                                                   <td align="left" bgcolor="#FFFFFF" valign="top"><b>软件介绍：</b><br /><asp:Label ID="lbIntro" runat=server></asp:Label></td>
                                               </tr>
                                           </table>
                                       </td>
                                   </tr>
                               </table>
                            </td>
                        </tr>
                     </table>     
                     <asp:Label ID="lbfsPath" runat="server" Text="" Visible="false"></asp:Label>
                     <table align=center width="520" border="0" cellpadding="0" cellspacing="0">
                      <tr><td align=center colspan=3 >
                      <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                              onrowdatabound="GridView1_RowDataBound" >
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
                            <asp:BoundField DataField="FileName" HeaderText="文件名" >
                                <HeaderStyle BackColor="#EDF6FF" Font-Bold="False" Font-Italic="False" 
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
                                    ForeColor="#003300" Width="326px" Height="30px" HorizontalAlign="Center" 
                                    VerticalAlign="Middle" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                                    Font-Strikeout="False" Font-Underline="False" ForeColor="#003300" 
                                    Height="25px" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle BackColor="#EDF6FF" Font-Bold="False" Font-Italic="False" 
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
                                    ForeColor="#003300" Width="85px" Height="30px" HorizontalAlign="Center" 
                                    VerticalAlign="Middle" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                                    Font-Strikeout="False" Font-Underline="False" ForeColor="#003300" 
                                    Height="25px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemTemplate>
                                <!--
                                  &nbsp;&nbsp;<img src="images/s16.gif" width="20" height="20" alt="" border="0"  />&nbsp;&nbsp;</a>
                                  -->
                                  <asp:HyperLink ID="HyperLink1"  runat="server"  Text="下载"></asp:HyperLink>
                                </ItemTemplate>    
                            </asp:TemplateField>
                             </Columns>
                                
                           </asp:GridView>
                      </td></tr>
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