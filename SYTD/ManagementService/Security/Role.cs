using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FluorineFx;

namespace ManagementService.Security
{
    [RemotingService]
    public class Role
    {
        /// <summary>
        /// subSiteCode如果该角色属于某个分站，则该字段填写分站代码，如果属于整站角色，则该字段为空
        /// </summary>
        /// <param name="roleCode"></param>
        /// <param name="roleName"></param>
        /// <param name="subSiteCode"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [DataTableType("Role.GetRoleList")]
        public DataTable GetRoleList(string roleCode,string roleName,string subSiteCode,int pageSize,int pageIndex)
        {
            roleCode = Com.Com.checkSql(roleCode);
            roleName = Com.Com.checkSql(roleName);
            subSiteCode = Com.Com.checkSql(subSiteCode);
            string tblname = "T_SystemRole";
            string fieldCollections = " 0 as ISCHECKED,";
            fieldCollections += "T_SystemRole.ID,";
            fieldCollections += "T_SystemRole.ROLECODE,";
            fieldCollections += "T_SystemRole.ROLENAME,";
            fieldCollections += "T_SystemRole.SUBSITECODE,";
            fieldCollections += "T_SystemRole.ADDMAN,";
            fieldCollections += "DateAdd(h,-8,T_SubSection.ADDTIME),";
            fieldCollections += "T_SubSection.SUBSITENAME";
            
            string orderField = "id";
            int orderType = 1;


            string strWhere = "";
            if (roleName != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_SystemRole.roleName like '%" + roleName + "%'";
            }
            if (roleCode != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_SystemUser.roleCode like '%" + roleCode + "%'";
            }
            if (subSiteCode!="")
            {
                if (strWhere != "") { strWhere += " add "; }
                strWhere += "T_SystemRole.subSiteCode='" + subSiteCode + "'";
            }
            string joinConditions = " left join T_SubSection on T_SubSection.SubSiteCode=T_SystemRole.SubSiteCode ";


            Com.CommonPage common = new Com.CommonPage();
            DataTable dt = common.CommonQueryPage(tblname, fieldCollections, strWhere, joinConditions, orderField, orderType, pageSize, pageIndex);

            return dt;
        }

        [DataTableType("Role.GetRoleInfoById")]
        public DataTable GetRoleInfoById(string id)
        {
            string strSql = "select ID,ROLECODE,ROLENAME,SUBSITECODE from T_SystemRole where id=" + id;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }

        public string AddRole(string roleCode, string roleName,string subSiteCode,string addMan, string[] users, string[] popedoms)
        {
            roleCode = Com.Com.checkSql(roleCode);
            roleName = Com.Com.checkSql(roleName);
            string result = "系统错误，保存失败。";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select roleCode from T_SystemRole where roleCode='" + roleCode + "'";
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt!=null && tempDt.Rows.Count>0)
            {
                result = "角色代码重复，保存失败";
            }
            else
            {
                strSql = "select roleName from T_SystemRole where roleName='" + roleName + "'";
                tempDt = Access.execSql(strSql);
                if (tempDt!=null && tempDt.Rows.Count>0)
                {
                    result = "角色名称重复，保存失败。";
                }
                else
                {
                    strSql = "insert into T_SystemRole(roleCode,roleName,subSiteCode,addMan) values('" + roleCode + "','" + roleName + "','" + subSiteCode + "','" + addMan + "')";
                    if (Access.execSqlNoQuery1(strSql))
                    {
                        tempDt = Access.execSql("select @@IDENTITY");
                        if (tempDt != null && tempDt.Rows.Count > 0)
                        {
                            result = "OK" + tempDt.Rows[0][0].ToString();
                        }
                        for (int i = 0; i < popedoms.Length; i++)
                        {
                            Access.execSqlNoQuery1("insert into T_SystemUserAndRolePopedom(type,roleCode,funcCode) values('ROLE','" + roleCode + "','" + popedoms[i] + "')");
                        }
                        for (int i = 0; i < users.Length; i++)
                        {
                            Access.execSqlNoQuery1("insert into T_SystemUserRole(userName,roleCode) values('" + users[i] + "','" + roleCode + "')");
                        }
                    }
                }
            }
            Access.Dispose();
            return result;
        }

