using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FluorineFx;
using System.IO;

namespace ManagementService.News
{
    [RemotingService]
    public class News
    {
        [DataTableType("News.GetNewsList")]
        public DataTable GetNewsList(
            string newsKind,
            string newsTitle,
            string addMan,
            string addStartTime,
            string addEndTime,
            string auditMan,
            string auditStartTime,
            string auditEndTime,
            string state,
            int pageSize,
            int pageIndex)
        {
            newsKind = Com.Com.checkSql(newsKind);
            newsTitle = Com.Com.checkSql(newsTitle);
            addMan = Com.Com.checkSql(addMan);
            addStartTime = Com.Com.checkSql(addStartTime);
            addEndTime = Com.Com.checkSql(addEndTime);
            auditMan = Com.Com.checkSql(auditMan);
            auditStartTime = Com.Com.checkSql(auditStartTime);
            auditEndTime = Com.Com.checkSql(auditEndTime);
            state = Com.Com.checkSql(state);
            string tblname = "T_NewsList";
            string fieldCollections = " 0 as ISCHECKED,";
            fieldCollections += "T_NewsList.ID,";
            fieldCollections += "T_NewsList.NEWSKIND,";
            fieldCollections += "T_NewsList.NEWSTITLE,";
            fieldCollections += "DateAdd(Hour,-8,T_NewsList.addTime) as ADDTIME,";
            fieldCollections += "T_NewsList.ADDMAN,";
            fieldCollections += "T_NewsList.AUDITMAN,";
            fieldCollections += "DateAdd(Hour,-8,T_NewsList.auditTime) as AUDITTIME,";
            fieldCollections += "T_NewsList.SEQUENCE,";
            fieldCollections += "T_NewsList.STATE,";
            fieldCollections += "case T_NewsList.state when -1 then '未发布' when 1 then '已发布' else '待审核' end as STATENAME";

            string orderField = "id";
            int orderType = 1;


            string strWhere = "";
            if (newsKind != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_NewsList.newsKind='" + newsKind + "'";
            }
            if (newsTitle != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_NewsList.newsTitle like '%" + newsTitle + "%'";
            }
            if (addMan != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_NewsList.addMan='" + addMan + "'";
            }
            if (addStartTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_NewsList.addTime>='" + addStartTime + " 00:00:00'";
            }
            if (addEndTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_NewsList.addTime<='" + addEndTime + " 23:59:59'";
            }
            if (auditMan != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_NewsList.auditMan='" + auditMan + "'";
            }
            if (auditStartTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_NewsList.auditTime>='" + auditStartTime + " 00:00:00'";
            }
            if (auditEndTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_NewsList.auditTime<='" + auditEndTime + " 23:59:59'";
            }
            if (state != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_NewsList.state=" + state + "";
            }

            string joinConditions = "";

            Com.CommonPage common = new Com.CommonPage();
            DataTable dt = common.CommonQueryPage(tblname, fieldCollections, strWhere, joinConditions, orderField, orderType, pageSize, pageIndex);

            return dt;
        }

        [DataTableType("News.GetNewsInfo")]
        public DataTable GetNewsInfo(string newsId)
        {
            newsId = Com.Com.checkSql(newsId);
            string strSql = "select ID,NEWSKIND,NEWSTITLE,NEWSCONTENT,DEFAULTPIC,ADDTIME,ADDMAN,AUDITMAN,AUDITTIME,SEQUENCE,STATE from T_NewsList where id=" + newsId;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }

