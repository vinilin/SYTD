using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ShareMan
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
        public void CommitMovie(
            BaseItem baseInfo,Movie movieInfo, FSM.FileSet fset,string srcIp)
        {
            ShareManDataContext context = new ShareManDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                movieInfo.ID = CommitBase(baseInfo, ref fset,srcIp,context);
                context.Movie.InsertOnSubmit(movieInfo);
                // 这里将文件列表发送到指定的服务器中
                Trans srv = new Trans();
                Position pos = new Position();
                QS.Queries qs = new ShareMan.QS.Queries();
                srv.Url = qs.QueryUrl("", 2);
                ArrayOfFile ff = Convert(fset);
                srv.Transform(ff, pos);
                context.Transaction.Commit();
            }
            catch (System.Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
        }

        [WebMethod]
        public void CommitCartoon(
            BaseItem baseInfo,Cartoon cartoonInfo, FSM.FileSet fset,string srcIp)
        {
            ShareManDataContext context = new ShareManDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                cartoonInfo.ID = CommitBase(baseInfo,ref fset,srcIp,context);
                context.Cartoon.InsertOnSubmit(cartoonInfo);
                context.Transaction.Commit();
            }
            catch (System.Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
        }

        [WebMethod]
        public void CommitMusic(
            BaseItem baseInfo,Music musicInfo, FSM.FileSet fset,string srcIp)
        {
            ShareManDataContext context = new ShareManDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                musicInfo.ID = CommitBase(baseInfo,ref fset,srcIp,context);
                context.Music.InsertOnSubmit(musicInfo);
                context.Transaction.Commit();
            }
            catch (System.Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
        }

        [WebMethod]
        public void CommitSoft(
            BaseItem baseInfo,Software softwarInfo, FSM.FileSet fset,string srcIp)
        {
            ShareManDataContext context = new ShareManDataContext();
            try
            {
                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();
                softwarInfo.ID = CommitBase(baseInfo,ref fset,srcIp,context);
                context.Software.InsertOnSubmit(softwarInfo);
                context.Transaction.Commit();
            }
            catch (System.Exception e)
            {
                context.Transaction.Rollback();
                throw e;
            }
        }

        [WebMethod]
        public void PublishMovie(long id,long[] area)
        {
            ShareManDataContext context = new ShareManDataContext();
            // 查找相关文件链接
            var link = from l in context.ItemLink where l.ID == id select l;
            foreach (ItemLink il in link)
            {
                var fslink = from f in context.FileSetLink where f.ID == il.SrcLink select f;
                FileSetLink fsl = fslink.ElementAt(0);
                FSM.FileSetMan fsm = new ShareMan.FSM.FileSetMan();
                /*
                 * 组装webService Url
                 */
                // fsm.Url = ......
                // 查询FileSet
                FSM.FileSet fs = fsm.QureyFileSet(fsl.FileSetID);
                long fsid = fsm.CreateFileSet(fs);
                // 查询基本信息
                var baseItems = from b in context.BaseItem where b.ID == id select b;
                BaseItem baseItem = baseItems.ElementAt(0);
                // 查询电影信息
                var movInfos = from m in context.Movie where m.ID == id select m;
                Movie movInfo = movInfos.ElementAt(0);

                // 调用WebService 发布到分站
                SB.ShareBrowse sb = new ShareMan.SB.ShareBrowse();
                /*
                 * 设置sb url
                 * */
                sb.PublishMovie(this.Convert(baseItem), this.Convert(movInfo), fsid);
                // 调用WebService 下发文件下载列表
                // .....
                Trans srv = new Trans();
                //srv.Transform();
            }
            //Distribute dis = new Distribute();
            //dis.ItemID = baseInfo.ID;
            //dis.AreaID = arId;
            //context.Distribute.InsertOnSubmit(dis);
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
                ++i; 
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
        private SB.Movie Convert(Movie movInfo)
        {
            SB.Movie mv = new ShareMan.SB.Movie
            {
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
                Category = baseItem.Category
            };
            return sbbi;
        }
        private long PublishBase(long id)
        {
            return 1;
        }

        private long CommitBase(BaseItem baseInfo, 
            ref FSM.FileSet fset,string srcIp,ShareManDataContext context)
        {
            context.BaseItem.InsertOnSubmit(baseInfo);
            context.SubmitChanges();
            // 插入審核信息
            Audit ad = new Audit
            {
                ID = baseInfo.ID,
                AuditDate = DateTime.Now,
                AuditOwner = 1,
                Reason = "",
                State = false
            };
            context.Audit.InsertOnSubmit(ad);

            // 開始創建文件集
            FSM.FileSetMan src = new ShareMan.FSM.FileSetMan();
            QS.Queries qs = new ShareMan.QS.Queries();
            // 查詢WebService服務地址
            src.Url = qs.QueryUrl(srcIp, 1);
            
            FileSetLink fsSrc = new FileSetLink();
            // 
            fsSrc.FileSetID = src.CreateFileSet(fset);
            fsSrc.IP = srcIp;
            /*
             * 此处查询当前活动的中转服务器
             * 
             * */
            FileSetLink fsInter = new FileSetLink();
            FSM.FileSetMan inter = new ShareMan.FSM.FileSetMan();

            /* 此处构造需要调用的WebService 地址
                srv.Url = "http://" + srcIp +""
             * */

            inter.Url = qs.QueryUrl("192.168.0.1", 1);

            fsInter.FileSetID = inter.CreateFileSet(fset);
            fset = inter.QureyFileSet(fsInter.FileSetID);
            // 下载文件ID
            //fset.ID = fsInter.FileSetID;

            fsInter.IP = "192.168.1.2";
            context.FileSetLink.InsertOnSubmit(fsSrc);
            context.FileSetLink.InsertOnSubmit(fsInter);
            context.SubmitChanges();

            ItemLink iLink = new ItemLink();
            iLink.ID = baseInfo.ID;
            iLink.SrcLink = fsSrc.ID;
            iLink.InterLink = fsInter.ID;
            context.ItemLink.InsertOnSubmit(iLink);

            context.SubmitChanges();
            return baseInfo.ID;
        }
    }
}
