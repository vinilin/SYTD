using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebService
{
    /// <summary>
    /// MasterControl 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class MasterControl : System.Web.Services.WebService
    {
        [WebMethod]
        public void CommitMovie(Movie m)
        {
        }

        [WebMethod]
        public void CommitCartoon(Cartoon c)
        {
        }

        [WebMethod]
        public void CommitMusic(Music m)
        {
        }

        [WebMethod]
        public void CommitSoft(Software s)
        {
        }

        [WebMethod]
        public void PublishMovie(long id)
        {
        }

        [WebMethod]
        public void PublishMusic(long id)
        {
        }

        [WebMethod]
        public void PublishSoft(long id)
        {
        }

        [WebMethod]
        public void PublishCartoon(long id)
        {
        }
    }
}
