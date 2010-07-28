using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FluorineFx;

namespace ManagementService.Com
{
    [RemotingService]
    public class Log
    {
        public bool writeLog(string moduleName,string logType,string logInfo,string opUser)
        {
            bool result = false;
            moduleName = Com.checkSql(moduleName);
            logType = Com.checkSql(logType);
            logInfo = Com.checkSql(logInfo);
            opUser = Com.checkSql(opUser);
            string ip=System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            string strSql = "insert into T_SystemLog(module,logType,logInfo,opUser,remoteIp) ";
            strSql += " values('" + moduleName + "','" + logType + "','" + logInfo + "','" + opUser + "','" + ip + "')";
            if (Access.execSqlNoQuery1(strSql))
            {
                result = true;
            }
            Access.Dispose();
            return result;
        }
    }
}
