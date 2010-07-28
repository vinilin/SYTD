using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using FluorineFx;

namespace ManagementService.Security
{
    [RemotingService]
    public class Security
    {
        #region 登陆验证
        [DataTableType("Security.Login")]
        public DataTable Login(string userName,string password)
        {
            string clientIp = GetClientIp();
            string strSql = "select T_SystemUser.USERNAME,T_SystemUser.TRUENAME,T_SystemUser.SUBCODE,T_SubSection.SUBNAME,T_SubSection.SERVERIP,";
            strSql += " T_SystemUser.ISBINDIP,T_SystemUser.BINDIP,'" + clientIp + "' AS CLIENTIP ";
            strSql += " from T_SystemUser ";
            strSql += " left join T_SubSection on T_SubSection.subCode=T_SystemUser.subCode ";
            //strSql += " where userName='" + userName + "' and password='" + new Common.EncrypeMD5().MD5(password) + "'";
            strSql += " where userName='" + userName + "'";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }
        #endregion

        #region 获取用户所属角色
        [DataTableType("Security.GetRoleByUserName")]
        public DataTable GetRoleByUserName(string userName)
        {
            string strSql = "select roleCode from T_SystemUserRole where userName='" + userName + "'";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }
        #endregion

        #region 获取用户角色以及用户关联
        public DataTable GetAllRoleByUserName(string userName,string[] roles)
        {
            string tempStr = "";
            for (int i = 0; i < roles.Length; i++)
            {
                if (i != 0)
                {
                    tempStr += ",";
                }
                tempStr += "'" + roles[i] + "'";
            }
            tempStr += " T_SystemRole.roleCode in (" + tempStr + ") ";
            string strSql = "select T_SystemRole.roleName,T_SystemRole.roleCode,T_SystemUserRole.userName,T_SystemUserRole.subCode ";
            strSql += " from T_SystemRole ";
            strSql += " left join T_SystemUserRole on T_SystemUserRole.roleCode = T_SystemRole.roleCode and T_SystemUserRole.userName='" + userName + "'";
            strSql += " where " + tempStr;
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt=Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }
        #endregion

        #region 根据用户名获取菜单
        public string GetUserMenu(string userName)
        {
            string result = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root>";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();

            string strSql = " select T_SystemMenu.pptr,T_SystemMenu.code,T_SystemMenu.listOrder,T_SystemMenu.text,T_SystemMenu.url,T_SystemMenu.param, ";
            strSql += "TempTbl.subCode ";
            strSql += "from T_SystemMenu ";
            strSql += "inner join  ";
            strSql += "     (select T_SystemMenu.code,T_SystemUserRole.subCode ";
            strSql += "     from T_SystemMenu ";
            strSql += "     inner join T_SystemRoleMenu on T_SystemRoleMenu.funcCode=T_SystemMenu.code and T_SystemRoleMenu.isShow=1 and T_SystemMenu.isShow=1 ";
            strSql += "     inner join T_SystemUserRole on T_SystemUserRole.roleCode=T_SystemRoleMenu.roleCode  ";
            strSql += "     where T_SystemUserRole.userName='" + userName + "' ";
            strSql += "     group by code,subCode) as tempTbl ";
            strSql += "on T_SystemMenu.code=tempTbl.code ";
            strSql += "order by T_SystemMenu.code,T_SystemMenu.listOrder ";
            DataTable dt = Access.execSql(strSql);
            string aaa = dt.Rows.Count.ToString();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] drs = dt.Select("pptr='MENU'", "listOrder Asc");
                if (drs != null && drs.Length > 0)
                {
                    for (int i = 0; i < drs.Length; i++)
                    {
                        result += "<menuItem code='" + drs[i]["code"].ToString() + "' text='" + drs[i]["text"].ToString() + "'";
                        if (drs[i]["url"].ToString() != "")
                        {
                            result += " url='" + drs[i]["url"].ToString().Replace(".", "/") + ".swf" + "' classPath='" + drs[i]["url"].ToString() + "' param='" + drs[i]["param"].ToString() + "'   subCode='" + drs[i]["subCode"] + "' ";
                        }
                        else
                        {
                            result += " url='' classPath='' param='' subCode='' ";
                        }
                        result += ">";
                        result += GetUserMenu(dt, drs[i]["code"].ToString());
                        result += "</menuItem>";
                    }
                }
            }
            Access.Dispose();
            result += "<menuItem code='016' text='密码修改' url='SubModule/System/PasswordSet.swf' classPath='SubModule.System.PasswordSet' param='' ></menuItem>";
            result += "</root>";
            return result;
        }

        private string GetUserMenu(DataTable dt,string pptr)
        {
            string result = "";
            try
            {
                DataRow[] drs = dt.Select("pptr='" + pptr + "'", "listOrder Asc");
                if (drs != null && drs.Length > 0)
                {
                    for (int i = 0; i < drs.Length; i++)
                    {
                        result += "<menuItem code='" + drs[i]["code"].ToString() + "' text='" + drs[i]["text"].ToString() + "'";
                        if (drs[i]["url"].ToString() != "")
                        {
                            result += " url='" + drs[i]["url"].ToString().Replace(".", "/") + ".swf" + "' classPath='" + drs[i]["url"].ToString() + "' param='" + drs[i]["param"].ToString() + "'  subCode='"+drs[i]["subCode"]+"' ";
                        }
                        else
                        {
                            result += " url='' classPath='' param='' subCode='' ";
                        }
                        result += ">";
                        result += GetUserMenu(dt, drs[i]["code"].ToString());
                        result += "</menuItem>";
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log.LogError(ex.ToString(), "");
            }
            return result;
        }
        #endregion

        #region 获取用户IP
        private string GetClientIp()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return result;

        }
        #endregion

        #region 修改密码
        public string UpdatePassword(string userName, string oldPassword, string newPassword)
        {
            userName = Com.Com.checkSql(userName);
            oldPassword = Com.Com.checkSql(oldPassword);
            newPassword = Com.Com.checkSql(newPassword);
            string result = "系统错误，修改失败。";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "select userName from T_SystemUser where userName='" + userName + "' and password= '" + new Common.EncrypeMD5().MD5(oldPassword) + "'";
            DataTable tempDt = Access.execSql(strSql);
            if (tempDt != null && tempDt.Rows.Count > 0)
            {
                strSql = "update T_SystemUser set password='" + new Common.EncrypeMD5().MD5(newPassword) + "' where userName= '" + userName + "'";
                if (Access.execSqlNoQuery1(strSql))
                {
                    result = "OK";
                }
            }
            else
            {
                result = "现密码提供错误，不能修改。";
            }
            Access.Dispose();
            return result;
        }
        #endregion

        
    }
}
