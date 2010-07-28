using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FluorineFx;

namespace ManagementService.Sys
{
    [RemotingService]    
    public class SubSection
    {
        [DataTableType("SubSection.GetSubList")]
        public DataTable GetSubList(string subCode,string subName,int pageSize,int pageIndex)
        {
            subCode = Com.Com.checkSql(subCode);
            subName = Com.Com.checkSql(subName);
            string tblname = "T_SubSection";
            string fieldCollections = " 0 as ISCHECKED,";
            fieldCollections += "T_SubSection.ID,";
            fieldCollections += "T_SubSection.SUBCODE,";
            fieldCollections += "T_SubSection.SUBNAME,";
            fieldCollections += "T_SubSection.SERVERIP,";
            fieldCollections += "T_SubSection.ISCENTER,";
            fieldCollections += "case T_SubSection.isCenter when 1 then '主站' else '分站' end as ISCENTERNAME";

            string orderField = "SUBCODE";
            int orderType = 0;


            string strWhere = "";
            if (subCode != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_SubSection.subCode like '%" + subCode + "%'";
            }
            if (subName != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_SubSection.subName like '%" + subName + "%'";
            }
            string joinConditions = "";


            Com.CommonPage common = new Com.CommonPage();
            DataTable dt = common.CommonQueryPage(tblname, fieldCollections, strWhere, joinConditions, orderField, orderType, pageSize, pageIndex);

            return dt;
        }

        [DataTableType("SubSection.GetSubInfoById")]
        public DataTable GetSubInfoById(string id)
        {
            string strSql = "select ID,SUBCODE,SUBNAME,ISCENTER,SERVERIP from T_SubSection where id=" + id;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }

        [DataTableType("SubSection.GetAllSubList")]
        public DataTable GetAllSubList()
        {
            string strSql = "select SUBCODE,SUBNAME from T_SubSection order by isCenter desc,subCode Asc";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }

        public string AddSub(string subCode,string subName,string serverIp,string isCenter)
        {
            string result = "系统错误，保存失败";
            if (!new Com.IP().IpCheck(serverIp))
            {
                result = "服务器IP地址不是一个有效的IP地址";
                return result;
            }
            subCode = Com.Com.checkSql(subCode);
            subName = Com.Com.checkSql(subName);
            serverIp = Com.Com.checkSql(serverIp);
            isCenter = Com.Com.checkSql(isCenter);
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select subCode from T_SubSection where subCode='" + subCode + "'";
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt!=null && tempDt.Rows.Count>0)
            {
                result = "分站编号重复";
            }
            else
            {
                strSql = "select subName from T_SubSection where subName='" + subName + "'";
                tempDt = Access.execSql(strSql);
                if (tempDt!=null && tempDt.Rows.Count>0)
                {
                    result = "分站名称重复";
                }
                else
                {
                    strSql = "insert into T_SubSection(subCode,subName,serverIp,isCenter) ";
                    strSql += " values('" + subCode + "','" + subName + "','" + serverIp + "'," + isCenter + ")";
                    if (Access.execSqlNoQuery1(strSql))
                    {
                        tempDt = Access.execSql("select @@IDENTITY");
                        if (tempDt!=null && tempDt.Rows.Count>0)
                        {
                            result = "OK" + tempDt.Rows[0][0].ToString();
                        }
                    }
                }
            }
            Access.Dispose();
            return result;
        }

        public string UpdateSub(string id,string subCode,string subName,string serverIp,string isCenter)
        {
            string result = "系统错误，保存失败";
            if (!new Com.IP().IpCheck(serverIp))
            {
                result = "服务器IP地址不是一个有效的IP地址";
                return result;
            }
            subCode = Com.Com.checkSql(subCode);
            subName = Com.Com.checkSql(subName);
            serverIp = Com.Com.checkSql(serverIp);
            isCenter = Com.Com.checkSql(isCenter);
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select subCode from T_SubSection where subCode='" + subCode + "' and id<>" + id;
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                result = "分站编号重复";
            }
            else
            {
                strSql = "select subName from T_SubSection where subName='" + subName + "' and id<>" + id;
                tempDt = Access.execSql(strSql);
                if (tempDt != null && tempDt.Rows.Count > 0)
                {
                    result = "分站名称重复";
                }
                else
                {
                    strSql = "update T_SubSection set ";
                    strSql += "subCode='" + subCode + "',";
                    strSql += "subName='" + subName + "',";
                    strSql += "serverIp='" + serverIp + "',";
                    strSql += "isCenter= " + isCenter + "";
                    strSql += " where id=" + id;
                    if (Access.execSqlNoQuery1(strSql))
                    {
                        result = "OK" + id;
                    }
                }
            }
            Access.Dispose();
            return result;
        }

        [DataTableType("SubSection.DeleteSub")]
        public DataTable  DeleteSub(FluorineFx.AMF3.ArrayCollection subs)
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("ID");
            DataColumn dc2 = new DataColumn("SUBCODE");
            DataColumn dc3 = new DataColumn("SUBNAME");
            DataColumn dc4 = new DataColumn("result");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);
            if (subs != null)
            {
                DataAccess.DataAccess Access = new DataAccess.DataAccess();
                for (int i = 0; i < subs.Count; i++)
                {
                    System.Collections.Hashtable uInfo = (System.Collections.Hashtable)subs[i];
                    if (Access.execSqlNoQuery1("delete T_SubSection where id=" + uInfo["ID"].ToString() + ""))
                    {
                        DataRow dr = dt.NewRow();
                        dr["ID"] = uInfo["ID"].ToString();
                        dr["SUBCODE"] = uInfo["SUBCODE"].ToString();
                        dr["SUBNAME"] = uInfo["SUBNAME"].ToString();
                        dr["result"] = "0";
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        dr["ID"] = uInfo["ID"].ToString();
                        dr["SUBCODE"] = uInfo["SUBCODE"].ToString();
                        dr["SUBNAME"] = uInfo["SUBNAME"].ToString();
                        dr["result"] = "失败。";
                        dt.Rows.Add(dr);
                    }
                }
                Access.Dispose();
            }
            return dt;
        }
    }
}
