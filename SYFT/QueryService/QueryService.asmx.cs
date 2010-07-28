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
    public enum ServiceType
    {
        QueryService,
        MasterControl,
        FileSetMan,
        ShareBrowse,
        InterServer,
        FileService
    };
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
        public string QueryUrl(string ip , ServiceType srvType)
        {
            switch (srvType)
            {
                case ServiceType.FileSetMan:
                    return "http://"+ip+"/FSM/FileSetMan.asmx";
                    break;
                case ServiceType.FileService:
                    return "http://"+ip+":50000/";
                    break;
                case ServiceType.ShareBrowse:
                    return "http://"+ip+"/SB/ShareBrowse.asmx";
                    break;
                case ServiceType.InterServer:
                    return "http://localhost/FSM/FileSetMan.asmx";
                default:
                    throw new Exception("无此服务!");
            }
            return "";
        }
        [WebMethod]
        public string QuerySrvIp(string  subCode)
        {
            SYTDDBDataContext context = new SYTDDBDataContext();
            T_SubSection subSection = context.T_SubSection.First(c => c.subCode == subCode);
            return subSection.serverIp;
        }

        [WebMethod]
        public string QueryActInterSrv()
        {
            //return System.Configuration
            return System.Configuration.ConfigurationSettings.AppSettings["ActInterServer"];
        }
    }
}
