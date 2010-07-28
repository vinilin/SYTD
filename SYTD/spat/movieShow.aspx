<%@ Page Language="C#" AutoEventWireup="true" CodeFile="movieShow.aspx.cs" Inherits="movieShow" %>

<%@ Register TagPrefix="uc"  TagName="header" Src="~/header.ascx"%>
<%@ Register TagPrefix="uc"  TagName="footer" Src="~/footer.ascx"%>
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
                                                   <td height="25" align="left" bgcolor="#FFFFFF"><b>主演：</b><asp:Label ID="lbActor" runat=server></asp:Label></td>
                                               </tr>
                                               <tr>
                                                   <td height="25" align="left" bgcolor="#FFFFFF"><b>导演：</b><asp:Label ID="lbDirector" runat=server></asp:Label></td>
                                               </tr>
                                               <tr>
                                                   <td height="25" align="left" bgcolor="#FFFFFF"><b>语言：</b><asp:Label ID="lbLanguage" runat=server></asp:Label></td>
                                               </tr>
                                               <tr>
                                                   <td height="25" align="left" bgcolor="#FFFFFF"><b>类别：</b><asp:Label ID="lbMovieType" runat=server></asp:Label></td>
                                               </tr>
                                               <tr>
                                                   <td height="25" align="left" bgcolor="#FFFFFF"><b>集数：</b><asp:Label ID="lbMovieNum" runat=server></asp:Label></td>
                                               </tr>
                                               <!--
                                               <tr>
                                                   <td height="25" align="left" bgcolor="#FFFFFF"><b>播放时长：</b><asp:Label ID="lbMovieLong" runat=server></asp:Label></td>
                                               </tr>
                                               -->
                                               <tr>
                                                   <td height="25" align="left" bgcolor="#FFFFFF"><asp:Label ID="lbPlay" runat="server"></asp:Label></td>
                                               </tr>
                                               <tr>
                                                   <td align="left" bgcolor="#FFFFFF" valign="top"><b>局情介绍：</b><br /><asp:Label ID="lbIntro" runat=server></asp:Label></td>
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
    function winOpen(Id,num)
    {
        window.open ('moviePlay.aspx?Id='+Id+'&num='+num, 'newwindow', 'height=390, width=500, top=150, left=250, toolbar=no, menubar=no, scrollbars=no, resizable=no,location=n o, status=no');        
    }
</script>