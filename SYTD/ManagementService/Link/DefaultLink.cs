using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FluorineFx;
using System.IO;

namespace ManagementService.Link
{
    [RemotingService]
    public class DefaultLink
    {
        [DataTableType("DefaultLink.GetLinkList")]
        public DataTable GetLinkList(string linkName,
            string linkUrl,
            string addMan,
            string addStartTime,
            string addEndTime,
            string auditMan,
            string auditStartTime,
            string auditEndTime,
            string state,int pageSize,int pageIndex)
        {
            linkName = Com.Com.checkSql(linkName);
            linkUrl = Com.Com.checkSql(linkUrl);
            addMan = Com.Com.checkSql(addMan);
            addStartTime = Com.Com.checkSql(addStartTime);
            addEndTime = Com.Com.checkSql(addEndTime);
            auditMan = Com.Com.checkSql(auditMan);
            auditStartTime = Com.Com.checkSql(auditStartTime);
            auditEndTime = Com.Com.checkSql(auditEndTime);
            state = Com.Com.checkSql(state);

            string tblname = "T_Defaultlink";
            string fieldCollections = " 0 as ISCHECKED,";
            fieldCollections += "T_Defaultlink.ID,";
            fieldCollections += "T_Defaultlink.LINKNAME,";
            fieldCollections += "T_Defaultlink.LINKURL,";
            fieldCollections += "T_Defaultlink.DEFAULTPIC,";
            fieldCollections += "T_Defaultlink.LISTORDER,";
            fieldCollections += "T_Defaultlink.ADDMAN,";
            fieldCollections += "DateAdd(Hour,-8,T_Defaultlink.ADDTIME) as ADDTIME,";
            fieldCollections += "T_Defaultlink.AUDITMAN,";
            fieldCollections += "DateAdd(Hour,-8,T_Defaultlink.AUDITTIME) as AUDITTIME,";
            fieldCollections += "T_Defaultlink.SEQUENCE,";
            fieldCollections += "T_Defaultlink.STATE,";
            fieldCollections += "case T_Defaultlink.STATE when 0 then '待审核' when 1 then '已发布' else '未发布' end as STATENAME ";

            string orderField = "listOrder";
            int orderType = 0;


            string strWhere = "";
            if (linkName != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_Defaultlink.linkName link '%" + linkName + "%'";
            }
            if (linkUrl != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_Defaultlink.linkUrl link '%" + linkUrl + "%'";
            }
            if (addMan != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_Defaultlink.addMan='" + addMan + "'";
            }
            if (addStartTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_Defaultlink.addTime>='" + addStartTime + " 00:00:00'";
            }
            if (addEndTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_Defaultlink.addTime<='" + addEndTime + " 23:59:59'";
            }
            if (auditMan != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_Defaultlink.auditMan='" + auditMan + "'";
            }
            if (auditStartTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_Defaultlink.auditTime>='" + auditStartTime + " 00:00:00'";
            }
            if (auditEndTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_Defaultlink.auditTime<='" + auditEndTime + " 23:59:59'";
            }
            if (state != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_Defaultlink.state=" + state + "";
            }

            string joinConditions = "";

            Com.CommonPage common = new Com.CommonPage();
            DataTable dt = common.CommonQueryPage(tblname, fieldCollections, strWhere, joinConditions, orderField, orderType, pageSize, pageIndex);

            return dt;
        }

        [DataTableType("DefaultLink")]
        public DataTable GetLinkInfo(string id)
        {
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select ID,LINKNAME,LINKURL,DEFAULTPIC,LISTORDER,ADDMAN,ADDTIME,AUDITMAN,AUDITTIME,STATE,SEQUENCE from T_DefaultLink where id=" + id;
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }

