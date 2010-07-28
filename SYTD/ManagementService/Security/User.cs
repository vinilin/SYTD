using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FluorineFx;

namespace ManagementService.Security
{
    [RemotingService]
    public class User
    {
        [DataTableType("User.GetUserList")]
        public DataTable GetUserList(string userName, string trueName, string subCode,string ManagementCode,int pageSize,int pageIndex)
        {
            userName = Com.Com.checkSql(userName);
            trueName = Com.Com.checkSql(trueName);
            subCode = Com.Com.checkSql(subCode);
            ManagementCode = Com.Com.checkSql(ManagementCode);

            string tblname = "T_SystemUser";
            string fieldCollections = " 0 as ISCHECKED,";
            fieldCollections += "T_SystemUser.ID,";
            fieldCollections += "T_SystemUser.USERNAME,";
            fieldCollections += "T_SystemUser.TRUENAME,";
            fieldCollections += "T_SystemUser.SUBCODE,";
            fieldCollections += "T_SystemUser.REMARK,";
            fieldCollections += "T_SystemUser.ADDMAN,";
            fieldCollections += "CASE T_SystemUser.ISBINDIP when 1 then T_SystemUser.BINDIP else '' end AS BINDIP,";
            fieldCollections += "CASE T_SystemUser.ISBINDIP when 1 then '是' else '否' end as ISBINDIP,";
            fieldCollections += "DateAdd(Hour,-8,T_SystemUser.ADDTIME) as ADDTIME,";
            fieldCollections += "T_SubSection.SUBNAME";

            string orderField = "id";
            int orderType = 1;


            string strWhere = "";
            if (ManagementCode != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += " T_SystemUser.ManagementCode='" + ManagementCode + "'";
            }
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
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "T_SystemUser.subCode='" + subCode + "'";
            }
            string joinConditions = " left join T_SubSection on T_SubSection.SubCode=T_SystemUser.SubCode ";
            //joinConditions += " inner join T_SystemUserRole on T_SystemUser.userName = T_SystemUserRole.userName";                 


            Com.CommonPage common = new Com.CommonPage();
            DataTable dt = common.CommonQueryPage(tblname, fieldCollections, strWhere, joinConditions, orderField, orderType, pageSize, pageIndex);

            return dt;
        }

        [DataTableType("User.GetUserInfoById")]
        public DataTable GetUserInfoById(string id)
        {
            string strSql = "select ID,USERNAME,TRUENAME,SUBCODE,REMARK,ISBINDIP,BINDIP from T_SystemUser where id=" + id;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }

