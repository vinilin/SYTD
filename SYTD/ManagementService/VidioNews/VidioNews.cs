﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FluorineFx;
using System.IO;

namespace ManagementService.VidioNews
{
    [RemotingService]
    public class VidioNews 
    {
        [DataTableType("VidioNews.GetVidioNewsList")]
        public DataTable GetVidioNewsList(string subcode,
            string ArticleType, //属于哪个栏目的数据
            string ArticleKind, //属于该栏目的哪个类别
            string ArticleTitle,
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
            subcode = Com.Com.checkSql(subcode);
            ArticleKind = Com.Com.checkSql(ArticleKind);
            ArticleTitle = Com.Com.checkSql(ArticleTitle);
            addMan = Com.Com.checkSql(addMan);
            addStartTime = Com.Com.checkSql(addStartTime);
            addEndTime = Com.Com.checkSql(addEndTime);
            auditMan = Com.Com.checkSql(auditMan);
            auditStartTime = Com.Com.checkSql(auditStartTime);
            auditEndTime = Com.Com.checkSql(auditEndTime);
            state = Com.Com.checkSql(state);

            string tblname = "T_VidioNewsList";
            string fieldCollections = " 0 as ISCHECKED,";
            fieldCollections += "T_VidioNewsList.ID,";
            fieldCollections += "T_VidioNewsList.SUBCODE,";
            fieldCollections += "T_SubSection.SUBNAME,";
            fieldCollections += "T_VidioNewsList.ARTICLEKIND,";
            fieldCollections += "T_SystemKind.text as ARTICLEKINDNAME,";
            fieldCollections += "T_SystemKind2.text as ARTICLEKINDNAME2,";
            fieldCollections += "T_VidioNewsList.ARTICLETITLE,";
            fieldCollections += "DateAdd(Hour,-8,T_VidioNewsList.addTime) as ADDTIME,";
            fieldCollections += "T_VidioNewsList.ADDMAN,";
            fieldCollections += "T_VidioNewsList.AUDITMAN,";
            fieldCollections += "DateAdd(Hour,-8,T_VidioNewsList.auditTime) as AUDITTIME,";
            fieldCollections += "T_VidioNewsList.SEQUENCE,";
            fieldCollections += "T_VidioNewsList.STATE,";
            fieldCollections += "T_VidioNewsList.SOURCE,";
            fieldCollections += "case T_VidioNewsList.state when -1 then '未发布' when 1 then '已发布' else '待审核' end as STATENAME";

            string orderField = "id";
            int orderType = 1;


            string strWhere = "";
            if (subcode!="")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_VidioNewsList.subCode='" + subcode + "'";
            }
            if (ArticleType != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_VidioNewsList.ArticleType='" + ArticleType + "'";
            }
            if (ArticleKind != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_VidioNewsList.ArticleKind='" + ArticleKind + "'";
            }
            if (ArticleTitle != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_VidioNewsList.ArticleTitle like '%" + ArticleTitle + "%'";
            }
            if (addMan != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_VidioNewsList.addMan='" + addMan + "'";
            }
            if (addStartTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_VidioNewsList.addTime>='" + addStartTime + " 00:00:00'";
            }
            if (addEndTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_VidioNewsList.addTime<='" + addEndTime + " 23:59:59'";
            }
            if (auditMan != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_VidioNewsList.auditMan='" + auditMan + "'";
            }
            if (auditStartTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_VidioNewsList.auditTime>='" + auditStartTime + " 00:00:00'";
            }
            if (auditEndTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_VidioNewsList.auditTime<='" + auditEndTime + " 23:59:59'";
            }
            if (state != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_VidioNewsList.state=" + state + "";
            }

            string joinConditions = " left join T_SubSection on T_SubSection.SubCode=T_VidioNewsList.SubCode ";
            joinConditions += " left join T_SystemKind on T_SystemKind.code=T_VidioNewsList.ArticleKind and T_VidioNewsList.subCode=T_SystemKind.subCode and T_SystemKind.kind='" + ArticleType + "'";
            joinConditions += " left join T_SystemKind as T_SystemKind2 on T_SystemKind2.code=T_VidioNewsList.ArticleKind2 and  T_VidioNewsList.subCode=T_SystemKind2.subCode and T_SystemKind2.kind='" + ArticleType + "'";

            Com.CommonPage common = new Com.CommonPage();
            DataTable dt = common.CommonQueryPage(tblname, fieldCollections, strWhere, joinConditions, orderField, orderType, pageSize, pageIndex);

            return dt;
        }

