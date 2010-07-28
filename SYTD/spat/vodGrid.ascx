<%@ Control Language="C#" AutoEventWireup="true" CodeFile="vodGrid.ascx.cs" Inherits="vodGrid" %>


<table cellpadding="0" cellspacing="0" border="0">
   <tr>
      <td>
      <asp:Table ID="table1" Runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0">
          <asp:TableRow>
              <asp:TableCell ID="tableCell1"></asp:TableCell>
          </asp:TableRow>
      </asp:Table>
      </td>
   </tr>
   <tr>
       <td>
         <table id="tablePage"  cellpadding="0" cellspacing="0" width="650" background="Images/Default/xpsj_g.gif">
              <tr>
      			<td align="left" style="width:40%" height="40">
                      <asp:label id="lbPageSummary" Runat="server"></asp:label></td>
                  <td align="right" style="width:60%">
                      <asp:linkbutton id="lbtnFristPage" runat="server" OnClick="lbtnFristPage_Click" CssClass="A1" style="font-family:webdings" Text="9" />
                          &nbsp;
                      <asp:linkbutton id="lbtnPrePage" runat="server" Visible="False" OnClick="lbtnPrePage_Click" CssClass="A1" style="font-family:webdings" Text="3" />&nbsp;
                      <asp:linkbutton id="lbtnPreDivision" runat="server" Visible="False" OnClick="lbtnPreDivision_Click" CssClass="A1">....</asp:linkbutton>&nbsp;
                      <asp:linkbutton id="lbtnPage1" runat="server" Visible="False" OnClick="lbtnPage1_Click" CssClass="A1" style="font-weight:bold;font-family:Arial;" Text="1" />&nbsp;
                      <asp:linkbutton id="lbtnPage2" runat="server" Visible="False" OnClick="lbtnPage2_Click" CssClass="A1"  style="font-weight:bold;font-family:Arial;" Text="2" />&nbsp;
                      <asp:linkbutton id="lbtnPage3" runat="server" Visible="False" OnClick="lbtnPage3_Click" CssClass="A1" style="font-weight:bold;font-family:Arial;" Text="3" />&nbsp;
                      <asp:linkbutton id="lbtnPage4" runat="server" Visible="False" OnClick="lbtnPage4_Click" CssClass="A1" style="font-weight:bold;font-family:Arial;" Text="4" />&nbsp;
                      <asp:linkbutton id="lbtnPage5" runat="server" Visible="False" OnClick="lbtnPage5_Click" CssClass="A1" style="font-weight:bold;font-family:Arial;" Text="5" />&nbsp;
                      <asp:linkbutton id="lbtnPage6" runat="server" Visible="False" OnClick="lbtnPage6_Click" CssClass="A1" style="font-weight:bold;font-family:Arial;" Text="6" />&nbsp;
                      <asp:linkbutton id="lbtnPage7" runat="server" Visible="False" OnClick="lbtnPage7_Click" CssClass="A1" style="font-weight:bold;font-family:Arial;" Text="7" />&nbsp;
                      <asp:linkbutton id="lbtnPage8" runat="server" Visible="False" OnClick="lbtnPage8_Click" CssClass="A1" style="font-weight:bold;font-family:Arial;" Text="8" />&nbsp;
                      <asp:linkbutton id="lbtnPage9" runat="server" Visible="False" OnClick="lbtnPage9_Click" CssClass="A1" style="font-weight:bold;font-family:Arial;" Text="9" />&nbsp;
                      <asp:linkbutton id="lbtnPage10" runat="server" Visible="False" OnClick="lbtnPage10_Click" CssClass="A1" style="font-weight:bold;font-family:Arial;" Text="10" />&nbsp;
                      <asp:linkbutton id="lbtnNextDivision" runat="server" Visible="False" OnClick="lbtnNextDivision_Click" CssClass="A1">....</asp:linkbutton>&nbsp;
                      <asp:linkbutton id="lbtnNextPage" runat="server" Visible="False" OnClick="lbtnNextPage_Click" CssClass="A1" style="font-family:webdings;" Text="4" />&nbsp;
                      <asp:linkbutton id="lbtnLastPage" runat="server"  Enabled="false" OnClick="lbtnLastPage_Click" CssClass="A1" style="font-family:webdings;" Text=":" /></td>
                  <td><img alt="" height="0" src="" width="25" /></td>
              </tr>
          </table>
      </td>
   </tr>
</table>