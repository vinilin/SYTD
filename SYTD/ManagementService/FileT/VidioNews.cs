using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FluorineFx;

namespace ManagementService.FileT
{
    [RemotingService]
    class VidioNews
    {
        public long category = 5;
        [DataTableType("VidioNews.GetVidioNewsList")]
        public DataTable GetList(string vidioName,
            string publishType,
            string addMan,
            string addStartTime,
            string addEndTime,
            string auditMan,
            string auditStartTime,
            string auditEndTime,
            string state,
            int pageSize,int pageIndex)
        {
            vidioName = Com.Com.checkSql(vidioName);
            publishType = Com.Com.checkSql(publishType);
            addMan = Com.Com.checkSql(addMan);
            addStartTime = Com.Com.checkSql(addStartTime);
            addEndTime = Com.Com.checkSql(addEndTime);
            auditMan = Com.Com.checkSql(auditMan);
            auditStartTime = Com.Com.checkSql(auditStartTime);
            addStartTime = Com.Com.checkSql(addStartTime);
            auditEndTime = Com.Com.checkSql(auditEndTime);
            state = Com.Com.checkSql(state);

            string tblname = "BaseItem";
            string fieldCollections = " 0 as ISCHECKED,";
            fieldCollections += "BaseItem.ID,";
            fieldCollections += "BaseItem.SUBCODE,";
            fieldCollections += "T_Subsection.SUBNAME,";
            fieldCollections += "BaseItem.OWNER as ADDMAN,";
            fieldCollections += "DateAdd(Hour,-8,BaseItem.BIRTH) as ADDTIME,";
            fieldCollections += "BaseItem.TITLE,";
            fieldCollections += "BaseItem.PUBLISHTYPE,";
            fieldCollections += "PublishType.name AS PUBLISHTYPENAME,";
            fieldCollections += "ISSUEDATE,";
            fieldCollections += "Audit.AuditOwner AS AUDITMAN,";
            fieldCollections += "Audit.STATE,";
            fieldCollections += "case Audit.state when 0 then null else DateAdd(Hour,-8,Audit.AuditDate) end as AUDITTIME,";
            fieldCollections += " VidioNews.Context ";
            fieldCollections += "BaseItem.Ext3 AS LANG,";
            fieldCollections += "case Audit.STATE when -1 then '不通过' when 1 then '通过' when 0 then '待审核' when 2 then '已发布'  end as STATENAME";

            string orderField = "id";
            int orderType = 1;


            string strWhere = "BaseItem.category = " + category;
            if ( movieName!= "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  BaseItem.title like '%" + movieName + "%'";
            }
            if (publishType != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  BaseItem.publishType=" + publishType + " ";
            }
            if (addMan != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " BaseItem.Owner = '" + addMan + "'";
            }
            if (addStartTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " BaseItem.Birth >= '" + addStartTime + " 00:00:00'";
            }
            if (addStartTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " BaseItem.Birth <= '"+addEndTime+" 23:59:59'";
            }
            if (auditMan!="")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " Audit.AuditOwner = '" + auditMan + "'";
            }
            if (auditStartTime!="")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " Audit.AuditDate >= '"+ auditStartTime +" 00:00:00'";
            }
            if (auditEndTime!="")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " Audit.AuditDate <= '"+ auditEndTime +" 23:59:59'";
            }
            if (state!="")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " Audit.State = "+state;
            }


            string joinConditions = " left join PublishType on PublishType.ID = BaseItem.publishType and PublishType.category = 5 ";
            joinConditions += " inner join Movie on Movie.Id=BaseItem.Id ";
            joinConditions += " inner join Audit on Audit.Id=BaseItem.Id ";
            joinConditions += " left join T_SubSection on T_SubSection.subCode=BaseItem.subCode ";

            Com.CommonPage common = new Com.CommonPage();
            DataTable dt = common.CommonQueryPage(tblname, fieldCollections, strWhere, joinConditions, orderField, orderType, pageSize, pageIndex);

            return dt;
        }
        [DataTableType("VidioNews.GetVidioNewsInfoById")]
        public DataTable GetVidioNewsInfoById(string BaseItemId)
        {
            string strSql = "select BaseItem.ID,";
            strSql += "BaseItem.OWNER as ADDMAN,";
            strSql += "DateAdd(Hour,-8,BaseItem.birth) as ADDTIME,";
            strSql += "BaseItem.TITLE,";
            strSql += "BaseItem.PUBLISHTYPE,";
            strSql += "BaseItem.ISSUEDATE,";
            strSql += "BaseItem.BRIEF,";
            strSql += "VidioNews.Context,";
            strSql += "BaseItem.Ext3 AS LANG,";
            strSql += "Audit.AUDITOWNER,";
            strSql += "Audit.AUDITDATE AS AUDITTIME ";
            strSql += " from BaseItem ";
            strSql += " inner join Movie on Movie.Id=BaseItem.Id ";
            strSql += " inner join Audit on Audit.Id=BaseItem.Id ";
            strSql += " where BaseItem.category = " + category + " and BaseItem.id=" + BaseItemId;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }
        public string AddVidioNews(string owner,
            string subCode,
            string title,
            int publishType,
            string publishTypeName,
            string issueDate,
            string brief,
            string ctxt,
            string lang,
            string defaultpic,
            string sequence,
            FluorineFx.AMF3.ArrayCollection fileSet,
            string srcIp)
        {
            string result="系统错误，保存失败";
            owner = Com.Com.checkSql(owner);
            title = Com.Com.checkSql(title);
            //publishType = Com.Com.checkSql(publishType);
            issueDate = Com.Com.checkSql(issueDate);
            brief = Com.Com.checkSql(brief);
            ctxt = Com.Com.checkSql(ctxt);
            DateTime birth = System.DateTime.Now;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select title from BaseItem where title = '" + title + "' and category = " + category;
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt!=null && tempDt.Rows.Count>0)
            {
                result = "该名称的视频新闻已经存在，增加失败。";
            }
            else
            {
                FT.BaseItem bi = new ManagementService.FT.BaseItem();
                bi.Owner = owner;
                bi.SubCode = subCode;
                bi.PublishType = publishType;
                bi.Title = title;
                bi.IssueDate = issueDate;
                bi.Birth = birth;
                bi.Brief = brief;
                bi.Category = category;
                bi.Ext1 = defaultpic;
                bi.Ext2 = sequence;
                bi.Ext3 = lang;

                FT.VidioNews vn = new ManagementService.FT.VidioNews();
                vn.Context = ctxt;

                FT.FileSet fs=new ManagementService.FT.FileSet();
                fs.Path = "视频新闻\\"+title+"\\";
                ManagementService.FT.File[]  files = new ManagementService.FT.File[fileSet.Count];
                for (int i = 0; i < fileSet.Count; i++)
                {
                    System.Collections.Hashtable uFileSet = (System.Collections.Hashtable)fileSet[i];
                    files[i] = new ManagementService.FT.File();
                    files[i].FileName = uFileSet["FILENAME"].ToString();
                    files[i].ShowIndex = i;
                }
                fs.File = files;
                FT.MasterControl control = new ManagementService.FT.MasterControl();
                try
                {
                    control.Url = new Common.SysConfig().GetManterControl();
                    result = "OK" + control.CommitVidioNews(bi, vn, fs, srcIp);
                }
                catch (System.Exception e)
                {
                    result = e.Message;
                }
            }
            Access.Dispose();
            return result;
        }

        public string UpdateVidioNews(string id,
            string title,
            int publishType,
            string issueDate,
            string brief,
            string ctxt,
            string lang,
            string defaultpic,
            string sequence,
            bool isReUpload,
            FluorineFx.AMF3.ArrayCollection fileSet)
        {
            string result = "系统错误，保存失败";
            issueDate = Com.Com.checkSql(issueDate);
            brief = Com.Com.checkSql(brief);
            ctxt = Com.Com.checkSql(ctxt);
                //访问WEBSERVICE
                FT.BaseItem bi = new ManagementService.FT.BaseItem();
                bi.ID = Convert.ToInt64(id);
                bi.IssueDate = issueDate;
                bi.PublishType = publishType;
                bi.Brief = brief;
                FT.VidioNews vn = new ManagementService.FT.VidioNews();
                mv.Context = ctxt;

                bi.Ext1 = defaultpic;
                bi.Ext2 = sequence;
                bi.Ext3 = lang;
                
                FT.MasterControl control = new ManagementService.FT.MasterControl();
                try
                {
                    control.Url = new Common.SysConfig().GetManterControl();
                    //result = "OK" + control.CommitMovie(bi, mv, fs, srcIp);
                    control.UpdateVidioNews(bi, vn);
                    result = "OK";
                }
                catch (System.Exception e)
                {
                    result = e.Message;
                }

                if (isReUpload)
                {//要重新上传文件
                    FT.FileSet fs = new ManagementService.FT.FileSet();
                    fs.Path = "视频新闻\\" + title + "\\";
                    ManagementService.FT.File[] files = new ManagementService.FT.File[fileSet.Count];
                    for (int i = 0; i < fileSet.Count; i++)
                    {
                        System.Collections.Hashtable uFileSet = (System.Collections.Hashtable)fileSet[i];
                        files[i] = new ManagementService.FT.File();
                        files[i].FileName = uFileSet["fileName"].ToString();
                        files[i].ShowIndex = i;
                    }
                    fs.File = files;
                    //重传文件
                    control.Url = new Common.SysConfig().GetManterControl();
                    control.UpdateFileSet(bi.ID,fs,"");
                }
            //}
            //Access.Dispose();
            return result;
        }

        //发布未通过可以让发布站点人员删除
        //未发布的应可以让审核人员删除
        [DataTableType("VidioNews.DeleteVidioNews")]
        public DataTable DeleteMovie(FluorineFx.AMF3.ArrayCollection vidios)
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("ID");
            DataColumn dc2 = new DataColumn("TITLE");
            DataColumn dc3 = new DataColumn("result");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            for (int i = 0; i < movies.Count; i++)
            {
                System.Collections.Hashtable uMovies = (System.Collections.Hashtable)vidios[i];
                DataRow dr = dt.NewRow();
                dr["ID"] = uMovies["ID"];
                dr["TITLE"] = uMovies["TITLE"];
                dt.Rows.Add(dr);
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //访问WEBSERVICE
                string result = "";
                FT.MasterControl control = new ManagementService.FT.MasterControl();
                control.Url = new Common.SysConfig().GetManterControl();
                control.RemoveVidioNews(Convert.ToInt64(dt.Rows[i]["ID"]));

                dt.Rows[i]["result"] = result;
            }
            return dt;
        }
        public string GetVidioPlayUrl(long id)
        {
            try
            {
                string url = new Common.SysConfig().GetVidioPlayUrl();
            }
            catch (Exception e)
            {
            }
                return "";  
        }
        public string AuditVidioNews(int id,int auditResult,string auditReason,string auditMan)
        {
            string result = "系统错误，审核失败。";
            auditReason = Com.Com.checkSql(auditReason);
            auditMan = Com.Com.checkSql(auditMan);

            FT.MasterControl control = new ManagementService.FT.MasterControl();
            control.Url = new Common.SysConfig().GetManterControl();
            
            try
            {
                //control.Audit(id, auditResult, auditReason, auditMan);
                control.Audit(id, auditResult, auditReason,auditMan);
                result = "OK";
            }
            catch (System.Exception e)
            {
                result = e.Message;
            }
            return result;
        }

        public string PublishVidioNews(long id, string[] dst)
        {
            string result = "系统错误，发布失败。";
            FT.MasterControl control = new ManagementService.FT.MasterControl();
            control.Url = new Common.SysConfig().GetManterControl();
            
            try
            {
                foreach (string d in dst)
                {
                    string[] tmp = new string[1];
                    tmp[0] = d;
                    try
                    {
                        control.PublishVidioNews(id, tmp);
                    }
                    catch(System.Exception ee)
                    {
                        result = d + "站点发布失败";
                        return result;
                    }
                }
                result = "OK";
            }
            catch (System.Exception e)
            {
                result = e.Message;
            }
            return result; 
        }
    }
}