        [DataTableType("VidioNews.GetKQFWArticleList")]
        public DataTable GetKQFWArticleList(string subcode,
            FluorineFx.AMF3.ArrayCollection kindCode,
            string ArticleType, //属于哪个栏目的数据
            string ArticleKind, //属于该栏目的哪个类别
            string ArticleTitle,
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
            subcode = Com.Com.checkSql(subcode);
            ArticleKind = Com.Com.checkSql(ArticleKind);
            ArticleTitle = Com.Com.checkSql(ArticleTitle);
            addMan = Com.Com.checkSql(addMan);
            addStartTime = Com.Com.checkSql(addStartTime);
            addEndTime = Com.Com.checkSql(addEndTime);
            auditMan = Com.Com.checkSql(auditMan);
            auditStartTime = Com.Com.checkSql(auditStartTime);
            auditEndTime = Com.Com.checkSql(auditEndTime);
            state = Com.Com.checkSql(state);

            string tblname = "T_VidioNewsList";
            string fieldCollections = " 0 as ISCHECKED,";
            fieldCollections += "T_VidioNewsList.ID,";
            fieldCollections += "T_VidioNewsList.SUBCODE,";
            fieldCollections += "T_SubSection.SUBNAME,";
            fieldCollections += "T_VidioNewsList.ARTICLEKIND,";
            fieldCollections += "T_SystemKind.text as ARTICLEKINDNAME,";
            fieldCollections += "T_SystemKind2.text as ARTICLEKINDNAME2,";
            fieldCollections += "T_VidioNewsList.ARTICLETITLE,";
            fieldCollections += "DateAdd(Hour,-8,.addTime) as ADDTIME,";
            fieldCollections += "T_VidioNewsList.ADDMAN,";
            fieldCollections += "T_VidioNewsList.AUDITMAN,";
            fieldCollections += "DateAdd(Hour,-8,.auditTime) as AUDITTIME,";
            fieldCollections += "T_VidioNewsList.SEQUENCE,";
            fieldCollections += "T_VidioNewsList.STATE,";
            fieldCollections += "T_VidioNewsList.SOURCE,";
            fieldCollections += "case T_VidioNewsList.state when -1 then '未发布' when 1 then '已发布' else '待审核' end as STATENAME";

            string orderField = "id";
            int orderType = 1;

            string tempStr = "";
            for (int i = 0; i < kindCode.Count; i++)
            {
                System.Collections.Hashtable uKind=(System.Collections.Hashtable)kindCode[i];
                if (i != 0) { tempStr += " or "; }
                tempStr += " T_VidioNewsList.ArticleKind='" + uKind["CODE"].ToString() + "' ";
            }
            
            string strWhere = "";
            if (tempStr != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " (" + tempStr + ") ";
            }
            if (subcode != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_VidioNewsList.subCode='" + subcode + "'";
            }
            if (ArticleType != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_VidioNewsList.ArticleType='" + ArticleType + "'";
            }
            if (ArticleKind != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_VidioNewsList.ArticleKind='" + ArticleKind + "'";
            }
            if (ArticleTitle != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_VidioNewsList.ArticleTitle like '%" + ArticleTitle + "%'";
            }
            if (addMan != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_VidioNewsList.addMan='" + addMan + "'";
            }
            if (addStartTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_VidioNewsList.addTime>='" + addStartTime + " 00:00:00'";
            }
            if (addEndTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_VidioNewsList.addTime<='" + addEndTime + " 23:59:59'";
            }
            if (auditMan != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_VidioNewsList.auditMan='" + auditMan + "'";
            }
            if (auditStartTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_VidioNewsList.auditTime>='" + auditStartTime + " 00:00:00'";
            }
            if (auditEndTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_VidioNewsList.auditTime<='" + auditEndTime + " 23:59:59'";
            }
            if (state != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_VidioNewsList.state=" + state + "";
            }

            string joinConditions = " left join T_SubSection on T_SubSection.SubCode=T_VidioNewsList.SubCode ";
            joinConditions += " left join T_SystemKind on T_SystemKind.code=T_VidioNewsList.ArticleKind and T_VidioNewsList.subCode=T_SystemKind.subCode and T_SystemKind.kind='" + ArticleType + "'";
            joinConditions += " left join T_SystemKind as T_SystemKind2 on T_SystemKind2.code=T_VidioNewsList.ArticleKind2 and  T_VidioNewsList.subCode=T_SystemKind2.subCode and T_SystemKind2.kind='" + ArticleType + "'";

            Com.CommonPage common = new Com.CommonPage();
            DataTable dt = common.CommonQueryPage(tblname, fieldCollections, strWhere, joinConditions, orderField, orderType, pageSize, pageIndex);

            return dt;
        }

