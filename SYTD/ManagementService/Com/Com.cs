using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using FluorineFx;

namespace ManagementService.Com
{
    [RemotingService]
    public class Com
    {
        public static string picUrl = "SYTDManagement-debug/ewebEditor/uploadFile/";

        public static string checkSql(string str)
        {
            return str.Replace("'", "''");
        }


        public bool CreateDir(string type, string sequence)
        {
            bool result = false;
            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath(new Common.SysConfig().GetValueByKey("webUrl") + "UploadFile/" + type + "/" + sequence);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch { }
            return result;
        }

        public bool DeleteDir(string type, string sequence)
        {
            bool result = false;
            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath(new Common.SysConfig().GetValueByKey("webUrl") + "UploadFile/" + type + "/" + sequence);
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            catch { }
            return result;
        }
    }
}