        public string UpdateRole(string id,string oleRoleCode,string roleCode,string roleName,string subSiteCode,string updateMan,string[] users,string[] popedoms)
        {
            id = Com.Com.checkSql(id);
            roleCode = Com.Com.checkSql(roleCode);
            roleName = Com.Com.checkSql(roleName);
            string result = "系统错误，保存失败。";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select roleCode from T_SystemRole where roleCode='" + roleCode + "' and id<>" + id;
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                result = "角色代码重复，保存失败";
            }
            else
            {
                strSql = "select roleName from T_SystemRole where roleName='" + roleName + "' and id<>" + id;
                tempDt = Access.execSql(strSql);
                if (tempDt != null && tempDt.Rows.Count > 0)
                {
                    result = "角色名称重复，保存失败。";
                }
                else
                {
                    strSql = "update T_SystemRole set ";
                    strSql += "roleCode='" + roleCode + "',";
                    strSql += "roleName='" + roleName + "',";
                    strSql += "subSiteCode='"+subSiteCode+"',";
                    strSql += "addMan='"+updateMan+"',";
                    strSql += "addTime='"+System.DateTime.Now.ToString("yyyy-MM-dd HH:mi:ss")+"' ";
                    strSql += " where id=" + id;
                    if (Access.execSqlNoQuery1(strSql))
                    {
                        Access.execSqlNoQuery1("delete T_SysteUserAndRolePopedom where type='ROLE' and roleCode='" + oleRoleCode + "'");
                        Access.execSqlNoQuery1("delete T_SystemUserRole where roleCode='" + oleRoleCode + "'");
                        for (int i = 0; i < popedoms.Length; i++)
                        {
                            Access.execSqlNoQuery1("insert into T_SystemUserAndRolePopedom(type,roleCode,funcCode) values('ROLE','" + roleCode + "','" + popedoms[i] + "')");
                        }
                        for (int i = 0; i < users.Length; i++)
                        {
                            Access.execSqlNoQuery1("insert into T_SystemUserRole(userName,roleCode) values('" + users[i] + "','" + roleCode + "')");
                        }
                        result = "OK" + id;
                    }
                }
            }
            Access.Dispose();
            return result;
        }

        [DataTableType("Role.DeleteRole")]
        public DataTable DeleteRole(FluorineFx.AMF3.ArrayCollection roles)
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("ID");
            DataColumn dc2 = new DataColumn("ROLECODE");
            DataColumn dc3 = new DataColumn("ROLENAME");
            DataColumn dc4 = new DataColumn("result");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);
            if (roles != null)
            {
                DataAccess.DataAccess Access = new DataAccess.DataAccess();
                for (int i = 0; i < roles.Count; i++)
                {
                    System.Collections.Hashtable uInfo = (System.Collections.Hashtable)roles[i];
                    if (Access.execSqlNoQuery1("delete T_SystemRole where id=" + uInfo["ID"].ToString() + ""))
                    {
                        Access.execSqlNoQuery1("Delete T_SystemUserAndRolePopedom where roleCode='" + uInfo["ROLECODE"] + "' and type='ROLE'");
                        Access.execSqlNoQuery1("delete T_SystemUserRole where roleCode='" + uInfo["ROLECODE"] + "'");

                        DataRow dr = dt.NewRow();
                        dr["ID"] = uInfo["ID"].ToString();
                        dr["ROLECODE"] = uInfo["ROLECODE"].ToString();
                        dr["ROLENAME"] = uInfo["ROLENAME"].ToString();
                        dr["result"] = "0";
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        dr["ID"] = uInfo["ID"].ToString();
                        dr["ROLECODE"] = uInfo["ROLECODE"].ToString();
                        dr["ROLENAME"] = uInfo["ROLENAME"].ToString();
                        dr["result"] = "失败。";
                        dt.Rows.Add(dr);
                    }
                }
                Access.Dispose();
            }
            return dt;
        }

        [DataTableType("Role.GetAllRoleByUserName")]
        public DataTable GetAllRoleByUserName(string userName)
        {
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select T_SystemRole.ROLECODE,T_SystemRole.ROLENAME,T_SystemRole.SUBSITECODE,T_SubSection.SUBSITENAME,case when T_SystemUserRole is null then '1' else '0' end as ISCHECKED ";
            strSql += " from T_SystemUser left join T_SystemUserRole on T_SystemUserRole.roleCode=T_SystemRol.roleCode and T_SystemUserRole.userName='" + userName + "'";
            strSql += " left join T_SubSection on T_SubSection.subSiteCode=T_SystemRole.subSiteCode";
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }
        
        public string GetRolePopedomXML(string roleCode)
        {
            string result = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root>";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "";
            if (roleCode != "")
            {
                strSql = "select T_SystemMenu.pptr,T_SystemMenu.code,T_SystemMenu.text,T_SystemMenu.listOrder,T_SystemUserAndRolePopedom.funcCode ";
                strSql += " from T_SystemMenu left join T_SystemUserAndRolePopedom on T_SystemMenu.code=T_SystemUserAndRolePopedom.funcCode and T_SystemUserAndRolePopedom.type='ROLE'";
                strSql += " and T_SystemUserAndRolePopedom.roleCode='" + roleCode + "'";
                strSql += " where T_SystemMenu.isShow=1";
            }
            else
            {
                strSql = "select T_SystemMenu.pptr,T_SystemMenu.code,T_SystemMenu.description as text,T_SystemMenu.listOrder,'' as funcCode ";
                strSql += " from T_SystemMenu ";
                strSql += " where T_SystemMenu.isShow=1";
            }
            DataTable dt = Access.execSql(strSql);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] drs = dt.Select("pptr='menu'", "listOrder Asc");
                if (drs != null && drs.Length > 0)
                {
                    for (int i = 0; i < drs.Length; i++)
                    {
                        result += "<item code='" + drs[i]["code"].ToString() + "' text='" + drs[i]["text"].ToString() + "' ";
                        if (drs[i]["funcCode"].ToString() != "")
                        {
                            result += "state='checked'";
                        }
                        else
                        {
                            result += "state='unchecked'";
                        }
                        result += " >";
                        result += GetRolePopedomXML(dt, drs[i]["code"].ToString());
                        result += "</item>";
                    }
                }
            }
            Access.Dispose();
            result += "</root>";
            return result;
        }

        private string GetRolePopedomXML(DataTable dt, string pptr)
        {
            string result = "";
            DataRow[] drs = dt.Select("pptr='" + pptr + "'", "listOrder Asc");
            if (drs != null && drs.Length > 0)
            {
                for (int i = 0; i < drs.Length; i++)
                {
                    result += "<item code='" + drs[i]["code"].ToString() + "' text='" + drs[i]["text"].ToString() + "' ";
                    if (drs[i]["funcCode"].ToString() != "")
                    {
                        result += "state='checked'";
                    }
                    else
                    {
                        result += "state='unchecked'";
                    }
                    result += " >";
                    result += GetRolePopedomXML(dt, drs[i]["code"].ToString());
                    result += "</item>";
                }
            }
            return result;
        }
    }
}
