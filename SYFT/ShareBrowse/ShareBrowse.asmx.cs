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
    [WebService(Namespace = "http://localhost/SB")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class ShareBrowse : System.Web.Services.WebService
    {
        [WebMethod]
        public void PublishMovie(BaseItem baseInfo, Movie mvInfo,PublishType pubType,long fsId)
        {
            ShareBrowseDBDataContext context = new ShareBrowseDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                PublishBase(baseInfo, fsId,pubType, context);
                context.Movie.InsertOnSubmit(mvInfo);
                context.SubmitChanges();
                context.Transaction.Commit();
            }
            catch(Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
        }

        [WebMethod]
        public void PublishVidioNews(BaseItem baseInfo, VidioNews vnInfo,PublishType pubType,long fsId)
        {
            ShareBrowseDBDataContext context = new ShareBrowseDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                PublishBase(baseInfo, fsId,pubType, context);
                context.VidioNews.InsertOnSubmit(vnInfo);
                context.SubmitChanges();
                context.Transaction.Commit();
            }
            catch(Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
        }
        private void PublishBase(BaseItem baseInfo, long fsId, PublishType pubType,ShareBrowseDBDataContext context)
        {
            context.BaseItem.InsertOnSubmit(baseInfo);
            var type = from t in context.PublishType where t.ID == pubType.ID select t;
            if(type.Count() < 1)
            {
                context.PublishType.InsertOnSubmit(pubType);
            }
            context.SubmitChanges();
            FileItem fl = new FileItem
                            {
                                ID = baseInfo.ID,
                                FileSetID = fsId
                            };
            context.FileItem.InsertOnSubmit(fl);
            context.SubmitChanges();
        }
        [WebMethod]
        public void PublishCartoon(BaseItem baseInfo,Cartoon c,PublishType pubType,long fsId)
        {
            ShareBrowseDBDataContext context = new ShareBrowseDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                PublishBase(baseInfo, fsId, pubType,context);
                context.Cartoon.InsertOnSubmit(c);
                context.SubmitChanges();
                context.Transaction.Commit();
            }
            catch(Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
        }

        [WebMethod]
        public void PublishSoft(BaseItem baseInfo,Software s,PublishType pubType,long fsId)
        {
            ShareBrowseDBDataContext context = new ShareBrowseDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                PublishBase(baseInfo, fsId, pubType,context);
                context.Software.InsertOnSubmit(s);
                context.SubmitChanges();
                context.Transaction.Commit();
            }
            catch(Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
        }

        [WebMethod]
        public void PublishMusic(BaseItem baseInfo,Music m, PublishType pubType,long fsId)
        {
            ShareBrowseDBDataContext context = new ShareBrowseDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                PublishBase(baseInfo, fsId, pubType,context);
                context.Music.InsertOnSubmit(m);
                context.SubmitChanges();
                context.Transaction.Commit();
            }
            catch(Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
        }

        [WebMethod]
        public void RemovePublish(long id)
        {
            ShareBrowseDBDataContext context = new ShareBrowseDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                // 删除FileItem
                var fiTmp = from f in context.FileItem where f.ID == id select f;
                foreach (FileItem fi in fiTmp)
                {
                    context.FileItem.DeleteOnSubmit(fi);
                }
                // 删除电影信息
                var mvTmp = from f in context.Movie where f.ID == id select f;
                foreach (Movie fi in mvTmp)
                {
                    context.Movie.DeleteOnSubmit(fi);
                }
                // 删除音乐信息

                var muTmp = from f in context.Music where f.ID == id select f;
                foreach (Music fi in muTmp)
                {
                    context.Music.DeleteOnSubmit(fi);
                }

                // 删除软件信息
                var soTmp = from f in context.Software where f.ID == id select f;
                foreach (Software fi in soTmp)
                {
                    context.Software.DeleteOnSubmit(fi);
                }

                // 删除动漫信息
                var caTmp = from f in context.Cartoon where f.ID == id select f;
                foreach (Cartoon fi in caTmp)
                {
                    context.Cartoon.DeleteOnSubmit(fi);
                }

                var vnTmp = from f in context.VidioNews where f.ID == id select f;
                foreach (VidioNews fi in vnTmp)
                {
                    context.VidioNews.DeleteOnSubmit(fi);
                }
                context.SubmitChanges();
                var baItem = from f in context.BaseItem where f.ID == id select f;
                foreach (BaseItem ba in baItem)
                {
                    context.BaseItem.DeleteOnSubmit(ba);
                }
                context.SubmitChanges();
                context.Transaction.Commit();
            }
            catch(Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
            
        }

        [WebMethod]
        public void UpdateMoive()
        {
        }
        [WebMethod]
        public void UpdateMusic()
        {
        }
        [WebMethod]
        public void UpdateSoft()
        {
        }
        [WebMethod]
        public void UpdateCartton()
        {
        }
    }
}
