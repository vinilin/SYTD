using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FluorineFx;

namespace ManagementService.FileT
{
    [RemotingService]
    class Progress
    {
        [DataTableType("Progress.GetList")]
        public DataTable GetList( string ip, int pageSize,int pageIndex )
        {

            string tblname = "Progress";
            string fieldCollections = "ID ,";
            fieldCollections += "TITLE , ";
            fieldCollections += "FILESETID , ";
            fieldCollections += "SRCIP , ";
            fieldCollections += "DSTIP, ";
            fieldCollections += "CMMITDATE, ";
            fieldCollections += "FINISHDATE, ";
            fieldCollections += "Owner as ADDMAN, ";
            fieldCollections += "CATEGORY, ";
            fieldCollections += "ext1 as TYPE, ";
            fieldCollections += "STATE , ";
            fieldCollections += "(case total when 0 then '0%' else STR((DOWNLOADED / TOTAL)*100)+'%' end) as [PERCENT],";
            fieldCollections += "TOTAL , ";
            fieldCollections += "DOWNLOADED";

            string joinConditions = " ";
            string orderField = "id";
            string strWhere = " SrcIp = '" + ip+"'";
            int orderType = 1;
            Com.CommonPage common = new Com.CommonPage();
            DataTable dt = common.CommonQueryPage(tblname, fieldCollections, strWhere, joinConditions, orderField, orderType, pageSize, pageIndex);
            return dt;
        }

        [DataTableType("Progress.GetUploadList")]
        public DataTable GetUploadList( int pageSize,int pageIndex )
        {
            string tblname = "Progress";
            string fieldCollections = "ID ,";
            fieldCollections += "TITLE , ";
            fieldCollections += "FILESETID , ";
            fieldCollections += "SRCIP , ";
            fieldCollections += "DSTIP, ";
            fieldCollections += "CMMITDATE, ";
            fieldCollections += "FINISHDATE, ";
            fieldCollections += "Owner as ADDMAN, ";
            fieldCollections += "CATEGORY, ";
            fieldCollections += "ext1 as TYPE, ";
            fieldCollections += "STATE , ";
            //fieldCollections += "((DOWNLOADED / TOTAL)*100) as PERCENT,";
            fieldCollections += "(case total when 0 then '0%' else STR((DOWNLOADED / TOTAL)*100)+'%' end) as [PERCENT],";
            fieldCollections += "TOTAL , ";
            fieldCollections += "DOWNLOADED";
            //fieldCollections += "(DOWNLOADED / TOTAL) as PERCENT";
            string joinConditions = " ";
            string orderField = "id";
            string strWhere = " Ext1 = '发布'";
            int orderType = 1;
            Com.CommonPage common = new Com.CommonPage();
            DataTable dt = common.CommonQueryPage(tblname, fieldCollections, strWhere, joinConditions, orderField, orderType, pageSize, pageIndex);
            return dt;
        }
    }
}
