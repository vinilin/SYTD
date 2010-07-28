using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

namespace ShareBrowse
{
    /// <summary>
    /// ShareBrowse 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class ShareBrowse : System.Web.Services.WebService
    {
        [WebMethod]
        public void PublishMovie(BaseItem baseInfo, Movie mvInfo,long fsId)
        {
            ShareBrowseDBDataContext context = new ShareBrowseDBDataContext();
        }

        [WebMethod]
        public void PublishCartoon(BaseItem baseInfo,Cartoon c)
        {
        }

        [WebMethod]
        public void PublishSoft(BaseItem baseInfo,Software s)
        {
        }

        [WebMethod]
        public void PublishMusic(BaseItem baseInfo,Music m)
        {
        }

    }
}
