using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Net;

namespace FileSetMan
{
    /// <summary>
    /// FileSetMan 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://localhost/FSM")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class FileSetMan : System.Web.Services.WebService
    {
        [WebMethod]
        public long CreateFileSet (FileSet fset, bool needProgress)
        {
            FileSetManDBDataContext context = new FileSetManDBDataContext();
            context.FileSet.InsertOnSubmit(fset);
            context.SubmitChanges();
            /*
            if (needProgress)
            {
                ShareManDBDataContext sm = new ShareManDBDataContext();
                Progress p = new Progress();
                p.downloaded = 0;
                p.total = 0;
                p.FileSetID = fset.ID;
                p.State = "等待中";
                IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0];
                p.ServerIp = ipAddr.ToString();
                sm.Progress.InsertOnSubmit(p);
                sm.SubmitChanges();
            }
             * */
            return fset.ID;
        }

        [WebMethod]
        public FileSet QureyFileSet(long id)
        {
            FileSetManDBDataContext context = new FileSetManDBDataContext();
            var fsets = from fs in context.FileSet where fs.ID == id select fs;
            foreach (FileSet fset in fsets)
            {
                return fset;
            }
            throw new Exception("无此文件集");
        }

        [WebMethod]
        public void DeleteFileSet(long id)
        {
            FileSetManDBDataContext context = new FileSetManDBDataContext();
            var files = from f in context.File where f.FileSetID == id select f;
            foreach (File f in files)
            {
                context.File.DeleteOnSubmit(f);
            }
            var fsets = from fs in context.FileSet where fs.ID == id select fs;
            foreach (FileSet fset in fsets)
            {
                context.FileSet.DeleteOnSubmit(fset);
            }
            context.SubmitChanges();

            ShareManDBDataContext sm = new ShareManDBDataContext();
            var tmp = from pro in sm.Progress where pro.FileSetID == id select pro;
            foreach (Progress pro in tmp)
            {
                sm.Progress.DeleteOnSubmit(pro);
            }
            sm.SubmitChanges();
            return;
        }

        [WebMethod]
        public int CommitFileSet(long id)
        {
            FileSetManDBDataContext context = new FileSetManDBDataContext();
            var fsets = from fs in context.FileSet where fs.ID == id select fs;
            foreach (FileSet fset in fsets)
            {
                fset.Ready = true;
            }
            context.SubmitChanges();
            ShareManDBDataContext sm = new ShareManDBDataContext();
            var tmp = from pro in sm.Progress where pro.FileSetID == id select pro;
            foreach (Progress pro in tmp)
            {
                pro.State = "已完成";
                pro.FinishDate = DateTime.Now;
            }
            sm.SubmitChanges();
            return 0;
        }

        [WebMethod]
        public int ProgressNotify(long id, long total, long downloaded)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            Progress p = new Progress();
            p.downloaded = downloaded;
            p.total = total;
            p.FileSetID = id;
            var tmp = from pro in context.Progress where pro.FileSetID == id select pro;
            foreach (Progress pro in tmp)
            {
                pro.downloaded = downloaded;
                pro.total = total;
                pro.State = "下载中";
            }
            context.SubmitChanges();
            return 0;
        }

        [WebMethod]
        public void CommitFile(long fsid , long fid)
        {
        }

        [WebMethod]
        public void CommitError(string msg)
        {

        }
    }
}