        public string AddNews(string newsKind, string newsTitle, string newsContent, string addMan, string sequence, string defaultPic)
        {
            string result = "系统错误，保存失败。";
            newsKind = Com.Com.checkSql(newsKind);
            newsTitle = Com.Com.checkSql(newsTitle);
            newsContent = Com.Com.checkSql(newsContent);
            addMan = Com.Com.checkSql(addMan);
            sequence = Com.Com.checkSql(sequence);
            string strSql = "select T_NewsList from newsList where newsTitle='" + newsTitle + "'";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                result = "已存在该标题内容的新闻，保存失败。";
            }
            else
            {
                strSql = "insert into T_NewsList(";
                strSql += "newsKind,";
                strSql += "newsTitle,";
                strSql += "newsContent,";
                strSql += "addMan,";
                strSql += "addTime,";
                strSql += "sequence,";
                strSql += "defaultPic,";
                strSql += "state)";
                strSql += " values(";
                strSql += "'" + newsKind + "',";
                strSql += "'" + newsTitle + "',";
                strSql += "'" + newsContent + "',";
                strSql += "'" + addMan + "',";
                strSql += "'" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                strSql += "'" + sequence + "',";
                strSql += "'" + defaultPic + "',";
                strSql += "0";
                strSql += ")";
                if (Access.execSqlNoQuery1(strSql))
                {
                    tempDt = Access.execSql("select @@IDENTITY");
                    if (tempDt != null && tempDt.Rows.Count > 0)
                    {
                        result = "OK" + tempDt.Rows[0][0].ToString();
                    }
                }
            }
            Access.Dispose();
            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath(new Common.SysConfig().GetValueByKey("picUploadUrl") + newsKind + "/" + sequence);
                if (Directory.Exists(path))
                {
                    string[] files = Directory.GetFiles(path);
                    if (files == null || files.Length == 0)
                    {
                        Directory.Delete(path, true);
                    }
                }
            }
            catch { }
            return result;
        }

        public string UpdateNews(string newsId, string newsKind, string newsTitle, string newsContent, string addMan,  string sequence, string defaultPic)
        {
            string result = "系统错误，保存失败。";
            newsId = Com.Com.checkSql(newsId);
            newsKind = Com.Com.checkSql(newsKind);
            newsTitle = Com.Com.checkSql(newsTitle);
            newsContent = Com.Com.checkSql(newsContent);
            addMan = Com.Com.checkSql(addMan);
            sequence = Com.Com.checkSql(sequence);
            string strSql = "select newsTitle from T_NewsList where newsTitle='" + newsTitle + "' and id<>" + newsId;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                result = "已存在该标题内容的新闻，保存失败。";
            }
            else
            {
                strSql = "update T_NewsList set ";
                strSql += "newsKind='" + newsKind + "',";
                strSql += "newsTitle='" + newsTitle + "',";
                strSql += "newsContent='" + newsContent + "',";
                strSql += "addMan='" + addMan + "',";
                strSql += "addTime='" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                strSql += "sequence='" + sequence + "',";
                strSql += "auditMan=null,auditTime=null,";
                strSql += "defaultPic='" + defaultPic + "',";
                strSql += "state=0 ";
                strSql += " where id=" + newsId;
                if (Access.execSqlNoQuery1(strSql))
                {
                    result = "OK" + newsId;
                }
            }
            Access.Dispose();

            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath(new Common.SysConfig().GetValueByKey("picUploadUrl") + newsKind + "/" + sequence);
                if (Directory.Exists(path))
                {
                    string[] files = Directory.GetFiles(path);
                    if (files == null || files.Length == 0)
                    {
                        Directory.Delete(path, true);
                    }
                }
            }
            catch { }
            return result;
        }

        public string newsPicSave(string newsId, string defaultPic)
        {
            string result = "系统错误，保存图片至数据库失败。";
            string strSql = "update T_NewsList set defaultPic='" + defaultPic + "' where id=" + newsId;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            if (Access.execSqlNoQuery1(strSql))
            {
                result = "OK";
            }
            Access.Dispose();
            return result;
        }

        public string DeleteNews(string type, string[] sequences, string[] ids)
        {
            string result = "系统错误，删除失败。";
            type = Com.Com.checkSql(type);
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            fileOperate.fileOperate fileOp = new fileOperate.fileOperate();
            fileOp.Url = new Common.SysConfig().GetValueByKey("fileUploadUrl") + "fileOperate.asmx";
            string str = "";
            for (int i = 0; i < ids.Length; i++)
            {
                str += ids[i];
                if (i != ids.Length - 1)
                {
                    str += ",";
                }
                Access.execSqlNoQuery1("delete T_NewsFile where newsId=" + ids[i]);
                fileOp.deleteDir("?:P)(OL>,ki8", type, ids[i]);
            }
            for (int i = 0; i < sequences.Length; i++)
            {
                try
                {
                    string path = System.Web.HttpContext.Current.Server.MapPath(new Common.SysConfig().GetValueByKey("picUploadUrl") + type + "/" + sequences[i]);
                    if (Directory.Exists(path))
                    {
                        //Directory.Delete(path, true);
                    }
                }
                catch { }
            }
            string strSql = "delete T_NewsList where id in (" + str + ")";
            if (Access.execSqlNoQuery1(strSql))
            {
                Access.execSqlNoQuery1("delete T_NewsPicFile where newsId in (" + str + ")");
                result = "OK";
            }
            Access.Dispose();
            return result;
        }


        public string BatchPub(string[] ids,  string auditMan)
        {
            string result = "系统错误，审核失败。";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string str = "";
            for (int i = 0; i < ids.Length; i++)
            {
                str += ids[i];
                if (i != ids.Length - 1)
                {
                    str += ",";
                }
            }
            string strSql = "update T_NewsList set auditMan='" + auditMan + "',auditTime='" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',state=1  where id in (" + str + ")";
            if (Access.execSqlNoQuery1(strSql))
            {
                result = "OK";
            }
            Access.Dispose();
            return result;
        }

        public string BatchNotPub(string[] ids, string auditMan)
        {
            string result = "系统错误，审核失败。";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string str = "";
            for (int i = 0; i < ids.Length; i++)
            {
                str += ids[i];
                if (i != ids.Length - 1)
                {
                    str += ",";
                }
            }
            string strSql = "update T_NewsList set auditMan='" + auditMan + "',auditTime='" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',state=-1 where id in (" + str + ")";
            if (Access.execSqlNoQuery1(strSql))
            {
                result = "OK";
            }
            Access.Dispose();
            return result;
        }


        public string PubNews(string newsId, string newsKind, string newsTitle, string newsContent, string auditMan,  string sequence, string defaultPic)
        {
            string result = "系统错误，保存失败。";
            newsId = Com.Com.checkSql(newsId);
            newsKind = Com.Com.checkSql(newsKind);
            newsTitle = Com.Com.checkSql(newsTitle);
            newsContent = Com.Com.checkSql(newsContent);
            auditMan = Com.Com.checkSql(auditMan);
            string strSql = "select T_NewsList from newsList where newsTitle='" + newsTitle + "' and id<>" + newsId;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                result = "已存在该标题内容的新闻，发布失败。";
            }
            else
            {
                strSql = "update T_NewsList set ";
                strSql += "newsKind='" + newsKind + "',";
                strSql += "newsTitle='" + newsTitle + "',";
                strSql += "newsContent='" + newsContent + "',";
                strSql += "auditMan='" + auditMan + "',";
                strSql += "auditTime='" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                strSql += "defaultPic='" + defaultPic + "',";
                strSql += "state=1 ";
                strSql += " where id=" + newsId;
                if (Access.execSqlNoQuery1(strSql))
                {
                    result = "OK";
                }
            }
            Access.Dispose();

            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath(new Common.SysConfig().GetValueByKey("picUploadUrl") + newsKind + "/" + sequence);
                if (Directory.Exists(path))
                {
                    string[] files = Directory.GetFiles(path);
                    if (files == null || files.Length == 0)
                    {
                        Directory.Delete(path, true);
                    }
                }
            }
            catch { }
            return result;
        }

        public string NotPubNews(string newsId, string newsKind, string newsTitle, string newsContent, string auditMan, string sequence, string defaultPic)
        {
            string result = "系统错误，保存失败。";
            newsId = Com.Com.checkSql(newsId);
            newsKind = Com.Com.checkSql(newsKind);
            newsTitle = Com.Com.checkSql(newsTitle);
            newsContent = Com.Com.checkSql(newsContent);
            auditMan = Com.Com.checkSql(auditMan);
            string strSql = "select T_NewsList from newsList where newsTitle='" + newsTitle + "' and id<>" + newsId;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                result = "已存在该标题内容的新闻，不发布失败。";
            }
            else
            {
                strSql = "update T_NewsList set ";
                strSql += "newsKind='" + newsKind + "',";
                strSql += "newsTitle='" + newsTitle + "',";
                strSql += "newsContent='" + newsContent + "',";
                strSql += "auditMan='" + auditMan + "',";
                strSql += "auditTime='" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                strSql += "defaultPic='" + defaultPic + "',";
                strSql += "state=-1 ";
                strSql += " where id=" + newsId;
                if (Access.execSqlNoQuery1(strSql))
                {
                    result = "ok";
                }
            }
            Access.Dispose();

            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath(new Common.SysConfig().GetValueByKey("picUploadUrl") + newsKind + "/" + sequence);
                if (Directory.Exists(path))
                {
                    string[] files = Directory.GetFiles(path);
                    if (files == null || files.Length == 0)
                    {
                        Directory.Delete(path, true);
                    }
                }
            }
            catch { }
            return result;
        }
    }
}