        [DataTableType("DefaultLink.AddLink")]
        public DataTable AddLink(string linkName,string linkUrl,string defaultPic,string addMan,string sequence)
        {
            DataTable dtResult = new DataTable();
            DataColumn dc1 = new DataColumn("LINKNAME");
            DataColumn dc2 = new DataColumn("ID");
            DataColumn dc3 = new DataColumn("LISTORDER");
            DataColumn dc4 = new DataColumn("result");
            dtResult.Columns.Add(dc1);
            dtResult.Columns.Add(dc2);
            dtResult.Columns.Add(dc3);
            dtResult.Columns.Add(dc4);

            linkName = Com.Com.checkSql(linkName);
            linkUrl = Com.Com.checkSql(linkUrl);
            defaultPic = Com.Com.checkSql(defaultPic);
            addMan = Com.Com.checkSql(addMan);
            sequence = Com.Com.checkSql(sequence);

            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select id from T_DefaultLink where linkName = '" + linkName + "'";
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt!=null && tempDt.Rows.Count>0)
            {
                DataRow dr = dtResult.NewRow();
                dr["LINKNAME"] = linkName;
                dr["ID"] = "";
                dr["LISTORDER"] = "";
                dr["result"] = "该名称的连接已存在";
                dtResult.Rows.Add(dr);
            }
            else
            {
                string listOrder = "1";
                strSql = "select count(*)+1 from T_DefaultLink";
                tempDt = Access.execSql(strSql);
                if (tempDt!=null && tempDt.Rows.Count>0)
                {
                    listOrder = tempDt.Rows[0][0].ToString();
                }
                strSql = "insert into T_DefaultLink(linkName,linkUrl,defaultPic,listOrder,addMan,state,sequence) ";
                strSql += " values('" + linkName + "','" + linkUrl + "','" + defaultPic + "','" + listOrder + "','" + addMan + "',0,'" + sequence + "')";
                if (Access.execSqlNoQuery1(strSql))
                {
                    string id = "";
                    tempDt = Access.execSql("select @@IDENTITY");
                    if (tempDt!=null && tempDt.Rows.Count>0)
                    {
                        id = tempDt.Rows[0][0].ToString();
                    }
                    DataRow dr = dtResult.NewRow();
                    dr["LINKNAME"] = linkName;
                    dr["ID"] = id;
                    dr["LISTORDER"] = listOrder;
                    dr["result"] = "OK";
                    dtResult.Rows.Add(dr);
                }
                else
                {
                    DataRow dr = dtResult.NewRow();
                    dr["LINKNAME"] = linkName;
                    dr["ID"] = "";
                    dr["LISTORDER"] = "";
                    dr["result"] = "系统错误，保存失败。";
                    dtResult.Rows.Add(dr);
                }
                
            }
            Access.Dispose();
            return dtResult;

        }

