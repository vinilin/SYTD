using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FluorineFx;

namespace ManagementService.FileT
{
    [RemotingService]
    class Publish
    {
        [DataTableType("Publish.GetSubSections")]
        public DataTable GetSubSections()
        {
            string strSql = "select ";
            strSql += " id as ID,  ";
            strSql += " subCode as SUBCODE,  ";
            strSql += " subName as SUBNAME,  ";
            strSql += " serverIp as SERVERIP,  ";
            strSql += " isCenter as ISCENTER";
            strSql += " from T_SubSection";
            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }

    }
}
