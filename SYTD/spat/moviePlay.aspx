<%@ Page Language="C#" AutoEventWireup="true" CodeFile="moviePlay.aspx.cs" Inherits="moviePlay" %>

<html>
<SCRIPT LANGUAGE="JavaScript">
<!-- Hide
function killErrors() {

return true;
}
window.onerror = killErrors;


// -->
 </SCRIPT>

<script language="JavaScript" type="text/JavaScript">
<!--
function setFull()
{
	if(!document.playfull.CanStop())
	{
	   alert("ӰƬδ��ʼ�����޷��л�Ϊȫ��ģʽ")
	}
	else
	{
	   alert("���ȷ����ť�����ȫ������ģʽ,��ȫ������ģʽ�а� Esc ���˳�ȫ��ģʽ")
	   document.playfull.SetFullScreen()
	}
}
//-->
</script>
 
 <SCRIPT>
//����ҳ�汣��
    function rf()
      {return false; }
    document.oncontextmenu = rf
    function keydown()
      {if(event.ctrlKey ==true || event.keyCode ==93 || event.shiftKey ==true){return false;} }
      document.onkeydown =keydown
    function drag()
      {return false;}
    document.ondragstart=drag 
function stopmouse(e) { 
		if (navigator.appName == 'Netscape' && (e.which == 3 || e.which == 2))  
 		return false; 
      else if  
      (navigator.appName == 'Microsoft Internet Explorer' && (event.button == 2 || event.button == 3)) {  
 		alert("��ӭ�����--лл��   ����");
		return false;  
 		}
		return true; 
      } 
      document.onmousedown=stopmouse;  
      if (document.layers) 
      window.captureEvents(Event.MOUSEDOWN); 
       window.onmousedown=stopmouse;  

</SCRIPT>