        public string UpdateLink(string id,string linkName,string linkUrl,string defaultPic,string addMan)
        {
            string result = "系统错误，保存失败。";
            id = Com.Com.checkSql(id);
            linkName = Com.Com.checkSql(linkName);
            linkUrl = Com.Com.checkSql(linkUrl);
            defaultPic = Com.Com.checkSql(defaultPic);
            addMan = Com.Com.checkSql(addMan);
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select id from T_DefaultLink where linkName='" + linkName + "' and id<>" + id;
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt!=null && tempDt.Rows.Count>0)
            {
                result += "该名称的连接已存在，保存失败。";
            }
            else
            {
                strSql = "update T_DefaultLink  set ";
                strSql += "linkName='"+linkName+"',";
                strSql += "linkUrl='"+linkUrl+"',";
                strSql += "defaultPic='"+defaultPic+"',";
                strSql += "addMan='" + addMan + "',";
                strSql += "addTime='" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "'";
                strSql += " where id=" + id;
                if (Access.execSqlNoQuery1(strSql))
                {
                    result = "OK";
                }
            }
            Access.Dispose();
            return result;
        }

        [DataTableType("DefaultLink.DeleteLink")]
        public DataTable DeleteLink(FluorineFx.AMF3.ArrayCollection links)
        {
            string type = "DEFAULTLINK";
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("ID");
            DataColumn dc2 = new DataColumn("LINKNAME");
            DataColumn dc3 = new DataColumn("LINKURL");
            DataColumn dc4 = new DataColumn("DEFAULTPIC");
            DataColumn dc5 = new DataColumn("SEQUENCE");
            DataColumn dc6 = new DataColumn("result");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);
            dt.Columns.Add(dc5);
            dt.Columns.Add(dc6);

            for (int i = 0; i < links.Count; i++)
            {
                System.Collections.Hashtable uLink = (System.Collections.Hashtable)links[i];
                DataRow dr = dt.NewRow();
                dr["ID"] = uLink["ID"].ToString();
                dr["LINKNAME"] = uLink["LINKNAME"].ToString();
                dr["LINKURL"] = uLink["LINKURL"].ToString();
                dr["DEFAULTPIC"] = uLink["DEFAULTPIC"].ToString();
                dr["SEQUENCE"] = uLink["SEQUENCE"].ToString();
                dr["result"] = "";
                dt.Rows.Add(dr);
            }
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Access.execSqlNoQuery1("delete T_DefaultLink where id=" + dt.Rows[i]["ID"].ToString()))
                {
                    ////////删除图片
                    string path = System.AppDomain.CurrentDomain.BaseDirectory + "UploadFile\\" + type + "\\" + dt.Rows[i]["SEQUENCE"].ToString();
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }
                    dt.Rows[i]["result"] = "OK";
                }
                else
                {
                    dt.Rows[i]["result"] = "失败";
                }
            }
            Access.Dispose();

            return dt;
        }

        public string PicSave(string linkId, string defaultPic)
        {
            linkId = Com.Com.checkSql(linkId);
            defaultPic = Com.Com.checkSql(defaultPic);
            string result = "系统错误，保存图片至数据库失败。";
            string strSql = "update T_DefaultLink set defaultPic='" + defaultPic + "' where id=" + linkId;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            if (Access.execSqlNoQuery1(strSql))
            {
                result = "OK";
            }
            Access.Dispose();
            return result;
        }

        public bool linkSort(string[] ids)
        {
            bool result = false;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            for (int i = 0; i < ids.Length; i++)
            {
                Access.execSqlNoQuery1("update T_DefaultLink set listOrder = " + (i + 1).ToString() + " where id=" + ids[i].ToString());

            }
            Access.Dispose();
            result = true;
            return result;
        }

        public string BatchPub(string[] ids,string auditMan)
        {
            string result = "系统错误，批量发布失败。";
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
            string strSql = "update T_DefaultLink set auditMan='" + auditMan + "',auditTime='" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',state=1  where id in (" + str + ")";
            if (Access.execSqlNoQuery1(strSql))
            {
                result = "OK";
            }
            Access.Dispose();
            return result;
        }

        public string BatchNotPub(string[] ids,string auditMan)
        {
            string result = "系统错误，批量不发布失败。";
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
            string strSql = "update T_DefaultLink set auditMan='" + auditMan + "',auditTime='" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',state=-1  where id in (" + str + ")";
            if (Access.execSqlNoQuery1(strSql))
            {
                result = "OK";
            }
            Access.Dispose();
            return result;
        }

        public string PubLink(string id,string linkName,string linkUrl,string defaultPic,string auditMan)
        {
            string result = "系统错误，保存失败。";
            id = Com.Com.checkSql(id);
            linkName = Com.Com.checkSql(linkName);
            linkUrl = Com.Com.checkSql(linkUrl);
            defaultPic = Com.Com.checkSql(defaultPic);
            auditMan = Com.Com.checkSql(auditMan);
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select id from T_DefaultLink where linkName='" + linkName + "' and id<>" + id;
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                result += "该名称的连接已存在，审核失败。";
            }
            else
            {
                strSql = "update T_DefaultLink  set ";
                strSql += "linkName='" + linkName + "',";
                strSql += "linkUrl='" + linkUrl + "',";
                strSql += "defaultPic='" + defaultPic + "',";
                strSql += "auditMan='" + auditMan + "',";
                strSql += "auditTime='"+System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',";
                strSql += "state=1";
                strSql += " where id=" + id;
                if (Access.execSqlNoQuery1(strSql))
                {
                    result = "OK";
                }
            }
            Access.Dispose();
            return result;
        }

        public string NotPubLink(string id,string linkName,string linkUrl,string defaultPic,string auditMan)
        {
            string result = "系统错误，保存失败。";
            id = Com.Com.checkSql(id);
            linkName = Com.Com.checkSql(linkName);
            linkUrl = Com.Com.checkSql(linkUrl);
            defaultPic = Com.Com.checkSql(defaultPic);
            auditMan = Com.Com.checkSql(auditMan);
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select id from T_DefaultLink where linkName='" + linkName + "' and id<>" + id;
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                result += "该名称的连接已存在，审核失败。";
            }
            else
            {
                strSql = "update T_DefaultLink  set ";
                strSql += "linkName='" + linkName + "',";
                strSql += "linkUrl='" + linkUrl + "',";
                strSql += "defaultPic='" + defaultPic + "',";
                strSql += "auditMan='" + auditMan + "',";
                strSql += "auditTime='" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',";
                strSql += "state=-1";
                strSql += " where id=" + id;
                if (Access.execSqlNoQuery1(strSql))
                {
                    result = "OK";
                }
            }
            Access.Dispose();
            return result;
        }
    }
}