        public string AddUser(string userName,string trueName,
                              string password,string subCode,
                              string remark,string addMan,
                              string isBindIp,string bindIp,
                              FluorineFx.AMF3.ArrayCollection roles,
                              string ManagementCode)
        {
            string result = "系统错误，保存失败。";
            userName = Com.Com.checkSql(userName);
            trueName = Com.Com.checkSql(trueName);
            remark = Com.Com.checkSql(remark);
            password = Com.Com.checkSql(password);
            subCode = Com.Com.checkSql(subCode);
            addMan = Com.Com.checkSql(addMan);
            isBindIp = Com.Com.checkSql(isBindIp);
            bindIp = Com.Com.checkSql(bindIp);
            ManagementCode = Com.Com.checkSql(ManagementCode);
            if (isBindIp=="1" && !new Com.IP().IpCheck(bindIp))
            {
                return "绑定IP地址不是一个有效IP";
            }
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select userName from T_SystemUser where userName='" + userName + "'";
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt!=null && tempDt.Rows.Count>0)
            {
                result = "用户名重复，保存失败。";
            }
            else
            {
                strSql = "insert into T_SystemUser(userName,password,trueName,subCode,remark,addMan,isBindIp,BindIp,ManagementCode) ";
                strSql += "values('" + userName + "','" + new Common.EncrypeMD5().MD5(password) + "','" + trueName + "','" + subCode + "','" + remark + "','" + addMan + "'," + isBindIp + ",'" + bindIp + "','" + ManagementCode + "')";
                if (Access.execSqlNoQuery1(strSql))
                {
                    tempDt = Access.execSql("select @@IDENTITY");
                    if (tempDt != null && tempDt.Rows.Count > 0)
                    {
                        result = "OK" + tempDt.Rows[0][0].ToString();
                    }
                    for(int i=0;i<roles.Count;i++)
                    {
                        System.Collections.Hashtable uRoles = (System.Collections.Hashtable)roles[i];
                        Access.execSqlNoQuery1("insert into T_SystemUserRole(userName,roleCode,subCode,kindCode) values('" + userName + "','" + uRoles["roleCode"].ToString() + "','"+uRoles["subCode"].ToString()+"','"+uRoles["kindCode"].ToString()+"')");
                    }
                }
            }
            Access.Dispose();
            return result;
        }

        public string UpdateUser(string id,string oleUserName,
            string userName,string password,
            string trueName,string subCode,
            string remark,string updateMan,
            string isBindIp,string bindIp,
            FluorineFx.AMF3.ArrayCollection roles)
        {
            string result = "系统错误，保存失败。";
            userName = Com.Com.checkSql(userName);
            trueName = Com.Com.checkSql(trueName);
            password = Com.Com.checkSql(password);
            remark = Com.Com.checkSql(remark);
            subCode = Com.Com.checkSql(subCode);
            updateMan = Com.Com.checkSql(updateMan);
            isBindIp = Com.Com.checkSql(isBindIp);
            bindIp = Com.Com.checkSql(bindIp);
            if (isBindIp == "1" && !new Com.IP().IpCheck(bindIp))
            {
                return "绑定IP地址不是一个有效IP";
            }
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
                strSql += "addTime='"+System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',";
                strSql += "isBindIp="+isBindIp+",";
                strSql += "BindIp = '"+bindIp+"' ";
                strSql += " where id=" + id;
                if (Access.execSqlNoQuery1(strSql))
                {
                    result = "OK" + id;
                    Access.execSqlNoQuery1("delete T_SystemUserRole where userName='" + oleUserName + "'");
                    for (int i = 0; i < roles.Count; i++)
                    {
                        System.Collections.Hashtable uRole = (System.Collections.Hashtable)roles[i];
                        Access.execSqlNoQuery1("insert into T_SystemUserRole(userName,roleCode,subCode,kindCode) values('" + userName + "','" + uRole["roleCode"].ToString() + "','" + uRole["subCode"].ToString() + "','" + uRole["kindCode"].ToString() + "')");
                    }
                }
            }
            Access.Dispose();
            return result;
        }

        [DataTableType("User.DeleteUser")]
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
                    if (uInfo["USERNAME"].ToString().ToLower() == "admin")
                    {
                        DataRow dr = dt.NewRow();
                        dr["ID"] = uInfo["ID"].ToString();
                        dr["USERNAME"] = uInfo["USERNAME"].ToString();
                        dr["TRUENAME"] = uInfo["TRUENAME"].ToString();
                        dr["result"] = "ADMIN用户不能删除。";
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        if (Access.execSqlNoQuery1("delete T_SystemUser where id=" + uInfo["ID"].ToString() + ""))
                        {
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
                }
                Access.Dispose();
            }
            return dt;
        }
        

        [DataTableType("User.GetAllUserList")]
        public DataTable GetAllUserList()
        {
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql("select TRUENAME,USERNAME from T_SystemUser order by TRUENAME");
            Access.Dispose();
            return dt;
        }

        [DataTableType("User.GetQJLMManageUser")]
        public DataTable GetQJLMManageRole()
        {
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql("select roleCode,roleName from T_SystemRole order by RoleCode");
            Access.Dispose();
            return dt;
        }

        [DataTableType("User.GetKqfwKindCode")]
        public DataTable GetKqfwKindCode(string userName,string roleCode)
        {
            DataTable resultDt = new DataTable();
            DataColumn dc1 = new DataColumn("CODE");
            DataColumn dc2 = new DataColumn("TEXT");
            resultDt.Columns.Add(dc1);
            resultDt.Columns.Add(dc2);
            string kindCode = "";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql("select kindCode,subCode from T_SystemUserRole where userName='" + userName + "' and roleCode='" + roleCode + "'");
            string subCode = "";
            if (dt!=null && dt.Rows.Count>0)
            {
                kindCode = dt.Rows[0]["kindCode"].ToString();
                subCode = dt.Rows[0]["subCode"].ToString();
            }
            string[] kind = kindCode.Split('|');
            for (int i = 0; i < kind.Length; i++)
            {
                string kindName = "";
                DataTable tempDt = Access.execSql("select text from t_SystemKind where kind='008' and pptr=1 and code='" + kind[i].ToString() + "' and subCode='" + subCode + "'");
                if (tempDt != null && tempDt.Rows.Count > 0)
                {
                    kindName = tempDt.Rows[0][0].ToString();
                }
                DataRow dr = resultDt.NewRow();
                dr["CODE"] = kind[i].ToString();
                dr["TEXT"] = kindName;
                resultDt.Rows.Add(dr);
            } 
            Access.Dispose();
            return resultDt;
        }

        [DataTableType("User.GetAllRoleByUserNameAndPPtrRole")]
        public DataTable GetAllRoleByUserNameAndPPtrRole(string userName,string pptrRoleCode,string[] foreCloseRole)
        {
            userName = Com.Com.checkSql(userName);
            pptrRoleCode = Com.Com.checkSql(pptrRoleCode);

            string strSql = "select case when T_SystemUserRole.userName is null then 0 else 1 end as ISCHECKED,";
            strSql += "T_SystemRole.ROLECODE,T_SystemRole.ROLENAME FROM T_SystemRole ";
            strSql += "left join T_SystemUserRole on T_SystemUserRole.roleCode = T_SystemRole.roleCode ";
            strSql += " and T_SystemUserRole.userName='" + userName + "'";
            strSql += " where T_SystemRole.parentCode='" + pptrRoleCode + "'";
            if (foreCloseRole.Length>0)
            {
                strSql += " and (";
                for (int i = 0; i < foreCloseRole.Length;i++ )
                {
                    if (i != 0)
                    {
                        strSql += " and ";
                    }
                    strSql += " T_SystemRole.roleCode <> '" + foreCloseRole[i] + "' ";
                }
                strSql += ")";
            }
            strSql += " order by roleCode";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }

        [DataTableType("User.GetKQFWAllKindByUserName")]
        public DataTable GetKQFWAllKindByUserName(string userName,string subCode)
        {
            userName = Com.Com.checkSql(userName);
            subCode = Com.Com.checkSql(subCode);
            //string strSql = "select case when T_SystemUserRole.KindCode is null then 0 else 1 end as ISCHECKED,";
            //strSql += "T_SystemKind.code as KINDCODE,T_SystemKind.text as KINDNAME ";
            //strSql += " from T_SystemKind ";
            //strSql += " left join T_SystemUserRole on T_SystemUserRole.kindCode= T_SystemKind.code and T_SystemKind.kind='008' and T_SystemKind.pptr='1' and T_SystemKind.subCode='" + subCode + "' and T_SystemUserRole.userName='" + userName + "' ";
            //strSql += " where T_SystemKind.kind='008' and T_SystemKind.pptr='1' and T_SystemKind.subCode='" + subCode + "' ";
            //DataAccess.DataAccess Access = new DataAccess.DataAccess();
            //DataTable dt = Access.execSql(strSql);
            //Access.Dispose();
            //return dt;

            string strSql = "select 0 as ISCHECKED,T_SystemKind.code as KINDCODE,T_SystemKind.text as KINDNAME ";
            strSql += " from T_SystemKind ";
            strSql += " where T_SystemKind.kind='008' and T_SystemKind.pptr='1' and T_SystemKind.subCode='" + subCode + "' ";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            strSql = "select kindCode from T_SystemUserRole where userName='" + userName + "' and roleCode='021'"; //查找该用户是否是分站管理员，并找出其管理的类别
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt!=null && tempDt.Rows.Count>0)
            {
                string[] kinds=tempDt.Rows[0]["kindCode"].ToString().Split('|');
                if (kinds.Length != 0)
                {
                    for (int i = 0; i < kinds.Length; i++)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (kinds[i].ToString() == dt.Rows[j]["KINDCODE"].ToString())
                            {
                                dt.Rows[j]["ISCHECKED"] = 1;
                            }
                        }
                    }
                }
            }
            Access.Dispose();
            return dt;
        }
    }
}
