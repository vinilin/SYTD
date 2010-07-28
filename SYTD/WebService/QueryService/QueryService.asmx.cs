using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

namespace QueryService
{
    /// <summary>
    /// Service1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://localhost/QS")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class Queries : System.Web.Services.WebService
    {

        [WebMethod]
        public string QueryUrl(string ip , int srvType)
        {
            switch (srvType)
            {
                case 1:
                    return "http://localhost/FSM/FileSetMan.asmx";
                    break;
                case 2:
                    return "http://192.168.1.2:50000/";
                    break;
                case 3:
                    return "http:://localhost/";
                    break;
                case 4:
                    return "http:://localhost/";
                    break;
            }
            return "";
        }
    }
}
