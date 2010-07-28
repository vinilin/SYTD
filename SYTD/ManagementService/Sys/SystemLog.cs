using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FluorineFx;

namespace ManagementService.Sys
{
    [RemotingService]
    public class SystemLog
    {
        [DataTableType("SystemLog.GetLogList")]
        public DataTable GetLogList(string module,string type,string startDate,string endDate,int pageSize,int pageIndex)
        {
            module = Com.Com.checkSql(module);
            type = Com.Com.checkSql(type);
            startDate = Com.Com.checkSql(startDate);
            endDate = Com.Com.checkSql(endDate);

            string tblname = "T_SystemLog";
            string fieldCollections = " 0 as ISCHECKED,";
            fieldCollections += "T_SystemLog.ID,";
            fieldCollections += "T_SystemLog.MODULE,";
            fieldCollections += "T_SystemLog.LOGTYPE,";
            fieldCollections += "DateAdd(Hour,-8,T_SystemLog.LOGTIME) as LOGTIME,";
            fieldCollections += "T_SystemLog.OPUSER,";
            fieldCollections += "T_SystemLog.LOGINFO,";
            fieldCollections += "T_SystemLog.REMOTEIP";

            string orderField = "id";
            int orderType = 1;


            string strWhere = "";
            if (module != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_SystemLog.module = '" + module + "'";
            }
            if (type != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "  T_SystemLog.logType = '" + type + "'";
            }
            if (startDate != "")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "T_SystemLog.logTime>='" + startDate + "'";
            }
            if (endDate!="")
            {
                if (strWhere != "") { strWhere += " and "; }
                strWhere += "T_SystemLog.logTime<='" + endDate + "'";
            }
            string joinConditions = "";


            Com.CommonPage common = new Com.CommonPage();
            DataTable dt = common.CommonQueryPage(tblname, fieldCollections, strWhere, joinConditions, orderField, orderType, pageSize, pageIndex);

            return dt;
        }

        [DataTableType("SystemLog.GetLogModule")]
        public  DataTable GetLogModule()
        {
            string strSql = "select MODULE from T_SystemLog group by module";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }

        [DataTableType("SystemLog.GetLogType")]
        public DataTable GetLogType()
        {
            string strSql = "select LOGTYPE from T_SystemLog group by logType";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }
    }
}
