using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

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
        public long CreateFileSet (FileSet fset)
        {
            FileSetManDBDataContext context = new FileSetManDBDataContext();
            context.FileSet.InsertOnSubmit(fset);
            context.SubmitChanges();
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
            return null;
        }

        [WebMethod]
        public void DeleteFileSet(long id)
        {
            FileSetManDBDataContext context = new FileSetManDBDataContext();
            var fsets = from fs in context.FileSet where fs.ID == id select fs;
            foreach (FileSet fset in fsets)
            {
                context.FileSet.DeleteOnSubmit(fset);
            }
            context.SubmitChanges();
            return;
        }

        [WebMethod]
        public void CommitFileSet(long id)
        {
            FileSetManDBDataContext context = new FileSetManDBDataContext();
            var fsets = from fs in context.FileSet where fs.ID == id select fs;
            foreach (FileSet fset in fsets)
            {
                fset.Ready = true;
            }
            context.SubmitChanges();
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
