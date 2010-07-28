using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FluorineFx;

namespace ManagementService.FileT
{
    [RemotingService]
    public class Kind
    {
        [DataTableType("Kind.GetKind")]
        public DataTable GetKind(int category)
        {
            string strSql = "select PublishType.ID AS CODE,";
            strSql += "PublishType.CATEGORY, ";
            strSql += "PublishType.NAME AS TEXT ";
            strSql += "from PublishType ";
            strSql += "where category = ";
            strSql += category.ToString();

            DataAccess.DataAccess Access = new DataAccess.DataAccess();
            DataTable dt = Access.execSql(strSql);
            Access.Dispose();
            return dt;
        }
    }
}
