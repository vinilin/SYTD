<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Predownload.aspx.cs" Inherits="Predownload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>   
    <table style="width:100%;">
        <tr>
   
    <asp:Repeater ID="Repeater1" runat="server">
        <HeaderTemplate>
            <table style="width:100%;">
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
              <%# Eval("value") %>  
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    
    </div>
    </form>
</body>
</html>
