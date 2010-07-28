using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class MusicPlay : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string fileName = "", title = "", type="";
        if (Request.QueryString["fileName"] != null)
        {
            fileName = Request.QueryString["fileName"].ToString();
        }
        if (Request.QueryString["title"] != null)
        {
            title = Request.QueryString["title"].ToString();
        }
        if (Request.QueryString["type1"] != null)
        {
            type = Request.QueryString["type1"].ToString();
        }
        bind(fileName, title, type);
    }

    private void bind(string fileName, string title,string type)
    {
        if (type == "playlist")
        {//playlist表示播放列表
            string str = "";
            str += "<object ID=video2 WIDTH=400 HEIGHT=120 CLASSID=CLSID:CFCDAA03-8BE4-11CF-B84B-0020AFBBCCFA>";
            str += "<param name=_ExtentX value=9657> ";
            str += "<param name=_ExtentY value=847> ";
            str += "<param name=AUTOSTART value=-1> ";
            str += "<param name=SHUFFLE value=0> ";
            str += "<param name=PREFETCH value=0> ";
            str += "<param name=NOLABELS value=0> ";
            str += "<param name=SRC value=\"MusicPlayList/" + fileName + "\"> ";
            str += "<param name=CONTROLS value=StatusBar,controlpanel,InfoPanel> ";
            str += "<param name=CONSOLE value=Clip1> ";
            str += "<param name=LOOP value=0> ";
            str += "<param name=NUMLOOP value=0>";
            str += "<param name=CENTER value=0> ";
            str += "<param name=MAINTAINASPECT value=0> ";
            str += "<param name=BACKGROUNDCOLOR value=#000000> ";
            str += "</object> ";
            lbPlayer.Text = str;
            this.Title = title;
        }
        else
        {
            string musicPlayUrl = new Common.SysConfig().GetMusicPlayUrl();
            string str = "";
            str += "<object ID=video2 WIDTH=400 HEIGHT=120 CLASSID=CLSID:CFCDAA03-8BE4-11CF-B84B-0020AFBBCCFA>";
            str += "<param name=_ExtentX value=9657> ";
            str += "<param name=_ExtentY value=847> ";
            str += "<param name=AUTOSTART value=-1> ";
            str += "<param name=SHUFFLE value=0> ";
            str += "<param name=PREFETCH value=0> ";
            str += "<param name=NOLABELS value=0> ";
            str += "<param name=SRC value=\"" + musicPlayUrl + "/" + title + "/" + fileName + "\"> ";
            str += "<param name=CONTROLS value=StatusBar,controlpanel,InfoPanel> ";
            str += "<param name=CONSOLE value=Clip1> ";
            str += "<param name=LOOP value=0> ";
            str += "<param name=NUMLOOP value=0>";
            str += "<param name=CENTER value=0> ";
            str += "<param name=MAINTAINASPECT value=0> ";
            str += "<param name=BACKGROUNDCOLOR value=#000000> ";
            str += "</object> ";
            lbPlayer.Text = str;
            this.Title = title;
        }
    }
}