        [DataTableType("VidioNews.GetVidioNewsInfo")]
        public DataTable GetVidioNewsInfo(string ArticleId)
        {
            ArticleId = Com.Com.checkSql(ArticleId);
            string strSql = "select ID,SUBCODE,ARTICLETYPE,ARTICLEKIND,ARTICLEKIND2,ARTICLETITLE,ARTICLECONTENT,DEFAULTVIDIO,ADDTIME,ADDMAN,AUDITMAN,AUDITTIME,SEQUENCE,STATE,SOURCE from T_VidioNewsList where id=" + ArticleId;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }

        public string AddVidioNews(string SubCode,string ArticleType,string ArticleKind,string ArticleKind2, string ArticleTitle, string ArticleContent, string addMan, string sequence, string defaultPic,string SOURCE)
        {
            string result = "系统错误，保存失败。";
            SubCode = Com.Com.checkSql(SubCode);
            ArticleType = Com.Com.checkSql(ArticleType);
            ArticleKind = Com.Com.checkSql(ArticleKind);
            ArticleKind2 = Com.Com.checkSql(ArticleKind2);
            ArticleTitle = Com.Com.checkSql(ArticleTitle);
            ArticleContent = Com.Com.checkSql(ArticleContent);
            addMan = Com.Com.checkSql(addMan);
            sequence = Com.Com.checkSql(sequence);
            SOURCE = Com.Com.checkSql(SOURCE);
            string strSql = "select ArticleTitle from T_VidioNewsList where ArticleTitle='" + ArticleTitle + "' and subCode='" + SubCode + "' and articleType='" + ArticleType + "'";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                result = "已存在该标题内容的文章，保存失败。";
            }
            else
            {
                strSql = "insert into T_VidioNewsList(";
                strSql += "SubCode,";
                strSql += "ArticleType,";
                strSql += "ArticleKind,";
                strSql += "ArticleKind2,";
                strSql += "ArticleTitle,";
                strSql += "ArticleContent,";
                strSql += "addMan,";
                strSql += "addTime,";
                strSql += "sequence,";
                strSql += "defaultVidio,";
                strSql += "source,";
                strSql += "state)";
                strSql += " values(";
                strSql += "'" + SubCode + "',";
                strSql += "'" + ArticleType + "',";
                strSql += "'" + ArticleKind + "',";
                strSql += "'" + ArticleKind2 + "',";
                strSql += "'" + ArticleTitle + "',";
                strSql += "'" + ArticleContent + "',";
                strSql += "'" + addMan + "',";
                strSql += "'" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                strSql += "'" + sequence + "',";
                strSql += "'" + defaultPic + "',";
                strSql += "'" + SOURCE + "',";
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
            /*
             try
             {
                 string path = System.Web.HttpContext.Current.Server.MapPath(Com.Com.picUrl + "/" + type + "/" + sequence + "/");
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
            */
            return result;
        }

