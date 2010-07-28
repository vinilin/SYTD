using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;

namespace ShareMan
{
    /// <summary>
    /// MasterControl 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://localhost/SM")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class MasterControl : System.Web.Services.WebService
    {
        [WebMethod]
        public long CommitMovie(
            BaseItem baseInfo,Movie movieInfo, FSM.FileSet fset,string srcIp)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                movieInfo.ID = CommitBase(baseInfo, ref fset,srcIp,context);
                context.Movie.InsertOnSubmit(movieInfo);
                context.SubmitChanges();
                context.Transaction.Commit();
            }
            catch (System.Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
            return baseInfo.ID;
        }

        [WebMethod]
        public long CommitVidioNews(
            BaseItem baseInfo,VidioNews vidioInfo, FSM.FileSet fset,string srcIp)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                vidioInfo.ID = CommitBase(baseInfo, ref fset,srcIp,context);
                context.VidioNews.InsertOnSubmit(vidioInfo);
                context.SubmitChanges();
                context.Transaction.Commit();
            }
            catch (System.Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
            return baseInfo.ID;
        }
        [WebMethod]
        public FSM.FileSet QueryFileSet(long id)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            var itemLink = from i in context.ItemLink where i.ID == id select i;  
            foreach (ItemLink il in itemLink)
            {
                var fsl = from f in context.FileSetLink where f.ID == il.InterLink select f;
                foreach (FileSetLink sl in fsl)
                {
                    FSM.FileSetMan fsm = new ShareMan.FSM.FileSetMan();
                    QS.Queries qs = new ShareMan.QS.Queries();
                    fsm.Url = qs.QueryUrl(sl.IP, QS.ServiceType.FileSetMan);
                    return fsm.QureyFileSet(sl.FileSetID);
                }
            }
            return null;
        }
        [WebMethod]
        public FSM.FileSet QueryFileSetOnSrc(long id)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            var itemLink = from i in context.ItemLink where i.ID == id select i;  
            foreach (ItemLink il in itemLink)
            {
                var fsl = from f in context.FileSetLink where f.ID == il.SrcLink select f;
                foreach (FileSetLink sl in fsl)
                {
                    FSM.FileSetMan fsm = new ShareMan.FSM.FileSetMan();
                    QS.Queries qs = new ShareMan.QS.Queries();
                    fsm.Url = qs.QueryUrl(sl.IP, QS.ServiceType.FileSetMan);
                    return fsm.QureyFileSet(sl.FileSetID);
                }
            }
            return null;
        }

        [WebMethod]
        public void Audit(long id, int rst,string reson, string audiMan)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                var tmp = from a in context.Audit where a.ID == id select a;
                foreach (Audit au in tmp)
                {
                    if(!string.IsNullOrEmpty(reson))
                        au.Reason = reson;
                    au.State = rst;
                    if(!string.IsNullOrEmpty(audiMan))
                        au.AuditOwner = audiMan;
                    au.AuditDate = DateTime.Now;
                }
                context.SubmitChanges();
                context.Transaction.Commit();
            }
            catch (System.Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
        }

        [WebMethod]
        public long CommitCartoon(
            BaseItem baseInfo,Cartoon cartoonInfo, FSM.FileSet fset,string srcIp)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                cartoonInfo.ID = CommitBase(baseInfo,ref fset,srcIp,context);
                context.Cartoon.InsertOnSubmit(cartoonInfo);
                context.SubmitChanges();
                context.Transaction.Commit();
            }
            catch (System.Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
            return baseInfo.ID;
        }

        [WebMethod]
        public long CommitMusic(
            BaseItem baseInfo,Music musicInfo, FSM.FileSet fset,string srcIp)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                musicInfo.ID = CommitBase(baseInfo,ref fset,srcIp,context);
                context.Music.InsertOnSubmit(musicInfo);
                context.SubmitChanges();
                context.Transaction.Commit();
            }
            catch (System.Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
            return baseInfo.ID;
        }

        [WebMethod]
        public long CommitSoft(
            BaseItem baseInfo,Software softwarInfo, FSM.FileSet fset,string srcIp)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                softwarInfo.ID = CommitBase(baseInfo,ref fset,srcIp,context);
                context.Software.InsertOnSubmit(softwarInfo);
                context.SubmitChanges();
                context.Transaction.Commit();
            }
            catch (System.Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
            return baseInfo.ID;
        }

        [WebMethod]
        public void RemoveMovie(long id)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                RemoveBase(id, context);
                var tmp1 = context.Movie.First(c => c.ID == id);
                context.Movie.DeleteOnSubmit(tmp1);
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
        public void RemoveCartoon(long id)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                RemoveBase(id, context);
                var tmp1 = context.Cartoon.First(c => c.ID == id);
                context.Cartoon.DeleteOnSubmit(tmp1);
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
        public void RemoveSoft(long id)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                RemoveBase(id, context);
                var tmp1 = context.Software.First(c => c.ID == id);
                context.Software.DeleteOnSubmit(tmp1);
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
        public void RemoveVidioNews(long id)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                RemoveBase(id, context);
                var tmp1 = context.VidioNews.First(c => c.ID == id);
                context.VidioNews.DeleteOnSubmit(tmp1);
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
        public void RemoveMusic(long id)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                RemoveBase(id, context);
                var tmp1 = context.Music.First(c => c.ID == id);
                context.Music.DeleteOnSubmit(tmp1);
                context.SubmitChanges();
                context.Transaction.Commit();
            }
            catch(Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
        }

        public void RemoveBase(long id, ShareManDBDataContext context)
        {
            QS.Queries qs = new QS.Queries();
            // 停止中转服务器文件传输
            StopTrans(qs.QueryActInterSrv());
            // 删除中转服务器上相关文件
            //DeleteFiles(,qs.QueryActInterSrv());
            // 清除服务器上的文件集信息
            DeleteFileSet(id,context);
            // 清楚已经发布的数据
            RemovePublish(id, context);
            // 清除其他相关信息
            CleanInfos(id, context);
        }
        public void RemovePublish(long id, ShareManDBDataContext context)
        {
            SB.ShareBrowse sb = new ShareMan.SB.ShareBrowse();
            QS.Queries qs = new ShareMan.QS.Queries();
            var subSite = from p in context.Distribute where p.ItemID == id select p;
            foreach (Distribute d in subSite)
            {
                // 查询发布站点IP
                var sub = context.T_SubSection.First(c => c.subCode == d.SubCode);
                sb.Url = qs.QueryUrl(sub.serverIp, QS.ServiceType.ShareBrowse);
                sb.RemovePublish(id);
            }
        }

        public void CleanInfos(long id, ShareManDBDataContext context)
        {
            Audit tmp = context.Audit.First(c => c.ID == id);
            context.Audit.DeleteOnSubmit(tmp);

            var tmp3 = context.BaseItem.First(c => c.ID == id);
            context.BaseItem.DeleteOnSubmit(tmp3);
        }

        public void StopTrans(string srvIp)
        {
            Trans ts = new Trans();
            QS.Queries qs = new QS.Queries();
            ts.Url = qs.QueryUrl(srvIp, QS.ServiceType.FileService);
            // 停止中转服务器上的任务
            ts.Stop();
        }

        public void DeleteFiles(long id, string srvIp)
        {
            Trans ts = new Trans();
            QS.Queries qs = new QS.Queries();
            ts.Url = qs.QueryUrl(srvIp, QS.ServiceType.FileService);
            // 停止中转服务器上的任务
            ts.Delete(id.ToString());
        }

        public void DeleteFileSet(long id, ShareManDBDataContext context)
        {
            FSM.FileSetMan fsm = new ShareMan.FSM.FileSetMan();
            QS.Queries qs = new QS.Queries();
            var il = from l in context.ItemLink where l.ID == id select l;
            foreach (ItemLink it in il)
            {
                long[] ids = {it.InterLink, it.SrcLink};
                var flink = from f in context.FileSetLink where ids.Contains(f.ID)  select f;
                foreach (FileSetLink fl in flink)
                {
                    fsm.Url = qs.QueryUrl(fl.IP, QS.ServiceType.FileSetMan);
                    // 先清除文件
                    DeleteFiles(fl.FileSetID, fl.IP);
                    // 在清除数据
                    fsm.DeleteFileSet(fl.FileSetID);
                    context.FileSetLink.DeleteOnSubmit(fl);
                }
                context.ItemLink.DeleteOnSubmit(it);
            }
        }

        [WebMethod]
        public void UpdateFileSet(long id, FSM.FileSet fs,string srcIp)
        {
            QS.Queries qs = new QS.Queries();
            // 停止中转服务器文件传输
            StopTrans(qs.QueryActInterSrv());
            // 删除中转服务器上相关文件
            //DeleteFiles(fs.ID,qs.QueryActInterSrv());
            // 清除服务器上的文件集信息
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                DeleteFileSet(id,context);
                FSM.FileSet tmp = CreateFileSet(id, fs, srcIp, context);
                TransFileSet("",srcIp,tmp);
            }
            catch (System.Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
        }

        public void TransFileSet(string srcIp,string dstIp, FSM.FileSet fset)
        {
            Trans ts = new Trans();
            QS.Queries qs = new QS.Queries();
            ts.Url = qs.QueryUrl(dstIp, QS.ServiceType.FileService);
            Position pos = new Position();
            pos.ip = srcIp;
            ts.Transform(Convert(fset),pos);
            ts.Start();
        }

        public FSM.FileSet CreateFileSet(long id, FSM.FileSet fset, string srcIp,ShareManDBDataContext context)
        {
            // 開始創建文件集
            FSM.FileSetMan fsm = new ShareMan.FSM.FileSetMan();
            QS.Queries qs = new ShareMan.QS.Queries();
//            qs.Url = "http://localhost/QS/QueryService.asmx";

            // 查詢WebService服務地址
            fsm.Url = qs.QueryUrl(srcIp, QS.ServiceType.FileSetMan);
            FileSetLink fsSrc = new FileSetLink();

            //在源站点创建文件集
            fsSrc.FileSetID = fsm.CreateFileSet(fset, true);
            fsSrc.IP = srcIp;

            FileSetLink fsInter = new FileSetLink();
            // 在中转服务器上创建文件集信息
            fsm.Url = qs.QueryUrl("", QS.ServiceType.InterServer);
            fsInter.FileSetID = fsm.CreateFileSet(fset,true);
            // 查询中转服务器上创建的文件集信息，并保存，用于启动传输任务
            return fsm.QureyFileSet(fsInter.FileSetID);
            //fsInter.IP = "";
        }

        [WebMethod]
        public void UpdateMovie(BaseItem baseInfo,Movie movieInfo)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                UpdateBase(baseInfo, context);
                var mv = from b in context.Movie where b.ID == baseInfo.ID select b;
                foreach (Movie b in mv)
                {
                    b.Player = movieInfo.Player;
                    b.Director = movieInfo.Director;
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
        public void UpdateMusic(BaseItem baseInfo,Music musicInfo)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                UpdateBase(baseInfo,context);
                var mu = from b in context.Music where b.ID == baseInfo.ID select b;
                foreach (Music b in mu)
                {
                    b.Author= musicInfo.Author;
                    b.Singer = musicInfo.Singer;
                }
                context.SubmitChanges();
                context.Transaction.Commit();
            }
            catch
            {
                context.Transaction.Rollback();
            }
            
        }


        [WebMethod]
        public void UpdateCartoon(BaseItem baseInfo,Cartoon cartoonInfo)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                UpdateBase(baseInfo, context);
                var mv = from b in context.Cartoon where b.ID == baseInfo.ID select b;
                foreach (Cartoon b in mv)
                {
                    b.Author = cartoonInfo.Author;
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
        public void UpdateSoft(BaseItem baseInfo,Software softInfo)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                UpdateBase(baseInfo, context);
                var mv = from b in context.Software where b.ID == baseInfo.ID select b;
                foreach (Software b in mv)
                {
                    b.Manufacturer= softInfo.Manufacturer;
                    b.Version = softInfo.Version;
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
        public void UpdateVidioNews(BaseItem baseInfo,VidioNews vidioInfo)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                UpdateBase(baseInfo, context);
                var vn = from b in context.VidioNews where b.ID == baseInfo.ID select b;
                foreach (VidioNews b in vn)
                {
                    b.Context = vidioInfo.Context;
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

        public void UpdateBase(BaseItem baseInfo, ShareManDBDataContext context)
        {
            var bi = from b in context.BaseItem where b.ID == baseInfo.ID select b;
            foreach (BaseItem baseItem in bi)
            {
                baseItem.Ext1 = baseInfo.Ext1;
                baseItem.Ext2 = baseInfo.Ext2;
                baseItem.Ext3 = baseInfo.Ext3;
                baseItem.Ext4 = baseInfo.Ext4;
                baseItem.Ext5 = baseInfo.Ext5;
                baseItem.Ext6 = baseInfo.Ext6;
                baseItem.Ext7 = baseInfo.Ext7;
                baseItem.Ext8 = baseInfo.Ext8;
                baseItem.PublishType = baseInfo.PublishType;
                baseItem.Brief = baseInfo.Brief;
                baseItem.IssueDate = baseInfo.IssueDate;
            }
            context.SubmitChanges();
        }


        private void Audit(long id, int rst)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                var tmp = from a in context.Audit where a.ID == id select a;
                foreach (Audit au in tmp)
                {
                    au.State = rst;
                    au.AuditDate = DateTime.Now;
                }
                context.SubmitChanges();
                context.Transaction.Commit();
            }
            catch (System.Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }

        }

        [WebMethod]
        public void PublishMovie(long id,string [] dst)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                // 查找相关文件链接
                ItemLink il = context.ItemLink.First(c => c.ID == id);
                FileSetLink fsl = context.FileSetLink.First(c => c.ID == il.InterLink);
                // 查询文件集信息
                FSM.FileSetMan fsm = new ShareMan.FSM.FileSetMan();
                QS.Queries qs = new ShareMan.QS.Queries();
                // 中转服务器ip
                string actIp = qs.QueryActInterSrv();

                fsm.Url = qs.QueryUrl(qs.QueryActInterSrv(), QS.ServiceType.FileSetMan);
                FSM.FileSet fs = fsm.QureyFileSet(fsl.FileSetID);
                // 发布
                foreach (string subCode in dst)
                {
                    string ip = qs.QuerySrvIp(subCode);
                    fsm.Url = qs.QueryUrl(ip, QS.ServiceType.FileSetMan);
                    long fsid = fsm.CreateFileSet(fs, true);
                    FSM.FileSet tmpFs = fsm.QureyFileSet(fsid);

                    // 查询基本信息
                    BaseItem baseItem = context.BaseItem.First(c => c.ID == id);
                    // 查询电影信息
                    Movie movieInfo = context.Movie.First(c => c.ID == id);

                    PublishType pubType = context.PublishType.First(c => c.ID == baseItem.PublishType);
                    SB.ShareBrowse sb = new SB.ShareBrowse();
                    string url = qs.QueryUrl(ip, QS.ServiceType.ShareBrowse);
                    sb.Url = url;
                    sb.PublishMovie(this.Convert(baseItem), this.Convert(movieInfo), this.Convert(pubType), fsid);
                    CreateProgress(baseItem, actIp, ip, fsid, "发布", context);
                    TransFileSet(actIp, ip, tmpFs);
                    Distribute trbut = new Distribute
                    {
                        SubCode = subCode,
                        ItemID = id
                    };
                    context.Distribute.InsertOnSubmit(trbut);
                    context.SubmitChanges();
                    Audit(id, 2);
                    context.Transaction.Commit();
                }
            }
            catch (System.Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
        }

        [WebMethod]
        public void PublishVidioNews(long id,string [] dst)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                // 查找相关文件链接
                ItemLink il = context.ItemLink.First(c => c.ID == id);
                FileSetLink fsl = context.FileSetLink.First(c => c.ID == il.InterLink);
                // 查询文件集信息
                FSM.FileSetMan fsm = new ShareMan.FSM.FileSetMan();
                QS.Queries qs = new ShareMan.QS.Queries();
                // 中转服务器ip
                string actIp = qs.QueryActInterSrv();

                fsm.Url = qs.QueryUrl(qs.QueryActInterSrv(), QS.ServiceType.FileSetMan);
                FSM.FileSet fs = fsm.QureyFileSet(fsl.FileSetID);
                // 发布
                foreach (string subCode in dst)
                {
                    string ip = qs.QuerySrvIp(subCode);
                    fsm.Url = qs.QueryUrl(ip, QS.ServiceType.FileSetMan);
                    long fsid = fsm.CreateFileSet(fs, true);
                    FSM.FileSet tmpFs = fsm.QureyFileSet(fsid);

                    // 查询基本信息
                    BaseItem baseItem = context.BaseItem.First(c => c.ID == id);
                    // 查询电影信息
                    VidioNews vnInfo = context.VidioNews.First(c => c.ID == id);

                    PublishType pubType = context.PublishType.First(c => c.ID == baseItem.PublishType);
                    SB.ShareBrowse sb = new SB.ShareBrowse();
                    string url = qs.QueryUrl(ip, QS.ServiceType.ShareBrowse);
                    sb.Url = url;
                    sb.PublishVidioNews(this.Convert(baseItem), this.Convert(vnInfo), this.Convert(pubType), fsid);
                    CreateProgress(baseItem, actIp, ip, fsid, "发布", context);
                    TransFileSet(actIp, ip, tmpFs);
                    Distribute trbut = new Distribute
                    {
                        SubCode = subCode,
                        ItemID = id
                    };
                    context.Distribute.InsertOnSubmit(trbut);
                    context.SubmitChanges();
                    Audit(id, 2);
                    context.Transaction.Commit();
                }
            }
            catch (System.Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
        }
        [WebMethod]
        public void PublishMusic(long id,string [] dst)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            // 查找相关文件链接
            //var link = from l in context.ItemLink where l.ID == id select l;
            ItemLink il  = context.ItemLink.First(c => c.ID == id);
            if (il == null)
            {
                throw new ArgumentException("il is null" + id.ToString());
            }
            //var fslink = from f in context.FileSetLink where f.ID == il.SrcLink select f;
            FileSetLink fsl = context.FileSetLink.First(c => c.ID == il.InterLink);
            if (fsl == null)
            {
                throw new ArgumentException("fsl is null" + il.SrcLink.ToString());
            }
            // 查询文件集信息
            FSM.FileSetMan fsm = new ShareMan.FSM.FileSetMan();
            QS.Queries qs = new ShareMan.QS.Queries();

            string actIp = qs.QueryActInterSrv();

            fsm.Url = qs.QueryUrl(qs.QueryActInterSrv(),QS.ServiceType.FileSetMan);
            FSM.FileSet fs = fsm.QureyFileSet(fsl.FileSetID);
            if(fs == null)
            {
                throw new ArgumentException("fs is null"+fsl.FileSetID.ToString());
            }


            // 发布
            foreach (string subCode in dst)
            {
                string ip = qs.QuerySrvIp(subCode);
                fsm.Url = qs.QueryUrl(ip, QS.ServiceType.FileSetMan);
                long fsid = fsm.CreateFileSet(fs,true);
                FSM.FileSet tmpFs = fsm.QureyFileSet(fsid);

                // 查询基本信息
                BaseItem baseItem = context.BaseItem.First(c => c.ID == id);
                // 查询电影信息
                Music musicInfo = context.Music.First(c => c.ID == id);
                PublishType pubType = context.PublishType.First(c => c.ID == baseItem.PublishType);

                SB.ShareBrowse sb = new SB.ShareBrowse();
                string url = qs.QueryUrl(ip,QS.ServiceType.ShareBrowse);
                sb.Url = url;
                sb.PublishMusic(this.Convert(baseItem), this.Convert(musicInfo), this.Convert(pubType),fsid);
                CreateProgress(baseItem, actIp, ip, fsid, "发布", context);
                TransFileSet(qs.QueryActInterSrv(), ip,tmpFs);
                Distribute trbut = new Distribute
                {
                    SubCode = subCode,
                    ItemID = id
                };
                context.Distribute.InsertOnSubmit(trbut);
                context.SubmitChanges();
                Audit(id, 2);
            }
        }

        [WebMethod]
        public void PublishSoft(long id, string[] dst)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            // 查找相关文件链接
            ItemLink il  = context.ItemLink.First(c => c.ID == id);

            FileSetLink fsl = context.FileSetLink.First(c => c.ID == il.InterLink);
            // 查询文件集信息
            FSM.FileSetMan fsm = new ShareMan.FSM.FileSetMan();
            QS.Queries qs = new ShareMan.QS.Queries();

            string actIp = qs.QueryActInterSrv();
            fsm.Url = qs.QueryUrl(qs.QueryActInterSrv(),QS.ServiceType.FileSetMan);
            FSM.FileSet fs = fsm.QureyFileSet(fsl.FileSetID);

            // 发布
            foreach (string subCode in dst)
            {
                string ip = qs.QuerySrvIp(subCode);
                fsm.Url = qs.QueryUrl(ip, QS.ServiceType.FileSetMan);
                long fsid = fsm.CreateFileSet(fs,true);
                FSM.FileSet tmpFs = fsm.QureyFileSet(fsid);

                // 查询基本信息
                BaseItem baseItem = context.BaseItem.First(c => c.ID == id);
                // 查询电影信息
                Software softInfo = context.Software.First(c => c.ID == id);
                PublishType pubType = context.PublishType.First(c => c.ID == baseItem.PublishType);

                SB.ShareBrowse sb = new SB.ShareBrowse();
                //sb.Url = "http://localhost/SB/ShareBrowse.asmx";
                string url = qs.QueryUrl(ip,QS.ServiceType.ShareBrowse);
                sb.Url = url;
                sb.PublishSoft(this.Convert(baseItem), this.Convert(softInfo), this.Convert(pubType),fsid);
                CreateProgress(baseItem, actIp, ip, fsid, "发布", context);
                TransFileSet(qs.QueryActInterSrv(), ip,tmpFs);
                Distribute trbut = new Distribute
                {
                    SubCode = subCode,
                    ItemID = id
                };
                context.Distribute.InsertOnSubmit(trbut);
                context.SubmitChanges();
                Audit(id, 2);
            }

        }

        [WebMethod]
        public void PublishCartoon(long id, string[] dst)
        {
            ShareManDBDataContext context = new ShareManDBDataContext();
            // 查找相关文件链接
            //var link = from l in context.ItemLink where l.ID == id select l;
            ItemLink il  = context.ItemLink.First(c => c.ID == id);

            //var fslink = from f in context.FileSetLink where f.ID == il.SrcLink select f;
            FileSetLink fsl = context.FileSetLink.First(c => c.ID == il.InterLink);
            // 查询文件集信息
            FSM.FileSetMan fsm = new ShareMan.FSM.FileSetMan();
            QS.Queries qs = new ShareMan.QS.Queries();

            string actIp = qs.QueryActInterSrv();
            fsm.Url = qs.QueryUrl(qs.QueryActInterSrv(),QS.ServiceType.FileSetMan);
            FSM.FileSet fs = fsm.QureyFileSet(fsl.FileSetID);

            // 发布
            foreach (string subCode in dst)
            {
                string ip = qs.QuerySrvIp(subCode);
                fsm.Url = qs.QueryUrl(ip, QS.ServiceType.FileSetMan);
                long fsid = fsm.CreateFileSet(fs,true);
                FSM.FileSet tmpFs = fsm.QureyFileSet(fsid);

                // 查询基本信息
                BaseItem baseItem = context.BaseItem.First(c => c.ID == id);
                PublishType pubType = context.PublishType.First(c => c.ID == baseItem.PublishType);
                // 查询电影信息
                Cartoon cartoonInfo = context.Cartoon.First(c => c.ID == id);

                SB.ShareBrowse sb = new SB.ShareBrowse();
                string url = qs.QueryUrl(ip,QS.ServiceType.ShareBrowse);
                sb.Url = url;
                sb.PublishCartoon(this.Convert(baseItem), this.Convert(cartoonInfo),this.Convert(pubType), fsid);
                CreateProgress(baseItem, actIp, ip, fsid, "发布", context);
                TransFileSet(qs.QueryActInterSrv(), ip,tmpFs);
                Distribute trbut = new Distribute
                {
                    SubCode = subCode,
                    ItemID = id
                };
                context.Distribute.InsertOnSubmit(trbut);
                context.SubmitChanges();
                Audit(id, 2 );
            }
        }

        private ArrayOfFile Convert(FSM.FileSet fset)
        {
            ArrayOfFile fs = new ArrayOfFile();
            fs.id = fset.ID.ToString();
            fs.path = fset.Path;
            fs.status = 0;
            fs.item = new File[fset.File.Length];
            for (int i = 0; i < fset.File.Count(); ++i)
            {
                fs.item[i] = Convert(fset.File[i]);
            }
            return fs;
        }
        private File Convert(FSM.File file)
        {
            return new File
            {
                id = file.ID.ToString(),
                name = file.FileName,
                status =0 
            };
        }
        private SB.PublishType Convert(PublishType pubType)
        {
            SB.PublishType p = new ShareMan.SB.PublishType
            {
                ID = pubType.ID,
                Category = pubType.Category,
                Name = pubType.Name
            };
            return p;
        }
        private SB.VidioNews Convert(VidioNews vInfo)
        {
            SB.VidioNews v = new ShareMan.SB.VidioNews
            {
                ID = vInfo.ID,
                Context = vInfo.Context
            };
            return v;
        }
        private SB.Music Convert(Music mInfo)
        {
            SB.Music m = new ShareMan.SB.Music
            {
                ID = mInfo.ID,
                Author = mInfo.Author,
                Singer = mInfo.Singer
            };
            return m;
        }
        private SB.Cartoon Convert(Cartoon cInfo)
        {
            SB.Cartoon ct = new ShareMan.SB.Cartoon
            {
                ID = cInfo.ID,
                Author = cInfo.Author
            };
            return ct;
        }
        private SB.Software Convert(Software sInfo)
        {
            SB.Software sw = new ShareMan.SB.Software
            {
                ID = sInfo.ID,
                Version = sInfo.Version,
                Manufacturer = sInfo.Manufacturer
            };
            return sw;
        }
        private SB.Movie Convert(Movie movInfo)
        {
            SB.Movie mv = new ShareMan.SB.Movie
            {
                ID = movInfo.ID,
                Director = movInfo.Director,
                Player = movInfo.Player
            };
            return mv;
        }
        private SB.BaseItem Convert(BaseItem baseItem)
        {
            SB.BaseItem sbbi = new ShareMan.SB.BaseItem
            {
                ID = baseItem.ID,
                IssueDate = baseItem.IssueDate,
                Owner = baseItem.Owner,
                Title = baseItem.Title,
                PublishType = baseItem.PublishType,
                Birth = baseItem.Birth,
                Brief = baseItem.Brief,
                BrowseCount = 0,
                Category = baseItem.Category,
                Ext1 = baseItem.Ext1,
                Ext2 = baseItem.Ext2,
                Ext3 = baseItem.Ext3,
                Ext4 = baseItem.Ext4,
                Ext5 = baseItem.Ext5,
                Ext6 = baseItem.Ext6,
                Ext7 = baseItem.Ext7,
                Ext8 = baseItem.Ext8,
            };
            return sbbi;
        }

        private long CommitBase(BaseItem baseInfo, 
            ref FSM.FileSet fset,string srcIp,ShareManDBDataContext context)
        {
            // 插入基本信息
            baseInfo.Ext4 = fset.File.Length.ToString();
            context.BaseItem.InsertOnSubmit(baseInfo);
            context.SubmitChanges();
            // 插入审核信息
            Audit ad = new Audit
            {
                ID = baseInfo.ID,
                AuditDate = DateTime.Now,
                State = 0 
            };
            context.Audit.InsertOnSubmit(ad);

            // 開始創建文件集
            FSM.FileSetMan src = new ShareMan.FSM.FileSetMan();
            QS.Queries qs = new ShareMan.QS.Queries();
            qs.Url ="http://localhost/QS/QueryService.asmx" ;

            // 查詢WebService服務地址
            src.Url = qs.QueryUrl(srcIp, QS.ServiceType.FileSetMan);
            FileSetLink fsSrc = new FileSetLink();
            //在源站点创建文件集
            fsSrc.FileSetID = src.CreateFileSet(fset,false);
            fsSrc.IP = srcIp;
            /*
             * 此处查询当前活动的中转服务器
             * 
             * */
            FileSetLink fsInter = new FileSetLink();
            FSM.FileSetMan inter = new ShareMan.FSM.FileSetMan();

            // 在中转服务器上创建文件集信息
            fsInter.IP = qs.QueryActInterSrv();
            inter.Url = qs.QueryUrl(fsInter.IP, QS.ServiceType.FileSetMan);
            fsInter.FileSetID = inter.CreateFileSet(fset,true);
            // 创建文件集进度信息
            CreateProgress(baseInfo,srcIp,fsInter.IP,fsInter.FileSetID,"上传",context);

            // 查询中转服务器上创建的文件集信息，并保存，用于启动传输任务
            fset = inter.QureyFileSet(fsInter.FileSetID);

            // 准备传输文件集
            Trans fsrv = new Trans();
            fsrv.Url = qs.QueryUrl(qs.QueryActInterSrv(), QS.ServiceType.FileService);
            Position pos = new Position();
            pos.ip = srcIp;
            fsrv.Transform(Convert(fset),pos);
            fsrv.Start();
            // 提交更改
            context.FileSetLink.InsertOnSubmit(fsSrc);
            context.FileSetLink.InsertOnSubmit(fsInter);
            context.SubmitChanges();

            // 创建文件链接信息
            ItemLink iLink = new ItemLink();
            iLink.ID = baseInfo.ID;
            iLink.SrcLink = fsSrc.ID;
            iLink.InterLink = fsInter.ID;
            context.ItemLink.InsertOnSubmit(iLink);
            context.SubmitChanges();
            return baseInfo.ID;
        }
        private void CreateProgress(BaseItem baseInfo, 
                        string srcIp, 
                        string dstIp, 
                        long fsIdOnDst, 
                        string extInfo,
                        ShareManDBDataContext context)
        {
            Progress progress = new Progress();
            progress.DstIp = dstIp;
            progress.SrcIp = srcIp;
            progress.FileSetID = fsIdOnDst;
            progress.CmmitDate = baseInfo.Birth;
            progress.downloaded = 0;
            progress.total = 0;
            //progress.AuditMan = auditMan;
            progress.State = "等待中";
            progress.ItemId = baseInfo.ID;
            progress.Title = baseInfo.Title;
            progress.Owner = baseInfo.Owner;
            switch(baseInfo.Category)
            {
                case 1:
                    progress.Category = "电影";
                    break;
                case 2:
                    progress.Category = "音乐";
                    break;
                case 3:
                    progress.Category = "软件";
                    break;
                case 4:
                    progress.Category = "动漫";
                    break;
            }
            progress.Ext1 = extInfo;
            context.Progress.InsertOnSubmit(progress);
            context.SubmitChanges();
        }
    }
}
