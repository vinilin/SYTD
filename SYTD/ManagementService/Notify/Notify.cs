using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FluorineFx;
using System.IO;

namespace ManagementService.Notify
{
    [RemotingService]
    public class Notify
    {


        [DataTableType("Notify.GetNotifyList")]
        public DataTable GetNotifyList(string subCode,
            string notifyTitle,
            string pubStartTime,
            string pubEndTime,
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
            subCode = Com.Com.checkSql(subCode);
            notifyTitle = Com.Com.checkSql(notifyTitle);
            pubStartTime = Com.Com.checkSql(pubStartTime);
            pubEndTime = Com.Com.checkSql(pubEndTime);
            addMan = Com.Com.checkSql(addMan);
            addStartTime = Com.Com.checkSql(addStartTime);
            addEndTime = Com.Com.checkSql(addEndTime);
            auditMan = Com.Com.checkSql(auditMan);
            auditStartTime = Com.Com.checkSql(auditStartTime);
            auditEndTime = Com.Com.checkSql(auditEndTime);
            state = Com.Com.checkSql(state);

            string tblname = "T_Notify";
            string fieldCollections = " 0 as ISCHECKED,";
            fieldCollections += "T_Notify.ID,";
            fieldCollections += "T_Notify.SUBCODE,";
            fieldCollections += "case T_Notify.SUBCODE when 'global' then '全局' else T_SubSection.SUBNAME end as SUBNAME,";
            fieldCollections += "T_Notify.NOTIFYTITLE,";
            fieldCollections += "DateAdd(Hour,-8,T_Notify.pubStartTime) as PUBSTARTTIME,";
            fieldCollections += "DateAdd(Hour,-8,T_Notify.pubEndTime) as PUBENDTIME,";
            fieldCollections += "DateAdd(Hour,-8,T_Notify.addTime) as ADDTIME,";
            fieldCollections += "T_Notify.ADDMAN,";
            fieldCollections += "T_Notify.AUDITMAN,";
            fieldCollections += "DateAdd(Hour,-8,T_Notify.auditTime) as AUDITTIME,";
            fieldCollections += "T_Notify.SEQUENCE,";
            fieldCollections += "T_Notify.STATE,";
            fieldCollections += "case T_Notify.state when -1 then '未发布' when 1 then '已发布' else '待审核' end as STATENAME";

            string orderField = "id";
            int orderType = 1;


            string strWhere = "";
            if (subCode != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_Notify.subCode = '" + subCode + "'";
            }
            if (notifyTitle != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_Notify.notifyTitle like '%" + notifyTitle + "%'";
            }
            if (addStartTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_Notify.pubStartTime>='" + pubStartTime + " 00:00:00'";
            }
            if (addEndTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_Notify.pubEndTime<='" + pubEndTime + " 23:59:59'";
            }
            if (addMan != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_Notify.addMan='" + addMan + "'";
            }
            if (addStartTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_Notify.addTime>='" + addStartTime + " 00:00:00'";
            }
            if (addEndTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_Notify.addTime<='" + addEndTime + " 23:59:59'";
            }
            if (auditMan != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_Notify.auditMan='" + auditMan + "'";
            }
            if (auditStartTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_Notify.auditTime>='" + auditStartTime + " 00:00:00'";
            }
            if (auditEndTime != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_Notify.auditTime<='" + auditEndTime + " 23:59:59'";
            }
            if (state != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_Notify.state=" + state + "";
            }

            string joinConditions = " left join T_SubSection on T_SubSection.SubCode=T_Notify.SubCode ";

            Com.CommonPage common = new Com.CommonPage();
            DataTable dt = common.CommonQueryPage(tblname, fieldCollections, strWhere, joinConditions, orderField, orderType, pageSize, pageIndex);

            return dt;
        }

        [DataTableType("Nofigy.GetNofigyInfo")]
        public DataTable GetNotifyInfo(string notifyId)
        {
            notifyId = Com.Com.checkSql(notifyId);
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select SUBCODE,NOTIFYTITLE,NOTIFYCONTENT,DateAdd(Hour,-8,PUBSTARTTIME) AS PUBSTARTTIME,DateAdd(Hour,-8,PUBENDTIME) AS PUBENDTIME,ADDMAN,ADDTIME,AUDITMAN,AUDITTIME,STATE,SEQUENCE from T_Notify where id=" + notifyId;
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }

