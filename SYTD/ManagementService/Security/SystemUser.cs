using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FluorineFx;

namespace ManagementService.Security
{
    [RemotingService]
    public class SystemUser
    {
        [DataTableType("SystemUser.GetUserList")]
        public DataTable GetUserList(string userName, string trueName, string subCode, int pageSize, int pageIndex)
        {
            userName = Com.Com.checkSql(userName);
            trueName = Com.Com.checkSql(trueName);
            subCode = Com.Com.checkSql(subCode);

            string tblname = "T_SystemUser";
            string fieldCollections = " 0 as ISCHECKED,";
            fieldCollections += "T_SystemUser.ID,";
            fieldCollections += "T_SystemUser.USERNAME,";
            fieldCollections += "T_SystemUser.TRUENAME,";
            fieldCollections += "T_SystemUser.SUBCODE,";
            fieldCollections += "T_SystemUser.REMARK,";
            fieldCollections += "T_SystemUser.ADDMAN,";
            fieldCollections += "DateAdd(Hour,-8,T_SystemUser.ADDTIME) as ADDTIME,";
            fieldCollections += "T_SubSection.SUBNAME";

            string orderField = "id";
            int orderType = 1;


            string strWhere = "";
            if (userName != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_SystemUser.userName like '%" + userName + "%'";
            }
            if (trueName != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_SystemUser.trueName like '%" + trueName + "%'";
            }
            if (subCode != "")
            {
                if (strWhere != "") { strWhere += " add "; }
                strWhere += "T_SystemUser.subCode='" + subCode + "'";
            }
            string joinConditions = " left join T_SubSection on T_SubSection.SubCode=T_SystemUser.SubCode ";


            Com.CommonPage common = new Com.CommonPage();
            DataTable dt = common.CommonQueryPage(tblname, fieldCollections, strWhere, joinConditions, orderField, orderType, pageSize, pageIndex);

            return dt;
        }

        [DataTableType("SystemUser.GetUserInfoById")]
        public DataTable GetUserInfoById(string id)
        {
            string strSql = "select ID,USERNAME,TRUENAME,SUBCODE,REMARK from T_SystemUser where id=" + id;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }

        public string AddUser(string userName, string trueName, string password, string subCode, string remark, string addMan, string[] roles, FluorineFx.AMF3.ArrayCollection popedomList)
        {
            string result = "系统错误，保存失败。";
            userName = Com.Com.checkSql(userName);
            trueName = Com.Com.checkSql(trueName);
            remark = Com.Com.checkSql(remark);
            password = Com.Com.checkSql(password);
            subCode = Com.Com.checkSql(subCode);
            addMan = Com.Com.checkSql(addMan);
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select userName from T_SystemUser where userName='" + userName + "'";
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                result = "用户名重复，保存失败。";
            }
            else
            {
                strSql = "insert into T_SystemUser(userName,password,trueName,subCode,remark,addMan) values('" + userName + "','" + new Common.EncrypeMD5().MD5(password) + "','" + trueName + "','" + subCode + "','" + remark + "','" + addMan + "')";
                if (Access.execSqlNoQuery1(strSql))
                {
                    for (int i = 0; i < roles.Length; i++)
                    {
                        Access.execSqlNoQuery1("insert into T_SystemUserRole(userName,roleCode) values('" + userName + "','" + roles[i] + "')");
                    }
                    for (int i = 0; i < popedomList.Count; i++)
                    {
                        System.Collections.Hashtable uPopdeom = (System.Collections.Hashtable)popedomList[i];
                        Access.execSqlNoQuery1("insert into T_SystemUserAndRolePopedom(type,userName,funcCode,subCode) values('USER','" + userName + "','" + uPopdeom["FUNCCODE"].ToString() + "','" + uPopdeom["SUBCODE"].ToString() + "')");
                    }
                    tempDt = Access.execSql("select @@IDENTITY");
                    if (tempDt != null && tempDt.Rows.Count > 0)
                    {
                        result = "OK" + tempDt.Rows[0][0].ToString();
                    }
                }
            }
            Access.Dispose();
            return result;
        }

