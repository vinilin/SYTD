using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FluorineFx;

namespace ManagementService.Sys
{
    [RemotingService]
    public class SubIp
    {
        [DataTableType("SubIp.GetIpList")]
        public DataTable GetIPList(string subCode, string ipStr, int pageSize, int pageIndex)
        {
            subCode = Com.Com.checkSql(subCode);
            ipStr = Com.Com.checkSql(ipStr);

            string tblname = "T_SubIpDivision";
            string fieldCollections = " 0 as ISCHECKED,";
            fieldCollections += "T_SubIpDivision.ID,";
            fieldCollections += "T_SubIpDivision.SUBCODE,";
            fieldCollections += "T_SubIpDivision.STARTIP,";
            fieldCollections += "T_SubIpDivision.ENDIP,";
            fieldCollections += "T_SubSection.SUBNAME";

            string orderField = "id";
            int orderType = 1;

            Com.IP ip = new ManagementService.Com.IP();
            string strWhere = "";
            if (subCode != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_SubIpDivision.subCode='" + subCode + "'";
            }
            if (ipStr != "" && ip.IpCheck(ipStr))
            {
                uint ipNumber = ip.IpConvertInt(ipStr);
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_SubIpDivision.startNumber <= " + ipNumber + " and T_SubIpDivision.endNumber>=" + ipNumber + "";
            }


            string joinConditions = " left join T_SubSection on T_SubSection.subCode=T_SubIpDivision.subCode ";

            Com.CommonPage common = new Com.CommonPage();
            DataTable dt = common.CommonQueryPage(tblname, fieldCollections, strWhere, joinConditions, orderField, orderType, pageSize, pageIndex);


            return dt;
        }

        [DataTableType("SubIp.GetIpInfo")]
        public DataTable GetIpInfoById(string id)
        {
            id = Com.Com.checkSql(id);
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql("select SUBCODE,STARTIP,ENDIP from T_SubIpDivision where id=" + id);
            Access.Dispose();
            return dt;
        }