        public string UpdateArticle(string ArticleId,string SubCode,string ArticleType, string ArticleKind,string ArticleKind2, string ArticleTitle, string ArticleContent, string addMan, string sequence, string defaultPic,string SOURCE)
        {
            string result = "系统错误，保存失败。";
            ArticleId = Com.Com.checkSql(ArticleId);
            SubCode = Com.Com.checkSql(SubCode);
            ArticleType = Com.Com.checkSql(ArticleType);
            ArticleKind = Com.Com.checkSql(ArticleKind);
            ArticleKind2 = Com.Com.checkSql(ArticleKind2);
            ArticleTitle = Com.Com.checkSql(ArticleTitle);
            ArticleContent = Com.Com.checkSql(ArticleContent);
            addMan = Com.Com.checkSql(addMan);
            sequence = Com.Com.checkSql(sequence);
            SOURCE = Com.Com.checkSql(SOURCE);
            string strSql = "select ArticleTitle from T_VidioNewsList where ArticleTitle='" + ArticleTitle + "'  and subCode='" + SubCode + "' and articleType='" + ArticleType + "' and id<>" + ArticleId;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                result = "已存在该标题内容的文章，保存失败。";
            }
            else
            {
                strSql = "update  T_VidioNewsList set ";
                strSql += "SubCode='" + SubCode + "',";
                strSql += "ArticleType='" + ArticleType + "',";
                strSql += "ArticleKind='" + ArticleKind + "',";
                strSql += "ArticleKind2='" + ArticleKind2 + "',";
                strSql += "ArticleTitle='" + ArticleTitle + "',";
                strSql += "ArticleContent='" + ArticleContent + "',";
                strSql += "addMan='" + addMan + "',";
                strSql += "addTime='" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                strSql += "sequence='" + sequence + "',";
                strSql += "auditMan=null,auditTime=null,";
                strSql += "defaultVidio='" + defaultPic + "',";
                strSql += "source='" + SOURCE + "',";
                strSql += "state=0 ";
                strSql += " where id=" + ArticleId;
                if (Access.execSqlNoQuery1(strSql))
                {
                    result = "OK" + ArticleId;
                }
            }
            Access.Dispose();
            /*
            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath(Com.Com.picUrl + "/" + type + "/" + sequence + "/");
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
            */
            return result;
        }

        public string VidioSave(string ArticleId, string defaultPic)
        {
            string result = "系统错误，保存图片至数据库失败。";
            string strSql = "update T_VidioNewsList set defaultVidio='" + defaultPic + "' where id=" + ArticleId;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            if (Access.execSqlNoQuery1(strSql))
            {
                result = "OK";
            }
            Access.Dispose();
            return result;
        }

        [DataTableType("VidioNews.DeleteVidioNews")]
        public DataTable DeleteVidioNews(FluorineFx.AMF3.ArrayCollection articles,string type)
        {
            type = Com.Com.checkSql(type);
            DataTable resultDt = new DataTable();
            DataColumn dc1 = new DataColumn("ID");
            DataColumn dc2 = new DataColumn("ARTICLETITLE");
            DataColumn dc3 = new DataColumn("SEQUENCE");
            DataColumn dc4 = new DataColumn("result");
            resultDt.Columns.Add(dc1);
            resultDt.Columns.Add(dc2);
            resultDt.Columns.Add(dc3);
            resultDt.Columns.Add(dc4);

            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            for (int i = 0; i < articles.Count;i++ )
            {
                System.Collections.Hashtable uArticle = (System.Collections.Hashtable)articles[i];
                string test = "select state from T_VidioNewsList where id=" + uArticle["ID"].ToString();
                DataTable tempDt = Access.execSql("select state from T_VidioNewsList where id=" + uArticle["ID"].ToString());
                if (tempDt!=null && tempDt.Rows.Count>0)
                {
                    if (tempDt.Rows[0]["state"].ToString()=="1")
                    {
                        DataRow dr = resultDt.NewRow();
                        dr["ID"] = uArticle["ID"].ToString();
                        dr["ARTICLETITLE"] = uArticle["ARTICLETITLE"].ToString();
                        dr["SEQUENCE"] = uArticle["SEQUENCE"].ToString();
                        dr["result"] = "该信息已发布，不能删除。";
                        resultDt.Rows.Add(dr);
                    }
                    else
                    {
                        if (Access.execSqlNoQuery1("delete T_VidioNewsList where id=" + uArticle["ID"].ToString()))
                        {
                            try
                            {  
                                //string path = System.AppDomain.CurrentDomain.BaseDirectory + "UploadFile\\" + type + "\\" + uArticle["SEQUENCE"].ToString();
                                string path = System.Web.HttpContext.Current.Server.MapPath(Com.Com.picUrl + "/" + type + "/" + uArticle["SEQUENCE"].ToString() + "/");
                                if (Directory.Exists(path))
                                {
                                    Directory.Delete(path, true);
                                }
                            }
                            catch { }
                            DataRow dr = resultDt.NewRow();
                            dr["ID"] = uArticle["ID"].ToString();
                            dr["ARTICLETITLE"] = uArticle["ARTICLETITLE"].ToString();
                            dr["SEQUENCE"] = uArticle["SEQUENCE"].ToString();
                            dr["result"] = "OK";
                            resultDt.Rows.Add(dr);
                        }
                        else
                        {
                            DataRow dr = resultDt.NewRow();
                            dr["ID"] = uArticle["ID"].ToString();
                            dr["ARTICLETITLE"] = uArticle["ARTICLETITLE"].ToString();
                            dr["SEQUENCE"] = uArticle["SEQUENCE"].ToString();
                            dr["result"] = "该信息已发布，不能删除。";
                            resultDt.Rows.Add(dr);
                        }
                    }
                }
                else
                {
                    DataRow dr = resultDt.NewRow();
                    dr["ID"] = uArticle["ID"].ToString();
                    dr["ARTICLETITLE"] = uArticle["ARTICLETITLE"].ToString();
                    dr["SEQUENCE"] = uArticle["SEQUENCE"].ToString();
                    dr["result"] = "系统错误，删除失败。";
                    resultDt.Rows.Add(dr);
                }
            }
            Access.Dispose();
            return resultDt;
        }


        public string BatchPub(string[] ids, string auditMan)
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
            string strSql = "update T_VidioNewsList set auditMan='" + auditMan + "',auditTime='" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',state=1  where id in (" + str + ")";
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
            string strSql = "update T_VidioNewsList set auditMan='" + auditMan + "',auditTime='" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',state=-1 where id in (" + str + ")";
            if (Access.execSqlNoQuery1(strSql))
            {
                result = "OK";
            }
            Access.Dispose();
            return result;
        }


        public string PubVidio(string ArticleId, string SubCode, string ArticleType, string ArticleKind,string ArticleKind2, string ArticleTitle, string ArticleContent, string auditMan, string sequence, string defaultPic)
        {
            string result = "系统错误，发布失败。";
            ArticleId = Com.Com.checkSql(ArticleId);
            SubCode = Com.Com.checkSql(SubCode);
            ArticleType = Com.Com.checkSql(ArticleType);
            ArticleKind = Com.Com.checkSql(ArticleKind);
            ArticleKind2 = Com.Com.checkSql(ArticleKind2);
            ArticleTitle = Com.Com.checkSql(ArticleTitle);
            ArticleContent = Com.Com.checkSql(ArticleContent);
            auditMan = Com.Com.checkSql(auditMan);
            sequence = Com.Com.checkSql(sequence);
            string strSql = "select ArticleTitle from T_VidioNewsList  where ArticleTitle='" + ArticleTitle + "'  and subCode='" + SubCode + "' and articleType='" + ArticleType + "' and id<>" + ArticleId;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                result = "已存在该标题内容的文章，发布失败。";
            }
            else
            {
                strSql = "update T_VidioNewsList set ";
                strSql += "SubCode='" + SubCode + "',";
                strSql += "ArticleType='" + ArticleType + "',";
                strSql += "ArticleKind='" + ArticleKind + "',";
                strSql += "ArticleKind2='" + ArticleKind2 + "',";
                strSql += "ArticleTitle='" + ArticleTitle + "',";
                strSql += "ArticleContent='" + ArticleContent + "',";
                strSql += "auditMan='" + auditMan + "',";
                strSql += "auditTime='" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                strSql += "sequence='" + sequence + "',";
                strSql += "defaultVidio='" + defaultPic + "',";
                strSql += "state=1 ";
                strSql += " where id=" + ArticleId;
                if (Access.execSqlNoQuery1(strSql))
                {
                    result = "OK" + ArticleId;
                }
            }
            Access.Dispose();
            /*
            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath(Com.Com.picUrl + "/" + type + "/" + sequence + "/");
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
            */
            return result;
        }

        public string NotPubVidio(string ArticleId, string SubCode, string ArticleType, string ArticleKind,string ArticleKind2, string ArticleTitle, string ArticleContent, string auditMan, string sequence, string defaultPic)
        {
            string result = "系统错误，不发布失败。";
            ArticleId = Com.Com.checkSql(ArticleId);
            SubCode = Com.Com.checkSql(SubCode);
            ArticleType = Com.Com.checkSql(ArticleType);
            ArticleKind = Com.Com.checkSql(ArticleKind);
            ArticleKind2 = Com.Com.checkSql(ArticleKind2);
            ArticleTitle = Com.Com.checkSql(ArticleTitle);
            ArticleContent = Com.Com.checkSql(ArticleContent);
            auditMan = Com.Com.checkSql(auditMan);
            sequence = Com.Com.checkSql(sequence);
            string strSql = "select ArticleTitle from T_VidioNewsList where ArticleTitle='" + ArticleTitle + "'  and subCode='" + SubCode + "' and articleType='" + ArticleType + "' and id<>" + ArticleId;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                result = "已存在该标题内容的文章，不发布失败。";
            }
            else
            {
                strSql = "update T_VidioNewsList set ";
                strSql += "SubCode='" + SubCode + "',";
                strSql += "ArticleType='" + ArticleType + "',";
                strSql += "ArticleKind='" + ArticleKind + "',";
                strSql += "ArticleKind2='" + ArticleKind2 + "',";
                strSql += "ArticleTitle='" + ArticleTitle + "',";
                strSql += "ArticleContent='" + ArticleContent + "',";
                strSql += "auditMan='" + auditMan + "',";
                strSql += "auditTime='" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                strSql += "sequence='" + sequence + "',";
                strSql += "defaultPic='" + defaultPic + "',";
                strSql += "state=-1 ";
                strSql += " where id=" + ArticleId;
                if (Access.execSqlNoQuery1(strSql))
                {
                    result = "OK" + ArticleId;
                }
            }
            Access.Dispose();
            /*
            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath(Com.Com.picUrl + "/" + type + "/" + sequence + "/");
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
            */
            return result;
        }
    }
}
