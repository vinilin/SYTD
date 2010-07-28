﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FluorineFx;

namespace ManagementService.FileT
{
    [RemotingService]
    public class Music
    {
        public long category = 2;

        [DataTableType("Music.GetMusicList")]
        public DataTable GetList(string songName,
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
            songName = Com.Com.checkSql(songName);
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
            fieldCollections += "Music.SINGER,";
            fieldCollections += "Music.AUTHOR,";
            fieldCollections += "BaseItem.Ext3 AS LANG,";
            //fieldCollections += "case Audit.STATE when -1 then '未发布' when 1 then '已发布' else '待审核' end as STATENAME";
            fieldCollections += "case Audit.STATE when -1 then '不通过' when 1 then '通过' when 0 then '待审核' when 2 then '已发布'  end as STATENAME";

            string orderField = "id";
            int orderType = 1;


            string strWhere = "BaseItem.category = " + category;
            if ( songName!= "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  BaseItem.title like '%" + songName + "%'";
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


            string joinConditions = " left join PublishType on PublishType.ID =BaseItem.publishType and PublishType.category = 2 ";
            joinConditions += " inner join Music on Music.Id=BaseItem.Id ";
            joinConditions += " inner join Audit on Audit.Id=BaseItem.Id ";
            joinConditions += " left join T_SubSection on T_SubSection.subCode=BaseItem.subCode ";

            Com.CommonPage common = new Com.CommonPage();
            DataTable dt = common.CommonQueryPage(tblname, fieldCollections, strWhere, joinConditions, orderField, orderType, pageSize, pageIndex);

            return dt;
        }

        public string PublishMusic(long id, string[] dst)
        {
            string result = "系统错误，发布失败。";
            FT.MasterControl control = new ManagementService.FT.MasterControl();
            control.Url = new Common.SysConfig().GetManterControl();
            
            try
            {
            //    control.PublishMusic(id, dst);
                foreach (string d in dst)
                {
                    string[] tmp = new string[1];
                    tmp[0] = d;
                    try
                    {
                        control.PublishMusic(id, tmp);
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

        [DataTableType("Music.GetMusicInfoById")]
        public DataTable GetMusicInfoById(string BaseItemId)
        {
            string strSql = "select BaseItem.ID,";
            strSql += "BaseItem.OWNER as ADDMAN,";
            strSql += "DateAdd(Hour,-8,BaseItem.birth) as ADDTIME,";
            strSql += "BaseItem.TITLE,";
            strSql += "BaseItem.PUBLISHTYPE,";
            strSql += "BaseItem.ISSUEDATE,";
            strSql += "BaseItem.BRIEF,";
            strSql += "Music.SINGER,";
            strSql += "Music.AUTHOR,";
            strSql += "BaseItem.Ext3 AS LANG,";
            strSql += "Audit.AUDITOWNER,";
            strSql += "Audit.AUDITDATE AS AUDITTIME ";
            strSql += " from BaseItem ";
            strSql += " inner join Music on Music.Id=BaseItem.Id ";
            strSql += " inner join Audit on Audit.Id=BaseItem.Id ";
            strSql += " where BaseItem.category = " + category + " and BaseItem.id=" + BaseItemId;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }

        public string AddMusic(string owner,
            string subCode,
            string title,
            int publishType,
            string publishTypeName,
            string issueDate,
            string brief,
            string singer,
            string author,
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
            singer = Com.Com.checkSql(singer);
            author = Com.Com.checkSql(author);
            DateTime birth = System.DateTime.Now;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select title from BaseItem where title = '" + title + "' and category = " + category;
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt!=null && tempDt.Rows.Count>0)
            {
                result = "该名称的电影已经存在，增加失败。";
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

                FT.Music mv = new ManagementService.FT.Music();
                mv.Author = author;
                mv.Singer= singer;

                FT.FileSet fs=new ManagementService.FT.FileSet();
                fs.Path = "音乐\\"+title+"\\";
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
                    result = "OK" + control.CommitMusic(bi, mv, fs, srcIp);
                }
                catch (System.Exception e)
                {
                    result = e.Message;
                }
            }
            Access.Dispose();
            return result;
        }

        public string UpdateMusic(string id,
            string title,
            int publishType,
            string issueDate,
            string brief,
            string singer,
            string author,
            string lang,
            string defaultpic,
            string sequence,
            bool isReUpload,
            FluorineFx.AMF3.ArrayCollection fileSet)
        {
            string result = "系统错误，保存失败";
            //owner = Com.Com.checkSql(owner);
            //title = Com.Com.checkSql(title);
            issueDate = Com.Com.checkSql(issueDate);
            brief = Com.Com.checkSql(brief);
            singer = Com.Com.checkSql(singer);
            author = Com.Com.checkSql(author);
            //DataAccess.DataAccess Access = new DataAccess.DataAccess();
            //string strSql = "select title from BaseItem where title='" + title + "' and category=" + category;
            //DataTable tempDt = Access.execSql(strSql);
            //if (tempDt != null && tempDt.Rows.Count > 0)
            //{
            //    result = "该名称的电影已经存在，增加失败。";
            //}
            //else
            //{
                //访问WEBSERVICE
                FT.BaseItem bi = new ManagementService.FT.BaseItem();
                bi.ID = Convert.ToInt64(id);
                bi.IssueDate = issueDate;
                bi.PublishType = publishType;
                bi.Brief = brief;
                FT.Music mv = new ManagementService.FT.Music();
                mv.Singer= singer;
                mv.Author= author;
                bi.Ext1 = defaultpic;
                bi.Ext2 = sequence;
                bi.Ext3 = lang;
                
                FT.MasterControl control = new ManagementService.FT.MasterControl();
                if (isReUpload)
                {//要重新上传文件
                    FT.FileSet fs = new ManagementService.FT.FileSet();
                    fs.Path = "音乐\\" + title + "\\";
                    ManagementService.FT.File[] files = new ManagementService.FT.File[fileSet.Count];
                    for (int i = 0; i < fileSet.Count; i++)
                    {
                        System.Collections.Hashtable uFileSet = (System.Collections.Hashtable)fileSet[i];
                        files[i] = new ManagementService.FT.File();
                        files[i].FileName = uFileSet["FILENAME"].ToString();
                        files[i].ShowIndex = i;
                    }
                    fs.File = files;
                    //重传文件
                }
                try
                {
                    control.Url = new Common.SysConfig().GetManterControl();
                    //result = "OK" + control.CommitMusic(bi, mv, fs, srcIp);
                    control.UpdateMusic(bi, mv);
                    result = "OK";
                }
                catch (System.Exception e)
                {
                    result = e.Message;
                }
            //}
            //Access.Dispose();
            return result;
        }

        //发布未通过可以让发布站点人员删除
        //未发布的应可以让审核人员删除
        [DataTableType("Music.DeleteMusic")]
        public DataTable DeleteMusic(FluorineFx.AMF3.ArrayCollection musics)
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("ID");
            DataColumn dc2 = new DataColumn("TITLE");
            DataColumn dc3 = new DataColumn("result");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            for (int i = 0; i < musics.Count; i++)
            {
                System.Collections.Hashtable uMusics = (System.Collections.Hashtable)musics[i];
                DataRow dr = dt.NewRow();
                dr["ID"] = uMusics["ID"];
                dr["TITLE"] = uMusics["TITLE"];
                dt.Rows.Add(dr);
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //访问WEBSERVICE
                string result = "";
                FT.MasterControl control = new ManagementService.FT.MasterControl();
                control.Url = new Common.SysConfig().GetManterControl();
                control.RemoveMusic(Convert.ToInt64(dt.Rows[i]["ID"]));
                dt.Rows[i]["result"] = result;
            }
            return dt;
        }

        public string AuditMusic(int id,int auditResult,string auditReason,string auditMan)
        {
            string result = "系统错误，审核失败。";
            auditReason = Com.Com.checkSql(auditReason);
            auditMan = Com.Com.checkSql(auditMan);

            FT.MasterControl control = new ManagementService.FT.MasterControl();
            control.Url = new Common.SysConfig().GetManterControl();
            try
            {
                control.Audit(id, auditResult, auditReason, auditMan);
                //control.Audit(id, auditResult, auditReason);
                result = "OK";
            }
            catch (System.Exception e)
            {
                result = e.Message;
            }
            return result;
        }

        [DataTableType("Music.GetFileSet")]
        public DataTable GetFileSet(string BaseItemId)
        {
            return null;
        }

    }
}