        public string AddNotify(string subCode,string notifyTitle,string notifyContent,string pubStartTime,string pubEndTime,string addMan,string sequence)
        {
            subCode = Com.Com.checkSql(subCode);
            notifyTitle = Com.Com.checkSql(notifyTitle);
            notifyContent = Com.Com.checkSql(notifyContent);
            pubStartTime = Com.Com.checkSql(pubStartTime);
            pubEndTime = Com.Com.checkSql(pubEndTime);
            addMan = Com.Com.checkSql(addMan);
            sequence = Com.Com.checkSql(sequence);

            string result = "系统错误，保存失败。";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "insert into T_Notify(subCode,notifyTitle,notifyContent,pubStartTime,pubEndTime,addMan,sequence,state) ";
            strSql += " values('" + subCode + "','" + notifyTitle + "','" + notifyContent + "','" + pubStartTime + "','" + pubEndTime + "','" + addMan + "','" + sequence + "',0)";
            if (Access.execSqlNoQuery1(strSql))
            {
                DataTable tempDt = Access.execSql("select @@IDENTITY");
                if (tempDt!=null && tempDt.Rows.Count>0)
                {
                    result = "OK" + tempDt.Rows[0][0].ToString();
                }
            }
            Access.Dispose();
            return result;
        }

        public string UpdateNotify(string notifyId,string subCode,string notifyTitle,string notifyContent,string pubStartTime,string pubEndTime,string addMan,string sequence)
        {
            subCode = Com.Com.checkSql(subCode);
            notifyTitle = Com.Com.checkSql(notifyTitle);
            notifyContent = Com.Com.checkSql(notifyContent);
            pubStartTime = Com.Com.checkSql(pubStartTime);
            pubEndTime = Com.Com.checkSql(pubEndTime);
            addMan = Com.Com.checkSql(addMan);
            sequence = Com.Com.checkSql(sequence);

            string result = "系统错误，保存失败。";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "update T_Notify set ";
            strSql += "subCode='" + subCode + "',";
            strSql += "notifyTitle='" + notifyTitle + "',";
            strSql += "notifyContent='" + notifyContent + "',";
            strSql += "pubStartTime='" + pubStartTime + "',";
            strSql += "pubEndTime='" + pubEndTime + "',";
            strSql += "addMan='" + addMan + "',";
            strSql += "addTime='"+System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',";
            strSql += "sequence= '" + sequence + "',";
            strSql += "state= 0,";
            strSql += "auditMan ='',";
            strSql += "auditTime = null";
            strSql += " where id=" + notifyId;
            if (Access.execSqlNoQuery1(strSql))
            {
                result = "OK" + notifyId;
            }
            Access.Dispose();
            return result;
        }

        [DataTableType("Notify.DeleteNotify")]
        public DataTable DeleteNotify(FluorineFx.AMF3.ArrayCollection notifys,string type)
        {
            type = Com.Com.checkSql(type);
            DataTable resultDt = new DataTable();
            DataColumn dc1 = new DataColumn("ID");
            DataColumn dc2 = new DataColumn("NOTIFYTITLE");
            DataColumn dc3 = new DataColumn("SEQUENCE");
            DataColumn dc4 = new DataColumn("result");
            resultDt.Columns.Add(dc1);
            resultDt.Columns.Add(dc2);
            resultDt.Columns.Add(dc3);
            resultDt.Columns.Add(dc4);

            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            for (int i = 0; i < notifys.Count; i++)
            {
                System.Collections.Hashtable uNotify = (System.Collections.Hashtable)notifys[i];
                string test = "select state from T_Notify where id=" + uNotify["ID"].ToString();
                DataTable tempDt = Access.execSql("select state from T_Notify where id=" + uNotify["ID"].ToString());
                if (tempDt != null && tempDt.Rows.Count > 0)
                {
                    if (tempDt.Rows[0]["state"].ToString() == "1")
                    {
                        DataRow dr = resultDt.NewRow();
                        dr["ID"] = uNotify["ID"].ToString();
                        dr["NOTIFYTITLE"] = uNotify["NOTIFYTITLE"].ToString();
                        dr["SEQUENCE"] = uNotify["SEQUENCE"].ToString();
                        dr["result"] = "该信息已发布，不能删除。";
                        resultDt.Rows.Add(dr);
                    }
                    else
                    {
                        if (Access.execSqlNoQuery1("delete T_Notify where id=" + uNotify["ID"].ToString()))
                        {
                            try
                            {
                                string path = System.Web.HttpContext.Current.Server.MapPath(Com.Com.picUrl + "/"+ type + "/" + uNotify["SEQUENCE"].ToString() + "/");
                                if (Directory.Exists(path))
                                {
                                    Directory.Delete(path, true);
                                }
                            }
                            catch { }
                            DataRow dr = resultDt.NewRow();
                            dr["ID"] = uNotify["ID"].ToString();
                            dr["NOTIFYTITLE"] = uNotify["NOTIFYTITLE"].ToString();
                            dr["SEQUENCE"] = uNotify["SEQUENCE"].ToString();
                            dr["result"] = "OK";
                            resultDt.Rows.Add(dr);
                        }
                        else
                        {
                            DataRow dr = resultDt.NewRow();
                            dr["ID"] = uNotify["ID"].ToString();
                            dr["NOTIFYTITLE"] = uNotify["NOTIFYTITLE"].ToString();
                            dr["SEQUENCE"] = uNotify["SEQUENCE"].ToString();
                            dr["result"] = "该信息已发布，不能删除。";
                            resultDt.Rows.Add(dr);
                        }
                    }
                }
                else
                {
                    DataRow dr = resultDt.NewRow();
                    dr["ID"] = uNotify["ID"].ToString();
                    dr["NOTIFYTITLE"] = uNotify["NOTIFYTITLE"].ToString();
                    dr["SEQUENCE"] = uNotify["SEQUENCE"].ToString();
                    dr["result"] = "系统错误，删除失败。";
                    resultDt.Rows.Add(dr);
                }
            }
            Access.Dispose();
            return resultDt;
        }