        public string UpdateUser(string id, string oleUserName, string userName, string password, string trueName, string subCode, string remark, string updateMan, string[] roles, FluorineFx.AMF3.ArrayCollection popedomList)
        {
            string result = "系统错误，保存失败。";
            userName = Com.Com.checkSql(userName);
            trueName = Com.Com.checkSql(trueName);
            password = Com.Com.checkSql(password);
            remark = Com.Com.checkSql(remark);
            subCode = Com.Com.checkSql(subCode);
            updateMan = Com.Com.checkSql(updateMan);
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select userName from T_SystemUser where userName='" + userName + "' and id<>" + id;
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                result = "用户名重复，保存失败。";
            }
            else
            {
                strSql = "update T_SystemUser set ";
                strSql += "userName='" + userName + "',";
                if (password != "")
                {
                    strSql += "password='" + new Common.EncrypeMD5().MD5(password) + "',";
                }
                strSql += "trueName='" + trueName + "',";
                strSql += "subCode='" + subCode + "',";
                strSql += "remark='" + remark + "',";
                strSql += "addMan='" + updateMan + "',";
                strSql += "addTime='" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                strSql += " where id=" + id;
                if (Access.execSqlNoQuery1(strSql))
                {
                    result = "OK" + id;
                    Access.execSqlNoQuery1("delete T_SystemUserRole where userName='" + oleUserName + "'");
                    Access.execSqlNoQuery1("delete T_SystemUserAndRolePopedom where userName='" + oleUserName + "'");
                    for (int i = 0; i < roles.Length; i++)
                    {
                        Access.execSqlNoQuery1("insert into T_SystemUserRole(userName,roleCode) values('" + userName + "','" + roles[i] + "')");
                    }
                    for (int i = 0; i < popedomList.Count; i++)
                    {
                        System.Collections.Hashtable uPopdeom = (System.Collections.Hashtable)popedomList[i];
                        Access.execSqlNoQuery1("insert into T_SystemUserAndRolePopedom(type,userName,funcCode,subCode) values('USER','" + userName + "','" + uPopdeom["FUNCCODE"].ToString() + "','" + uPopdeom["SUBCODE"].ToString() + "')");
                    }
                }
            }
            Access.Dispose();
            return result;
        }

        [DataTableType("SystemUser.DeleteUser")]
        public DataTable DeleteUser(FluorineFx.AMF3.ArrayCollection users)
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("ID");
            DataColumn dc2 = new DataColumn("USERNAME");
            DataColumn dc3 = new DataColumn("TRUENAME");
            DataColumn dc4 = new DataColumn("result");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);
            if (users != null)
            {
                DataAccess.DataAccess Access = new DataAccess.DataAccess();
                for (int i = 0; i < users.Count; i++)
                {
                    System.Collections.Hashtable uInfo = (System.Collections.Hashtable)users[i];
                    if (Access.execSqlNoQuery1("delete T_SystemUser where id=" + uInfo["ID"].ToString() + ""))
                    {
                        Access.execSqlNoQuery1("Delete T_SystemUserAndRolePopedom where userName='" + uInfo["USERNAME"] + "' and type='USER'");
                        Access.execSqlNoQuery1("delete T_SystemUserRole where userName='" + uInfo["USERNAME"] + "'");

                        DataRow dr = dt.NewRow();
                        dr["ID"] = uInfo["ID"].ToString();
                        dr["USERNAME"] = uInfo["USERNAME"].ToString();
                        dr["TRUENAME"] = uInfo["TRUENAME"].ToString();
                        dr["result"] = "0";
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        dr["ID"] = uInfo["ID"].ToString();
                        dr["USERNAME"] = uInfo["USERNAME"].ToString();
                        dr["TRUENAME"] = uInfo["TRUENAME"].ToString();
                        dr["result"] = "失败。";
                        dt.Rows.Add(dr);
                    }
                }
                Access.Dispose();
            }
            return dt;
        }

        [DataTableType("SystemUser.GetAllUserByRoleCode")]
        public DataTable GetAllUserByRoleCode(string roleCode)
        {
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select T_SystemUser.USERNAME,T_SystemUser.TRUENAME,T_SystemUser.REMARK,T_System.SUBSITECODE,T_SubSection.SUBSITENAME,case when T_SystemUserRole is null then '1' else '0' end as ISCHECKED ";
            strSql += " from T_SystemUser left join T_SystemUserRole on T_SystemUserRole.userName=T_SystemUser.userName and T_SystemUserRole.roleCode='" + roleCode + "'";
            strSql += " left join T_SubSection on T_SubSection.subSiteCode=T_SystemUser.subSiteCode";
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }
        
        public string GetUserPopedomXML(string userName)
        {
            string result = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root>";
            DataTable subList = new Sys.SubSection().GetAllSubList();
            DataColumn dc = new DataColumn("checked");
            subList.Columns.Add(dc);
            for (int i = 0; i < subList.Rows.Count; i++)
            {
                subList.Rows[i]["checked"] = "";
            }
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "";
            if (userName != "")
            {
                strSql = "select T_SystemMenu.pptr,T_SystemMenu.code,T_SystemMenu.text,T_SystemMenu.listOrder,T_SystemUserAndRolePopedom.funcCode,T_SystemUserAndRolePopedom.subCode ";
                strSql += ",T_SystemMenu.popedomDifference ";
                strSql += " from T_SystemMenu left join T_SystemUserAndRolePopedom on T_SystemMenu.code=T_SystemUserAndRolePopedom.funcCode and T_SystemUserAndRolePopedom.type='USER'";
                strSql += " and T_SystemUserAndRolePopedom.userName='" + userName + "'";
                strSql += " where T_SystemMenu.isShow=1 and popedomMode='system'";
            }
            else
            {
                strSql = "select T_SystemMenu.pptr,T_SystemMenu.code,T_SystemMenu.text,T_SystemMenu.listOrder,'' as funcCode,'' as subCode ";
                strSql += ",T_SystemMenu.popedomDifference ";
                strSql += " from T_SystemMenu ";
                strSql += " where T_SystemMenu.isShow=1 and popedomMode='system'";
            }
            DataTable dt = Access.execSql(strSql);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] drs = dt.Select("pptr='menu'", "listOrder Asc");
                if (drs != null && drs.Length > 0)
                {
                    for (int i = 0; i < drs.Length; i++)
                    {

                        bool isAllChecked = true;
                        string tempResult = GetUserPopedomXML(dt, drs[i]["code"].ToString(), subList, out isAllChecked);
                        result += "<item type='func' code='" + drs[i]["code"].ToString() + "' text='" + drs[i]["text"].ToString() + "' popedomDifference='" + drs[i]["popedomDifference"].ToString() + "' ";
                        if (drs[i]["funcCode"].ToString() != "")
                        {
                            if (isAllChecked)
                            {
                                result += "state='checked'";
                            }
                            else
                            {
                                result += "state='schrodinger'";
                            }
                        }
                        else
                        {
                            result += "state='unchecked'";
                        }
                        result += " >";
                        result += tempResult;
                        result += "</item>";
                    }
                }
            }
            Access.Dispose();
            result += "</root>";
            return result;
        }
        private string GetUserPopedomXML(DataTable dt, string pptr, DataTable subList, out bool isAllChecked)
        {
            string result = "";
            isAllChecked = true;
            DataRow[] pptrDrs = dt.Select("code='" + pptr + "'");
            DataRow[] drs = dt.Select("pptr='" + pptr + "'", "listOrder Asc");
            if (pptrDrs != null && pptrDrs.Length > 0 && drs != null && drs.Length > 0)
            {
                isAllChecked = true;
                if (pptrDrs[0]["popedomDifference"].ToString().ToUpper() == "GLOBALANDSUBSITE")
                {
                    DataTable tempDt = subList.Copy();
                    DataRow dr = tempDt.NewRow();
                    dr["SUBCODE"] = "global";
                    dr["SUBNAME"] = "全局";
                    tempDt.Rows.InsertAt(dr, 0);
                    /////////////////////////////////////////////////////////////////////////////////////
                    string divisions = "";
                    for (int i = 0; i < drs.Length; i++)
                    {
                        if (drs[i]["subCode"].ToString() != "")
                        {
                            divisions = drs[i]["subCode"].ToString();
                            break;
                        }
                    }
                    if (divisions != "")
                    {
                        string[] divs = divisions.Split('|');
                        for (int i = 0; i < divs.Length; i++)
                        {
                            for (int j = 0; j < tempDt.Rows.Count; j++)
                            {
                                if (divs[i].ToString().ToUpper() == tempDt.Rows[j]["subCode"].ToString().ToUpper())
                                {
                                    tempDt.Rows[j]["checked"] = "checked";
                                    break;
                                }
                            }
                        }
                    }
                    ///////////////////////////////////////////////////////////////////////////////////
                    result += AddDivision(tempDt, drs[0]["pptr"].ToString(), pptrDrs[0]["popedomDifference"].ToString(), out isAllChecked);
                }
                else if (pptrDrs[0]["popedomDifference"].ToString().ToUpper() == "SUBSITE")
                {
                    DataTable tempDt = subList.Copy();
                    /////////////////////////////////////////////////////////////////////////////////////
                    string divisions = "";
                    for (int i = 0; i < drs.Length; i++)
                    {
                        if (drs[i]["subCode"].ToString() != "")
                        {
                            divisions = drs[i]["subCode"].ToString();
                            break;
                        }
                    }
                    if (divisions != "")
                    {
                        string[] divs = divisions.Split('|');
                        for (int i = 0; i < divs.Length; i++)
                        {
                            for (int j = 0; j < tempDt.Rows.Count; j++)
                            {
                                if (divs[i].ToString().ToUpper() == tempDt.Rows[j]["subCode"].ToString().ToUpper())
                                {
                                    tempDt.Rows[j]["checked"] = "checked";
                                    break;
                                }
                            }
                        }
                    }
                    ///////////////////////////////////////////////////////////////////////////////////
                    result += AddDivision(tempDt, drs[0]["pptr"].ToString(), pptrDrs[0]["popedomDifference"].ToString(), out isAllChecked);
                }

                for (int i = 0; i < drs.Length; i++)
                {
                    result += "<item type='func' code='" + drs[i]["code"].ToString() + "' text='" + drs[i]["text"].ToString() + "' popedomDifference='" + drs[i]["popedomDifference"].ToString() + "' ";

                    //isAllChecked = true;
                    string tempResult = GetUserPopedomXML(dt, drs[i]["code"].ToString(), subList, out isAllChecked);
                    if (drs[i]["funcCode"].ToString() != "")
                    {
                        if (isAllChecked)
                        {
                            result += "state='checked'";
                        }
                        else
                        {
                            result += "state='schrodinger'";
                            isAllChecked = false;
                        }
                    }
                    else
                    {
                        result += "state='unchecked'";
                        isAllChecked = false;
                    }

                    result += " >";
                    result += tempResult;
                    result += "</item>";
                }
            }
            return result;
        }

        private string AddDivision(DataTable subList, string code, string popedomDifference, out bool isAllChecked)
        {
            string result = "";
            bool isChecked = false;
            isAllChecked = true;
            for (int i = 0; i < subList.Rows.Count; i++)
            {
                if (subList.Rows[i]["checked"].ToString() != "")
                {
                    if (!isChecked) { isChecked = true; }
                }
                else
                {
                    if (isAllChecked) { isAllChecked = false; }
                }
            }
            if (isChecked)
            {
                if (isAllChecked)
                {
                    result = "<item type='Division' code='" + code + "' text='管理的站点' state='checked' popedomDifference='" + popedomDifference + "'>";
                }
                else
                {
                    result = "<item type='Division' code='" + code + "' text='管理的站点' state='schrodinger' popedomDifference='" + popedomDifference + "'>";
                }
            }
            else
            {
                result = "<item type='Division' code='" + code + "' text='管理的站点' state='unchecked' popedomDifference='" + popedomDifference + "'>";
            }
            for (int i = 0; i < subList.Rows.Count; i++)
            {
                result += "<item type='division' code='" + subList.Rows[i]["SUBCODE"].ToString() + "' text='" + subList.Rows[i]["SUBNAME"].ToString() + "' popedomDifference='" + popedomDifference + "'";
                if (subList.Rows[i]["checked"].ToString() != "")
                {
                    result += "state='checked' ";
                }
                else
                {
                    result += "state='unchecked' ";

                }
                result += "></item>";
            }
            result += "</item>";
            return result;
        }

        [DataTableType("SystemUser.GetAllUserList")]
        public DataTable GetAllUserList()
        {
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql("select TRUENAME,USERNAME from T_SystemUser order by TRUENAME");
            Access.Dispose();
            return dt;
        }
    }
}