<head id="Head1" runat="server">
<title></title>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
<style type=text/css>
.font1  { color:red; }
body, td, p  { font-size:12px; }
.style1 {color: #0000FF}
</style>
</head>
<body bgcolor="#ffffff" leftMargin='0' topMargin='0' MARGINWIDTH='0' MARGINHEIGHT='0' 
  oncontextmenu='self.event.returnValue=false;' onkeydown='if(event.keyCode==78&&event.ctrlKey)return false;' 
  onselectstart='event.returnValue=false' ondragstart='window.event.returnValue=false'>
  <form runat="server" id="form1">
<table width=496 height=374 border=0 align="center" cellpadding=0 cellspacing=0 bgcolor="#666666">
  <!--DWLayoutTable-->
  <tbody> 
    <tr align="center" valign="top"> 
      <td width="496" height="347"> <table width="496" border="0" cellpadding="0" cellspacing="1">
          <!--DWLayoutTable-->
          <tr> 
            <td width="298" align="center" valign="top" bgcolor="#000000"> <div align="center"> 
                 <script language="javaScript">              
var ERR_NonePlayer="��������ʾ��:����ϵͳ��û�а�װReal Player�����������������ء�\n\nMovie���������رա�";
var ERR_FileNotFind="��ʾ:�����û����࣬���������ƣ������߻��߻��������ŵ�ַ�ۿ���";
var ERR_NotLocateServer="��ʾ:�����û����࣬���������ƣ������߻��߻��������ŵ�ַ�ۿ���";
var ERR_UnkownError="��ʾ:�����û����࣬���������ƣ������߻��߻��������ŵ�ַ�ۿ���";
                </script>
                <script language="VBScript">                                    
on error resume next
RealPlayerG2 = (NOT IsNull(CreateObject("rmocx.RealPlayer G2 Control")))\n'); 
RealPlayer5 = (NOT IsNull(CreateObject("RealPlayer.RealPlayer(tm) ActiveX Control (32-bit)")))
RealPlayer4 = (NOT IsNull(CreateObject("RealVideo.RealVideo(tm) ActiveX Control (32-bit)")))
if not RealPlayerG2 and RealPlayer5 and RealPlayer4 then
//		if MsgBox("����������޷��Զ��������µ������������Ƿ�Ҫ���ز����������ţ�", vbYesNo) = vbYes then
//			window.location = "http://51cd.com/2.exe"
//		end if
     MsgBox("����������޷��Զ��������µ���������,���ֶ�����");
end if

Sub player_OnBuffering(lFlags,lPercentage)
	if (lPercentage=100) then
		StartPlay=false
		if (FirstPlay) then
			FirstPlay=false
			id=player 
		end if	
		exit sub
	end if
End Sub
Sub player_OnErrorMessage(uSeverity, uRMACode, uUserCode, pUserString, pMoreInfoURL, pErrorString)
select case player.GetLastErrorRMACode()
		case -2147221496
			window.alert(ERR_FileNotFind)
		case -2147221433,-2147221428,-2147221417,-2147217468
			window.alert(ERR_NotLocateServer)
		case else
			window.alert(ERR_UnkownError)
	end select
End Sub
                </script>
                <table width="100%" height="170" border="0" align="center" cellpadding="0" cellspacing="0">
                  <tr> 
                    <td>
                    <asp:Label ID="lbReponse" runat="server"></asp:Label>
                    <%--<object id="player" name="player" classid="clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA" width="496" height="300">
                      <param name="AUTOSTART" value="-1">
                      <param name="SHUFFLE" value="0">
                      <param name="PREFETCH" value="0">
                      <param name="NOLABELS" value="0">
                      <param name="CONTROLS" value="Imagewindow">
                      <param name="CONSOLE" value="clip1">
                      <param name="LOOP" value="0">
                      <param name="NUMLOOP" value="0">
                      <param name="CENTER" value="1">
                      <param name=SRC value=rtsp://127.0.0.1/movie/���Ƭ/��/1.RM>
                      <param name="MAINTAINASPECT" value="1">
                      <param name="BACKGROUNDCOLOR" value="#000000">
                    </object>--%>
                      <object ID="RP2" CLASSID="clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA" WIDTH="496" HEIGHT="30">
                        <param name="_ExtentX" value="4657">
                        <param name="_ExtentY" value="794">
                        <param name="AUTOSTART" value="-1">

                        <param name="SHUFFLE" value="0">
                        <param name="PREFETCH" value="0">
                        <param name="NOLABELS" value="-1">
                        <param name="CONTROLS" value="ControlPanel">
                        <param name="CONSOLE" value="clip1">
                        <param name="LOOP" value="0">
                        <param name="NUMLOOP" value="0">
                        <param name="CENTER" value="0">
                        <param name="MAINTAINASPECT" value="0">
                        <param name="BACKGROUNDCOLOR" value="#000000">
                      </object>
                      <object classid=clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA height=30 id="RP3"  name="RP3" width=496>
                        <param name="_ExtentX" value="4657">
                        <param name="_ExtentY" value="794">
                        <param name="AUTOSTART" value="-1">
                        <param name="SHUFFLE" value="0">
                        <param name="PREFETCH" value="0">
                        <param name="NOLABELS" value="-1">
                        <param name="CONTROLS" value="StatusBar">
                        <param name="CONSOLE" value="clip1">
                        <param name="LOOP" value="0">
                        <param name="NUMLOOP" value="0">
                        <param name="CENTER" value="0">
                        <param name="MAINTAINASPECT" value="0">
                        <param name="BACKGROUNDCOLOR" value="#000000">
                      </object></td>
                  </tr>
                </table>
              </div></td>
          </tr>
        </table>
        <div align="right">
          <table width="140" border=0 align="left" cellpadding=0 cellspacing=1 bgcolor="#000000">
            <!--DWLayoutTable-->
            <tbody>
              <tr>
                <td width="395">                      <div align="left">
                    <table width=150 height=25 
      border=1 align="left" cellpadding=0 cellspacing=0 bordercolor=#0000FF>
                      <tbody>
                        <tr>
                          <td width="100%" bordercolor=#00FF00 bgcolor=#009900><div align="left">
                              <table cellspacing=0 cellpadding=0 width=150 
      border=0>
                                <tbody>
                                  <tr>
                                    <td width="496"><div align="center">
                                        <div align="center"><font color="#FFFFFF" style="CURSOR: hand" onclick="document.player.SetFullScreen()">���ȫ������ ��Esc���� </font> </div>
                                            </div>
                              </table>
                          </div></td>
                        </tr>
                      </tbody>
                    </table>
                      </div></td></tr>
            </tbody>
          </table>
          <!--
          <table width="100%" height="25" border="0" cellpadding="0" cellspacing="0">
            <tr>
              <td width="100%" bgcolor="#009A00"><div align="center" class="style1">���޷��տ����������</div></td>
            </tr>
          </table>-->
      </div></td>
                <td width="12" align="center">
                <div align="center"> 
                </div></td>
    </tr>
  </tbody>
</table> 
<asp:Label ID="lbMessage" runat="server"></asp:Label>
</form>
</body>
</html>

<%--
<script language="javascript"> 
player.SetEnableContextMenu(false);
player.SetWantErrors(true);
document.player.DoStop();
document.player.DoPlay();
document.player.SetSource("<%=request.querystring(\"url\")%>");
</script>--%>