        public string PubNotify(string notifyId,string subCode,string notifyTitle,string notifyContent,string pubStartTime,string pubEndTime,string auditMan,string sequence)
        {
            subCode = Com.Com.checkSql(subCode);
            notifyTitle = Com.Com.checkSql(notifyTitle);
            notifyContent = Com.Com.checkSql(notifyContent);
            pubStartTime = Com.Com.checkSql(pubStartTime);
            pubEndTime = Com.Com.checkSql(pubEndTime);
            auditMan = Com.Com.checkSql(auditMan);
            sequence = Com.Com.checkSql(sequence);

            string result = "系统错误，保存失败。";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "update T_Notify set ";
            strSql += "subCode='" + subCode + "',";
            strSql += "notifyTitle='" + notifyTitle + "',";
            strSql += "notifyContent='" + notifyContent + "',";
            strSql += "pubStartTime='" + pubStartTime + "',";
            strSql += "pubEndTime='" + pubEndTime + "',";
            strSql += "auditMan='" + auditMan + "',";
            strSql += "auditTime='" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
            strSql += "state= 1";
            strSql += " where id=" + notifyId;
            if (Access.execSqlNoQuery1(strSql))
            {
                result = "OK" + notifyId;
            }
            Access.Dispose();
            return result;
        }

        public string NotPubNotify(string notifyId,string subCode,string notifyTitle,string notifyContent,string pubStartTime,string pubEndTime,string auditMan,string sequence)
        {
            subCode = Com.Com.checkSql(subCode);
            notifyTitle = Com.Com.checkSql(notifyTitle);
            notifyContent = Com.Com.checkSql(notifyContent);
            pubStartTime = Com.Com.checkSql(pubStartTime);
            pubEndTime = Com.Com.checkSql(pubEndTime);
            auditMan = Com.Com.checkSql(auditMan);
            sequence = Com.Com.checkSql(sequence);

            string result = "系统错误，保存失败。";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "update T_Notify set ";
            strSql += "subCode='" + subCode + "',";
            strSql += "notifyTitle='" + notifyTitle + "',";
            strSql += "notifyContent='" + notifyContent + "',";
            strSql += "pubStartTime='" + pubStartTime + "',";
            strSql += "pubEndTime='" + pubEndTime + "',";
            strSql += "auditMan='" + auditMan + "',";
            strSql += "auditTime='" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";
            strSql += "state= -1";
            strSql += " where id=" + notifyId;
            if (Access.execSqlNoQuery1(strSql))
            {
                result = "OK" + notifyId;
            }
            Access.Dispose();
            return result;
        }

        public string BatchPub(string[] ids,string auditMan)
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
            string strSql = "update T_Notify set auditMan='" + auditMan + "',auditTime='" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',state=1  where id in (" + str + ")";
            if (Access.execSqlNoQuery1(strSql))
            {
                result = "OK";
            }
            Access.Dispose();
            return result;
        }

        public string BatchNotPub(string[] ids,string auditMan)
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
            string strSql = "update T_Notify set auditMan='" + auditMan + "',auditTime='" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',state=-1  where id in (" + str + ")";
            if (Access.execSqlNoQuery1(strSql))
            {
                result = "OK";
            }
            Access.Dispose();
            return result;
        }

        [DataTableType("Notify.GetRoleByUserName")]
        public DataTable GetRoleByUserName(string userName,string roleCode)
        {
            string strSql = "select roleCode from T_SystemUserRole where roleCode='" + roleCode + "' and userName = '" + userName + "'";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }
    }
}