        public string AddIp(string subCode, string startIp, string endIp)
        {
            string result = "系统错误，保存失败";
            subCode = Com.Com.checkSql(subCode);
            startIp = Com.Com.checkSql(startIp);
            endIp = Com.Com.checkSql(endIp);

            Com.IP ip=new Com.IP();
            if (!ip.IpCheck(startIp))
            {
                result = "开始地址不是有效的IP地址";
            }
            else
            {
                if (!ip.IpCheck(endIp))
                {
                    result = "结束地址不是有效的IP地址";
                }
                else
                {
                    uint startNumber = ip.IpConvertInt(startIp);
                    uint endNumber = ip.IpConvertInt(endIp);
                    if (startNumber > endNumber)
                    {
                        result = "结束IP是开始IP之前的IP地址，保存失败";
                    }
                    else
                    {
                        DataAccess.DataAccess Access = new DataAccess.DataAccess();
                        string strSql = "select T_SubIpDivision.subCode,T_SubSection.subName from T_SubIpDivision ";
                        strSql += " left join T_SubSection on T_SubSection.subCode=T_SubIpDivision.subCode ";
                        strSql += " where (T_SubIpDivision.startNumber<=" + startNumber + " and T_SubIpDivision.endNumber>=" + startNumber + ") ";
                        strSql += " or (T_SubIpDivision.startNumber<=" + endNumber + " and T_SubIpDivision.endNumber>=" + endNumber + ")";
                        strSql += " or (T_SubIpDivision.startNumber>=" + startNumber + " and T_SubIpDivision.endNumber<=" + endNumber + ") ";
                        //strSql += " and T_SubIpDivision.subCode<>'" + subCode + "'";
                        DataTable tempDt = Access.execSql(strSql);
                        if (tempDt != null && tempDt.Rows.Count > 0)
                        {
                            result = "你添加的IP段与“" + tempDt.Rows[0]["subName"].ToString() + "”站点的IP有交差，保存失败。";
                        }
                        else
                        {
                            strSql = "insert into T_SubIpDivision(subCode,startIp,endIp,startNumber,endNumber)";
                            strSql += " values('" + subCode + "','" + startIp + "','" + endIp + "'," + startNumber + "," + endNumber + ")";
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
                    }
                }
            }
            return result;
        }

        public string UpdateIp(string id, string subCode, string startIp, string endIp)
        {
            string result = "系统错误，保存失败";
            subCode = Com.Com.checkSql(subCode);
            startIp = Com.Com.checkSql(startIp);
            endIp = Com.Com.checkSql(endIp);
            Com.IP ip = new ManagementService.Com.IP();
            if (!ip.IpCheck(startIp))
            {
                result = "开始地址不是有效的IP地址";
            }
            else
            {
                if (!ip.IpCheck(endIp))
                {
                    result = "结束地址不是有效的IP地址";
                }
                else
                {
                    uint startNumber = ip.IpConvertInt(startIp);
                    uint endNumber = ip.IpConvertInt(endIp);
                    if (startNumber > endNumber)
                    {
                        result = "结束IP是开始IP之前的IP地址，保存失败";
                    }
                    else
                    {
                        DataAccess.DataAccess Access = new DataAccess.DataAccess();
                        string strSql = "select T_SubIpDivision.subCode,T_SubIpDivision.startIp,T_SubIpDivision.endIp,T_SubSection.subName from T_SubIpDivision ";
                        strSql += " left join T_SubSection on T_SubSection.subCode=T_SubIpDivision.subCode ";
                        strSql += " where ((T_SubIpDivision.startNumber<=" + startNumber + " and T_SubIpDivision.endNumber>=" + startNumber + ") ";
                        strSql += " or (T_SubIpDivision.startNumber<=" + endNumber + " and T_SubIpDivision.endNumber>=" + endNumber + ")";
                        strSql += " or (T_SubIpDivision.startNumber>=" + startNumber + " and T_SubIpDivision.endNumber<=" + endNumber + ") ";
                        strSql += ") and T_SubIpDivision.id<>" + id;
                        DataTable tempDt = Access.execSql(strSql);
                        if (tempDt != null && tempDt.Rows.Count > 0)
                        {
                            result = "你添加的IP段与站点“" + tempDt.Rows[0]["subName"].ToString() + "”的IP：" + tempDt.Rows[0]["startIp"].ToString() + "——" + tempDt.Rows[0]["endIp"].ToString() + "有交差，保存失败。";
                        }
                        else
                        {
                            strSql = "update T_SubIpDivision set ";
                            strSql += "subCode='" + subCode + "',";
                            strSql += "startIp='" + startIp + "',";
                            strSql += "endIp='" + endIp + "',";
                            strSql += "startNumber=" + startNumber + ",";
                            strSql += "endNumber=" + endNumber + "";
                            strSql += " where id=" + id;
                            if (Access.execSqlNoQuery1(strSql))
                            {

                                result = "OK" + id.ToString();

                            }
                        }
                        Access.Dispose();
                    }
                }
            }
            return result;
        }

        [DataTableType("SubIp.DeleteIp")]
        public DataTable DeleteIp(FluorineFx.AMF3.ArrayCollection ips)
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("ID");
            DataColumn dc2 = new DataColumn("SUBCODE");
            DataColumn dc3 = new DataColumn("SUBNAME");
            DataColumn dc4 = new DataColumn("STARTIP");
            DataColumn dc5 = new DataColumn("ENDIP");
            DataColumn dc6 = new DataColumn("result");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);
            dt.Columns.Add(dc5);
            dt.Columns.Add(dc6);
            if (ips != null)
            {
                DataAccess.DataAccess Access = new DataAccess.DataAccess();
                for (int i = 0; i < ips.Count; i++)
                {
                    System.Collections.Hashtable uInfo = (System.Collections.Hashtable)ips[i];
                    if (Access.execSqlNoQuery1("delete T_SubIpDivision where id=" + uInfo["ID"].ToString() + ""))
                    {
                        DataRow dr = dt.NewRow();
                        dr["ID"] = uInfo["ID"].ToString();
                        dr["SUBCODE"] = uInfo["SUBCODE"].ToString();
                        dr["SUBNAME"] = uInfo["SUBNAME"].ToString();
                        dr["STARTIP"] = uInfo["STARTIP"].ToString();
                        dr["ENDIP"] = uInfo["ENDIP"].ToString();
                        dr["result"] = "0";
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        dr["ID"] = uInfo["ID"].ToString();
                        dr["SUBCODE"] = uInfo["SUBCODE"].ToString();
                        dr["SUBNAME"] = uInfo["SUBNAME"].ToString();
                        dr["STARTIP"] = uInfo["STARTIP"].ToString();
                        dr["ENDIP"] = uInfo["ENDIP"].ToString();
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
